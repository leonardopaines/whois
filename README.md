## Desafio WhoIs

Projeto publicado em [whois.leonardo.poa.br](https://whois.leonardo.poa.br/)

### Tecnologias Utilizadas
1. API 2 / MVC 5 .NET
2. Front-end Bootstrap
3. Banco de dados MySQL
4. ORM Entity Framework 6 c#

### Estrutura da Solução
1. Base.Api
2. Base.DataModel
3. Base.Resolver
4. Base.Services

```markdown

# Configurações
## Base de Dados
Executar Script (https://github.com/leopaines/whois/blob/master/project/Base.DataModel/Entities/whois.sql) para a geração da estrutura da base/tables MySQL

## Definir ConectionString
Alterar conexão no web.config, localizado em project/Base.Api/Web.config.

 <connectionStrings>
    <add name="MySQLContext" connectionString="server=???;port=???;user id=???;
    password=???;database=whois;persistsecurityinfo=True"      providerName="MySql.Data.MySqlClient" />
  </connectionStrings>


```


[Portfólio](https://www.leonardo.poa.br)
