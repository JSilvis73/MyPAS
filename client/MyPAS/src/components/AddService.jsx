import React, { useState } from "react";
import { useEffect } from "react";
import FormInput from "./FormInput";

export default function AddService({ patientId }) {
  const [newProcedure, setNewProcedure] = useState({
    procedureName: "",
    procedureDate: "",
    cptCode: "",
    cptAmount: "",
    patientChargedAmount: "",
    patientId: patientId ?? 0,
  });

  // 👇 this keeps patientId in sync if patientID changes
  useEffect(() => {
    setNewProcedure((prev) => ({
      ...prev,
      patientId: patientId ?? 0,
    }));
  }, [patientId]);

  const handleFormInputChange = (e) => {
    const { name, value } = e.target;

    setNewProcedure((prev) => ({
      ...prev,
      [name]:
        name === "cptAmount" || name === "patientChargedAmount"
          ? parseFloat(value) || 0
          : value,
    }));
  };

  const handleSubmitForm = async (e) => {
    e.preventDefault();

    const formattedProcedure = {
      ...newProcedure,
      cptAmount: parseFloat(newProcedure.cptAmount) || 0,
      patientChargedAmount: parseFloat(newProcedure.patientChargedAmount) || 0,
      patientId: Number(newProcedure.patientId),
    };

    console.log("patientId prop:", patientId);
    console.log("newProcedure before POST:", formattedProcedure);

    try {
      const response = await fetch("http://localhost:5044/api/procedure", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(formattedProcedure),
      });

      if (!response.ok) {
        const errorText = await response.text();
        throw new Error(`Server error ${response.status}: ${errorText}`);
      }
    } catch (err) {
      console.error("Submit failed:", err.message);
    }

    alert("Procedure added!");
    setNewProcedure({
      procedureName: "",
      procedureDate: "",
      cptCode: "",
      cptAmount: "",
      patientChargedAmount: "",
      patientId: patientId ?? 0,
    });
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
        </div>
      </div>
    </div>
  );
}
