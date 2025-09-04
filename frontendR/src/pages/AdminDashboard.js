import React, { useEffect, useState } from "react";
import { getUsers, deleteUser, getResumes } from "../api/resumeApi";

const AdminDashboard = () => {
  const [users, setUsers] = useState([]);
  const [resumes, setResumes] = useState([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    fetchData();
  }, []);

  const fetchData = async () => {
    try {
      setLoading(true);
      const usersRes = await getUsers();
      const resumesRes = await getResumes();
// console.log(usersRes)
      // ✅ Use fallback empty arrays to avoid "undefined.length" error
      setUsers(usersRes);
      console.log(resumesRes)
      setResumes(resumesRes );
    } catch (error) {
      console.error("Error fetching admin data:", error);
      setUsers([]);   // ✅ reset to safe values
      setResumes([]);
    } finally {
      setLoading(false);
    }
  };

  const handleDeleteUser = async (id) => {
    if (window.confirm("Are you sure you want to delete this user?")) {
      try {
        await deleteUser(id);
        setUsers((prev) => prev.filter((u) => u.id !== id)); // ✅ safer update
      } catch (error) {
        console.error("Error deleting user:", error);
      }
    }
  };

  if (loading) return <p>Loading...</p>;

  return (
    <div style={{ padding: "20px" }}>
      <h2>👤 User Management</h2>
      <table border="1" cellPadding="10" style={{ width: "100%", marginBottom: "30px" }}>
        <thead>
          <tr>
            <th>ID</th>
            <th>Full Name</th>
            <th>Email</th>
            <th>Actions</th>
          </tr>
        </thead>
        <tbody>
          {users.length > 0 ? (
            users.map((user) => (
              // {console.log(user)}
              <tr key={user.id}>
                {/* {if(user.id!=1)} */}
                <td>{user.id}</td>

                <td>{user.fullName}</td>
                <td>{user.email}</td>
                <td>
             <button
    style={{
      background: user.email === "admin@resumebuilder.com" ? "grey" : "red",
      color: "white",
      padding: "5px 10px",
      border: "none",
      cursor: user.email === "admin@resumebuilder.com" ? "not-allowed" : "pointer",
    }}
    onClick={() => handleDeleteUser(user.id)}
    disabled={user.email === "admin@resumebuilder.com"} // ✅ disable button for admin
  >
    Delete
  </button>
</td>

                {/* <td>
                  <button
                    style={{
                      background: "red",
                      color: "white",
                      padding: "5px 10px",
                      border: "none",
                      cursor: "pointer",
                    }}
                    onClick={() => handleDeleteUser(user.id)}
                  >
                    Delete
                  </button>
                </td> */}
              </tr>
            ))
          ) : (
            <tr>
              <td colSpan="4">No users found</td>
            </tr>
          )}
        </tbody>
      </table>

      <h2>📄 Resumes</h2>
      <table border="1" cellPadding="10" style={{ width: "100%" }}>
        <thead>
          <tr>
            <th>ID</th>
            <th>User ID</th>
            <th>Title</th>
            <th>Created At</th>
          </tr>
        </thead>
        <tbody>
          {resumes.length > 0 ? (
            resumes.map((resume) => (
              <tr key={resume.id}>
                <td>{resume.id}</td>
                <td>{resume.user.id}</td>
                <td>{resume.title}</td>
                <td>{resume.createdAt ? new Date(resume.createdAt).toLocaleString() : "N/A"}</td>
              </tr>
            ))
          ) : (
            <tr>
              <td colSpan="4">No resumes found</td>
            </tr>
          )}
        </tbody>
      </table>
    </div>
  );
};

export default AdminDashboard;
 
//import React, { useEffect, useState } from "react";
// import { getUsers, deleteUser, getResumes } from "../api/resumeApi";

// const AdminDashboard = () => {
//   const [users, setUsers] = useState([]);
//   const [resumes, setResumes] = useState([]);
//   const [loading, setLoading] = useState(true);

//   useEffect(() => {
//     fetchData();
//   }, []);

//   const fetchData = async () => {
//     try {
//       setLoading(true);
//       const usersRes = await getUsers();
//       const resumesRes = await getResumes();
//       setUsers(usersRes.data);
//       setResumes(resumesRes.data);
//     } catch (error) {
//       console.error("Error fetching admin data:", error);
//     } finally {
//       setLoading(false);
//     }
//   };

//   const handleDeleteUser = async (id) => {
//     if (window.confirm("Are you sure you want to delete this user?")) {
//       try {
//         await deleteUser(id);
//         setUsers(users.filter((u) => u.id !== id));
//       } catch (error) {
//         console.error("Error deleting user:", error);
//       }
//     }
//   };

//   if (loading) return <p>Loading...</p>;

//   return (
//     <div style={{ padding: "20px" }}>
//       <h2>👤 User Management</h2>
//       <table border="1" cellPadding="10" style={{ width: "100%", marginBottom: "30px" }}>
//         <thead>
//           <tr>
//             <th>ID</th>
//             <th>Full Name</th>
//             <th>Email</th>
//             <th>Actions</th>
//           </tr>
//         </thead>
//         <tbody>
//           {users.length > 0 ? (
//             users.map((user) => (
//               <tr key={user.id}>
//                 <td>{user.id}</td>
//                 <td>{user.fullName}</td>
//                 <td>{user.email}</td>
//                 <td>
//                   <button 
//                     style={{ background: "red", color: "white", padding: "5px 10px", border: "none" }} 
//                     onClick={() => handleDeleteUser(user.id)}
//                   >
//                     Delete
//                   </button>
//                 </td>
//               </tr>
//             ))
//           ) : (
//             <tr><td colSpan="4">No users found</td></tr>
//           )}
//         </tbody>
//       </table>

//       <h2>📄 Resumes</h2>
//       <table border="1" cellPadding="10" style={{ width: "100%" }}>
//         <thead>
//           <tr>
//             <th>ID</th>
//             <th>User ID</th>
//             <th>Title</th>
//             <th>Created At</th>
//           </tr>
//         </thead>
//         <tbody>
//           {resumes.length > 0 ? (
//             resumes.map((resume) => (
//               <tr key={resume.id}>
//                 <td>{resume.id}</td>
//                 <td>{resume.userId}</td>
//                 <td>{resume.title}</td>
//                 <td>{new Date(resume.createdAt).toLocaleString()}</td>
//               </tr>
//             ))
//           ) : (
//             <tr><td colSpan="4">No resumes found</td></tr>
//           )}
//         </tbody>
//       </table>
//     </div>
//   );
// };

// export default AdminDashboard;
