import React, { useEffect, useMemo, useState } from "react";
import { ErrorI, OptionsI } from "./types";
import { handleCallValidate } from "./utils";

/**
 *
 * @param initialValues
 * @param options
 * @returns inputs, handleChange, handleChangeTextarea, handleSubmit, errors
 * @desc this is a form hook use to handle form state management and validations
 */

// for now, do not add d initialValues object directly. store it in a state or a variable before passing it
const useForm = <T>(initialValues: T, options?: OptionsI<T>) => {
  const initialValuesMemo = useMemo(() => initialValues, [initialValues]);
  const schemaMemo = useMemo(() => options?.schema, [options?.schema]);
  const optionsMemo = useMemo(() => options, [options]);

  const [inputs, setInputs] = useState<T>(initialValuesMemo);
  const [errors, setErrors] = useState<ErrorI>({});

  // effect to create an initial error object from the initial values
  useEffect(() => {
    const errorKeys = Object.keys(initialValuesMemo);

    const errorObj: ErrorI = {};

    errorKeys.forEach((err) => {
      errorObj[err] = "";
    });

    setErrors(errorObj);
  }, [initialValuesMemo]);

  // effect to update input based on change to the initialValues
  // important when input values depends on fetched data
  useEffect(() => {
    if (initialValuesMemo) {
      setInputs(initialValuesMemo);
    }
  }, [initialValuesMemo]);

  // effect to automate validation
  // to do
  useEffect(() => {
    if (
      inputs &&
      schemaMemo &&
      optionsMemo?.validateOptions !== "validateOnSubmit"
    ) {
      const errorObj = handleCallValidate(inputs, schemaMemo);

      setErrors(errorObj);
    }
  }, [inputs, optionsMemo?.validateOptions, schemaMemo]);

  // handle change function for input elements
  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    if (e.target.type === "file") {
      if (e.target.files) {
        setInputs((prev) => ({
          ...prev,
          [e.target.name]: e.target?.files![0] || null,
        }));
      }
    } else if (e.target.type === "radio") {
      setInputs((prev) => ({
        ...prev,
        [e.target.name]: e.target.value,
      }));
    } else {
      setInputs((prev) => ({
        ...prev,
        [e.target.name]: e.target.checked || e.target.value,
      }));
    }
  };

  // handle change function for text area elements
  const handleChangeTextarea = (e: React.ChangeEvent<HTMLTextAreaElement>) => {
    setInputs((prev: T) => ({
      ...prev,
      [e.target.name]: e.target.value,
    }));
  };

  // handle submit function. important for enforcing validations using the provided schema
  const handleSubmit = (e: React.FormEvent<HTMLFormElement>) => {
    e.preventDefault();

    let errors: ErrorI = {};
    let isError = false;

    if (schemaMemo) {
      errors = handleCallValidate(inputs, schemaMemo);
      isError = !!Object.keys(errors).length;
    }

    if (optionsMemo?.validateOptions === "validateOnSubmit") {
      if (isError) {
        setErrors(errors);
        return;
      }
    }

    setErrors({});

    if (options?.submitFunc) {
      options.submitFunc(inputs);
    }
  };

  // const handleFile = (e) => {
  //   const file = e.target.files[0];

  //   if (file) {
  //     if (file && file.type.startsWith("image")) {
  //       setInputs((prev) => ({
  //         ...prev,
  //         [e.target.name]: file,
  //       }));
  //     } else {
  //       setInputs((prev) => ({
  //         ...prev,
  //         [e.target.name]: null,
  //       }));
  //     }
  //   }
  // };

  // const handleFileRemove = (name, arg) => {
  //   console.log(arg);
  //   setInputs((prev) => {
  //     // const remnant = prev.filter(item => prev.indexOf(item) !== arg);
  //     const data = prev[name];

  //     data.splice(arg, 1);

  //     // console.log(data);

  //     return {
  //       ...prev,
  //       [name]: data,
  //     };
  //   });
  // };

  return { inputs, handleChange, handleChangeTextarea, handleSubmit, errors };
};

export default useForm;
