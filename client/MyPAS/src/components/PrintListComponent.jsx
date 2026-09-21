import React from "react";
import jsPDF from "jspdf";

export default function PrintListComponent({ patients }) {
  const handlePrint = () => {
    const doc = new jsPDF();

    doc.setFontSize(18);
    doc.text("MyPAS Patient Search Results", 20, 20);

    doc.setFontSize(12);
    doc.text(
      `Found ${patients.length} patient${patients.length === 1 ? "" : "s"}`,
      20,
      30
    );

    let y = 45;

    doc.text("Count | Patient ID | Last Name, First Name", 20, y);
    y += 10;

    patients.forEach((patient, index) => {
      doc.text(
        `${index + 1}. ${patient.id} - ${patient.lastName}, ${patient.firstName}`,
        20,
        y
      );

      y += 10;

      // Start a new page if we run out of room
      if (y > 280) {
        doc.addPage();
        y = 20;
      }
    });

    doc.save("MyPAS-Search-Results.pdf");
  };

  return (
    <button
      type="button"
      onClick={handlePrint}
      className="rounded-lg border border-white p-2 hover:bg-blue-500"
    >
      Download PDF
    </button>
  );
}