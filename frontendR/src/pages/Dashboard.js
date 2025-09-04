import { useEffect, useState } from "react";
import { getMyResumes, deleteResume, downloadResumePdf } from "../api/resumeApi";
import { useNavigate } from "react-router-dom";
import './Dashboard.css';

export default function Dashboard() {
  const [resumes, setResumes] = useState([]);
  const navigate = useNavigate();

  const fetchResumes = async () => {
    const data = await getMyResumes();
    setResumes(data);
  };

  useEffect(() => {
    fetchResumes();
  }, []);


  const handleDownload = async (id) => {
    await downloadResumePdf(id);
  };

  return (
    <div>
      <h1>My Resumes</h1>
      <button onClick={() => navigate("/resume-form")}>Create New Resume</button>

      {resumes.map((r) => (
        <div key={r.id} style={{ border: "1px solid #ccc", margin: "10px", padding: "10px" }}>
          <h3>{r.title}</h3>
          <button onClick={() => navigate(`/resume-form/${r.id}`)}>Edit</button>
          {/* <button onClick={() => handleDelete(r.id)} style={{ marginLeft: "10px", color: "red" }}>
            Delete
          </button> */}
          <button onClick={() => handleDownload(r.id)} style={{ marginLeft: "10px" }}>
            Download PDF
          </button>
        </div>
      ))}
    </div>
  );
}
