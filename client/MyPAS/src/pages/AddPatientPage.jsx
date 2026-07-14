import React, { use, useState } from "react";
import FormInput from "../components/FormInput";

export default function AddPatientPage() {
  const [isSubmitting, setIsSubmitting] = useState(false);
  const [msg, setMsg] = useState({});
  // Fields for New Patient
  const [newPatient, setNewPatient] = useState({
    firstName: "",
    lastName: "",
    address: "",
    city: "",
    state: "",
    zip: "",
    age: "",
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
    setMsg({});
  };

  // Handle submition of form.
  const handleSubmit = async (e) => {
    e.preventDefault(); // This prevents form from refreshing.

  if (!newPatient.firstName ) 
    { 
      setMsg({ ...msg, firstName: "First name is required." });
      return;
    }

    if (!newPatient.lastName) {
      setMsg({ ...msg, lastName: "Last name is required." });
      return;
    }

  if (newPatient.age < 1) {
    setMsg({ ...msg, age: "Age must be at least 1." });
    return;
  }

  if (newPatient.email === "" && newPatient.phone === "") {
    setMsg({ ...msg, email: "Either email or phone is required." });
    return;
  }

      if (isSubmitting) return;
    setIsSubmitting(true);

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

      setMsg({ ...msg, success: "Patient added successfully." });

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
      setIsSubmitting(false);
    } catch (error) {
      setIsSubmitting(false);
      throw new Error("Failed to add patient");
    }
  };

  return (
    <div className="m-4 max-w-xl mx-auto bg-gray-800 text-white border border-gray-600 rounded-lg p-6 shadow-lg">
      
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
            disabled={isSubmitting}
            className="mt-4 border rounded-lg p-2 bg-gray-600 hover:bg-black"
          >
            {isSubmitting ? "Submitting..." : "Submit"}
          </button>
        </form>
    
      <div className="text-center">
        {msg.firstName && <p className="text-red-500">{msg.firstName}</p>}
        {msg.lastName && <p className="text-red-500">{msg.lastName}</p>}
        {msg.age && <p className="text-red-500">{msg.age}</p>}
        {msg.email && <p className="text-red-500">{msg.email}</p>}
        {msg.success && <p className="text-green-500">{msg.success}</p>}
      </div>
    </div>
  );
}
