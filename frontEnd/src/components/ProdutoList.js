import React, { useState, useEffect } from 'react';
import api from '../services/api';

function ProductsList() {
  const [Productss, setProductss] = useState([]);
  const [novoProducts, setNovoProducts] = useState({name:'', price:0, categoryName:'', CategoryId:0});
  const [loading, setLoading] = useState(false);
const [editId, setEditId] = useState(null);
  
  const prepararEdicao = (produto) => {
  setEditId(produto.id); // Grava o ID que está sendo mexido
  setNovoProducts({      // Preenche os inputs
    name: produto.name,
    price: produto.price,
    CategoryId: produto.CategoryId,
    categoryName: produto.categoryName
  });
};
  // Carregar Productss ao iniciar
  useEffect(() => {
    carregarProductss();
  }, []);

  const handleInputChange = (e) => {
    // 1. Pega o nome do campo (ex: 'price') e o valor digitado (ex: '10')
    const { name, value } = e.target;
    
    // 2. Atualiza o estado
    setNovoProducts(prevState => ({
      ...prevState,    // Copia tudo o que já estava lá (para não apagar os outros campos)
      [name]: value    // Atualiza SÓ o campo que mudou
    }));
  };
  // Função para buscar Productss do backend
  const carregarProductss = async () => {
    try {
      setLoading(true);
      const response = await api.get('/Products'); // Chama GET /api/Products
      setProductss(response.data);
    } catch (error) {
      console.error('Erro ao carregar Productss:', error);
      alert('Erro ao carregar Productss');
    } finally {
      setLoading(false);
    }
  };

  // Função para adicionar Products
  const adicionarProducts = async () => {
    try {
      const Products = { name: novoProducts.name, price: novoProducts.price, CategoryId: novoProducts.CategoryId };
      const response = await api.post('/Products', Products); // Chama POST /api/Products
      
      // Adiciona o novo Products à lista
      setProductss([...Productss, response.data]);
      await carregarProductss();

      setNovoProducts({
        name: '',
        price: 0,
        CategoryId: 0,
        categoryName: ''
      });
      

    } catch (error) {
      console.error('Erro ao adicionar Products:', error);
      alert('Erro ao adicionar Products');
    }
  };

  const updateProdutos = async () => {
  if (!editId) return; // Segurança

  try {
    // Garante que enviamos números para o backend
    const payload = { 
      name: novoProducts.name, 
      price: Number(novoProducts.price),
      CategoryId: Number(novoProducts.CategoryId) 
    };

    await api.put(`/Products/${editId}`, payload);
    
    // Recarrega a lista para trazer o 'categoryName' atualizado do banco
    await carregarProductss(); 

    // Limpa o formulário e sai do modo edição
    setEditId(null);
    setNovoProducts({name:'', price:0, categoryName:'', CategoryId:0});

  } catch (error) {
    console.error('Erro ao atualizar:', error);
    alert('Erro ao atualizar produto');
  }
};
  // Função para deletar Products
  const deletarProducts = async (id) => {
    try {
      await api.delete(`/Products/${id}`); // Chama DELETE /api/Products/{id}
      setProductss(Productss.filter(p => p.id !== id));

    } catch (error) {
      console.error('Erro ao deletar Products:', error);
    }
  };

  return (

      <div>
      <h2>Lista de Produtos</h2>
      
      {/* Formulário para adicionar */}
      <div style={{ marginBottom: '20px' }}>
        <label>Produto: </label><input
          type="text"
          value={novoProducts.name}
          onChange={handleInputChange}
          name="name"
          placeholder="nome do produto"
          style={{ padding: '8px', marginRight: '10px' }}
        />
        <label>Preço: </label><input
          type="number"
          value={novoProducts.price}
          onChange={handleInputChange}
          name="price"
          placeholder="preço do produto"
          style={{ padding: '8px', marginRight: '10px' }}
        />
        <label>Id categoria: </label><input
          type="number"
          value={novoProducts.CategoryId}
          onChange={handleInputChange}
          name="CategoryId"
          placeholder="id da categoria"
          style={{ padding: '8px', marginRight: '10px' }}
        />
       {/* Se editId for null (Adicionar), mostra um botão. Se tiver ID, mostra Salvar e Cancelar */}
        {!editId ? (
          <button onClick={adicionarProducts} style={{ padding: '8px 16px' }}>
            Adicionar
          </button>
        ) : (
          <>
            <button onClick={updateProdutos} style={{ padding: '8px 16px', backgroundColor: 'orange' }}>
              Salvar Edição
            </button>
            <button 
              onClick={() => { 
                setEditId(null); 
                setNovoProducts({name:'', price:0, categoryName:'', CategoryId:0}); 
              }} 
              style={{ marginLeft: '10px', padding: '8px 16px' }}
            >
              Cancelar
            </button>
          </>
        )}
      </div>

      {/* Lista de Productss */}
      {loading ? (
        <p>Carregando...</p>
      ) : (
        <table>
          <tr>
            <th style={{border: '1px solid #dddddd', textAlign: 'left', padding: '8px'}}>Produto</th>
            <th style={{border: '1px solid #dddddd', textAlign: 'left', padding: '8px'}}>Valor</th>
            <th style={{border: '1px solid #dddddd', textAlign: 'left', padding: '8px'}}>Categoria</th>
            <th style={{border: '1px solid #dddddd', textAlign: 'left', padding: '8px'}}>Ação</th>
          </tr>
          {Productss.map((Products, index) => (
            <tr key={Products.id || index} style={{ marginBottom: '10px' }}>
              
              <td style={{border: '1px solid #dddddd', textAlign: 'left', padding: '8px'}}>{Products.name } </td>
              <td style={{border: '1px solid #dddddd', textAlign: 'left', padding: '8px'}}>{Products.price }</td>
              <td style={{border: '1px solid #dddddd', textAlign: 'left', padding: '8px'}}>{Products.categoryName }</td>
              
             <td style={{border: '1px solid #dddddd', textAlign: 'left', padding: '8px'}}>
  
  {/* --- BOTÃO NOVO AQUI --- */}
  <button 
    onClick={() => prepararEdicao(Products)}
    style={{ marginRight: '10px', padding: '5px 10px' }}
  >
    Editar
  </button>
  {/* ----------------------- */}

  <button 
    onClick={() => deletarProducts(Products.id || index)}
    style={{ marginLeft: '10px', padding: '5px 10px' }}
  >
    Excluir
  </button>
</td>
              </tr>

          ))}
        </table>
      )}
      </div>

  );
}

export default ProductsList;