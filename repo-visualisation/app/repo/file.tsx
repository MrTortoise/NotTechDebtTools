export interface PathName {
  path: string[];
  name: string;
}

// builds an array for the path for the depth of nodes in the tree
export function toPathName(absolutePath: string) : PathName {
  const parts = absolutePath.split('/');
  const name = parts.pop() ?? "";
  const path = parts.reverse();
  return { path, name };
}