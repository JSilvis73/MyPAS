import { useEffect, useState } from "react";
import { Navigate, useParams } from "react-router-dom";
import DisplayList from "../components/DisplayList";
import AddProcedure from "../components/AddProcedure";
import AddPayment from "../components/AddPayment";
import UpdatePatientForm from "../components/UpdatePatientForm";

export default function PatientDetailsPage({ type }) {
  // Page State
  const { id } = useParams();
  const [loading, setLoading] = useState(true);

  // Object States
  const [patient, setPatient] = useState(null);
  const [procedures, setProcedures] = useState([]);
  const [payments, setPayments] = useState([]);

  // Form Displays
  const [toggleServiceForm, setToggleServiceForm] = useState(false);
  const [togglePaymentForm, setTogglePaymentForm] = useState(false);
  const [toggleUpdatePatientForm, setToggleUpdatePatientForm] = useState(false);
  const [showProcedures, setShowProcedures] = useState(false);
  const [showPayments, setShowPayments] = useState(false);
  const baseUrl = import.meta.env.VITE_API_BASE_URL;

  // On render load patient/services/payments.
  useEffect(() => {
    fetch(`${baseUrl}/api/patients/${id}`)
      .then((res) => {
        if (!res.ok) throw new Error("Patient not found");
        return res.json();
      })
      .then((data) => {
        setPatient(data);
        setLoading(false);
      })
      .catch((err) => {
        console.error(err);
        setLoading(false);
      });
  }, [id]);

  useEffect(() => {
    fetchProcedures();
    fetchPayments();
  }, [id]);

  const fetchProcedures = async () => {
    if (!id) return;

    fetch(`${baseUrl}/api/procedure/patient/${id}`)
      .then((res) => {
        if (!res.ok) throw new Error("Procedures not found.");
        return res.json();
      })
      .then((data) => {
        // Optional: sort procedures by date (most recent first)
        const sorted = [...data].sort(
          (a, b) => new Date(b.procedureDate) - new Date(a.procedureDate),
        );
        setProcedures(sorted);
      })
      .catch((err) => {
        console.error(err);
      });
  };

  const fetchPayments = async () => {
    if (!id) return;
    fetch(`${baseUrl}/api/payments/patients/${id}/payments`)
      .then((res) => {
        if (!res.ok) throw new Error("Payments not found.");
        return res.json();
      })
      .then((data) => {
        const sortedPayments = [...data].sort(
          (a, b) => new Date(b.paymentDate) - new Date(a.paymentDate),
        );
        setPayments(sortedPayments);
      })
      .catch((err) => console.error(err));
  };

  // Delete patient section.
  const deleteSelectedPatient = async (id) => {
    const confirmDelete = window.confirm(
      "Are you sure you want to delete this patient?",
    );
    if (!confirmDelete) return;

    try {
      const response = await fetch(`${baseUrl}/api/patients/${id}`, {
        method: "DELETE",
        headers: {
          "Content-Type": "application/json",
        },
      });

      if (!response.ok) {
        throw new Error("Failed to delete patient");
      }

      alert("Patient deleted successfully.");
      // Optionally refresh list or navigate away here
    } catch (error) {
      console.error("Error deleting patient:", error.message);
      alert("There was a problem deleting the patient.");
    }
  };

  // Handle Input Form Changes.
  const handleToggleServicesForm = () => {
    setToggleServiceForm((prev) => !prev);
  };

  const handleTogglePaymentForm = () => {
    setTogglePaymentForm((prev) => !prev);
  };

  const handleToggleUpdatePatientForm = () => {
    setToggleUpdatePatientForm((prev) => !prev);
  };

  const handleToggleShowProcedures = () => {
    setShowProcedures((prev) => !prev);
  };

  const handleToggleShowPayments = () => {
    setShowPayments((prev) => !prev);
  };

  if (loading) {
    return (
      <div className="flex justify-center items-center h-screen ">
        <div className="animate-spin h-10 w-10 border-4 border-white border-b-transparent rounded-full"></div>
      </div>
    );
  }

  if (!patient) {
    return <div className="p-4 text-red-500">Patient not found.</div>;
  }

  return (
    <div className="w-3xl max-w-6xl mx-auto bg-gray-800 text-white border border-gray-600 rounded-lg m-4 p-4 shadow-lg">
      <h1 className="text-center text-2xl font-semibold mb-4">
        Patient Details
      </h1>
      <div className="flex flex-col flex-wrap items-center gap-4 bg-gray-700 rounded-xl  p-2">
        <p>
          <strong>ID: </strong>
          {patient.id}
        </p>
        <p>
          <strong>Name: </strong>
          {patient.lastName}, {patient.firstName}
        </p>
        <p>
          <strong>Age: </strong>
          {patient.age}
        </p>
        <div id="address-section" className="flex flex-wrap gap-4">
          <p>
            <strong>Address: </strong>
            {patient.address}
          </p>
          <p>
            <strong>City: </strong>
            {patient.city}
          </p>
          <p>
            <strong>State: </strong>
            {patient.state}
          </p>
          <p>
            <strong>Zip: </strong>
            {patient.zip}
          </p>
        </div>
        <div id="contact-section" className="flex flex-wrap gap-4">
          <p>
            <strong>Phone: </strong>
            {patient.phone}
          </p>
          <p>
            <strong>Email: </strong>
            {patient.email}
          </p>
        </div>

        <div className="flex gap-4">
          <button
            type="button"
            className="hover:bg-black hover:text-white p-2 border border-white rounded-xl hover:bg-green-500"
            onClick={handleToggleUpdatePatientForm}
          >
            Update Patient
          </button>
          <button
            type="button"
            className="hover:bg-red-500 hover:text-white p-2 border border-white rounded-xl"
            onClick={() => deleteSelectedPatient(patient.id)}
          >
            Delete Patient
          </button>
        </div>
        {toggleUpdatePatientForm ? <UpdatePatientForm patient={patient} /> : ""}
      </div>

      {/* Procedures and Payments Section */}
      <h2 className="mt-2 text-center p-2">
        <strong>Procedures</strong>
      </h2>

      <div className=" mt-2 flex flex-col gap-2 bg-gray-700 rounded-xl p-2">
        {showProcedures ? (
          <div className="text-center">Procedures Hidden</div>
        ) : (
          <DisplayList
            items={procedures}
            type="procedure"
            patientId={patient.id}
          />
        )}

        <div className="flex gap-4 justify-center">
          <button
            type="button"
            className="hover:bg-black hover:text-white p-2 border border-white rounded-xl hover:bg-green-500"
            onClick={handleToggleShowProcedures}
          >
            Show/Hide Procedures
          </button>

          <button
            type="button"
            className="hover:bg-black hover:text-white p-2 border border-white rounded-xl hover:bg-blue-500"
            onClick={handleToggleServicesForm}
          >
            Add Procedure
          </button>
        </div>
        {toggleServiceForm ? <AddProcedure patientId={Number(id)} onProcedureAdded={fetchProcedures} /> : ""}
      </div>
      <h2 className="mt-2 text-center p-2">
        <strong>Payments</strong>
      </h2>
      <div className=" mt-2 flex flex-col gap-2 bg-gray-700 rounded-xl p-2">
        {showPayments ? (
          <div className="text-center">Payments Hidden</div>
        ) : (
          <DisplayList items={payments} type="payment" />
        )}
        <div className="flex justify-center gap-4">
          <button
            type="button"
            className="hover:bg-black hover:text-white p-2 border border-white rounded-xl hover:bg-green-500"
            onClick={handleToggleShowPayments}
          >
            Show/Hide Payments
          </button>
          <button
            type="button"
            className="hover:bg-black hover:text-white p-2 border border-white rounded-xl hover:bg-blue-500"
            onClick={handleTogglePaymentForm}
          >
            Add Payment
          </button>
        </div>
        {togglePaymentForm ? <AddPayment patientId={Number(id)} onPaymentAdded={fetchPayments} /> : ""}
      </div>
    </div>
  );
}
