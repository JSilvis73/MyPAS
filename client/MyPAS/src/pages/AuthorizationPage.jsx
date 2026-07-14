import React, { useState } from "react";
import AuthSignIn from "../components/AuthSignIn";
import AuthRegister from "../components/AuthRegister";
import { useAuth } from "../context/AuthContext";

export default function AuthorizationPage() {
  const { signIn } = useAuth();
  const [toggleRegisterComponent, setToggleRegisterComponent] = useState(false);

  const handleToggleRegister = () => {
    setToggleRegisterComponent((prev) => !prev);
  }


  const handleSignIn = (authResult) => {
    signIn(authResult);
  }

     
  

  return (
    <div>
      <div className="h-screen flex flex-col items-center justify-center bg-gray-800 text-white text-center rounded-lg">
        <h1 className="text-5xl font-bold mb-8">
          MyMed
        </h1>

        <div className="flex flex-col items-center">{toggleRegisterComponent ? <AuthRegister /> : <AuthSignIn />}</div>
        <button className="border rounded-lg m-4 p-2 hover:bg-blue-700" onClick={handleToggleRegister}>
          {!toggleRegisterComponent ? "Register" : "Log in"}
        </button>
      </div>
    </div>
  );
};
