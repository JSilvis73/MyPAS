import React, { useEffect, useState } from 'react';
import { useParams } from 'react-router-dom';

export default function DisplayListDetails() {
  const { id } = useParams();
  const [item, setItem] = useState(null);
  const [loading, setLoading] = useState(true);
  const baseUrl = import.meta.env.VITE_API_BASE_URL;

  useEffect(() => {
    const fetchService = async () => {
      try {
        const response = await fetch(`${baseUrl}/api/services/${id}`);
        if (!response.ok) throw new Error("Service not found");
        const data = await response.json();
        setItem(data);
      } catch (error) {
        console.error(error);
        alert("Failed to load service.");
      } finally {
        setLoading(false);
      }
    };

    fetchService();
  }, [id]);

  const handleDelete = async () => {
    if (!window.confirm("Are you sure?")) return;

    try {
      const res = await fetch(`${baseUrl}/api/services/${id}`, {
        method: 'DELETE'
      });
      if (res.ok) {
        alert("Deleted!");
        // navigate back or refresh parent
      } else {
        alert("Delete failed");
      }
    } catch (err) {
      console.error(err);
      alert("Error deleting.");
    }
  };

  if (loading) return <p>Loading...</p>;
  if (!item) return <p>Service not found</p>;

  return (
    <div className="p-4">
      <h2 className="text-xl font-bold mb-2">Service Details</h2>
      <p><strong>Service:</strong> {item.serviceName}</p>
      <p><strong>Date:</strong> {item.serviceDate}</p>
      <p><strong>CPT Code:</strong> {item.cptCode}</p>
      <p><strong>Charge:</strong> ${item.patientChargedAmount}</p>

      <div className="mt-4 flex gap-4">
        <button onClick={() => alert("Edit coming soon")} className="bg-yellow-400 px-4 py-2 rounded">
          Edit
        </button>
        <button onClick={handleDelete} className="bg-red-600 px-4 py-2 text-white rounded">
          Delete
        </button>
      </div>
    </div>
  );
}
