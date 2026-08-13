import React from "react";
import FormInput from "./FormInput";

export default function ActivateUserForm() {
  const [activateUserForm, setActivateUserForm] = React.useState({
    email: "",
    confirmedActivate: "",
  });

  const handleFormInputChange = (e) => {
    const { name, value } = e.target;
    setActivateUserForm((prev) => ({
      ...prev,
      [name]: value,
    }));
    console.log(activateUserForm);
  };

  const handleSubmit = (e) => {
    e.preventDefault();
    alert("Attempting to activate user.");

    // Validate form inputs
    if (!activateUserForm.email) {
      alert("Email is required.");
      return;
    }

    // Email validation
    const emailPattern = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    if (!emailPattern.test(activateUserForm.email)) {
      alert("Invalid email format.");
      return;
    }

    if (activateUserForm.confirmedActivate !== "Confirm") {
      alert("Please confirm activation.");
      return;
    }

    // API call to activate user
    const baseURL = import.meta.env.VITE_API_BASE_URL;
    const token = localStorage.getItem("token");

    try {
      fetch(
        `${baseURL}/api/Auth/activateUserAdmin?emailToActivate=${encodeURIComponent(activateUserForm.email)}`,
        {
          headers: {
            "Content-Type": "application/json",
            "Authorization": `Bearer ${token}`,
          },
          method: "PATCH",
          body: JSON.stringify({ email: activateUserForm.email }),
        },
      )
        .then((response) => {
            console.log("Response status:", response.status);
          if (!response.ok) {
            throw new Error("Failed to activate user.");
          }
          return response.json();
        })
        .then((data) => {
          console.log("User activated successfully:", data);
          alert("User activated successfully.");
        });
    } catch (error) {
      console.error("Error activating user:", error);
      alert("An error occurred while activating the user.");
    }
  };

  return (
    <div className="mt-4 border-2 border-blue-500 rounded-lg">
      <h1 className="m-2">Activate User</h1>
      <form
        className="p-2 flex flex-col items-center text-center gap-1"
        onSubmit={handleSubmit}
      >
        <FormInput
          props={{
            inputName: "User Email",
            type: "text",
            name: "email",
            placeholder: "Email",
            onChange: handleFormInputChange,
            value: activateUserForm.email,
          }}
        />
        <label htmlFor="confirmedActivate">Confirm Activation</label>
        <select
          name="confirmedActivate"
          value={activateUserForm.confirmedActivate}
          onChange={handleFormInputChange}
        >
          <option className="text-red-500" value="">
            Select
          </option>
          <option className="text-green-500" value="Confirm">
            Confirm
          </option>
        </select>
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
