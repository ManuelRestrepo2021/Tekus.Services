# Tekus Services API

Aplicativo de prueba técnica para **TEKUS S.A.S.** construido con **ASP.NET Core 8**, **Entity Framework Core** y **JWT**.  
Permite administrar proveedores, servicios y países, incluyendo:

- CRUD de **proveedores**, **servicios** y **países**.
- Asociación de **servicios por proveedor**.
- Asociación de **servicios por país**.
- **Campos personalizados** por proveedor.
- Consulta de **países desde un servicio externo simulado**.
- **Indicadores / reportes** (ej. cantidad de servicios por país).
- **Autenticación JWT** con usuario por defecto.
- **Paginación, búsqueda y ordenamiento** en todos los listados.
- **Pruebas unitarias** para `ProviderService`.

---

## Tecnologías y arquitectura

- **.NET**: .NET 8
- **Framework web**: ASP.NET Core Web API
- **ORM**: Entity Framework Core 8 (SQL Server, InMemory para tests)
- **Autenticación**: JWT Bearer
- **Documentación**: Swagger / OpenAPI
- **Pruebas**:
  - xUnit
  - FluentAssertions
  - EF Core InMemory

### Estructura de proyectos (DDD simplificado)

La solución está separada por capas siguiendo principios de DDD (Domain Driven Design) y Clean Architecture:

- `Tekus.Services.Domain`  
  - Entidades de dominio (`Provider`, `Service`, `Country`, `ProviderCustomField`, etc.).
  - No tiene dependencias hacia otras capas.

- `Tekus.Services.Application`  
  - Interfaces de servicios de aplicación (`IProviderService`, `IServiceService`, `ICountryService`, `IReportService`, `IAuthService`, `IExternalCountryService`).
  - DTOs (`ProviderDto`, `ProviderUpsertDto`, `ServiceDto`, `ServiceUpsertDto`, `CountryDto`, `CountryUpsertDto`, `PagedResult<T>`, etc.).
  - Reglas de negocio a nivel de aplicación.

- `Tekus.Services.Infrastructure`  
  - `AppDbContext` (EF Core).
  - Implementaciones de servicios de aplicación (`ProviderService`, `ServiceService`, `CountryService`, `ReportService`, `AuthService`, `ExternalCountryService`).
  - Mapeos entre entidades y DTOs.

- `Tekus.Services.Api`  
  - ASP.NET Core Web API.
  - Controladores (`ProvidersController`, `ServicesController`, `CountriesController`, `ReportsController`, `AuthController`).
  - Configuración de autenticación JWT y Swagger.

- `Tekus.Services.Tests`  
  - Proyecto de pruebas xUnit.
  - Pruebas unitarias para `ProviderService`.
  - Uso de `AppDbContext` con `InMemoryDatabase`.

---

## Modelo de datos

### Entidades principales

- **Provider**  
  - `Id` (int, PK, Identity)  
  - `Nit` (string, requerido, único)  
  - `Name` (string, requerido)  
  - `Email` (string, requerido, formato email)  
  - Relación con servicios (`ProviderService` / similar).  
  - Relación con campos personalizados (`ProviderCustomField`).

- **Service**
  - `Id` (int, PK, Identity)
  - `Name` (string, requerido)
  - `HourlyRateUsd` (decimal, requerido)
  - Relación con proveedores.
  - Relación con países (`ServiceCountry` / similar).

- **Country**
  - `Id` (int, PK, Identity)
  - `Name` (string, requerido)
  - `IsoCode` (string, requerido, único, ej. `CO`, `PE`, `MX`)

- **ProviderCustomField**
  - `Id` (int, PK, Identity)
  - `ProviderId` (FK a `Provider`)
  - `Key` (string, nombre del campo personalizado)
  - `Value` (string, valor del campo)

- Tablas de relación (según implementación real en el proyecto):
  - **ProviderService**: `ProviderId`, `ServiceId`
  - **ServiceCountry**: `ServiceId`, `CountryId`

> Nota: El repositorio incluye scripts SQL de creación y datos iniciales en el archivo Script_SQL.sql.

---

## Requisitos funcionales cubiertos

1. **CRUD de proveedores**
   - `GET /api/Providers` (paginación, búsqueda, ordenamiento)
   - `GET /api/Providers/{id}`
   - `POST /api/Providers`
   - `PUT /api/Providers/{id}`
   - `DELETE /api/Providers/{id}`

2. **CRUD de servicios**
   - `GET /api/Services`
   - `GET /api/Services/{id}`
   - `POST /api/Services`
   - `PUT /api/Services/{id}`
   - `DELETE /api/Services/{id}`

3. **CRUD de países**
   - `GET /api/Countries`
   - `GET /api/Countries/{id}`
   - `POST /api/Countries`
   - `PUT /api/Countries/{id}`
   - `DELETE /api/Countries/{id}`

4. **Servicio externo de países (simulado)**
   - `IExternalCountryService` + `ExternalCountryService` (simulado en memoria).
   - `GET /api/Countries/external`: lista los países de la fuente externa.
   - `POST /api/Countries/sync-external`: sincroniza países externos a la BD local.

5. **Campos personalizados por proveedor**
   - Modelo `ProviderCustomField`.
   - Endpoint(s) para administrar campos personalizados, por ejemplo:
     - `GET /api/Providers/{providerId}` incluye campos personalizados en el `ProviderDto`.
     - (Opcional) Endpoints específicos como:
       - `GET /api/Providers/{providerId}/custom-fields`
       - `POST /api/Providers/{providerId}/custom-fields`
   - Permite casos como:
     - “Número de contacto en Marte”
     - “Cantidad de mascotas en nómina”

6. **Indicadores / reportes**
   - `ReportsController` / `IReportService`
   - Ejemplo de endpoint:
     - `GET /api/Reports/summary`
   - Indicadores posibles (según implementación en el código):
     - Cantidad de servicios por país.
     - Cantidad de proveedores por país.
   - Respuesta en un DTO de resumen.

7. **Paginación, búsqueda y ordenamiento**
   - Para listados (`GET /api/Providers`, `GET /api/Services`, `GET /api/Countries`):
     - Parámetros:
       - `page` (int, >= 1)
       - `pageSize` (int, > 0)
       - `search` (string, opcional)
       - `sortField` (string, opcional)
       - `sortDir` (string, opcional, ej. `asc` / `desc`)
   - Respuesta estandarizada con `PagedResult<T>`:
     ```json
     {
       "items": [ ... ],
       "totalCount": 42,
       "page": 1,
       "pageSize": 10
     }
     ```

8. **Validaciones de datos**
   - DTOs con data annotations:
     - `[Required]`, `[StringLength]`, `[EmailAddress]`, `[Range]`, etc.
   - Verificación de `ModelState.IsValid` en todos los `POST` y `PUT`.
   - Respuestas `400 BadRequest` cuando el modelo no es válido.

9. **Autenticación JWT**
   - Login con usuario por defecto.
   - Generación de token JWT con:
     - `Issuer`, `Audience`, `Key` desde `appsettings.json` (`JwtSettings`).
   - Protege los controladores:
     - `[Authorize]` en `CountriesController`, `ServicesController`, `ProvidersController`, `ReportsController`.
     - `AuthController` queda público para poder obtener el token.
   - Configuración en `Program.cs`:
     - `AddAuthentication().AddJwtBearer(...)`
     - Validación de issuer, audience, signing key y lifetime.

10. **Documentación Swagger**
    - `AddEndpointsApiExplorer()` y `AddSwaggerGen(...)`.
    - Definición de esquema de seguridad Bearer:
      - `Authorization: Bearer {token}`.
    - Botón **Authorize** para enviar el token JWT en todas las peticiones.

11. **Pruebas unitarias**
    - Proyecto `Tekus.Services.Tests`.
    - Pruebas xUnit para `ProviderService`:
      - Crear proveedor.
      - Obtener por id (sin excepción cuando existe).
      - Retornar `null` cuando el proveedor no existe.
      - Eliminar proveedor existente.
      - Eliminar proveedor inexistente (`false`).

---

## Configuración y ejecución

### Requisitos

- **Visual Studio 2022** (o superior) con el workload de **ASP.NET y desarrollo web**  
  o  
  **VS Code** + **.NET 8 SDK**
- SQL Server local (o conexión ajustada en `appsettings.json`).

### 1. Clonar el repositorio

```bash
git clone https://github.com/<owner>/<repo>.git
cd <repo>
```

### 2. Configurar cadena de conexión y JWT

En `Tekus.Services.Api/appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=.;Database=TekusServicesDb;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True"
  },
  "JwtSettings": {
    "Key": "ClaveSuperSecretaDeAlMenos32Caracteres",
    "Issuer": "Tekus.Services",
    "Audience": "Tekus.Services.Client"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}
```

Ajustar `DefaultConnection` a tu entorno (instancia de SQL Server).

### 3. Crear base de datos

#### Opción A: usando scripts SQL (recomendado para la prueba)

1. En la carpeta `db/` del repo:
   - `create-database.sql`: crea la base de datos y tablas.
   - `seed-data.sql`: inserta datos iniciales (≥ 10 registros por tabla).

2. Ejecutar en SQL Server Management Studio o `sqlcmd`:

```sql
-- En SSMS o Azure Data Studio:
-- 1. create-database.sql
-- 2. seed-data.sql
```

#### Opción B: usando migraciones de EF Core

Si el proyecto incluye migraciones:

```bash
cd Tekus.Services.Api
dotnet ef database update
```

(asegurarse de tener instalado `dotnet-ef` globalmente si es necesario).

### 4. Ejecutar la API

#### Visual Studio

1. Abrir la solución `Tekus.Services.sln`.
2. Establecer `Tekus.Services.Api` como proyecto de inicio.
3. Presionar **F5** o **Ctrl+F5**.
4. Se abrirá Swagger en `https://localhost:xxxx/swagger`.

#### CLI

```bash
cd Tekus.Services.Api
dotnet run
```

---

## Uso de la API

### 1. Autenticación (obtener token JWT)

1. En Swagger, ir a `POST /api/Auth/login`.
2. Enviar un body similar a:

```json
{
  "username": "admin",
  "password": "Admin123!"
}
```

> Nota: Usuario/contraseña por defecto pueden estar hardcodeados en `AuthService` o en `appsettings`. Ver implementación concreta.

3. Copiar el valor de `token` devuelto.

4. En Swagger, pulsar el botón **Authorize** (arriba a la derecha):

   - En el campo de valor, escribir solo el **token** o `Bearer {token}`, según la configuración.  
     Con la configuración actual, normalmente se pega **solo el token** y Swagger agrega el prefijo `Bearer`.

5. Pulsar **Authorize → Close**.

A partir de aquí, todas las peticiones a endpoints protegidos enviarán:

```http
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

### 2. Proveedores

- **Listar** (paginado, búsqueda, ordenamiento):

  `GET /api/Providers?page=1&pageSize=10&search=tekus&sortField=name&sortDir=asc`

- **Obtener por id**:

  `GET /api/Providers/1`

- **Crear**:

  ```http
  POST /api/Providers
  Content-Type: application/json

  {
    "nit": "900123456-7",
    "name": "Importaciones Tekus S.A.S.",
    "email": "contacto@tekus.co",
    "servicesIds": [1, 2]
  }
  ```

- **Actualizar**:

  ```http
  PUT /api/Providers/1
  Content-Type: application/json

  {
    "nit": "900123456-7",
    "name": "Importaciones Tekus S.A.S.",
    "email": "soporte@tekus.co",
    "servicesIds": [1, 3]
  }
  ```

- **Eliminar**:

  `DELETE /api/Providers/1`

### 3. Campos personalizados de proveedor

Según la implementación, puede ser:

- Incluidos directamente en `ProviderDto`:

  ```json
  {
    "id": 1,
    "nit": "900123456-7",
    "name": "Importaciones Tekus S.A.S.",
    "email": "contacto@tekus.co",
    "customFields": [
      { "key": "NumeroContactoMarte", "value": "555-SPACE" },
      { "key": "MascotasNomina", "value": "3" }
    ]
  }
  ```

- Y un endpoint para agregarlos / actualizarlos (ver Swagger según la implementación exacta).

### 4. Servicios

- `GET /api/Services`
- `GET /api/Services/{id}`
- `POST /api/Services`
- `PUT /api/Services/{id}`
- `DELETE /api/Services/{id}`

Ejemplo de creación:

```json
{
  "name": "Descarga espacial de contenidos",
  "hourlyRateUsd": 120.50,
  "countryIds": [1, 2, 3]   // Colombia, Perú, México
}
```

### 5. Países

- `GET /api/Countries`
- `GET /api/Countries/{id}`
- `POST /api/Countries`
- `PUT /api/Countries/{id}`
- `DELETE /api/Countries/{id}`

### 6. Servicio externo de países

- `GET /api/Countries/external`  
  Devuelve lista simulada de países (Colombia, Perú, México, Chile, Argentina, etc.).

- (Opcional) `POST /api/Countries/sync-external`  
  Sincroniza los países externos a la base de datos local si aún no existen.

### 7. Reportes / indicadores

- `GET /api/Reports/summary`  
  Devuelve indicadores agregados, por ejemplo:

```json
{
  "servicesByCountry": [
    { "country": "Colombia", "servicesCount": 5 },
    { "country": "Perú", "servicesCount": 3 }
  ],
  "providersByCountry": [
    { "country": "Colombia", "providersCount": 4 },
    { "country": "México", "providersCount": 2 }
  ]
}
```

(La estructura concreta depende del DTO definido en `IReportService`.)

---

## Pruebas

### Ejecutar pruebas desde Visual Studio

1. Menú **Prueba → Explorador de pruebas**.
2. Clic en **Ejecutar todas las pruebas**.
3. Deberías ver las pruebas de `ProviderServiceTests` pasando (iconos verdes).

### Ejecutar pruebas desde la consola

```bash
dotnet test
```

o solo el proyecto de tests:

```bash
dotnet test .\Tekus.Services.Tests\Tekus.Services.Tests.csproj
```

---

## Consideraciones finales

- El proyecto sigue una arquitectura por capas con principios de DDD, separando bien:
  - Dominio
  - Aplicación
  - Infraestructura
  - API
  - Tests
- El código está documentado con comentarios XML en los controladores y servicios clave.
- Swagger expone toda la API de forma interactiva, incluyendo autenticación JWT.
- La solución incluye:
  - Scripts de base de datos (`create-database.sql`, `seed-data.sql`).
  - Diagrama de entidad-relación.
  - Proyecto de pruebas unitarias.

Para la evaluación de la prueba:

- Revisar primero `README.md` (este archivo).
- Ejecutar los scripts de DB.
- Lanzar la API y probar en Swagger:
  - Login (`/api/Auth/login`).
  - CRUD de Proveedores, Servicios y Países.
  - Servicio externo de países.
  - Reportes.
  - Campos personalizados (si aplica).

Cualquier duda sobre la configuración o funcionamiento puede consultarse en el código o a través del correo indicado en el enunciado de la prueba.
