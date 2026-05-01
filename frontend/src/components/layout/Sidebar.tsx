import { Link, useLocation } from 'react-router-dom';
import {
  LayoutDashboard,
  Users,
  MapPin,
  ClipboardList,
  Package,
  Scissors,
  ShoppingCart,
  BarChart3,
  Settings,
  LogOut,
  X,
} from 'lucide-react';
import { useAuthStore } from '../../store/authStore';

interface SidebarProps {
  isOpen: boolean;
  onClose: () => void;
}

const menuItems = [
  { path: '/dashboard', icon: LayoutDashboard, label: 'Dashboard', roles: ['Administrador', 'Supervisor', 'Obrero'] },
  { path: '/users', icon: Users, label: 'Usuarios', roles: ['Administrador'] },
  { path: '/lotes', icon: MapPin, label: 'Lotes', roles: ['Administrador'] },
  { path: '/tareas', icon: ClipboardList, label: 'Labores', roles: ['Administrador', 'Supervisor', 'Obrero'] },
  { path: '/cosecha', icon: Scissors, label: 'Cosecha', roles: ['Administrador', 'Supervisor', 'Obrero'] },
  { path: '/inventario', icon: Package, label: 'Inventario', roles: ['Administrador'] },
  { path: '/ordenes', icon: ShoppingCart, label: 'Órdenes', roles: ['Administrador'] },
  { path: '/reportes', icon: BarChart3, label: 'Reportes', roles: ['Administrador'] },
  { path: '/configuracion', icon: Settings, label: 'Configuración', roles: ['Administrador'] },
];

export function Sidebar({ isOpen, onClose }: SidebarProps) {
  const location = useLocation();
  const { user, logout } = useAuthStore();

  const filteredItems = menuItems.filter(
    (item) => !item.roles || item.roles.includes(user?.rol || '')
  );

  return (
    <>
      {isOpen && (
        <div
          className="fixed inset-0 bg-black/50 z-40 md:hidden"
          onClick={onClose}
        />
      )}
      <aside className={`
        fixed inset-y-0 left-0 z-50 w-64 bg-white dark:bg-gray-900 border-r border-gray-200 dark:border-gray-700 flex flex-col
        transform transition-transform duration-300 ease-in-out
        md:relative md:translate-x-0
        ${isOpen ? 'translate-x-0' : '-translate-x-full'}
      `}>
        <div className="flex items-center justify-between p-6 border-b border-gray-200 dark:border-gray-700">
          <div>
            <h1 className="text-xl font-bold text-primary-600">BananaGestion</h1>
            <p className="text-sm text-gray-500 dark:text-gray-400">Gestión de Finca</p>
          </div>
          <button
            onClick={onClose}
            className="md:hidden p-1 rounded-lg hover:bg-gray-100 dark:hover:bg-gray-800"
          >
            <X className="w-5 h-5 text-gray-500" />
          </button>
        </div>

        <nav className="flex-1 p-4 space-y-1 overflow-y-auto">
          {filteredItems.map((item) => {
            const isActive = location.pathname === item.path;
            return (
              <Link
                key={item.path}
                to={item.path}
                onClick={() => onClose()}
                className={`flex items-center gap-3 px-4 py-2 rounded-lg transition-colors ${
                  isActive
                    ? 'bg-primary-50 text-primary-600 dark:bg-primary-900/30'
                    : 'text-gray-600 dark:text-gray-400 hover:bg-gray-100 dark:hover:bg-gray-800'
                }`}
              >
                <item.icon className="w-5 h-5" />
                <span>{item.label}</span>
              </Link>
            );
          })}
        </nav>

        <div className="p-4 border-t border-gray-200 dark:border-gray-700">
          <div className="mb-4 px-4">
            <p className="font-medium text-gray-900 dark:text-white">{user?.nombre} {user?.apellido}</p>
            <p className="text-sm text-gray-500 dark:text-gray-400">{user?.rol}</p>
          </div>
          <button
            onClick={logout}
            className="flex items-center gap-3 px-4 py-2 w-full rounded-lg text-gray-600 dark:text-gray-400 hover:bg-gray-100 dark:hover:bg-gray-800 transition-colors"
          >
            <LogOut className="w-5 h-5" />
            <span>Cerrar Sesión</span>
          </button>
        </div>
      </aside>
    </>
  );
}
