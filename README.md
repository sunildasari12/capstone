BackEnd
**ARCHITECTURE CORE FILES**

**Program.cs** - **Application Startup & Configuration**
**Purpose**: The main entry point that configures the entire application
**Key Functions**:
- Sets up dependency injection (services, repositories, database)
- Configures JWT authentication middleware
- Sets up CORS for React frontend communication
- Configures Swagger/OpenAPI documentation
- Defines middleware pipeline order

**How it helps**: This is the heart of your application that wires everything together and makes all components work in harmony.

**appsettings.json** - **Configuration Settings**
**Purpose**: Stores application configuration values
**Contains**:
- Database connection strings
- JWT secret key and issuer/audience settings
- Logging configuration
- Allowed hosts

**How it helps**: Centralizes configuration, makes deployment easier, and keeps secrets out of code.

---

**AUTHENTICATION & AUTHORIZATION FILES**

**AuthController.cs** - **User Registration & Login**
**Purpose**: Handles user authentication endpoints
**Endpoints**:
- `POST /api/auth/register` - Creates new user accounts
- `POST /api/auth/login` - Authenticates users and returns JWT tokens

**How it helps**: Provides secure user authentication using BCrypt hashing and JWT tokens.

**JwtService.cs** - **JWT Token Management**
**Purpose**: Generates and validates JWT tokens
**Functions**:
- Creates tokens with user claims (ID, email, role, name)
- Uses symmetric security key encryption

**How it helps**: Securely manages user sessions and enables stateless authentication.

**IJwtService.cs** - **JWT Service Contract**
**Purpose**: Interface defining token service methods
**How it helps**: Enables dependency injection and makes code testable.

---

**USER MANAGEMENT FILES**

**UserController.cs** - **User Profile Access**
**Purpose**: Allows users to view their own profile
**Endpoint**: `GET /api/users/{id}` - Gets user details (excluding password)

**How it helps**: Provides user profile functionality while maintaining security.

**AppUser.cs** - **User Model Definition**
**Purpose**: Defines the user data structure
**Properties**: ID, Name, Email, PasswordHash, Role, CreatedAt
**How it helps**: Represents users in the database with proper validation attributes.

**IUserRepository.cs** & **UserRepository.cs** - **User Data Access**
**Purpose**: Handles database operations for users
**Methods**: GetByEmail, GetById, Create, GetAll
**How it helps**: Separates data access logic from business logic.

---

**RESUME MANAGEMENT FILES**

**ResumeController.cs** - **Resume CRUD Operations**
**Purpose**: Main controller for resume management
**Endpoints**:
- `GET /api/resumes/my` - Get user's resumes
- `POST /api/resumes` - Create new resume
- `PUT /api/resumes/{id}` - Update resume
- PDF download functionality

**How it helps**: Provides complete resume management with education/experience sections.

**Resume.cs, Education.cs, Experience.cs** - **Data Models**
**Purpose**: Define the structure of resumes and their components
**How it helps**: Entity Framework uses these to create database tables and manage relationships.

**IResumeService.cs** & **ResumeService.cs** - **Resume Business Logic**
**Purpose**: Handles business operations for resumes
**How it helps**: Separates business rules from data access and presentation layers.

**IResumeRepository.cs** & **ResumeRepository.cs** - **Resume Data Access**
**Purpose**: Database operations for resumes
**How it helps**: Implements repository pattern for clean data access.

---

**AI INTEGRATION FILES**

**AiController.cs** - **AI-Powered Improvements**
**Purpose**: Provides AI enhancement endpoints
**Endpoints**:
- `POST /api/ai/improve-summary` - Enhances resume summaries
- `POST /api/ai/improve-experience` - Improves experience descriptions

**How it helps**: Adds intelligent content improvement features.

**IAiService.cs** & **AiService.cs** - **AI Service Layer**
**Purpose**: Interface and implementation for AI features
**Current**: Stubbed implementation (ready for OpenAI integration)
**How it helps**: Provides structure for integrating real AI APIs.

---

**ADMINISTRATION FILES**

**AdminController.cs** - **Administrative Functions**
**Purpose**: Admin-only operations
**Endpoints**:
- `GET /api/admin/users` - View all users
- `DELETE /api/admin/users/{id}` - Delete users
- `GET /api/admin/resumes` - View all resumes

**How it helps**: Provides system management capabilities for administrators.

---

**DATA LAYER FILES**

**ApplicationDbContext.cs** - **Database Context**
**Purpose**: Entity Framework database context
**Functions**:
- Defines DbSets for all entities
- Configures relationships and constraints
- Seeds initial admin user
- Sets up table mappings

**How it helps**: Bridges your C# code with the SQL database.

---

**DTO (DATA TRANSFER OBJECT) FILES**

**AuthDtos.cs** - **Authentication Data Transfer Objects**
**Contains**: RegisterDto, LoginDto, AuthResultDto
**How it helps**: Provides strongly-typed objects for API requests/responses.

**ResumeCreateDto.cs**, **ResumeUpdateDto.cs** - **Resume DTOs**
**Purpose**: Define data shapes for creating/updating resumes
**How it helps**: Separates API contracts from database models.

---

**OTHER CONTROLLERS**

**HomeController.cs** - **Public Landing Page**
**Purpose**: Provides public API information
**Endpoint**: `GET /api/home/homepage` - Public welcome message
**How it helps**: Gives guests information about the application.

**ExportController.cs** - **Export Functionality**
**Purpose**: Handles resume export (currently stubbed)
**How it helps**: Ready for PDF/other export format implementation.

---

**TEST/EXAMPLE FILES**

**WeatherForecastController.cs** - **Example Controller**
**Purpose**: Default ASP.NET Core template controller
**How it helps**: Provides example API endpoint structure.

---

**HOW THESE FILES WORK TOGETHER**

**Request Flow Example (User creates resume):**
1. **Frontend** → `AuthController` (login) → JWT token
2. **Frontend** (with token) → `ResumeController` (create resume)
3. **ResumeController** → `ResumeService` → `ResumeRepository`
4. **ResumeRepository** → `ApplicationDbContext` → SQL Database
5. **Response** flows back through the same layers

**Security Flow:**
1. JWT middleware validates tokens automatically
2. `[Authorize]` attributes protect endpoints
3. Role-based authorization (`[Authorize(Roles = "Admin")]`) restricts access
4. User ID from token ensures users only access their own data

**AI Integration Ready:**
- The `AiService` is structured to easily replace stubbed methods with real OpenAI API calls
- Clean separation makes AI integration a simple drop-in replacement

This architecture provides:

**Security** with proper authentication/authorization
**Scalability** with separated layers
**Maintainability** with clean code organization
**Extensibility** with interfaces and dependency injection
**Documentation** with Swagger/OpenAPI

The project is well-structured for both current functionality and future enhancements!

****FrontEnd****

**CORE CONFIGURATION FILES**

**axios.js** - **HTTP Client Configuration**
**Purpose**: Configures axios for API communication
**Key Features**:
- Sets base URL to backend API
- Automatically adds JWT token to all requests
- Handles 401 errors by redirecting to login
- Sets default content type

**How it helps**: Provides consistent, secure API communication throughout the app.

**App.js** - **Main Application Router**
**Purpose**: Defines all application routes and navigation structure
**Routes**:
- Public routes: Home, Login, Register
- Protected routes: Dashboard, Resume forms
- Role-based routing: Admin dashboard

**How it helps**: Manages navigation and controls access based on authentication.

---

**AUTHENTICATION FILES**

**Login.js** & **Register.js** - **User Authentication**
**Purpose**: Handle user login and registration
**Features**:
- Form validation and submission
- JWT token decoding to extract user role
- Role-based redirection (Admin → /admin, User → /dashboard)
- Beautiful CSS styling with gradients and animations

**How it helps**: Provides secure user authentication with proper role handling.

**Login.css** - **Authentication Styling**
**Purpose**: Beautiful styling for login/register forms
**Features**: Gradient backgrounds, animations, responsive design
**How it helps**: Creates professional-looking auth pages.

---

**SECURITY & ROUTING FILES**

**ProtectedRoute.js** - **Route Protection**
**Purpose**: Guards routes that require authentication
**Features**:
- Checks for JWT token presence
- Optional role-based protection (for admin routes)
- Redirects unauthorized users appropriately

**How it helps**: Ensures only authenticated users can access protected content.

**Navbar.js** - **Navigation Component**
**Purpose**: Provides navigation with role-based links
**Features**:
- Shows different links for Admin vs User roles
- Handles logout functionality
- Dynamic navigation based on authentication state

**How it helps**: Creates intuitive navigation that adapts to user roles.

---

**RESUME MANAGEMENT FILES**

**resumeApi.js** - **API Service Layer**
**Purpose**: Contains all API calls for resume operations
**Endpoints**:
- `getMyResumes()` - Get user's resumes
- `createResume()` - Create new resume
- `updateResume()` - Edit existing resume
- `downloadResumePdf()` - Download as PDF
- Admin endpoints: `getUsers()`, `deleteUser()`, `getResumes()`

**How it helps**: Centralizes all API communication for clean, maintainable code.

**ResumeForm.js** - **Resume Creation/Editing**
**Purpose**: Form for creating and editing resumes
**Features**:
- Dynamic form for education and experience sections
- Handles both create and edit modes
- Real-time form state management
- Support for multiple education/experience entries

**How it helps**: Provides comprehensive resume editing interface.

**Dashboard.js** - **User Dashboard**
**Purpose**: Displays user's resumes and actions
**Features**:
- Lists all user resumes
- Edit, delete, and download options
- Create new resume button
- Clean card-based layout

**How it helps**: Main workspace for users to manage their resumes.

**Dashboard.css** - **Dashboard Styling**
**Purpose**: Modern, responsive styling for dashboard
**Features**: Card layouts, hover effects, responsive grid
**How it helps**: Creates professional user interface.

---

**ADMINISTRATION FILES**

**AdminDashboard.js** - **Admin Management Interface**
**Purpose**: Admin-only user and resume management
**Features**:
- User management table with delete functionality
- Resume overview table
- Protection against deleting admin account
- Error handling and loading states

**How it helps**: Provides admin tools for system management.

---

**PAGE COMPONENTS**

**Home.js** - **Landing Page**
**Purpose**: Simple welcome page
**How it helps**: Public-facing introduction to the application.

**ResumePreview.js** - **Resume Preview**
**Purpose**: Preview functionality (currently basic)
**How it helps**: Could be extended for live resume preview.

---

**STYLING FILES**

**style.css** - **Global Styles**
**Purpose**: Base styling for the entire application
**Features**: Navigation styling, form styles, basic layout
**How it helps**: Provides consistent look and feel across the app.

---

**HOW THESE FILES WORK TOGETHER**

**User Flow Example (Create Resume):**
1. **User logs in** → `Login.js` → receives JWT token → redirected to `Dashboard.js`
2. **User clicks "Create Resume"** → navigates to `ResumeForm.js`
3. **User fills form** → submits → `resumeApi.js` sends to backend
4. **Backend processes** → returns success → redirects to `Dashboard.js`
5. **Dashboard fetches** updated resumes via `resumeApi.js`

**Authentication Flow:**
1. **Login/Register** → API call → JWT token stored
2. **Token decoded** → role extracted → stored in localStorage
3. **ProtectedRoute** checks token/role → grants or denies access
4. **Navbar** displays appropriate links based on role
5. **axios interceptors** automatically add token to requests

**Admin Flow:**
1. **Admin logs in** → role detected → redirected to `AdminDashboard.js`
2. **AdminDashboard** fetches users/resumes via admin API endpoints
3. **Admin can manage** users and view all system resumes

---

**KEY FEATURES ENABLED**

**1. Role-Based Access Control**
- Users see appropriate dashboards based on role
- Admin gets system management capabilities
- Protected routes enforce authentication

**2. Complete CRUD Operations**
- Create, read, update, delete resumes
- Dynamic form handling for complex data structures
- PDF download functionality

**3. Professional UI/UX**
- Modern, responsive design
- Beautiful authentication pages
- Intuitive navigation
- Loading states and error handling

**4. Security**
- JWT token management
- Automatic token attachment to requests
- Protected routes
- Secure API communication

**5. Scalable Architecture**
- Separated API layer
- Component-based structure
- Reusable protected route component
- Centralized styling

---

**RESPONSIVE DESIGN**
The CSS files ensure the application works well on:
- Desktop computers
- Tablets
- Mobile devices

---

**READY FOR ENHANCEMENTS**

**Current Strengths:**
 Complete authentication system
 Full resume management
 Admin functionality
 Professional styling
 Error handling
 Responsive design

**Potential Enhancements:**
1. **Live Preview**: Enhance `ResumePreview.js` for real-time preview
2. **AI Integration**: Connect to actual AI services
3. **More Export Formats**: Add Word/HTML export
4. **Templates**: Resume template selection
5. **Collaboration**: Multi-user resume editing

This frontend architecture provides a **professional, secure, and scalable** user interface that perfectly complements your backend API! 
