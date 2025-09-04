import { useState } from "react";
import { useNavigate } from "react-router-dom";
import api from "../api/axios";

export default function Register() {
  const [fullName, setFullName] = useState("");
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const navigate = useNavigate();

  const handleRegister = async (e) => {
    e.preventDefault();
    try {
      await api.post("/api/auth/register", { 
        FullName: fullName, 
        Email: email, 
        Password: password 
      });
      alert("Registered successfully, please login!");
      navigate("/login");
    } catch (err) {
      console.log(err.response?.data); // check backend error
      alert("Registration failed: " + (err.response?.data?.message || ""));
    }
  };

  return (
    <div className="form-container">
      <h2>Register</h2>
      <form onSubmit={handleRegister}>
        <input type="text" placeholder="Full Name"
               value={fullName} onChange={(e) => setFullName(e.target.value)} />
        <input type="email" placeholder="Email"
               value={email} onChange={(e) => setEmail(e.target.value)} />
        <input type="password" placeholder="Password"
               value={password} onChange={(e) => setPassword(e.target.value)} />
        <button type="submit">Register</button>
      </form>
    </div>
  );
}
