
import "../../styles/VentaDetalles.css";

function VentaDetalles({ cliente, setCliente, metodoPago, setMetodoPago, tipoComprobante, setTipoComprobante }) {
  return (
    <div className="venta-detalles-container">
      
      <div className="venta-detalles-field">
        <label>Cliente</label>
        <select value={cliente} onChange={(e) => setCliente(e.target.value)}>
          <option value="">Seleccionar cliente</option>
          <option value="Consumidor Final">Consumidor Final</option>
          <option value="Cliente Registrado">Cliente Registrado</option> 
        </select>
      </div>

      <div className="venta-detalles-field">
        <label>Método de Pago</label>
        <select value={metodoPago} onChange={(e) => setMetodoPago(e.target.value)}>
          <option value="">Seleccionar</option>
          <option value="Efectivo">Efectivo</option>
          <option value="Débito">Débito</option>
          <option value="Tarjeta prepaga">Tarjeta prepaga</option>
          <option value="Transferencia">Transferencia</option>
        </select>
      </div>

      <div className="venta-detalles-field">
        <label>Tipo de Comprobante</label>
        <select value={tipoComprobante} onChange={(e) => setTipoComprobante(e.target.value)}>
          <option value="">Seleccionar</option>
          <option value="Comprobante interno">Comprobante interno</option>
          <option value="Comprobante devolucion">Comprobante devolución</option>
          <option value="Factura A">Factura A</option>
          <option value="Factura B">Factura B</option>
          <option value="Factura C">Factura C</option>
        </select>
      </div>

    </div>
  );
}

export default VentaDetalles;