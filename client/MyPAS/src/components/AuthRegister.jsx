import React, { useState } from "react";
import FormInput from "./FormInput";

export default function AuthRegister() {
  const [newUserFormData, setNewUserFormData] = useState({
    email: "",
    password: "",
    confirmPassword: "",
  });

  const handleFormChange = (e) => {
    const { name, value } = e.target;
    setNewUserFormData((prevData) => ({
      ...prevData,
      [name]: value,
    }));
  };

  const handleSubmit = (e) => {
    e.preventDefault();
    // Form Validation
    if (!newUserFormData.email || !newUserFormData.password) {
      alert("Please fill in all fields");
      throw new Error("All fields are required");
    }

    // Email validation
    const emailPattern = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    if (!emailPattern.test(newUserFormData.email)) {
      alert("Invalid email format");
      throw new Error("Invalid email format");
    }

    // Ensure that passwords match.
    if (newUserFormData.password !== newUserFormData.confirmPassword) {
      alert("Passwords do not match");
      throw new Error("Passwords do not match");
    }




  };

  return (
    <div className="flex flex-col items-center border border-blue-600 w-sm ">
      <h2 className="text-2xl mb-4 flex flex-col items-center p-2">
        <strong>Register</strong>
      </h2>
      <form>
        <FormInput
          props={{
            name: "email",
            type: "email",
            inputName: "Email",
            placeholder: "Example1@gmail.com",
            onChange: handleFormChange,
            value: newUserFormData.email,
          }}
        />
        <FormInput
          props={{
            name: "password",
            type: "password",
            inputName: "Password",
            placeholder: "",
            onChange: handleFormChange,
            value: newUserFormData.password,
          }}
        />
                <FormInput
          props={{
            name: "confirmPassword",
            type: "password",
            inputName: "Confirm Password",
            placeholder: "",
            onChange: handleFormChange,
            value: newUserFormData.confirmPassword,
          }}
        />
      </form>
      <button
        className="bg-blue-600 text-white p-2 rounded mt-4"
        onClick={handleSubmit}
      >
        Register
      </button>
    </div>
  );
}
