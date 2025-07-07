import React from "react";
import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
import MainLayout from "./components/MainLayout";
import HomePage from "./pages/HomePage";
import AddPatientPage from "./pages/AddPatientPage";
import SearchPage from "./pages/SearchPage";
import PatientDetailsPage from "./pages/PatientDetailsPage";
import AboutPage from "./pages/AboutPage";
import AddService from "./components/AddService";
import OperationsPage from "./pages/OperationsPage";
import DisplayListDetails from "./pages/DisplayListDetails";
import ContactPage from "./pages/ContactPage";
import AuthorizationPage from "./pages/AuthorizationPage";


function App() {
 const user = null;

 if (!user) {
  return (
    <div>
      <AuthorizationPage />
    </div>
  );
 }
 else
 {
  
  return (
    <Router>
      <MainLayout user={user}>
        
        <Routes>
          <Route path="/" element={<HomePage />} />
          <Route path="/search" element={<SearchPage />} />
          <Route path="/add-patient" element={<AddPatientPage />} />
          <Route path="/patients/:id" element={<PatientDetailsPage />} />
          <Route path="/add-service" element={<AddService />} />
          <Route path="/operations" element={<OperationsPage />} />
          <Route path="/about" element={<AboutPage />} />
          <Route path="/contact" element={<ContactPage />} /> 
         <Route path="/services/:id" element={<DisplayListDetails />} />
         </Routes>
      </MainLayout>
    </Router>
  );
 }


}

export default App;
