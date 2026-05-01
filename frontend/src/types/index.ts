export interface User {
  id: string;
  email: string;
  nombre: string;
  apellido: string;
  telefono?: string;
  rol: 'Administrador' | 'Supervisor' | 'Obrero';
  activo: boolean;
  fechaCreacion: string;
  ultimoLogin?: string;
}

export interface AuthResponse {
  id: string;
  email: string;
  nombre: string;
  apellido: string;
  rol: string;
  token: string;
}

export interface Lote {
  id: string;
  codigo: string;
  nombre: string;
  hectareas: number;
  ubicacion?: string;
  latitud?: number;
  longitud?: number;
  activo: boolean;
  fechaCreacion: string;
  notas?: string;
}

export interface TaskConfig {
  id: string;
  nombre: string;
  descripcion?: string;
  tipoLabor: string;
  tipo: string;
  frecuenciaDias?: number;
  insumoCantidad?: number;
  insumoId?: string;
  insumoNombre?: string;
  requiereFoto: boolean;
  requiereFirma: boolean;
  requiereGps: boolean;
  activo: boolean;
}

export interface TaskAssignment {
  id: string;
  taskConfigId: string;
  taskConfigNombre: string;
  userId: string;
  userNombre: string;
  loteId: string;
  loteNombre: string;
  fechaProgramada: string;
  fechaCompletada?: string;
  status: string;
  notas?: string;
}

export interface TaskLog {
  id: string;
  taskAssignmentId: string;
  fechaRegistro: string;
  fotoUrl?: string;
  firmaUrl?: string;
  latitud?: number;
  longitud?: number;
  observaciones?: string;
  cantidadInsumoUtilizado?: number;
}

export interface Product {
  id: string;
  codigo: string;
  nombre: string;
  descripcion?: string;
  unidad: string;
  stockMinimo: number;
  stockActual: number;
  activo: boolean;
  isLowStock: boolean;
}

export interface InventoryMovement {
  id: string;
  productId: string;
  productNombre: string;
  loteId?: string;
  loteNombre?: string;
  userId?: string;
  userNombre?: string;
  tipo: string;
  cantidad: number;
  stockAnterior: number;
  stockNuevo: number;
  referencia?: string;
  notas?: string;
  fecha: string;
}

export interface HarvestCalendar {
  id: string;
  semana: number;
  ano: number;
  colorCinta: string;
  fechaInicio: string;
  fechaFin: string;
  activo: boolean;
}

export interface HarvestRecord {
  id: string;
  harvestCalendarId?: string;
  semana: number;
  loteId: string;
  loteNombre: string;
  userId: string;
  userNombre: string;
  fecha: string;
  cantidadRacimosEmbolsados: number;
  cantidadRacimosCortados: number;
  racimosPerdidos?: number;
  colorCinta: string;
  pesoTotal?: number;
  notas?: string;
  boxRecords: HarvestBoxRecord[];
}

export interface HarvestBoxRecord {
  id: string;
  boxTypeId: string;
  boxTypeCodigo: string;
  cantidad: number;
  pesoTotal?: number;
}

export interface BoxType {
  id: string;
  codigo: string;
  descripcion: string;
  capacidadKilos?: number;
  activo: boolean;
}

export interface HarvestOrder {
  id: string;
  numeroOrden: string;
  semanaEmbarque: number;
  fechaEmbarque: string;
  pdfUrl?: string;
  cliente?: string;
  notas?: string;
  procesada: boolean;
  boxDetails: OrderBoxDetail[];
}

export interface OrderBoxDetail {
  id: string;
  boxTypeId: string;
  boxTypeCodigo: string;
  cantidadPlanificada: number;
  cantidadEmpacada: number;
  notas?: string;
}
