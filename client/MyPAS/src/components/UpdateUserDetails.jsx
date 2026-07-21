import React, { useState } from "react";
import { useAuth } from "../context/AuthContext";
import FormInput from "./FormInput";

export default function UpdateUserDetails() {
  const { user } = useAuth();
  const [userFieldsToUpdate, setUserFieldsToUpdate] = useState({
    userName: user.userName,
    email: user.email,
    firstName: user.firstName,
    lastName: user.lastName,
    phone: user.phone,
    address: user.address,
    city: user.city,
    state: user.state,
    zip: user.zip
  });

  const handleFormInputChange = (e) => {
    const [{name: value}] = e.target;
    setUserFieldsToUpdate((prev) =>({
        ...prev,
        [name] : value
    })
    );
  }

  const handleUserDetailsUpdate = (e) => {
    e.preventDefault();
    alert("Implementing")
  }

  return (
    <div className="mt-4 w-full">
      <h3 className="text-lg text-center font-bold mb-2">Update User Details</h3>
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
            inputName: "Email",
            type: "text",
            name: "email",
            value: userFieldsToUpdate.email,
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
                <FormInput
          props={{
            inputName: "Address",
            type: "text",
            name: "address",
            value: userFieldsToUpdate.address,
            onChange: handleFormInputChange,
          }}
        />
                        <FormInput
          props={{
            inputName: "City",
            type: "text",
            name: "city",
            value: userFieldsToUpdate.city,
            onChange: handleFormInputChange,
          }}
        />
                        <FormInput
          props={{
            inputName: "State",
            type: "text",
            name: "state",
            value: userFieldsToUpdate.state,
            onChange: handleFormInputChange,
          }}
        />
                        <FormInput
          props={{
            inputName: "Zip",
            type: "text",
            name: "zip",
            value: userFieldsToUpdate.zip,
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
