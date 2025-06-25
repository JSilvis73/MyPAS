import { useEffect, useState } from "react";
import { Navigate, useParams } from "react-router-dom";
import DisplayList from "../components/DisplayList";
import AddService from "../components/AddService";
import AddPayment from "../components/AddPayment";
import UpdatePatientForm from "../components/UpdatePatientForm";

export default function PatientDetailsPage() {
  // Page State
  const { id } = useParams();
  const [loading, setLoading] = useState(true);

  // Object States
  const [patient, setPatient] = useState(null);
  const [services, setServices] = useState([]);
  const [payments, setPayments] = useState([]);

  // Form Displays
  const [toggleServiceForm, setToggleServiceForm] = useState(false);
  const [togglePaymentForm, setTogglePaymentForm] = useState(false);
  const [toggleUpdatePatientForm, setToggleUpdatePatientForm] = useState(false);
  const [showServices, setShowServices] = useState(false);
  const [showpayments, setShowPayments] = useState(false);
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
    if (!id) return;

    fetch(`${baseUrl}/api/services/patient/${id}`)
      .then((res) => {
        if (!res.ok) throw new Error("Services not found.");
        return res.json();
      })
      .then((data) => {
        // Optional: sort services by date (most recent first)
        const sorted = [...data].sort(
          (a, b) => new Date(b.serviceDate) - new Date(a.serviceDate)
        );
        setServices(sorted);
      })
      .catch((err) => {
        console.error(err);
      });
  }, [id]);

  useEffect(() => {
    fetch(`${baseUrl}/api/payments/patients/${id}/payments`)
      .then((res) => {
        if (!res.ok) throw new Error("Payments not found.");
        return res.json();
      })
      .then((data) => {
        const sortedPayments = [...data].sort(
          (a, b) => new Date(b.paymentDate) - new Date(a.paymentDate)
        );
        setPayments(sortedPayments);
      })
      .catch((err) => console.error(err));
  }, []);

  // Delete patient section.
  const deleteSelectedPatient = async (id) => {
    const confirmDelete = window.confirm(
      "Are you sure you want to delete this patient?"
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

  const handleTogglPaymentForm = () => {
    setTogglePaymentForm((prev) => !prev);
  };

  const handleToggleUpdatePatientForm = () => {
    setToggleUpdatePatientForm((prev) => !prev);
  };

  const handleToggleShowServices = () => {
    setShowServices((prev) => !prev);
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
    <div className="w-3xl max-w-6xl mx-auto bg-gray-800 text-white border border-gray-600 rounded-lg p-4 shadow-lg">
      <h1 className="text-center text-2xl font-semibold mb-4">
        Patient Details
      </h1>
      <div className="flex flex-col items-center gap-4 bg-gray-700 rounded-xl  p-2">
        <p>
          <strong>ID: </strong> {patient.id}
        </p>
        <div className="flex gap-4">
          <p>
            <strong>Name: </strong> {patient.lastName}, {patient.firstName}
          </p>
        </div>
        <div>
          <p>
            <strong>Age: </strong> {patient.age}
          </p>
        </div>
        <div className="flex gap-4">
          <p>
            <strong>Address: </strong>
            {patient.address}
          </p>
        </div>
        <div className="flex gap-4">
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
        <div className="flex gap-4">
          <button
            type="button"
            className="hover:bg-black hover:text-white p-2 border border-white rounded-xl"
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
      <h2 className="mt-2 text-center p-2">
        <strong>Services</strong>
      </h2>

      <div className=" mt-2 flex flex-col gap-2 bg-gray-700 rounded-xl p-2">
        {showServices ? (
          <div className="text-center">Services Hidden</div>
        ) : (
          <DisplayList items={services} type="service" patientId={patient.id} />
        )}

        <div className="flex gap-4 justify-center">
          <button
            type="button"
            className="hover:bg-black hover:text-white p-2 border border-white rounded-xl"
            onClick={handleToggleShowServices}
          >
            Show/Hide Services
          </button>

          <button
            type="button"
            className="hover:bg-black hover:text-white p-2 border border-white rounded-xl"
            onClick={handleToggleServicesForm}
          >
            Add Service
          </button>
        </div>
        {toggleServiceForm ? <AddService patientId={Number(id)} /> : ""}
      </div>
      <h2 className="mt-2 text-center p-2">
        <strong>Payments</strong>
      </h2>
      <div className=" mt-2 flex flex-col gap-2 bg-gray-700 rounded-xl p-2">
        {showpayments ? (
          <div className="text-center">Payments Hidden</div>
        ) : (
          <DisplayList items={payments} type="payment" />
        )}
        <div className="flex justify-center gap-4">
          <button
            type="button"
            className="hover:bg-black hover:text-white p-2 border border-white rounded-xl"
            onClick={handleToggleShowPayments}
          >
            Show/Hide Payments
          </button>
          <button
            type="button"
            className="hover:bg-black hover:text-white p-2 border border-white rounded-xl"
            onClick={handleTogglPaymentForm}
          >
            Add Payment
          </button>
        </div>
        {togglePaymentForm ? <AddPayment patientId={Number(id)} /> : ""}
      </div>
    </div>
  );
}
