import { useEffect, useState } from 'react';
import { toast } from 'react-hot-toast';
import { Button } from '../components/ui/Button';
import { Card } from '../components/ui/Card';
import { Modal } from '../components/ui/Modal';
import { Input } from '../components/ui/Input';
import { Table, TableRow, TableCell } from '../components/ui/Table';
import { loteService } from '../services/api';
import type { Lote } from '../types';
import { Plus, Edit2, Trash2 } from 'lucide-react';
import { useForm } from 'react-hook-form';

export function LotesPage() {
  const [lotes, setLotes] = useState<Lote[]>([]);
  const [loading, setLoading] = useState(true);
  const [modalOpen, setModalOpen] = useState(false);
  const [editingLote, setEditingLote] = useState<Lote | null>(null);
  const { register, handleSubmit, reset, formState: { errors } } = useForm();

  useEffect(() => {
    loadLotes();
  }, []);

  const loadLotes = async () => {
    try {
      const res = await loteService.getAll();
      setLotes(res.data);
    } catch (error) {
      toast.error('Error al cargar lotes');
    } finally {
      setLoading(false);
    }
  };

  const onSubmit = async (data: any) => {
    try {
      if (editingLote) {
        await loteService.update(editingLote.id, data);
        toast.success('Lote actualizado');
      } else {
        await loteService.create(data);
        toast.success('Lote creado');
      }
      setModalOpen(false);
      setEditingLote(null);
      reset();
      loadLotes();
    } catch (error: any) {
      toast.error(error.response?.data?.message || 'Error al guardar');
    }
  };

  const openEdit = (lote: Lote) => {
    setEditingLote(lote);
    reset({
      codigo: lote.codigo,
      nombre: lote.nombre,
      hectareas: lote.hectareas,
      ubicacion: lote.ubicacion,
      notas: lote.notas,
    });
    setModalOpen(true);
  };

  const deleteLote = async (id: string) => {
    if (!confirm('¿Eliminar este lote?')) return;
    try {
      await loteService.delete(id);
      toast.success('Lote eliminado');
      loadLotes();
    } catch (error) {
      toast.error('Error al eliminar');
    }
  };

  if (loading) {
    return <div className="flex justify-center py-12">Cargando...</div>;
  }

  return (
    <div>
      <div className="flex justify-between items-center mb-6">
        <h1 className="text-2xl font-bold text-gray-900 dark:text-white">Lotes</h1>
        <Button onClick={() => { setEditingLote(null); reset(); setModalOpen(true); }}>
          <Plus className="w-4 h-4" /> Nuevo Lote
        </Button>
      </div>

      <Card>
        <Table headers={['Código', 'Nombre', 'Hectáreas', 'Ubicación', 'Estado', 'Acciones']}>
          {lotes.map((lote) => (
            <TableRow key={lote.id}>
              <TableCell className="font-medium">{lote.codigo}</TableCell>
              <TableCell>{lote.nombre}</TableCell>
              <TableCell>{lote.hectareas}</TableCell>
              <TableCell>{lote.ubicacion || '-'}</TableCell>
              <TableCell>
                <span className={`px-2 py-1 text-xs rounded-full ${
                  lote.activo ? 'bg-green-100 text-green-700' : 'bg-red-100 text-red-700'
                }`}>
                  {lote.activo ? 'Activo' : 'Inactivo'}
                </span>
              </TableCell>
              <TableCell>
                <div className="flex gap-2">
                  <button onClick={() => openEdit(lote)} className="p-1 hover:bg-gray-100 dark:hover:bg-gray-700 rounded">
                    <Edit2 className="w-4 h-4 text-gray-500" />
                  </button>
                  <button onClick={() => deleteLote(lote.id)} className="p-1 hover:bg-gray-100 dark:hover:bg-gray-700 rounded">
                    <Trash2 className="w-4 h-4 text-red-500" />
                  </button>
                </div>
              </TableCell>
            </TableRow>
          ))}
        </Table>
        {lotes.length === 0 && (
          <p className="text-center py-8 text-gray-500">No hay lotes registrados</p>
        )}
      </Card>

      <Modal
        isOpen={modalOpen}
        onClose={() => { setModalOpen(false); setEditingLote(null); reset(); }}
        title={editingLote ? 'Editar Lote' : 'Nuevo Lote'}
      >
        <form onSubmit={handleSubmit(onSubmit)} className="space-y-4">
          <Input
            label="Código"
            {...register('codigo', { required: !editingLote })}
            error={errors.codigo ? 'Requerido' : undefined}
            disabled={!!editingLote}
          />
          <Input
            label="Nombre"
            {...register('nombre', { required: true })}
            error={errors.nombre ? 'Requerido' : undefined}
          />
          <Input
            label="Hectáreas"
            type="number"
            step="0.01"
            {...register('hectareas', { required: true, min: 0 })}
            error={errors.hectareas ? 'Requerido' : undefined}
          />
          <Input label="Ubicación" {...register('ubicacion')} />
          <Input label="Notas" {...register('notas')} />
          <div className="flex gap-3 pt-4">
            <Button type="button" variant="secondary" onClick={() => setModalOpen(false)} className="flex-1">
              Cancelar
            </Button>
            <Button type="submit" className="flex-1">
              {editingLote ? 'Actualizar' : 'Crear'}
            </Button>
          </div>
        </form>
      </Modal>
    </div>
  );
}
