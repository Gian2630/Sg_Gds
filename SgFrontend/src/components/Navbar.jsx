import { Link } from "react-router-dom";

function Navbar() {
  return (
    <div style={{
        width: "180px",
        height: "100vh",
        background: "#333",
        color: "white",
        display: "flex",
        flexDirection: "column",
        padding: "20px",
        position: "fixed",
        left: 0,
        top: 0
    }}>
      <h2 style={{ marginBottom: "20px" }}>Granel</h2>
      <Link to="/NuevaVenta" style={linkStyle}>Nueva Venta</Link>
      <Link to="/caja" style={linkStyle}>Caja</Link>
      <Link to="/clientes" style={linkStyle}>Clientes</Link>
      <Link to="/Productos" style={linkStyle}>Productos</Link>
      <Link to="/" style={linkStyle}>Volver</Link>
    </div>
  );
}

const linkStyle = {
  color: "white",
  textDecoration: "none",
  margin: "10px 0"
};

export default Navbar;
