import { z } from "zod";
import { parseCSVContent } from "zod-csv";

export const ageSchema = z.object({
  entity: z.string(),
  ageMonths: z.coerce.number()
});

export type Age = z.infer<typeof ageSchema>;

export const parseAgeCsv = (content: string) => {
  return parseCSVContent(content, ageSchema);
}

export default function Ages({ages}: {ages: Age[]}) {
  if(ages.length>0){return (
    <div>
      <h1>Ages</h1>
    </div>
  )}
  
  
}