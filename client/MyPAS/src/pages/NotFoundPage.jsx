import React from "react";
import { PiQuestionMarkFill } from "react-icons/pi";

export default function NotFoundPage() {
  return (
    <div className="bg-gray-800 text-white border border-gray-600 rounded-lg p-6 shadow-lg max-w-xl mx-auto mt-8">
      <div className="text-center flex flex-col items-center justify-center gap-4 text-white">
        <PiQuestionMarkFill className="mx-auto text-6xl hover:text-blue-500" />
        <h1 className="text-4xl font-bold">404</h1>
        <p className="text-lg">The page you are looking for does not exist.</p>
      </div>
    </div>
  );
}
