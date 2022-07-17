import { Button, FormField, FormFieldRadio } from "components";
import { Schema, useForm } from "hooks/utils/useForm";
import React, { useEffect, useState } from "react";
import { PersonI } from "types/client";
import "./updateUser.scss";

export interface UpdateUserData {
  firstName: string;
  surName: string;
  age: string;
  mobile: string;
  sex: string;
}

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
};

interface Props {
  handleUpdateUser: (values: PersonI) => Promise<void>;
  selectedUser: PersonI | undefined;
  isUpdateUserLoading: boolean;
}

const UpdateUser: React.FC<Props> = ({
  handleUpdateUser,
  selectedUser,
  isUpdateUserLoading,
}) => {
  const [initialState, setInitialState] = useState<PersonI>({
    firstName: "",
    age: 0,
    mobile: "",
    sex: "",
    active: "",
    surname: "",
    identity: 0,
  });

  const { inputs, errors, handleChange, handleSubmit } = useForm(initialState, {
    submitFunc: handleUpdateUser,
    schema,
    validateOptions: "validateOnSubmit",
  });

  useEffect(() => {
    if (selectedUser) {
      setInitialState(selectedUser);
    }
  }, [selectedUser]);

  return (
    <form className="updateUser" onSubmit={handleSubmit}>
      <h3 className="updateUser__header">Update User</h3>
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
          { name: "Male", value: "0" },
          { name: "Female", value: "1" },
        ]}
        name="sex"
        optionLabel="name"
        optionValue="name"
        state={inputs.sex}
        error={errors.sex}
        onChange={handleChange}
      />
      <div className="updateUser__btnDiv">
        <Button loading={isUpdateUserLoading} type="submit" fullWidth>
          Add User
        </Button>
      </div>
    </form>
  );
};

export default UpdateUser;
