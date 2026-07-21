import { useEffect, useState } from "react";
import { useAuth } from "../context/AuthContext";
import FormInput from "../components/FormInput";
import ChangePassword from "../components/ChangePassword";
import UpdateUserDetails from "../components/UpdateUserDetails";

export default function UserDetailsPage() {
  const {user} = useAuth();
  // Page State
  const [loading, setLoading] = useState(true);
  const [editing, setEditing] = useState(false);
  const [editingPassword, setEditingPassword] = useState(false);

  const handleFormInputChange = (e) => {
    const { name, value } = e.target;
    auth.setUser((prev) => ({
      ...prev,
      [name]: value
    }));
  };

  const handleUpdateUser = (e) => {
    alert("Submitting Update User Details: Still needs implemented.")
  }


  return (
    <div className="w-3xl max-w-6xl flex flex-col  items-center gap-2 mx-auto bg-gray-800 text-white border border-gray-600 rounded-lg m-4 p-4 shadow-lg">
      <h1 className="text-2xl font-bold mb-4">User Details</h1>
      <h2 className="text-xl font-bold mb-2">Settings</h2>
      <div className="flex flex-col gap-2 text-left lg:grid lg:grid-cols-2">
        <p><strong>Username:</strong> {user.userName || "N/A"}</p>
        <p><strong>Email:</strong> {user.email || "N/A"}</p>
        <p><strong>First Name:</strong> {user.firstName || "N/A"}</p>
        <p><strong>Last Name:</strong> {user.lastName || "N/A"}</p>
        <p><strong>Phone:</strong> {user.phone || "N/A"}</p>
        <p><strong>Address:</strong> {user.address || "N/A"}</p>
        <p><strong>Role:</strong> {user.roles || "N/A"}</p>
        
          
      
      </div>
      <button type="button" className="mt-4 bg-blue-500 hover:bg-blue-700 text-white font-bold py-2 px-4 rounded" onClick={() => setEditingPassword(!editingPassword)}>
        {editingPassword ? "Cancel Change Password" : "Change Password"}
      </button>
      <button type="button"  className="mt-4 bg-blue-500 hover:bg-blue-700 text-white font-bold py-2 px-4 rounded" onClick={() => setEditing(!editing)}>
        {editing ? "Cancel Edit User" : "Edit User Details"}
      </button>
      {editing && (
        <div className="mt-4 w-full">
          <UpdateUserDetails />

        </div>
      )}

      {editingPassword && (
        <ChangePassword />
      )}

      
    </div>
  );
}
