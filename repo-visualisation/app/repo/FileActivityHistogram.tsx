"use client"

import { Age, parseAgeCsv } from "./Ages";

import { useEffect, useRef, useState } from "react"
import * as d3 from "d3"
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from "@/components/ui/card"


interface HistogramData {
  ageMonths: number
  count: number
}



export default function FileActivityHistogram({ages = []}:{ages : Age[]} )  {

  if ( ages.length === 0) {
    return <div>No Historyt</div>
  }

  return (
    <div className="">
      <div className="">
      
        <Graph ages={ages}/>
      </div>
    </div>
  )
}
function Graph({ages = []}:{ages : Age[]}){

  const svgRef = useRef<SVGSVGElement>(null)

  const [data, setData] = useState<HistogramData[]>([])

  const parseData = (ages: Age[]): HistogramData[] => {
    // Group by ageMonths and count
    const grouped = d3.rollup(
      ages,
      (v) => v.length,
      (d) => d.ageMonths,
    )

    // Convert to array and sort by ageMonths
    const histogramData: HistogramData[] = Array.from(grouped, ([ageMonths, count]) => ({
      ageMonths,
      count,
    })).sort((a, b) => a.ageMonths - b.ageMonths)

    return histogramData
  }

  const updateChart = () => {
    try {
      const parsedData = parseData(ages)
      setData(parsedData)
    } catch (error) {
      console.error("Error parsing data:", error)
    }
  }

  useEffect(() => {
    updateChart()
  }, [])

  useEffect(() => {
    if (!data.length || !svgRef.current) return

    const svg = d3.select(svgRef.current)
    svg.selectAll("*").remove()

    const margin = { top: 20, right: 30, bottom: 40, left: 50 }
    const width = 800 - margin.left - margin.right
    const height = 400 - margin.top - margin.bottom

    const g = svg
      .attr("width", width + margin.left + margin.right)
      .attr("height", height + margin.top + margin.bottom)
      .append("g")
      .attr("transform", `translate(${margin.left},${margin.top})`)

    // Scales
    const xScale = d3
      .scaleBand()
      .domain(data.map((d) => d.ageMonths.toString()))
      .range([0, width])
      .padding(0.1)

    const yScale = d3
      .scaleLinear()
      .domain([0, d3.max(data, (d) => d.count) || 0])
      .nice()
      .range([height, 0])

    // Color scale
    const colorScale = d3.scaleSequential(d3.interpolateBlues).domain([0, d3.max(data, (d) => d.count) || 0])

    // Bars
    g.selectAll(".bar")
      .data(data)
      .enter()
      .append("rect")
      .attr("class", "bar")
      .attr("x", (d) => xScale(d.ageMonths.toString()) || 0)
      .attr("width", xScale.bandwidth())
      .attr("y", (d) => yScale(d.count))
      .attr("height", (d) => height - yScale(d.count))
      .attr("fill", (d) => colorScale(d.count))
      .attr("stroke", "#1f2937")
      .attr("stroke-width", 1)
      .style("cursor", "pointer")
      .on("mouseover", function (event, d) {
        d3.select(this).attr("opacity", 0.8)

        // Tooltip
        const tooltip = d3
          .select("body")
          .append("div")
          .attr("class", "tooltip")
          .style("position", "absolute")
          .style("background", "rgba(0, 0, 0, 0.8)")
          .style("color", "white")
          .style("padding", "8px")
          .style("border-radius", "4px")
          .style("font-size", "12px")
          .style("pointer-events", "none")
          .style("z-index", "1000")

        tooltip
          .html(`
          <strong>${d.ageMonths} months ago</strong><br/>
          Files changed: ${d.count}
        `)
          .style("left", event.pageX + 10 + "px")
          .style("top", event.pageY - 10 + "px")
      })
      .on("mouseout", function () {
        d3.select(this).attr("opacity", 1)
        d3.selectAll(".tooltip").remove()
      })

    // Value labels on bars
    g.selectAll(".label")
      .data(data)
      .enter()
      .append("text")
      .attr("class", "label")
      .attr("x", (d) => (xScale(d.ageMonths.toString()) || 0) + xScale.bandwidth() / 2)
      .attr("y", (d) => yScale(d.count) - 5)
      .attr("text-anchor", "middle")
      .style("font-size", "12px")
      .style("font-weight", "bold")
      .style("fill", "#1f2937")
      .text((d) => d.count)

    // X-axis
    g.append("g")
      .attr("transform", `translate(0,${height})`)
      .call(d3.axisBottom(xScale))
      .append("text")
      .attr("x", width / 2)
      .attr("y", 35)
      .attr("fill", "black")
      .style("text-anchor", "middle")
      .style("font-size", "14px")
      .style("font-weight", "bold")
      .text("Months Ago")

    // Y-axis
    g.append("g")
      .call(d3.axisLeft(yScale))
      .append("text")
      .attr("transform", "rotate(-90)")
      .attr("y", -35)
      .attr("x", -height / 2)
      .attr("fill", "black")
      .style("text-anchor", "middle")
      .style("font-size", "14px")
      .style("font-weight", "bold")
      .text("Number of Files Changed")

    // Grid lines
    g.selectAll(".grid-line")
      .data(yScale.ticks())
      .enter()
      .append("line")
      .attr("class", "grid-line")
      .attr("x1", 0)
      .attr("x2", width)
      .attr("y1", (d) => yScale(d))
      .attr("y2", (d) => yScale(d))
      .attr("stroke", "#e5e7eb")
      .attr("stroke-dasharray", "2,2")
      .attr("opacity", 0.7)
  }, [data])

  return (
    <div className="">
      <div className="max-w-6xl mx-auto space-y-6">
        <Card>
          <CardHeader>
            <CardTitle>File Activity Histogram</CardTitle>
            <CardDescription>
              Visualize file changes over time. Shows the number of files changed in each month period.
            </CardDescription>
          </CardHeader>
        </Card>

        <Card>
          <CardContent className="p-6">
            <div className="w-full overflow-x-auto">
              <svg ref={svgRef} className="w-full h-auto"></svg>
            </div>
          </CardContent>
        </Card>

        {data.length > 0 && (
          <Card>
            <CardHeader>
              <CardTitle>Summary Statistics</CardTitle>
            </CardHeader>
            <CardContent>
              <div className="grid grid-cols-2 md:grid-cols-4 gap-4">
                <div className="text-center">
                  <div className="text-2xl font-bold text-blue-600">{data.reduce((sum, d) => sum + d.count, 0)}</div>
                  <div className="text-sm text-gray-600">Total Files</div>
                </div>
                <div className="text-center">
                  <div className="text-2xl font-bold text-green-600">{Math.max(...data.map((d) => d.ageMonths))}</div>
                  <div className="text-sm text-gray-600">Oldest Change (months)</div>
                </div>
                <div className="text-center">
                  <div className="text-2xl font-bold text-purple-600">{Math.max(...data.map((d) => d.count))}</div>
                  <div className="text-sm text-gray-600">Peak Activity</div>
                </div>
                <div className="text-center">
                  <div className="text-2xl font-bold text-orange-600">{data.length}</div>
                  <div className="text-sm text-gray-600">Active Months</div>
                </div>
              </div>
            </CardContent>
          </Card>
        )}
      </div>
    </div>
  )
}
