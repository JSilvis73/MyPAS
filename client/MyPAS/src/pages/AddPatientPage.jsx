import { useState } from "react";
import FormInput from "../components/FormInput";

export default function AddPatientPage() {
  const [isSubmitting, setIsSubmitting] = useState(false);
  const [msg, setMsg] = useState({});

  const [newPatient, setNewPatient] = useState({
    firstName: "",
    lastName: "",
    address: "",
    city: "",
    state: "",
    zip: "",
    age: "",
    phone: "",
    email: "",
  });

  const baseUrl = import.meta.env.VITE_API_BASE_URL;

  // Handle input changes
  const handleFormInputChange = (e) => {
    const { name, value } = e.target;

    setNewPatient((prev) => ({
      ...prev,
      [name]: value,
    }));

    setMsg({});
  };

  // Handle form submission
  const handleSubmit = async (e) => {
    e.preventDefault();

    if (!newPatient.firstName) {
      setMsg({ firstName: "First name is required." });
      return;
    }

    if (!newPatient.lastName) {
      setMsg({ lastName: "Last name is required." });
      return;
    }

    if (!newPatient.age || Number(newPatient.age) < 1) {
      setMsg({ age: "Age must be at least 1." });
      return;
    }

    if (
      newPatient.email.trim() === "" &&
      newPatient.phone.trim() === ""
    ) {
      setMsg({ contact: "Either email or phone is required." });
      return;
    }

    if (isSubmitting) return;

    setIsSubmitting(true);

    try {
      const response = await fetch(`${baseUrl}/api/patients`, {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify(newPatient),
      });

      if (!response.ok) {
        throw new Error("Failed to add patient.");
      }

      const result = await response.json();

      console.log("Patient added:", result);

      setMsg({
        success: "Patient added successfully.",
      });

      setNewPatient({
        firstName: "",
        lastName: "",
        address: "",
        city: "",
        state: "",
        zip: "",
        age: "",
        phone: "",
        email: "",
      });
    } catch (error) {
      console.error("Error adding patient:", error);

      setMsg({
        error: "There was a problem adding the patient.",
      });
    } finally {
      setIsSubmitting(false);
    }
  };

  return (
    <div className="flex flex-col items-center gap-2 text-center">
      <h1 className="text-2xl mb-4 text-blue-500">
        <strong>Create New Patient</strong>
      </h1>

      {/* Patient Card */}
      <div className="mx-auto w-full max-w-5xl rounded-lg border border-gray-600 bg-gray-800 p-4 text-white shadow-lg">
        <form onSubmit={handleSubmit}>
          {/* Patient Information */}
          <div className="overflow-hidden rounded-xl border border-gray-600 bg-gray-700">
            <h3 className="border-b border-gray-600 p-3 text-lg font-semibold text-blue-500">
              Patient Information
            </h3>

            <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4">
              <div className="border-b border-gray-600 p-3 lg:border-r">
                <FormInput
                  props={{
                    inputName: "Last Name:",
                    type: "text",
                    name: "lastName",
                    value: newPatient.lastName,
                    placeholder: "Doe",
                    onChange: handleFormInputChange,
                  }}
                />
              </div>

              <div className="border-b border-gray-600 p-3 lg:border-r">
                <FormInput
                  props={{
                    inputName: "First Name:",
                    type: "text",
                    name: "firstName",
                    value: newPatient.firstName,
                    placeholder: "John",
                    onChange: handleFormInputChange,
                  }}
                />
              </div>

              <div className="border-b border-gray-600 p-3 lg:border-r">
                <FormInput
                  props={{
                    inputName: "Age:",
                    type: "number",
                    name: "age",
                    value: newPatient.age,
                    placeholder: "18",
                    onChange: handleFormInputChange,
                  }}
                />
              </div>

              <div className="border-b border-gray-600 p-3">
                <FormInput
                  props={{
                    inputName: "Phone:",
                    type: "text",
                    name: "phone",
                    value: newPatient.phone,
                    placeholder: "330-987-1234",
                    onChange: handleFormInputChange,
                  }}
                />
              </div>
            </div>

            {/* Address */}
            <h3 className="border-b border-t border-gray-600 p-3 text-lg font-semibold text-blue-500">
              Address Information
            </h3>

            <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4">
              <div className="border-b border-gray-600 p-3 lg:col-span-2 lg:border-r">
                <FormInput
                  props={{
                    inputName: "Address:",
                    type: "text",
                    name: "address",
                    value: newPatient.address,
                    placeholder: "1234 Cherry St.",
                    onChange: handleFormInputChange,
                  }}
                />
              </div>

              <div className="border-b border-gray-600 p-3 lg:border-r">
                <FormInput
                  props={{
                    inputName: "City:",
                    type: "text",
                    name: "city",
                    value: newPatient.city,
                    placeholder: "Kent",
                    onChange: handleFormInputChange,
                  }}
                />
              </div>

              <div className="border-b border-gray-600 p-3">
                <FormInput
                  props={{
                    inputName: "State:",
                    type: "text",
                    name: "state",
                    value: newPatient.state,
                    placeholder: "OH",
                    onChange: handleFormInputChange,
                  }}
                />
              </div>

              <div className="p-3 lg:col-span-4">
                <FormInput
                  props={{
                    inputName: "Zip:",
                    type: "text",
                    name: "zip",
                    value: newPatient.zip,
                    placeholder: "44230",
                    onChange: handleFormInputChange,
                  }}
                />
              </div>
            </div>

            {/* Contact */}
            <h3 className="border-b border-t border-gray-600 p-3 text-lg font-semibold text-blue-500">
              Contact Information
            </h3>

            <div className="grid grid-cols-1 md:grid-cols-2">
              <div className="border-b border-gray-600 p-3 md:border-r">
                <FormInput
                  props={{
                    inputName: "Phone:",
                    type: "text",
                    name: "phone",
                    value: newPatient.phone,
                    placeholder: "330-987-1234",
                    onChange: handleFormInputChange,
                  }}
                />
              </div>

              <div className="border-b border-gray-600 p-3">
                <FormInput
                  props={{
                    inputName: "Email:",
                    type: "email",
                    name: "email",
                    value: newPatient.email,
                    placeholder: "JDoe10@gmail.com",
                    onChange: handleFormInputChange,
                  }}
                />
              </div>
            </div>
          </div>

          {/* Messages */}
          <div className="mt-4 text-center">
            {msg.firstName && (
              <p className="text-red-500">{msg.firstName}</p>
            )}

            {msg.lastName && (
              <p className="text-red-500">{msg.lastName}</p>
            )}

            {msg.age && (
              <p className="text-red-500">{msg.age}</p>
            )}

            {msg.contact && (
              <p className="text-red-500">{msg.contact}</p>
            )}

            {msg.error && (
              <p className="text-red-500">{msg.error}</p>
            )}

            {msg.success && (
              <p className="text-green-500">{msg.success}</p>
            )}
          </div>

          {/* Submit */}
          <div className="mt-4 flex justify-center">
            <button
              type="submit"
              disabled={isSubmitting}
              className="rounded-xl border border-white p-2 transition-colors hover:bg-blue-500 disabled:cursor-not-allowed disabled:opacity-50"
            >
              {isSubmitting ? "Submitting..." : "Create Patient"}
            </button>
          </div>
        </form>
      </div>
    </div>
  );
}