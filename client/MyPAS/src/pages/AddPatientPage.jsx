import React, { use, useState } from "react";
import FormInput from "../components/FormInput";

export default function AddPatientPage() {
  // Fields for New Patient
  const [newPatient, setNewPatient] = useState({
    firstName: "",
    lastName: "",
    address: "",
    city: "",
    state: "",
    zip: "",
    age: 0,
    phone: "",
    email: "",
  });

  const baseUrl = import.meta.env.VITE_API_BASE_URL;

  // Handle input changes
  const handleFormInputChange = (e) => {
    const { name, value } = e.target;
    setNewPatient((prevOptions) => ({
      ...prevOptions,
      [name]: value,
    }));
  };

  // Handle submition of form.
  const handleSubmit = async (e) => {
    e.preventDefault(); // This prevents form from refreshing.

    // Establish connection and create new patient
    try {
      const response = await fetch(`${baseUrl}/api/patients`, {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify(newPatient),
      });

      if (!response.ok) {
        throw new Error("Failed to add patient");
      }

      const result = await response.json();
      console.log("Patient added:", result);

      alert("New patient added");

      // Clear form data
      setNewPatient({
        firstName: "",
        lastName: "",
        address: "",
        city: "",
        state: "",
        zip: "",
        age: 0,
        phone: "",
        email: "",
      });
    } catch (error) {
      throw new Error("Failed to add patient");
    }
  };

  return (
    <div className="size-lg bg-gray-800 text-white border-4 rounded-lg p-4">
      <div className="m-2">
        <form className="flex  flex-col items-center" onSubmit={handleSubmit}>
          <h2 className="text-2xl mb-4"><strong>Create New Patient</strong></h2>
          <div className="flex flex-wrap gap-2 items-center justify-center">
            <FormInput
              props={{
                inputName: "Last Name:",
                type: "text",
                name: "lastName",
                value: newPatient.lastName,
                placeholder: "Doe",
                onChange: handleFormInputChange,
              }}
            />
            <FormInput
              props={{
                inputName: "First Name:",
                type: "text",
                name: "firstName",
                value: newPatient.firstName,
                placeholder: "John",
                onChange: handleFormInputChange,
              }}
            />
            <FormInput
              props={{
                inputName: "Age:",
                type: "number",
                name: "age",
                value: newPatient.age,
                placeholder: "18",
                onChange: handleFormInputChange,
              }}
            />

            <FormInput
              props={{
                inputName: "Address:",
                type: "text",
                name: "address",
                value: newPatient.address,
                placeholder: "1234 Cherry St.",
                onChange: handleFormInputChange,
              }}
            />
            <FormInput
              props={{
                inputName: "City:",
                type: "text",
                name: "city",
                value: newPatient.city,
                placeholder: "Kent",
                onChange: handleFormInputChange,
              }}
            />
            <FormInput
              props={{
                inputName: "State:",
                type: "text",
                name: "state",
                value: newPatient.state,
                placeholder: "OH",
                onChange: handleFormInputChange,
              }}
            />
            <FormInput
              props={{
                inputName: "Zip:",
                type: "text",
                name: "zip",
                value: newPatient.zip,
                placeholder: "44230",
                onChange: handleFormInputChange,
              }}
            />
            <FormInput
              props={{
                inputName: "Phone:",
                type: "text",
                name: "phone",
                value: newPatient.phone,
                placeholder: "330-987-1234",
                onChange: handleFormInputChange,
              }}
            />
            <FormInput
              props={{
                inputName: "Email:",
                type: "text",
                name: "email",
                value: newPatient.email,
                placeholder: "JDoe10@Gmail.com",
                onChange: handleFormInputChange,
              }}
            />
          </div>
          <button
            type="submit"
            className="mt-4 border rounded-lg p-2 bg-gray-600 hover:bg-black"
          >
            Submit
          </button>
        </form>
      </div>
    </div>
  );
}
