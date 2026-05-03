import { useState } from 'react';
import { useForm } from 'react-hook-form';
import { useNavigate } from 'react-router-dom';
import { toast } from 'react-hot-toast';
import { Input } from '../components/ui/Input';
import { Select } from '../components/ui/Select';
import { Button } from '../components/ui/Button';
import { AuthLayout } from '../components/layout/Layout';
import { authService } from '../services/api';
import { useAuthStore } from '../store/authStore';
import type { User } from '../types';

interface RegisterForm {
  email: string;
  password: string;
  confirmPassword: string;
  nombre: string;
  apellido: string;
  telefono?: string;
  rol: string;
}

export function RegisterPage() {
  const [loading, setLoading] = useState(false);
  const navigate = useNavigate();
  const login = useAuthStore((state) => state.login);
  const { register, handleSubmit, formState: { errors } } = useForm<RegisterForm>();

  const onSubmit = async (data: RegisterForm) => {
    if (data.password !== data.confirmPassword) {
      toast.error('Las contraseñas no coinciden');
      return;
    }
    setLoading(true);
    try {
      const response = await authService.register({
        email: data.email,
        password: data.password,
        nombre: data.nombre,
        apellido: data.apellido,
        telefono: data.telefono,
        rol: data.rol,
      });
      const user: User = {
        id: response.data.id,
        email: response.data.email,
        nombre: response.data.nombre,
        apellido: response.data.apellido,
        rol: response.data.rol,
        activo: true,
        fechaCreacion: new Date().toISOString(),
      };
      login(user, response.data.token);
      toast.success('Cuenta creada exitosamente');
      navigate('/dashboard');
    } catch (error: any) {
      // Handle validation errors array
      if (error.response?.data?.errors) {
        const errors = error.response.data.errors;
        const firstError = Array.isArray(errors) ? errors[0] : errors;
        toast.error(firstError?.message || 'Error de validación');
      } else {
        toast.error(error.response?.data?.message || 'Error al registrarse');
      }
    } finally {
      setLoading(false);
    }
  };

  return (
    <AuthLayout>
      <div className="card">
        <h2 className="text-2xl font-bold text-center mb-6 text-gray-900 dark:text-white">
          Crear Cuenta
        </h2>
        <form onSubmit={handleSubmit(onSubmit)} className="space-y-4">
          <div className="grid grid-cols-2 gap-4">
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
          <Input
            label="Contraseña"
            type="password"
            placeholder="••••••••"
            {...register('password', { required: true, minLength: 6 })}
            error={errors.password?.type === 'required' ? 'Requerida' : errors.password?.type === 'minLength' ? 'Mínimo 6 caracteres' : undefined}
          />
          <Input
            label="Confirmar Contraseña"
            type="password"
            placeholder="••••••••"
            {...register('confirmPassword', { required: true })}
            error={errors.confirmPassword ? 'Requerida' : undefined}
          />
          <Button type="submit" className="w-full" disabled={loading}>
            {loading ? 'Creando...' : 'Crear Cuenta'}
          </Button>
        </form>
        <p className="text-center text-sm text-gray-500 dark:text-gray-400 mt-4">
          ¿Ya tienes cuenta?{' '}
          <a href="/login" className="text-primary-600 hover:underline">
            Inicia Sesión
          </a>
        </p>
      </div>
    </AuthLayout>
  );
}
