import React from "react";
import Card from "../components/Card";
import { CiSearch } from "react-icons/ci";
import { IoCreateOutline } from "react-icons/io5";
import { FaTasks } from "react-icons/fa";
import { useAuth } from "../context/AuthContext";

export default function HomePage() {
  const { user } = useAuth();

  return (
    <div className="flex flex-col text-white items-center justify-content p-4">
      <h2 className="text-2xl mb-4">
        <strong>Home</strong>
      </h2>
      <p>Welcome {user?.userName}</p>

      <div className="flex justify-center text-center flex-wrap space-x-4 gap-4 mt-4">
        <Card
          props={{
            route: "/search",
            name: "Search",
            img: <CiSearch className="h-20 w-20" />,
          }}
        />
        <Card
          props={{
            route: "/add-patient",
            name: "Create",
            img: <IoCreateOutline className="h-20 w-20" />,
          }}
        />
        <Card
          props={{
            route: "/operations",
            name: "Operations",
            img: <FaTasks className="h-18 w-18" />,
          }}
        />
      </div>
    </div>
  );
}
