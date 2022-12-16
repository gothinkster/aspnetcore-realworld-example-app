export function objectToQueryString<T, V>(object: Partial<Record<keyof T, V>>): string {
  return Object.entries(object)
    .map(([key, value]) => `${key}=${value}`)
    .join('&');
}
