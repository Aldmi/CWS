using System;
using FluentAssertions;
using Shared.Helpers;
using Xunit;

namespace Shared.Test
{
    public class HelperStringTest
    {
        [Fact]
        public void RemovingExtraSpacesNormalUseTest()
        {
            //Arrage
            var str = " Строка вот такая  то    555  69     ";

            //Act
            var res = str.RemovingExtraSpaces();

            //Asert
            res.Should().Be("Строка вот такая то 555 69");
        }


        [Fact]
        public void RemovingExtraSpaceseEmptyStringTest()
        {
            //Arrage
            var str = String.Empty;

            //Act
            var res = str.RemovingExtraSpaces();

            //Asert
            res.Should().BeEmpty();
        }


        [Fact]
        public void RemovingExtraSpaceseNullTest()
        {
            //Arrage
            string str = null;

            //Act
            var res = str.RemovingExtraSpaces();

            //Asert
            res.Should().BeNull();
        }

    }
}
