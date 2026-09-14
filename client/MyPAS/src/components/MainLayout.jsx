import React from "react";
import { Link } from "react-router-dom";
import { FaUserCircle } from "react-icons/fa";
import { IoCreateOutline } from "react-icons/io5";
import { CiSearch } from "react-icons/ci";
import { FaTasks } from "react-icons/fa";
import { useAuth } from "../context/AuthContext";
import { CiSettings } from "react-icons/ci";
import medkitLogo from "../assets/images/medKit.png";
import UserProfileMenu from "./UserProfileMenu";

const MainLayout = ({ children }) => {
  const { user, signOut } = useAuth();
  const [isUserProfileMenuOpen, setIsUserProfileMenuOpen] =
    React.useState(false);

  const openUserProfileMenu = () => {
    setIsUserProfileMenuOpen(!isUserProfileMenuOpen);
  };

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
      <header className="bg-gray-800 text-white p-4 shadow-md">
        <div className="max-w-6xl mx-auto">
          {/* Top row */}
          <div className="flex items-center justify-between">
            {/* Logo */}
            <Link to="/" className="shrink-0">
              <img
                src={medkitLogo}
                alt="MyPAS Logo"
                className="h-10 w-10 hover:animate-pulse"
              />
            </Link>

            {/* Navigation */}
            <nav className="hidden md:flex items-center gap-4">
              <Link to="/" className="hover:text-gray-300">
                Home
              </Link>

              <Link
                to="/search"
                className="flex items-center gap-1 hover:text-gray-300"
              >
                <CiSearch />
                Search
              </Link>

              <Link
                to="/add-patient"
                className="flex items-center gap-1 hover:text-gray-300"
              >
                <IoCreateOutline />
                Create Patient
              </Link>

              <Link
                to="/operations"
                className="flex items-center gap-1 hover:text-gray-300"
              >
                <FaTasks />
                Operations
              </Link>

              <Link to="/about" className="hover:text-gray-300">
                About
              </Link>

              <Link to="/contact" className="hover:text-gray-300">
                Contact
              </Link>
            </nav>

            {/* User */}
            <div className="relative shrink-0">
              <button
                onClick={openUserProfileMenu}
                className="flex items-center gap-2 hover:text-gray-300"
              >
                <FaUserCircle className="text-xl" />
                <span className="hidden lg:inline text-sm">{user?.email}</span>
              </button>

              {isUserProfileMenuOpen && (
                <div className="absolute right-0 top-full mt-2 bg-gray-800 border border-gray-600 rounded-lg p-2 shadow-lg z-50">
                  <UserProfileMenu />
                </div>
              )}
            </div>
          </div>

          {/* Mobile navigation */}
          <nav className="flex md:hidden justify-center flex-wrap gap-x-5 gap-y-2 mt-4 pt-3 border-t border-gray-700">
            <Link to="/" className="hover:text-gray-300">
              Home
            </Link>

            <Link
              to="/search"
              className="flex items-center gap-1 hover:text-gray-300"
            >
              <CiSearch />
              Search
            </Link>

            <Link
              to="/add-patient"
              className="flex items-center gap-1 hover:text-gray-300"
            >
              <IoCreateOutline />
              Create Patient
            </Link>

            <Link
              to="/operations"
              className="flex items-center gap-1 hover:text-gray-300"
            >
              <FaTasks />
              Operations
            </Link>

            <Link to="/about" className="hover:text-gray-300">
              About
            </Link>

            <Link to="/contact" className="hover:text-gray-300">
              Contact
            </Link>
          </nav>
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
