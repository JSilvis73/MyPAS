import React, { useState } from "react";
import FormInput from "./FormInput";

export default function AuthLogIn({setUser}) {
  // State to hold the authentication options.
  const [authOptions, setAuthOptions] = useState({
    email: "",
    password: "",
  });

  // Handle input changes.
  const handleFormInputChange = (e) => {
    const { name, value } = e.target;
    setAuthOptions((prevOptions) => ({
      ...prevOptions,
      [name]: value,
    }));
  };

  const handleSubmit = (e) => {
    e.preventDefault();
    // Form Validation
    if (!authOptions.email || !authOptions.password) {
      alert("Please fill in all fields");
      throw new Error("All fields are required");
    }

    // Email validation
    const emailPattern = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    if (!emailPattern.test(authOptions.email)) {
      alert("Invalid email format");
      throw new Error("Invalid email format");
    }

    // Password validation
    if (authOptions.password.length < 6) {
      alert("Password must be at least 6 characters long");
      throw new Error("Password must be at least 6 characters long");
    }

    // Submit the form data
    const baseUrl = import.meta.env.VITE_API_BASE_URL;
    fetch(`${baseUrl}/api/auth/login`, {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify(authOptions),
    })
      .then((response) => {
        if (!response.ok) throw new Error("Login failed");
        return response.json();
      })
      .then((data) => {
        alert("Login successful");
        // Handle successful login (e.g., redirect or update state)
        localStorage.setItem("token", data.token);
        localStorage.setItem("user", JSON.stringify(data.user));
        setUser(data.user); // Update the user state in the parent component
        console.log("User data:", data.user);
        //window.location.reload(); // Reload to reflect the logged-in state

      })
      .catch((error) => {
        console.error("Error during login:", error);
        alert("Login failed. Please try again.");
      });
      
  };

  return (
    <div className="border flex flex-col items-center p-2">
      <h2 className="text-2xl mb-4">
        <strong>Log In</strong>
      </h2>
      <form>
        <FormInput
          props={{
            inputName: "Email",
            type: "email",
            name: "email",
            placeholder: "ExampleUser@gmail.com",
            onChange: handleFormInputChange,
            value: authOptions.email,
          }}
        />
        <FormInput
          props={{
            inputName: "Password",
            type: "password",
            name: "password",
            placeholder: "*********",
            onChange: handleFormInputChange,
            value: authOptions.password,
          }}
        />
      </form>
      <button
        className="border rounded-xl p-2 mt-4 bg-blue-500 text-white hover:bg-blue-700"
        onClick={handleSubmit}
      >
        Log In
      </button> 
    </div>
  );
}
