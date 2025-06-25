import React from "react";

export default function AboutPage() {
  return (
    <div className="max-w-xl mx-auto mt-8 bg-gray-800 text-white border border-gray-600 rounded-lg p-6 shadow-lg">
      <div className="flex flex-col items-center gap-2">
       <h2 className="text-2xl mb-4"><strong>About</strong></h2>
        <p>
          Welcome to MyPAS, your trusted partner in patient accounting and
          healthcare management. Our Mission At MyPAS, our mission is simple: To
          empower healthcare providers with intuitive, reliable tools that
          streamline patient billing and record-keeping — so you can focus on
          what matters most: quality care.
        </p>
      </div>
    </div>
  );
}
