import React, { useState } from "react";
import { useEffect } from "react";
import FormInput from "./FormInput";

export default function AddService({ patientId, onProcedureAdded }) {
  // State for holding data
  const [newProcedure, setNewProcedure] = useState({
    procedureName: "",
    procedureDate: "",
    cptCode: "",
    cptAmount: "",
    patientChargedAmount: "",
    patientId: patientId ?? 0,
  });

  const [msg, setMsg] = useState({});

  // Base URL for API requests
  const baseUrl = import.meta.env.VITE_API_BASE_URL;

  // This keeps patientId in sync if patientID changes
  useEffect(() => {
    setNewProcedure((prev) => ({
      ...prev,
      patientId: patientId ?? 0,
    }));
  }, [patientId]);

  // Handle input changes
  const handleFormInputChange = (e) => {
    const { name, value } = e.target;

    setNewProcedure((prev) => ({
      ...prev,
      [name]: value,
    }));
  };

  // Handle submission of form
  const handleSubmitForm = async (e) => {
    e.preventDefault(); // This prevents form from refreshing.

    setMsg({}); // Clear previous messages

    const errors = {}; // Object to hold validation errors

    // Validation checks
    if (!newProcedure.procedureName) {
      errors.procedureName = "Procedure name is required.";
    }

    if (!newProcedure.procedureDate) {
      errors.procedureDate = "Procedure date is required.";
    }

    if (newProcedure.patientChargedAmount === "" || isNaN(newProcedure.patientChargedAmount)) {
      errors.patientChargedAmount = "Patient charged amount is required.";
    }

    if (Object.keys(errors).length > 0) {
      setMsg(errors);
      return;
    }

    // Prepare the data for submission
    const formattedProcedure = {
      ...newProcedure,
      cptAmount: parseFloat(newProcedure.cptAmount) || 0,
      patientChargedAmount: parseFloat(newProcedure.patientChargedAmount) || 0,
      patientId: Number(newProcedure.patientId),
    };

    // Submit the data to the server
    try {
      const response = await fetch(`${baseUrl}/api/procedure`, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(formattedProcedure),
      });

      if (!response.ok) {
        const errorText = await response.text();
        throw new Error(`Server error ${response.status}: ${errorText}`);
      }

      // Only happens if the POST succeeded
      alert("Procedure added!");

      setNewProcedure({
        procedureName: "",
        procedureDate: "",
        cptCode: "",
        cptAmount: "",
        patientChargedAmount: "",
        patientId: patientId ?? 0,
      });

      setMsg({});

      // Call the callback to refresh the procedure list
      onProcedureAdded();
    } catch (err) {
      console.error("Submit failed:", err.message);
    }
  };

  return (
    <div>
      <div className="size-lg bg-gray-800 text-white border-4 rounded-lg p-4">
        <div className="m-2">
          <form
            className="flex flex-col items-center"
            onSubmit={handleSubmitForm}
          >
            <h1 className="text-2xl mb-4">Add New Procedure</h1>
            <div className="flex flex-wrap gap-2 items-center justify-center">
              <FormInput
                props={{
                  inputName: "Name",
                  type: "text",
                  name: "procedureName",
                  value: newProcedure.procedureName,
                  placeholder: "Blood Work",
                  onChange: handleFormInputChange,
                }}
              />

              <FormInput
                props={{
                  inputName: "Date of Service",
                  type: "date",
                  name: "procedureDate",
                  value: newProcedure.procedureDate,
                  onChange: handleFormInputChange,
                }}
              />

              <FormInput
                props={{
                  inputName: "CPT Code",
                  type: "text",
                  name: "cptCode",
                  value: newProcedure.cptCode,
                  placeholder: "ABC-123",
                  onChange: handleFormInputChange,
                }}
              />

              <FormInput
                props={{
                  inputName: "CPT Amount",
                  type: "number",
                  step: "0.01",
                  name: "cptAmount",
                  value: newProcedure.cptAmount,
                  placeholder: "75.00",
                  onChange: handleFormInputChange,
                }}
              />

              <FormInput
                props={{
                  inputName: "Patient Charge",
                  type: "number",
                  step: "0.01",
                  name: "patientChargedAmount",
                  value: newProcedure.patientChargedAmount,
                  placeholder: "50.00",
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
          <div className="mt-4 text-center">
            {msg.procedureName && (
              <p className="text-red-500">{msg.procedureName}</p>
            )}
            {msg.procedureDate && (
              <p className="text-red-500">{msg.procedureDate}</p>
            )}
            {msg.patientChargedAmount && (
              <p className="text-red-500">{msg.patientChargedAmount}</p>
            )}
          </div>
        </div>
      </div>
    </div>
  );
}
