# ASP.NET Core Request Pipeline (Meridian API)

This document describes how an HTTP request flows through the Meridian API host (Kestrel) and how our middleware ordering works.

---

## High-level flow

Client
|
| (HTTP/HTTPS request)
v
Kestrel (server)
|
v
Middleware pipeline (top-to-bottom, in the order registered)
|
v
Endpoint selection + execution (Controllers / Minimal APIs)
|
v
Response (back through middleware, bottom-to-top)
|
v
Client

---

## Core principles

1. **Order matters**
   Middleware executes in the exact order it is registered in `Program.cs`.

2. **Short-circuiting exists**
   Any middleware can end the request early (redirect, return 401/403, return 404, etc.) and prevent later middleware from running.

3. **Outbound response flows in reverse**
   The response travels back up the pipeline, allowing earlier middleware to observe/modify it (e.g., logging, exception handling).

---

## Meridian API baseline pipeline (current)

Below is the intended ordering (conceptual):

1. **Exception handling**

- Establishes a global boundary so unhandled exceptions are converted into safe, consistent error responses (ProblemDetails).
- Must be early to catch failures from everything below.

2. **Swagger (development only)**

- Registers `/swagger` UI and `/swagger/v1/swagger.json`.
- Must be registered before any catch-all fallback endpoints (if added later).

3. **HTTPS redirection**

- Redirects HTTP → HTTPS to enforce encrypted transport.

4. **Routing**

- Matches the request to the correct endpoint (controller action).
- Provides endpoint metadata used by authorization policies.

5. **Authorization**

- Enforces access policies for the selected endpoint.
- **Important:** When authentication is added, `UseAuthentication()` must run **before** `UseAuthorization()`.

6. **Endpoints (controllers)**

- Executes the matched controller action.
- Produces the response (or throws an exception caught by the exception handler).

---

## Middleware ordering reference (recommended)

> When Authentication is added later, the critical ordering is:

- `UseRouting()`
- `UseAuthentication()` ✅ identity is established
- `UseAuthorization()` ✅ policies are evaluated
- `MapControllers()`

---

## Common failure modes (and what they mean)

### Swagger 404

- Swagger middleware not registered, or
- Swagger registered after a fallback/catch-all endpoint, or
- Wrong URL (`/swagger` vs `/swagger/index.html`)

### 401/403 confusion

- Authorization is enabled but Authentication is not configured, or
- `UseAuthentication()` is missing/misordered (must be before authorization), or
- Endpoint has `[Authorize]` but no auth scheme exists.

### HTTPS errors

- Using `https://` on an HTTP-only port, or
- Dev certificate issues (local), or
- Browser TLS cache after cert reset.

---

## Notes for future expansion

As the platform grows, additional middleware may be introduced (examples):

- CORS (browser clients)
- Rate limiting
- Response compression
- Structured request logging / correlation IDs
- Health checks endpoints and readiness probes
