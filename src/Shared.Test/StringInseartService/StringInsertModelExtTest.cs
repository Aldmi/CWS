using Domain.InputDataModel.Shared.StringInseartService.Model;
using FluentAssertions;
using Shared.Mathematic;
using Xunit;

namespace Shared.Test.StringInseartService
{
    public class StringInsertModelExtTest
    {

        [Fact]
        public void With_MathematicFormula_Test()
        {
            //Arrange
            var formula= new MathematicFormula("x+10");
            var insModelExt = new StringInsertModelExt("X2", ":X2", null, null, formula);

            //Act
            var data = 15;
            var result = insModelExt.CalcFinishValue(data);

            //Asert
            result.Should().Be("19");
        }


        [Fact]
        public void WithOut_MathematicFormula_Apply_Test()
        {
            //Arrange
            var insModelExt = new StringInsertModelExt("X2", ":X2", null, null, null);

            //Act
            var data = 15;
            var result = insModelExt.CalcFinishValue(data);

            //Asert
            result.Should().Be("0F");
        }
    }
}