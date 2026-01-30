# estoque api

API desenvolvida em **C# (.NET 8)** com **Entity Framework Core** e **SQLite**.  
O projeto realiza operações básicas de CRUD para gerenciar produtos.

---

##  Como rodar

Abra o terminal na pasta do projeto e execute:

```bash
Navegue para o diretório Estoque.Api e rode esse comando no terminal 'dotnet run'
Navegue para o diretório frontend e rode 'npm start'
O backEnd funcionara na porta 5100
O frontEnd funcionara na porta 3000
para fazer o CRUD via frontend é so colocar no navegador localhost:3000

Endpoints principais
GET /api/products → Lista todos os produtos
GET /api/products/{id} → Retorna um produto específico
POST /api/products → Cria um novo produto
PUT /api/products/{id} → Atualiza um produto existente
DELETE /api/products/{id} → Remove um produto


GET /api/Category → Lista todos as categorias
GET /api/Category/{id} → Retorna uma categoria específico
POST /api/Category → Cria uma nova categoria
PUT /api/Category/{id} → Atualiza um categoria existente
DELETE /api/Category/{id} → Remove um categoria

existe também o Swagger UI que pode ser acessado no navegador http://localhost:5100/swagger


Existe um join entre as tabelas category e products, uma chave extrangeira, de category, na tabela Products, então conseguimos apresentar o nome da categoria sem estar na tabela produto


Exemplo de requisições cURL
criar
curl -X POST "http://localhost:5100/api/products" \
  -H "Content-Type: application/json" \
  -d '{"name":"Café 500g","price":29.9,"category":"Mercearia"}'

Listar
curl "http://localhost:5100/api/products"

update
curl -X PUT "http://localhost:5100/api/products/1" \
  -H "Content-Type: application/json" \
  -d '{"name":"Café Premium","price":34.9,"category":"Mercearia"}'

delete 
curl -X DELETE "http://localhost:5100/api/products/1"


Autor -> Henrique Nassif Braga
RA -> 22251711