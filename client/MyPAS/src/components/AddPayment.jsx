import React, { useState, useEffect } from "react";
import FormInput from "./FormInput";

export default function AddPayment({ patientId }) {
  const [newPayment, setNewPayment] = useState({
    paymentMethod: "",
    paymentDate: "",
    paymentAmount: "",
    patientId: patientId,
  });

  const [procedures, setProcedures] = useState([]);
  const [selectedProcedureId, setSelectedProcedureId] = useState(null);

  useEffect(() => {
    const fetchProcedures = async () => {
      try {
        const res = await fetch(`http://localhost:5044/api/procedure/patient/${patientId}`);
        const data = await res.json();
        setProcedures(data);
        if (data.length > 0) setSelectedProcedureId(data[0].id);
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

    const formattedPayment = {
      method: newPayment.paymentMethod,
      paymentDate: newPayment.paymentDate,
      amount: parseFloat(newPayment.paymentAmount) || 0,
      patientId: Number(newPayment.patientId),
      procedureId: selectedProcedureId
    };

    try {
      const response = await fetch("http://localhost:5044/api/payments", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(formattedPayment)
      });

      console.log(JSON.stringify(formattedPayment));
      if (!response.ok) throw new Error("Payment submission failed");
      alert("Payment added");

      setNewPayment({
        paymentMethod: "",
        paymentDate: "",
        paymentAmount: "",
        patientId: patientId
      });
    } catch (err) {
      console.error(err);
      alert("Error submitting payment");
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
            onChange={(e) => setSelectedProcedureId(Number(e.target.value))}
          >
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
    </div>
  );
}
