import React, { useState, useEffect, } from "react";
import { useParams } from "react-router-dom";
import FormInput from "./FormInput";

export default function UpdatePayment({ patientId, }) {
const {id} = useParams();

    // Fields for payment information.
  const [updatePayment, setupdatePayment] = useState({
    paymentMethod: "",
    paymentDate: "",
    paymentAmount: "",
   // patientId: patientId,
  });

  const handleFormInputChange = (e) => {
    const { name, value } = e.target;
    setupdatePayment((prev) => ({ 
        ...prev,
         [name]: 
         name=== "paymentAmount" ? parseFloat(value) || 0 
         : value 
        }));
  };

  const handleSubmitForm = async (e) => {
    e.preventDefault();

    console.log(id);
    // Validation: Ensure all required fields are filled
    if (updatePayment.paymentMethod === "" && updatePayment.paymentDate === "" && updatePayment.paymentAmount === "") {
      alert("Forms cannot be empty. Please fill out fields that require modification.");
      return;
    }

    const formattedPayment = {
        ...updatePayment,
      method: updatePayment.paymentMethod,
      paymentDate: updatePayment.paymentDate || null,
      amount: parseFloat(updatePayment.paymentAmount),
      patientId: Number(updatePayment.patientId),
    };

    try {
      const response = await fetch(`http://localhost:5044/api/payments/update/${id}`, {
        method: "PATCH",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(formattedPayment)
      });

      if (!response.ok) throw new Error("Payment submission failed.");
      alert("Payment updated");

      setupdatePayment({
        paymentMethod: "",
        paymentDate: "",
        paymentAmount: "",
       // patientId: patientId
      });
    } catch (err) {
      console.error(err);
      alert("Error submitting payment");
    }
  };

  return (
    <div className="size-lg bg-gray-800 text-white border-4 rounded-lg p-4">
      <form className="flex flex-col items-center" onSubmit={handleSubmitForm}>
        <h1 className="text-2xl mb-4">Update Payment</h1>

        <div className="flex flex-wrap gap-2 items-center justify-center">
          <FormInput
            props={{
              inputName: "Method",
              type: "text",
              name: "paymentMethod",
              value: updatePayment.paymentMethod,
              placeholder: "Visa",
              onChange: handleFormInputChange,
            }}
          />
          <FormInput
            props={{
              inputName: "Date of Payment",
              type: "date",
              name: "paymentDate",
              value: updatePayment.paymentDate,
              placeholder: "2025-05-20",
              onChange: handleFormInputChange,
            }}
          />
          <FormInput
            props={{
              inputName: "Amount",
              type: "number",
              name: "paymentAmount",
              value: updatePayment.paymentAmount,
              placeholder: "50.50",
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
  );
}
