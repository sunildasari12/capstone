import { useState } from "react";
import ResumeForm from "../components/ResumeForm";

export default function ResumePreview() {
  const [resume, setResume] = useState(null);

  return (
    <div>
      <h1>Resume Preview</h1>
      <ResumeForm onSubmit={(data) => setResume(data)} />
      {resume && (
        <div className="resume-preview">
          <h2>{resume.name}</h2>
          <p>{resume.email}</p>
          <h3>Skills</h3>
          <p>{resume.skills}</p>
          <h3>Experience</h3>
          <p>{resume.experience}</p>
        </div>
      )}
    </div>
  );
}
