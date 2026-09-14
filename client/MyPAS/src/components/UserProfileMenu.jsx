import React from "react";
import { Link } from "react-router-dom";
import { CiSettings } from "react-icons/ci";
import { useAuth } from "../context/AuthContext";
import { TbLogout } from "react-icons/tb";
import { RiAdminFill } from "react-icons/ri";

export default function UserProfileMenu() {
  const { user, signOut } = useAuth();

  const handleLogOut = () => {
    try {
      signOut(); // Clear auth state and local storage
      alert("You have been logged out.");
    } catch (error) {
      console.error("Error during logout:", error);
    }
  };

  return (
    <div className="flex flex-col gap-2">
      <Link
        to="/user-details"
        className="flex items-center gap-2 hover:underline"
      >
        <CiSettings className="text-xl" />
        Settings
      </Link>

      <Link
        to="/admin-dashboard"
        className="flex items-center gap-2 hover:underline"
      >
        <RiAdminFill  className="text-xl" />
        Admin Dashboard
      </Link>

      <button
        className="flex items-center gap-2 text-red-500 hover:underline"
        onClick={handleLogOut}
        title="Logout"
      >
        <TbLogout className="text-xl" />
        Logout
      </button>
    </div>
  );
}
