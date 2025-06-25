import React from "react";

export default function OperationsPage() {
  return (
    <div>
      <div className="size-lg bg-gray-800 text-white text-center border-4 rounded-lg p-4">
        <h2 className="text-2xl mb-4"><strong>Operations</strong></h2>
        <div className="flex gap-4">
          <button className="border p-2 rounded-xl hover:bg-white hover:text-black">Import Patients List</button>
        <button className="border p-2 rounded-xl hover:bg-white hover:text-black">Export Patients List</button>
        </div>
      </div>
    </div>
  );
}
