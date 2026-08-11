import { useEffect, useState } from "react";
import { useAuth } from "../context/AuthContext";
import FormInput from "../components/FormInput";
import ChangePassword from "../components/ChangePassword";
import UpdateUserDetails from "../components/UpdateUserDetails";
import AdminDashboard from "../components/AdminDashboard";

export default function UserDetailsPage() {
  const { user, deactivateSelf, refreshUser, signOut } = useAuth();
  console.log("User: ", user);
  // Page State
  const [loading, setLoading] = useState(true);
  const [editing, setEditing] = useState(false);
  const [editingPassword, setEditingPassword] = useState(false);

  const handleFormInputChange = (e) => {
    const { name, value } = e.target;
    auth.setUser((prev) => ({
      ...prev,
      [name]: value,
    }));
  };

  const handleDeactivateUser = async () => {
    try {
      if (
        !confirm(
          "Are you sure you want to deactivate your account? This action cannot be undone.",
        )
      ) {
        return;
      }
      await deactivateSelf();
      alert("User deactivated successfully.");
      signOut();
    } catch (error) {
      console.error("Error deactivating user:", error);
      alert("Failed to deactivate user.");
    }
  };

  return (
    <div className="w-3xl max-w-6xl flex flex-col  items-center gap-2 mx-auto bg-gray-800 text-white border border-gray-600 rounded-lg m-4 p-4 shadow-lg">
      <h1 className="text-2xl font-bold mb-4">
        {" "}
        {user?.roles?.includes("Admin") ? "Admin" : "User"} Details
      </h1>
      <h2 className="text-xl font-bold mb-2">Settings</h2>
      <div className="m-2 flex flex-col gap-2 text-left lg:grid lg:grid-cols-2">
        <p>
          <strong>Username:</strong> {user.userName || "N/A"}
        </p>
        <p>
          <strong>Email:</strong> {user.email || "N/A"}
        </p>
        <p>
          <strong>First Name:</strong> {user.firstName || "N/A"}
        </p>
        <p>
          <strong>Last Name:</strong> {user.lastName || "N/A"}
        </p>
        <p>
          <strong>Phone:</strong> {user.phone || "N/A"}
        </p>
        <p>Active: {user.isActive ? "Yes" : "No"}</p>
      </div>

      <div>
        <strong className="m-2">Roles:</strong>
        <div className="flex flex-wrap gap-2">
          {user.roles?.map((role) => (
            <p key={role}>{role}</p>
          ))}
        </div>
      </div>

      <button
        type="button"
        className="mt-4 bg-blue-500 hover:bg-blue-700 text-white font-bold py-2 px-4 rounded"
        onClick={() => setEditingPassword(!editingPassword)}
      >
        {editingPassword ? "Cancel Change Password" : "Change Password"}
      </button>
      <button
        type="button"
        className="mt-4 bg-blue-500 hover:bg-blue-700 text-white font-bold py-2 px-4 rounded"
        onClick={() => setEditing(!editing)}
      >
        {editing ? "Cancel Edit User" : "Edit User Details"}
      </button>
      <button
        type="button"
        className="mt-4 bg-red-500 hover:bg-red-700 text-white font-bold py-2 px-4 rounded"
        onClick={handleDeactivateUser}
      >
        Deactivate Account
      </button>

      {editing && (
        <div className="mt-4 w-full">
          <UpdateUserDetails />
        </div>
      )}

      {editingPassword && <ChangePassword />}

      {user?.roles?.includes("Admin") ? <AdminDashboard /> : ""}
    </div>
  );
}
