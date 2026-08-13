import React, { useState } from "react";
import { useAuth } from "../context/AuthContext";
import AssignRole from "./AssignRole";
import DeactivateUserForm from "./DeactivateUserForm";
import ActivateUserForm from "./ActivateUserForm";

export default function AdminDashboard() {
  // State for display
  const { user } = useAuth();
  const [adminAction, setAdminAction] = useState(null);

  const handleAdminAction = (e) => {
    const action = e.target.name;

    setAdminAction((current) => (current === action ? null : action));
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
          name="activateUser"
          className="p-2 border rounded-md hover:bg-green-600"
          onClick={handleAdminAction}
        >
          Activate User
        </button>
        <button
          name="deactivateUser"
          className="p-2 border rounded-md hover:bg-red-600"
          onClick={handleAdminAction}
        >
          Deactivate User
        </button>
      </div>
      <div className="m-2">
        {adminAction === "addUserToRole" ? <AssignRole mode={adminAction}/> : null}
      {adminAction =="removeUserFromRole" ? <AssignRole mode ={adminAction} /> : null}
      {adminAction == "activateUser" ? <ActivateUserForm  mode={adminAction} /> : null }
      {adminAction == "deactivateUser" ? <DeactivateUserForm  mode={adminAction} /> : null }
      </div>
    </div>
  );
}
