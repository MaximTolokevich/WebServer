using System;
using DI;
using FluentAssertions;
using Xunit;

namespace DiTests
{
    public class MyServiceCollectionTests
    {
        [Fact]
        public void Add_Should_Throw_When_Generic_Type_Not_Interface()
        {
            var collection = new MyServiceCollection();
            Action act =()=> collection.Add<string>(new object());
            act.Should().Throw<ArgumentException>();
        }
    }
}
