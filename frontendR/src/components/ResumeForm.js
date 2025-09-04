import { useState, useEffect } from "react";
import { useNavigate, useParams } from "react-router-dom";
import { createResume, updateResume, getResumeById } from "../api/resumeApi";

export default function ResumeForm() {
  const { id } = useParams(); // undefined for new resume
  const navigate = useNavigate();
  const [formData, setFormData] = useState({
    title: "",
    summary: "",
    educations: [{ institution: "", degree: "", fieldOfStudy: "", startYear: "", endYear: "" }],
    experiences: [{ company: "", title: "", description: "", startYear: "", endYear: "" }]
  });

  // Load existing resume if editing
  useEffect(() => {
    if (id) {
      getResumeById(id).then((res) => setFormData(res));
    }
  }, [id]);

  const handleChange = (e) => setFormData({ ...formData, [e.target.name]: e.target.value });

  const handleEducationChange = (index, e) => {
    const newEducations = [...formData.educations];
    newEducations[index][e.target.name] = e.target.value;
    setFormData({ ...formData, educations: newEducations });
  };

  const handleExperienceChange = (index, e) => {
    const newExperiences = [...formData.experiences];
    newExperiences[index][e.target.name] = e.target.value;
    setFormData({ ...formData, experiences: newExperiences });
  };

  const addEducation = () => {
    setFormData({
      ...formData,
      educations: [...formData.educations, { institution: "", degree: "", fieldOfStudy: "", startYear: "", endYear: "" }]
    });
  };

  const addExperience = () => {
    setFormData({
      ...formData,
      experiences: [...formData.experiences, { company: "", title: "", description: "", startYear: "", endYear: "" }]
    });
  };

  const handleSave = async () => {
    try {
      if (id) {
        await updateResume(id, formData);
      } else {
        await createResume(formData);
      }
      alert("Resume saved successfully!");
      navigate("/dashboard"); // redirect to dashboard
    } catch (err) {
      console.error(err);
      alert("Error saving resume!");
    }
  };

  return (
    <div>
      <h2>{id ? "Edit Resume" : "New Resume"}</h2>
      <input name="title" placeholder="Resume Title" value={formData.title} onChange={handleChange} />
      <textarea name="summary" placeholder="Summary" value={formData.summary} onChange={handleChange} />

      <h3>Education</h3>
      {formData.educations.map((edu, i) => (
        <div key={i}>
          <input name="institution" placeholder="Institution" value={edu.institution} onChange={(e) => handleEducationChange(i, e)} />
          <input name="degree" placeholder="Degree" value={edu.degree} onChange={(e) => handleEducationChange(i, e)} />
          <input name="fieldOfStudy" placeholder="Field of Study" value={edu.fieldOfStudy} onChange={(e) => handleEducationChange(i, e)} />
          <input name="startYear" placeholder="Start Year" value={edu.startYear} onChange={(e) => handleEducationChange(i, e)} />
          <input name="endYear" placeholder="End Year" value={edu.endYear} onChange={(e) => handleEducationChange(i, e)} />
        </div>
      ))}
      <button onClick={addEducation}>Add Education</button>

      <h3>Experience</h3>
      {formData.experiences.map((exp, i) => (
        <div key={i}>
          <input name="company" placeholder="Company" value={exp.company} onChange={(e) => handleExperienceChange(i, e)} />
          <input name="title" placeholder="Title" value={exp.title} onChange={(e) => handleExperienceChange(i, e)} />
          <textarea name="description" placeholder="Description" value={exp.description} onChange={(e) => handleExperienceChange(i, e)} />
          <input name="startYear" placeholder="Start Year" value={exp.startYear} onChange={(e) => handleExperienceChange(i, e)} />
          <input name="endYear" placeholder="End Year" value={exp.endYear} onChange={(e) => handleExperienceChange(i, e)} />
        </div>
      ))}
      <button onClick={addExperience}>Add Experience</button>

      <button onClick={handleSave}>Save Resume</button>
    </div>
  );
}
