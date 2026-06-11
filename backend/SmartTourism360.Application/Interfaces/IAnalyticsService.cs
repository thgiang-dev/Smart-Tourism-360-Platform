using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SmartTourism360.Application.DTOs.Analytics;

namespace SmartTourism360.Application.Interfaces
{
    public interface IAnalyticsService
    {
        Task TrackEventAsync(TrackEventRequest request, Guid? userId = null);
        Task<AnalyticsSummaryDto> GetSummaryAsync(DateTime? fromDate, DateTime? toDate, Guid? regionId = null);
        Task<IEnumerable<TopDestinationAnalyticsDto>> GetTopDestinationsAsync(DateTime? fromDate, DateTime? toDate, int limit = 10);
        Task<IEnumerable<TopTourAnalyticsDto>> GetTopToursAsync(DateTime? fromDate, DateTime? toDate, int limit = 10);
        Task<IEnumerable<TopHotspotAnalyticsDto>> GetTopHotspotsAsync(DateTime? fromDate, DateTime? toDate, int limit = 10);
        Task<IEnumerable<TopRouteAnalyticsDto>> GetTopRoutesAsync(DateTime? fromDate, DateTime? toDate, int limit = 10);
        Task<IEnumerable<EventsByDayDto>> GetEventsByDayAsync(DateTime? fromDate, DateTime? toDate, string? eventName = null);
    }
}
