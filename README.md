# Tienda UCN
Proyecto correspondiente al taller de Backend del ramo de IDWM.

## Tecnologías Utilizadas

- **Framework:** ASP.NET Core 9.0
- **Versionado:** Git + Conventional Commits
- **Base de Datos:** Sqlite
- **ORM:** Entity Framework Core

## Instalación y Configuración Local

### Requisitos Previos

- **.NET 9 SDK**: [Download](https://dotnet.microsoft.com/download/dotnet/9.0)
- **Visual Studio Code**: [Download](https://code.visualstudio.com/)
- **Git** [Download](https://git-scm.com/install/windows)

### Instalar extensiones en VsCode
- **C# Dev Kit**
- **C#**
- **.NET Install Tool**
- **C# Extensions**
- **SQLite**

### 1. Clonar el Repositorio

```bash
git clone https://github.com/carlos44440/Taller-Backend-IDWM-1erSem-2026.git
```

### 2. Cambiar de rama

Abrir la terminal en VsCode y moverse al directorio del proyecto:

```bash
cd .\Taller-Backend-IDWM-1erSem-2026\
```

```bash
git checkout develop
```

### 3. Establecer las variables de entorno

Crear el archivo **.env**:

```bash
cp .env.example .env
```

Configurar las variables de **.env**:

```bash
DATA_BASE_URL = Data Source=database.db
```

Crear el archivo **appsettings.json**:

```bash
cp appsettings.example.json appsettings.json
```

Actualizar las siguientes variables en las seccion **User** de **appsettings.json**:

```bash
"Name": "Admin",
"Email": "admin@tiendaucn.cl",
"Rut": "12345678-9",
"PhoneNumber": "+569 123456789",
"BirthDate": "1990-01-01",
"Gender": "Otro",
"Password": "Admin1234!"
"RandomUserPassword": "Random1234!",
```

### 4. Instalar Entity Framework

```bash
dotnet tool install --global dotnet-ef
```

### 5. Instalar Dependencias

```bash
dotnet restore
```

### 6. Compilar el Proyecto

```bash
dotnet build
```

### 7. Crear Base de datos

```bash
dotnet ef database update
```

### 8. Ejecutar el Proyecto

```bash
dotnet run
```

El servicio estará disponible en: http://localhost:5254

### 9. Visualizar Base de datos

Abrir opciones en VsCode:

```bash
Shift + Ctrl + p
```

Buscar y presionar **SQLite: Open Database**

Finalemente abrir **database.db**
