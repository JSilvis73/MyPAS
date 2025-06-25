import React from "react";
import { Link } from "react-router";
import { CiSearch } from "react-icons/ci";


export default function Card({ props }) {
  return (
    <Link
      to={props.route}
      className="px-6 py-3 rounded-xl 
           bg-white/10 border border-white/30 
           backdrop-blur-lg text-white font-semibold 
           shadow-[0_8px_32px_0_rgba(31,38,135,0.37)] 
           hover:bg-white/20 hover:scale-105 
           transition-all duration-300 ease-in-out"
    >
      <div className="h-20 w-20 border rounded-xl">{props.img}</div>
      <h3 className="hover:underline">{props.name}</h3>
    </Link>
  );
}
