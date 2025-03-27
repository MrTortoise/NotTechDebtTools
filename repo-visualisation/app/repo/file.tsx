export interface PathName {
  path: string[];
  name: string;
}

export function toPathName(absolutePath: string) : PathName {
  const parts = absolutePath.split('/');
  const name = parts.pop() ?? "";
  const path = parts.reverse();
  return { path, name };
}