import React from "react";

export default function FormInput({ props }) {
  const handleChange = (e) => {
    //prompt("Default");
  };

  return (
    <div>
      <label className="m-1">{props.inputName}</label>
      <input
        className="flex flex-col bg-gray-600 border rounded-sm"
        type={props.type}
        name={props.name}
        placeholder={props.placeholder}
        onChange={props.onChange}
        value={props.value}
      />
    </div>
  );
}
