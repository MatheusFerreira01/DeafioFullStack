# DesafioFullStack
DesafioFullStack Pleno, construção de um sistema completo com comunicação IoT

Matheus Henrique Ferreira,
	Primeiramente queria agradecer a oportunidade de mostrar meus poucos conhecimentos e de os colocar em prática.

Primeiro acesso:
	Usuário = admin 
	Senha = admin
	
Escopo do Projeto 

	A ideia foi seguir ao máximo a risca do que estava descrito no escopo do documento, porém fazer de uma forma simples,
	para que o projeto seja o mais plug-play possível. Por conta disso, não é necessária nenhuma configuração inicial,
	apenas ficar de olho caso a porta da API mude de 7078, se mudar basta alterar no app settings da página Web.
	
	O projeto foi dividido em 4 programas separados, onde 3 deles (DeviceIoTGenerator, Api e Web) são executados simultaneamente em
	que irei detalhar no próximo tópico;		
	
Conjunto do Projeto:
 
	DeviceIoTGenerator:
		Este projeto é responsável por emular os dispositivos IoT sensores de chuva; a princípio tentei criar
		utilizando MQTT, porém como a ideia seria a troca de informações ser feita em Telnet, estudei também
		a possibilidade de utilizar Azure IoT, Amazon IoT ou Google IoT, plataformas que eu poderia subir e emular
		meus dispositivos online, mas como eles geram um pequeno custo optei por usar meu próprio emulador, o mesmo para as emulações;
		
		Basicamente, seu funcionamento é abrir as conexões com as URLs e portas dos dispositivos cadastrados e ficar escutando qualquer mensagem que lhes é enviado;
		
	DataBase:
		Este é um projeto apenas biblioteca, onde o intuito é servir como base de dados, com o pensamento de um sistema leve e fácil de se construir. Os dados são armazenados
		em arquivos Csv imitando a arquitetura de um Banco de Dados, porém é totalmente viável utilizar um Banco de Dados como Sql Server, o atrelando a um Entity Framework 
		para maior robustez e eficiência;
		
		Além disso, ele tem como função pegar todos os caminhos dos arquivos e servir de rota para todos os projetos ao precisarem acessar esses arquivos;
	
	Modelos:
		Este é também um projeto biblioteca, onde tem como intuito armazenam as classes Modelo do projeto, facilitando assim na hora de criar as mesmas estruturas de dados nos
		demais projetos deste conjunto;
	
	Api:
		Seguindo ao máximo a Api proposta no escopo definido, ela tem como intuito fazer nossa autenticação de usuário e garantir que o usuário tenha as permissões para navegar
		pelo sistema, acessando todas as rotas necessárias, em se tratando das rotas, como no próprio documento temos 1 rota para cada ação básica de um CRUD, sendo elas: Adicionar,
		Remover, Editar, além de adicionais como: uma para pegar todos os dispositivos e uma para pegar um específico;
	
	Web:
		Responsável por ser toda a interface do usuário com nossos dados e os dispositivos, a web possui diversas páginas onde irei especificá-las abaixo,
		o design é simples e direto para deixar bem fácil de se navegar e sem muitas poluições visuais fazendo o usuário ir sempre direto ao ponto;
			
			Login: Como próprio nome diz, é a tela onde o usuário efetua o login na página e é aqui que nossa API entra em ação utilizando Basic Authorization
				para validar se os dados de usuários constam no banco ou não para que ele possa navegar;
			
				Além disso, ele vai retornar para o sistema qual o perfil de acesso do usuário, para que ele possa navegar apenas nas funções pré-determinadas;
			
			Home: Nossa tela principal consta 3 botões no centro da tela que irão redirecionar para outras páginas, e um outro botão no canto superior direito
				que realizará o logoff do usuário;
				
				Monitorar Dispositivos Cadastrados: Te leva para uma tabela com todos os nossos dispositivos cadastrados, onde o usuário pode escolher os de seu interesse para monitorar;
					Ao ir para próxima tela temos a página de monitoramento;
					
					Monitoramento: Esta é a tela mais complexa do sistema, em que aqui é possível realizar várias ações e realmente monitorar os volumes de chuva devolvidos pelos nossos 
								dispositivos;
								
								Aqui, o usuário pode tanto enviar comandos manualmente para os dispositivos através da lista de seleções e o botão Enviar,
								ou utilizar da função RealTime que irá buscar os valores em tempo real e isso será exibido em tempo real no DashBoard logo abaixo;
				
				Botão Gerenciamento de Usuários: Permite todas as funções básicas de criação, deleção e alteração dos usuários do sistema;
					
				
				Botão Gerenciamento Dispositivos: Permite todas as funções básicas de criação, deleção e alteração dos dispositivos do sistema;
				
Melhorias:

	Por fim, algumas melhorias que eu consigo propor para futuras alterações:
		Primeiramente, com certeza utilizar um banco de dados Sql atrelado a um serviço de nuvem como Azure ou Amazon, e utilizar o Entity Framework para ajudar nos
		versionamentos do banco, melhorar nossa estrutura com os projetos e simplificar ainda mais nossas ações tanto na API como na Web, atrelado a isso, aproveitar
		a plataforma nuvem para utilizar seu serviço IoT, podendo assim deixar todos os dados importantes seguros e acessíveis a qualquer momento;
		
	Para a API, acredito que a melhoria principal seria utilizar uma autenticação mais robusta, como JWT ou OAuth, para deixar ainda mais segura a transição dos dados pelo site e API,
	além disso, concentrar todas as comunicações feitas com o banco pela API, seja para pegar dispositivos, usuários ou quaisquer outros dados sensíveis contidos no banco, assim todas
	nossas requisições estariam no mesmo lugar.	
	
Conclusões:

	No fim, foi um desafio muito grande e empolgante de se fazer e com certeza muito foi aprendido nesse trajeto;
	
	Obrigado.
