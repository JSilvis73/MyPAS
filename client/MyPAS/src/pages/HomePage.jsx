import React from "react";
import Card from "../components/Card";
import { CiSearch } from "react-icons/ci";
import { IoCreateOutline } from "react-icons/io5";
import { FaTasks } from "react-icons/fa";
import { useAuth } from "../context/AuthContext";

export default function HomePage() {
  const { user } = useAuth();

  return (
    <div className="flex flex-col items-center gap-2">
      <h1 className="text-2xl mb-4 text-blue-500">
        <strong>Home</strong>
      </h1>
      <p>Welcome, {user?.userName}!</p>

      <div className="text-center m-4 bg-gray-800 p-4 rounded-lg shadow-lg border border-gray-600">
        <h2 className="text-blue-500">
          <strong>News:</strong>
        </h2>
        <p>
          MyPAS is a new patient management system designed to streamline
          healthcare operations.
        </p>
      </div>

      <div className="text-center m-4 bg-gray-800 p-4 rounded-lg shadow-lg border border-gray-600">
        <h2 className="text-blue-500">
          <strong>Quick Actions:</strong>
        </h2>
        <p>Select an action below to get started.</p>

        <div className="flex justify-center text-center flex-wrap space-x-4 gap-4 m-4">
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
    </div>
  );
}
