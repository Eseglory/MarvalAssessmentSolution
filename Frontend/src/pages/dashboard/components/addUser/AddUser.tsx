import { Button, FormField, FormFieldRadio } from "components";
import { Schema, useForm } from "hooks/utils/useForm";
import React from "react";
import { PersonI } from "types/client";
import "./addUser.scss";

const initialState: PersonI = {
  firstName: "",
  surname: "",
  age: 0,
  mobile: "",
  sex: "",
  active: "",
  identity: 0,
};

const schema: Schema = {
  firstName: {
    type: "string",
    required: true,
  },
  surname: {
    type: "string",
    required: true,
  },
  age: {
    type: "number",
    required: true,
  },
  mobile: {
    type: "string",
    required: true,
  },
  sex: {
    type: "string",
    required: true,
  },
  active: {
    type: "string",
    required: true,
  },
};

interface Props {
  handleAddUser: (values: PersonI) => Promise<void>;
  isAddUserLoading: boolean;
  err: string;
}

const AddUser: React.FC<Props> = ({ handleAddUser, isAddUserLoading, err }) => {
  const { inputs, errors, handleChange, handleSubmit } = useForm(initialState, {
    submitFunc: handleAddUser,
    schema,
    validateOptions: "validateOnSubmit",
  });

  return (
    <form className="addUser" onSubmit={handleSubmit}>
      <h3 className="addUser__header">Add User</h3>
      <FormField
        error={errors.firstName}
        value={inputs.firstName}
        label="First Name"
        name="firstName"
        onChange={handleChange}
      />
      <FormField
        error={errors.surname}
        value={inputs.surname}
        label="Last Name"
        name="surname"
        onChange={handleChange}
      />
      <FormField
        error={errors.age}
        value={inputs.age}
        label="Age"
        type="number"
        name="age"
        onChange={handleChange}
      />
      <FormField
        error={errors.mobile}
        value={inputs.mobile}
        label="Mobile"
        type="tel"
        name="mobile"
        onChange={handleChange}
      />
      <FormFieldRadio
        data={[
          { name: "Male", value: "M" },
          { name: "Female", value: "F" },
        ]}
        name="sex"
        optionLabel="name"
        optionValue="value"
        state={inputs.sex}
        error={errors.sex}
        onChange={handleChange}
      />
      <FormFieldRadio
        data={[
          { name: "TRUE", value: "TRUE" },
          { name: "FALSE", value: "FALSE" },
        ]}
        name="active"
        optionLabel="name"
        optionValue="value"
        state={inputs.active}
        error={errors.active}
        onChange={handleChange}
      />
      {err && <div className="addUser__err">{err}</div>}

      <div className="addUser__btnDiv">
        <Button loading={isAddUserLoading} type="submit" fullWidth>
          Add User
        </Button>
      </div>
    </form>
  );
};

export default AddUser;
