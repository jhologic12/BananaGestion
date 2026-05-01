import { useEffect, useState } from 'react';
import { toast } from 'react-hot-toast';
import { Button } from '../components/ui/Button';
import { Card } from '../components/ui/Card';
import { Modal } from '../components/ui/Modal';
import { Input } from '../components/ui/Input';
import { Select } from '../components/ui/Select';
import { Table, TableRow, TableCell } from '../components/ui/Table';
import { tareaService, loteService, userService } from '../services/api';
import type { TaskAssignment, TaskConfig, Lote, User } from '../types';
import { Plus, Check, Clock, AlertTriangle, Settings, Edit2 } from 'lucide-react';
import { format } from 'date-fns';

export function TareasPage() {
  const [assignments, setAssignments] = useState<TaskAssignment[]>([]);
  const [configs, setConfigs] = useState<TaskConfig[]>([]);
  const [lotes, setLotes] = useState<Lote[]>([]);
  const [obreros, setObreros] = useState<User[]>([]);
  const [loading, setLoading] = useState(true);
  const [assignmentModalOpen, setAssignmentModalOpen] = useState(false);
  const [configModalOpen, setConfigModalOpen] = useState(false);
  const [editingConfig, setEditingConfig] = useState<TaskConfig | null>(null);
  const [tab, setTab] = useState<'pendientes' | 'configs'>('pendientes');
  const [configLoading, setConfigLoading] = useState(false);
  
  // Form state for config
  const [configForm, setConfigForm] = useState({
    nombre: '',
    tipoLabor: 'Fumigacion',
    tipo: 'Preventiva',
    frecuenciaDias: '',
    requiereFoto: 'false',
    requiereFirma: 'false',
    requiereGps: 'false',
    descripcion: '',
    activo: true,
  });

  // Form state for assignment
  const [assignmentForm, setAssignmentForm] = useState({
    taskConfigId: '',
    userId: '',
    loteId: '',
    fechaProgramada: '',
    notas: '',
  });

  useEffect(() => {
    loadData();
  }, []);

  const loadData = async () => {
    try {
      const [assignRes, configRes, loteRes, obreroRes] = await Promise.all([
        tareaService.getAssignments(),
        tareaService.getConfigs(),
        loteService.getActive(),
        userService.getObreros(),
      ]);
      setAssignments(assignRes.data);
      setConfigs(configRes.data);
      setLotes(loteRes.data);
      setObreros(obreroRes.data);
    } catch (error: any) {
      toast.error(error.response?.data?.message || 'Error al cargar datos');
    } finally {
      setLoading(false);
    }
  };

  const handleConfigChange = (e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>) => {
    setConfigForm({ ...configForm, [e.target.name]: e.target.value });
  };

  const handleAssignmentChange = (e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>) => {
    setAssignmentForm({ ...assignmentForm, [e.target.name]: e.target.value });
  };

  const openEditConfig = (config: TaskConfig) => {
    setEditingConfig(config);
    setConfigForm({
      nombre: config.nombre,
      tipoLabor: config.tipoLabor,
      tipo: config.tipo,
      frecuenciaDias: config.frecuenciaDias?.toString() || '',
      requiereFoto: config.requiereFoto ? 'true' : 'false',
      requiereFirma: config.requiereFirma ? 'true' : 'false',
      requiereGps: config.requiereGps ? 'true' : 'false',
      descripcion: config.descripcion || '',
      activo: config.activo,
    });
    setConfigModalOpen(true);
  };

  const handleCreateConfig = async () => {
    if (!configForm.nombre.trim()) {
      toast.error('El nombre es requerido');
      return;
    }
    
    setConfigLoading(true);
    try {
      const payload = {
        nombre: configForm.nombre,
        descripcion: configForm.descripcion,
        tipoLabor: configForm.tipoLabor,
        tipo: configForm.tipo,
        frecuenciaDias: configForm.frecuenciaDias ? parseInt(configForm.frecuenciaDias) : undefined,
        insumoId: undefined,
        requiereFoto: configForm.requiereFoto === 'true',
        requiereFirma: configForm.requiereFirma === 'true',
        requiereGps: configForm.requiereGps === 'true',
        activo: configForm.activo,
      };
      
      if (editingConfig) {
        await tareaService.updateConfig(editingConfig.id, payload);
        toast.success('Labor actualizada exitosamente');
      } else {
        await tareaService.createConfig(payload);
        toast.success('Labor creada exitosamente');
      }
      
      setConfigModalOpen(false);
      resetConfigForm();
      loadData();
    } catch (error: any) {
      toast.error(error.response?.data?.message || 'Error al guardar configuración');
    } finally {
      setConfigLoading(false);
    }
  };

  const handleCreateAssignment = async () => {
    const errors = [];
    if (!assignmentForm.taskConfigId) errors.push('Tipo de Labor');
    if (!assignmentForm.userId) errors.push('Obrero');
    if (!assignmentForm.loteId) errors.push('Lote');
    if (!assignmentForm.fechaProgramada) errors.push('Fecha');
    
    if (errors.length > 0) {
      toast.error(`Campos requeridos: ${errors.join(', ')}`);
      return;
    }
    
    try {
      await tareaService.createAssignment({
        taskConfigId: assignmentForm.taskConfigId,
        userId: assignmentForm.userId,
        loteId: assignmentForm.loteId,
        fechaProgramada: new Date(assignmentForm.fechaProgramada).toISOString(),
        notas: assignmentForm.notas,
      });
      toast.success('Asignación creada');
      setAssignmentModalOpen(false);
      resetAssignmentForm();
      loadData();
    } catch (error: any) {
      toast.error(error.response?.data?.message || 'Error al crear asignación');
    }
  };

  const updateStatus = async (id: string, status: string) => {
    try {
      await tareaService.updateStatus(id, status);
      toast.success('Estado actualizado');
      loadData();
    } catch (error: any) {
      toast.error(error.response?.data?.message || 'Error al actualizar');
    }
  };

  const getStatusIcon = (status: string) => {
    switch (status) {
      case 'Completada': return <Check className="w-4 h-4 text-green-600" />;
      case 'EnProgreso': return <Clock className="w-4 h-4 text-blue-600" />;
      default: return <AlertTriangle className="w-4 h-4 text-yellow-600" />;
    }
  };

  const resetConfigForm = () => {
    setEditingConfig(null);
    setConfigForm({
      nombre: '',
      tipoLabor: 'Fumigacion',
      tipo: 'Preventiva',
      frecuenciaDias: '',
      requiereFoto: 'false',
      requiereFirma: 'false',
      requiereGps: 'false',
      descripcion: '',
      activo: true,
    });
  };

  const resetAssignmentForm = () => {
    setAssignmentForm({
      taskConfigId: '',
      userId: '',
      loteId: '',
      fechaProgramada: '',
      notas: '',
    });
  };

  if (loading) return <div className="flex justify-center py-12">Cargando...</div>;

  return (
    <div>
      <div className="flex justify-between items-center mb-6">
        <h1 className="text-2xl font-bold text-gray-900 dark:text-white">Gestión de Labores</h1>
        <div className="flex gap-3">
          <Button onClick={() => { resetConfigForm(); setConfigModalOpen(true); }} variant="secondary">
            <Settings className="w-4 h-4" /> Nueva Labor
          </Button>
          <Button onClick={() => { resetAssignmentForm(); setAssignmentModalOpen(true); }}>
            <Plus className="w-4 h-4" /> Nueva Asignación
          </Button>
        </div>
      </div>

      {configs.filter(c => c.activo).length === 0 && (
        <div className="bg-yellow-50 border border-yellow-200 rounded-lg p-4 mb-6">
          <p className="text-yellow-800">
            <strong>Nota:</strong> No hay tipos de labor configurados. 
            <button 
              onClick={() => { resetConfigForm(); setConfigModalOpen(true); }} 
              className="underline ml-1 text-yellow-900"
            >
              Cree uno primero
            </button> 
            para poder crear asignaciones.
          </p>
        </div>
      )}

      <div className="flex gap-4 mb-6">
        <button
          onClick={() => setTab('pendientes')}
          className={`px-4 py-2 rounded-lg font-medium ${
            tab === 'pendientes' ? 'bg-primary-600 text-white' : 'bg-gray-200 dark:bg-gray-700'
          }`}
        >
          Asignaciones Pendientes
        </button>
        <button
          onClick={() => setTab('configs')}
          className={`px-4 py-2 rounded-lg font-medium ${
            tab === 'configs' ? 'bg-primary-600 text-white' : 'bg-gray-200 dark:bg-gray-700'
          }`}
        >
          Configuración de Labores ({configs.length})
        </button>
      </div>

      {tab === 'pendientes' ? (
        <Card>
          <Table headers={['Tarea', 'Obrero', 'Lote', 'Fecha', 'Estado', 'Acciones']}>
            {assignments.map((a) => (
              <TableRow key={a.id}>
                <TableCell className="font-medium">{a.taskConfigNombre}</TableCell>
                <TableCell>{a.userNombre}</TableCell>
                <TableCell>{a.loteNombre}</TableCell>
                <TableCell>{format(new Date(a.fechaProgramada), 'dd/MM/yyyy')}</TableCell>
                <TableCell>
                  <div className="flex items-center gap-2">
                    {getStatusIcon(a.status)}
                    <span>{a.status}</span>
                  </div>
                </TableCell>
                <TableCell>
                  {a.status !== 'Completada' && (
                    <Button size="sm" onClick={() => updateStatus(a.id, 'Completada')}>
                      Completar
                    </Button>
                  )}
                </TableCell>
              </TableRow>
            ))}
          </Table>
          {assignments.length === 0 && (
            <p className="text-center py-8 text-gray-500">No hay asignaciones pendientes</p>
          )}
        </Card>
      ) : (
        <Card>
          <Table headers={['Nombre', 'Tipo Labor', 'Tipo', 'Frecuencia', 'Requiere Foto', 'Activo', 'Acciones']}>
            {configs.map((c) => (
              <TableRow key={c.id}>
                <TableCell className="font-medium">{c.nombre}</TableCell>
                <TableCell>{c.tipoLabor}</TableCell>
                <TableCell>{c.tipo}</TableCell>
                <TableCell>{c.frecuenciaDias ? `${c.frecuenciaDias} días` : '-'}</TableCell>
                <TableCell>{c.requiereFoto ? 'Sí' : 'No'}</TableCell>
                <TableCell>
                  <span className={`px-2 py-1 text-xs rounded-full ${
                    c.activo ? 'bg-green-100 text-green-700' : 'bg-red-100 text-red-700'
                  }`}>
                    {c.activo ? 'Activo' : 'Inactivo'}
                  </span>
                </TableCell>
                <TableCell>
                  <button 
                    onClick={() => openEditConfig(c)} 
                    className="p-1 hover:bg-gray-100 dark:hover:bg-gray-700 rounded"
                    title="Editar"
                  >
                    <Edit2 className="w-4 h-4 text-blue-500" />
                  </button>
                </TableCell>
              </TableRow>
            ))}
          </Table>
          {configs.length === 0 && (
            <p className="text-center py-8 text-gray-500">No hay configuraciones. Cree una para comenzar.</p>
          )}
        </Card>
      )}

      {/* Modal para crear/editar asignación */}
      <Modal
        isOpen={assignmentModalOpen}
        onClose={() => { setAssignmentModalOpen(false); resetAssignmentForm(); }}
        title="Nueva Asignación"
      >
        <div className="space-y-4">
          <Select
            label="Tipo de Labor"
            name="taskConfigId"
            value={assignmentForm.taskConfigId}
            onChange={handleAssignmentChange}
            options={[
              { value: '', label: 'Seleccione una labor...' },
              ...configs.filter(c => c.activo).map(c => ({ value: c.id, label: c.nombre }))
            ]}
          />
          {configs.filter(c => c.activo).length === 0 && (
            <p className="text-sm text-yellow-600">Primero cree un tipo de labor</p>
          )}
          <Select
            label="Obrero"
            name="userId"
            value={assignmentForm.userId}
            onChange={handleAssignmentChange}
            options={[
              { value: '', label: 'Seleccione un obrero...' },
              ...obreros.map(u => ({ value: u.id, label: `${u.nombre} ${u.apellido}` }))
            ]}
          />
          <Select
            label="Lote"
            name="loteId"
            value={assignmentForm.loteId}
            onChange={handleAssignmentChange}
            options={[
              { value: '', label: 'Seleccione un lote...' },
              ...lotes.map(l => ({ value: l.id, label: l.nombre }))
            ]}
          />
          <Input
            label="Fecha Programada"
            type="date"
            name="fechaProgramada"
            value={assignmentForm.fechaProgramada}
            onChange={handleAssignmentChange}
          />
          <Input label="Notas" name="notas" value={assignmentForm.notas} onChange={handleAssignmentChange} />
          <div className="flex gap-3 pt-4">
            <Button variant="secondary" onClick={() => { setAssignmentModalOpen(false); resetAssignmentForm(); }} className="flex-1">
              Cancelar
            </Button>
            <Button onClick={handleCreateAssignment} className="flex-1" disabled={configs.filter(c => c.activo).length === 0}>
              Crear
            </Button>
          </div>
        </div>
      </Modal>

      {/* Modal para crear/editar tipo de labor */}
      <Modal
        isOpen={configModalOpen}
        onClose={() => { setConfigModalOpen(false); resetConfigForm(); }}
        title={editingConfig ? 'Editar Labor' : 'Nueva Tipo de Labor'}
      >
        <div className="space-y-4">
          <Input
            label="Nombre *"
            name="nombre"
            value={configForm.nombre}
            onChange={handleConfigChange}
            placeholder="Ej: Fumigación, Riego, Abonado"
          />
          <Select
            label="Tipo de Labor *"
            name="tipoLabor"
            value={configForm.tipoLabor}
            onChange={handleConfigChange}
            options={[
              { value: 'Fumigacion', label: 'Fumigación' },
              { value: 'Riego', label: 'Riego' },
              { value: 'Abonado', label: 'Abonado' },
              { value: 'Deshierba', label: 'Deshierba' },
              { value: 'Inspeccion', label: 'Inspección' },
              { value: 'Cosecha', label: 'Cosecha' },
              { value: 'Mantenimiento', label: 'Mantenimiento' },
              { value: 'Desmache', label: 'Desmache' },
              { value: 'Deshoja', label: 'Deshoja' },
              { value: 'Fertilizacion', label: 'Fertilización' },
              { value: 'Amarre', label: 'Amarre' },
              { value: 'Desbane', label: 'Desbane' },
              { value: 'Embolse', label: 'Embolse' },
              { value: 'Otro', label: 'Otro' },
            ]}
          />
          <Select
            label="Tipo *"
            name="tipo"
            value={configForm.tipo}
            onChange={handleConfigChange}
            options={[
              { value: 'Preventiva', label: 'Preventiva' },
              { value: 'Correctiva', label: 'Correctiva' },
              { value: 'Mantenimiento', label: 'Mantenimiento' },
            ]}
          />
          <Input
            label="Frecuencia (días)"
            type="number"
            name="frecuenciaDias"
            value={configForm.frecuenciaDias}
            onChange={handleConfigChange}
            placeholder="Ej: 7 para semanal"
          />
          <Select
            label="¿Requiere Foto? *"
            name="requiereFoto"
            value={configForm.requiereFoto}
            onChange={handleConfigChange}
            options={[
              { value: 'false', label: 'No' },
              { value: 'true', label: 'Sí' },
            ]}
          />
          <Select
            label="¿Requiere Firma? *"
            name="requiereFirma"
            value={configForm.requiereFirma}
            onChange={handleConfigChange}
            options={[
              { value: 'false', label: 'No' },
              { value: 'true', label: 'Sí' },
            ]}
          />
          <Select
            label="¿Requiere GPS? *"
            name="requiereGps"
            value={configForm.requiereGps}
            onChange={handleConfigChange}
            options={[
              { value: 'false', label: 'No' },
              { value: 'true', label: 'Sí' },
            ]}
          />
          <Input label="Descripción" name="descripcion" value={configForm.descripcion} onChange={handleConfigChange} placeholder="Detalles adicionales..." />
          {editingConfig && (
            <Select
              label="Estado"
              name="activo"
              value={configForm.activo ? 'true' : 'false'}
              onChange={handleConfigChange}
              options={[
                { value: 'true', label: 'Activo' },
                { value: 'false', label: 'Inactivo' },
              ]}
            />
          )}
          <div className="flex gap-3 pt-4">
            <Button variant="secondary" onClick={() => { setConfigModalOpen(false); resetConfigForm(); }} className="flex-1">
              Cancelar
            </Button>
            <Button onClick={handleCreateConfig} className="flex-1" disabled={configLoading}>
              {configLoading ? 'Guardando...' : (editingConfig ? 'Actualizar' : 'Crear')}
            </Button>
          </div>
        </div>
      </Modal>
    </div>
  );
}
