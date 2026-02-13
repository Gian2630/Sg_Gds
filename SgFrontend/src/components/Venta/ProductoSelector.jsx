import { useState, useEffect } from "react";
import { getProductos } from "../../api/config.js";

function ProductoSelector({ onSelect, onClose }) {
  const [productos, setProductos] = useState([]);
  const [search, setSearch] = useState("");

  useEffect(() => {
    const fetchProductos = async () => {
      const data = await getProductos();
      setProductos(data);
    };
    fetchProductos();
  }, []);

  const filtrados = productos.filter(p =>
    p.nombre.toLowerCase().includes(search.toLowerCase())
  );

  return (
    <div className="selector-overlay">
      <div className="selector-modal">
        
        <button className="selector-close" onClick={onClose}>×</button>

        <h2>Seleccionar producto</h2>

        <input
          className="selector-search"
          type="text"
          placeholder="Buscar producto..."
          value={search}
          onChange={(e) => setSearch(e.target.value)}
        />

        <table className="selector-table">
          <thead>
            <tr>
              <th>Nombre</th>
              <th>Precio</th>
              <th></th>
            </tr>
          </thead>

          <tbody>
            {filtrados.map((p) => (
              <tr key={p.id}>
                <td>{p.nombre}</td>
                <td>${p.precioventa}</td>
                <td>
                  <button
                    className="selector-add"
                    onClick={() => onSelect(p)}
                  >
                    Agregar
                  </button>
                </td>
              </tr>
            ))}

            {filtrados.length === 0 && (
              <tr>
                <td colSpan="3" style={{ textAlign: "center", padding: "20px" }}>
                  Cargando Productos...
                </td>
              </tr>
            )}
          </tbody>
        </table>

      </div>
    </div>
  );
}

export default ProductoSelector;
