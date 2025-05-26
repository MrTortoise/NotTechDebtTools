"use client";

import { useState } from "react";
import DataProvider, { CouplingData } from "./DataProvider";
import HotSpots, { HotSpot, parseHotSpotCsv } from "./HotSpots";
import Ages, { Age, parseAgeCsv } from "./Ages";
import Coupling, { parseCouplingCsv } from "./Coupling";
import FileActivityHistogram from "./FileActivityHistogram";



export default function Home() {
  const [data, setData] = useState<CouplingData>({});
  const [hotSpots, setHotSpots] = useState<HotSpot[]>([]);
  const [hotSpotErrors, setHotSpotErrors] = useState<string>("");
  const [ages, setAges] = useState<Age[]>([]);
  const [ageErrors, setAgeErrors] = useState<string>("");
  const [couplingData, setCouplingData] = useState<CouplingItem[]>([]);
  const [couplingErrors, setCouplingErrors] = useState<string>("");

  function parseHotSpotData(data: CouplingData) {
    const hotspotData = data["hotspot.csv"];
    if (!hotspotData) {
      console.warn("No hotspot data found");
      return;
    }
    const parsedHotSpotData = parseHotSpotCsv(hotspotData);

    if (!parsedHotSpotData.success) {
      setHotSpotErrors(JSON.stringify(parsedHotSpotData.errors, null, 2));
    } else {
      setHotSpots(parsedHotSpotData.validRows);
    }

    // setHotSpotData(parsedHotSpotData);
    console.log("Parsed HotSpot Data:", parsedHotSpotData);
  }

  function parseAgeData(data: CouplingData){
    const ageData = data["age.csv"];
    if (!ageData) {
      console.warn("No age data found");
      return;
    }
    const parsedAgeData = parseAgeCsv(ageData);

    if (!parsedAgeData.success) {
      setAgeErrors(JSON.stringify(parsedAgeData.errors, null, 2));
    } else {
      setAges(parsedAgeData.validRows);
    }

    // setHotSpotData(parsedHotSpotData);
    console.log("Parsed Age Data:", parsedAgeData);
  }

  
function parseCouplingData(data: CouplingData) {
  const couplingData = data["coupling.csv"];
  if (!couplingData) {
    console.warn("No coupling data found");
    return;
  }
  const parsedCouplingData = parseCouplingCsv(couplingData);

  if (!parsedCouplingData.success) {
    setCouplingErrors(JSON.stringify(parsedCouplingData.errors, null, 2));
  } else {
    setCouplingData(parsedCouplingData.validRows);
  }

  // setHotSpotData(parsedHotSpotData);
  console.log("Parsed Coupling Data:", parsedCouplingData);
}

  const parseData = (data: CouplingData) => {
    parseHotSpotData(data);
    parseAgeData(data);
    parseCouplingData(data)
  }
  const Errors = ({errorType, errors }: {errorType:string, errors: string }) => {
    if (errors == "" || errors == null) {
      return;
    }

    return (<><h1>{errorType} Errors</h1><pre>{errors}</pre></>)
  }

  return (
    <div className="grid grid-rows-2 items-center justify-items-center">
      <h1 className="headi">Data Visualisation</h1>
      {/* <Barchart /> */}
      <DataProvider onUpdate={(incomingData) => {
        console.log("Data received:", incomingData);
        parseData(incomingData);
      }} />
      <Errors errorType="Age"  errors={ageErrors} />
      <Errors errorType="Hotspot"  errors={hotSpotErrors} />
      <Errors errorType="Coupling"  errors={couplingErrors} />
      
      <FileActivityHistogram ages={ages}/>

      
      <HotSpots hotSpots={hotSpots} ages={ages}/>
      {/* <pre>{JSON.stringify(data, null, 2).slice(0, 1000)}</pre>  */}

      
      <Coupling couplingData={couplingData}/>

    </div>
  );


}

