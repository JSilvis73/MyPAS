import React, { useState } from "react";
import FormInput from "./FormInput";

export default function AuthRegister() {
  const [newUserFormData, setNewUserFormData] = useState({
    email:"",
    password:"",
    confirmPassword: "",
  });




  return (
    <div className="flex flex-col items-center border border-blue-600 w-sm ">
      <h2 className="text-2xl mb-4 flex flex-col items-center p-2">
        <strong>Register</strong>
      </h2>
      <form>
        <FormInput
          props={{
            type: "email",
            inputName: "Email",
            placeholder: "Example1@gmail.com",
            onChange: "",
            value: "",
          }}
        />
        <FormInput
          props={{
            type: "password",
            inputName: "Password",
            placeholder: "",
            onChange: "",
            value: "",
          }}
        />
                <FormInput
          props={{
            type: "password",
            inputName: "Confirm Password",
            placeholder: "",
            onChange: "",
            value: "",
          }}
        />
      </form>
    </div>
  );
}
