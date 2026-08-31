import React, { useEffect, useState } from "react";
import { useParams, useLocation, useNavigate } from "react-router-dom";
import UpdateProcedure from "../components/UpdateProcedure";
import UpdatePayment from "../components/UpdatePayment";

export default function DisplayListDetails() {
  const { id } = useParams();
  const [item, setItem] = useState(null);
  const [itemType, setItemType] = useState(null); // 'procedure' or 'payment'
  const [loading, setLoading] = useState(true);
  const [isUpdating, setIsUpdating] = useState(false);
  const baseUrl = import.meta.env.VITE_API_BASE_URL;

  const navigation = useNavigate();
  const location = useLocation();

  useEffect(() => {
    const fetchService = async () => {
      try {
        const itemType = location.pathname.includes("/payments/")
          ? "payments"
          : "procedure";
        setItemType(itemType);

        const response = await fetch(`${baseUrl}/api/${itemType}/${id}`);
        if (!response.ok)
          throw new Error(
            `${itemType.charAt(0).toUpperCase() + itemType.slice(1)} not found`,
          );
        const data = await response.json();
        setItem(data);
      } catch (error) {
        console.error(error);
      } finally {
        setLoading(false);
      }
    };

    fetchService();
  }, [id]);

  const toggleUpdate = async (e) => {
    setIsUpdating(!isUpdating);
  };

  const handleDelete = async () => {
    if (!window.confirm("Are you sure?")) return;

    try {
      const res = await fetch(`${baseUrl}/api/${itemType}/${id}`, {
        method: "DELETE",
      });
      if (res.ok) {
        alert("Deleted!");
        // navigate back or refresh parent
      } else if (res.status === 400) {
        alert("Procedure can not be deleted with payments attached.");
        return;
      } else {
        alert("Delete failed.");
      }
    } catch (err) {
      console.error(err);
      alert("Error deleting.");
    }
  };

  if (loading) return <p>Loading...</p>;
  if (!item)
    return (
      <p className="m-4 text-center text-red-500">
        {itemType.charAt(0).toUpperCase() + itemType.slice(1)} not found.
      </p>
    );

  return (
    <div className="m-2 flex flex-col gap-2 items-center  max-w-6xl mx-auto bg-gray-800 text-white border border-gray-600 rounded-lg p-4 shadow-lg">
      <h2 className="text-2xl font-bold mb-2">
        {itemType.charAt(0).toUpperCase() + itemType.slice(1)} Details
      </h2>

      <div className="grid lg:grid-cols-3 md:grid-cols-2 sm:grid-cols-2 gap-2 bg-gray-700 rounded-xl p-2 w-full">
        {itemType === "procedure" ? (
          <>
            <p>
              <strong>ID:</strong> {item.id}
            </p>
            <p>
              <strong>Patient ID:</strong> {item.patientId}
            </p>
            <p>
              <strong>Procedure Name:</strong> {item.procedureName}
            </p>
            <p>
              <strong>Date:</strong> {item.procedureDate}
            </p>
            <p>
              <strong>CPT Code:</strong> {item.cptCode}
            </p>
            <p>
              <strong>CPT Amount:</strong> ${item.cptAmount}
            </p>
            <p>
              <strong>Charge:</strong> ${item.patientChargedAmount}
            </p>
          </>
        ) : (
          <>
            <p>
              <strong>ID:</strong> {item.id}
            </p>
            <p>
              <strong>Patient ID:</strong> {item.patientId}
            </p>
            <p>
              <strong>Procedure ID:</strong> {item.procedureId}
            </p>
            <p>
              <strong>Date:</strong> {item.paymentDate}
            </p>
            <p>
              <strong>Method:</strong> {item.method}
            </p>
            <p>
              <strong>Charge:</strong> ${item.amount}
            </p>
          </>
        )}
      </div>
      <div className="mt-4 flex gap-4 justify-center">
        <button
          onClick={toggleUpdate}
          className="bg-yellow-600 px-4 py-2 rounded"
        >
          Edit
        </button>
        <button
          onClick={handleDelete}
          className="bg-red-600 px-4 py-2 text-white rounded"
        >
          Delete
        </button>
      </div>
      {isUpdating && itemType === "procedure" ? (
        <div className="flex flex-col items-center gap-4 bg-gray-700 rounded-xl  p-2 mt-4">
          <UpdateProcedure patientId={item.patientId} Id={item.id} />
        </div>
      ) : null}
      {isUpdating && itemType === "payments" ? (
        <div className="flex flex-col items-center gap-4 bg-gray-700 rounded-xl  p-2 mt-4">
          <UpdatePayment patientId={item.patientId} Id={item.id} />
        </div>
      ) : null}
    </div>
  );
}
