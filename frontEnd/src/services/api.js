import axios from 'axios';

// Vamos apontar DIRETAMENTE para o backend, sem usar o proxy do package.json
// Isso evita erros de "ECONNREFUSED" falsos do servidor de desenvolvimento
const api = axios.create({
  baseURL: 'http://localhost:5100/api', 
  headers: {
    'Content-Type': 'application/json'
  }
});

export default api;