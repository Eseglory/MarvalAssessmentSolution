// export type ErrorI = Record<string, string>;

export type UnitsI = "KB" | "MB" | "GB";

interface MinMaxObj {
  size: number;
  unit: UnitsI;
}

export interface SchemaObj {
  /** This contains the possible validation types. It is a required during validation. */
  type: "string" | "number" | "email" | "file" | "boolean";
  errorMsg?: Partial<Record<ErrorMsgKeys, string>>;
  min?: number | MinMaxObj;
  max?: number | MinMaxObj;
  regex?: RegExp;
  required?: boolean;
  fileType?: "image" | "video";
}

type ErrorMsgKeys = keyof Omit<SchemaObj, "type" | "errorMsg"> | "invalid";

export type Schema = Record<string, SchemaObj>;
export interface OptionsI<T> {
  validateOptions?: "validateOnSubmit";
  submitFunc?: (values: T) => void;
  schema?: Schema;
}
export type ErrorI = Record<keyof Schema, string>;
export type FieldValueI = string | number | FileList | null | boolean;
export type FieldObjI = { [key: string]: FieldValueI };
export type InputI = Record<string, FieldValueI>;

export interface ValidateI<T> {
  rules: SchemaObj;
  formFieldName: string;
  inputs: T;
}
