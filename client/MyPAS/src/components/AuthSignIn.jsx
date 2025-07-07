import React, { useState } from "react";
import FormInput from "./FormInput";

export default function AuthSignIn() {
  const [authOptions, setAuthOptions] = useState({
    email: "",
    password: "",
  });

  // Handle input changes
  const handleFormInputChange = (e) => {
    const { name, value } = e.target;
    setAuthOptions((prevOptions) => ({
      ...prevOptions,
      [name]: value,
    }));
  };

  return (
    <div className="border flex flex-col items-center p-2">
      <h2 className="text-2xl mb-4">
        <strong>Sign In</strong>
      </h2>
      <form>
        <FormInput
          props={{
            inputName: "Email",
            type: "email",
            name: "userEmail",
            value: authOptions.email,
            placeholder: "ExampleUser@gmail.com",
            onChange: handleFormInputChange,
          }}
        />
      </form>
    </div>
  );
}
