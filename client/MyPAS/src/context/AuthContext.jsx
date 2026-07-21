
import React, { createContext, useContext, useState, useEffect } from "react";
import {jwtDecode} from "jwt-decode";

const AuthContext = createContext();

export default function AuthProvider({ children }) {
    const [user, setUser] = useState(null);

    // Load user on app start.
    useEffect(() => {
        const token = localStorage.getItem("token");
        const userInStorage = localStorage.getItem("user");
        
        if (token && userInStorage)
        {
            try {
                const storedUser = JSON.parse(userInStorage);
 
                setUser(storedUser);
                  console.log("User loaded from storage:", storedUser);
            } catch (error) {
                console.error("Invalid token:", error);
                localStorage.removeItem("token");
                localStorage.removeItem("user");
                setUser(null);
            }
        }

    }, []);

    // Sign in takes the authentication result, stores it in local storage, and updates the user state.
    const signIn = (authResult) => 
        {
            localStorage.setItem("token", authResult.token);
            localStorage.setItem("user", JSON.stringify(authResult.user));
            
            setUser(authResult.user);
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
