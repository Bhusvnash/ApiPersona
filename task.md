# ApiPersonas — Tareas del Proyecto
> Última revisión: 2026-10-01

---

##  Completado

### Models
- [x] Eliminada clase `Usuario` — reemplazada por DTOs
- [x] `GetUsuario(string Nombre, string Pass)` — record para recibir datos en requests (POST/PUT)
- [x] `SendUsuario(long Id, string Nombre)` — record para responder en requests (GET), sin exponer la contraseña

### Interface `IUsuarioRepository`
- [x] `GetAllAsync()` → `List<SendUsuario>`
- [x] `GetByIdAsync(id)` → `SendUsuario?`
- [x] `CreateAsync(GetUsuario)` → `(bool, long?)`
- [x] `UpdateAsync(long id, GetUsuario)` → `bool`
- [x] `DeleteAsync(id)` → `bool`
- [X] `GetByNombreAsync(string nombre)` → `long? id`
- [x] `GetPassAsync(long id)` → `string?` (hash almacenado)

### Repositorios (MySQL + SqlServer)
- [x] `MysqlUsuarioRepository` — implementa todos los métodos de `IUsuarioRepository`
- [x] `SqlServerUsuarioRepository` — implementa todos los métodos de `IUsuarioRepository`
- [x] Hashing de contraseña en `CreateAsync` con `Encoder.HashPassword()` *(MySQL y SqlServer)*
- [x] **Bug fix:** `AddWithValue("@telefono", ...)` corregido a `AddWithValue("@pass", ...)` en ambos repositorios
- [x] **Bug fix:** `UpdateAsync` en `SqlServerUsuarioRepository` — parámetro `@id` correctamente agregado
- [x] Implementa  `GetByNombreAsync` en ambos repositorios
- [x] Implementar `GetPassAsync` en ambos repositorios

### Service `Encoder`
- [x] `HashPassword(string password)` → `string` (BCrypt)
- [x] `VerifyPassword(string password, string hash)` → `bool` (BCrypt)

### Controller `UsuarioController` (`/usuario`)
- [x] `GET    /usuario`       → `[{}]`  lista todos los usuarios (retorna `SendUsuario[]`)
- [x] `GET    /usuario/{id}`  → `{}`    obtiene usuario por id (retorna `SendUsuario`)
- [x] `POST   /usuario`       → crea usuario — recibe `GetUsuario`, hashea pass, retorna `SendUsuario`
- [x] `PUT    /usuario/{id}`  → edita usuario — recibe `GetUsuario`, hashea pass
- [x] `DELETE /usuario/{id}`  → elimina usuario
- [x] Try-catch en todos los endpoints con respuestas HTTP apropiadas (`400`, `404`, `500`)

### Controller `AuthController`
- [x] `GET /login` → sirve `wwwroot/auth/login.html`

### Program.cs / Infraestructura
- [x] DI: `IPersonaRepository` → `MysqlPersonaRepository`
- [x] DI: `IUsuarioRepository` → `MysqlUsuarioRepository`
- [x] Middleware de logging en consola con colores (método, ruta, tiempo, status)
- [x] CORS abierto para desarrollo (`AllowAnyOrigin`)
- [x] Archivos estáticos habilitados (`UseStaticFiles`)

### Hashing
- [x] `Encoder.HashPassword()` generación de hash BCrypt

---

##  Pendiente

### Auth — lógica de login
- [ ] Corregir `AuthController` para inyectar `IUsuarioRepository` (actualmente usa `IPersonaRepository`)
- [ ] Validar credenciales con `Encoder.VerifyPassword()`
- [ ] `POST /auth/login` → recibe `GetUsuario { nombre, pass }`, retorna JWT token
- [ ] Instalar paquete `Microsoft.AspNetCore.Authentication.JwtBearer`
- [ ] Generar y retornar JWT tras login exitoso

### Endpoints adicionales
- [ ] `GET  /auth/usuarios/all` → lista todos los usuarios (ruta con prefijo `/auth`)
- [ ] `POST /auth/register`     → registra usuario y retorna token

### Front
- [ ] Página `wwwroot/auth/login.html` — formulario de login
- [ ] Página `wwwroot/app/` — aplicación principal

---

##  Bugs Resueltos

| # | Archivo | Problema | Estado |
|---|---------|----------|--------|
| 1 | `MysqlUsuarioRepository.cs` | `AddWithValue("@telefono", ...)` en `CreateAsync` — parámetro SQL incorrecto |  Corregido |
| 2 | `SqlServerUsuarioRepository.cs` | `AddWithValue("@telefono", ...)` en `CreateAsync` — parámetro SQL incorrecto |  Corregido |
| 3 | `SqlServerUsuarioRepository.cs` | `UpdateAsync` no enviaba parámetro `@id` en la query |  Corregido |
| 4 | `UsuarioController.cs` | `CreatedAtAction` con sintaxis rota `new { Id= }` |  Corregido |
| 5 | `UsuarioController.cs` | `PUT` no recibía `id` como argumento de ruta |  Corregido |
| 6 | `SqlServerUsuarioRepository.cs` | `CreateAsync` no hasheaba la contraseña (inconsistente con MySQL) |  Corregido |

##  Bugs Pendientes

| # | Archivo | Problema |
|---|---------|----------|
| 1 | `AuthController.cs` | Inyecta `IPersonaRepository` en lugar de `IUsuarioRepository` para el login |

---

##  Orden sugerido de ejecución

| Paso | Tarea | Bloque |
|:----:|-------|--------|
| X | Corregir bugs de parámetros SQL en repositorios | Repositorios |
| X | Implementar DTOs `GetUsuario` / `SendUsuario` | Models |
| X | Actualizar `IUsuarioRepository` y repositorios para los nuevos DTOs | Interface + Repos |
| X | Agregar `PUT /usuario/{id}` y `DELETE /usuario/{id}` al controller | Controller |
| X | Agregar try-catch en todos los endpoints | Controller |
| X | Agregar `GetByNombreAsync` y `GetPassAsync` a `IUsuarioRepository` | Interface |
| 2 | Implementar los nuevos métodos en MySQL y SqlServer | Repositorios |
| 3 | Corregir `AuthController` para inyectar `IUsuarioRepository` | Auth |
| 4 | Implementar validación de credenciales con `Encoder.VerifyPassword` | Auth |
| 5 | Instalar paquete JWT y configurar en `Program.cs` | Auth |
| 6 | Implementar generación y retorno de JWT en `POST /auth/login` | Auth |
| 7 | Crear página de login en `wwwroot/auth/login.html` | Front |
| 8 | Crear página de app en `wwwroot/app/` | Front |