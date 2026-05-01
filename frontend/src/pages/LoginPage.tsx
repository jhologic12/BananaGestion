import { useState } from 'react';
import { useForm } from 'react-hook-form';
import { useNavigate } from 'react-router-dom';
import { toast } from 'react-hot-toast';
import { Input } from '../components/ui/Input';
import { Button } from '../components/ui/Button';
import { AuthLayout } from '../components/layout/Layout';
import { authService } from '../services/api';
import { useAuthStore } from '../store/authStore';
import type { User } from '../types';

interface LoginForm {
  email: string;
  password: string;
}

export function LoginPage() {
  const [loading, setLoading] = useState(false);
  const navigate = useNavigate();
  const login = useAuthStore((state) => state.login);
  const { register, handleSubmit, formState: { errors } } = useForm<LoginForm>();

  const onSubmit = async (data: LoginForm) => {
    setLoading(true);
    try {
      const response = await authService.login(data.email, data.password);
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
      toast.success('Bienvenido');
      navigate('/dashboard');
    } catch (error: any) {
      toast.error(error.response?.data?.message || 'Error al iniciar sesión');
    } finally {
      setLoading(false);
    }
  };

  return (
    <AuthLayout>
      <div className="card">
        <h2 className="text-2xl font-bold text-center mb-6 text-gray-900 dark:text-white">
          Iniciar Sesión
        </h2>
        <form onSubmit={handleSubmit(onSubmit)} className="space-y-4">
          <Input
            label="Email"
            type="email"
            placeholder="correo@ejemplo.com"
            {...register('email', { required: true })}
            error={errors.email ? 'Email requerido' : undefined}
          />
          <Input
            label="Contraseña"
            type="password"
            placeholder="••••••••"
            {...register('password', { required: true })}
            error={errors.password ? 'Contraseña requerida' : undefined}
          />
          <Button type="submit" className="w-full" disabled={loading}>
            {loading ? 'Iniciando...' : 'Iniciar Sesión'}
          </Button>
        </form>
        <p className="text-center text-sm text-gray-500 dark:text-gray-400 mt-4">
          ¿No tienes cuenta?{' '}
          <a href="/register" className="text-primary-600 hover:underline">
            Regístrate
          </a>
        </p>
      </div>
    </AuthLayout>
  );
}
