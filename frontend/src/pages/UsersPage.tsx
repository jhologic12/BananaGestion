import { useEffect, useState } from 'react';
import { toast } from 'react-hot-toast';
import { Button } from '../components/ui/Button';
import { Card } from '../components/ui/Card';
import { Modal } from '../components/ui/Modal';
import { Input } from '../components/ui/Input';
import { Select } from '../components/ui/Select';
import { Table, TableRow, TableCell } from '../components/ui/Table';
import { userService } from '../services/api';
import type { User } from '../types';
import { Plus, Edit2, Trash2, Power, PowerOff } from 'lucide-react';
import { format } from 'date-fns';
import { useForm } from 'react-hook-form';

interface CreateUserRequest {
  email: string;
  password: string;
  nombre: string;
  apellido: string;
  telefono?: string;
  rol: string;
}

export function UsersPage() {
  const [users, setUsers] = useState<User[]>([]);
  const [loading, setLoading] = useState(true);
  const [modalOpen, setModalOpen] = useState(false);
  const [editingUser, setEditingUser] = useState<User | null>(null);
  const { register, handleSubmit, reset, formState: { errors } } = useForm<CreateUserRequest>();

  useEffect(() => {
    loadUsers();
  }, []);

  const loadUsers = async () => {
    try {
      const res = await userService.getAll();
      setUsers(res.data);
    } catch (error: any) {
      toast.error(error.response?.data?.message || 'Error al cargar usuarios');
    } finally {
      setLoading(false);
    }
  };

  const onSubmit = async (data: CreateUserRequest) => {
    try {
      if (editingUser) {
        toast.error('Edición no implementada aún');
      } else {
        await userService.create(data);
        toast.success('Usuario creado');
      }
      setModalOpen(false);
      setEditingUser(null);
      reset();
      loadUsers();
    } catch (error: any) {
      toast.error(error.response?.data?.message || 'Error al guardar');
    }
  };

  const openEdit = (user: User) => {
    setEditingUser(user);
    setModalOpen(true);
  };

  const deleteUser = async (id: string) => {
    if (!confirm('¿Eliminar este usuario?')) return;
    try {
      await userService.delete(id);
      toast.success('Usuario eliminado');
      loadUsers();
    } catch (error: any) {
      toast.error(error.response?.data?.message || 'Error al eliminar');
    }
  };

  const toggleStatus = async (user: User) => {
    const action = user.activo ? 'desactivar' : 'activar';
    if (!confirm(`¿${action.charAt(0).toUpperCase() + action.slice(1)} este usuario?`)) return;
    try {
      await userService.toggleStatus(user.id);
      toast.success(`Usuario ${action}`);
      loadUsers();
    } catch (error: any) {
      toast.error(error.response?.data?.message || 'Error al cambiar estado');
    }
  };

  const getRoleBadge = (rol: string) => {
    const styles: Record<string, string> = {
      Administrador: 'bg-red-100 text-red-700',
      Supervisor: 'bg-blue-100 text-blue-700',
      Obrero: 'bg-green-100 text-green-700',
    };
    return (
      <span className={`px-2 py-1 text-xs rounded-full ${styles[rol] || 'bg-gray-100 text-gray-700'}`}>
        {rol}
      </span>
    );
  };

  if (loading) {
    return <div className="flex justify-center py-12">Cargando...</div>;
  }

  return (
    <div>
      <div className="flex justify-between items-center mb-6">
        <h1 className="text-2xl font-bold text-gray-900 dark:text-white">Usuarios</h1>
        <Button onClick={() => { setEditingUser(null); reset(); setModalOpen(true); }}>
          <Plus className="w-4 h-4" /> Nuevo Usuario
        </Button>
      </div>

      <Card>
        <Table headers={['Nombre', 'Email', 'Teléfono', 'Rol', 'Estado', 'Creado', 'Acciones']}>
          {users.map((user) => (
            <TableRow key={user.id}>
              <TableCell className="font-medium">
                {user.nombre} {user.apellido}
              </TableCell>
              <TableCell>{user.email}</TableCell>
              <TableCell>{user.telefono || '-'}</TableCell>
              <TableCell>{getRoleBadge(user.rol)}</TableCell>
              <TableCell>
                <span className={`px-2 py-1 text-xs rounded-full ${
                  user.activo ? 'bg-green-100 text-green-700' : 'bg-red-100 text-red-700'
                }`}>
                  {user.activo ? 'Activo' : 'Inactivo'}
                </span>
              </TableCell>
              <TableCell>{format(new Date(user.fechaCreacion), 'dd/MM/yyyy')}</TableCell>
              <TableCell>
                <div className="flex gap-2">
                  <button 
                    onClick={() => openEdit(user)} 
                    className="p-1 hover:bg-gray-100 dark:hover:bg-gray-700 rounded"
                    title="Editar"
                  >
                    <Edit2 className="w-4 h-4 text-gray-500" />
                  </button>
                  <button 
                    onClick={() => toggleStatus(user)} 
                    className="p-1 hover:bg-gray-100 dark:hover:bg-gray-700 rounded"
                    title={user.activo ? 'Desactivar' : 'Activar'}
                  >
                    {user.activo 
                      ? <PowerOff className="w-4 h-4 text-orange-500" />
                      : <Power className="w-4 h-4 text-green-500" />
                    }
                  </button>
                  {user.activo && (
                    <button 
                      onClick={() => deleteUser(user.id)} 
                      className="p-1 hover:bg-gray-100 dark:hover:bg-gray-700 rounded"
                      title="Eliminar"
                    >
                      <Trash2 className="w-4 h-4 text-red-500" />
                    </button>
                  )}
                </div>
              </TableCell>
            </TableRow>
          ))}
        </Table>
        {users.length === 0 && (
          <p className="text-center py-8 text-gray-500">No hay usuarios registrados</p>
        )}
      </Card>

      <Modal
        isOpen={modalOpen}
        onClose={() => { setModalOpen(false); setEditingUser(null); reset(); }}
        title={editingUser ? 'Editar Usuario' : 'Nuevo Usuario'}
      >
        <form onSubmit={handleSubmit(onSubmit)} className="space-y-4">
          <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
            <Input
              label="Nombre"
              placeholder="Juan"
              {...register('nombre', { required: true })}
              error={errors.nombre ? 'Requerido' : undefined}
            />
            <Input
              label="Apellido"
              placeholder="Pérez"
              {...register('apellido', { required: true })}
              error={errors.apellido ? 'Requerido' : undefined}
            />
          </div>
          <Input
            label="Email"
            type="email"
            placeholder="correo@ejemplo.com"
            {...register('email', { required: true })}
            error={errors.email ? 'Email requerido' : undefined}
            disabled={!!editingUser}
          />
          <Input
            label="Teléfono"
            type="tel"
            placeholder="+1234567890"
            {...register('telefono')}
          />
          <Select
            label="Rol"
            {...register('rol', { required: true })}
            options={[
              { value: 'Obrero', label: 'Obrero' },
              { value: 'Supervisor', label: 'Supervisor' },
              { value: 'Administrador', label: 'Administrador' },
            ]}
          />
          {!editingUser && (
            <>
              <Input
                label="Contraseña"
                type="password"
                placeholder="••••••••"
                {...register('password', { required: !editingUser, minLength: 6 })}
                error={errors.password?.type === 'required' ? 'Requerida' : errors.password?.type === 'minLength' ? 'Mínimo 6 caracteres' : undefined}
              />
            </>
          )}
          <div className="flex gap-3 pt-4">
            <Button type="button" variant="secondary" onClick={() => setModalOpen(false)} className="flex-1">
              Cancelar
            </Button>
            <Button type="submit" className="flex-1">
              {editingUser ? 'Actualizar' : 'Crear'}
            </Button>
          </div>
        </form>
      </Modal>
    </div>
  );
}
