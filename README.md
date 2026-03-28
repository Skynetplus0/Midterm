# Midterm – Short-Term Stay Booking Backend

Developer:İsmet Mete Baykasoğlu
Number:21070001017

Büyük Ölçekli Sistemler için Sistem Mimarisi

---------------------------------------------------------------------------
## Project Overview

This project is a backend system for a fictitious short-term stay platform similar to Airbnb.  
Hosts can create listings, guests can query available listings, book stays, and leave reviews, while administrators can generate listing reports and import listings from CSV files.

The project was developed as a RESTful backend API with versioned endpoints, JWT-based authentication, Swagger documentation, paging support, API gateway integration, and load testing.

---

### Demo Users for testing on deployed link
- `host@test.com / 123456`
- `guest@test.com / 123456`
- `admin@test.com / 123456`



## Repository Link

GitHub Repository:  
https://github.com/Skynetplus0/Midterm

---

## Deployed Swagger URLs

Main API Swagger:  
https://midterm-6wep.onrender.com/swagger/index.html

Gateway Swagger:  
https://midtermgateway.onrender.com/swagger/index.html

---

##Youtube Video Link
https://youtu.be/sEVK9SP7Rqc


## Project Structure

This solution contains multiple backend services:

- **StayBooking.Api**  
  Main API for:
  - authentication
  - host listing creation
  - guest listing search
  - booking
  - review
  - CSV import

- **StayBooking.AdminApi**  
  Separate API for:
  - admin reporting

- **StayBooking.Gateway**  
  API gateway for:
  - request routing
  - gateway-level rate limiting

- **load-tests**  
  k6 load test scripts

---

## Technologies Used

- .NET 8
- ASP.NET Core Web API
- Entity Framework Core
- JWT Authentication
- Swagger / OpenAPI
- API Versioning
- YARP Reverse Proxy Gateway
- k6 Load Testing
- Render (deployment)

---

## Architecture and Design

The system follows service-oriented and layered design principles:

- **Controllers** handle HTTP requests and responses
- **Services** contain business logic
- **Repositories** handle database access
- **DTOs** are used for request and response models
- **JWT** secures protected endpoints
- **Swagger** documents the APIs
- **Gateway** routes incoming requests to the correct API
- **API versioning** is implemented using `/api/v1/...`

This separation was chosen to satisfy the requirement of avoiding direct database logic inside controllers and to keep the codebase modular and maintainable.

---

## Assumptions

The following assumptions were made during development:

1. A booking can have only one review.
2. Only the guest who booked a stay can review that stay.
3. Listings with overlapping confirmed bookings are excluded from guest search results.
4. Payment processing is out of scope.
5. Listing search supports paging.
6. Listing search is limited in the main API business logic to **3 requests per day per client**.
7. Gateway-level rate limiting is also implemented.
8. A separate Admin API is used for reporting.
9. The same database structure is reused across the services for simplicity.

---

## Data Model (ER Summary)

### Users
- Id
- FullName
- Email
- PasswordHash
- Role
- CreatedAt

### Listings
- Id
- HostId
- Title
- Description
- NoOfPeople
- Country
- City
- Price
- IsActive
- CreatedAt

### Bookings
- Id
- ListingId
- GuestId
- FromDate
- ToDate
- PeopleNamesJson
- Status
- CreatedAt

### Reviews
- Id
- BookingId
- ListingId
- GuestId
- Rating
- Comment
- CreatedAt

### GuestQueryUsage
- Id
- ClientKey
- QueryDate
- Count

### Relationship Summary
- One **User** can create many **Listings**
- One **User** can create many **Bookings**
- One **User** can create many **Reviews**
- One **Listing** can have many **Bookings**
- One **Listing** can have many **Reviews**
- One **Booking** can have zero or one **Review**

---

## Main API Endpoints

### Authentication
- `POST /api/v1/auth/login`

### Host
- `POST /api/v1/hosts/listings`

### Guest
- `GET /api/v1/guests/listings/search`
- `POST /api/v1/guests/bookings`
- `POST /api/v1/guests/reviews`

### Admin
- `GET /api/v1/admin/reports/listings`
- `POST /api/v1/admin/listings/import`

---

## Authentication

JWT authentication is used for protected endpoints.

### Roles
- Host
- Guest
- Admin

### Demo Users
- `host@test.com / 123456`
- `guest@test.com / 123456`
- `admin@test.com / 123456`

---

## API Gateway

The project includes a gateway service to route incoming traffic to the appropriate backend service.

### Routing Purpose
- Main API requests are routed to the main backend API
- Admin report requests are routed to the separate Admin API

### Gateway Features
- reverse proxy
- centralized routing
- rate limiting

---

## Load Testing

Load testing was performed using **k6**.

### Endpoints Tested

1. **Query Listings**
   - `GET /api/v1/guests/listings/search`
   - Selected because it is a key public read endpoint and expected to receive frequent access.

2. **Book a Stay**
   - `POST /api/v1/guests/bookings`
   - Selected because it is a key write endpoint involving validation and database persistence.

### Load Test Scenarios

Each test was executed under three load scenarios:

- **Normal Load:** 20 virtual users for 30 seconds
- **Peak Load:** 50 virtual users for 30 seconds
- **Stress Load:** 100 virtual users for 30 seconds

---

## Load Test Scripts

The following k6 scripts are included in the repository:

- `load-tests/query-listings.js`
- `load-tests/book-stay.js`

---

## Load Test Results

### 1. Query Listings
Script: `query-listings.js`

#### Combined Result Summary
- **Average response time:** 289.24 ms
- **95th percentile response time (p95):** 822.79 ms
- **Requests per second:** 39.41 req/s
- **Error rate:** 0.00%

| Endpoint | Avg Response Time | p95 Response Time | Requests/sec | Error Rate |
|---------|-------------------|-------------------|--------------|------------|
| GET /api/v1/guests/listings/search | 289.24 ms | 822.79 ms | 39.41 | 0.00% |

### 2. Book a Stay
Script: `book-stay.js`

#### Combined Result Summary
- **Average response time:** 15.11 ms
- **95th percentile response time (p95):** 49.62 ms
- **Requests per second:** 50.07 req/s
- **Error rate:** 100.00%

| Endpoint | Avg Response Time | p95 Response Time | Requests/sec | Error Rate |
|---------|-------------------|-------------------|--------------|------------|
| POST /api/v1/guests/bookings | 15.11 ms | 49.62 ms | 50.07 | 100.00% |

### Interpretation of Booking Test Result

The booking endpoint returned a 100% failure rate in the executed test scenario.  
This does not necessarily indicate poor raw performance; instead, it indicates that the requests were rejected by application-level business rules or test data constraints.  
Typical causes include invalid/expired JWT token usage, booking date overlaps, capacity conflicts, or test payload reuse that caused every request to fail validation or availability checks.

---

## Load Test Analysis

The Query Listings endpoint performed well under normal, peak, and stress load, maintaining a low average response time and zero failed requests.  
Its p95 response time remained below the defined 2-second threshold, indicating acceptable responsiveness even under heavier concurrency.  
The Book a Stay endpoint showed very fast response times, but all requests failed, which suggests that the bottleneck was not processing speed but request validity or booking business-rule conflicts.  
To improve the realism and usefulness of booking load tests, unique booking windows, verified JWT tokens, and clean test listings should be used.  
Future scalability improvements may include stronger indexing, caching for read-heavy queries, and stricter separation of read and write workloads.

---

## Screenshots / Graphs

Load test screenshots should be added to the repository under a folder such as:

- `Load_Test_İmages/query-listings_k6-test.png`
- `Load_Test_İmages/book-stay-k6-loadtesting.png`

These screenshots should show:
- k6 terminal results
- average response time
- p95
- request throughput
- error rate

---

## Issues Encountered

Some implementation and deployment issues encountered during development:

1. Splitting responsibilities between multiple APIs while keeping a consistent authentication model.
2. Configuring gateway routing and rate limiting correctly.
3. Managing booking overlap rules during testing.
4. Ensuring Swagger worked correctly with JWT-protected endpoints.
5. Deployment platform constraints and configuration differences between local and hosted environments.

---

## Potential Improvements

- Add caching for listing search responses
- Add more database indexes for search and booking fields
- Isolate booking test data more carefully for load testing
- Improve gateway policies with more endpoint-specific rate limiting
- Separate databases per service in a more advanced microservice version

---

## Video Demo

https://youtu.be/sEVK9SP7Rqc


---






