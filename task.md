# ApiPersonas — Tareas del Proyecto
> Última revisión: 2026-09-30

---

## ✅ Completado

### Interface `IUsuarioRepository`
- [x] `GetAllAsync()` → `List<Usuario>`
- [x] `GetByIdAsync(id)` → `Usuario?`
- [x] `CreateAsync(DtoUsuario)` → `bool`
- [x] `UpdateAsync(Usuario)` → `bool`
- [x] `DeleteAsync(id)` → `bool`

### Repositorios (MySQL + SqlServer)
- [x] `MysqlUsuarioRepository` — implementa todos los métodos de `IUsuarioRepository`
- [x] `SqlServerUsuarioRepository` — implementa todos los métodos de `IUsuarioRepository`
- [x] Hashing de contraseña en `CreateAsync` con `Encoder.HashPassword()` *(solo MySQL por ahora)*

### Service `Encoder`
- [x] `HashPassword(string password)` → `string` (BCrypt)
- [x] `VerifyPassword(string password, string hash)` → `bool` (BCrypt)

### Controller `UsuarioController` (`/usuario`)
- [x] `GET  /usuario`       → `[{}]`  lista todos los usuarios
- [x] `GET  /usuario/{id}`  → `{}`    obtiene usuario por id
- [x] `POST /usuario`       → `bool`  crea usuario (con hash de pass)

- [X] `DELETE /usuario/{id}` → eliminar usuario
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
### Controller `UsuarioController` (`/usuario`)
- [] `PUT /usuario/{id}`    → editar usuario (con re-hash de pass si cambia)
### Interface `IUsuarioRepository` — métodos de Auth
- [ ] `GetByNombreAsync(string nombre)` → `long? id`
- [ ] `GetPassAsync(long id)` → `string?` (hash almacenado)

### Repositorios — nuevos métodos
- [ ] Implementar `GetByNombreAsync` en `MysqlUsuarioRepository`
- [ ] Implementar `GetByNombreAsync` en `SqlServerUsuarioRepository`
- [ ] Implementar `GetPassAsync` en `MysqlUsuarioRepository`
- [ ] Implementar `GetPassAsync` en `SqlServerUsuarioRepository`
- [ ] `SqlServerUsuarioRepository.CreateAsync` — agregar hash de contraseña

### Auth — lógica de login
- [ ] Corregir `AuthController` para inyectar `IUsuarioRepository` (actualmente usa `IPersonaRepository`)
- [ ] Validar credenciales con `Encoder.VerifyPassword()`
- [ ] `POST /auth/login` → recibe `{ nombre, pass }`, retorna JWT token
- [ ] Instalar paquete `Microsoft.AspNetCore.Authentication.JwtBearer`
- [ ] Generar y retornar JWT tras login exitoso

### Endpoints adicionales
- [ ] `GET  /auth/usuarios/all` → lista todos los usuarios (ruta con prefijo `/auth`)
- [ ] `POST /auth/register`     → registra usuario y retorna token

### Front
- [ ] Página `wwwroot/auth/login.html` — formulario de login
- [ ] Página `wwwroot/app/` — aplicación principal

---

## ⚠️ Bugs / Inconsistencias detectadas

| # | Archivo | Línea | Problema |
|---|---------|-------|---------|
| 1 | `MysqlUsuarioRepository.cs` | L20 | Query usa tabla `usuarios` (plural); el resto usa `usuario` (singular) — verificar nombre real en BD |
| 2 | `MysqlUsuarioRepository.cs` | L56 | `AddWithValue("id", @id)` — falta el `@` en el nombre del parámetro, debe ser `"@id"` |
| 3 | `SqlServerUsuarioRepository.cs` | L100-103 | `UpdateAsync` no agrega el parámetro `@id`, la query fallaría en runtime |
| 4 | `AuthController.cs` | L15-20 | Inyecta `IPersonaRepository` en lugar de `IUsuarioRepository` para el login |
| 5 | `SqlServerUsuarioRepository.cs` | L71-89 | `CreateAsync` no hashea la contraseña (inconsistente con MySQL) |

---

##  Orden sugerido de ejecución

| Paso | Tarea | Bloque |
|:----:|-------|--------|
| X | Corregir bugs de parámetros SQL (`@id`) en repositorios | Repositorios |
| 2 | Agregar `GetByNombreAsync` y `GetPassAsync` a `IUsuarioRepository` | Interface |
| 3 | Implementar los nuevos métodos en MySQL y SqlServer | Repositorios |
| 4 | Agregar `PUT /usuario/{id}` y `DELETE /usuario/{id}` al controller | Controller |
| 5 | Corregir `AuthController` para inyectar `IUsuarioRepository` | Auth |
| 6 | Implementar validación de credenciales con `Encoder.VerifyPassword` | Auth |
| 7 | Instalar paquete JWT y configurar en `Program.cs` | Auth |
| 8 | Implementar generación y retorno de JWT en `POST /auth/login` | Auth |
| 9 | Crear página de login en `wwwroot/auth/login.html` | Front |
| 10 | Crear página de app en `wwwroot/app/` | Front |
    