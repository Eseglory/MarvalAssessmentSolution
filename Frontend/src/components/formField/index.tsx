import React, { useId } from "react";
import "./formField.scss";

interface Props extends React.InputHTMLAttributes<HTMLInputElement> {
  label: string;
  error?: string;
}

const FormField: React.FC<Props> = ({
  label,
  error,
  type = "text",
  ...props
}) => {
  const id = useId();
  return (
    <div className="formField">
      <label className="formField__label" htmlFor={id}>
        {label}
      </label>
      <input id={id} className="formField__input" type={type} {...props} />
      {error && <span className="formField__error">{error}</span>}
    </div>
  );
};

export default FormField;
