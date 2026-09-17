import React from "react";
import { Link } from "react-router-dom";

export default function DisplayList({ items, type }) {
  if (!items || items.length === 0) {
    return (
      <div className="m-2 text-center text-gray-300">
        No {type}s found.
      </div>
    );
  }

  return (
    <div className="p-2 w-full">
      <div className="flex flex-col gap-2">
        {items.map((item) => (
          <div
            key={item.id}
            className="bg-gray-600 rounded-xl p-2 text-white"
          >
            {type === "payment" ? (
              <div className="grid grid-cols-2 sm:grid-cols-3 lg:grid-cols-[1fr_2fr_1fr_1fr_1fr_auto] gap-3 lg:gap-4 items-center">

                {/* ID */}
                <div className="hidden lg:block">
                  <strong className="block text-blue-300">
                    ID:
                  </strong>
                  <span className="break-words">
                    {item.id}
                  </span>
                </div>

                {/* Procedure ID */}
                <div>
                  <strong className="block text-blue-300">
                    Procedure ID:
                  </strong>
                  <span className="break-words">
                    {item.procedureId}
                  </span>
                </div>

                {/* Method */}
                <div>
                  <strong className="block text-blue-300">
                    Method:
                  </strong>
                  <span className="break-words">
                    {item.method}
                  </span>
                </div>

                {/* Date */}
                <div className="hidden sm:block">
                  <strong className="block text-blue-300">
                    Date:
                  </strong>
                  <span className="break-words">
                    {item.paymentDate}
                  </span>
                </div>

                {/* Amount */}
                <div>
                  <strong className="block text-blue-300">
                    Amount:
                  </strong>
                  <span className="break-words">
                    ${item.amount}
                  </span>
                </div>

                {/* View Details */}
                <Link
                  to={`/payments/${item.id}`}
                  className="border border-white rounded-lg p-2 text-center hover:bg-green-500 transition-colors"
                >
                  View Details
                </Link>
              </div>
            ) : type === "procedure" ? (
              <div className="grid grid-cols-2 sm:grid-cols-3 lg:grid-cols-[1fr_2fr_1fr_1fr_1fr_auto] gap-3 lg:gap-4 items-center">

                {/* ID */}
                <div className="hidden lg:block">
                  <strong className="block text-blue-300">
                    ID:
                  </strong>
                  <span className="break-words">
                    {item.id}
                  </span>
                </div>

                {/* Name */}
                <div className="col-span-2 sm:col-span-2 lg:col-span-1 min-w-0">
                  <strong className="block text-blue-300">
                    Name:
                  </strong>
                  <span className="break-words">
                    {item.procedureName}
                  </span>
                </div>

                {/* Date */}
                <div>
                  <strong className="block text-blue-300">
                    Date:
                  </strong>
                  <span className="break-words">
                    {item.procedureDate}
                  </span>
                </div>

                {/* CPT Code */}
                <div className="hidden sm:block">
                  <strong className="block text-blue-300">
                    CPT Code:
                  </strong>
                  <span className="break-words">
                    {item.cptCode}
                  </span>
                </div>

                {/* Cost */}
                <div>
                  <strong className="block text-blue-300">
                    Cost:
                  </strong>
                  <span className="break-words">
                    ${item.patientChargedAmount}
                  </span>
                </div>

                {/* View Details */}
                <Link
                  to={`/procedure/${item.id}`}
                  className="border border-white rounded-lg p-2 text-center hover:bg-green-500 transition-colors"
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