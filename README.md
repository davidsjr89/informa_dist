# informa_dist


--> Criar containers e fazer os testes.

docker run -d -p 9000:9000 --name sonarqube -v sonarqube_conf:/opt/sonarqube/conf -v sonarqube_extensions:/opt/sonarqube/extensions -v sonarqube_logs:/opt/sonarqube/logs -v sonarqube_data:/opt/sonarqube/data sonarqube

--> Ele irá te gerar uma chave quando você acessar o sistema, pelo o link localhost:9000

chave
c9ee840976876f8eea2465d9638d077cec726ad4

Para instalar o sonnarquberscanner, só rodar no msdos

dotnet tool install --global dotnet-sonarscanner

--> Dentro da pasta do projeto rodar o seguinte comando via msdos.

dotnet sonarscanner begin /k:"informa_dist" /d:sonar.host.url="http://localhost:9000"  /d:sonar.login="c9ee840976876f8eea2465d9638d077cec726ad4"

--> Segundo comando

dotnet build c:\Projetos\informa_dist\src\backend\informa

--> Terceiro comando

dotnet sonarscanner end /d:sonar.login="c9ee840976876f8eea2465d9638d077cec726ad4"


ef core
--> criar a base via linha de comando.
add-migrations colocar_o_nome

--> atualizar o migration, após criar.
update-database

--> caso queira gerar o script para rodar diretamente no banco
script-migration


