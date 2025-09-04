import api from "./axios";

const token = localStorage.getItem("token");
const headers = { Authorization: `Bearer ${token}` };

// Get resumes of current user
export const getMyResumes = async () => {
  const res = await api.get("/api/resumes/my", { headers });
  return res.data;
};
//admin api///////////
// Get all users
export const getUsers = async () => {
  const res = await api.get("/api/Admin/users", { headers });
  return res.data;
};

// Delete user by ID
export const deleteUser = async (id) => {
  const res = await api.delete(`/api/Admin/users/${id}`, { headers });
  return res.data;
};

// Get all resumes (admin can see all)
export const getResumes = async () => {
  const res = await api.get("/api/Admin/resumes", { headers });
  return res.data;
};
// Get a single resume by ID
export const getResumeById = async (id) => {
  const res = await api.get(`/api/resumes/my`, { headers }); // fetch all and filter
  const resume = res.data.find(r => r.id === parseInt(id));
  if (!resume) throw new Error("Resume not found");
  return resume;
};

// Create a new resume
export const createResume = async (resume) => {
  const res = await api.post("/api/resumes", resume, { headers });
  return res.data;
};

// Update an existing resume
export const updateResume = async (id, resume) => {
  const res = await api.put(`/api/resumes/${id}`, resume, { headers });
  return res.data;
};

// // Delete resume
// export const deleteResume = async (id) => {
//   const res = await api.delete(`/api/resumes/${id}`, { headers });
//   return res.data;
// };

// Download resume as PDF
export const downloadResumePdf = async (id) => {
  const res = await api.get(`/api/resumes/${id}/download`, {
    headers,
    responseType: "blob", // important for files
  });

  // Create a link to download
  const url = window.URL.createObjectURL(new Blob([res.data]));
  const link = document.createElement("a");
  link.href = url;
  link.setAttribute("download", `Resume_${id}.pdf`);
  document.body.appendChild(link);
  link.click();
  link.remove();
};
// ---------------------- ADMIN APIs ----------------------
