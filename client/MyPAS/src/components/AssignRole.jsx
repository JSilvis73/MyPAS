import React, { useState } from "react";
import FormInput from "./FormInput";

export default function AssignRole({ mode }) {
  const [assignRoleForm, setAssignRoleForm] = useState({
    email: "",
    role: "",
  });

  const availableRoles = ["Admin", "User"];

  const handleFormInputChange = (e) => {
    const { name, value } = e.target;
    setAssignRoleForm((prev) => ({ ...prev, [name]: value }));
    console.log(assignRoleForm);
  };

  const handleSubmit = (e) => {
    e.preventDefault();
    
    console.log(mode);

    // Email validation
    if (assignRoleForm.email === "") {
      alert("Email is required.");
      throw new Error("Email is required.");
    }

    const emailPattern = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;

    if (!emailPattern.test(assignRoleForm.email)) {
      alert("Invalid email format");
      throw new Error("Invalid email format.");
    }

    const baseURL = import.meta.env.VITE_API_BASE_URL;
    const token = localStorage.getItem("token");

    console.log(`${baseURL}/api/Auth/${mode}`);

    fetch(`${baseURL}/api/Auth/${mode}`, {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
        "Authorization": `Bearer ${token}`,
      },
      body: JSON.stringify({
        email: assignRoleForm.email,
        role: assignRoleForm.role,
      }),
    })
    .then((response) => {
      if (!response.ok) {
        throw new Error("Failed to change role.");
      }
      return response.json();
    })
    .then((data) => {
      console.log("User role changed successfully.");
      alert("Role change successful.");

      // Reset form
      setAssignRoleForm(
        {
          email: "",
          role: ""
        }
      );
    })





  };

  return (
    <div className="">
      <h1 className="m-1 text-xl">Assign Role</h1>
      <p>
        {mode === "addUserToRole" ? "Add Role" : ""}
        {mode === "removeUserFromRole" ? "Remove Role" : ""}
      </p>
      <form
        onSubmit={handleSubmit}
        className="m-2 p-2 border-2 border-blue-500 rounded-lg  flex flex-col items-center"
      >
        <FormInput
          props={{
            inputName: "User Email",
            type: "text",
            name: "email",
            placeholder: "Email",
            onChange: handleFormInputChange,
            value: assignRoleForm.email,
          }}
        />
        <select
          name="role"
          value={assignRoleForm.role}
          onChange={handleFormInputChange}
          className="m-2 p-1 border rounded-lg"
        >
          <option className="text-black" value="">Select Role</option>
          <option className="text-black" value="Admin">
            Admin
          </option>
          <option className="text-black" value="User">
            User
          </option>
        </select>
        <button
          className="mt-4 bg-blue-500 hover:bg-blue-700 text-white font-bold py-2 px-4 rounded"
          type="submit"
        >
          Submit
        </button>
      </form>
    </div>
  );
}
