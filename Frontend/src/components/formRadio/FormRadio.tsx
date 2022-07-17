import cs from "classnames";
import { useEffect, useState } from "react";
import { v4 } from "uuid";
import "./formRadio.scss";

interface sortedDataI {
  value: string;
  label: string;
  id: string;
}

export interface FormRadioI {
  label?: string;
  name: string;
  data: any[];
  optionValue: string;
  optionLabel: string;
  className?: string;
  error?: string;
  state: string;
  onChange: (e: React.ChangeEvent<HTMLInputElement>) => void;
}

const FormRadio: React.FC<FormRadioI> = ({
  label,
  name,
  data,
  optionLabel,
  optionValue,
  className,
  error,
  state,
  onChange,
}) => {
  const [sortedData, setSortedData] = useState<sortedDataI[]>([]);

  useEffect(() => {
    const opts = data.map((item) => {
      let optionData: sortedDataI = {
        value: item[optionValue],
        label: item[optionLabel],
        id: v4(),
      };

      return optionData;
    });

    setSortedData(opts);
  }, [data, optionLabel, optionValue]);

  const classes = cs("formRadio", { [`${className}`]: className });

  return (
    <div className={classes}>
      <label className="formRadio__label">{label}</label>
      <div className="formRadio__radios">
        {sortedData.map((item) => (
          <div key={item.id} className="formRadio__radio">
            <input
              className="formRadio__input"
              type="radio"
              id={item.id}
              value={item.value}
              name={name}
              checked={state === item.value}
              onChange={onChange}
            />
            <label className="formRadio__label" htmlFor={item.id}>
              {item.label}
            </label>
          </div>
        ))}
      </div>
      {error && <span className="formRadio__error">{error}</span>}
    </div>
  );
};

export default FormRadio;
