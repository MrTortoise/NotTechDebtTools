import { useEffect, useRef } from "react";
import { z } from "zod";
import { parseCSVContent } from "zod-csv";

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
    {
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
        const width = 1000;
        const height = 1000;
        const radius = width / 2;
        const fontSize = 2;

        useEffect(() => {
            if (diagramRef.current) {
                chart();
            }
        }, [couplingData]);

        interface CoupledFile {
            fullPath: string;
            coupledFiles: {
                fullPath: string,
                probability: number,
                frequency: number
            }[];
        }

        function createCoupledFileDictionary(couplingData: CouplingData[]) {
            const coupledFileDictionary: Record<string, CoupledFile[]> = {};
            couplingData.forEach((item) => {
                const source = item.source;
                const target = item.target;
                if (!coupledFileDictionary[source]) {
                    coupledFileDictionary[source] = { fullPath: source, coupledFiles: [] };
                }
                if (!coupledFileDictionary[target]) {
                    coupledFileDictionary[target] = [];
                }
                coupledFileDictionary[source].push({ fulltarget);
                coupledFileDictionary[target].push(source);
            });
            return coupledFileDictionary;
        }

        const chart = () => {
            console.log("coupling data set in use effect:", couplingData);


        }

        return (
            <div>
                <h1>Coupling</h1>
                <svg ref={diagramRef} width={width} height={height}></svg>
                {/* <pre>{JSON.stringify(couplingData, null, 2)}</pre>  */}
            </div>
        );
    }