import { Navigate } from "react-router-dom";

export default function ProtectedRoute({ children, role }) {
  const token = localStorage.getItem("token");
  const userRole = localStorage.getItem("role"); // make sure you save role in localStorage after login

  if (!token) {
    return <Navigate to="/login" />;
  }

  // If a role is required and user does not match → block access
  if (role && userRole !== role) {
    return <Navigate to="/dashboard" />; // redirect non-admins to their dashboard
  }

  return children;
}


//////////////////////////////////////////////
// import { Navigate } from "react-router-dom";

// export default function ProtectedRoute({ children }) {
//   const token = localStorage.getItem("token");
//   return token ? children : <Navigate to="/login" />;
// }
////////////////////////////////////////////////////////