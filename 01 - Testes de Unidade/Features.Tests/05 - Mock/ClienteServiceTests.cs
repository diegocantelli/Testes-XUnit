﻿using System.Linq;
using System.Threading;
using Features.Clientes;
using MediatR;
using Moq;
using Xunit;

namespace Features.Tests
{
    [Collection(nameof(ClienteBogusCollection))]
    public class ClienteServiceTests
    {
        readonly ClienteTestsBogusFixture _clienteTestsBogus;

        public ClienteServiceTests(ClienteTestsBogusFixture clienteTestsFixture)
        {
            _clienteTestsBogus = clienteTestsFixture;
        }

        [Fact(DisplayName = "Adicionar Cliente com Sucesso")]
        [Trait("Categoria", "Cliente Service Mock Tests")]
        public void ClienteService_Adicionar_DeveExecutarComSucesso()
        {
            // Arrange
            var cliente = _clienteTestsBogus.GerarClienteValido();

            //Utilizando a biblioteca Mock para gerar instâncias de classes, resolvendo automaticamente
            // as suas dependências
            var clienteRepo = new Mock<IClienteRepository>();
            var mediatr = new Mock<IMediator>();

            //A instâcia estará disponínel através da propriedade Object
            var clienteService = new ClienteService(clienteRepo.Object, mediatr.Object);

            // Act
            clienteService.Adicionar(cliente);

            // Assert
            Assert.True(cliente.EhValido());

            //Verificando se um determinado método dentro de Cliente repositório foi chamado uma vez
            clienteRepo.Verify(r => r.Adicionar(cliente),Times.Once);

            //It.IsAny -> Valida se o método publish recebeu algum objeto que implementa a interface INotification
            // e que o método Publish tenha sido chamado 1 vez
            mediatr.Verify(m=>m.Publish(It.IsAny<INotification>(),CancellationToken.None),Times.Once);
        }

        [Fact(DisplayName = "Adicionar Cliente com Falha")]
        [Trait("Categoria", "Cliente Service Mock Tests")]
        public void ClienteService_Adicionar_DeveFalharDevidoClienteInvalido()
        {
            // Arrange
            var cliente = _clienteTestsBogus.GerarClienteInvalido();
            var clienteRepo = new Mock<IClienteRepository>();
            var mediatr = new Mock<IMediator>();

            var clienteService = new ClienteService(clienteRepo.Object, mediatr.Object);

            // Act
            clienteService.Adicionar(cliente);

            // Assert
            Assert.False(cliente.EhValido());
            clienteRepo.Verify(r => r.Adicionar(cliente), Times.Never);
            mediatr.Verify(m => m.Publish(It.IsAny<INotification>(), CancellationToken.None), Times.Never);
        }

        [Fact(DisplayName = "Obter Clientes Ativos")]
        [Trait("Categoria", "Cliente Service Mock Tests")]
        public void ClienteService_ObterTodosAtivos_DeveRetornarApenasClientesAtivos()
        {
            // Arrange
            var clienteRepo = new Mock<IClienteRepository>();
            var mediatr = new Mock<IMediator>();

            //Setup -> Serve para indicar o que deve acontecer quando o método acionado dentro do setup for chamado
            // Returns -> indica o que deverá ser retornado quando o método dentro do Setup for acionado
            clienteRepo.Setup(c => c.ObterTodos())
                .Returns(_clienteTestsBogus.ObterClientesVariados());

            var clienteService = new ClienteService(clienteRepo.Object, mediatr.Object);

            // Act
            var clientes = clienteService.ObterTodosAtivos();

            // Assert 
            clienteRepo.Verify(r => r.ObterTodos(), Times.Once);
            Assert.True(clientes.Any());
            Assert.False(clientes.Count(c=>!c.Ativo) > 0);
        }
    }
}