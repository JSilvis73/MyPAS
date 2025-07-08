import React, { useState } from "react";
import AuthSignIn from "../components/AuthSignIn";
import AuthRegister from "../components/AuthRegister";

export default function AuthorizationPage() {
  const [toggleRegisterComponent, setToggleRegisterComponent] = useState(false);

  const handleToggleRegister = () => {
    setToggleRegisterComponent((prev) => !prev);
  };

  return (
    <div>
      <div className="size-lg bg-gray-800 text-white text-center border-4 rounded-lg p-4">
        <h2 className="text-2xl mb-4">
          <strong>MyMed</strong>
        </h2>
        <h2>Authorization</h2>

        <div className="flex flex-col items-center">{toggleRegisterComponent ? <AuthRegister /> : <AuthSignIn />}</div>
        <button className=" border rounded-xl p-2" onClick={handleToggleRegister}>
          {!toggleRegisterComponent ? "Register" : "Sign in"}
        </button>
      </div>
    </div>
  );
}
