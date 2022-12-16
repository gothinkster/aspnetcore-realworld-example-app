export function redirect(path: string) {
  location.hash = `#/${path}`;
}
