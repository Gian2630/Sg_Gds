function VentaTable({ productos, eliminarProducto, actualizarCantidad }) {
  return (
    <table className="venta-table">
      <thead>
        <tr>
          <th>Nombre</th>
          <th>Precio</th>
          <th>Cantidad</th>
          <th>Total</th>
          <th></th>
        </tr>
      </thead>
      <tbody>
        {productos.length === 0 ? (
          <tr>
            <td colSpan="6" style={{ textAlign: "center" }}>
              No hay productos agregados
            </td>
          </tr>
        ) : (
          productos.map((p) => (
            <tr key={p.id}>
              <td>{p.nombre}</td>
              <td>${p.precioventa}</td>

              {/* 👇 Campo editable de cantidad */}
              <td>
                <input
                  type="number"
                  value={p.cantidad}
                  min="0.01"
                  step="0.01"
                  onChange={(e) => actualizarCantidad(p.id, parseFloat(e.target.value))}
                  style={{ width: "80px", textAlign: "center" }}
                />
              </td>

              <td>${(p.cantidad * p.precioventa).toFixed(2)}</td>
              <td>
                <button onClick={() => eliminarProducto(p.id)}>❌</button>
              </td>
            </tr>
          ))
        )}
      </tbody>
    </table>
  );
}

export default VentaTable;