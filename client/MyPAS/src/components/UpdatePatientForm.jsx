import React from "react";
import FormInput from "./FormInput";
import { useState } from "react";

export default function UpdatePatientForm({ patient }) {
  // Fields for New Patient
  const [patientToUpdate, setPatientToUpdate] = useState({
    id: patient.id,
    firstName: patient.firstName,
    lastName: patient.lastName,
    address: patient.address,
    city: patient.city,
    state: patient.state,
    zip: patient.zip,
    age: patient.age,
    phone: patient.phone,
    email: patient.email,
  });
  const baseUrl = import.meta.env.VITE_API_BASE_URL;

  // Handle input changes
  const handleFormInputChange = (e) => {
    const { name, value } = e.target;
    setPatientToUpdate((prevOptions) => ({
      ...prevOptions,
      [name]: value,
    }));
  };

  const handleSubmitForm = async (e) => {
    e.preventDefault(); // This prevents form from refreshing.

    // Establish connection and create new patient
    try {
      const response = await fetch(`${baseUrl}/api/patients/${patient.id}`, {
        method: "PUT",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify(patientToUpdate),
      });

      console.log("Submitting update:", patientToUpdate);


      if (!response.ok) {
        throw new Error("Failed to update patient");
      }

      const result = await response.json();
      console.log("Patient updated:", result);

      alert("Patient updated");

      // Clear form data
      setPatientToUpdate({
        firstName: patient.firstName,
        lastName: patient.lastName,
        address: patient.address,
        city: patient.city,
        state: patient.state,
        zip: patient.zip,
        age: patient.age,
        phone: patient.phone,
        email: patient.email,
      });
    } catch (error) {
      throw new Error("Failed to update patient");
    }
  };

  return (
    <div className="size-lg bg-gray-800 text-white border-4 rounded-lg p-4">
      <div className="m-2">
        <form
          className="flex  flex-col items-center"
          onSubmit={handleSubmitForm}
        >
          <h1 className="text-2xl mb-4">Update Patient</h1>

          <div className="flex flex-wrap gap-2 items-center justify-center">
            <FormInput
              props={{
                inputName: "Last Name:",
                type: "text",
                name: "lastName",
                value: patientToUpdate.lastName,
                placeholder: "Doe",
                onChange: handleFormInputChange,
              }}
            />
            <FormInput
              props={{
                inputName: "First Name:",
                type: "text",
                name: "firstName",
                value: patientToUpdate.firstName,
                placeholder: "John",
                onChange: handleFormInputChange,
              }}
            />
            <FormInput
              props={{
                inputName: "Age:",
                type: "number",
                name: "age",
                value: patientToUpdate.age,
                placeholder: "18",
                onChange: handleFormInputChange,
              }}
            />

            <FormInput
              props={{
                inputName: "Address:",
                type: "text",
                name: "address",
                value: patientToUpdate.address,
                placeholder: "1234 Cherry St.",
                onChange: handleFormInputChange,
              }}
            />
            <FormInput
              props={{
                inputName: "City:",
                type: "text",
                name: "city",
                value: patientToUpdate.city,
                placeholder: "Kent",
                onChange: handleFormInputChange,
              }}
            />
            <FormInput
              props={{
                inputName: "State:",
                type: "text",
                name: "state",
                value: patientToUpdate.state,
                placeholder: "OH",
                onChange: handleFormInputChange,
              }}
            />
            <FormInput
              props={{
                inputName: "Zip:",
                type: "text",
                name: "zip",
                value: patientToUpdate.zip,
                placeholder: "44230",
                onChange: handleFormInputChange,
              }}
            />
            <FormInput
              props={{
                inputName: "Phone:",
                type: "text",
                name: "phone",
                value: patientToUpdate.phone,
                placeholder: "330-987-1234",
                onChange: handleFormInputChange,
              }}
            />
            <FormInput
              props={{
                inputName: "Email:",
                type: "text",
                name: "email",
                value: patientToUpdate.email,
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
