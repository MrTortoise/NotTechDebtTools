import Image from "next/image";
import Link from 'next/link';


export default function Home() {
  return (
    <div className="grid grid-rows-2 items-center justify-items-center">
      <h1>Repo Visualisation</h1>
      <Link href="/repo">Go to Repo</Link>
    
    </div>
  );
}
