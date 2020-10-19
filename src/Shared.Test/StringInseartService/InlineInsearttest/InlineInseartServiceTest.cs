
using System.Collections.Generic;
using CSharpFunctionalExtensions;
using Domain.InputDataModel.Shared.StringInseartService.InlineInseart;
using Domain.InputDataModel.Shared.StringInseartService.Model.InlineStringInsert;
using FluentAssertions;
using Xunit;

namespace Shared.Test.StringInseartService.InlineInsearttest
{
    public class InlineInseartServiceTest
    {
        #region TheoryData
        public static IEnumerable<object[]> InseartsDatas => new[]
        {
            new object[]
            {
                "rere {$StationCutVar} ere",
                "rere %010205{StationCutVar:MW_replace} ere"
            },
            new object[]
            {
                "",
                ""
            },
            new object[]
            {
                " ",
                " "
            },
            new object[]
            {
                "rere {$StationCutVar} ere {$Time} ooo",
                "rere %010205{StationCutVar:MW_replace} ere %0368541{Time:t} ooo"
            },
            new object[]
            {
                "rere {$StationCutVar} ere {$Ups} ooo",
                "rere %010205{StationCutVar:MW_replace} ere !!!InlineStrKeyNotFound!!! ooo"
            }
        };
        #endregion
        [Theory]
        [MemberData(nameof(InseartsDatas))]
        public void ExecInlineInseart(string str, string expectedStr)
        {
            //Arrange
            var storage = new InlineStringInsertModelStorage();
            storage.AddNew("{$StationCutVar}", new InlineStringInsertModel("{$StationCutVar}", "%010205{StationCutVar:MW_replace}", "Описание 1"));
            storage.AddNew("{$Time}", new InlineStringInsertModel("{$Time}", "%0368541{Time:t}", "Описание 2"));
            var serv= new InlineInseartService(storage);

            //Act
            var (isSuccess, _, value) = serv.ExecuteInseart(str);

            //Assert
            isSuccess.Should().BeTrue();
            value.Should().Be(expectedStr);
        }



        [Fact]
        public void Null_InStr()
        {
            //Arrange
            var storage = new InlineStringInsertModelStorage();
            storage.AddNew("{$StationCutVar}", new InlineStringInsertModel("{$StationCutVar}", "%010205{StationCutVar:MW_replace}", "Описание 1"));
            storage.AddNew("{$Time}", new InlineStringInsertModel("{$Time}", "%0368541{Time:t}", "Описание 2"));
            var serv = new InlineInseartService(storage);
            string str = null;

            //Act
            var (isSuccess, _, value, error) = serv.ExecuteInseart(str);

            //Assert
            isSuccess.Should().BeFalse();
            error.Should().Be("НЕИЗВЕСТНАЯ ОШИБКА в функции ExecInlineInseart: 'Value cannot be null. (Parameter 'input')'");
        }
    }
}