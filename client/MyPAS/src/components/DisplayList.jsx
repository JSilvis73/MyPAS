import React, { useState } from "react";
import { Link } from "react-router-dom";

export default function DisplayList({ items, type }) {
  if (items[0] == null) {
    return <div className="m-2 text-center">No {type}s found.</div>;
  }

  return (
    <div className="p-2">
      <div className="flex flex-col gap-2">
        {items.map((item) => (
          <div key={item.id} className="bg-gray-600  rounded-xl p-2 text-white">
            {type === "payment" ? (
              <div className="flex justify-center items-center gap-4">
                <p>
                  <strong>ID:</strong> {item.id}
                </p>
                <p>
                  <strong>Method:</strong> {item.method}
                </p>
                <p>
                  <strong>Date:</strong> {item.paymentDate}
                </p>
                <p>
                  <strong>Amount:</strong> ${item.amount}
                </p>
                <Link
                  to={`/payments/${item.id}`}
                  className="border border-white rounded-lg p-1  hover:bg-green-500"
                >
                  View Details
                </Link>
              </div>
            ) : type === "procedure" ? (
              <div className="flex justify-center items-center gap-4">
                <p>
                  <strong>ID:</strong> {item.id}
                </p>
                <p>
                  <strong>Procedure:</strong> {item.procedureName}
                </p>
                <p>
                  <strong>Date:</strong> {item.procedureDate}
                </p>
                <p>
                  <strong>CPT Code:</strong> {item.cptCode}
                </p>
                <p>
                  <strong>Cost: </strong> ${item.patientChargedAmount}
                </p>
                <Link
                  to={`/procedure/${item.id}`}
                  className="border border-white rounded-lg p-1 hover:bg-green-500"
                >
                  View Details
                </Link>
              </div>
            ) : (
              <p>Unknown item type</p>
            )}
          </div>
        ))}
      </div>
    </div>
  );
}
