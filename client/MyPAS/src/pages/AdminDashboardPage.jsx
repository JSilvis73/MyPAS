import React from "react";
import AdminDashboard from "../components/AdminDashboard";
import AdminUserSearch from "../components/AdminUserSearch";

export default function AdminDashboardPage() {
  return (
    <div className="flex flex-col  items-center gap-2 mx-auto bg-gray-800 text-white border border-gray-600 rounded-lg m-4 p-4 shadow-lg">
      <h1><strong>Admin Dashboard</strong></h1>
      <AdminDashboard />

      <AdminUserSearch />
    </div>
  );
}
