﻿using Xunit;

namespace Demo.Tests
{
    public class CalculadoraTests
    { 
    
        //Fact -> Indica que se tratade um teste unitário que não receberá parâmetros
        //Os métodos devem ser públicos e retornarem void
        [Fact]
        public void Calculadora_Somar_RetornarValorSoma()
        {
            // Arrange
            var calculadora = new Calculadora();

            // Act
            var resultado = calculadora.Somar(2, 2);

            // Assert
            Assert.Equal(4, resultado);
        }

        //Theory -> É quando desejamos passar valores como parâmetro do método para testar
        //Deve ser usado em conjunto com o InlineData, que é onde esses parâmetros são passados
        //Os parâmetros informados no inlineData devem estar na mesma ordem daqueles utilizados no método, para não haver
        //inconsistências no teste
        [Theory]
        [InlineData(1,1,2)]
        [InlineData(2, 2, 4)]
        [InlineData(4, 2, 6)]
        [InlineData(7, 3, 10)]
        [InlineData(6, 6, 12)]
        [InlineData(9, 9, 18)]
        public void Calculadora_Somar_RetornarValoresSomaCorretos(double v1, double v2, double total)
        {
            // Arrange
            var calculadora = new Calculadora();

            // Act
            var resultado = calculadora.Somar(v1, v2);

            // Assert
            Assert.Equal(total, resultado);
        }
    }
}