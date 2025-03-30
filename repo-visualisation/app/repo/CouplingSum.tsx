import { useEffect } from "react";
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


        const chart = () =>{
            
        }

        return (
            <div>
                <h1>Coupling</h1>
                <svg ref={diagramRef} width={width} height={height}></svg>
                {/* <pre>{JSON.stringify(couplingData, null, 2)}</pre>  */}
            </div>
        );
    }