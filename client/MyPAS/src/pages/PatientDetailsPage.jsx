import { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import DisplayList from "../components/DisplayList";
import AddProcedure from "../components/AddProcedure";
import AddPayment from "../components/AddPayment";
import UpdatePatientForm from "../components/UpdatePatientForm";

export default function PatientDetailsPage() {
  const { id } = useParams();
  const baseUrl = import.meta.env.VITE_API_BASE_URL;

  // Page state
  const [loading, setLoading] = useState(true);

  // Data state
  const [patient, setPatient] = useState(null);
  const [procedures, setProcedures] = useState([]);
  const [payments, setPayments] = useState([]);

  // Form/display state
  const [toggleServiceForm, setToggleServiceForm] = useState(false);
  const [togglePaymentForm, setTogglePaymentForm] = useState(false);
  const [toggleUpdatePatientForm, setToggleUpdatePatientForm] =
    useState(false);
  const [showProcedures, setShowProcedures] = useState(false);
  const [showPayments, setShowPayments] = useState(false);

  // -----------------------------
  // Fetch patient
  // -----------------------------
  useEffect(() => {
    const fetchPatient = async () => {
      try {
        const res = await fetch(`${baseUrl}/api/patients/${id}`);

        if (!res.ok) {
          throw new Error("Patient not found.");
        }

        const data = await res.json();
        setPatient(data);
      } catch (err) {
        console.error(err);
      } finally {
        setLoading(false);
      }
    };

    fetchPatient();
  }, [id, baseUrl]);

  // -----------------------------
  // Fetch procedures/payments
  // -----------------------------
  useEffect(() => {
    fetchProcedures();
    fetchPayments();
  }, [id]);

  const fetchProcedures = async () => {
    if (!id) return;

    try {
      const res = await fetch(`${baseUrl}/api/procedure/patient/${id}`);

      if (!res.ok) {
        throw new Error("Procedures not found.");
      }

      const data = await res.json();

      const sorted = [...data].sort(
        (a, b) =>
          new Date(b.procedureDate) - new Date(a.procedureDate),
      );

      setProcedures(sorted);
    } catch (err) {
      console.error(err);
    }
  };

  const fetchPayments = async () => {
    if (!id) return;

    try {
      const res = await fetch(
        `${baseUrl}/api/payments/patients/${id}/payments`,
      );

      if (!res.ok) {
        throw new Error("Payments not found.");
      }

      const data = await res.json();

      const sorted = [...data].sort(
        (a, b) =>
          new Date(b.paymentDate) - new Date(a.paymentDate),
      );

      setPayments(sorted);
    } catch (err) {
      console.error(err);
    }
  };

  // -----------------------------
  // Delete patient
  // -----------------------------
  const deleteSelectedPatient = async (patientId) => {
    const confirmDelete = window.confirm(
      "Are you sure you want to delete this patient?",
    );

    if (!confirmDelete) return;

    try {
      const response = await fetch(
        `${baseUrl}/api/patients/${patientId}`,
        {
          method: "DELETE",
        },
      );

      if (!response.ok) {
        throw new Error("Failed to delete patient.");
      }

      alert("Patient deleted successfully.");
    } catch (error) {
      console.error("Error deleting patient:", error);
      alert("There was a problem deleting the patient.");
    }
  };

  // -----------------------------
  // Toggle handlers
  // -----------------------------
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

  // -----------------------------
  // Loading
  // -----------------------------
  if (loading) {
    return (
      <div className="flex h-screen items-center justify-center">
        <div className="h-10 w-10 animate-spin rounded-full border-4 border-white border-b-transparent" />
      </div>
    );
  }

  if (!patient) {
    return (
      <div className="p-4 text-center text-red-500">
        Patient not found.
      </div>
    );
  }

  return (
    <div className="flex flex-col items-center gap-2 text-center">
      <h1 className="m-2 text-2xl font-semibold text-blue-500">
        Patient Details
      </h1>

      {/* Patient Card */}
      <div className="mx-auto w-full max-w-5xl rounded-lg border border-gray-600 bg-gray-800 p-4 text-white shadow-lg">
        
        {/* Patient Information */}
        <div className="overflow-hidden rounded-xl border border-gray-600 bg-gray-700">
          
          <h3 className="border-b border-gray-600 p-3 text-lg font-semibold text-blue-500">
            Patient Information
          </h3>

          <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4">
            <div className="border-b border-gray-600 p-3 lg:border-r">
              <strong className="block text-blue-300">ID:</strong>
              <span>{patient.id}</span>
            </div>

            <div className="border-b border-gray-600 p-3 lg:border-r">
              <strong className="block text-blue-300">Last Name:</strong>
              <span>{patient.lastName}</span>
            </div>

            <div className="border-b border-gray-600 p-3 lg:border-r">
              <strong className="block text-blue-300">First Name:</strong>
              <span>{patient.firstName}</span>
            </div>

            <div className="border-b border-gray-600 p-3">
              <strong className="block text-blue-300">Age:</strong>
              <span>{patient.age}</span>
            </div>
          </div>

          {/* Address */}
          <h3 className="border-b border-t border-gray-600 p-3 text-lg font-semibold text-blue-500">
            Address Information
          </h3>

          <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4">
            <div className="border-b border-gray-600 p-3 lg:border-r">
              <strong className="block text-blue-300">Address:</strong>
              <span className="break-words">
                {patient.address || "N/A"}
              </span>
            </div>

            <div className="border-b border-gray-600 p-3 lg:border-r">
              <strong className="block text-blue-300">City:</strong>
              <span>{patient.city || "N/A"}</span>
            </div>

            <div className="border-b border-gray-600 p-3 lg:border-r">
              <strong className="block text-blue-300">State:</strong>
              <span>{patient.state || "N/A"}</span>
            </div>

            <div className="border-b border-gray-600 p-3">
              <strong className="block text-blue-300">Zip:</strong>
              <span>{patient.zip || "N/A"}</span>
            </div>
          </div>

          {/* Contact */}
          <h3 className="border-b border-t border-gray-600 p-3 text-lg font-semibold text-blue-500">
            Contact Information
          </h3>

          <div className="grid grid-cols-1 md:grid-cols-2">
            <div className="border-b border-gray-600 p-3 md:border-r">
              <strong className="block text-blue-300">Phone:</strong>
              <span>{patient.phone || "N/A"}</span>
            </div>

            <div className="border-b border-gray-600 p-3">
              <strong className="block text-blue-300">Email:</strong>
              <span className="break-words">
                {patient.email || "N/A"}
              </span>
            </div>
          </div>
        </div>

        {/* Patient Actions */}
        <div className="my-4 flex flex-wrap justify-center gap-4">
          <button
            type="button"
            className="rounded-xl border border-white p-2 transition-colors hover:bg-green-500 hover:text-white"
            onClick={handleToggleUpdatePatientForm}
          >
            Update Patient
          </button>

          <button
            type="button"
            className="rounded-xl border border-white p-2 transition-colors hover:bg-red-500 hover:text-white"
            onClick={() => deleteSelectedPatient(patient.id)}
          >
            Delete Patient
          </button>
        </div>

        {toggleUpdatePatientForm && (
          <UpdatePatientForm patient={patient} />
        )}

        {/* Procedures */}
        <section className="mt-4">
          <h2 className="p-2 text-center text-xl font-semibold">
            Procedures
          </h2>

          <div className="flex flex-col gap-2 rounded-xl bg-gray-700 p-2">
            {showProcedures ? (
              <div className="p-2 text-center text-gray-300">
                Procedures Hidden
              </div>
            ) : (
              <DisplayList
                items={procedures}
                type="procedure"
                patientId={patient.id}
              />
            )}

            <div className="flex flex-wrap justify-center gap-4">
              <button
                type="button"
                className="rounded-xl border border-white p-2 transition-colors hover:bg-green-500"
                onClick={handleToggleShowProcedures}
              >
                Show/Hide Procedures
              </button>

              <button
                type="button"
                className="rounded-xl border border-white p-2 transition-colors hover:bg-blue-500"
                onClick={handleToggleServicesForm}
              >
                Add Procedure
              </button>
            </div>

            {toggleServiceForm && (
              <AddProcedure
                patientId={Number(id)}
                onProcedureAdded={fetchProcedures}
              />
            )}
          </div>
        </section>

        {/* Payments */}
        <section className="mt-4">
          <h2 className="p-2 text-center text-xl font-semibold">
            Payments
          </h2>

          <div className="flex flex-col gap-2 rounded-xl bg-gray-700 p-2">
            {showPayments ? (
              <div className="p-2 text-center text-gray-300">
                Payments Hidden
              </div>
            ) : (
              <DisplayList
                items={payments}
                type="payment"
              />
            )}

            <div className="flex flex-wrap justify-center gap-4">
              <button
                type="button"
                className="rounded-xl border border-white p-2 transition-colors hover:bg-green-500"
                onClick={handleToggleShowPayments}
              >
                Show/Hide Payments
              </button>

              <button
                type="button"
                className="rounded-xl border border-white p-2 transition-colors hover:bg-blue-500"
                onClick={handleTogglePaymentForm}
              >
                Add Payment
              </button>
            </div>

            {togglePaymentForm && (
              <AddPayment
                patientId={Number(id)}
                onPaymentAdded={fetchPayments}
              />
            )}
          </div>
        </section>
      </div>
    </div>
  );
}