import React, { useState, useEffect } from "react";
import FormInput from "./FormInput";

export default function AddPayment({ patientId }) {
  // State for holding data
  const [newPayment, setNewPayment] = useState({
    paymentMethod: "",
    paymentDate: "",
    paymentAmount: "",
    patientId: patientId,
  });
  const [msg, setMsg] = useState({});
  const [procedures, setProcedures] = useState([]);
  const [selectedProcedureId, setSelectedProcedureId] = useState(null);
  const baseUrl = import.meta.env.VITE_API_BASE_URL;

  useEffect(() => {
    const fetchProcedures = async () => {
      try {
        const res = await fetch(
          `${baseUrl}/api/procedure/patient/${patientId}`,
        );
        const data = await res.json();
        setProcedures(data);
        setSelectedProcedureId(null);
      } catch (err) {
        console.error("Failed to load procedures:", err);
      }
    };
    fetchProcedures();
  }, [patientId]);

  const handleFormInputChange = (e) => {
    const { name, value } = e.target;
    setNewPayment((prev) => ({ ...prev, [name]: value }));
  };

  const handleSubmitForm = async (e) => {
    e.preventDefault();

    // Validation checks
    const errors = {};

    console.log(
      `Payment details\nMethod: ${newPayment.paymentMethod}\nDate: ${newPayment.paymentDate}\nAmountPaid: ${newPayment.paymentAmount}\nProcedure: ${selectedProcedureId}`,
    );

    if (!newPayment.paymentMethod) {
      errors.paymentMethod = "Payment method is required.";
    }

    if (!newPayment.paymentDate) {
      errors.paymentDate = "Payment date is required.";
    }

    if (newPayment.paymentAmount === "" || isNaN(newPayment.paymentAmount)) {
      errors.paymentAmount = "Payment amount is required.";
    }

    if (selectedProcedureId === null) {
      errors.selectedProcedureId = "A procedure must be selected.";
    }

    if (Object.keys(errors).length > 0) {
      setMsg(errors);
      return;
    }

    // Construct payment for api request
    const formattedPayment = {
      method: newPayment.paymentMethod,
      paymentDate: newPayment.paymentDate,
      amount: parseFloat(newPayment.paymentAmount) || 0,
      patientId: Number(newPayment.patientId),
      procedureId: selectedProcedureId,
    };

    // Send Request
    try {
      const response = await fetch(`${baseUrl}/api/payments`, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(formattedPayment),
      });

      console.log(JSON.stringify(formattedPayment));

      if (!response.ok) {
        const errorText = await response.text();
        throw new Error(`Server error ${response.status}: ${errorText}`);
      }

      alert("Payment added");

      setNewPayment({
        paymentMethod: "",
        paymentDate: "",
        paymentAmount: "",
        patientId: patientId,
      });

      setSelectedProcedureId(null);
    } catch (err) {
      console.error("Submit failed:", err.message);
    }
  };

  return (
    <div className="size-lg bg-gray-800 text-white border-4 rounded-lg p-4">
      <form className="flex flex-col items-center" onSubmit={handleSubmitForm}>
        <h1 className="text-2xl mb-4">Add New Payment</h1>

        <div className="flex flex-wrap gap-2 items-center justify-center">
          <FormInput
            props={{
              inputName: "Method",
              type: "text",
              name: "paymentMethod",
              value: newPayment.paymentMethod,
              placeholder: "Visa",
              onChange: handleFormInputChange,
            }}
          />
          <FormInput
            props={{
              inputName: "Date of Payment",
              type: "date",
              name: "paymentDate",
              value: newPayment.paymentDate,
              placeholder: "2025-05-20",
              onChange: handleFormInputChange,
            }}
          />
          <FormInput
            props={{
              inputName: "Amount",
              type: "number",
              name: "paymentAmount",
              value: newPayment.paymentAmount,
              placeholder: "50.50",
              onChange: handleFormInputChange,
            }}
          />
        </div>

        <div className="mt-4">
          <label className="mr-2">Service:</label>
          <select
            className="border p-2 rounded bg-white text-black"
            value={selectedProcedureId || ""}
            onChange={(e) =>
              setSelectedProcedureId(
                e.target.value === "" ? null : Number(e.target.value),
              )
            }
          >
            <option value="">Select Procedure</option>
            {procedures.map((p) => (
              <option key={p.id} value={p.id}>
                {p.id}: {p.procedureName || `Procedure #${p.id}`}
              </option>
            ))}
          </select>
        </div>

        <button
          type="submit"
          className="mt-4 border rounded-lg p-2 bg-gray-600 hover:bg-black"
        >
          Submit
        </button>
      </form>
      <div className="m-4 text-center">
        {msg.paymentMethod && (
          <p className="text-red-500">{msg.paymentMethod}</p>
        )}
        {msg.paymentDate && <p className="text-red-500">{msg.paymentDate}</p>}
        {msg.paymentAmount && (
          <p className="text-red-500">{msg.paymentAmount}</p>
        )}
        {msg.selectedProcedureId && (
          <p className="text-red-500">{msg.selectedProcedureId}</p>
        )}
      </div>
    </div>
  );
}
