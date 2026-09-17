import React from "react";

export default function ContactPage() {
  return (
    <div className="flex flex-col items-center gap-2">
      <h2 className="text-2xl mb-4 text-blue-500">
        <strong>Contact</strong>
      </h2>
      <div className="max-w-xl mx-auto bg-gray-800 text-white border border-gray-600 rounded-lg p-6 shadow-lg text-center">
        <h3 className="mb-2 text-blue-500">
          <strong>Support</strong>
        </h3>
        <p>
          <strong className="text-blue-300">Email: </strong>
          <br />
          ExampleEmail@gmail.com
        </p>
        <p>
          <strong className="text-blue-300">Phone: </strong>
          <br />
          330-940-9200
        </p>
      </div>
    </div>
  );
}
