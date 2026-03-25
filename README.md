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

Abre una terminal en el directorio que desees almacenar este proyecto y ejecuta el siguiente comando:
```bash
git clone https://github.com/carlos44440/Taller-Backend-IDWM-1erSem-2026.git
```

Navega a la carpeta del proyecto clonado
```bash
cd .\Taller-Backend-IDWM-1erSem-2026\
```

Abre VsCode con el siguiente comando:
```bash
code .
```

### 2. Cambiar de rama

Abrir la terminal en VsCode y cambia a la rama develop
```bash
git checkout develop
```

### 3. Establecer las variables de entorno

Crear el archivo **.env**, desde la terminal en VsCode ejecuta este comando:
```bash
cp .env.example .env
```

Configurar las variables de **.env**:
```bash
DATA_BASE_URL = Data Source=<nombreBD>.db
RESEND_API_KEY = tu_resend_api_key
JWT_SECRET = your_jwt_secret_key
```
- Reemplace `<nombreBD>` por el nombre que tendra su base de datos.
- Reemplace `RESEND_API_KEY` con su API key de resend; para ello puede obtener su API key en el siguiente enlace: [Resend - API keys](https://resend.com/api-keys).
- Reemplace `JWT_SECRET` con una clave secreta segura de al menos 32 caracteres.

### 4. Establecer las configuraciones en appsettings.json

Crear el archivo **appsettings.json**:
```bash
cp appsettings.example.json appsettings.json
```

**En caso de considerar necesario actualizar las siguientes variables en cada sección:**

**Token:**
- Reemplace `ExpirationTimeInHours` con la cantidad de horas tras las cuales expirarán los tokens.

**VerificationCode:**
- Reemplace `ExpirationTimeInMinutes` con el tiempo en minutos para que expire el código de verificación.
- Reemplace `MaxFailedAttempts` con el número máximo de intentos fallidos permitidos, antes de que se bloquee la cuenta del usuario.
- Reemplace `WaitingTimeInMinutesAfterResendEmail` con el tiempo de espera en minutos antes de permitir el reenvio de un nuevo correo de verificación.

**EmailConfiguration**:
- Reemplace `From` con la dirección de salida, se recomienda `Tienda - UCN <onboarding@resend.dev>`. Ten en cuenta que, al usar el dominio de prueba, solo podrás enviar correos a la dirección con la que te registraste en Resend.

**Jobs:**
- Reemplace `CronJobDeleteUnconfirmedUsers` con la expresión cron que define la ejecución automática para eliminar usuarios no verificados. **Valor recomendado:** `30 20 * * *` (ejecuta la tarea diariamente a las 20:30).
- Reemplace `CronJobDeleteExpiredTokens` con la expresión cron que define la ejecución automática para eliminar tokens expirados. **Valor recomendado:** `30 20 * * *` (ejecuta la tarea diariamente a las 20:30).
- Reemplace `TimeZone` con la zona horaria utilizada para la ejecución de las tareas programadas. **Valor recomendado:** `Pacific SA Standard Time` (zona horaria de Chile).
- Reemplace `DaysToDeleteUnverifiedAccount` con la cantidad de días máximos permitidos antes de eliminar una cuenta que no esta verificada.

**HangfireDashboard:**
- Reemplace `DashboardPath` con la ruta de acceso donde estará disponible el panel de control. **Valor recomendado:** `/hangfire`

**User:**

Datos del usuario administrador (`AdminUser`)
- Reemplace `Name` con el nombre para el admin
- Reemplace `Email` siguiendo este formato example@dominio.cl
- Reemplace `Rut` siguiendo este formato XXXXXXXX-X
- Reemplace `BirthDate` siguiendo este formato YYYY-MM-DD
- Reemplace `PhoneNumber` siguiendo este formato +569 XXXXXXXX
- Reemplace `Gender` con cualquiera de estas opciones "Masculino | Femenino | Otro"
- Reemplace `Password` con una contraseña alfanumérica que contenga al menos una letra mayúscula y al menos un carácter especial.

Contraseña para usuarios aleatorios
- Reemplace `RandomUserPassword` con una contraseña alfanumérica que contenga al menos una letra mayúscula y al menos un carácter especial.

### 5. Instalar Entity Framework
```bash
dotnet tool install --global dotnet-ef
```

### 6. Instalar Dependencias

```bash
dotnet restore
```

### 7. Compilar el Proyecto

```bash
dotnet build
```

### 8. Crear Base de datos

```bash
dotnet ef database update
```

### 9. Ejecutar el Proyecto

```bash
dotnet run
```

El servicio estará disponible en: http://localhost:5254

### 10. Visualizar Base de datos

Abrir opciones en VsCode:

```bash
Shift + Ctrl + p
```

Buscar y presionar **SQLite: Open Database**

Finalemente abrir **database.db**
