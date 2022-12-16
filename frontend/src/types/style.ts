export function classObjectToClassName(obj: Record<string, boolean>): string {
  return Object.entries(obj)
    .filter(([, active]) => active)
    .map(([name]) => name)
    .join(' ');
}
