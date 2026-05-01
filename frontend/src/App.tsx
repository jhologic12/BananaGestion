import { BrowserRouter, Routes, Route, Navigate } from 'react-router-dom';
import { Toaster } from 'react-hot-toast';
import { Layout } from './components/layout/Layout';
import { LoginPage } from './pages/LoginPage';
import { RegisterPage } from './pages/RegisterPage';
import { DashboardPage } from './pages/DashboardPage';
import { LotesPage } from './pages/LotesPage';
import { TareasPage } from './pages/TareasPage';
import { CosechaPage } from './pages/CosechaPage';
import { InventarioPage } from './pages/InventarioPage';
import { UsersPage } from './pages/UsersPage';
import { useAuthStore } from './store/authStore';

function ProtectedRoute({ children }: { children: React.ReactNode }) {
  const { isAuthenticated } = useAuthStore();
  return isAuthenticated ? <>{children}</> : <Navigate to="/login" />;
}

function RoleRoute({ children, allowedRoles }: { children: React.ReactNode; allowedRoles: string[] }) {
  const { user } = useAuthStore();
  if (!allowedRoles.includes(user?.rol || '')) {
    return <Navigate to="/dashboard" />;
  }
  return <>{children}</>;
}

function App() {
  return (
    <BrowserRouter>
      <Toaster position="top-right" />
      <Routes>
        <Route path="/login" element={<LoginPage />} />
        <Route path="/register" element={<RegisterPage />} />
        <Route
          path="/"
          element={
            <ProtectedRoute>
              <Layout />
            </ProtectedRoute>
          }
        >
          <Route index element={<Navigate to="/dashboard" />} />
          <Route path="dashboard" element={<DashboardPage />} />
          <Route path="lotes" element={<RoleRoute allowedRoles={['Administrador']}><LotesPage /></RoleRoute>} />
          <Route path="tareas" element={<TareasPage />} />
          <Route path="cosecha" element={<CosechaPage />} />
          <Route path="inventario" element={<RoleRoute allowedRoles={['Administrador']}><InventarioPage /></RoleRoute>} />
          <Route path="users" element={<RoleRoute allowedRoles={['Administrador']}><UsersPage /></RoleRoute>} />
          <Route path="ordenes" element={<RoleRoute allowedRoles={['Administrador']}><div className="text-center py-12">Órdenes - En desarrollo</div></RoleRoute>} />
          <Route path="reportes" element={<RoleRoute allowedRoles={['Administrador']}><div className="text-center py-12">Reportes - En desarrollo</div></RoleRoute>} />
          <Route path="configuracion" element={<RoleRoute allowedRoles={['Administrador']}><div className="text-center py-12">Configuración - En desarrollo</div></RoleRoute>} />
        </Route>
      </Routes>
    </BrowserRouter>
  );
}

export default App;
