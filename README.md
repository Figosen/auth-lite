# auth-lite
A custom C# Identity Provider built from scratch to master auth internals. Implements manual JWT creation (Access, ID &amp; Refresh tokens), a BFF pattern with HttpOnly cookies, cryptographic DPoP binding, and an OAuth 2.1 Authorization Code Flow with PKCE for deep security understanding.

## Roadmap / Project Phases
<!-- ![Hihi](https://img.shields.io/badge/status-COMPLETED-succes) -->
### 1. Basic Auth & Manual Token Generation ![Hihi](https://img.shields.io/badge/status-IN_PROGRESS-blue)
- **Goal:** Manual JWT construction without external libraries (`Header.Payload.Signature`).
- **Endpoints:** `/api/login` and `/api/refresh`.
- **Tokens:** Generation of **Access Token** (authorization), **ID Token** (authentication), and **Refresh Token** (rotation/db storage).

### 2. Basic Authorization & Custom Middleware ![Hihi](https://img.shields.io/badge/status-TODO-inactive)
- **Goal:** Access control based on claims (`sub`, `role`) and expiration time (`exp`).
- **Execution:** Build your own custom middleware for validation and attaching `ClaimsPrincipal` to `HttpContext`.

### 3. BFF Pattern & HttpOnly Cookies ![Hihi](https://img.shields.io/badge/status-TODO-inactive)
- **Goal:** Protection against XSS by removing tokens from `localStorage`.
- **Execution:** Set tokens via `HttpOnly`, `Secure`, and `SameSite=Strict` cookies that are automatically attached to API calls.

### 4. DPoP (Demonstrating Proof-of-Possession) ![Hihi](https://img.shields.io/badge/status-TODO-inactive)
- **Goal:** Cryptographic binding of tokens to the client to prevent token replaying upon leaks.
- **Execution:** The client generates an RSA/ECDSA key pair and sends a signed DPoP header with every API call.

### 5. Identity Provider (OAuth 2.1 & PKCE) ![Hihi](https://img.shields.io/badge/status-TODO-inactive)
- **Goal:** Fully functional mini-authorization server.
- **`/authorize`:** Receives `client_id`, `redirect_uri`, and `code_challenge` (PKCE) and issues a one-time `authorization_code`.
- **`/token`:** Verifies `code_verifier` against `code_challenge` and issues the final tokens.
* **Mål:** Fuldgyldig mini-authorization server.
* **`/authorize`:** Modtager `client_id`, `redirect_uri` og `code_challenge` (PKCE) og udsteder en engangs `authorization_code`.
* **`/token`:** Verificerer `code_verifier` mod `code_challenge` og udsteder de endelige tokens.