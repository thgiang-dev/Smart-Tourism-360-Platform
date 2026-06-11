import api from '../utils/api'
import { getOrCreateSessionId, getDeviceType } from '../utils/session'

/**
 * Tracks an anonymous user interaction event.
 * Runs fire-and-forget and absorbs errors silently.
 * 
 * @param {Object} param0 
 * @param {string} param0.eventName - The whitelisted name of the event
 * @param {string|null} param0.targetType - The type of target entity (e.g. 'destination', 'tour')
 * @param {string|null} param0.targetId - The Guid of the target entity
 * @param {Object} param0.metadata - Additional properties to attach to the event
 */
export async function trackEvent({ eventName, targetType = null, targetId = null, metadata = {} }) {
  try {
    const sessionId = getOrCreateSessionId()
    const finalMetadata = {
      ...metadata,
      deviceType: getDeviceType(),
      userAgent: navigator.userAgent,
      path: window.location.pathname
    }

    // Convert metadata object to JSON string
    const metadataString = JSON.stringify(finalMetadata)

    // Enforce 5KB safety limit
    if (metadataString.length > 5000) {
      if (import.meta.env.DEV) {
        console.warn(`[Analytics Warning] Metadata size for event ${eventName} exceeds limit. Skipping metadata.`)
      }
      // Log without metadata or truncate
      return
    }

    const payload = {
      eventName,
      targetType,
      targetId,
      sessionId,
      metadata: metadataString
    }

    await api.post('/api/analytics/events', payload)
  } catch (error) {
    // Fail silently so UX is never disrupted
    if (import.meta.env.DEV) {
      console.warn(`[Analytics Service Error] Failed to track event ${eventName}:`, error)
    }
  }
}
