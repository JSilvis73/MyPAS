import React from "react";
import { Link } from "react-router-dom";
import { FaUserCircle } from "react-icons/fa";
import { IoCreateOutline } from "react-icons/io5";
import { CiSearch } from "react-icons/ci";
import { FaTasks } from "react-icons/fa";
import { useAuth } from "../context/AuthContext";
import { CiSettings } from "react-icons/ci";

const MainLayout = ({ children }) => {
  const { user, signOut } = useAuth();

  const baseUrl = import.meta.env.VITE_API_BASE_URL;

  const handleLogout = () => {
    try {
      signOut(); // Clear auth state and local storage
      alert("You have been logged out.");
    } catch (error) {
      console.error("Error during logout:", error);
    }
  };

  return (
    <div className="flex flex-col min-h-screen mx-auto bg-gray-900 text-white ">
      {/* Header */}
      <header className="md:bg-gray-800 text-white p-4">
        <div className="flex justify-between   items-center max-w-6xl mx-auto">
          {/* Left: Logo */}
          <div className="flex-start text-xl font-bold">
            <Link to="/">MyPAS</Link>
          </div>

          {/* Center: Navigation */}
          <nav className="flex space-x-4 gap-2">
            <Link to="/" className="hover:underline">
              Home
            </Link>
            <Link
              to="/search"
              className="flex flex-row items-center gap-1 hover:underline"
            >
              <CiSearch />
              Search
            </Link>
            <Link
              to="/add-patient"
              className="flex flex-row items-center gap-1 hover:underline"
            >
              <IoCreateOutline />
              Create Patient
            </Link>
            <Link
              to="/operations"
              className="flex flex-row items-center gap-1 hover:underline"
            >
              <FaTasks />
              Operations
            </Link>
            <Link to="/about" className="hover:underline">
              About
            </Link>
            <Link to="/contact" className="hover:underline">
              Contact
            </Link>
          </nav>

          {/* Right: User Icon */}
          <div className="flex items-center gap-2">
            <FaUserCircle className="hover:animate-spin " />
            <p className="text-sm text-center">{user?.email}</p>

            <Link
              to="/user-details"
              className="flex flex-row items-center gap-1 hover:underline"
            >
              <CiSettings className="hover:animate-spin" title="Settings"  />
              
            </Link>

            <button
              className="text-sm text-red-500 hover:underline"
              onClick={handleLogout}
            >
              Logout
            </button>
          </div>
        </div>
      </header>

      {/* Main Content */}
      <main className="flex-grow max-h-content w-full ">{children}</main>

      {/* Footer */}
      <footer className="bg-gray-800 text-white text-center p-4">
        &copy; {new Date().getFullYear()} MyPAS
      </footer>
    </div>
  );
};

export default MainLayout;
