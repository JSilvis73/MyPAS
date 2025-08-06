import React, { useState } from "react";
import FormInput from "./FormInput";

export default function AuthRegister() {
  // State to hold the authentication options.
  const [newUserFormData, setNewUserFormData] = useState({
    email: "",
    password: "",
    confirmPassword: "",
    firstName: "",
    lastName: "",
  });

  // Handle input changes.
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

    // Password validation
    if (newUserFormData.password.length < 6) {
      alert("Password must be at least 6 characters long");
      throw new Error("Password must be at least 6 characters long");
    }

    // Check for at least one uppercase letter in the password
    const hasUpperCase = /[A-Z]/.test(newUserFormData.password);
    if (!hasUpperCase) {
      alert("Password must contain at least one uppercase letter");
      throw new Error("Password must contain at least one uppercase letter");
    }

    // Check for at least one special character in the password
    // This regex checks for any character that is not a letter or number
    const regex = /[^a-zA-Z0-9 ]/;
    const hasSpecialChar = regex.test(newUserFormData.password);

    if (!hasSpecialChar) {
      alert("Password must contain at least one special character");
      throw new Error("Password must contain at least one special character");
    }

    // Ensure that passwords match.
    if (newUserFormData.password !== newUserFormData.confirmPassword) {
      alert("Passwords do not match");
      throw new Error("Passwords do not match");
    }

    // Submit the form data to the server
    const baseUrl = import.meta.env.VITE_API_BASE_URL;
    fetch(`${baseUrl}/api/Auth/register`, {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify({
        email: newUserFormData.email,
        password: newUserFormData.password,
        firstName: newUserFormData.firstName,
        lastName: newUserFormData.lastName,
      }),
    })
      .then((response) => {
        if (!response.ok) {
          throw new Error("Failed to register user");
        }
        return response.json();
      })
      .then((data) => {
        console.log("User registered successfully:", data);
        alert("Registration successful");
        // Reset form
        setNewUserFormData({
          email: "",
          password: "",
          confirmPassword: "",
        });
      })
      .catch((error) => {
        console.error("Error during registration:", error);
        alert("Registration failed");
      });
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
        <FormInput
          props={{
            name: "firstName",
            type: "text",
            inputName: "First Name",
            placeholder: "John",
            onChange: handleFormChange,
            value: newUserFormData.firstName,
          }}
        />
        <FormInput
          props={{
            name: "lastName",
            type: "text",
            inputName: "Last Name",
            placeholder: "Doe",
            onChange: handleFormChange,
            value: newUserFormData.lastName,
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
