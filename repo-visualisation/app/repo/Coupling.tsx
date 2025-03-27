"use client";

import { z } from "zod";
import { parseCSVContent } from "zod-csv";
import { toPathName } from "./file";
import { v4 as uuidv4 } from 'uuid';
import * as d3 from "d3";
import { useEffect, useRef } from "react";

export const couplingSchema = z.object({
  source: z.string(),
  target: z.string(),
  frequency: z.coerce.number(),
  probability: z.coerce.number()
});

export type CouplingData = z.infer<typeof couplingSchema>;

export const parseCouplingCsv = (content: string) => {
  return parseCSVContent(content, couplingSchema);
}

export default function Coupling({ couplingData }: { couplingData: CouplingData[] }) {
  if (couplingData.length == 0) {
    return (
      <div>
        No Coupling Data
      </div>)
  }

  const diagramRef = useRef<SVGSVGElement | null>(null);

  const colorin = '#00f';
	const colorout = '#f00';
	const colornone = '#ccc';
	const width = 2000;
  const height = 2000;
	const radius = width / 2;
	const fontSize = 2;

  let node;
	let leaves;
	let over;

	let link;

	let el;
	let thing;
  

  useEffect(() => {
    if (diagramRef.current) {
      chart();
    }
  }, [couplingData]);

function createPath(node: { children?: any[]; }, path: any[]) {
		if (path.length == 0) return node;

		if (!node.children) {
			node.children = [];
		}

		const p = path.pop();
		let nextNode = node.children.find((c) => c.name === p);
		if (!nextNode) {
			nextNode = { name: p, id: uuidv4() };
			node.children.push(nextNode);
		}

		return createPath(nextNode, path);
	}

  function ToTree(node: { children?: any[]; }, toAddList: any[]) {
		toAddList.forEach((current) => {
			const pathName = toPathName(current.fullPath);
			const file = pathName.name;
			const path = pathName.path;
			const nodeToAddLeaf = createPath(node, path);

			if (!nodeToAddLeaf.children) nodeToAddLeaf.children = [];
			nodeToAddLeaf.children.push({ ...current, name: file, id: current.fullPath });
		});

		return node;
	}

  function linkLeaves(leaves) {
		const lookup = new Map(leaves.map((l) => [l.data.fullPath, l]));
		console.log({ lookup, leaves });
		leaves.forEach((leaf) => {
			leaf.coupledFrom = [];
			leaf.coupledTo = [];
			if (leaf.data.coupledTo) {
				leaf.coupledTo = leaf.data.coupledTo.map((ct) => [leaf, lookup.get(ct.fullPath)]);
			}
			if (leaf.data.coupledFrom) {
				leaf.coupledFrom = leaf.data.coupledFrom.map((cf) => [leaf, lookup.get(cf.fullPath)]);
			}
		});

		console.log({ leavesLinked: leaves });
	}

  const chart = () => {
		console.log("data set in use effect:", couplingData);

    const rows = couplingData.map((d) => {return{
      fullPath: d.source,
      coupledToFullPath: d.target,
      degree: d.probability,
      revisions: d.frequency
    }});
    console.log({ rows });



		//rows.sort((a, b) => d3.ascending(a.fullPath, b.fullPath));

		// take items that are coupled to many things and reduce
		// to one thing with a collection
		// this will show the risk of breaking something else
		const coupledToCollatedMap = {};
		rows.forEach((r) => {
			const leaf = { fullPath: r.coupledToFullPath, degree: r.degree, revisions: r.revisions };
			if (!coupledToCollatedMap[r.fullPath]) {
				coupledToCollatedMap[r.fullPath] = {
					fullPath: r.fullPath,
					coupledTo: [leaf]
				};
			} else {
				coupledToCollatedMap[r.fullPath].coupledTo.push(leaf);
			}
		});
		console.log({ coupledToCollatedMap });

		// // go through and find the opposite direction of coupling and populate that
		// // this will be used to show reverse coupling (ie the risk of a suprise - ie being broken by something else)
		const keys = Object.keys(coupledToCollatedMap);
		const coupledFromCollated = { ...coupledToCollatedMap };
		keys.forEach((k) => {
			const current = coupledToCollatedMap[k];
			// take each thing we are coupled to
			current.coupledTo.forEach((coupledTo) => {
				// if that item exists already (then it has both from and to coupling)
				const coupledFrom = coupledFromCollated[coupledTo];
				if (!coupledFrom) {
					coupledFromCollated[coupledTo.fullPath] = {
						fullPath: coupledTo.fullPath,
						coupledFrom: [
							{
								fullPath: current.fullPath,
								degree: coupledTo.degree,
								revisions: coupledTo.revisions
							}
						]
					};
					if (coupledTo.coupledTo)
						coupledFromCollated[coupledTo.fullPath].coupledTo = coupledTo.coupledTo;
				} else {
					coupledFromCollated[coupledTo.fullPath].coupledFrom.push({
						fullPath: current.fullPath,
						degree: coupledTo.degree,
						revisions: coupledTo.revisions
					});
				}
			});
		});

		console.log({ coupledFromCollated });

		// const collatedValues = Object.values(coupledFromCollated);
		// const couplingTree = ToTree({}, collatedValues);
		// console.log({ couplingTree });

		const coupledToValues = Object.values(coupledFromCollated);
		const collateToTree = ToTree({}, coupledToValues);
		console.log({ collateToTree });
		const h = d3
			.hierarchy(collateToTree)
			.sort((a, b) => d3.ascending(a.height, b.height) || d3.ascending(a.data.name, b.data.name));
		console.log({ h });
		const tree = d3.cluster().size([2 * Math.PI, radius - 125]);
		leaves = tree(h).leaves();
		console.log({ leaves });

		const linkedTree = linkLeaves(leaves);
		const svg = d3.select(diagramRef.current).attr('viewBox', [-width / 2, -width / 2, width, width]);

		node = svg
			.append('g')
			.attr('font-family', 'sans-serif')
			.attr('font-size', fontSize)
			.selectAll('g')
			.data(leaves)
			.join('g')
			.attr('transform', (d) => `rotate(${(d.x * 180) / Math.PI - 90}) translate(${d.y},0)`)
			.append('text')
			.attr('dy', '0.1em')
			.attr('x', (d) => (d.x < Math.PI ? 6 : -6))
			.attr('text-anchor', (d) => (d.x < Math.PI ? 'start' : 'end'))
			.attr('transform', (d) => (d.x >= Math.PI ? 'rotate(180)' : null))
			.text((d) => d.data.name)
			.each(function (d) {
				d.text = this;
			})
			.on('mouseover', overed)
			.on('mouseout', outed)
			.call((text) =>
				text.append('title').text(
					(d) => `${d.data.name}
		${d.coupledTo.length} coupledTo
		${d.coupledFrom.length} coupledFrom`
				)
			);

		const coupledToLeaves = leaves.flatMap((leaf) => leaf.coupledTo);
		const coupledFromLeaves = leaves.flatMap((leaf) => leaf.coupledFrom);
		const coupledLEaves = [...coupledFromLeaves, ...coupledToLeaves];

		function overed(event, d) {
			link.style('mix-blend-mode', null);
			d3.select(this).attr('font-weight', 'bold');
			d3.selectAll(d.coupledFrom.map((d) => d.path))
				.attr('stroke', colorin)
				.raise();
			d3.selectAll(d.coupledFrom.map(([d]) => d.text))
				.attr('fill', colorin)
				.attr('font-weight', 'bold');
			d3.selectAll(d.coupledTo.map((d) => d.path))
				.attr('stroke', colorout)
				.raise();
			d3.selectAll(d.coupledTo.map(([, d]) => d.text))
				.attr('fill', colorout)
				.attr('font-weight', 'bold');
		}

		function outed(_event, d) {
			link.style('mix-blend-mode', 'multiply');
			d3.select(this).attr('font-weight', null);
			d3.selectAll(d.coupledFrom.map((d) => d.path)).attr('stroke', null);
			d3.selectAll(d.coupledFrom.map(([d]) => d.text))
				.attr('fill', null)
				.attr('font-weight', null);
			d3.selectAll(d.coupledTo.map((d) => d.path)).attr('stroke', null);
			d3.selectAll(d.coupledTo.map(([, d]) => d.text))
				.attr('fill', null)
				.attr('font-weight', null);
		}

		const dave = d3
			.lineRadial()
			.curve(d3.curveBundle.beta(0.85))
			.radius((d) => d.y)
			.angle((d) => d.x);

		let link = svg
			.append('g')
			.attr('stroke', colornone)
			.attr('fill', 'none')
			.selectAll('path')
			.data(coupledLEaves)
			.join('path')
			.style('mix-blend-mode', 'multiply')
			.attr('d', ([i, o]) => dave(i.path(o)))
			.each(function (d) {
				d.path = this;
			});

		console.log({ link });
		return svg.node();
	};

  return (
    <div>
      <h1>Coupling</h1>
      <svg ref={diagramRef} width={width} height={height}></svg>
      {/* <pre>{JSON.stringify(couplingData, null, 2)}</pre>  */}
    </div>
  );
}