# 🚀 BananaGestion - Guía de Despliegue

## Arquitectura de Producción

```
┌─────────────┐     HTTPS     ┌──────────────┐     HTTPS     ┌──────────────┐
│   Vercel    │──────────────▶│   Render     │──────────────▶│   Supabase   │
│  (Frontend) │               │   (API .NET) │               │ (PostgreSQL) │
│  React/Vite │               │   Port 5000  │               │   Port 5432  │
└─────────────┘               └──────────────┘               └──────────────┘
```

---

## 1️⃣ Supabase (Base de Datos)

1. Ve a [supabase.com](https://supabase.com/dashboard) → **New Project**
2. Configura:
   - **Name**: `bananagestion`
   - **Database Password**: ⚠️ Guárdala (no se muestra de nuevo)
   - **Region**: Elige la más cercana a tus usuarios
3. Espera ~2 minutos a que se cree el proyecto
4. Ve a **Settings** (engranaje) → **Database**
5. En **Connection string**, selecciona **URI** y copia:
   ```
   postgresql://postgres.[PROJECT_ID]:[PASSWORD]@aws-0-[REGION].pooler.supabase.com:6543/postgres
   ```
6. ⚠️ **Importante**: Si usas el pooler puerto `6543`, asegúrate de que funcione. Si hay problemas, usa el puerto directo `5432`.

---

## 2️⃣ Render (Backend API)

1. Ve a [render.com/dashboard](https://dashboard.render.com) → **New +** → **Blueprint**
2. Conecta tu repositorio: `jhologic12/BananaGestion`
3. El Blueprint detectará automáticamente el archivo `render.yaml`
4. **Configura las siguientes variables de entorno**:

| Variable | Valor |
|----------|-------|
| `ASPNETCORE_ENVIRONMENT` | `Production` |
| `ConnectionStrings__DefaultConnection` | `postgresql://postgres.[PROJECT]:[PASSWORD]@aws-0-[REGION].pooler.supabase.com:6543/postgres` |
| `Jwt__Key` | `tlhwEUA/I51Q5Vm3XaW38Quj0Yp4iUhifWtLD4wXBfZVLpvTPtyPrUoxj74IQkuwBHJYPfjwdJlo9y5/qCDNpw==` |
| `Jwt__Issuer` | `BananaGestion` |
| `Jwt__Audience` | `BananaGestionApp` |
| `Jwt__ExpireMinutes` | `60` |
| `AllowedOrigins__0` | *(Dejar vacío por ahora, se configura después de Vercel)* |

5. Click **Apply** → Render construirá y desplegará automáticamente
6. ⏳ Espera ~3-5 minutos (el primer despliegue en free tier puede tardar)
7. Anota la URL de tu API: `https://bananagestion-api-xxxx.onrender.com`

---

## 3️⃣ Vercel (Frontend)

1. Ve a [vercel.com/new](https://vercel.com/new)
2. Importa el repositorio: `jhologic12/BananaGestion`
3. **Configura el build**:

| Setting | Valor |
|---------|-------|
| **Framework Preset** | `Vite` |
| **Root Directory** | `frontend` |
| **Build Command** | `npm run build` |
| **Output Directory** | `dist` |
| **Install Command** | `npm install` |

4. **Variables de Entorno**:

| Variable | Valor |
|----------|-------|
| `VITE_API_URL` | `https://bananagestion-api-xxxx.onrender.com/api` |

5. Click **Deploy**
6. ⏳ Espera ~1-2 minutos
7. Anota tu URL: `https://bananagestion-xxxx.vercel.app`

---

## 4️⃣ Configuración Final (CORS)

1. Vuelve a **Render Dashboard** → Tu servicio → **Environment**
2. Actualiza `AllowedOrigins__0` con la URL de Vercel:
   ```
   https://bananagestion-xxxx.vercel.app
   ```
3. Click **Save Changes** → El servicio se redeplegará automáticamente

---

## ✅ Verificación

1. **API Health**: Abre `https://tu-api.onrender.com/api/swagger` → Debe mostrar Swagger UI
2. **Frontend**: Abre `https://tu-app.vercel.app` → Debe cargar la página de login
3. **Login**: Registra un usuario nuevo → Se creará como `Obrero` por defecto
4. **Mobile**: Abre en tu teléfono o usa DevTools (F12 → modo móvil) → El menú hamburguesa debe funcionar

---

## 🔧 Solución de Problemas

### API no responde (502/503)
- Render free tier se "duerme" después de 15 min de inactividad → Primera petición tarda ~30s
- Revisa logs en Render Dashboard → **Logs** tab

### Error de CORS
- Verifica que `AllowedOrigins__0` tenga la URL exacta de Vercel (sin `/` al final)
- Debe ser `https://tu-app.vercel.app`, NO `https://tu-app.vercel.app/`

### Error de Base de Datos
- Verifica la conexión en Supabase → **Settings** → **Database**
- Si usas pooler, intenta cambiar puerto `6543` → `5432`
- Revisa logs de Render para ver el error exacto de Npgsql

### Frontend muestra errores de red
- Verifica que `VITE_API_URL` termine en `/api`
- Revisa Network tab en DevTools (F12)

---

## 🔐 Credenciales Generadas

### JWT Signing Key
```
tlhwEUA/I51Q5Vm3XaW38Quj0Yp4iUhifWtLD4wXBfZVLpvTPtyPrUoxj74IQkuwBHJYPfjwdJlo9y5/qCDNpw==
```
⚠️ **Guárdala en un lugar seguro**. Si la pierdes, todos los tokens existentes dejarán de funcionar.

---

## 📊 Monitoreo

- **Render Logs**: Dashboard → Tu servicio → **Logs**
- **Vercel Analytics**: Dashboard → Tu proyecto → **Analytics**
- **Supabase Logs**: Dashboard → Tu proyecto → **Settings** → **Logs**
