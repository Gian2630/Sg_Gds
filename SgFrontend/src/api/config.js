import axios from "axios";

const api = axios.create({
  baseURL: "https://localhost:7067/api", 
});

export const getProductos = async () => {
  const res = await api.get("/Productos");
  return res.data;
};

export default api;