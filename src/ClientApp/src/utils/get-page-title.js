import defaultSettings from '@/settings'

const title = defaultSettings.title || 'Adnc'

export default function getPageTitle(pageTitle) {
  if (pageTitle) {
    return `${pageTitle} - ${title}`
  }
  return `${title}`
}
