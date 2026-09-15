import React from "react";

export default function OperationsPage() {
  return (
    <div>
      <div className="max-w-xl mx-auto bg-gray-800 text-white border border-gray-600 rounded-lg p-6 shadow-lg text-center">
        <h2 className="text-2xl  mb-4"><strong>Operations</strong></h2>
        <div className="flex items-center justify-center space-x-4">
          <button className="border p-2 rounded-xl hover:bg-white hover:text-black">Import Patients List</button>
        <button className="border p-2 rounded-xl hover:bg-white hover:text-black">Export Patients List</button>
        </div>
      </div>
    </div>
  );
}
