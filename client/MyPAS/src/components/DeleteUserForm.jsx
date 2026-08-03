import React, { useState } from "react";
import FormInput from "./FormInput";

export default function DeleteUserForm() {
  const [deleteUserForm, setDeleteUserForm] = useState({
    email: "",
    confirmedDelete: "",
  });

  const baseURL = import.meta.env.VITE_API_BASE_URL;

  const token = localStorage.getItem("token");

  const handleFormInputChange = (e) => {
    const { name, value } = e.target;

    setDeleteUserForm((prev) => ({
      ...prev,
      [name]: value,
    }));
  };

  const handleSubmit = (e) => {
    e.preventDefault();

    console.log(deleteUserForm.confirmedDelete);
    if (!deleteUserForm.email) {
      alert("Email is required.");
      return;
    }

    if (deleteUserForm.confirmedDelete !== "Confirm") {
      alert("Please confirm delete.");
      return;
    }

    alert("Attempting to delete user.");

    
    fetch(`${baseURL}/api/Auth/deleteUserByEmail?emailToDelete=${encodeURIComponent(deleteUserForm.email)}`, {
      method: "DELETE",
      headers: {
        "Content-Type": "application/json",
        "Authorization": `Bearer ${token}`,
      },
    })
      .then((response) => {
        if (response.ok) {
          alert("User deleted successfully.");
        } else {
          alert("Failed to delete user.");
        }
      })
      .catch((error) => {
        console.error("Error deleting user:", error);
        alert("An error occurred while deleting the user.");
      });
  };

  return (
    <div className="mt-4 border-2 border-blue-500 rounded-lg">
      <h1 className="m-2">Delete User</h1>
      <form
        className="p-2 flex flex-col items-center text-center gap-1"
        onSubmit={handleSubmit}
      >
        <FormInput
          props={{
            inputName: "User Email",
            type: "text",
            name: "email",
            placeholder: "Email",
            onChange: handleFormInputChange,
            value: deleteUserForm.email,
          }}
        />
        <label htmlFor="confirmedDelete">Confirm Delete</label>
        <select
          name="confirmedDelete"
          value={deleteUserForm.confirmedDelete}
          onChange={handleFormInputChange}
          
        >
          <option className="text-red-500" value="">Unconfirmed</option>
          <option className="text-green-500" value="Confirm">Confirmed</option>
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
