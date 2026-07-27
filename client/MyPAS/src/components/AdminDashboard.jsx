import React, { useState } from "react";
import { useAuth } from "../context/AuthContext";

export default function AdminDashboard() {
  // State for display
  const { user } = useAuth();
  const [adminAction, setAdminAction] = useState(null);

  const handleAdminAction = (e) => {
    const action = e.target.name;

    if (action === "deleteUser") {
      handleDeleteUser();
      return;
    }

    setAdminAction((current) => (current === action ? null : action));
  };

  const handleDeleteUser = (e) => {
    var confirmation = confirm(
      `Are you sure you want to delete user: ${user.userName}?`,
    );

    confirmation ? alert("User has been deleted.") : alert("User not deleted.");
  };
  return (
    <div className="m-2 text-center">
      <h1 className="m-2 text-xl font-bold">Admin Actions</h1>
      <div className="flex flex-wrap gap-2">
        <button
          name="addRole"
          className="p-2 border rounded-md hover:bg-green-600"
          onClick={handleAdminAction}
        >
          {adminAction === "addRole"
            ? "Cancel Add User To Role"
            : "Add User To Role"}
        </button>
        <button
          name="removeRole"
          className="p-2 border rounded-md hover:bg-orange-600"
          onClick={handleAdminAction}
        >
          {adminAction === "removeRole"
            ? "Cancel Remove User Role"
            : "Remove User Role"}
        </button>
        <button
          name="deleteUser"
          className="p-2 border rounded-md hover:bg-red-600"
          onClick={handleAdminAction}
        >
          Delete User
        </button>
      </div>
      <div className="m-2">{adminAction ? "Assigning" : "N/A"}</div>
    </div>
  );
}
