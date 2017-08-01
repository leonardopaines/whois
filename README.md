## Desafio WhoIs

Projeto publicado em [whois.leonardo.poa.br](https://whois.leonardo.poa.br/)

### Tecnologias Utilizadas
1. API 2 / MVC 5 .NET
2. Front-end Bootstrap
3. Banco de dados MySQL
4. ORM Entity Framework 6 c#

### Estrutura da Solução
1. Base.Api - Projeto principal.
2. Base.DataModel - Entidades, Repositórios e Unidade de Trabalho.
3. Base.Resolver - Inversão de Controle (IoC)
4. Base.Services - Serviços / Regras de Negócio

### Configurações

#### Banco de Dados
MySQL versão 5.6

#### Scripts
Executar Script (https://github.com/leopaines/whois/blob/master/project/Base.DataModel/Entities/whois.sql) para a geração da estrutura base/tables MySQL

#### ConnectionString
Definir configuração de conexão no web.config, localizado em project/Base.Api/Web.config
```markdown
 <connectionStrings>
    <add name="MySQLContext" connectionString="server=???;port=???;user id=???;
    password=???;database=whois;persistsecurityinfo=True" providerName="MySql.Data.MySqlClient" />
  </connectionStrings>

```

[Portfólio](https://www.leonardo.poa.br)
