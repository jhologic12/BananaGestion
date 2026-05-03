import axios from 'axios';
import { useAuthStore } from '../store/authStore';

const API_URL = import.meta.env.VITE_API_URL || 'http://localhost:5000/api';

const api = axios.create({
  baseURL: API_URL,
  timeout: 30000,
  headers: {
    'Content-Type': 'application/json',
  },
});

api.interceptors.request.use((config) => {
  const token = useAuthStore.getState().token;
  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
  }
  return config;
});

api.interceptors.response.use(
  (response) => response,
  (error) => {
    if (error.response?.status === 401) {
      useAuthStore.getState().logout();
      window.location.href = '/login';
    }
    return Promise.reject(error);
  }
);

export const authService = {
  login: (email: string, password: string) =>
    api.post('/auth/login', { email, password }),
  register: (data: {
    email: string;
    password: string;
    nombre: string;
    apellido: string;
    telefono?: string;
    rol: string;
  }) => api.post('/auth/register', data),
  changePassword: (currentPassword: string, newPassword: string) =>
    api.post('/auth/change-password', { currentPassword, newPassword }),
};

export const userService = {
  getAll: () => api.get('/users'),
  getById: (id: string) => api.get(`/users/${id}`),
  getObreros: () => api.get('/users/obreros'),
  getSupervisores: () => api.get('/users/supervisores'),
  create: (data: {
    email: string;
    password: string;
    nombre: string;
    apellido: string;
    telefono?: string;
    rol: string;
  }) => api.post('/auth/register', data),
  update: (id: string, data: Partial<{ nombre: string; apellido: string; telefono?: string }>) =>
    api.put(`/users/${id}`, data),
  delete: (id: string) => api.delete(`/users/${id}`),
  toggleStatus: (id: string) => api.put(`/users/${id}/toggle-status`),
};

export const loteService = {
  getAll: () => api.get('/lotes'),
  getActive: () => api.get('/lotes/active'),
  getById: (id: string) => api.get(`/lotes/${id}`),
  create: (data: {
    codigo: string;
    nombre: string;
    hectareas: number;
    ubicacion?: string;
    latitud?: number;
    longitud?: number;
    notas?: string;
  }) => api.post('/lotes', data),
  update: (id: string, data: Partial<{
    nombre: string;
    hectareas: number;
    ubicacion?: string;
    latitud?: number;
    longitud?: number;
    activo?: boolean;
    notas?: string;
  }>) => api.put(`/lotes/${id}`, data),
  delete: (id: string) => api.delete(`/lotes/${id}`),
};

export const tareaService = {
  getConfigs: () => api.get('/tareas/configs'),
  getConfigById: (id: string) => api.get(`/tareas/configs/${id}`),
  createConfig: (data: {
    nombre: string;
    descripcion?: string;
    tipoLabor: string;
    tipo: string;
    frecuenciaDias?: number;
    insumoCantidad?: number;
    insumoId?: string;
    requiereFoto: boolean;
    requiereFirma: boolean;
    requiereGps: boolean;
  }) => api.post('/tareas/configs', data),
  updateConfig: (id: string, data: Partial<{
    nombre: string;
    descripcion?: string;
    frecuenciaDias?: number;
    insumoCantidad?: number;
    insumoId?: string;
    requiereFoto?: boolean;
    requiereFirma?: boolean;
    requiereGps?: boolean;
    activo?: boolean;
  }>) => api.put(`/tareas/configs/${id}`, data),
  deleteConfig: (id: string) => api.delete(`/tareas/configs/${id}`),
  getAssignments: (userId?: string) =>
    api.get('/tareas/assignments', { params: { userId } }),
  getAssignmentsByRange: (start: string, end: string) =>
    api.get('/tareas/assignments/range', { params: { start, end } }),
  getAssignmentById: (id: string) => api.get(`/tareas/assignments/${id}`),
  getOverdue: () => api.get('/tareas/assignments/overdue'),
  createAssignment: (data: {
    taskConfigId: string;
    userId: string;
    loteId: string;
    fechaProgramada: string;
    notas?: string;
  }) => api.post('/tareas/assignments', data),
  updateStatus: (id: string, status: string) =>
    api.put(`/tareas/assignments/${id}/status`, null, { params: { status } }),
  getTaskLog: (assignmentId: string) => api.get(`/tareas/logs/${assignmentId}`),
  createTaskLog: (data: FormData) =>
    api.post('/tareas/logs', data, {
      headers: { 'Content-Type': 'multipart/form-data' },
    }),
};

export const inventarioService = {
  getProducts: () => api.get('/inventario/products'),
  getProductById: (id: string) => api.get(`/inventario/products/${id}`),
  getLowStock: () => api.get('/inventario/products/low-stock'),
  createProduct: (data: {
    codigo: string;
    nombre: string;
    descripcion?: string;
    unidad: string;
    stockMinimo: number;
  }) => api.post('/inventario/products', data),
  updateProduct: (id: string, data: Partial<{
    nombre: string;
    descripcion?: string;
    unidad?: string;
    stockMinimo?: number;
    activo?: boolean;
  }>) => api.put(`/inventario/products/${id}`, data),
  deleteProduct: (id: string) => api.delete(`/inventario/products/${id}`),
  getMovements: (params?: { productId?: string; start?: string; end?: string }) =>
    api.get('/inventario/movements', { params }),
  createMovement: (data: {
    productId: string;
    tipo: string;
    cantidad: number;
    loteId?: string;
    referencia?: string;
    notas?: string;
  }) => api.post('/inventario/movements', data),
};

export const cosechaService = {
  getCalendar: (year: number) => api.get(`/cosecha/calendar/${year}`),
  getCalendarByWeek: (week: number, year: number) =>
    api.get(`/cosecha/calendar/week/${week}/${year}`),
  
  getEncinte: (year: number) => api.get(`/cosecha/encinte/${year}`),
  getEncinteByWeek: (semana: number, ano: number) => 
    api.get(`/cosecha/encinte/${semana}/${ano}`),
  createEncinte: (data: {
    loteId: string;
    semanaEncinte: number;
    anoEncinte: number;
    cantidadRacimosEmbolsados: number;
    colorCinta: string;
    fecha: string;
    notas?: string;
  }) => api.post('/cosecha/encinte', data),
  
  getCosechas: (year: number) => api.get(`/cosecha/cosecha/${year}`),
  getCosechasByEncinte: (semana: number, ano: number) => 
    api.get(`/cosecha/cosecha/encinte/${semana}/${ano}`),
  createCosecha: (data: {
    loteId: string;
    semanaEncinte: number;
    anoEncinte: number;
    semanaCosecha: number;
    anoCosecha: number;
    estado: string;
    cantidadRacimos: number;
    colorCinta: string;
    fecha: string;
    notas?: string;
  }) => api.post('/cosecha/cosecha', data),
  updateCosecha: (id: string, data: {
    cantidadRacimos: number;
    notas?: string;
  }) => api.put(`/cosecha/cosecha/${id}`, data),
  
  getProyeccion: (year?: number) => 
    api.get('/cosecha/proyeccion', { params: year ? { year } : {} }),
  
  getBoxTypes: () => api.get('/cosecha/boxtypes'),
  createBoxType: (data: { codigo: string; descripcion: string; capacidadKilos?: number }) =>
    api.post('/cosecha/boxtypes', data),
};

export default api;
