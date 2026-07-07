import React, { useEffect, useState } from 'react';
import { useParams } from 'react-router-dom';
import UpdateProcedure from '../components/UpdateProcedure';

export default function DisplayListDetails() {
  const { id } = useParams();
  const [item, setItem] = useState(null);
  const [loading, setLoading] = useState(true);
  const [isUpdating, setIsUpdating] = useState(false);
  const baseUrl = import.meta.env.VITE_API_BASE_URL;

  useEffect(() => {
    const fetchService = async () => {
      try {
        const response = await fetch(`${baseUrl}/api/procedure/${id}`);
        if (!response.ok) throw new Error("Procedure not found");
        const data = await response.json();
        setItem(data);
      } catch (error) {
        console.error(error);
        alert("Failed to load procedure.");
      } finally {
        setLoading(false);
      }
    };

    fetchService();
  }, [id]);

  const toggleUpdate = async (e) => {
    setIsUpdating(!isUpdating); 
  }

  const handleDelete = async () => {
    if (!window.confirm("Are you sure?")) return;

    try {
      const res = await fetch(`${baseUrl}/api/procedure/${id}`, {
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
  if (!item) return <p>Procedure not found</p>;

  return (
    <div className="w-3xl max-w-6xl mx-auto bg-gray-800 text-white border border-gray-600 rounded-lg p-4 shadow-lg">
    
    <div className="flex flex-col items-center gap-4 bg-gray-700 rounded-xl p-2 mt-4">
      <h2 className="text-2xl font-bold mb-2">Procedure Details</h2>
      <p><strong>Procedure:</strong> {item.procedureName}</p>
      <p><strong>Date:</strong> {item.procedureDate}</p>
      <p><strong>CPT Code:</strong> {item.cptCode}</p>
      <p><strong>Charge:</strong> ${item.patientChargedAmount}</p>

      <div className="mt-4 flex gap-4 justify-center">
        <button onClick={toggleUpdate} className="bg-yellow-600 px-4 py-2 rounded">
          Edit
        </button>
        <button onClick={handleDelete} className="bg-red-600 px-4 py-2 text-white rounded">
          Delete
        </button>
      </div>
   
    </div>
    {isUpdating? 
        <div className="flex flex-col items-center gap-4 bg-gray-700 rounded-xl  p-2 mt-4">
      <UpdateProcedure patientId={item.patientId} id={id} />
    </div> : 
      null}
    </div>
  );
}
