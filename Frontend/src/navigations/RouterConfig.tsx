import { useAuth } from "contexts/AuthContext";
import React, { lazy, Suspense } from "react";
import { Navigate, Route, Routes } from "react-router-dom";
import ProtectedRoute from "./ProtectedRoute";
import { Path } from "./routes";

const Login = lazy(() => import("pages/login"));
const Dashboard = lazy(() => import("pages/dashboard"));
const SinglePerson = lazy(() => import("pages/singlePerson"));

const RouterConfig = () => {
  const { isAuthenticated } = useAuth();

  return (
    <Suspense fallback={<div>Loading...</div>}>
      <Routes>
        <Route
          path={Path.Index}
          element={
            <Navigate to={isAuthenticated ? Path.Dashboard : Path.Login} />
          }
        />
        <Route
          path={Path.Login}
          element={
            isAuthenticated ? <Navigate to={Path.Dashboard} /> : <Login />
          }
        />
        <Route element={<ProtectedRoute isAuthenticated={isAuthenticated!} />}>
          <Route path={Path.Dashboard} element={<Dashboard />} />
          <Route path={"/person/:id"} element={<SinglePerson />} />
        </Route>
      </Routes>
    </Suspense>
  );
};

export default RouterConfig;
