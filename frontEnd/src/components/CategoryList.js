import React, { useState, useEffect } from 'react';
import api from '../services/api';

function CategoryList() {
  const [Categoriess, setCategoriess] = useState([]);
  const [novoCategories, setNovoCategories] = useState({name:''});
  const [loading, setLoading] = useState(false);
  
  // 1. NOVO: Estado para guardar o ID que estamos editando
  const [editId, setEditId] = useState(null);

  useEffect(() => {
    carregarCategoriess();
  }, []);

  const handleInputChange = (e) => {
    const { name, value } = e.target;
    setNovoCategories(prevState => ({
      ...prevState,
      [name]: value
    }));
  };

  const carregarCategoriess = async () => {
    try {
      setLoading(true);
      const response = await api.get('/Category');
      setCategoriess(response.data);
    } catch (error) {
      console.error('Erro ao carregar Categorias:', error);
      alert('Erro ao carregar Categorias');
    } finally {
      setLoading(false);
    }
  };

  const adicionarCategories = async () => {
    try {
      // Nota: Mantive sua estrutura, mas categorias geralmente não precisam de price/CategoryId
      const Categories = { name: novoCategories.name, price: novoCategories.price, CategoryId: novoCategories.CategoryId };
      const response = await api.post('/Category', Categories);
      setCategoriess([...Categoriess, response.data]);
      setNovoCategories({name:''}); // Ajuste leve para limpar como objeto
      await carregarCategoriess();
    } catch (error) {
      console.error('Erro ao adicionar Categoria:', error);
      alert('Erro ao adicionar Categoria');
    }
  };

  // 2. NOVO: Função para jogar o item da tabela para o input
  const prepararEdicao = (c) => {
    setEditId(c.id); // Guarda o ID
    setNovoCategories({ name: c.name }); // Preenche o input
  };

  // 3. NOVO: Função que faz o UPDATE (PUT)
  const salvarAtualizacao = async () => {
    if (!editId) return; // Se não tem ID, não faz nada

    try {
      // Envia a atualização para o backend
      await api.put(`/Category/${editId}`, { name: novoCategories.name });
      
      // Atualiza a lista visualmente (map)
      setCategoriess(Categoriess.map(c => 
        c.id === editId ? { ...c, name: novoCategories.name } : c
      ));

      // Limpa tudo
      setEditId(null);
      setNovoCategories({ name: '' });

    } catch (error) {
      console.error('Erro ao atualizar Categoria:', error);
      alert('Erro ao atualizar');
    }
  };

  const deletarCategories = async (id) => {
    try {
      await api.delete(`/Category/${id}`);
      setCategoriess(Categoriess.filter(p => p.id !== id));
    } catch (error) {
      console.error('Erro ao deletar Categoria:', error);
    }
  };

  return (
      <div>
      <h2>Lista de Categorias</h2>
      
      <div style={{ marginBottom: '20px' }}>
        <label>Categoria: </label>
        <input
          type="text"
          value={novoCategories.name}
          onChange={handleInputChange}
          name="name"
          placeholder="Categoria"
          style={{ padding: '8px', marginRight: '10px' }}
        />
        
        {/* Alterado: Se tiver editId, esconde o botão Adicionar */}
        {!editId && (
          <button onClick={adicionarCategories} style={{ padding: '8px 16px' }}>
            Adicionar
          </button>
        )}

        {/* Alterado: Botão Atualizar agora chama salvarAtualizacao e só aparece na edição */}
        <button 
          onClick={editId ? salvarAtualizacao : carregarCategoriess} 
          style={{ marginLeft: '10px', padding: '8px 16px', backgroundColor: editId ? 'orange' : '' }}
        >
          {editId ? 'Salvar Edição' : 'Recarregar Lista'}
        </button>

        {/* Botão extra para cancelar edição se quiser */}
        {editId && (
          <button onClick={() => { setEditId(null); setNovoCategories({name:''}); }} style={{ marginLeft: '10px' }}>
            Cancelar
          </button>
        )}
      </div>

      {loading ? (
        <p>Carregando...</p>
      ) : (
        <table>
          <tbody>
          <tr>
            <th style={{border: '1px solid #dddddd', textAlign: 'left', padding: '8px'}}>Id</th>
            <th style={{border: '1px solid #dddddd', textAlign: 'left', padding: '8px'}}>Categoria</th>
            <th style={{border: '1px solid #dddddd', textAlign: 'left', padding: '8px'}}>Ação</th>
          </tr>
          {Categoriess.map((c, index) => (
            <tr key={c.id || index} style={{ marginBottom: '10px' }}>
              <td style={{border: '1px solid #dddddd', textAlign: 'left', padding: '8px'}}>{c.id || c} </td>
              <td style={{border: '1px solid #dddddd', textAlign: 'left', padding: '8px'}}>{c.name || c} </td>
              
              <td style={{border: '1px solid #dddddd', textAlign: 'left', padding: '8px'}}>
                {/* NOVO: Botão Editar para chamar a função prepararEdicao */}
                <button 
                  onClick={() => prepararEdicao(c)}
                  style={{ marginRight: '10px', padding: '5px 10px' }}
                >
                  Editar
                </button>

                <button 
                  onClick={() => deletarCategories(c.id || index)}
                  style={{ marginLeft: '10px', padding: '5px 10px' }}
                >
                  Excluir
                </button>
              </td>
            </tr>
          ))}
          </tbody>
        </table>
      )}
      </div>
  );
}

export default CategoryList;