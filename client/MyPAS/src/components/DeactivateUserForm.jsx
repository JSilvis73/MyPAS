import React, { useState } from "react";
import FormInput from "./FormInput";

export default function DeactivateUserForm() {
  const [deactivateUserForm, setDeactivateUserForm] = useState({
    email: "",
    confirmedDeactivate: "",
  });

  const baseURL = import.meta.env.VITE_API_BASE_URL;

  const token = localStorage.getItem("token");

  const handleFormInputChange = (e) => {
    const { name, value } = e.target;

    setDeactivateUserForm((prev) => ({
      ...prev,
      [name]: value,
    }));
  };

  const handleSubmit = (e) => {
    e.preventDefault();

    console.log(deactivateUserForm.confirmedDeactivate);
    if (!deactivateUserForm.email) {
      alert("Email is required.");
      return;
    }

    if (deactivateUserForm.confirmedDeactivate !== "Confirm") {
      alert("Please confirm deactivation.");
      return;
    }

    alert("Attempting to deactivate user.");

    
    fetch(`${baseURL}/api/Auth/deleteUserByEmail?emailToDelete=${encodeURIComponent(deactivateUserForm.email)}`, {
      method: "DELETE",
      headers: {
        "Content-Type": "application/json",
        "Authorization": `Bearer ${token}`,
      },
    })
      .then((response) => {
        if (response.ok) {
          alert("User deactivated successfully.");
        } else {
          alert("Failed to deactivate user.");
        }
      })
      .catch((error) => {
        console.error("Error deactivating user:", error);
        alert("An error occurred while deactivating the user.");
      });
  };

  return (
    <div className="mt-4 border-2 border-blue-500 rounded-lg">
      <h1 className="m-2">Deactivate User</h1>
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
            value: deactivateUserForm.email,
          }}
        />
        <label htmlFor="confirmedDeactivate">Confirm Deactivation</label>
        <select
          name="confirmedDeactivate"
          value={deactivateUserForm.confirmedDeactivate}
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
