"use client";

import { useState } from "react";

export interface CouplingData {
  [filename: string]: string
}

function DataProvider({ onUpdate }: { onUpdate: (data: Record<string, string>) => void }) {
  const [directoryData, setDirectoryData] = useState<Record<string, string>>({});

  const handleDirectorySelection = async () => {
    try {
      // Simulate directory selection by asking the user to upload multiple files
      const input = document.createElement("input");
      input.type = "file";
      input.multiple = true;
      input.accept = ".csv"; // Restrict to CSV files
      input.click();

      input.onchange = async (event) => {
      const files = (event.target as HTMLInputElement).files;
      if (!files) return;

      const dataMap: Record<string, string> = {};

      for (const file of Array.from(files)) {
        const content = await file.text();

        // // Parse the file content (assuming CSV format)
        // const rows = content.split("\n").filter((row) => row.trim() !== "");
        // const headers = rows[0].split(","); // Assuming the first row contains column headers
        // const data = rows.slice(1).map((row) => {
        // const values = row.split(",");
        // return headers.reduce((acc, header, index) => {
        //   acc[header.trim()] = values[index]?.trim() || "";
        //   return acc;
        // }, {} as Record<string, string>);
        // });

        dataMap[file.name] = content;
      }

      setDirectoryData(dataMap);
      onUpdate(dataMap); // Pass the data back to the parent component
      };
    } catch (error) {
      console.error("Error reading files:", error);
    }
  };

  return (
    <div>
      <button className="btn btn-blue" onClick={handleDirectorySelection}>Select Directory</button>
      {/* <pre>{JSON.stringify(directoryData, null, 2)}</pre> */}
    </div>
  );
}

export default DataProvider;