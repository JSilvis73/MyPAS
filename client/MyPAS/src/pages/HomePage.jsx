import React from "react";
import Card from "../components/Card";
import { CiSearch } from "react-icons/ci";
import { IoCreateOutline } from "react-icons/io5";
import { FaTasks } from "react-icons/fa";
import { useAuth } from "../context/AuthContext";


export default function HomePage() {
  const auth = useAuth();
  console.log("HomePage user:", auth.user);
  return (
    <div className="m-4 w-3xl max-w-6xl mx-auto bg-gray-800 text-white text-center border border-gray-600 rounded-lg p-4 shadow-lg">
      <h2 className="text-2xl mb-4"><strong>Home</strong></h2>
      <p>Welcome {auth.user?.userName}</p>
     
        <div className="flex justify-center flex-wrap space-x-4 gap-4 mt-4">
          <Card props={{route: "/search", name: "Search", img: <CiSearch className="h-20 w-20" />}} />
          <Card props={{route: "/add-patient", name: "Create", img: <IoCreateOutline className="h-20 w-20"/> }} />
          <Card props={{route: "/operations", name: "Operations", img: <FaTasks className="h-18 w-18"/> }} />        
        </div>
        
      

    </div>
    
  );
}
