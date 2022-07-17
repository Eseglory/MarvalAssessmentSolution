import React, { useState } from "react";
import { Button, FormField } from "components";
import "./login.scss";
import { Schema, useForm } from "hooks/utils/useForm";
import { login, LoginData } from "services/auth";
import { useAuth } from "contexts/AuthContext";
import { useNavigate } from "react-router-dom";
import { Path } from "navigations/routes";

const initialValues: LoginData = {
  email: "",
  password: "",
};

const schema: Schema = {
  email: {
    type: "email",
    required: true,
  },
  password: {
    type: "string",
    required: true,
  },
};

const Login = () => {
  const { inputs, errors, handleChange, handleSubmit } = useForm(
    initialValues,
    {
      schema,
      validateOptions: "validateOnSubmit",
      submitFunc: onSubmit,
    }
  );

  const [loading, setLoading] = useState(false);
  const { setIsAuthenticated, setUser } = useAuth();

  const navigate = useNavigate();

  async function onSubmit(values: LoginData) {
    setLoading(true);

    try {
      const res = await login(values);

      if (res) {
        const user = {
          email: res.email,
          role: res.role,
          token: res.jwtToken,
        };
        sessionStorage.setItem("Template_User", JSON.stringify(user));
        setIsAuthenticated!(true);
        setUser!(user);

        navigate(Path.Dashboard);
      }
      setLoading(false);
    } catch (error) {
      setLoading(false);
    }
  }

  return (
    <div className="login">
      <h1 className="login__header">Sign In</h1>
      <form className="login__form" onSubmit={handleSubmit}>
        <FormField
          name="email"
          value={inputs.email}
          label="Email"
          type="email"
          error={errors.email}
          onChange={handleChange}
        />
        <FormField
          name="password"
          value={inputs.password}
          label="Password"
          type="password"
          error={errors.password}
          onChange={handleChange}
        />
        <div className="login__btnDiv">
          <Button loading={loading} fullWidth>
            Login
          </Button>
        </div>
      </form>
    </div>
  );
};

export default Login;
