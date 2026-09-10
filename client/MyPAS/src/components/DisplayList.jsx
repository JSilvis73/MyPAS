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
              <div className="grid grid-cols-[1fr_2fr_1fr_1fr_1fr_auto] gap-4 items-center">
                <div>
                  <strong className="block">ID:</strong>
                  <span className="break-words">{item.id}</span>
                </div>
                <div>
                  <strong className="block">Procedure Id:</strong>
                  <span className="break-words">{item.procedureId}</span>
                </div>
                <div>
                  <strong className="block">Method:</strong>
                  <span className="break-words">{item.method}</span>
                </div>
                <div>
                  <strong className="block">Date:</strong>
                  <span className="break-words">{item.paymentDate}</span>
                </div>
                <div>
                  <strong className="block">Amount:</strong>
                  <span className="break-words">${item.amount}</span>
                </div>
                <Link
                  to={`/payments/${item.id}`}
                  className="border border-white rounded-lg p-1  hover:bg-green-500"
                >
                  View Details
                </Link>
              </div>
            ) : type === "procedure" ? (
              <div className="grid grid-cols-[1fr_2fr_1fr_1fr_1fr_auto] gap-4 items-center">
                <div>
                  <strong className="block">ID:</strong>
                  <span className="break-words">{item.id}</span>
                </div>
                <div>
                  <strong className="block">Name:</strong>
                  <span className="break-words">{item.procedureName}</span>
                </div>
                <div>
                  <strong className="block">Date:</strong>
                  <span className="break-words">{item.procedureDate}</span>
                </div>
                <div>
                  <strong className="block">CPT Code:</strong>
                  <span className="break-words">{item.cptCode}</span>
                </div>
                <div>
                  <strong className="block">Cost:</strong>
                  <span className="break-words">
                    ${item.patientChargedAmount}
                  </span>
                </div>
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
