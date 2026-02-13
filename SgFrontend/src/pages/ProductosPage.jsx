import { useEffect, useState } from "react";
import { getProductos } from "../api/config.js";
import "../styles/ProductosPage.css";


function ProductosPage() {
  const [productos, setProductos] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    const fetchProductos = async () => {
        try {
            const data = await getProductos();
            console.log("Datos desde la API:", data);
        console.log("Es array?", Array.isArray(data));

        // Si no es array, lo convertimos
        const lista = Array.isArray(data) ? data : Object.values(data);
            setProductos(data);
        } catch (err) {
            setError("Error al cargar productos");
        } finally {
            setLoading(false);
        }
    };

    fetchProductos();
  }, []);
 
    

    console.log("Productos en el estado:", productos);

  return (
        <div className="productos-container">
      <h1>Lista de Productos</h1>
      <h3>Total productos: {productos.length}</h3>

      <table className="productos-table">
        <thead>
            <tr>
                <th>ID</th>
                <th>Nombre</th>
                <th>Precio</th>
            </tr>
        </thead>
        <tbody>
          {productos.map((p) => (
            <tr key={p.id}>
              <td>{p.id}</td>
              <td>{p.nombre}</td>
              <td>${p.precioventa}</td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>     
  );
}

export default ProductosPage;


