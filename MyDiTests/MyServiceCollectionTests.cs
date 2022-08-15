using System;
using System.Collections.Generic;
using FluentAssertions;
using MyDi.DI;
using Xunit;

namespace MyDiTests
{
    public class MyServiceCollectionTests
    {
        [Fact]
        public void Add_Should_Throw_When_Generic_Type_Not_Interface()
        {
            // Arrange
            var collection = new MyServiceCollection(new List<MyServiceProvider>());

            // Act
            Action act =()=> collection.Add<string>(new object());

            // Assert
            act.Should().Throw<ArgumentException>();
        }
    }
}
