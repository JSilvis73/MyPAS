import React, { useState } from "react";
import { useEffect } from "react";
import FormInput from "./FormInput";

export default function AddService({ patientId }) {
  const [newService, setNewService] = useState({
    serviceName: "",
    serviceDate: "",
    cptCode: "",
    cptAmount: "",
    patientChargedAmount: "",
    patientId: patientId ?? 0,
  });

  // 👇 this keeps patientId in sync if patientID changes
  useEffect(() => {
    setNewService((prev) => ({
      ...prev,
      patientId: patientId ?? 0,
    }));
  }, [patientId]);

  const handleFormInputChange = (e) => {
    const { name, value } = e.target;

    setNewService((prev) => ({
      ...prev,
      [name]:
        name === "cptAmount" || name === "patientChargedAmount"
          ? parseFloat(value) || 0
          : value,
    }));
  };

  const handleSubmitForm = async (e) => {
    e.preventDefault();

    const formattedService = {
      ...newService,
      cptAmount: parseFloat(newService.cptAmount) || 0,
      patientChargedAmount: parseFloat(newService.patientChargedAmount) || 0,
      patientId: Number(newService.patientId),
    };

    console.log("patientId prop:", patientId);
    console.log("newService before POST:", formattedService);

    try {
      const response = await fetch("http://localhost:5044/api/services", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(formattedService),
      });

      if (!response.ok) {
        const errorText = await response.text();
        throw new Error(`Server error ${response.status}: ${errorText}`);
      }
    } catch (err) {
      console.error("Submit failed:", err.message);
    }

    alert("Service added!");
    setNewService({
      serviceName: "",
      serviceDate: "",
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
            <h1 className="text-2xl mb-4">Add New Service</h1>
            <div className="flex flex-wrap gap-2 items-center justify-center">
              <FormInput
                props={{
                  inputName: "Name",
                  type: "text",
                  name: "serviceName",
                  value: newService.serviceName,
                  placeholder: "Blood Work",
                  onChange: handleFormInputChange,
                }}
              />

              <FormInput
                props={{
                  inputName: "Date of Service",
                  type: "date",
                  name: "serviceDate",
                  value: newService.serviceDate,
                  onChange: handleFormInputChange,
                }}
              />

              <FormInput
                props={{
                  inputName: "CPT Code",
                  type: "text",
                  name: "cptCode",
                  value: newService.cptCode,
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
                  value: newService.cptAmount,
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
                  value: newService.patientChargedAmount,
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
