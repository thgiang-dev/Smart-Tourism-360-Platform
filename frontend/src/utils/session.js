/**
 * Generates or retrieves an anonymous sessionId stored in localStorage
 * to track user session activities anonymously.
 */
export function getOrCreateSessionId() {
  let sessionId = localStorage.getItem('st360_session_id')
  if (!sessionId) {
    const timestamp = Date.now()
    const random = Math.random().toString(36).substring(2, 11)
    sessionId = `st360-session-${timestamp}-${random}`
    localStorage.setItem('st360_session_id', sessionId)
  }
  return sessionId
}

/**
 * Returns the client device type based on width
 */
export function getDeviceType() {
  const width = window.innerWidth
  if (width < 768) {
    return 'mobile'
  } else if (width < 1024) {
    return 'tablet'
  } else {
    return 'desktop'
  }
}
