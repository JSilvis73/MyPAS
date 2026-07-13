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
  const handleClearSearchOptions = () => {
    setSearchOptions({ patientId: "", lastName: "", firstName: "" });
    setFilteredPatients(patients);
  };

  const handleClear = () => {
    setFilteredPatients([]);
  }


  // Display
  return (
    <div className=" m-2  text-center">
      <div className="max-w-6xl mx-auto bg-gray-800 text-white border border-gray-600 rounded-lg p-6 shadow-lg  ">
        <div className="m-2">
          <h1 className="text-2xl font-bold mb-4">
            <strong>Search</strong>
          </h1>

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
                    <button
          className="p-1 mt-4 border rounded-lg hover:bg-red-500 hover:text-black"
          onClick={handleClearSearchOptions}
        >
          Clear
        </button>
          </form>
        </div>
      </div>

      <div className="max-w-6xl flex flex-col items-center mx-auto bg-gray-800 text-white border border-gray-600 rounded-lg p-6 shadow-lg m-4">
        <h1 className="text-2xl font-bold mb-4">Search Results:</h1>
        
        <div className="m-2 flex flex-col gap-2 items-center justify-center">
        <p className="m-2 font-bold ">
          Found {filteredPatients.length} match
          {filteredPatients.length === 1 ? "" : "es"}
        </p>
        <button type="button" className="p-1 border rounded-lg hover:text-black hover:bg-red-500"
          onClick={handleClear}>
              Clear Results
        </button>
       </div>

       {/* Display the filtered patients in a grid layout with links to their individual pages.*/}
        <div className="m-2 grid md:grid-cols-2 lg:grid-cols-3 gap-2">
        {filteredPatients.map((patient) => (
          <Link key={patient.id} to={`/patients/${patient.id}`}>
            <div className="p-2 rounded-xl  bg-white/10 border border-white/30 backdrop-blur-lg text-white font-semibold shadow-[0_8px_32px_0_rgba(31,38,135,0.37)] hover:bg-white/20 hover:scale-105 transition-all duration-300 ease-in-out">
              Id: {patient.id}: {patient.lastName}, {patient.firstName}
            </div>
          </Link>
        ))}
         </div>
      </div>

    </div>

  );
}
