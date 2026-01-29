# Task Management API

API REST desarrollada en **.NET 8** para la gestión de tareas (Tasks), aplicando principios de **Clean Architecture**, manejo centralizado de errores y persistencia con **SQL Server** mediante **Entity Framework Core**.

---

## 🧩 Descripción de la solución

La API permite:

- ✅ Crear tareas
- 📋 Obtener todas las tareas activas
- 🔍 Obtener una tarea por ID
- ✏️ Actualizar tareas existentes
- 🗑️ Eliminar tareas de forma lógica (soft delete)

### Comportamientos importantes

- El endpoint **GET /tasks** devuelve **solo tareas activas** (`IsActive = true`)
- Las tareas eliminadas lógicamente no se devuelven en los listados
- El sistema valida que el **título de la tarea sea único**
- Existen **tareas precargadas** en la base de datos mediante migraciones

---

## 🚀 Pasos para levantar el proyecto localmente

### 1️⃣ Requisitos

- .NET SDK **8.0**
- SQL Server (LocalDB o instancia local/remota)
- EF Core CLI

**Verificar SDK:**

```bash
dotnet --version
```

### 2️⃣ Configuración de la base de datos

Modificar el archivo `appsettings.json` y configurar la cadena de conexión:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=TaskManagementDb;Trusted_Connection=True;TrustServerCertificate=True;"
}
```

> **Nota:** Modificar el servidor `localhost` por el que desee utilizar.

### 3️⃣ Restaurar dependencias

Las dependencias no están versionadas en el repositorio.

**Ejecutar:**

```bash
dotnet restore
```

### 4️⃣ Aplicar migraciones y crear la base de datos

```bash
dotnet ef database update
```

Este paso:

- Crea la base de datos
- Aplica el esquema
- Inserta tareas iniciales precargadas

### 5️⃣ Ejecutar la API

```bash
dotnet run
```

La API estará disponible por defecto en:

- **API:** `http://localhost:5216`
- **Swagger:** `http://localhost:5216/swagger`

---

## 🛠 Arquitectura y tecnologías

### Stack tecnológico

- **.NET SDK 8.0**
- **SQL Server**
- **Entity Framework Core**

### Arquitectura en capas

- **API** - Controllers y configuración
- **Application** - Servicios, DTOs y lógica de aplicación
- **Domain** - Entidades y reglas de negocio
- **Infrastructure** - Persistencia y repositorios

---

## 📌 Notas adicionales

- El manejo de errores se realiza mediante un **Global Exception Handler** que devuelve respuestas estándar `ProblemDetails`
- La eliminación de tareas es **lógica** (soft delete)
- El proyecto incluye **pruebas unitarias** para los servicios de dominio

---
