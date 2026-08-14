import React, { useState, useEffect } from "react";
import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
import MainLayout from "./components/MainLayout";
import HomePage from "./pages/HomePage";
import AddPatientPage from "./pages/AddPatientPage";
import SearchPage from "./pages/SearchPage";
import PatientDetailsPage from "./pages/PatientDetailsPage";
import AboutPage from "./pages/AboutPage";
import AddProcedure from "./components/AddProcedure";
import OperationsPage from "./pages/OperationsPage";
import DisplayListDetails from "./pages/DisplayListDetails";
import ContactPage from "./pages/ContactPage";
import AuthorizationPage from "./pages/AuthorizationPage";
import UserDetailsPage from "./pages/UserDetailsPage";
import  AuthProvider  from "./context/AuthContext";
import { useAuth } from "./context/AuthContext";
import AdminDashboardPage from "./pages/AdminDashboardPage";


function AppRoutes() {
const {user} = useAuth();

if (!user) {
  return <AuthorizationPage />;
}
  
  return (
   
    
    
      <MainLayout>
        <Routes>
          <Route path="/" element={<HomePage />} />
          <Route path="/search" element={<SearchPage />} />
          <Route path="/add-patient" element={<AddPatientPage />} />
          <Route path="/patients/:id" element={<PatientDetailsPage />} />
          <Route path="/add-procedure" element={<AddProcedure />} />
          <Route path="/operations" element={<OperationsPage />} />
          <Route path="/about" element={<AboutPage />} />
          <Route path="/contact" element={<ContactPage />} /> 
         <Route path="/procedure/:id" element={<DisplayListDetails />} />
         <Route path="/payments/:id" element={<DisplayListDetails />} />
         <Route path="/user-details" element={<UserDetailsPage />} />
         <Route path="/admin-dashboard" element={<AdminDashboardPage />} />
         </Routes>
      </MainLayout>
    
  );
 }

function App() {
  return (
    <AuthProvider>
      <Router>
      <AppRoutes />
      </Router>
    </AuthProvider>
    
  );
}

export default App;
