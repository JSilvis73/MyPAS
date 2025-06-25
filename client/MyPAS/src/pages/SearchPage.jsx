import React, { useEffect, useState } from "react";
import FormInput from "../components/FormInput";
import { FaSearch } from "react-icons/fa";
import { Link } from "react-router-dom";

export default function SearchPage() {
  const [patients, setPatients] = useState([]);
  const [filteredPatients, setFilteredPatients] = useState([]);
  const [searchOptions, setSearchOptions] = useState({
    patientId: "",
    lastName: "",
    firstName: "",
  });
  const baseUrl = import.meta.env.VITE_API_BASE_URL;

  // Get List of patients initially and refresh during search.
  useEffect(() => {
    fetch(`${baseUrl}/api/patients`)
      .then((res) => res.json())
      .then((data) => {
        setPatients(data);
        setFilteredPatients(data);
      })
      .catch((err) => console.error("Error fetching data:", err));
  }, []);

  const handleFormInputChange = (e) => {
    const { name, value } = e.target;
    setSearchOptions((prev) => ({ ...prev, [name]: value }));
  };

  // Find filtered patients based on search.
  const handleFormSubmit = (e) => {
    e.preventDefault();

    const filtered = patients.filter((p) => {
      return (
        p.lastName
          .toLowerCase()
          .includes(searchOptions.lastName.toLowerCase()) &&
        p.firstName
          .toLowerCase()
          .includes(searchOptions.firstName.toLowerCase()) &&
        p.id.toString().includes(searchOptions.patientId)
      );
    });

    setFilteredPatients(filtered);
  };

  // Reset form data.
  const handleClear = () => {
    setSearchOptions({ patientId: "", lastName: "", firstName: "" });
    setFilteredPatients(patients);
  };

  // Display
  return (
    <div>
      <div className="size-lg bg-gray-800 text-white text-center border-4 rounded-lg p-4">
        <div className="m-2">
          <h2 className="text-2xl mb-4">
            <strong>Search</strong>
          </h2>

          <form
            className="flex flex-wrap gap-2 items-center justify-center"
            onSubmit={handleFormSubmit}
          >
            <FormInput
              props={{
                inputName: "Patient Id:",
                type: "number",
                name: "patientId",
                value: searchOptions.patientId,
                placeholder: "0",
                onChange: handleFormInputChange,
              }}
            />
            <FormInput
              props={{
                inputName: "Last Name:",
                type: "text",
                name: "lastName",
                value: searchOptions.lastName,
                placeholder: "Doe",
                onChange: handleFormInputChange,
              }}
            />
            <FormInput
              props={{
                inputName: "First Name:",
                type: "text",
                name: "firstName",
                value: searchOptions.firstName,
                placeholder: "John",
                onChange: handleFormInputChange,
              }}
            />

            <button
              className="p-1 mt-4 border rounded-lg hover:bg-white hover:text-black"
              type="submit"
            >
              <FaSearch className="inline mr-1" />
              Search
            </button>
          </form>
        </div>
      </div>

      <div className="flex flex-col items-center bg-gray-800 rounded-md text-white m-1 p-2">
        <h1 className="mt-2 text-lg">Search Results:</h1>
        <p className="text-sm">
          Found {filteredPatients.length} match
          {filteredPatients.length === 1 ? "" : "es"}
        </p>
        <button
          className="mt-1 border p-1 rounded-md hover:bg-white hover:text-black"
          onClick={handleClear}
        >
          Clear
        </button>

        {filteredPatients.map((patient) => (
          <Link key={patient.id} to={`/patients/${patient.id}`}>
            <div className="px-6 py-3 rounded-xl m-2 bg-white/10 border border-white/30 backdrop-blur-lg text-white font-semibold shadow-[0_8px_32px_0_rgba(31,38,135,0.37)] hover:bg-white/20 hover:scale-105 transition-all duration-300 ease-in-out">
              {patient.id}: {patient.lastName}, {patient.firstName}
            </div>
          </Link>
        ))}
      </div>
    </div>
  );
}
