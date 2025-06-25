import React from "react";
import Card from "../components/Card";
import { CiSearch } from "react-icons/ci";
import { IoCreateOutline } from "react-icons/io5";
import { FaTasks } from "react-icons/fa";


export default function HomePage() {
  return (
    <div className="size-lg bg-gray-800 text-white text-center border-4 rounded-lg p-4">
      <h2 className="text-2xl mb-4"><strong>Home</strong></h2>
      <p>Welcome </p>
      <div className="flex mt-2 gap-4">
        <div className="flex flex-wrap  rounded-lg gap-4 ">
          <Card props={{route: "/search", name: "Search", img: <CiSearch className="h-20 w-20" />}} />
          <Card props={{route: "/add-patient", name: "Create", img: <IoCreateOutline className="h-20 w-20"/> }} />
          <Card props={{route: "/operations", name: "Operations", img: <FaTasks className="h-18 w-18"/> }} />        
        </div>
        
      </div>

    </div>
    
  );
}
