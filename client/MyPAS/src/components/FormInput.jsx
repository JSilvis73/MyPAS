import React from "react";

export default function FormInput({ props }) {
  return (
    <div className="flex flex-col items-center">
      <label className="m-1 text-blue-300">
        {props.inputName}
      </label>

      <input
        className="bg-gray-600 text-white border border-gray-600 rounded-lg p-2 shadow-lg m-1"
        type={props.type}
        name={props.name}
        placeholder={props.placeholder}
        onChange={props.onChange}
        value={props.value}
      />
    </div>
  );
}