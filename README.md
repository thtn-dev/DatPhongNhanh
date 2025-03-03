# DatPhongNhanh - Quick Room Booking System

![DatPhongNhanh Logo](https://via.placeholder.com/200x50)

## Overview

DatPhongNhanh ("Quick Booking" in English) is a modern, high-performance room booking platform built with ASP.NET (.NET 9) and React. The system provides a seamless experience for users to search, compare, and book accommodations with real-time availability and instant confirmation.

## Key Features

- **Fast and Responsive Booking**: Optimized for speed with Redis caching and efficient data processing
- **Secure Authentication**: Implements OpenID Connect and OAuth 2.0 standards for robust security
- **Real-time Availability**: Instant room status updates across all devices
- **Advanced Search**: Find accommodations by location, price, amenities, and more
- **Booking Management**: Easy modification and cancellation of reservations
- **Payment Integration**: Secure payment processing with multiple options
- **User Reviews and Ratings**: Transparent feedback system for quality assurance
- **Admin Dashboard**: Comprehensive management interface for property owners

## Technology Stack

### Backend
- **Framework**: ASP.NET Core (.NET 9)
- **Data Access**: 
  - Entity Framework Core for ORM
  - Dapper for high-performance queries
- **Mapping**: AutoMapper for object-to-object mapping
- **Validation**: FluentValidation for request validation
- **Scheduling**: Quartz.NET for background jobs and notifications
- **Caching**: Redis for high-performance data caching
- **Authentication**: 
  - OpenIdDict for OpenID Connect server implementation
  - OAuth 2.0 for secure authorization

### Frontend
- **Framework**: React with JavaScript/TypeScript
- **State Management**: Redux/Context API
- **UI Components**: Material UI/Ant Design
- **API Communication**: Axios/Fetch API

## Getting Started

### Prerequisites
- .NET 9 SDK
- Node.js (v16+)
- Redis Server
- SQL Server/PostgreSQL

### Installation

1. Clone the repository:
```bash
git clone https://github.com/yourusername/DatPhongNhanh.git
cd DatPhongNhanh
```

2. Set up the backend:
```bash
cd Backend
dotnet restore
dotnet build
```

3. Configure the database:
```bash
dotnet ef database update
```

4. Set up the frontend:
```bash
cd ../Frontend
npm install
```

5. Run the application:
```bash
# Terminal 1 (Backend)
cd Backend
dotnet run

# Terminal 2 (Frontend)
cd Frontend
npm start
```

6. Access the application:
- API: http://localhost:5000
- Web Client: http://localhost:3000

## Architecture

DatPhongNhanh follows a clean architecture pattern with clear separation of concerns:

- **API Layer**: RESTful endpoints for client communication
- **Service Layer**: Business logic implementation
- **Repository Layer**: Data access and persistence
- **Domain Layer**: Core business entities and rules

## Performance Optimization

- Redis caching for frequently accessed data
- Dapper for performance-critical database operations
- Asynchronous processing for non-blocking operations
- Optimized React rendering with proper state management

## Security Features

- JWT-based authentication
- Role-based access control
- HTTPS encryption
- Input validation and sanitization
- Protection against common web vulnerabilities (XSS, CSRF, SQL Injection)

## Deployment

The system supports deployment to various environments:

- **Development**: Local setup with debugging tools
- **Staging**: Pre-production environment for testing
- **Production**: Optimized for performance and security

### Deployment Options
- Docker containers
- Azure App Service
- AWS Elastic Beanstalk
- Traditional IIS hosting

## Roadmap

- Mobile application (React Native)
- AI-powered recommendations
- Advanced analytics dashboard
- Integration with more payment gateways
- Multi-language support

## Contributing

We welcome contributions! Please read our [Contributing Guide](CONTRIBUTING.md) for details on our code of conduct and the process for submitting pull requests.

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## Contact

For any inquiries or support, please contact us at support@datphongnhanh.com

---

© 2025 DatPhongNhanh. All rights reserved.
