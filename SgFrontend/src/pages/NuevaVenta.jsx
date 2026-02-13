import { useState } from "react";
import ProductoSelector from "../components/Venta/ProductoSelector";
import VentaTable from "../components/Venta/VentaTable";
import VentaDetalles from "../components/Venta/VentaDetalles";
import "../styles/NuevaVentaPage.css";

function NuevaVenta() {
  const [productosVenta, setProductosVenta] = useState([]);
  const [mostrarSelector, setMostrarSelector] = useState(false);
  const [cliente, setCliente] = useState("");
  const [metodoPago, setMetodoPago] = useState("");
  const [tipoComprobante, setTipoComprobante] = useState("");

  const agregarProducto = (producto) => {
    const precio = Number(producto.precioventa);
    // Evita duplicados: si ya está, solo aumenta la cantidad
    const existente = productosVenta.find((p) => p.id === producto.id);
    if (existente) {
      setProductosVenta(
        productosVenta.map((p) =>
          p.id === producto.id ? { ...p, cantidad: p.cantidad + 1 } : p
        )
      );
    } else {
      setProductosVenta([...productosVenta, { ...producto, precioventa: precio, cantidad: 1 }]);
    }
    setMostrarSelector(false);
  };

  const eliminarProducto = (id) => {
    setProductosVenta(productosVenta.filter((p) => p.id !== id));
  };

  const actualizarCantidad = (id, nuevaCantidad) => {
    if (isNaN(nuevaCantidad) || nuevaCantidad <= 0) return;

    setProductosVenta(
      productosVenta.map((p) =>
        p.id === id ? { ...p, cantidad: nuevaCantidad } : p
      )
    );
  };

  const totalVenta = productosVenta.reduce(
    (total, p) => total + p.cantidad * p.precioventa,
    0
  );
  console.log(productosVenta);

  const finalizarVenta = () => {
  if (!cliente || !metodoPago || !tipoComprobante) {
    alert("Debes completar cliente, método de pago y comprobante.");
    return;
  }

  const venta = {
    cliente,
    metodoPago,
    tipoComprobante,
    productos: productosVenta,
    total: totalVenta,
    fecha: new Date().toISOString()
  };

  console.log("VENTA COMPLETA → ", venta);
};

  return (
    
    <div className="nueva-venta-container">
      <h1>Venta</h1>
          {console.log("PRODUCTOS:", productosVenta)}
          {console.log("TOTAL:", totalVenta)}
      
        <VentaTable
           productos={productosVenta}
           eliminarProducto={eliminarProducto}
           actualizarCantidad={actualizarCantidad}          
        />

        <VentaDetalles
            cliente={cliente}
            setCliente={setCliente}
            metodoPago={metodoPago}
            setMetodoPago={setMetodoPago}
            tipoComprobante={tipoComprobante}
            setTipoComprobante={setTipoComprobante}
        />


      <div className="venta-actions">
          <button 
              className="agregar-button"
              onClick={() => setMostrarSelector(true)}>
              Agregar Producto
          </button>     
          <h3 className="total-container">
              Total: ${isNaN(totalVenta) ? "0.00" : totalVenta.toFixed(2)}
          </h3>
          <button 
              className="finalizar-button"
              onClick={finalizarVenta}
      >       
          Finalizar Venta
         </button>
      </div>

      {mostrarSelector && (
        <ProductoSelector
          onSelect={agregarProducto}
          onClose={() => setMostrarSelector(false)}
        />
      )}
    </div>
  );
}

export default NuevaVenta;