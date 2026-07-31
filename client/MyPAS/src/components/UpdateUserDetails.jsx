import React, { useState } from "react";
import { useAuth } from "../context/AuthContext";
import FormInput from "./FormInput";

export default function UpdateUserDetails() {
  const { user, refreshUser } = useAuth();
  const baseURL = import.meta.env.VITE_API_BASE_URL;
  const [userFieldsToUpdate, setUserFieldsToUpdate] = useState({
    userName: user.userName,
    firstName: user.firstName,
    lastName: user.lastName,
    phone: user.phone,
  });

  const handleFormInputChange = (e) => {
    const { name, value } = e.target;
    setUserFieldsToUpdate((prev) => ({
      ...prev,
      [name]: value,
    }));
  };

  const handleUserDetailsUpdate = (e) => {
    e.preventDefault();
    const token = localStorage.getItem("token");

    // Validation

    fetch(`${baseURL}/api/Auth/update`, {
      method: "PATCH",
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${token}`,
      },
      body: JSON.stringify({
        firstName: userFieldsToUpdate.firstName,
        lastName: userFieldsToUpdate.lastName,
        userName: userFieldsToUpdate.userName,
        phone: userFieldsToUpdate.phone,
      }),
    })
      .then((response) => {
        if (!response.ok) {
          throw new Error("Failed to update user.");
        }

        return response.json;
      })
      .then(async () => {
        await refreshUser();
        alert("User updated.");
      })
      .catch((error) => {
        console.error(error);
        alert(error.message);
      });
  };

  return (
    <div className="mt-4 w-full">
      <h3 className="text-lg text-center font-bold mb-2">
        Update User Details
      </h3>
      <form
        className="flex flex-col items-center gap-2"
        onSubmit={handleUserDetailsUpdate}
      >
        <div className="grid grid-cols-2 gap-2">
          <FormInput
            props={{
              inputName: "UserName",
              type: "text",
              name: "userName",
              value: userFieldsToUpdate.userName,
              onChange: handleFormInputChange,
            }}
          />
          <FormInput
            props={{
              inputName: "First Name",
              type: "text",
              name: "firstName",
              value: userFieldsToUpdate.firstName,
              onChange: handleFormInputChange,
            }}
          />
          <FormInput
            props={{
              inputName: "Last Name",
              type: "text",
              name: "lastName",
              value: userFieldsToUpdate.lastName,
              onChange: handleFormInputChange,
            }}
          />
          <FormInput
            props={{
              inputName: "Phone",
              type: "text",
              name: "phone",
              value: userFieldsToUpdate.phone,
              onChange: handleFormInputChange,
            }}
          />
        </div>
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
