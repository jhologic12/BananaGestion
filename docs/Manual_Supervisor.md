# Manual de Usuario - Supervisor
## Sistema de Gestión de Finca Bananera - BananaGestion

---

## Tabla de Contenidos

1. [Introducción](#1-introducción)
2. [Acceso al Sistema](#2-acceso-al-sistema)
3. [Dashboard](#3-dashboard)
4. [Módulo Labores](#4-módulo-labores)
5. [Módulo Cosecha](#5-módulo-cosecha)
6. [Limitaciones del Rol](#6-limitaciones-del-rol)
7. [Gestión de Sesión](#7-gestión-de-sesión)
8. [Apéndice: Referencia Rápida](#apéndice-referencia-rápida)

---

## 1. Introducción

### 1.1 Descripción del Sistema

BananaGestion es un sistema completo para la gestión de una finca bananera. Permite controlar labores, cosecha, inventario y órdenes de embarque de manera centralizada.

### 1.2 Rol del Supervisor

El **Supervisor** tiene acceso a los módulos necesarios para supervisar las operaciones diarias de campo:

| Módulo | Acceso |
|--------|:---:|
| Dashboard | ✅ Ver |
| Labores | ✅ Ver y gestionar |
| Cosecha | ✅ Ver y registrar |

### 1.3 Módulos No Accesibles

Los siguientes módulos **NO** están disponibles para el rol de Supervisor:

| Módulo | Motivo |
|--------|--------|
| Usuarios | Gestión administrativa reservada al Administrador |
| Lotes | Configuración de infraestructura reservada al Administrador |
| Inventario | Control de stock reservado al Administrador |
| Órdenes | Módulo en desarrollo, acceso restringido |
| Reportes | Módulo en desarrollo, acceso restringido |
| Configuración | Configuración del sistema reservada al Administrador |

> **Nota:** Si intenta acceder directamente a estas URLs, el sistema lo redirigirá automáticamente al Dashboard.

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
2. Ingrese su **correo electrónico** y **contraseña** asignados por el Administrador
3. Haga clic en **Iniciar Sesión**

### 2.2 Usuario Inactivo

Si su cuenta ha sido desactivada por el Administrador, verá el mensaje **"Usuario inactivo"** al intentar iniciar sesión. Contacte al Administrador para que reactive su cuenta.

---

## 3. Dashboard

El Dashboard es la página principal. Muestra un resumen general del estado de la finca.

### 3.1 Tarjetas de Métricas

| Tarjeta | Descripción |
|---------|-------------|
| **Tareas Pendientes** | Número total de tareas asignadas pendientes de completar |
| **Tareas Vencidas** | Tareas cuya fecha programada ya pasó sin ser completadas |
| **Racimos Cortados** | Total de racimos cosechados (estado "Cortado") a la fecha |
| **Bajo Stock** | Productos de inventario con stock por debajo del mínimo (solo informativo) |
| **Obreros Activos** | Número de trabajadores con rol "Obrero" registrados |
| **Lotes Activos** | Número de parcelas habilitadas en el sistema |

### 3.2 Proyección de Cosecha

Muestra las próximas 4 semanas con datos de encinte y la cantidad de racimos encintados. Cada semana tiene asociado un color de cinta rotativo.

### 3.3 Actividad Reciente

Lista las últimas 5 tareas con su estado actual (Pendiente, EnProgreso, Completada).

---

## 4. Módulo Labores

### 4.1 Navegación

1. Haga clic en **Labores** en el menú lateral
2. Verá tres pestañas: **Configuración**, **Pendientes** y **Vencidas**

### 4.2 Pestaña: Configuración de Tareas

Como Supervisor, puede **crear, editar y eliminar** tipos de tarea.

#### 4.2.1 Crear Tipo de Tarea

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

#### 4.2.2 Editar Configuración

1. Haga clic en el ícono de **lápiz** en la configuración deseada
2. Modifique los campos
3. Haga clic en **Actualizar**

#### 4.2.3 Eliminar Configuración

1. Haga clic en el ícono de **basurero** en la configuración deseada
2. Confirme la eliminación

### 4.3 Pestaña: Tareas Pendientes

Lista todas las tareas asignadas con su estado:

| Estado | Descripción |
|--------|-------------|
| **Pendiente** | Tarea programada sin iniciar |
| **EnProgreso** | Tarea en ejecución |
| **Completada** | Tarea finalizada |

#### 4.3.1 Crear Asignación de Tarea

1. Haga clic en **Nueva Asignación**
2. Seleccione:
   - **Configuración:** Tipo de tarea a asignar
   - **Usuario:** Trabajador responsable (Obrero disponible)
   - **Lote:** Parcela donde se ejecutará
   - **Fecha Programada:** Cuándo debe realizarse
   - **Notas:** Observaciones adicionales
3. Haga clic en **Crear**

#### 4.3.2 Ver Detalle de Tarea

Cada tarea en la lista muestra:
- Nombre de la configuración
- Lote asignado
- Usuario responsable
- Fecha programada
- Estado actual

### 4.4 Pestaña: Tareas Vencidas

Muestra las tareas cuya fecha programada ya pasó y no han sido completadas. Útil para identificar tareas atrasadas y reprogramarlas.

### 4.5 Cambiar Estado de Tarea

1. Localice la tarea en la lista de pendientes
2. Haga clic en el botón de cambio de estado
3. Seleccione: **Pendiente**, **EnProgreso** o **Completada**

---

## 5. Módulo Cosecha

### 5.1 Navegación

1. Haga clic en **Cosecha** en el menú lateral
2. Verá tres pestañas: **Encinte**, **Cosecha** y **Proyección**

### 5.2 Pestaña: Encinte

Muestra las 52 semanas del año organizadas en tarjetas con su color de cinta y la cantidad de racimos encintados.

#### 5.2.1 Registrar Encinte

1. Haga clic en **Nuevo Encinte**
2. Complete el formulario:

| Campo | Descripción |
|-------|-------------|
| **Lote** | Seleccione la parcela donde se realizó el encinte |
| **Semana de Encinte** | Semana del año (1-52) con su color de cinta asociado |
| **Cantidad de Racimos Embolsados** | Número total de racimos encintados |
| **Notas** | Observaciones adicionales (opcional) |

3. Haga clic en **Guardar**

#### 5.2.2 Tabla de Colores de Cinta

Cada semana del año tiene un color de cinta asignado que se repite en ciclos de 8 semanas:

| Semanas | Color |
|---------|-------|
| 1, 9, 17, 25, 33, 41, 49 | Verde |
| 2, 10, 18, 26, 34, 42, 50 | Amarillo |
| 3, 11, 19, 27, 35, 43, 51 | Blanco |
| 4, 12, 20, 28, 36, 44, 52 | Azul |
| 5, 13, 21, 29, 37, 45 | Rojo |
| 6, 14, 22, 30, 38, 46 | Café |
| 7, 15, 23, 31, 39, 47 | Negro |
| 8, 16, 24, 32, 40, 48 | Naranja |

### 5.3 Pestaña: Cosecha

Muestra el seguimiento completo de cada semana de encinte:

| Dato | Descripción |
|------|-------------|
| **Encinte** | Total de racimos encintados en esa semana |
| **Semitallo** | Racimos marcados el sábado |
| **Cortados** | Racimos cosechados el lunes |
| **Pendientes** | Racimos aún por cosechar |
| **Proyección** | Semanas estimadas para el corte |

#### 5.3.1 Registrar Semitallo

1. Haga clic en **Registrar Semitallo/Corte**
2. Seleccione:
   - **Semana de Encinte Referencia:** Semana del encinte origen
   - **Lote:** Parcela donde se realizará
   - **Tipo de Registro:** Seleccione **Semitallo (Sábado - Marcar racimos)**
   - **Cantidad de Racimos:** Número de racimos marcados
   - **Notas:** Observaciones (opcional)
3. Haga clic en **Guardar**

#### 5.3.2 Registrar Corte

1. Haga clic en **Registrar Semitallo/Corte**
2. Seleccione:
   - **Semana de Encinte Referencia:** Semana del encinte origen
   - **Lote:** Parcela donde se realizará
   - **Tipo de Registro:** Seleccione **Cortado (Lunes - Cosecha)**
   - **Cantidad de Racimos:** Número de racimos cosechados
   - **Notas:** Observaciones (opcional)
3. Haga clic en **Guardar**

#### 5.3.3 Regla de Validación - Sin Valores Negativos

> **IMPORTANTE:** El sistema no permite registrar más racimos de los disponibles.

La fórmula de cálculo es:

```
Disponibles = Encintados - Semitallo - Cortados
```

Si intenta registrar una cantidad mayor a los disponibles, el sistema mostrará un error con el siguiente detalle:

- Cantidad disponible para esa semana
- Total encintados
- Total semitallo registrado
- Total cortado registrado

**Ejemplo de mensaje de error:**
> "No hay suficientes racimos para corte. Disponibles para la semana 9: 50. Encintados: 166, Semitallo: 60, Cortados: 56"

### 5.4 Pestaña: Proyección

Tabla resumen con la proyección de cosecha basada en datos de encinte:

| Columna | Descripción |
|---------|-------------|
| **Semana Encinte** | Semana del encinte con su color de cinta |
| **Color** | Nombre del color asignado |
| **Encintados** | Total de racimos encintados en esa semana |
| **Semitallo** | Total de racimos marcados |
| **Cortados** | Total de racimos cosechados |
| **Pendientes** | Racimos aún por cosechar (Encintados - Semitallo - Cortados) |
| **Proyección Corte** | Rango de semanas estimadas para cosecha (S min - S máx) |
| **Estado** | Completado / En Proceso / BARRIDO |

> **BARRIDO:** Indica que la proyección de corte supera la semana 52 del año. Los racimos pendientes se cosecharían en el siguiente año.

---

## 6. Limitaciones del Rol

### 6.1 Módulos No Accesibles

El Supervisor **NO** tiene acceso a los siguientes módulos:

| Módulo | ¿Por qué no? |
|--------|-------------|
| **Usuarios** | La gestión de usuarios es responsabilidad exclusiva del Administrador |
| **Lotes** | La creación y configuración de lotes es responsabilidad del Administrador |
| **Inventario** | El control de stock de insumos es responsabilidad del Administrador |
| **Órdenes** | Módulo en desarrollo, acceso restringido |
| **Reportes** | Módulo en desarrollo, acceso restringido |
| **Configuración** | La configuración del sistema es responsabilidad del Administrador |

### 6.2 Redirección Automática

Si intenta acceder directamente a una URL restringida (ej: `/users`, `/lotes`, `/inventario`), el sistema lo **redirigirá automáticamente al Dashboard**.

### 6.3 Solicitar Acceso

Si necesita acceso a algún módulo restringido, contacte al Administrador del sistema.

---

## 7. Gestión de Sesión

### 7.1 Cerrar Sesión

1. Haga clic en el botón **Cerrar Sesión** ubicado en la parte inferior de la barra lateral
2. Será redirigido a la pantalla de login

### 7.2 Usuario Inactivo

Si su cuenta ha sido desactivada por el Administrador, no podrá iniciar sesión. El sistema mostrará el mensaje **"Usuario inactivo"**. Contacte al Administrador para que reactive su cuenta usando el botón de activar (ícono verde ⚡).

---

## Apéndice: Referencia Rápida

### Atajos de Navegación Disponibles

| Módulo | Ruta |
|--------|------|
| Dashboard | /dashboard |
| Labores | /tareas |
| Cosecha | /cosecha |

### URLs Restringidas (No accesibles para Supervisor)

| Módulo | Ruta |
|--------|------|
| Usuarios | /users |
| Lotes | /lotes |
| Inventario | /inventario |
| Órdenes | /ordenes |
| Reportes | /reportes |
| Configuración | /configuracion |

### Flujo de Trabajo Típico del Supervisor

1. **Inicio de día:** Revisar Dashboard → Tareas Pendientes y Vencidas
2. **Planificación:** Crear asignaciones de tareas en Labores
3. **Seguimiento:** Verificar estado de tareas y cambiar estados según avance
4. **Cosecha:** Registrar encinte y cosecha (semitallo/corte) en Cosecha
5. **Proyección:** Consultar proyección para planificar próximas semanas
6. **Fin de día:** Verificar tareas completadas y cerrar sesión
