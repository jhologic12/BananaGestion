# Manual de Usuario - Administrador
## Sistema de Gestión de Finca Bananera - BananaGestion

---

## Tabla de Contenidos

1. [Introducción](#1-introducción)
2. [Acceso al Sistema](#2-acceso-al-sistema)
3. [Dashboard](#3-dashboard)
4. [Módulo Usuarios](#4-módulo-usuarios)
5. [Módulo Lotes](#5-módulo-lotes)
6. [Módulo Labores](#6-módulo-labores)
7. [Módulo Cosecha](#7-módulo-cosecha)
8. [Módulo Inventario](#8-módulo-inventario)
9. [Módulo Órdenes](#9-módulo-órdenes)
10. [Módulo Reportes](#10-módulo-reportes)
11. [Configuración](#11-configuración)
12. [Gestión de Sesión](#12-gestión-de-sesión)

---

## 1. Introducción

### 1.1 Descripción del Sistema

BananaGestion es un sistema completo para la gestión de una finca bananera. Permite controlar labores, cosecha, inventario y órdenes de embarque de manera centralizada.

### 1.2 Arquitectura

| Componente | Tecnología |
|------------|-----------|
| Frontend | React 18 + TypeScript + TailwindCSS |
| Backend API | .NET 8 (Clean Architecture) |
| Base de Datos | PostgreSQL 16 |
| Autenticación | JWT (JSON Web Tokens) |
| Contenedores | Docker / Docker Compose |

### 1.3 Roles del Sistema

| Rol | Descripción |
|-----|-------------|
| **Administrador** | Acceso completo a todos los módulos del sistema |
| **Supervisor** | Acceso solo a Dashboard, Labores y Cosecha |
| **Obrero** | Acceso básico para registrar ejecución de tareas |

### 1.4 Acceso al Sistema

| Servicio | URL |
|----------|-----|
| Frontend | http://localhost |
| API | http://localhost:5000 |
| Swagger | http://localhost:5000/swagger |

---

## 2. Acceso al Sistema

### 2.1 Iniciar Sesión

1. Abra el navegador y vaya a **http://localhost**
2. Ingrese su **correo electrónico** y **contraseña**
3. Haga clic en **Iniciar Sesión**

### 2.2 Registro de Usuario

1. En la pantalla de login, haga clic en **Registrarse**
2. Complete los campos:
   - Nombre
   - Apellido
   - Correo electrónico
   - Teléfono (opcional)
   - Rol (Administrador, Supervisor u Obrero)
   - Contraseña (mínimo 6 caracteres)
3. Haga clic en **Registrarse**

> **Nota:** El primer usuario registrado como Administrador podrá crear usuarios adicionales desde el módulo de Usuarios.

### 2.3 Cerrar Sesión

- Haga clic en el botón **Cerrar Sesión** ubicado en la parte inferior de la barra lateral.

---

## 3. Dashboard

El Dashboard es la página principal del sistema. Muestra un resumen general del estado de la finca.

### 3.1 Tarjetas de Métricas

| Tarjeta | Descripción |
|---------|-------------|
| **Tareas Pendientes** | Número total de tareas asignadas que aún no han sido completadas |
| **Tareas Vencidas** | Tareas cuya fecha programada ya pasó sin ser completadas |
| **Racimos Cortados** | Total de racimos cosechados (estado "Cortado") a la fecha |
| **Bajo Stock** | Productos de inventario cuyo stock actual está por debajo del mínimo |
| **Obreros Activos** | Número de trabajadores con rol "Obrero" registrados y activos |
| **Lotes Activos** | Número de parcelas/terrenos habilitados en el sistema |

### 3.2 Proyección de Cosecha

Muestra las próximas 4 semanas con datos de encinte y la cantidad de racimos encintados. Cada semana tiene asociado un color de cinta rotativo.

### 3.3 Actividad Reciente

Lista las últimas 5 tareas con su estado actual (Pendiente, EnProgreso, Completada).

---

## 4. Módulo Usuarios

**Acceso:** Solo Administrador

### 4.1 Ver Lista de Usuarios

1. Navegue a **Usuarios** en el menú lateral
2. Se muestra una tabla con:
   - Nombre completo
   - Correo electrónico
   - Teléfono
   - Rol
   - Estado (Activo/Inactivo)
   - Fecha de creación
   - Acciones

### 4.2 Crear Usuario

1. Haga clic en el botón **Nuevo Usuario** (esquina superior derecha)
2. Complete el formulario:
   - **Nombre:** Nombre del usuario (requerido)
   - **Apellido:** Apellido del usuario (requerido)
   - **Email:** Correo electrónico único (requerido)
   - **Teléfono:** Número de contacto (opcional)
   - **Rol:** Seleccione Administrador, Supervisor u Obrero
   - **Contraseña:** Mínimo 6 caracteres (requerida)
3. Haga clic en **Crear**

### 4.3 Editar Usuario

1. En la lista de usuarios, haga clic en el ícono de **lápiz** (Editar)
2. Modifique los campos deseados
3. Haga clic en **Actualizar**

### 4.4 Desactivar Usuario

1. En la lista de usuarios, haga clic en el ícono de **apagar** (naranja)
2. Confirme la acción en el diálogo
3. El usuario quedará en estado **Inactivo** y no podrá iniciar sesión

### 4.5 Activar Usuario

1. En la lista de usuarios, haga clic en el ícono de **encender** (verde) en usuarios inactivos
2. Confirme la acción en el diálogo
3. El usuario volverá a estado **Activo** y podrá iniciar sesión

### 4.6 Eliminar Usuario

1. En la lista de usuarios, haga clic en el ícono de **basurero** (rojo)
2. Confirme la eliminación
3. El usuario será desactivado (eliminación suave)

> **Nota:** Un Supervisor no puede eliminar ni modificar a un Administrador.

---

## 5. Módulo Lotes

**Acceso:** Solo Administrador

### 5.1 Ver Lista de Lotes

1. Navegue a **Lotes** en el menú lateral
2. Se muestra una tabla con:
   - Código
   - Nombre
   - Hectáreas
   - Ubicación
   - Estado (Activo/Inactivo)
   - Acciones

### 5.2 Crear Lote

1. Haga clic en **Nuevo Lote**
2. Complete el formulario:
   - **Código:** Identificador único del lote (ej: L-001)
   - **Nombre:** Nombre descriptivo del lote
   - **Hectáreas:** Tamaño en hectáreas (número decimal)
   - **Ubicación:** Descripción textual de la ubicación
   - **Latitud / Longitud:** Coordenadas GPS (opcionales)
   - **Notas:** Observaciones adicionales
3. Haga clic en **Crear**

### 5.3 Editar Lote

1. Haga clic en el ícono de **lápiz** en el lote deseado
2. Modifique los campos
3. Haga clic en **Actualizar**

### 5.4 Desactivar Lote

1. Haga clic en el ícono de **basurero**
2. Confirme la acción
3. El lote quedará inactivo y no aparecerá en listas de selección

---

## 6. Módulo Labores

**Acceso:** Todos los roles (Administrador, Supervisor, Obrero)

### 6.1 Pestaña: Configuración de Tareas

Solo visible para Administrador y Supervisor.

#### 6.1.1 Crear Tipo de Tarea

1. Haga clic en **Nueva Configuración**
2. Complete:
   - **Nombre:** Nombre de la labor (ej: Desmache, Deshoja, Fumigación)
   - **Tipo de Labor:** Categoría de la tarea
   - **Tipo:** Frecuente o Puntual
   - **Frecuencia (días):** Cada cuántos días se repite (0 para puntual)
   - **Requiere Foto:** Sí/No
   - **Requiere Firma:** Sí/No
   - **Requiere GPS:** Sí/No
3. Haga clic en **Crear**

#### 6.1.2 Editar/Eliminar Configuración

- Use los íconos de **lápiz** (editar) o **basurero** (eliminar) en la fila correspondiente

### 6.2 Pestaña: Tareas Pendientes

Lista todas las tareas asignadas con su estado:

| Estado | Descripción |
|--------|-------------|
| **Pendiente** | Tarea programada sin iniciar |
| **EnProgreso** | Tarea en ejecución |
| **Completada** | Tarea finalizada |

#### 6.2.1 Crear Asignación de Tarea

1. Haga clic en **Nueva Asignación**
2. Seleccione:
   - **Configuración:** Tipo de tarea a asignar
   - **Usuario:** Trabajador responsable
   - **Lote:** Parcela donde se ejecutará
   - **Fecha Programada:** Cuándo debe realizarse
   - **Notas:** Observaciones adicionales
3. Haga clic en **Crear**

#### 6.2.2 Cambiar Estado de Tarea

1. Localice la tarea en la lista
2. Haga clic en el botón de cambio de estado
3. Seleccione el nuevo estado

### 6.3 Pestaña: Tareas Vencidas

Muestra las tareas cuya fecha programada ya pasó y no han sido completadas.

### 6.4 Registrar Ejecución de Tarea

1. Seleccione una tarea asignada
2. Complete el registro:
   - **Foto:** Subir imagen de la ejecución (si es requerida)
   - **Firma:** Cargar firma digital (si es requerida)
   - **Latitud / Longitud:** Coordenadas GPS del punto de ejecución (si es requerido)
   - **Observaciones:** Notas sobre la labor realizada
3. Haga clic en **Registrar**

---

## 7. Módulo Cosecha

**Acceso:** Todos los roles (Administrador, Supervisor, Obrero)
**Escritura (Registrar):** Solo Administrador y Supervisor

### 7.1 Pestaña: Encinte

Muestra las 52 semanas del año con su color de cinta asociado y la cantidad de racimos encintados.

#### 7.1.1 Registrar Encinte

1. Haga clic en **Nuevo Encinte**
2. Seleccione:
   - **Lote:** Parcela donde se realizó el encinte
   - **Semana de Encinte:** Semana del año (1-52) con su color de cinta
   - **Cantidad de Racimos Embolsados:** Número de racimos encintados
   - **Notas:** Observaciones opcionales
3. Haga clic en **Guardar**

#### 7.1.2 Tabla de Colores de Cinta

| Semana | Color | Semana | Color |
|--------|-------|--------|-------|
| 1 | Verde | 27 | Blanco |
| 2 | Amarillo | 28 | Azul |
| 3 | Blanco | 29 | Rojo |
| 4 | Azul | 30 | Café |
| 5 | Rojo | 31 | Negro |
| 6 | Café | 32 | Naranja |
| 7 | Negro | 33 | Verde |
| 8 | Naranja | 34 | Amarillo |
| 9 | Verde | 35 | Blanco |
| 10 | Amarillo | 36 | Azul |
| 11 | Blanco | 37 | Rojo |
| 12 | Azul | 38 | Café |
| 13 | Rojo | 39 | Negro |
| 14 | Café | 40 | Naranja |
| 15 | Negro | 41 | Verde |
| 16 | Naranja | 42 | Amarillo |
| 17 | Verde | 43 | Blanco |
| 18 | Amarillo | 44 | Azul |
| 19 | Blanco | 45 | Rojo |
| 20 | Azul | 46 | Café |
| 21 | Rojo | 47 | Negro |
| 22 | Café | 48 | Naranja |
| 23 | Negro | 49 | Verde |
| 24 | Naranja | 50 | Amarillo |
| 25 | Verde | 51 | Blanco |
| 26 | Amarillo | 52 | Azul |

### 7.2 Pestaña: Cosecha

Muestra el seguimiento de cada semana: Encinte, Semitallo, Cortados, Pendientes y Proyección.

#### 7.2.1 Registrar Semitallo

1. Haga clic en **Registrar Semitallo/Corte**
2. Seleccione:
   - **Semana de Encinte Referencia:** Semana del encinte origen
   - **Lote:** Parcela donde se realizará
   - **Tipo de Registro:** Seleccione **Semitallo (Sábado - Marcar racimos)**
   - **Cantidad de Racimos:** Número de racimos marcados
   - **Notas:** Observaciones opcionales
3. Haga clic en **Guardar**

#### 7.2.2 Registrar Corte

1. Haga clic en **Registrar Semitallo/Corte**
2. Seleccione:
   - **Semana de Encinte Referencia:** Semana del encinte origen
   - **Lote:** Parcela donde se realizará
   - **Tipo de Registro:** Seleccione **Cortado (Lunes - Cosecha)**
   - **Cantidad de Racimos:** Número de racimos cosechados
   - **Notas:** Observaciones opcionales
3. Haga clic en **Guardar**

#### 7.2.3 Regla de Validación - Sin Valores Negativos

El sistema valida que no se puedan registrar más racimos de los disponibles:

- **Disponibles = Encintados - Semitallo - Cortados**
- Si intenta registrar una cantidad mayor a los disponibles, el sistema mostrará un error indicando:
  - Cantidad disponible
  - Total encintados
  - Total semitallo
  - Total cortados

### 7.3 Pestaña: Proyección

Tabla resumen con la proyección de cosecha basada en datos de encinte:

| Columna | Descripción |
|---------|-------------|
| Semana Encinte | Semana del encinte con su color |
| Color | Nombre del color de cinta |
| Encintados | Total de racimos encintados |
| Semitallo | Total de racimos marcados (semitallo) |
| Cortados | Total de racimos cosechados |
| Pendientes | Racimos aún por cosechar |
| Proyección Corte | Semanas estimadas de corte (mín-máx) |
| Estado | Completado / En Proceso / BARRIDO |

> **BARRIDO:** Indica que la proyección de corte supera la semana 52 del año.

---

## 8. Módulo Inventario

**Acceso:** Solo Administrador

### 8.1 Ver Productos

1. Navegue a **Inventario** en el menú lateral
2. Se muestra la lista de productos con:
   - Código
   - Nombre
   - Unidad de medida
   - Stock mínimo
   - Stock actual
   - Estado (Activo/Inactivo)
   - Acciones

### 8.2 Crear Producto

1. Haga clic en **Nuevo Producto**
2. Complete:
   - **Código:** Identificador único
   - **Nombre:** Nombre del producto/insumo
   - **Descripción:** Detalle del producto (opcional)
   - **Unidad:** Unidad de medida (ej: kg, litros, unidades)
   - **Stock Mínimo:** Cantidad mínima antes de generar alerta
3. Haga clic en **Crear**

### 8.3 Editar/Desactivar Producto

- Use los íconos de **lápiz** (editar) o **basurero** (desactivar)

### 8.4 Registrar Movimiento de Inventario

1. Haga clic en **Nuevo Movimiento**
2. Seleccione:
   - **Producto:** Insumo a mover
   - **Tipo:** Entrada o Salida
   - **Cantidad:** Número de unidades
   - **Lote:** Parcela asociada (opcional)
   - **Referencia:** Número de orden o documento
   - **Notas:** Observaciones
3. Haga clic en **Guardar**

### 8.5 Alertas de Stock Bajo

- Los productos cuyo stock actual está por debajo del stock mínimo aparecen destacados en el Dashboard bajo "Bajo Stock"
- Se recomienda revisar periódicamente el inventario y reabastecer a tiempo

---

## 9. Módulo Órdenes

**Acceso:** Solo Administrador
**Estado:** En desarrollo

Este módulo permitirá gestionar órdenes de embarque para la exportación de bananas. Funcionalidad pendiente de implementación.

---

## 10. Módulo Reportes

**Acceso:** Solo Administrador
**Estado:** En desarrollo

Módulo pendiente para la generación de reportes exportables en PDF/Excel.

---

## 11. Configuración

**Acceso:** Solo Administrador
**Estado:** En desarrollo

Módulo pendiente para configuración avanzada del sistema.

---

## 12. Gestión de Sesión

### 12.1 Cambiar Contraseña

1. Use la funcionalidad de cambio de contraseña desde la API
2. Proporcione su contraseña actual y la nueva contraseña

### 12.2 Cerrar Sesión

1. Haga clic en el botón **Cerrar Sesión** en la barra lateral inferior
2. Será redirigido a la pantalla de login

---

## Apéndice A: Referencia Rápida

### Atajos de Navegación

| Módulo | Ruta |
|--------|------|
| Dashboard | /dashboard |
| Usuarios | /users |
| Lotes | /lotes |
| Labores | /tareas |
| Cosecha | /cosecha |
| Inventario | /inventario |
| Órdenes | /ordenes |
| Reportes | /reportes |
| Configuración | /configuracion |

### Permisos por Rol

| Módulo | Administrador | Supervisor | Obrero |
|--------|:---:|:---:|:---:|
| Dashboard | ✅ | ✅ | ✅ |
| Usuarios | ✅ | ❌ | ❌ |
| Lotes | ✅ | ❌ | ❌ |
| Labores | ✅ | ✅ | ✅ |
| Cosecha | ✅ | ✅ | ✅ |
| Inventario | ✅ | ❌ | ❌ |
| Órdenes | ✅ | ❌ | ❌ |
| Reportes | ✅ | ❌ | ❌ |
| Configuración | ✅ | ❌ | ❌ |
