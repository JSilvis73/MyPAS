import React, { useState } from "react";
import { useEffect } from "react";
import { useParams } from "react-router-dom";
import FormInput from "./FormInput";

export default function UpdateProcedure({ patientId }) {
  const { id } = useParams();

  // State for storing data
  const [newProcedure, setNewProcedure] = useState({
    procedureName: "",
    procedureDate: "",
    cptCode: "",
    cptAmount: "",
    patientChargedAmount: "",
    patientId: patientId ?? 0,
  });

  const [msg, setMsg] = useState({});

  const baseUrl = import.meta.env.VITE_API_BASE_URL;

  const handleFormInputChange = (e) => {
    const { name, value } = e.target;

    setNewProcedure((prev) => ({
      ...prev,
      [name]: value,
    }));
  };

  const handleSubmitForm = async (e) => {
    e.preventDefault();

    const errors = {};

    // Validation checks
    if (
      !newProcedure.procedureName &&
      !newProcedure.procedureDate &&
      !newProcedure.cptCode &&
      !newProcedure.cptAmount &&
      !newProcedure.patientChargedAmount
    ) {
      errors.fields = "No fields were populated.";
      setMsg(errors);
      return;
    }

    console.log(
      `Procedure before update:\nName: ${newProcedure.procedureName}\nDate: ${newProcedure.procedureDate}\nCPTCode: ${newProcedure.cptCode}\nCPTAmount ${newProcedure.cptAmount}\nPatientPay: ${newProcedure.patientChargedAmount}`,
    );

    const formattedProcedure = {
      ...newProcedure,
      procedureDate: newProcedure.procedureDate || null,
      cptAmount: parseFloat(newProcedure.cptAmount),
      patientChargedAmount: parseFloat(newProcedure.patientChargedAmount),
      patientId: Number(newProcedure.patientId),
    };

    console.log("patientId prop:", patientId);
    console.log("newProcedure before POST:", formattedProcedure);

    try {
      const response = await fetch(`${baseUrl}/api/procedure/${id}`, {
        method: "PATCH",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(formattedProcedure),
      });

      if (!response.ok) {
        const errorText = await response.text();
        throw new Error(`Server error ${response.status}: ${errorText}`);
      }

      alert("Procedure updated!");
      setNewProcedure({
        procedureName: "",
        procedureDate: "",
        cptCode: "",
        cptAmount: "",
        patientChargedAmount: "",
        patientId: patientId ?? 0,
      });

      setMsg({});
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
            <h1 className="text-2xl mb-4">Update Procedure</h1>
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
          <div className="m-4 text-center">
            {msg.fields && <p className="text-red-500">{msg.fields}</p>}
          </div>
        </div>
      </div>
    </div>
  );
}
