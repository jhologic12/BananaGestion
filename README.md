# BananaGestion - Sistema de Gestión de Finca Bananera

Sistema completo para la gestión de una finca bananera, incluyendo control de labores, cosecha, inventario e inventario.

## Arquitectura

```
BananaGestion/
├── src/
│   ├── BananaGestion.Api/          # API REST con .NET 8
│   ├── BananaGestion.Application/  # Lógica de negocio (MediatR, FluentValidation)
│   ├── BananaGestion.Domain/       # Entidades y enums
│   └── BananaGestion.Infrastructure/ # Acceso a datos (EF Core, servicios)
├── frontend/                        # React + TypeScript + TailwindCSS
└── docker/                          # Configuración Docker
```

## Stack Tecnológico

### Backend
- **.NET 8** con Clean Architecture
- **Entity Framework Core** + PostgreSQL
- **MediatR** para CQRS
- **JWT** para autenticación
- **SendGrid** para notificaciones email
- **Azure Blob Storage** para archivos (compatible S3)
- **SixLabors.ImageSharp** para conversión TIF

### Frontend
- **React 18** + TypeScript
- **TailwindCSS** para estilos
- **React Router** para navegación
- **Zustand** para state management
- **React Hook Form** + **Yup** para validación
- **React Hot Toast** para notificaciones
- **Lucide React** para iconos

## Módulos

1. **Autenticación**: Login/registro con roles (Administrador, Supervisor, Obrero)
2. **Lotes**: Gestión de parcelas de la finca
3. **Labores**: Programación y seguimiento de tareas (Desmache, Deshoja, Fumigación, etc.)
4. **Cosecha**: Registro de embolsado, corte y cajas por semana
5. **Inventario**: Control de stock de insumos
6. **Órdenes**: Gestión de órdenes de embarque
7. **Reportes**: Dashboard y métricas

## Inicio Rápido

### Con Docker (Recomendado)

```bash
cd docker
docker-compose up -d
```

La aplicación estará disponible en:
- Frontend: http://localhost
- API: http://localhost:5000
- Swagger: http://localhost:5000/swagger

### Desarrollo Local

#### Backend
```bash
cd src/BananaGestion.Api
dotnet restore
dotnet run
```

#### Frontend
```bash
cd frontend
npm install
npm run dev
```

## Configuración

### Variables de Entorno (API)

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=bananagestion;Username=postgres;Password=postgres"
  },
  "Jwt": {
    "Key": "tu-clave-secreta-minimo-32-caracteres",
    "Issuer": "BananaGestion",
    "Audience": "BananaGestionApp",
    "ExpireMinutes": 480
  },
  "EmailSettings": {
    "SendGridApiKey": "tu-api-key"
  }
}
```

### Variables de Entorno (Frontend)

```env
VITE_API_URL=http://localhost:5000/api
```

## Modelos de Datos Principales

### Users
- id, email, passwordHash, nombre, apellido, telefono, rol, activo

### Lotes
- id, codigo, nombre, hectareas, ubicacion, latitud, longitud

### TaskConfig
- id, nombre, tipoLabor, tipo, frecuenciaDias, requiereFoto/Firma/GPS

### TaskAssignment
- id, taskConfigId, userId, loteId, fechaProgramada, status

### TaskLog
- id, taskAssignmentId, fotoUrl, firmaUrl, latitud, longitud, observaciones

### HarvestRecord
- id, loteId, userId, fecha, cantidadRacimosEmbolsados/Cortados, colorCinta

### Product
- id, codigo, nombre, unidad, stockMinimo, stockActual

### BoxType
- id, codigo, descripcion, capacidadKilos

## Funcionalidades Especiales

### Gestión de Fotos TIF
El sistema convierte automáticamente las fotos capturadas a formato TIF con compresión LZW.

### Calendario de Cosecha
Asignación automática de colores de cinta por semana del año.

### Proyección de Cosecha
Basado en datos históricos, predice la cosecha de las próximas semanas.

### Notificaciones Email
- Recordatorios de tareas pendientes
- Alertas de stock bajo

## Próximos Pasos

1. Completar módulo de Órdenes
2. Implementar reportes exportables (PDF/Excel)
3. Agregar geofencing para validación GPS
4. Implementar firma digital táctil
5. Dashboard financiero completo

## Licencia

MIT
