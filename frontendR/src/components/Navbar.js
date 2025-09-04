// import { Link, useNavigate } from "react-router-dom";
import { Link, useNavigate } from "react-router-dom";
export default function Navbar() {
  const navigate = useNavigate();

  const handleLogout = () => {
    localStorage.removeItem("token");
    localStorage.removeItem("role"); // remove role on logout
    navigate("/login");
  };

  const token = localStorage.getItem("token");
  const role = localStorage.getItem("role"); // "Admin" or "User"

  return (
    <nav className="navbar">
      <Link to="/">Home</Link>

      {token ? (
        <>
          {role === "Admin" && <Link to="/admin">Dashboard</Link>}
          {role === "User" && <Link to="/dashboard">Dashboard</Link>}
          <button onClick={handleLogout}>Logout</button>
        </>
      ) : (
        <>
          <Link to="/login">Login</Link>
          <Link to="/register">Register</Link>
        </>
      )}
    </nav>
  );
}
