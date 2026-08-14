import React from "react";
import { useState, useEffect } from "react";
import { Form } from "react-router";

export default function AdminUserSearch() {
  const [searchUserForm, setSearchUserForm] = useState({
    email: "",
  });

  const [userList, setUserList] = useState([]);

  const handleFormInputChange = (e) => {
    const { name, value } = e.target;
    setSearchUserForm((prev) => ({
      ...prev,
      [name]: value,
    }));
  };

  useEffect(() => {
    fetch(`${import.meta.env.VITE_API_BASE_URL}/api/Auth/getAllUsers`, {
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${localStorage.getItem("token")}`,
      },
    })
      .then((response) => {
        if (!response.ok) {
          throw new Error("Network response was not ok");
        }
        return response.json();
      })
      .then((data) => {
        setUserList(data);
      })
      .catch((err) => console.error("Error fetching data:", err));
  }, []);

  const handleSubmit = (e) => {
    e.preventDefault();
    const filteredUsers = userList.filter((user) =>
      user.email.toLowerCase().includes(searchUserForm.email.toLowerCase()),
    );
    setUserList(filteredUsers);
  };

  return (
    <div className="text-center m-2">
      <h1 className="text-xl font-bold mb-4">Admin User Search</h1>
      <form className="flex flex-col gap-2 "
        onSubmit={handleSubmit}>
        <label htmlFor="email">Search User by Email</label>
        <input
          className="bg-gray-600 p-2 rounded-lg text-white"
          id="email"
          type="text"
          name="email"
          value={searchUserForm.email || ""}
          onChange={handleFormInputChange}
        />
      </form>

      <div>
        {userList.length > 0 ? (
          <ul className="m-2 gap-2 flex flex-col items-center">
            {userList.map((user) => (
              <li
                key={user.id}
                className="bg-gray-700 p-2 rounded-lg hover:bg-gray-600"
              >
                {user.email}
              </li>
            ))}
          </ul>
        ) : (
          <p className="text-gray-400">No users found.</p>
        )}
      </div>
    </div>
  );
}
