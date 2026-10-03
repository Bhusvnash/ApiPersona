# ApiPersonas -- Tareas del Proyecto
> Ultima revision: 2026-10-03

---

## Completado

### Models
- [x] Eliminada clase `Usuario` -- reemplazada por DTOs
- [x] `GetUsuario(string Nombre, string Pass)` -- record para recibir datos en requests (POST/PUT)
- [x] `SendUsuario(long Id, string Nombre)` -- record para responder en requests (GET), sin exponer la contrasena

### Interface `IUsuarioRepository`
- [x] `GetAllAsync()` -> `List<SendUsuario>`
- [x] `GetByIdAsync(id)` -> `SendUsuario?`
- [x] `CreateAsync(GetUsuario)` -> `(bool, long?)`
- [x] `UpdateAsync(long id, GetUsuario)` -> `bool`
- [x] `DeleteAsync(id)` -> `bool`
- [x] `GetByNombreAsync(string nombre)` -> `long? id`
- [x] `GetPassAsync(long id)` -> `string?` (hash almacenado)

### Repositorios (MySQL + SqlServer)
- [x] `MysqlUsuarioRepository` -- implementa todos los metodos de `IUsuarioRepository`
- [x] `SqlServerUsuarioRepository` -- implementa todos los metodos de `IUsuarioRepository`
- [x] Hashing de contrasena en `CreateAsync` con `Encoder.HashPassword()` *(MySQL y SqlServer)*
- [x] **Bug fix:** `AddWithValue("@telefono", ...)` corregido a `AddWithValue("@pass", ...)` en ambos repositorios
- [x] **Bug fix:** `UpdateAsync` en `SqlServerUsuarioRepository` -- parametro `@id` correctamente agregado
- [x] Implementar `GetByNombreAsync` en ambos repositorios
- [x] Implementar `GetPassAsync` en ambos repositorios

### Service `Encoder`
- [x] `HashPassword(string password)` -> `string` (BCrypt)
- [x] `VerifyPassword(string password, string hash)` -> `bool` (BCrypt)

### Controller `UsuarioController` (`/usuario`)
- [x] `GET    /usuario`       -> `[{}]`  lista todos los usuarios (retorna `SendUsuario[]`)
- [x] `GET    /usuario/{id}`  -> `{}`    obtiene usuario por id (retorna `SendUsuario`)
- [x] `POST   /usuario`       -> crea usuario -- recibe `GetUsuario`, hashea pass, retorna `SendUsuario`
- [x] `PUT    /usuario/{id}`  -> edita usuario -- recibe `GetUsuario`, hashea pass
- [x] `DELETE /usuario/{id}`  -> elimina usuario
- [x] Try-catch en todos los endpoints con respuestas HTTP apropiadas (`400`, `404`, `500`)

### Controller `AuthController`
- [x] Inyeccion corregida: ahora inyecta `IUsuarioRepository` (ya no `IPersonaRepository`)
- [x] `POST /auth/login` -> recibe `GetUsuario { nombre, pass }`, valida credenciales con `Encoder.VerifyPassword`
- [x] `GET /` -> redirige a `/auth/login.html`

### Program.cs / Infraestructura
- [x] DI: `IPersonaRepository` -> `MysqlPersonaRepository`
- [x] DI: `IUsuarioRepository` -> `MysqlUsuarioRepository`
- [x] Middleware de logging en consola con colores (metodo, ruta, tiempo, status)
- [x] CORS abierto para desarrollo (`AllowAnyOrigin`)
- [x] Archivos estaticos habilitados (`UseStaticFiles`)

### Hashing
- [x] `Encoder.HashPassword()` generacion de hash BCrypt
- [x] `Encoder.VerifyPassword()` verificacion de contrasena contra hash

---

## Pendiente

### Auth -- Implementacion de JWT

#### Que es JWT

JSON Web Token (JWT) es un estandar abierto (RFC 7519) que define un formato compacto y autocontenido para transmitir informacion entre dos partes como un objeto JSON firmado digitalmente. Un token JWT se compone de tres partes separadas por puntos: `Header.Payload.Signature`.

- **Header**: Indica el algoritmo de firma (ej. `HS256`) y el tipo de token (`JWT`).
- **Payload**: Contiene los "claims" (datos del usuario, roles, tiempo de expiracion).
- **Signature**: Garantiza que el token no ha sido alterado, firmado con una clave secreta.

#### Pasos para implementar JWT en este proyecto

**Paso 1 -- Instalar el paquete NuGet:**
```bash
dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer
```

**Paso 2 -- Configurar las claves en `appsettings.json`:**
Agregar una seccion `Jwt` con la clave secreta, emisor, audiencia y tiempo de expiracion. La clave secreta debe tener al menos 32 caracteres para HS256.
```json
{
  "Jwt": {
    "Key": "CLAVE_SECRETA_DE_AL_MENOS_32_CARACTERES",
    "Issuer": "ApiPersonas",
    "Audience": "ApiPersonasClients",
    "ExpireMinutes": 60
  }
}
```

**Paso 3 -- Registrar la autenticacion JWT en `Program.cs`:**
Antes de `builder.Build()`, configurar el esquema de autenticacion con `AddAuthentication` y `AddJwtBearer`, leyendo las opciones desde `IConfiguration`. Validar: `IssuerSigningKey`, `ValidIssuer`, `ValidAudience`, `ValidateLifetime`.

**Paso 4 -- Agregar middleware en el pipeline:**
Agregar `app.UseAuthentication()` **antes** de `app.UseAuthorization()` en el pipeline de middleware de `Program.cs`.

**Paso 5 -- Crear un servicio `JwtService`:**
Crear una clase en `services/` que genere tokens JWT a partir de un `SendUsuario`. Utilizar `System.IdentityModel.Tokens.Jwt` para crear un `JwtSecurityToken` con los claims del usuario (id, nombre), la clave de firma y la expiracion.

**Paso 6 -- Modificar `AuthController.Login`:**
Tras la verificacion exitosa de contrasena, en lugar de retornar solo `{ Mensaje: "Login exitoso" }`, invocar `JwtService` para generar el token y retornarlo en la respuesta:
```json
{ "token": "eyJhbGciOiJIUzI1NiIs..." }
```

**Paso 7 -- Proteger endpoints con `[Authorize]`:**
Agregar el atributo `[Authorize]` a los controladores o acciones que requieran autenticacion (ej. `PersonaController`, `UsuarioController`). Dejar `AuthController.Login` y `POST /usuario` (registro) sin proteccion.

#### Tareas JWT (checklist)
- [ ] Instalar paquete `Microsoft.AspNetCore.Authentication.JwtBearer`
- [ ] Agregar seccion `Jwt` en `appsettings.json` con Key, Issuer, Audience, ExpireMinutes
- [ ] Configurar `AddAuthentication` + `AddJwtBearer` en `Program.cs`
- [ ] Agregar `app.UseAuthentication()` antes de `app.UseAuthorization()` en el pipeline
- [ ] Crear servicio `services/JwtService.cs` para generar tokens
- [ ] Modificar `AuthController.Login` para retornar el token JWT tras login exitoso
- [ ] Agregar `[Authorize]` a `PersonaController` y `UsuarioController`
- [ ] (Opcional) Endpoint `POST /auth/register` para registro publico con retorno de token

### Endpoints adicionales
- [ ] `POST /auth/register` -> registra usuario y retorna token JWT

### Cadenas de conexion
- [ ] Mover las cadenas de conexion hardcodeadas de `MysqlConnection` y `sqlServerConnection` a `appsettings.json`
- [ ] Inyectar `IConfiguration` en los servicios de conexion via DI

### Front
- [ ] Pagina `wwwroot/auth/login.html` -- formulario de login funcional
- [ ] Logica JS de login: `fetch POST /auth/login`, guardar JWT en `localStorage`, redirigir a `/index`
- [ ] Enviar cabecera `Authorization: Bearer <token>` en las peticiones protegidas desde el frontend
- [ ] Pagina `wwwroot/app/` -- aplicacion principal

---

## Bugs Resueltos

| # | Archivo | Problema | Estado |
|---|---------|----------|--------|
| 1 | `MysqlUsuarioRepository.cs` | `AddWithValue("@telefono", ...)` en `CreateAsync` -- parametro SQL incorrecto | Corregido |
| 2 | `SqlServerUsuarioRepository.cs` | `AddWithValue("@telefono", ...)` en `CreateAsync` -- parametro SQL incorrecto | Corregido |
| 3 | `SqlServerUsuarioRepository.cs` | `UpdateAsync` no enviaba parametro `@id` en la query | Corregido |
| 4 | `UsuarioController.cs` | `CreatedAtAction` con sintaxis rota `new { Id= }` | Corregido |
| 5 | `UsuarioController.cs` | `PUT` no recibia `id` como argumento de ruta | Corregido |
| 6 | `SqlServerUsuarioRepository.cs` | `CreateAsync` no hasheaba la contrasena (inconsistente con MySQL) | Corregido |
| 7 | `AuthController.cs` | Inyectaba `IPersonaRepository` en lugar de `IUsuarioRepository` para el login | Corregido |

## Bugs Pendientes

| # | Archivo | Problema | Prioridad |
|---|---------|----------|-----------|
| 1 | `MysqlUsuarioRepository.cs` | `GetByNombreAsync` usa parametro `@id` en vez de `@nombre` en `AddWithValue` (linea 143). La query dice `WHERE nombre = @nombre` pero se asigna `@id`. Esto provoca que el login falle en MySQL. | CRITICA |

---

## Tareas de Mejora: Manejo de Errores

### Prioridad Alta
- [ ] **`PersonaController`**: No tiene try-catch en ningun endpoint. Una excepcion de base de datos propagaria un error 500 generico sin informacion util. Agregar manejo de excepciones consistente como en `UsuarioController`.
- [ ] **`AuthController.Login`**: No tiene try-catch. Si la base de datos no responde, la excepcion se propaga sin control. Envolver en try-catch y retornar `500` con mensaje generico.
- [ ] **`GetPassAsync` (ambos repos)**: Si `ExecuteScalarAsync` retorna `null` (usuario sin contrasena o inexistente), `Convert.ToString` devuelve `""` (cadena vacia). `VerifyPassword("", hash)` no lanzara excepcion pero compararia contra un string vacio. Mejorar: retornar `string?` y validar null en el controller.

### Prioridad Media
- [ ] **Validacion de entrada en `UsuarioController.Create`**: No valida que `Nombre` y `Pass` no esten vacios o nulos antes de hashear. Si `Pass` es null, `BCrypt.HashPassword(null)` lanza `ArgumentNullException`.
- [ ] **Validacion de entrada en `UsuarioController.Update`**: Mismo problema que `Create`. No valida campos vacios.
- [ ] **Respuestas de error inconsistentes**: `UsuarioController` retorna `StatusCode(400, ...)` en lugar de `BadRequest(...)` en `Update` y `Delete`. `PersonaController` retorna `BadRequest()` y `NotFound()` sin cuerpo. Estandarizar el formato de error en toda la API.
- [ ] **Codigos HTTP incorrectos**: En `UsuarioController.Update` y `Delete`, cuando el recurso no se encuentra, se retorna `400 Bad Request` en vez de `404 Not Found`. Separar la logica: verificar existencia con `GetByIdAsync` antes de operar.

### Prioridad Baja
- [ ] **Usings innecesarios**: `SqlServerUsuarioRepository.cs` importa `Microsoft.VisualBasic` y `Org.BouncyCastle.Crypto.Operators` que no se utilizan. `MysqlUsuarioRepository.cs` importa `MySqlX.XDevAPI` sin uso. Eliminar para mantener el codigo limpio.
- [ ] **Nomenclatura inconsistente**: El archivo de la interfaz se llama `IUsarioRepository.cs` (falta una 'u' en 'Usuario'). La clase `sqlServerConnection` no sigue PascalCase (`SqlServerConnection`). El directorio `services` deberia ser `Services` para consistencia con el resto del proyecto.

---

## Orden sugerido de ejecucion

| Paso | Tarea | Bloque |
|:----:|-------|--------|
| 1 | Corregir bug critico: parametro `@id` -> `@nombre` en `MysqlUsuarioRepository.GetByNombreAsync` | Bug Fix |
| 2 | Agregar try-catch a `PersonaController` y `AuthController` | Errores |
| 3 | Agregar validacion de campos vacios/nulos en `UsuarioController` Create y Update | Errores |
| 4 | Estandarizar formato de respuestas de error en todos los controllers | Errores |
| 5 | Mover cadenas de conexion a `appsettings.json` | Infraestructura |
| 6 | Instalar paquete JWT y configurar autenticacion en `Program.cs` | Auth/JWT |
| 7 | Crear `JwtService` para generacion de tokens | Auth/JWT |
| 8 | Modificar `AuthController.Login` para retornar JWT | Auth/JWT |
| 9 | Agregar `[Authorize]` a controladores protegidos | Auth/JWT |
| 10 | Crear pagina de login funcional en `wwwroot/auth/login.html` | Front |
| 11 | Implementar logica JS de login con almacenamiento de token | Front |

---

## Evaluacion de Seguridad y Arquitectura

### Seguridad

**Estado actual: en progreso, con riesgos criticos por resolver.**

Lo que esta bien:
- Las contrasenas se almacenan como hashes BCrypt, nunca en texto plano.
- Las consultas SQL utilizan parametros (`AddWithValue`), lo que previene SQL Injection.
- El DTO `SendUsuario` excluye la contrasena de las respuestas de la API.
- El flujo de login no revela si el error es por nombre o contrasena (mensaje generico "Nombre o contrasena incorrectos").

Lo que falta o es riesgoso:
- **No hay autenticacion implementada.** Todos los endpoints (personas, usuarios, incluyendo GET/PUT/DELETE de usuarios) son publicos. Cualquier cliente puede listar, modificar o eliminar usuarios sin credencial alguna. La implementacion de JWT es el paso mas critico pendiente.
- **Cadenas de conexion hardcodeadas** en el codigo fuente con credenciales de base de datos visibles. Si el repositorio es publico, las credenciales quedan expuestas. Deben moverse a `appsettings.json` (excluido del control de versiones) o a variables de entorno.
- **CORS completamente abierto** (`AllowAnyOrigin`, `AllowAnyMethod`, `AllowAnyHeader`). Aceptable solo en desarrollo local, pero debe restringirse a origenes conocidos antes de cualquier despliegue.
- **Bug critico en `MysqlUsuarioRepository.GetByNombreAsync`**: el parametro SQL mal nombrado (`@id` en vez de `@nombre`) puede provocar que el login siempre falle o que se busque al usuario incorrecto, dependiendo del motor.
- **Sin rate limiting.** El endpoint de login es vulnerable a ataques de fuerza bruta. No hay mecanismo para limitar intentos fallidos.
- **`ex.Message` en respuestas 500.** Exponer detalles internos de excepciones al cliente puede revelar informacion sobre la estructura de la base de datos, rutas del sistema o configuracion interna.

### Arquitectura

**Estado actual: base solida con deuda tecnica menor.**

Lo que esta bien:
- La separacion en capas (Controllers -> Repositories -> Services) esta clara y permite cambiar de motor de base de datos (MySQL <-> SQL Server) con una sola linea en `Program.cs`.
- El uso del patron Repository con interfaz (`IPersonaRepository`, `IUsuarioRepository`) y la Inyeccion de Dependencias facilita las pruebas unitarias y el desacoplamiento.
- El patron DTO con records de C# es idiomatico, inmutable y separa correctamente la entrada de la salida.

Lo que se puede mejorar:
- **Las clases de conexion no estan en el contenedor de DI.** Los repositorios instancian `new MysqlConnection()` / `new sqlServerConnection()` directamente, lo que impide inyectar configuracion y dificulta las pruebas. Deberian registrarse como servicios.
- **Las conexiones no implementan `IDisposable`.** El patron actual (try/finally con Open/Close) funciona pero es fragil. Si el `finally` falla, la conexion queda abierta. Utilizar `using` con `IDisposable` seria mas robusto.
- **No hay capa de servicio (Service Layer).** La logica de negocio (hashear contrasena, validar campos) esta dentro de los controllers. Extraerla a una capa de servicios de negocio mejoraria la separacion de responsabilidades.
- **Nomenclatura inconsistente.** El directorio `services` no sigue PascalCase, la clase `sqlServerConnection` no sigue la convencion de C#, y el archivo `IUsarioRepository.cs` tiene un error tipografico.
- **No hay logging estructurado.** El middleware usa `Console.WriteLine` con colores, lo cual es util para desarrollo pero no es adecuado para produccion. Deberia usarse `ILogger<T>` inyectado por DI.