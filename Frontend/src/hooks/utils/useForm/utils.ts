import { ErrorI, FieldObjI, Schema, UnitsI, ValidateI } from "./types";

const emailRegex =
  // eslint-disable-next-line no-useless-escape
  /^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;

// const KiloByte = 1024;

// todo validations
// phone numbers
// string with only numbers

// find, test and store different regex for different purposes.
// eg, strings with numbers only, password fields, phone number fields etc.

// this handles validation using the rules provided
export const handleValidate = <T>({
  rules,
  formFieldName,
  inputs,
}: ValidateI<T>) => {
  const errorObj: ErrorI = {};
  const inputsCopy = inputs as unknown;
  let fieldValue = (inputsCopy as FieldObjI)[formFieldName];
  const fieldType = rules.type;

  // this adds an extra check incase the user passes a field type of email and didn't pass any regex
  // just an extra advantage to passing field type as email for email validations
  if (fieldType === "email" && !rules.regex) {
    if (!emailRegex.test(fieldValue as string)) {
      errorObj[formFieldName] =
        rules.errorMsg?.invalid || `Invalid email address`;
    }
  }

  //   validations when regex rule is passed
  if (rules.regex) {
    if (fieldType === "string") {
      if (!rules.regex.test(fieldValue as string)) {
        errorObj[formFieldName] = rules.errorMsg?.regex || `Regex test failed`;
      }
    } else if (fieldType === "number") {
      if (!rules.regex.test(fieldValue as string)) {
        errorObj[formFieldName] = rules.errorMsg?.regex || `Regex test failed`;
      }
    } else if (fieldType === "email") {
      if (!rules.regex.test(fieldValue as string)) {
        errorObj[formFieldName] = rules.errorMsg?.regex || `Regex test failed`;
      }
    }
  }

  //   validations when min rule is passed
  if (rules.min) {
    if (fieldType === "string") {
      if ((fieldValue as string).length < rules.min) {
        errorObj[formFieldName] =
          rules.errorMsg?.min || `Minimum length is ${rules.min}`;
      }
    } else if (fieldType === "email") {
      if ((fieldValue as string).length < rules.min) {
        errorObj[formFieldName] =
          rules.errorMsg?.min || `Minimum length is ${rules.min}`;
      }
    } else if (fieldType === "number") {
      if (isNaN(fieldValue as number)) {
        errorObj[formFieldName] =
          rules.errorMsg?.invalid || `Please enter a valid number`;
      } else if (Number(fieldValue) < rules.min) {
        errorObj[formFieldName] =
          rules.errorMsg?.min || `Minimum length is ${rules.min}`;
      }
    } else if (fieldType === "file") {
      const files = fieldValue as FileList;

      if (files && files[0]) {
        const singleFile = files[0];

        if (typeof rules.min === "number") {
          if (singleFile.size < rules.min) {
            errorObj[formFieldName] =
              rules.errorMsg?.min || `Minimum size is ${rules.min} Bytes`;
          }
        } else {
          const checkedValueInByte = convertToByte(
            rules.min.size,
            rules.min.unit
          );

          if (singleFile.size < checkedValueInByte) {
            errorObj[formFieldName] =
              rules.errorMsg?.min ||
              `Minimum size is ${rules.min.size} ${rules.min.unit}`;
          }
        }

        if (
          rules.fileType === "image" &&
          !singleFile.type.startsWith("image")
        ) {
          errorObj[formFieldName] =
            rules.errorMsg?.invalid || `Selected file is not an image`;
        }

        if (
          rules.fileType === "video" &&
          !singleFile.type.startsWith("video")
        ) {
          errorObj[formFieldName] =
            rules.errorMsg?.invalid || `Selected file is not a video`;
        }
      }
    }
  }

  //   validations when max rule is passed
  if (rules.max) {
    if (fieldType === "string") {
      if ((fieldValue as string).length > rules.max) {
        errorObj[formFieldName] =
          rules.errorMsg?.max || `Minimum length is ${rules.max}`;
      }
    } else if (fieldType === "email") {
      if ((fieldValue as string).length > rules.max) {
        errorObj[formFieldName] =
          rules.errorMsg?.max || `Minimum length is ${rules.max}`;
      }
    } else if (fieldType === "number") {
      if (isNaN(fieldValue as number)) {
        errorObj[formFieldName] =
          rules.errorMsg?.invalid || `Please enter a valid number`;
      } else if (Number(fieldValue) > rules.max) {
        errorObj[formFieldName] =
          rules.errorMsg?.max || `Maximum length is ${rules.max}`;
      }
    } else if (fieldType === "file") {
      //   if (rules.fileType === "image") {
      const files = fieldValue as FileList;

      if (files && files[0]) {
        const singleFile = files[0];

        if (typeof rules.max === "number") {
          if (singleFile.size > rules.max) {
            errorObj[formFieldName] =
              rules.errorMsg?.max || `Minimum size is ${rules.max} Bytes`;
          }
        } else {
          const checkedValueInByte = convertToByte(
            rules.max.size,
            rules.max.unit
          );

          if (singleFile.size > checkedValueInByte) {
            errorObj[formFieldName] =
              rules.errorMsg?.max ||
              `Maximum size is ${rules.max.size} ${rules.max.unit}`;
          }
        }

        if (
          rules.fileType === "image" &&
          !singleFile.type.startsWith("image")
        ) {
          errorObj[formFieldName] =
            rules.errorMsg?.invalid || `Selected file is not an image`;
        }

        if (
          rules.fileType === "video" &&
          !singleFile.type.startsWith("video")
        ) {
          errorObj[formFieldName] =
            rules.errorMsg?.invalid || `Selected file is not a video`;
        }
      }
      //   }
    }
  }

  //   validations when required rule is passed
  if (rules.required) {
    if (fieldType === "string") {
      if ((fieldValue as string).length < 1) {
        errorObj[formFieldName] =
          rules.errorMsg?.required || `Field is required`;
      }
    } else if (fieldType === "email") {
      if ((fieldValue as string).length < 1) {
        errorObj[formFieldName] =
          rules.errorMsg?.required || `Field is required`;
      }
    } else if (fieldType === "number") {
      if (isNaN(fieldValue as number)) {
        errorObj[formFieldName] =
          rules.errorMsg?.invalid || `Please enter a valid number`;
      } else if (Number(fieldValue) < 1) {
        errorObj[formFieldName] =
          rules.errorMsg?.required || `Field is required`;
      }
    } else if (fieldType === "file") {
      if (!fieldValue || !(fieldValue as FileList).length) {
        errorObj[formFieldName] =
          rules.errorMsg?.required || `No file selected`;
      }
    } else if (fieldType === "boolean") {
      if (!fieldValue) {
        errorObj[formFieldName] = rules.errorMsg?.required || `Required field`;
      }
    }
  }

  return errorObj;
};

// this calls the handle validate function
export const handleCallValidate = <T>(inputs: T, schemaMemo: Schema) => {
  const schemaKeys = Object.keys(schemaMemo);
  let errorObj: ErrorI = {};

  schemaKeys.forEach((key) => {
    const rules = schemaMemo[key];

    errorObj = {
      ...errorObj,
      ...handleValidate({ rules, formFieldName: key, inputs }),
    };
  });

  return errorObj;
};

export const convertFromByte = (sizeInBytes: number, unit: UnitsI) => {
  const marker = 1000; // can change to 1024 if required
  const kiloByte = marker;
  const megaByte = marker ** 2;
  const gigaByte = marker ** 3;
  //   const teraByte = marker * 4;

  let convertedValue: number;

  switch (unit) {
    case "KB":
      convertedValue = sizeInBytes / kiloByte;
      break;

    case "MB":
      convertedValue = sizeInBytes / megaByte;
      break;

    case "GB":
      convertedValue = sizeInBytes / gigaByte;
      break;

    default:
      convertedValue = 0;
      break;
  }

  return convertedValue;
};

export const convertToByte = (size: number, unit: UnitsI) => {
  const marker = 1000; // can change to 1024 if required
  const kiloByte = marker;
  const megaByte = marker ** 2;
  const gigaByte = marker ** 3;
  //   const teraByte = marker * 4;

  let convertedValue: number;

  switch (unit) {
    case "KB":
      convertedValue = size * kiloByte;
      break;

    case "MB":
      convertedValue = size * megaByte;
      break;

    case "GB":
      convertedValue = size * gigaByte;
      break;

    default:
      convertedValue = 0;
      break;
  }

  return convertedValue;
};
