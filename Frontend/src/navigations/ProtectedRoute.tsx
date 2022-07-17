import React from "react";
import { Navigate, Outlet } from "react-router-dom";
import { Path } from "./routes";

interface Props {
  isAuthenticated: boolean;
}

const ProtectedRoute: React.FC<Props> = ({ isAuthenticated }) => {
  if (!isAuthenticated) {
    return <Navigate to={Path.Login} replace />;
  }

  return <Outlet />;
};

export default ProtectedRoute;
