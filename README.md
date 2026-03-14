# Course Enrollment System

A full-stack course enrollment application built with Blazor WebAssembly and ASP.NET Core Web API, featuring JWT authentication, in-memory database, and complete CRUD operations.

## Tech Stack

- **Frontend**: Blazor WebAssembly
- **Backend**: ASP.NET Core Web API
- **Database**: Entity Framework Core (In-Memory)
- **Auth**: ASP.NET Core Identity + JWT Bearer Tokens
- **UI**: Bootstrap 5 + Bootstrap Icons

## Project Structure

```
CourseEnrollment/
├── API/                          # Backend Web API
│   ├── Controllers/
│   │   ├── AuthController.cs     # Registration & Login
│   │   ├── CoursesController.cs  # Course CRUD
│   │   └── EnrollmentsController.cs
│   ├── Data/
│   │   ├── ApplicationDbContext.cs
│   │   └── ApplicationUser.cs
│   └── Services/
│       └── TokenService.cs       # JWT token generation
├── Client/                       # Blazor WASM Frontend
│   ├── Pages/
│   │   ├── Home.razor
│   │   ├── Login.razor
│   │   ├── Register.razor
│   │   ├── AvailableCourses.razor
│   │   └── MyCourses.razor
│   ├── Services/
│   │   ├── AuthService.cs
│   │   ├── CourseService.cs
│   │   └── CustomAuthStateProvider.cs
│   └── Layout/
│       └── NavMenu.razor
└── Shared/                       # Shared DTOs and Models
    ├── Models/
    │   ├── Course.cs
    │   └── StudentCourse.cs
    └── DTOs/
        └── DTOs.cs
```

## Getting Started

### Prerequisites
- .NET 9.0 SDK

### Running the Application

**Terminal 1 - Start the API:**
```bash
cd CourseEnrollment/API
dotnet run
```
API runs at: `https://localhost:7000`

**Terminal 2 - Start the Client:**
```bash
cd CourseEnrollment/Client
dotnet run
```
Client runs at: `https://localhost:7001`

Open your browser and go to `https://localhost:7001`

## Features

- Student registration and login with JWT authentication
- Browse available courses
- Enroll in multiple courses (many-to-many relationship)
- View enrolled courses with enrollment dates
- Unenroll from courses
- Course capacity management
- Protected routes and API endpoints
- Responsive UI with animations

## API Endpoints

### Authentication
| Method | Endpoint | Description |
|--------|----------|-------------|
| POST | `/api/auth/register` | Register new student |
| POST | `/api/auth/login` | Login and get JWT token |

### Courses
| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/courses` | Get all courses |
| GET | `/api/courses/{id}` | Get course by ID |
| POST | `/api/courses` | Create new course |
| PUT | `/api/courses/{id}` | Update course |
| DELETE | `/api/courses/{id}` | Delete course |

### Enrollments
| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/enrollments/my-courses` | Get enrolled courses |
| GET | `/api/enrollments/available-courses` | Get available courses |
| POST | `/api/enrollments/enroll/{courseId}` | Enroll in a course |
| DELETE | `/api/enrollments/unenroll/{courseId}` | Unenroll from a course |

## Sample Data

The app comes pre-seeded with 5 courses:

| Course | Code | Credits | Max Students |
|--------|------|---------|-------------|
| Introduction to C# | CS101 | 3 | 30 |
| ASP.NET Core | CS201 | 4 | 25 |
| Blazor WebAssembly | CS301 | 4 | 20 |
| Entity Framework Core | CS202 | 3 | 25 |
| Azure Fundamentals | AZ100 | 3 | 35 |

## Architecture

### Authentication Flow
1. User registers or logs in
2. API validates credentials and generates JWT token
3. Client stores token in localStorage
4. Token is sent with every API request via Authorization header
5. API validates token on each request

### Database Design
- **ApplicationUser** - Extends IdentityUser with FirstName, LastName
- **Course** - Course details with capacity
- **StudentCourse** - Junction table for many-to-many relationship (StudentId + CourseId composite key)

### Security
- Password hashing with ASP.NET Core Identity
- JWT Bearer token authentication
- CORS policy restricted to client origin
- HTTPS enforcement
- Authorization on all protected endpoints

## Notes

- Uses in-memory database — data resets when the API restarts
- Password requirements: minimum 6 characters, at least 1 digit
- JWT tokens expire after 24 hours
- Swagger UI available at `https://localhost:7000/swagger`

## Author

Sam Moloi
