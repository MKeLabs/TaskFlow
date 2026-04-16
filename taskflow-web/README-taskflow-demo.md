# TaskFlow SPA demo (Angular) – teaching notes

This Angular app is intentionally small: it demonstrates the minimum pieces you need to move from an MVC mindset (pages rendered by the server) to an SPA mindset (UI rendered on the client, backend is a JSON API).

## Run it

- Start the API (TaskFlow):
  - `dotnet run --project ..\\TaskFlow.API`
  - API base URL in development: `https://localhost:7226`
- Start Angular:
  - `npm install`
  - `npm start`
  - App URL: `http://localhost:4200`

The Angular dev server proxies `/api/*` to the ASP.NET Core API (see `proxy.conf.json`), so Angular calls `/api/...` without hardcoding the backend host.

## What to show students (MVC → SPA mental shift)

### 1) “The backend is no longer a page factory”
In MVC, controllers return HTML views. In an SPA, controllers return **JSON** and the browser renders the UI.

- **Backend**: `GET /api/projects` returns data.
- **Frontend**: `ProjectsComponent` renders a list.

### 2) Authentication becomes “attach a credential to API calls”
With JWT:
- Login is an API call: `POST /api/auth/login`
- The response contains an `accessToken`.
- The SPA stores the token (here: `localStorage`) and attaches it to API requests.

Files to point at:
- `src/app/core/auth/auth.service.ts` (login/register + token storage)
- `src/app/core/auth/auth.interceptor.ts` (adds `Authorization: Bearer ...` automatically)

### 3) Authorization in SPAs is two-layered
There are **two** protections:

- **Backend authorization** is the real gatekeeper: `[Authorize]` on controllers.
- **Frontend guard** is UX: it stops navigation and sends you to `/login`, but it is **not security** (users can still call the API directly).

Files to point at:
- `src/app/core/auth/auth.guard.ts` (route guard for `/projects`)
- `TaskFlow.API/Controllers/*Controller.cs` (the real `[Authorize]`)

### 4) Routing is now client-side
In MVC, URLs map to controller actions + views.
In Angular, URLs map to **components**.

File to point at:
- `src/app/app.routes.ts`

### 5) Debugging: show the request, not the screen
Have students open DevTools → Network and show:
- Login request → response JSON contains `accessToken`
- Subsequent `/api/projects` request includes `Authorization: Bearer ...`
- If the token is missing/expired, the API returns **401**

## Demo script (quick classroom flow)

1. Hit `/projects` while logged out → guard redirects to `/login`.
2. Log in → observe token stored (Application tab → Local Storage).
3. Navigate to `/projects` → list loads.
4. Delete token in Local Storage and refresh `/projects` → see 401 / redirect again.

## Notes / trade-offs you can discuss

- **Token storage**: `localStorage` is simple for teaching, but in real apps you also teach mitigation (short token lifetimes, refresh tokens, BFF pattern, etc.).
- **CORS vs proxy**: we use a dev proxy (`proxy.conf.json`) to keep the demo frictionless. For real deployment, configure CORS in the API.

