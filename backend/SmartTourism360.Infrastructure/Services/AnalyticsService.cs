using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartTourism360.Application.DTOs.Analytics;
using SmartTourism360.Application.Interfaces;
using SmartTourism360.Domain.Entities;
using SmartTourism360.Infrastructure.Data;

namespace SmartTourism360.Infrastructure.Services
{
    public class AnalyticsService : IAnalyticsService
    {
        private readonly SmartTourism360DbContext _context;

        // Whitelists for validation
        private static readonly HashSet<string> EventNameWhitelist = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "view_destination",
            "click_map_marker",
            "open_destination_from_map",
            "open_virtual_tour",
            "view_panorama",
            "click_hotspot",
            "navigate_panorama",
            "play_audio",
            "play_video",
            "view_route",
            "click_route_destination",
            "open_destination_from_route",
            "open_virtual_tour_from_route",
            "search_destination",
            "filter_category",
            "search_route",
            "filter_route_theme",
            "view_3d_map",
            "click_3d_marker",
            "fly_to_destination",
            "view_3d_route"
        };

        private static readonly HashSet<string> TargetTypeWhitelist = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "destination",
            "tour",
            "panorama",
            "hotspot",
            "route",
            "category",
            "audio",
            "video",
            "search",
            "map",
            "route_theme"
        };

        public AnalyticsService(SmartTourism360DbContext context)
        {
            _context = context;
        }

        // Helper to get beginning of the day in UTC
        private static DateTime? GetStartOfDayUtc(DateTime? date)
        {
            if (!date.HasValue) return null;
            return date.Value.Date.ToUniversalTime();
        }

        // Helper to get end of the day (23:59:59.999) in UTC
        private static DateTime? GetEndOfDayUtc(DateTime? date)
        {
            if (!date.HasValue) return null;
            return date.Value.Date.AddDays(1).AddTicks(-1).ToUniversalTime();
        }

        public async Task TrackEventAsync(TrackEventRequest request, Guid? userId = null)
        {
            if (string.IsNullOrWhiteSpace(request.EventName) || !EventNameWhitelist.Contains(request.EventName.Trim()))
            {
                throw new ArgumentException($"Event name '{request.EventName}' is invalid or not whitelisted.");
            }

            if (!string.IsNullOrWhiteSpace(request.TargetType) && !TargetTypeWhitelist.Contains(request.TargetType.Trim()))
            {
                throw new ArgumentException($"Target type '{request.TargetType}' is invalid or not whitelisted.");
            }

            if (!string.IsNullOrWhiteSpace(request.Metadata))
            {
                var byteCount = Encoding.UTF8.GetByteCount(request.Metadata);
                if (byteCount > 5120) // 5KB
                {
                    throw new ArgumentException("Metadata payload size exceeds the maximum limit of 5KB.");
                }
            }

            var newEvent = new AnalyticsEvent
            {
                EventName = request.EventName.Trim().ToLowerInvariant(),
                TargetType = request.TargetType?.Trim().ToLowerInvariant(),
                TargetId = request.TargetId,
                SessionId = request.SessionId?.Trim(),
                UserId = userId,
                Metadata = request.Metadata
            };

            await _context.AnalyticsEvents.AddAsync(newEvent);
            await _context.SaveChangesAsync();
        }

        public async Task<AnalyticsSummaryDto> GetSummaryAsync(DateTime? fromDate, DateTime? toDate, Guid? regionId = null)
        {
            var query = _context.AnalyticsEvents.AsQueryable();
            var startUtc = GetStartOfDayUtc(fromDate);
            var endUtc = GetEndOfDayUtc(toDate);

            if (startUtc.HasValue)
            {
                query = query.Where(e => e.CreatedAt >= startUtc.Value);
            }

            if (endUtc.HasValue)
            {
                query = query.Where(e => e.CreatedAt <= endUtc.Value);
            }

            if (regionId.HasValue)
            {
                query = query.Where(e =>
                    (e.TargetType == "destination" && _context.Destinations.Any(d => d.Id == e.TargetId && d.RegionId == regionId.Value)) ||
                    (e.TargetType == "tour" && _context.VirtualTours.Any(vt => vt.Id == e.TargetId && vt.Destination.RegionId == regionId.Value)) ||
                    (e.TargetType == "panorama" && _context.Panoramas.Any(p => p.Id == e.TargetId && p.VirtualTour.Destination.RegionId == regionId.Value)) ||
                    (e.TargetType == "hotspot" && _context.Hotspots.Any(h => h.Id == e.TargetId && h.Panorama.VirtualTour.Destination.RegionId == regionId.Value)) ||
                    (e.TargetType == "route" && _context.Routes.Any(r => r.Id == e.TargetId && r.RegionId == regionId.Value))
                );
            }

            var totalEvents = await query.CountAsync();
            var totalDestinationViews = await query.CountAsync(e => e.EventName == "view_destination");
            var totalVirtualTourOpens = await query.CountAsync(e => e.EventName == "open_virtual_tour");
            var totalHotspotClicks = await query.CountAsync(e => e.EventName == "click_hotspot" || e.EventName == "navigate_panorama");
            var totalPanoramaNavigations = await query.CountAsync(e => e.EventName == "view_panorama");
            var totalRouteViews = await query.CountAsync(e => e.EventName == "view_route");

            return new AnalyticsSummaryDto
            {
                TotalEvents = totalEvents,
                TotalDestinationViews = totalDestinationViews,
                TotalVirtualTourOpens = totalVirtualTourOpens,
                TotalHotspotClicks = totalHotspotClicks,
                TotalPanoramaNavigations = totalPanoramaNavigations,
                TotalRouteViews = totalRouteViews
            };
        }

        public async Task<IEnumerable<TopDestinationAnalyticsDto>> GetTopDestinationsAsync(DateTime? fromDate, DateTime? toDate, int limit = 10)
        {
            var fDateUtc = GetStartOfDayUtc(fromDate);
            var tDateUtc = GetEndOfDayUtc(toDate);

            var query = from d in _context.Destinations
                        select new TopDestinationAnalyticsDto
                        {
                            DestinationId = d.Id,
                            DestinationName = d.Name,
                            ViewCount = _context.AnalyticsEvents.Count(e => e.EventName == "view_destination" && e.TargetType == "destination" && e.TargetId == d.Id && (!fDateUtc.HasValue || e.CreatedAt >= fDateUtc.Value) && (!tDateUtc.HasValue || e.CreatedAt <= tDateUtc.Value)),
                            TourOpenCount = _context.AnalyticsEvents.Count(e => e.EventName == "open_virtual_tour" && (e.TargetType == "tour" && _context.VirtualTours.Any(vt => vt.Id == e.TargetId && vt.DestinationId == d.Id) || e.TargetType == "destination" && e.TargetId == d.Id) && (!fDateUtc.HasValue || e.CreatedAt >= fDateUtc.Value) && (!tDateUtc.HasValue || e.CreatedAt <= tDateUtc.Value))
                        };

            return await query
                .Where(d => d.ViewCount > 0 || d.TourOpenCount > 0)
                .OrderByDescending(d => d.ViewCount)
                .Take(limit)
                .ToListAsync();
        }

        public async Task<IEnumerable<TopTourAnalyticsDto>> GetTopToursAsync(DateTime? fromDate, DateTime? toDate, int limit = 10)
        {
            var fDateUtc = GetStartOfDayUtc(fromDate);
            var tDateUtc = GetEndOfDayUtc(toDate);

            var query = from vt in _context.VirtualTours
                        select new TopTourAnalyticsDto
                        {
                            TourId = vt.Id,
                            TourTitle = vt.Title,
                            OpenCount = _context.AnalyticsEvents.Count(e => e.EventName == "open_virtual_tour" && (e.TargetType == "tour" && e.TargetId == vt.Id || e.TargetType == "destination" && e.TargetId == vt.DestinationId) && (!fDateUtc.HasValue || e.CreatedAt >= fDateUtc.Value) && (!tDateUtc.HasValue || e.CreatedAt <= tDateUtc.Value)),
                            PanoramaNavigationCount = _context.AnalyticsEvents.Count(e => e.EventName == "view_panorama" && e.TargetType == "panorama" && _context.Panoramas.Any(p => p.Id == e.TargetId && p.VirtualTourId == vt.Id) && (!fDateUtc.HasValue || e.CreatedAt >= fDateUtc.Value) && (!tDateUtc.HasValue || e.CreatedAt <= tDateUtc.Value))
                        };

            return await query
                .Where(t => t.OpenCount > 0 || t.PanoramaNavigationCount > 0)
                .OrderByDescending(t => t.OpenCount)
                .Take(limit)
                .ToListAsync();
        }

        public async Task<IEnumerable<TopHotspotAnalyticsDto>> GetTopHotspotsAsync(DateTime? fromDate, DateTime? toDate, int limit = 10)
        {
            var fDateUtc = GetStartOfDayUtc(fromDate);
            var tDateUtc = GetEndOfDayUtc(toDate);

            var query = from h in _context.Hotspots
                        select new TopHotspotAnalyticsDto
                        {
                            HotspotId = h.Id,
                            HotspotTitle = h.Title,
                            HotspotType = h.Type,
                            ClickCount = _context.AnalyticsEvents.Count(e => (e.EventName == "click_hotspot" || e.EventName == "navigate_panorama") && e.TargetType == "hotspot" && e.TargetId == h.Id && (!fDateUtc.HasValue || e.CreatedAt >= fDateUtc.Value) && (!tDateUtc.HasValue || e.CreatedAt <= tDateUtc.Value))
                        };

            return await query
                .Where(h => h.ClickCount > 0)
                .OrderByDescending(h => h.ClickCount)
                .Take(limit)
                .ToListAsync();
        }

        public async Task<IEnumerable<TopRouteAnalyticsDto>> GetTopRoutesAsync(DateTime? fromDate, DateTime? toDate, int limit = 10)
        {
            var fDateUtc = GetStartOfDayUtc(fromDate);
            var tDateUtc = GetEndOfDayUtc(toDate);

            // 1. Fetch routes and view counts from database
            var routesList = await (from r in _context.Routes
                                    select new
                                    {
                                        RouteId = r.Id,
                                        RouteTitle = r.Title,
                                        ViewCount = _context.AnalyticsEvents.Count(e => e.EventName == "view_route" && e.TargetType == "route" && e.TargetId == r.Id && (!fDateUtc.HasValue || e.CreatedAt >= fDateUtc.Value) && (!tDateUtc.HasValue || e.CreatedAt <= tDateUtc.Value))
                                    }).ToListAsync();

            // 2. Fetch destination click events in memory
            var clickEvents = await _context.AnalyticsEvents
                .Where(e => e.EventName == "click_route_destination" && e.Metadata != null && (!fDateUtc.HasValue || e.CreatedAt >= fDateUtc.Value) && (!tDateUtc.HasValue || e.CreatedAt <= tDateUtc.Value))
                .Select(e => new { e.Metadata })
                .ToListAsync();

            // 3. Perform JSON string check in-memory to prevent database type casting errors
            var result = routesList.Select(r => new TopRouteAnalyticsDto
            {
                RouteId = r.RouteId,
                RouteTitle = r.RouteTitle,
                ViewCount = r.ViewCount,
                DestinationClickCount = clickEvents.Count(e => e.Metadata != null && e.Metadata.Contains(r.RouteId.ToString()))
            })
            .Where(r => r.ViewCount > 0 || r.DestinationClickCount > 0)
            .OrderByDescending(r => r.ViewCount)
            .Take(limit)
            .ToList();

            return result;
        }

        public async Task<IEnumerable<EventsByDayDto>> GetEventsByDayAsync(DateTime? fromDate, DateTime? toDate, string? eventName = null)
        {
            var query = _context.AnalyticsEvents.AsQueryable();
            var startUtc = GetStartOfDayUtc(fromDate);
            var endUtc = GetEndOfDayUtc(toDate);

            if (startUtc.HasValue)
            {
                query = query.Where(e => e.CreatedAt >= startUtc.Value);
            }

            if (endUtc.HasValue)
            {
                query = query.Where(e => e.CreatedAt <= endUtc.Value);
            }

            if (!string.IsNullOrEmpty(eventName))
            {
                query = query.Where(e => e.EventName == eventName.Trim().ToLowerInvariant());
            }

            var data = await query
                .GroupBy(e => e.CreatedAt.Date)
                .Select(g => new { Date = g.Key, Count = g.Count() })
                .OrderBy(x => x.Date)
                .ToListAsync();

            return data.Select(x => new EventsByDayDto
            {
                Date = x.Date.ToString("yyyy-MM-dd"),
                Count = x.Count
            }).ToList();
        }
    }
}
