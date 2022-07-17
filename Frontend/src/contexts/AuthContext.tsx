import { Path } from "navigations/routes";
import React, {
  ReactNode,
  createContext,
  useState,
  useContext,
  useEffect,
} from "react";
import { useNavigate } from "react-router-dom";

interface AuthProviderProps {
  children: ReactNode;
}

interface AuthContextI {
  isAuthenticated: boolean;
  user: UserState | undefined;
  setIsAuthenticated: React.Dispatch<React.SetStateAction<boolean>>;
  setUser: React.Dispatch<React.SetStateAction<UserState | undefined>>;
  handleLogout: () => void;
}

interface UserState {
  email: string;
  token: string;
  role: string;
}

const AuthContext = createContext<AuthContextI | null>(null);

const AuthProvider: React.FC<AuthProviderProps> = ({ children }) => {
  const [isAuthenticated, setIsAuthenticated] = useState(true);
  const [user, setUser] = useState<UserState>();

  const navigate = useNavigate();

  useEffect(() => {
    const sessionStorageUser = sessionStorage.getItem("Template_User");

    if (sessionStorageUser) {
      setUser(JSON.parse(sessionStorageUser) as UserState);
      setIsAuthenticated(true);
    } else {
      setUser(undefined);
      setIsAuthenticated(false);
    }
  }, []);

  const handleLogout = () => {
    sessionStorage.removeItem("Template_User");
    setUser(undefined);
    setIsAuthenticated(false);
    navigate(Path.Login);
  };

  const value: AuthContextI = {
    isAuthenticated,
    user,
    setIsAuthenticated,
    setUser,
    handleLogout,
  };

  return <AuthContext.Provider value={value}>{children}</AuthContext.Provider>;
};

export const useAuth = () => {
  const values = useContext(AuthContext);

  return { ...values };
};

export default AuthProvider;
