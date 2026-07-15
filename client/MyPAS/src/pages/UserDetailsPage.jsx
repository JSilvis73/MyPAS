import { useEffect, useState } from "react";
import { useAuth } from "../context/AuthContext";
import FormInput from "../components/FormInput";

export default function UserDetailsPage() {
  const auth = useAuth();

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

  const handlePasswordChange = (e) => {
    alert("Submitting Change Password: Still needs implemented.");
  }

  const handleUpdateUser = (e) => {
    alert("Submitting Update User Details: Still needs implemented.")
  }


  return (
    <div className="w-3xl max-w-6xl flex flex-col  items-center gap-2 mx-auto bg-gray-800 text-white border border-gray-600 rounded-lg m-4 p-4 shadow-lg">
      <h1 className="text-2xl font-bold mb-4">User Details</h1>
      <h2 className="text-xl font-bold mb-2">Settings</h2>
      <div className="flex flex-col gap-2 text-left lg:grid lg:grid-cols-2">
        <p><strong>Username:</strong> {auth?.user?.userName || "N/A"}</p>
        <p><strong>Email:</strong> {auth?.user?.email || "N/A"}</p>
        <p><strong>First Name:</strong> {auth?.user?.firstName || "N/A"}</p>
        <p><strong>Last Name:</strong> {auth?.user?.lastName || "N/A"}</p>
        <p><strong>Phone:</strong> {auth?.user?.phone || "N/A"}</p>
        <p><strong>Address:</strong> {auth?.user?.address || "N/A"}</p>
        <p><strong>Role:</strong> {auth?.user?.role || "N/A"}</p>
      </div>
      <button type="button" className="mt-4 bg-blue-500 hover:bg-blue-700 text-white font-bold py-2 px-4 rounded" onClick={() => setEditingPassword(!editingPassword)}>
        {editingPassword ? "Cancel Change Password" : "Change Password"}
      </button>
      <button type="button" className="mt-4 bg-blue-500 hover:bg-blue-700 text-white font-bold py-2 px-4 rounded" onClick={() => setEditing(!editing)}>
        {editing ? "Cancel Edit User" : "Edit User Details"}
      </button>
      {editing && (
        <div className="mt-4 w-full">
          <h3 className="text-lg text-center font-bold mb-2">Edit User Details</h3>
          <form className="flex flex-col items-center gap-2" onSubmit={handleUpdateUser}>
            <FormInput props={{
              inputName: "Username:",
              type: "text",
              name: "userName",
              value: auth?.user?.userName || "",
              onChange: handleFormInputChange
            }} />
            <FormInput
              props={{
                inputName: "First Name:",
                type: "text",
                name: "firstName",
                value: auth?.user?.firstName || "",
                onChange: handleFormInputChange 
              }}
            />
            <FormInput
              props={{
                inputName: "Last Name:",
                type: "text",
                name: "lastName",
                value: auth?.user?.lastName || "",
                onChange: handleFormInputChange
              }}
            />
            <FormInput
              props={{
                inputName: "Email:",
                type: "email",
                name: "email",
                value: auth?.user?.email || "",
                onChange: handleFormInputChange
              }}
            />
            <button className="mt-4 bg-blue-500 hover:bg-blue-700 text-white font-bold py-2 px-4 rounded" type="submit">Submit</button>
          </form>
        </div>
      )}

      {editingPassword && (
         <div className="mt-4 w-full">
          <h3 className="text-lg text-center font-bold mb-2">Change Password</h3>
          <form className="flex flex-col items-center gap-2" onSubmit={handlePasswordChange}>
            <FormInput props={{
              inputName: "Password",
              type: "password",
              name: "password",
              value: "",
              onChange: handleFormInputChange
            }} />
            <FormInput props={{
              inputName: "New Password",
              type: "password",
              name: "newpassword",
              value: "",
              onChange: handleFormInputChange
            }} />
                        <FormInput props={{
              inputName: "Confirm New Password",
              type: "password",
              name: "confirmnewpassword",
              value: "",
              onChange: handleFormInputChange
            }} />
            <button className="mt-4 bg-blue-500 hover:bg-blue-700 text-white font-bold py-2 px-4 rounded" type="submit">Submit</button>
          </form>
          </div>
      )}
    </div>
  );
}
