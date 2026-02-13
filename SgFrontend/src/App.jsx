import { useState, useEffect } from 'react'
import { getProductos } from './api/config.js'
import Navbar from './components/Navbar.jsx'
import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
import ProductosPage from './pages/ProductosPage.jsx';
import NuevaVenta from './pages/NuevaVenta.jsx';


function App() {
  const [productos, setProductos] = useState([])
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState(null)
  
  useEffect(() => {
    getProductos()
      .then(data => {
        console.log('Respuesta de la API:', data) // <-- Agrega esto
        setProductos(data)
        setLoading(false)
      })
      .catch(err => {
        setError('Error al cargar la página')
        setLoading(false)
      })
    }, [])

  if (loading) return <p>Cargando pagina</p>
  if (error) return <p style={{color: 'red'}}>{error}</p>

  return (
    <div className='app-layout'>

      <Navbar />
      <div className="main-content" style={{ padding: "0rem" }}>
        <Routes>
                {/* Página inicial */}       
          <Route path="/NuevaVenta" element={<NuevaVenta />} /> {/* Página de ventas */}
          <Route path="/Productos" element={<ProductosPage />} /> {/* Página de ventas */}
        </Routes>
      </div>
    </div>
  )

}
export default App;
