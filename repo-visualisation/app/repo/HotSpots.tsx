import * as d3 from "d3";
import { z } from "zod";
import { parseCSVContent } from "zod-csv";
import { PathName, toPathName } from "./file";
import { Age } from "./Ages";
import { rootCertificates } from "tls";
import { Children } from "react";

export const hotSpotSchema = z.object({
  entity: z.string(),
  numberOfAuthors: z.coerce.number(),
  numberOfRevisions: z.coerce.number()
});

export type HotSpot = z.infer<typeof hotSpotSchema>;

export const parseHotSpotCsv = (content: string) => {
  return parseCSVContent(content, hotSpotSchema);
}

interface HotSpotHeatItem {
  fullPath: string;
  pathName: PathName;
  heat: number
}

interface ComplexityItem {
  fullPath: string;
  complexity: number;
}
interface Leaf {
  name: string;
  heat: number;
  complexity: number;
}

type Node = Leaf | Tree

interface Tree {
  name: string;
  children: Node[];
}
function addLeafToTree(tree: Tree, path: string[], leaf: { name: string; heat: number; complexity: number; }) {
  if (path.length == 0) {
    tree.children.push(leaf);
  } else {
    const key = path.pop() ?? "This should never happen";
    if (key === "This should never happen") { throw new Error("This should never happen - adding leaf to tree for heatmap failed to pop a path that should always exist") }

    if (!tree.children.find((c) => c.name === key)) {
      const newNode = { name: key, children: [] };
      tree.children.push(newNode);
    }
    const node = tree.children.find((c) => c.name === key) as Tree;
    addLeafToTree(node, path, leaf);
  }
}


const mapHotSpotToTree = (hotSpots: HotSpot[], complexityItems: ComplexityItem[]) => {
  const heatItems: HotSpotHeatItem[] = hotSpots.map((hotSpot) => {
    return {
      fullPath: hotSpot.entity,
      pathName: toPathName(hotSpot.entity),
      heat: hotSpot.numberOfAuthors * hotSpot.numberOfRevisions
    }
  })

  const tree = { name: 'root', children: [] };
  const averageComplexity = complexityItems.reduce((acc, item) => acc + item.complexity, 0) / complexityItems.length

  heatItems.forEach((item) => {
    const complexity = complexityItems.find((complexityItem) => complexityItem.fullPath === item.fullPath) ?? { fullPath: item.fullPath, complexity: averageComplexity }
    const complexityValue = complexity.complexity
    const leaf = { name: item.pathName.name, heat: item.heat, complexity: complexityValue };

    addLeafToTree(tree, item.pathName.path, leaf);
  })

  console.log({ tree });
  return tree;

}

export default function HotSpots({ hotSpots = [], ages = [] }: { hotSpots: HotSpot[], ages: Age[] }) {

  // we are defining a hotspot as number of authors * number of revisions
  // we do this because we have hypothesising that code thats changed a lot will have problems

  // we also think that code changed by many different people is more likley to have bugs

  // so we are multipling as it is a crude way of finding the hottest spots
  // maybe we should normalise number of authors agiinst total authors 
  // Eg 100 revisions by 100 different people is probably different to 100 revisions by 1 person


  if (hotSpots.length === 0 || ages.length === 0) {
    return <div>No HotSpots</div>
  }

  const maxAge = ages.reduce((acc, age) => Math.max(acc, age.ageMonths), 0);
  const treeData = mapHotSpotToTree(
    hotSpots,
    ages.map((age) => ({ fullPath: age.entity, complexity: maxAge - age.ageMonths })
    ))


  return (
    <div>
      <h1>HotSpot</h1>
      <div>Size is the amount and no authors, redness is recency of change</div>
      <HotSpotsDiagram treeData={treeData} />
      {/* <pre>{JSON.stringify(hotSpots, null, 2)}</pre> */}
    </div>
  )

}

function HotSpotsDiagram({ treeData }: { treeData: Tree }) {
  const height = 1000;
  const width = 1000;

  let padding = 3; // seperation between circles
  let margin = 1;
  let marginTop = margin;
  let marginBottom = margin;
  let marginLeft = margin;
  let marginRight = margin;
  let opacityScale;
  let stroke = '#bbb'; 
	let minRadiusText = 5;
  let strokeWidth = 0.5;

  let root = d3.hierarchy<Node>(treeData, undefined)
  console.log("Root:", root);

  root = root.sum((d) => 'heat' in d ? Math.max(0, d.heat) : 0);

  d3
    .pack<Node>()
    .size([width - marginLeft - marginRight, height - marginTop - marginBottom])
    .padding(padding)(root);

  const descendants = root.descendants();
  const leaves: d3.HierarchyNode<Leaf>[] = descendants.filter((d) => !d.children && !isNaN('complexity' in d.data ? d.data.complexity : NaN));
  const maxComplexity = leaves
    .map((d) => 'complexity' in d.data ? d.data.complexity : 0)
    .reduce((p, c) => Math.max(p, c));

  opacityScale = d3.scaleLinear().domain([0, maxComplexity]);

  function CamelToSpaces(name: string): string {
    return name.replace(/([a-z])([A-Z])/g, "$1 $2");
  }

  return (
    <svg
      //viewBox={`${-marginLeft} ${-marginTop} ${width} ${height}`}
      width={width}
      height={height}
      style={{ maxWidth: "100%", height: "auto" }}
      fontFamily="sans-serif"
      fontSize="4"
      textAnchor="middle"
      
    >
      <defs>
        {descendants.map((d, i) =>
          !d.children && d.r > minRadiusText ? (
            <clipPath key={`clip-${i}`} id={`leaf${i}`}>
              <circle cx={d.x} cy={d.y} r={d.r} />
            </clipPath>
          ) : null
        )}
      </defs>

      {descendants.map((d, i) =>
        !d.children ? (
          <g key={`leaf-group-${i}`}>
            <circle
              cx={d.x}
              cy={d.y}
              r={d.r}
              strokeWidth={strokeWidth}
              fill="red"
              fillOpacity={1 - opacityScale(d.data.complexity)}
            >
              <title>{CamelToSpaces(d.data.name)}</title>
            </circle>
            {d.r > minRadiusText && (
              <text
                x={d.x}
                y={d.y}
                clipPath={`url(#leaf${i})`}
              >
                {CamelToSpaces(d.data.name)}
              </text>
            )}
          </g>
        ) : (
          <circle
            key={`circle-${i}`}
            cx={d.x}
            cy={d.y}
            r={d.r}
            fillOpacity="0"
            stroke={stroke}
            strokeWidth={strokeWidth}
          >
            <title>{CamelToSpaces(d.data.name)}</title>
          </circle>
        )
      )}
    </svg>
  );
}

