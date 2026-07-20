import { useState } from "react";
import { useAuth } from "../context/AuthContext";
import FormInput from "./FormInput";

export default function ChangePassword() {
  const {user} = useAuth();

  const baseUrl = import.meta.env.VITE_API_BASE_URL;
  const token = localStorage.getItem("token");


  const [newPasswordFields, setNewPasswordFields] = useState({
    userEmail: user.email,
    currentPassword: "",
    newPassword: "",
    confirmPassword:""
  });

  const handleFormInputChange = (e) => {
    const { name, value } = e.target;
    setNewPasswordFields((prevOptions) => ({
      ...prevOptions,
      [name]: value,
    }));
  };

  const handlePasswordChange = async (e) => {
    e.preventDefault();

    if (newPasswordFields.currentPassword == "")
      {
        alert("Current password must be populated.");
        throw new Error("Current password must be populated.");
      }
    

    if (newPasswordFields.newPassword != newPasswordFields.confirmPassword)
      {
        alert("New password and confirm password must match.");
        throw new Error("New password and confirm password must match.");
      }

    var changePasswordDTO = {
        email: newPasswordFields.userEmail,
        currentPassword: newPasswordFields.currentPassword,
        newPassword: newPasswordFields.newPassword
      };
  
      

    try {
      const response = await fetch(`${baseUrl}/api/auth/changePassword`, {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
          "Authorization": `Bearer ${token}`
        },
        body: JSON.stringify(changePasswordDTO),
      });

      if (!response.ok) {
        alert("Failed to change password. You may need to log back in.")
        throw new Error("Failed to change password.");
      }

      Alert("Password Changed.");
      console.log("Password changed.");

      // Clear Password Fields
      setNewPasswordFields({
        currentPassword: "",
        newPassword: "",
        confirmPassword: "",
      });
    } catch (error) {
      throw new Error("Failed to change password.");
    }
  };

  return (
    <div className="mt-4 w-full">
      <h3 className="text-lg text-center font-bold mb-2">Change Password</h3>
      <form
        className="flex flex-col items-center gap-2"
        onSubmit={handlePasswordChange}
      >
        <FormInput
          props={{
            inputName: "Current Password",
            type: "text",
            name: "currentPassword",
            value: newPasswordFields.currentPassword,
            onChange: handleFormInputChange,
          }}
        />
        <FormInput
          props={{
            inputName: "New Password",
            type: "text",
            name: "newPassword",
            value: newPasswordFields.newPassword,
            onChange: handleFormInputChange,
          }}
        />
        <FormInput
          props={{
            inputName: "Confirm New Password",
            type: "text",
            name: "confirmPassword",
            value: newPasswordFields.confirmPassword,
            onChange: handleFormInputChange,
          }}
        />
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
