import { useEffect, useState } from 'react';
import { toast } from 'react-hot-toast';
import { Button } from '../components/ui/Button';
import { Card } from '../components/ui/Card';
import { Modal } from '../components/ui/Modal';
import { Input } from '../components/ui/Input';
import { Select } from '../components/ui/Select';
import { cosechaService, loteService } from '../services/api';
import { Plus, Calendar, AlertTriangle, Check, Scissors } from 'lucide-react';

interface HarvestCalendar {
  id: string;
  semana: number;
  ano: number;
  colorCinta: string;
  colorNombre?: string;
  fechaInicio: string;
  fechaFin: string;
  activo: boolean;
}

interface Encinte {
  id: string;
  loteId: string;
  loteNombre: string;
  semanaEncinte: number;
  anoEncinte: number;
  cantidadRacimosEmbolsados: number;
  colorCinta: string;
  fecha: string;
  notas?: string;
}

interface Cosecha {
  id: string;
  loteId: string;
  loteNombre: string;
  semanaEncinte: number;
  anoEncinte: number;
  semanaCosecha: number;
  anoCosecha: number;
  estado: 'Semitallo' | 'Cortado';
  cantidadRacimos: number;
  colorCinta: string;
  fecha: string;
  notas?: string;
}

interface Proyeccion {
  semanaEncinte: number;
  anoEncinte: number;
  encintados: number;
  semitallo: number;
  cortados: number;
  pendientes: number;
  semanaProyeccionMin: number;
  semanaProyeccionMax: number;
  isBarrido: boolean;
  colorCinta: string;
  colorNombre?: string;
}

interface Lote {
  id: string;
  codigo: string;
  nombre: string;
}

type TabType = 'encinte' | 'cosecha' | 'proyeccion';

export function CosechaPage() {
  const [activeTab, setActiveTab] = useState<TabType>('encinte');
  const [calendar, setCalendar] = useState<HarvestCalendar[]>([]);
  const [encintes, setEncintes] = useState<Encinte[]>([]);
  const [cosechas, setCosechas] = useState<Cosecha[]>([]);
  const [proyecciones, setProyecciones] = useState<Proyeccion[]>([]);
  const [lotes, setLotes] = useState<Lote[]>([]);
  const [currentYear, setCurrentYear] = useState(2026);
  const [loading, setLoading] = useState(true);
  
  const [encinteModalOpen, setEncinteModalOpen] = useState(false);
  const [cosechaModalOpen, setCosechaModalOpen] = useState(false);
  
  const [encinteForm, setEncinteForm] = useState({
    loteId: '',
    semanaEncinte: 1,
    cantidadRacimosEmbolsados: 0,
    notas: '',
  });
  
  const [cosechaForm, setCosechaForm] = useState({
    loteId: '',
    semanaEncinte: 1,
    estado: 'Semitallo' as 'Semitallo' | 'Cortado',
    cantidadRacimos: 0,
    notas: '',
  });

  useEffect(() => {
    loadData();
  }, [currentYear]);

  const loadData = async () => {
    try {
      setLoading(true);
      const [calRes, encRes, cosRes, proyRes, lotRes] = await Promise.all([
        cosechaService.getCalendar(currentYear),
        cosechaService.getEncinte(currentYear),
        cosechaService.getCosechas(currentYear),
        cosechaService.getProyeccion(currentYear),
        loteService.getActive(),
      ]);
      
      setCalendar(calRes.data);
      setEncintes(encRes.data);
      setCosechas(cosRes.data);
      setProyecciones(proyRes.data);
      setLotes(lotRes.data);
    } catch (error) {
      toast.error('Error al cargar datos');
    } finally {
      setLoading(false);
    }
  };

  const getEncinteForWeek = (semana: number) => {
    return encintes.filter(e => e.semanaEncinte === semana);
  };

  const getCosechasForEncinteWeek = (semana: number) => {
    return cosechas.filter(c => c.semanaEncinte === semana);
  };

  const getProyeccionForWeek = (semana: number) => {
    return proyecciones.find(p => p.semanaEncinte === semana);
  };

  const handleCreateEncinte = async () => {
    try {
      const semanaData = calendar.find(c => c.semana === encinteForm.semanaEncinte);
      await cosechaService.createEncinte({
        loteId: encinteForm.loteId,
        semanaEncinte: encinteForm.semanaEncinte,
        anoEncinte: currentYear,
        cantidadRacimosEmbolsados: encinteForm.cantidadRacimosEmbolsados,
        colorCinta: semanaData?.colorCinta || '#000000',
        fecha: new Date().toISOString(),
        notas: encinteForm.notas,
      });
      toast.success('Encinte registrado exitosamente');
      setEncinteModalOpen(false);
      setEncinteForm({ loteId: '', semanaEncinte: 1, cantidadRacimosEmbolsados: 0, notas: '' });
      loadData();
    } catch (error: any) {
      toast.error(error.response?.data?.message || error.response?.data?.title || 'Error al registrar encinte');
    }
  };

  const handleCreateCosecha = async () => {
    try {
      const encintesSemana = encintes.filter(e => e.semanaEncinte === cosechaForm.semanaEncinte);
      const cosechasSemana = cosechas.filter(c => c.semanaEncinte === cosechaForm.semanaEncinte);
      
      const totalEncintados = encintesSemana.reduce((sum, e) => sum + e.cantidadRacimosEmbolsados, 0);
      const totalSemitallo = cosechasSemana.filter(c => c.estado === 'Semitallo').reduce((sum, c) => sum + c.cantidadRacimos, 0);
      const totalCortados = cosechasSemana.filter(c => c.estado === 'Cortado').reduce((sum, c) => sum + c.cantidadRacimos, 0);
      const disponibles = totalEncintados - totalSemitallo - totalCortados;

      if (cosechaForm.cantidadRacimos <= 0) {
        toast.error('La cantidad de racimos debe ser mayor a cero');
        return;
      }

      if (cosechaForm.cantidadRacimos > disponibles) {
        toast.error(
          `No hay suficientes racimos para ${cosechaForm.estado === 'Semitallo' ? 'semitallo' : 'corte'}. ` +
          `Disponibles en la semana ${cosechaForm.semanaEncinte}: ${disponibles}. ` +
          `Encintados: ${totalEncintados}, Semitallo: ${totalSemitallo}, Cortados: ${totalCortados}`
        );
        return;
      }

      const semanaData = calendar.find(c => c.semana === cosechaForm.semanaEncinte);
      const hoy = new Date();
      const semanaActual = Math.ceil((hoy.getTime() - new Date(hoy.getFullYear(), 0, 1).getTime()) / (7 * 24 * 60 * 60 * 1000));
      
      await cosechaService.createCosecha({
        loteId: cosechaForm.loteId,
        semanaEncinte: cosechaForm.semanaEncinte,
        anoEncinte: currentYear,
        semanaCosecha: semanaActual,
        anoCosecha: currentYear,
        estado: cosechaForm.estado,
        cantidadRacimos: cosechaForm.cantidadRacimos,
        colorCinta: semanaData?.colorCinta || '#000000',
        fecha: new Date().toISOString(),
        notas: cosechaForm.notas,
      });
      toast.success(`${cosechaForm.estado === 'Semitallo' ? 'Semitallo' : 'Corte'} registrado exitosamente`);
      setCosechaModalOpen(false);
      setCosechaForm({ loteId: '', semanaEncinte: 1, estado: 'Semitallo', cantidadRacimos: 0, notas: '' });
      loadData();
    } catch (error: any) {
      toast.error(error.response?.data?.message || error.response?.data?.title || 'Error al registrar cosecha');
    }
  };

  const getWeekColor = (colorCinta: string) => {
    const colorMap: Record<string, string> = {
      '#00FF00': 'bg-green-500',
      '#FFFF00': 'bg-yellow-400',
      '#FFFFFF': 'bg-white border border-gray-300',
      '#0000FF': 'bg-blue-500',
      '#FF0000': 'bg-red-500',
      '#8B4513': 'bg-amber-800',
      '#000000': 'bg-black',
      '#FFA500': 'bg-orange-500',
    };
    return colorMap[colorCinta] || 'bg-gray-400';
  };

  const renderTabs = () => (
    <div className="flex border-b border-gray-200 mb-6">
      {[
        { id: 'encinte' as TabType, label: 'Encinte', icon: Calendar },
        { id: 'cosecha' as TabType, label: 'Cosecha', icon: Scissors },
        { id: 'proyeccion' as TabType, label: 'Proyección', icon: AlertTriangle },
      ].map(({ id, label, icon: Icon }) => (
        <button
          key={id}
          onClick={() => setActiveTab(id)}
          className={`flex items-center gap-2 px-6 py-3 text-sm font-medium transition-colors ${
            activeTab === id
              ? 'border-b-2 border-green-600 text-green-600'
              : 'text-gray-500 hover:text-gray-700'
          }`}
        >
          <Icon className="w-4 h-4" />
          {label}
        </button>
      ))}
    </div>
  );

  const renderEncinteTab = () => (
    <div>
      <div className="flex justify-between items-center mb-4">
        <h2 className="text-lg font-semibold text-gray-800">Registro de Encinte</h2>
        <Button onClick={() => setEncinteModalOpen(true)}>
          <Plus className="w-4 h-4 mr-2" />
          Nuevo Encinte
        </Button>
      </div>
      
      <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 xl:grid-cols-4 gap-4">
        {calendar.filter(c => c.activo).map((week) => {
          const encinteSemana = getEncinteForWeek(week.semana);
          const totalEncintados = encinteSemana.reduce((sum, e) => sum + e.cantidadRacimosEmbolsados, 0);
          
          return (
            <Card key={week.id} className="p-4">
              <div className="flex items-center gap-2 mb-3">
                <div className={`w-4 h-4 rounded ${getWeekColor(week.colorCinta)}`} />
                <span className="font-semibold">S{week.semana}</span>
                <span className="text-sm text-gray-500">{week.colorNombre}</span>
              </div>
              <div className="space-y-1 text-sm">
                <div className="flex justify-between">
                  <span className="text-gray-600">Encintados:</span>
                  <span className="font-medium text-white">{totalEncintados}</span>
                </div>
              </div>
              <div className="mt-3 pt-3 border-t border-gray-100">
                <p className="text-xs text-gray-500">
                  {new Date(week.fechaInicio).toLocaleDateString('es-ES', { day: 'numeric', month: 'short' })}
                </p>
              </div>
            </Card>
          );
        })}
      </div>
    </div>
  );

  const renderCosechaTab = () => (
    <div>
      <div className="flex justify-between items-center mb-4">
        <h2 className="text-lg font-semibold text-gray-800">Seguimiento de Cosecha</h2>
        <Button onClick={() => setCosechaModalOpen(true)}>
          <Plus className="w-4 h-4 mr-2" />
          Registrar Semitallo/Corte
        </Button>
      </div>
      
      <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 xl:grid-cols-4 gap-4">
        {calendar.filter(c => c.activo).map((week) => {
          const cosechasSemana = getCosechasForEncinteWeek(week.semana);
          const semitallo = cosechasSemana.filter(c => c.estado === 'Semitallo').reduce((sum, c) => sum + c.cantidadRacimos, 0);
          const cortados = cosechasSemana.filter(c => c.estado === 'Cortado').reduce((sum, c) => sum + c.cantidadRacimos, 0);
          const proyeccion = getProyeccionForWeek(week.semana);
          
          return (
            <Card key={week.id} className={`p-4 ${proyeccion?.isBarrido ? 'ring-2 ring-red-500' : ''}`}>
              <div className="flex items-center gap-2 mb-3">
                <div className={`w-4 h-4 rounded ${getWeekColor(week.colorCinta)}`} />
                <span className="font-semibold">S{week.semana}</span>
                <span className="text-sm text-gray-500">{week.colorNombre}</span>
                {proyeccion?.isBarrido && (
                  <span className="ml-auto bg-red-100 text-red-700 text-xs px-2 py-0.5 rounded-full flex items-center gap-1">
                    <AlertTriangle className="w-3 h-3" />
                    BARRIDO
                  </span>
                )}
              </div>
              
              <div className="space-y-2 text-sm">
                <div className="flex justify-between items-center">
                  <span className="text-gray-600 flex items-center gap-1">
                    <Calendar className="w-3 h-3" /> Encinte:
                  </span>
                  <span className="font-medium text-white">{proyeccion?.encintados || 0}</span>
                </div>
                <div className="flex justify-between items-center">
                  <span className="text-gray-600 flex items-center gap-1">
                    <Scissors className="w-3 h-3" /> Semitallo:
                  </span>
                  <span className="font-medium text-blue-600">{semitallo}</span>
                </div>
                <div className="flex justify-between items-center">
                  <span className="text-gray-600 flex items-center gap-1">
                    <Check className="w-3 h-3" /> Cortados:
                  </span>
                  <span className="font-medium text-green-600">{cortados}</span>
                </div>
                <div className="flex justify-between items-center">
                  <span className="text-gray-600">Pendientes:</span>
                  <span className={`font-medium ${(proyeccion?.pendientes || 0) > 0 ? 'text-amber-600' : 'text-white'}`}>
                    {proyeccion?.pendientes || 0}
                  </span>
                </div>
                <div className="pt-2 border-t border-gray-100">
                  <span className="text-xs text-gray-500">
                    Proyección: S{proyeccion?.semanaProyeccionMin || week.semana + 11} - S{proyeccion?.semanaProyeccionMax || week.semana + 13}
                  </span>
                </div>
              </div>
            </Card>
          );
        })}
      </div>
    </div>
  );

  const renderProyeccionTab = () => (
    <div>
      <div className="flex justify-between items-center mb-4">
        <h2 className="text-lg font-semibold text-gray-800">Proyección de Cosecha</h2>
      </div>
      
      <div className="overflow-x-auto">
        <table className="w-full">
          <thead>
            <tr className="bg-gray-50">
              <th className="px-4 py-3 text-left text-sm font-semibold text-gray-600">Semana Encinte</th>
              <th className="px-4 py-3 text-center text-sm font-semibold text-gray-600">Color</th>
              <th className="px-4 py-3 text-center text-sm font-semibold text-gray-600">Encintados</th>
              <th className="px-4 py-3 text-center text-sm font-semibold text-gray-600">Semitallo</th>
              <th className="px-4 py-3 text-center text-sm font-semibold text-gray-600">Cortados</th>
              <th className="px-4 py-3 text-center text-sm font-semibold text-gray-600">Pendientes</th>
              <th className="px-4 py-3 text-center text-sm font-semibold text-gray-600">Proyección Corte</th>
              <th className="px-4 py-3 text-center text-sm font-semibold text-gray-600">Estado</th>
            </tr>
          </thead>
          <tbody>
            {proyecciones.map((proy) => (
              <tr key={`${proy.anoEncinte}-${proy.semanaEncinte}`} className="border-b border-gray-100">
                <td className="px-4 py-3 text-sm">
                  <div className="flex items-center gap-2">
                    <div className={`w-3 h-3 rounded ${getWeekColor(proy.colorCinta)}`} />
                    <span className="font-medium">S{proy.semanaEncinte} ({proy.anoEncinte})</span>
                  </div>
                </td>
                <td className="px-4 py-3 text-center text-sm text-gray-600">
                  {proy.colorNombre}
                </td>
                <td className="px-4 py-3 text-center text-sm font-medium">
                  {proy.encintados}
                </td>
                <td className="px-4 py-3 text-center text-sm text-blue-600">
                  {proy.semitallo}
                </td>
                <td className="px-4 py-3 text-center text-sm text-green-600">
                  {proy.cortados}
                </td>
                <td className="px-4 py-3 text-center text-sm text-amber-600">
                  {proy.pendientes}
                </td>
                <td className="px-4 py-3 text-center text-sm text-gray-600">
                  S{proy.semanaProyeccionMin} - S{proy.semanaProyeccionMax}
                </td>
                <td className="px-4 py-3 text-center">
                  {proy.isBarrido ? (
                    <span className="bg-red-100 text-red-700 text-xs px-2 py-1 rounded-full flex items-center gap-1 mx-auto w-fit">
                      <AlertTriangle className="w-3 h-3" />
                      BARRIDO
                    </span>
                  ) : proy.cortados === proy.encintados ? (
                    <span className="bg-green-100 text-green-700 text-xs px-2 py-1 rounded-full">
                      Completado
                    </span>
                  ) : (
                    <span className="bg-yellow-100 text-yellow-700 text-xs px-2 py-1 rounded-full">
                      En Proceso
                    </span>
                  )}
                </td>
              </tr>
            ))}
          </tbody>
        </table>
        
        {proyecciones.length === 0 && (
          <div className="text-center py-12 text-gray-500">
            No hay proyecciones disponibles
          </div>
        )}
      </div>
    </div>
  );

  return (
    <div className="p-6">
      <div className="flex justify-between items-center mb-6">
        <div>
          <h1 className="text-2xl font-bold text-gray-900">Cosecha</h1>
          <p className="text-sm text-gray-500 mt-1">Gestión de encinte, semitallo y corte</p>
        </div>
        <div className="flex items-center gap-2">
          <Button variant="outline" onClick={() => setCurrentYear(y => y - 1)}>←</Button>
          <span className="font-semibold px-4">{currentYear}</span>
          <Button variant="outline" onClick={() => setCurrentYear(y => y + 1)}>→</Button>
        </div>
      </div>

      {renderTabs()}

      {loading ? (
        <div className="text-center py-12 text-gray-500">Cargando...</div>
      ) : (
        <>
          {activeTab === 'encinte' && renderEncinteTab()}
          {activeTab === 'cosecha' && renderCosechaTab()}
          {activeTab === 'proyeccion' && renderProyeccionTab()}
        </>
      )}

      <Modal
        isOpen={encinteModalOpen}
        onClose={() => setEncinteModalOpen(false)}
        title="Registrar Encinte"
      >
        <div className="space-y-4">
          <Select
            label="Lote"
            value={encinteForm.loteId}
            onChange={(e) => setEncinteForm(f => ({ ...f, loteId: e.target.value }))}
            options={[
              { value: '', label: 'Seleccione un lote' },
              ...lotes.map(l => ({ value: l.id, label: `${l.nombre} (${l.codigo})` }))
            ]}
          />
          
          <Select
            label="Semana de Encinte"
            value={encinteForm.semanaEncinte.toString()}
            onChange={(e) => setEncinteForm(f => ({ ...f, semanaEncinte: parseInt(e.target.value) }))}
            options={calendar.map(w => ({
              value: w.semana.toString(),
              label: `Semana ${w.semana} - ${w.colorNombre}`
            }))}
          />
          
          <Input
            label="Cantidad de Racimos Embolsados"
            type="number"
            min="0"
            value={encinteForm.cantidadRacimosEmbolsados}
            onChange={(e) => setEncinteForm(f => ({ ...f, cantidadRacimosEmbolsados: parseInt(e.target.value) || 0 }))}
          />
          
          <Input
            label="Notas (opcional)"
            value={encinteForm.notas}
            onChange={(e) => setEncinteForm(f => ({ ...f, notas: e.target.value }))}
          />
          
          <div className="flex justify-end gap-2 pt-4">
            <Button variant="outline" onClick={() => setEncinteModalOpen(false)}>Cancelar</Button>
            <Button onClick={handleCreateEncinte}>Guardar</Button>
          </div>
        </div>
      </Modal>

      <Modal
        isOpen={cosechaModalOpen}
        onClose={() => setCosechaModalOpen(false)}
        title="Registrar Semitallo o Corte"
      >
        <div className="space-y-4">
          <Select
            label="Semana de Encinte Referencia"
            value={cosechaForm.semanaEncinte.toString()}
            onChange={(e) => setCosechaForm(f => ({ ...f, semanaEncinte: parseInt(e.target.value) }))}
            options={calendar.map(w => ({
              value: w.semana.toString(),
              label: `Semana ${w.semana} - ${w.colorNombre}`
            }))}
          />
          
          <Select
            label="Lote"
            value={cosechaForm.loteId}
            onChange={(e) => setCosechaForm(f => ({ ...f, loteId: e.target.value }))}
            options={[
              { value: '', label: 'Seleccione un lote' },
              ...lotes.map(l => ({ value: l.id, label: `${l.nombre} (${l.codigo})` }))
            ]}
          />
          
          <Select
            label="Tipo de Registro"
            value={cosechaForm.estado}
            onChange={(e) => setCosechaForm(f => ({ ...f, estado: e.target.value as 'Semitallo' | 'Cortado' }))}
            options={[
              { value: 'Semitallo', label: 'Semitallo (Sábado - Marcar racimos)' },
              { value: 'Cortado', label: 'Cortado (Lunes - Cosecha)' }
            ]}
          />
          
          <Input
            label="Cantidad de Racimos"
            type="number"
            min="0"
            value={cosechaForm.cantidadRacimos}
            onChange={(e) => setCosechaForm(f => ({ ...f, cantidadRacimos: parseInt(e.target.value) || 0 }))}
          />
          
          <Input
            label="Notas (opcional)"
            value={cosechaForm.notas}
            onChange={(e) => setCosechaForm(f => ({ ...f, notas: e.target.value }))}
          />
          
          <div className="flex justify-end gap-2 pt-4">
            <Button variant="outline" onClick={() => setCosechaModalOpen(false)}>Cancelar</Button>
            <Button onClick={handleCreateCosecha}>Guardar</Button>
          </div>
        </div>
      </Modal>
    </div>
  );
}
