import { useEffect, useState } from 'react';
import { toast } from 'react-hot-toast';
import { Card } from '../components/ui/Card';
import { ClipboardList, Package, Scissors, AlertTriangle, Users, MapPin } from 'lucide-react';
import { tareaService, cosechaService, inventarioService, loteService, userService } from '../services/api';

export function DashboardPage() {
  const [stats, setStats] = useState({
    tareasPendientes: 0,
    tareasVencidas: 0,
    productosBajoStock: 0,
    racimosMes: 0,
    totalObreros: 0,
    totalLotes: 0,
  });
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    loadDashboard();
  }, []);

  const loadDashboard = async () => {
    try {
      const [assignments, overdue, lowStock, cosechas, lotes, users] = await Promise.all([
        tareaService.getAssignments(),
        tareaService.getOverdue(),
        inventarioService.getLowStock(),
        cosechaService.getCosechas(new Date().getFullYear()),
        loteService.getActive(),
        userService.getObreros(),
      ]);

      const racimos = cosechas.data
        .filter((r: any) => r.estado === 'Cortado')
        .reduce((sum: number, r: any) => sum + (r.cantidadRacimos || 0), 0);

      setStats({
        tareasPendientes: assignments.data.length,
        tareasVencidas: overdue.data.length,
        productosBajoStock: lowStock.data.length,
        racimosMes: racimos,
        totalObreros: users.data.length,
        totalLotes: lotes.data.length,
      });
    } catch (error) {
      toast.error('Error al cargar dashboard');
    } finally {
      setLoading(false);
    }
  };

  const statCards = [
    { title: 'Tareas Pendientes', value: stats.tareasPendientes, icon: ClipboardList, color: 'text-blue-600', bg: 'bg-blue-50' },
    { title: 'Tareas Vencidas', value: stats.tareasVencidas, icon: AlertTriangle, color: 'text-red-600', bg: 'bg-red-50' },
    { title: 'Racimos Cortados', value: stats.racimosMes, icon: Scissors, color: 'text-primary-600', bg: 'bg-primary-50' },
    { title: 'Bajo Stock', value: stats.productosBajoStock, icon: Package, color: 'text-orange-600', bg: 'bg-orange-50' },
    { title: 'Obreros Activos', value: stats.totalObreros, icon: Users, color: 'text-purple-600', bg: 'bg-purple-50' },
    { title: 'Lotes Activos', value: stats.totalLotes, icon: MapPin, color: 'text-green-600', bg: 'bg-green-50' },
  ];

  if (loading) {
    return (
      <div className="flex items-center justify-center h-64">
        <div className="animate-spin rounded-full h-8 w-8 border-b-2 border-primary-600"></div>
      </div>
    );
  }

  return (
    <div>
      <h1 className="text-2xl font-bold text-gray-900 dark:text-white mb-6">Dashboard</h1>
      
      <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
        {statCards.map((stat, index) => (
          <Card key={index} className="flex items-center gap-4">
            <div className={`p-3 rounded-lg ${stat.bg}`}>
              <stat.icon className={`w-6 h-6 ${stat.color}`} />
            </div>
            <div>
              <p className="text-sm text-gray-500 dark:text-gray-400">{stat.title}</p>
              <p className="text-2xl font-bold text-gray-900 dark:text-white">{stat.value}</p>
            </div>
          </Card>
        ))}
      </div>

      <div className="grid grid-cols-1 lg:grid-cols-2 gap-6 mt-6">
        <Card title="Proyección de Cosecha">
          <ProyeccionCosecha />
        </Card>
        <Card title="Actividad Reciente">
          <ActividadReciente />
        </Card>
      </div>
    </div>
  );
}

function ProyeccionCosecha() {
  const [projection, setProjection] = useState<any[]>([]);

  useEffect(() => {
    cosechaService.getProyeccion().then((res) => setProjection(res.data.slice(0, 4)));
  }, []);

  return (
    <div className="space-y-3">
      {projection.length > 0 ? (
        projection.map((p: any) => (
          <div key={`${p.anoEncinte}-${p.semanaEncinte}`} className="flex justify-between items-center">
            <span className="text-sm text-gray-600 dark:text-gray-400">
              S{p.semanaEncinte} ({p.colorNombre || 'Sin color'})
            </span>
            <div className="flex items-center gap-2">
              <div className="w-32 h-2 bg-gray-200 dark:bg-gray-700 rounded-full overflow-hidden">
                <div
                  className="h-full bg-primary-500 rounded-full"
                  style={{ width: `${Math.min((p.encintados / 100) * 100, 100)}%` }}
                />
              </div>
              <span className="text-sm font-medium">{p.encintados} encintados</span>
            </div>
          </div>
        ))
      ) : (
        <p className="text-gray-500 text-sm">Sin datos disponibles</p>
      )}
    </div>
  );
}

function ActividadReciente() {
  const [tareas, setTareas] = useState<any[]>([]);

  useEffect(() => {
    tareaService.getAssignments().then((res) => setTareas(res.data.slice(0, 5)));
  }, []);

  return (
    <div className="space-y-3">
      {tareas.length > 0 ? (
        tareas.map((tarea) => (
          <div key={tarea.id} className="flex justify-between items-center py-2 border-b dark:border-gray-700 last:border-0">
            <div>
              <p className="text-sm font-medium text-gray-900 dark:text-white">{tarea.taskConfigNombre}</p>
              <p className="text-xs text-gray-500">{tarea.loteNombre} - {tarea.userNombre}</p>
            </div>
            <span className={`text-xs px-2 py-1 rounded-full ${
              tarea.status === 'Completada' ? 'bg-green-100 text-green-700' :
              tarea.status === 'EnProgreso' ? 'bg-blue-100 text-blue-700' :
              'bg-gray-100 text-gray-700'
            }`}>
              {tarea.status}
            </span>
          </div>
        ))
      ) : (
        <p className="text-gray-500 text-sm">Sin actividad reciente</p>
      )}
    </div>
  );
}
