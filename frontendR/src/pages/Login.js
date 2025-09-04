import { useState } from "react";
import { useNavigate } from "react-router-dom";
import api from "../api/axios";
import {jwtDecode} from "jwt-decode"; // 👈 import jwt-decode
import "./Login.css";

export default function Login() {
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const navigate = useNavigate();

  const handleLogin = async (e) => {
    e.preventDefault();
    try {
      const res = await api.post("/api/auth/login", { email, password });
      
      const token = res.data.token;
      localStorage.setItem("token", token);

      // 👇 Decode the JWT to extract role
      const decoded = jwtDecode(token);
      const role = decoded["role"] || decoded["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"];
      
      if (role) {
        localStorage.setItem("role", role);
      }
console.log(role)
      // 👇 Redirect based on role
      if (role === "Admin") {
        navigate("/admin");
      } else {
        navigate("/dashboard");
      }

    } catch {
      alert("Invalid login");
    }
  };

  return (
    <div className="form-container">
      <h2>Login</h2>
      <form onSubmit={handleLogin}>
        <input
          type="email"
          placeholder="Email"
          value={email}
          onChange={(e) => setEmail(e.target.value)}
        />
        <input
          type="password"
          placeholder="Password"
          value={password}
          onChange={(e) => setPassword(e.target.value)}
        />
        <button type="submit">Login</button>
      </form>
    </div>
  );
}



