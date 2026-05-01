import { useEffect, useState } from 'react';
import { toast } from 'react-hot-toast';
import { Button } from '../components/ui/Button';
import { Card } from '../components/ui/Card';
import { Modal } from '../components/ui/Modal';
import { Input } from '../components/ui/Input';
import { Select } from '../components/ui/Select';
import { Table, TableRow, TableCell } from '../components/ui/Table';
import { inventarioService, loteService } from '../services/api';
import type { Product, InventoryMovement, Lote } from '../types';
import { Plus, AlertTriangle, ArrowDown } from 'lucide-react';
import { format } from 'date-fns';
import { useForm } from 'react-hook-form';

export function InventarioPage() {
  const [products, setProducts] = useState<Product[]>([]);
  const [movements, setMovements] = useState<InventoryMovement[]>([]);
  const [lotes, setLotes] = useState<Lote[]>([]);
  const [loading, setLoading] = useState(true);
  const [productModal, setProductModal] = useState(false);
  const [movementModal, setMovementModal] = useState(false);
  const [tab, setTab] = useState<'stock' | 'movimientos'>('stock');
  const { register, handleSubmit, reset } = useForm();

  useEffect(() => {
    loadData();
  }, []);

  const loadData = async () => {
    try {
      const [productRes, movementRes, loteRes] = await Promise.all([
        inventarioService.getProducts(),
        inventarioService.getMovements(),
        loteService.getActive(),
      ]);
      setProducts(productRes.data);
      setMovements(movementRes.data);
      setLotes(loteRes.data);
    } catch (error) {
      toast.error('Error al cargar datos');
    } finally {
      setLoading(false);
    }
  };

  const onSubmitProduct = async (data: any) => {
    try {
      await inventarioService.createProduct(data);
      toast.success('Producto creado');
      setProductModal(false);
      reset();
      loadData();
    } catch (error) {
      toast.error('Error al crear producto');
    }
  };

  const onSubmitMovement = async (data: any) => {
    try {
      await inventarioService.createMovement(data);
      toast.success('Movimiento registrado');
      setMovementModal(false);
      reset();
      loadData();
    } catch (error) {
      toast.error('Error al crear movimiento');
    }
  };

  if (loading) return <div className="flex justify-center py-12">Cargando...</div>;

  return (
    <div>
      <div className="flex justify-between items-center mb-6">
        <h1 className="text-2xl font-bold text-gray-900 dark:text-white">Inventario</h1>
        <div className="flex gap-3">
          <Button onClick={() => { reset(); setMovementModal(true); }}>
            <ArrowDown className="w-4 h-4" /> Registrar Movimiento
          </Button>
          <Button onClick={() => { reset(); setProductModal(true); }}>
            <Plus className="w-4 h-4" /> Nuevo Producto
          </Button>
        </div>
      </div>

      <div className="flex gap-4 mb-6">
        <button
          onClick={() => setTab('stock')}
          className={`px-4 py-2 rounded-lg font-medium ${
            tab === 'stock' ? 'bg-primary-600 text-white' : 'bg-gray-200 dark:bg-gray-700'
          }`}
        >
          Stock Actual
        </button>
        <button
          onClick={() => setTab('movimientos')}
          className={`px-4 py-2 rounded-lg font-medium ${
            tab === 'movimientos' ? 'bg-primary-600 text-white' : 'bg-gray-200 dark:bg-gray-700'
          }`}
        >
          Movimientos
        </button>
      </div>

      {tab === 'stock' ? (
        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-4">
          {products.map((p) => (
            <Card key={p.id} className={p.isLowStock ? 'border-2 border-red-400' : ''}>
              {p.isLowStock && (
                <div className="flex items-center gap-2 text-red-600 text-sm mb-2">
                  <AlertTriangle className="w-4 h-4" />
                  <span>Stock Bajo</span>
                </div>
              )}
              <div className="flex justify-between items-start">
                <div>
                  <h3 className="font-semibold text-gray-900 dark:text-white">{p.nombre}</h3>
                  <p className="text-sm text-gray-500">Código: {p.codigo}</p>
                </div>
                <span className="text-xs bg-gray-100 dark:bg-gray-700 px-2 py-1 rounded">
                  {p.unidad}
                </span>
              </div>
              <div className="mt-4 flex justify-between items-end">
                <div>
                  <p className="text-sm text-gray-500">Stock Actual</p>
                  <p className={`text-2xl font-bold ${p.isLowStock ? 'text-red-600' : 'text-primary-600'}`}>
                    {p.stockActual}
                  </p>
                </div>
                <div className="text-right">
                  <p className="text-sm text-gray-500">Mínimo</p>
                  <p className="text-lg font-medium">{p.stockMinimo}</p>
                </div>
              </div>
              <div className="mt-3">
                <div className="w-full h-2 bg-gray-200 dark:bg-gray-700 rounded-full overflow-hidden">
                  <div
                    className={`h-full rounded-full ${p.isLowStock ? 'bg-red-500' : 'bg-primary-500'}`}
                    style={{ width: `${Math.min((p.stockActual / (p.stockMinimo * 2)) * 100, 100)}%` }}
                  />
                </div>
              </div>
            </Card>
          ))}
          {products.length === 0 && (
            <div className="col-span-full text-center py-12 text-gray-500">
              No hay productos registrados
            </div>
          )}
        </div>
      ) : (
        <Card>
          <Table headers={['Fecha', 'Producto', 'Tipo', 'Cantidad', 'Stock Anterior', 'Stock Nuevo', 'Referencia']}>
            {movements.map((m) => (
              <TableRow key={m.id}>
                <TableCell>{format(new Date(m.fecha), 'dd/MM/yyyy HH:mm')}</TableCell>
                <TableCell className="font-medium">{m.productNombre}</TableCell>
                <TableCell>
                  <span className={`px-2 py-1 text-xs rounded-full ${
                    m.tipo === 'Entrada' ? 'bg-green-100 text-green-700' : 'bg-red-100 text-red-700'
                  }`}>
                    {m.tipo}
                  </span>
                </TableCell>
                <TableCell className={m.tipo === 'Entrada' ? 'text-green-600' : 'text-red-600'}>
                  {m.tipo === 'Entrada' ? '+' : '-'}{m.cantidad}
                </TableCell>
                <TableCell>{m.stockAnterior}</TableCell>
                <TableCell>{m.stockNuevo}</TableCell>
                <TableCell>{m.referencia || '-'}</TableCell>
              </TableRow>
            ))}
          </Table>
          {movements.length === 0 && (
            <p className="text-center py-8 text-gray-500">No hay movimientos</p>
          )}
        </Card>
      )}

      <Modal isOpen={productModal} onClose={() => setProductModal(false)} title="Nuevo Producto">
        <form onSubmit={handleSubmit(onSubmitProduct)} className="space-y-4">
          <Input label="Código" {...register('codigo', { required: true })} />
          <Input label="Nombre" {...register('nombre', { required: true })} />
          <Input label="Descripción" {...register('descripcion')} />
          <div className="grid grid-cols-2 gap-4">
            <Input label="Unidad" {...register('unidad', { required: true })} placeholder="kg, L, unidades..." />
            <Input label="Stock Mínimo" type="number" step="0.01" {...register('stockMinimo', { required: true })} />
          </div>
          <div className="flex gap-3 pt-4">
            <Button type="button" variant="secondary" onClick={() => setProductModal(false)} className="flex-1">
              Cancelar
            </Button>
            <Button type="submit" className="flex-1">Crear</Button>
          </div>
        </form>
      </Modal>

      <Modal isOpen={movementModal} onClose={() => setMovementModal(false)} title="Registrar Movimiento">
        <form onSubmit={handleSubmit(onSubmitMovement)} className="space-y-4">
          <Select
            label="Producto"
            {...register('productId', { required: true })}
            options={products.map(p => ({ value: p.id, label: `${p.nombre} (${p.stockActual} ${p.unidad})` }))}
          />
          <Select
            label="Tipo de Movimiento"
            {...register('tipo', { required: true })}
            options={[
              { value: 'Entrada', label: 'Entrada (Compra/Reabastecimiento)' },
              { value: 'Salida', label: 'Salida (Uso/Aplicación)' },
            ]}
          />
          <Input label="Cantidad" type="number" step="0.01" {...register('cantidad', { required: true })} />
          <Select
            label="Lote (opcional)"
            options={[{ value: '', label: 'Sin asignar' }, ...lotes.map(l => ({ value: l.id, label: l.nombre }))]}
            {...register('loteId')}
          />
          <Input label="Referencia" {...register('referencia')} placeholder="N° factura, orden..." />
          <Input label="Notas" {...register('notas')} />
          <div className="flex gap-3 pt-4">
            <Button type="button" variant="secondary" onClick={() => setMovementModal(false)} className="flex-1">
              Cancelar
            </Button>
            <Button type="submit" className="flex-1">Registrar</Button>
          </div>
        </form>
      </Modal>
    </div>
  );
}
