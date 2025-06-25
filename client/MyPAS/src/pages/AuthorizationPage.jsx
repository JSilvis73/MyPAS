import React, { useState } from "react";

export default function AuthorizationPage() {
  // State for authorization
  const [userCredentials, setUSerCredentials] = useState({
    userEmail: "",
    userPassword: "",
  });

  // Handle changes to form.
  const handleFormChanges = () => {};

  // Handle submition of form.
  const handleSubmit = (e) => {
    prompt("Form submit not set up.");
  };

  return (
    <div className="flex flex-col items-center">
      <form
        onSubmit={handleSubmit}
        className="flex flex-col items-center text-white bg-gray-800 gap-4 border rounded-lg p-4 mt-6"
      >
        <h1 className="text-2xl mb-4">
          <strong>Authorization</strong>
        </h1>
        <p>Please sign in.</p>
        <input
          className="border rounded-md p-1 "
          type="email"
          name="userEmail"
          // value={userEmail}
          placeholder="Example@email.com"
          //onChange={handleFormChanges}
        />

        <input
          className="border rounded-md p-1"
          type="password"
          name="userPassword"
          // value={userPassword}
          placeholder="Password"
          // onChange={handleFormChanges}
        />

        <button 
        className="p-1 mt-4 border rounded-lg hover:bg-white hover:text-black"
        type="submit">Sign In</button>
      </form>
    </div>
  );
}
