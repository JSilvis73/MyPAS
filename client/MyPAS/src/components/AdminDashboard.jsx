import React, { useState } from "react";
import { useAuth } from "../context/AuthContext";
import AssignRole from "./AssignRole";

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
          name="addUserToRole"
          className="p-2 border rounded-md hover:bg-green-600"
          onClick={handleAdminAction}
        >
          {adminAction === "addUserToRole"
            ? "Cancel Add User To Role"
            : "Add User To Role"}
        </button>
        <button
          name="removeUserFromRole"
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
      <div className="m-2">
        {adminAction === "addUserToRole" ? <AssignRole mode={adminAction}/> : null}
      {adminAction =="removeUserFromRole" ? <AssignRole mode ={adminAction} /> : null}
      </div>
    </div>
  );
}
