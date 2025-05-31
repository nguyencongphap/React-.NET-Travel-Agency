import {
  createContext,
  useState,
  type Dispatch,
  type ReactNode,
  type SetStateAction,
} from "react";

// TODO: update later for more security
interface AuthState {
  username: string;
  password: string;
  accessToken: string;
  refreshToken: string;
}

const AuthContext = createContext<
  | {
      auth: AuthState | undefined;
      setAuth: Dispatch<SetStateAction<AuthState | undefined>>;
    }
  | undefined
>(undefined);

interface AuthProviderProps {
  children: ReactNode;
}

export const AuthProvider = ({ children }: AuthProviderProps) => {
  const [auth, setAuth] = useState<AuthState>();

  return (
    <AuthContext.Provider value={{ auth, setAuth }}>
      {children}
    </AuthContext.Provider>
  );
};

export default AuthContext;

// TODO: Continue at 16:20
