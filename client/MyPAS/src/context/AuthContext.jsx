
import React, { createContext, useContext, useState, useEffect } from "react";
import {jwtDecode} from "jwt-decode";

const AuthContext = createContext();

export default function AuthProvider({ children }) {
    const [user, setUser] = useState(null);

    // Load user on app start.
    useEffect(() => {
        const token = localStorage.getItem("token");
        
        if (token)
        {
            try {
                const decodedToken = jwtDecode(token);
                const user = {
                    id: decodedToken.sub,
                    email: decodedToken.email,
                    firstName: decodedToken.firstName,
                    lastName: decodedToken.lastName,
                    userName: decodedToken.username,
                    roles: decodedToken.roles,
                }
                setUser(user);
                console.log("User loaded from token:", user);
                console.log("Decoded token:", decodedToken);
            } catch (error) {
                console.error("Invalid token:", error);
                localStorage.removeItem("token");
                setUser(null);
            }
        }

    }, []);

    // Sign in takes the authentication result, stores it in local storage, and updates the user state.
    const signIn = (authResult) => 
        {
            localStorage.setItem("token", authResult.token);
            const decodedToken = jwtDecode(authResult.token);

                  const user = {
                    id: decodedToken.sub,
                    email: decodedToken.email,
                    firstName: decodedToken.firstName,
                    lastName: decodedToken.lastName,
                    userName: decodedToken.username,
                    roles: decodedToken.roles,
                }
            
            setUser(user);
        }

    // Sign out clears clients local storage and resets user state.
    const signOut = () => {
        localStorage.removeItem("token");
        localStorage.removeItem("user");
        setUser(null);
    };

    return (
        <AuthContext.Provider value={{ user, signIn, signOut }}>
            {children}
        </AuthContext.Provider>
    );

};
    export function useAuth() {
        return useContext(AuthContext);
    }
