import React from "react";
import { useState, useEffect } from "react";

export default function AdminUserSearch() {
  const [searchUserForm, setSearchUserForm] = useState({
    email: "",
    userName: "",
  });

  const [userList, setUserList] = useState([]);
  const [filteredUsers, setFilteredUsers] = useState([]);

  const handleFormInputChange = (e) => {
    const { name, value } = e.target;
    setSearchUserForm((prev) => ({
      ...prev,
      [name]: value,
    }));
  };

  useEffect(() => {
    const getAllUsers = async () => {
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
          setFilteredUsers(data);
        })
        .catch((err) => console.error("Error fetching data:", err));
    };
    getAllUsers();
  }, []);

  const handleSubmit = (e) => {
    e.preventDefault();
    const filteredUsers = userList.filter((user) =>
      user.email.toLowerCase().includes(searchUserForm.email.toLowerCase()) ||
      user.userName.toLowerCase().includes(searchUserForm.userName.toLowerCase())
    );
    setFilteredUsers(filteredUsers);
  };

  const resetSearch = () => {
    setSearchUserForm({ email: "", userName: "" });
    setFilteredUsers(userList);
  };

  return (
    <div className="text-center m-2">
      <h1 className="text-xl font-bold mb-4">Admin User Search</h1>
      <form className="flex flex-col gap-2 " onSubmit={handleSubmit}>
        <label htmlFor="email">Email</label>
        <input
          className="bg-gray-600 p-2 rounded-lg text-white"
          id="email"
          type="text"
          name="email"
          value={searchUserForm.email || ""}
          onChange={handleFormInputChange}
        />
        <label htmlFor="userName">UserName</label>
        <input
          className="bg-gray-600 p-2 rounded-lg text-white"
          id="userName"
          type="text"
          name="userName"
          value={searchUserForm.userName || ""}
          onChange={handleFormInputChange}
        />
        <button
          className="bg-blue-500 p-2 rounded-lg hover:bg-blue-600"
          type="submit"
        >
          Search
        </button>
        <button
          className="bg-gray-500 p-2 rounded-lg hover:bg-gray-600"
          type="button"
          onClick={resetSearch}
        >
          Reset
        </button>
      </form>

      <div>
        {filteredUsers.length > 0 ? (
          <ul className="m-2 gap-2 flex flex-col items-center">
            {filteredUsers.map((user) => (
              <li
                key={user.id}
                className="bg-gray-700 p-2 rounded-lg hover:bg-gray-600 gap-2 flex flex-col items-center"
              >
                <p>{user.email}</p>
                <p>{user.userName!==user.email? user.userName : ""}</p>
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
