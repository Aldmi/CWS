using System;
using System.Collections.Generic;
using System.Linq;
using Domain.InputDataModel.Autodictor.IndependentInsearts.Factory;
using Domain.InputDataModel.Autodictor.Model;
using Domain.InputDataModel.Base.InseartServices.IndependentInsearts.Factory;
using Domain.InputDataModel.Base.ProvidersConcrete.ByRuleDataProviders.Rules;
using FluentAssertions;
using Shared.Helpers;
using Shared.Services.StringInseartService;
using Shared.Services.StringInseartService.IndependentInseart;
using Xunit;

namespace Shared.Test
{
    public class CreateInseartDictTest
    {
        private const string Pattern =  ViewRule<AdInputType>.Pattern;


        #region TheoryData
        public static IEnumerable<object[]>  CreateInseartDictDatas => new[]
        {
            new object[]
            {
                "\u0002{AddressDevice:X2} {Nbyte:D3}",
                new List<StringInsertModel>
                {
                    new StringInsertModel("{AddressDevice:X2}", "AddressDevice", ":X2"),
                    new StringInsertModel("{Nbyte:D3}", "Nbyte", ":D3")
                }
            },
            new object[]
            {
                "0x57{Nbyte:X2 fff {CRCXor[0x02-0x03]:X2}",
                new List<StringInsertModel>
                {
                    new StringInsertModel("{CRCXor[0x02-0x03]:X2}", "CRCXor[0x02-0x03]", ":X2")
                }
            },
            new object[]
            {
                "0x57{Nbyte:X2 {kkk:ff} {} {CRCXor[0x02-0x03]:X2}",
                new List<StringInsertModel>
                {
                    new StringInsertModel("{kkk:ff}", "kkk", ":ff"),
                    new StringInsertModel("{CRCXor[0x02-0x03]:X2}", "CRCXor[0x02-0x03]", ":X2")
                }
            },
            new object[]
            {
                "0x57{ fff {{{ {CRCXor[0x02-0x03]:X2} }gfgf:X2",
                new List<StringInsertModel>
                {
                    new StringInsertModel("{CRCXor[0x02-0x03]:X2}", "CRCXor[0x02-0x03]", ":X2")
                }
            },
            new object[]
            {
                "0x57{NumberOfTrain}  {CRCXor[0x02-0x03]:X2}",
                new List<StringInsertModel>
                {
                    new StringInsertModel("{NumberOfTrain}", "NumberOfTrain",String.Empty),
                    new StringInsertModel("{CRCXor[0x02-0x03]:X2}", "CRCXor[0x02-0x03]", ":X2")
                }
            }
        };
        #endregion


        [Theory]
        [MemberData(nameof(CreateInseartDictDatas))]
        public void NormalReplacementTest(string str, List<StringInsertModel> expectedInsertModels)
        {
            //Act
            var dict=  InseartDictFactory.CreateDistinctByReplacement(str, Pattern);

            //Asert
            var moldels= dict.Values.ToArray();
            for (int i = 0; i < moldels.Length; i++)
            {
                var model = moldels[i];
                var expectedModel = expectedInsertModels[i];
                model.Replacement.Should().Be(expectedModel.Replacement);
                model.VarName.Should().Be(expectedModel.VarName);
                model.Format.Should().Be(expectedModel.Format);
            }
        }


        [Fact]
        public void CreateInseartDictWithoutReplacementTest()
        {
            //Arrange
            var str = "0x570xAA0xFF";

            //Act
            var dict= InseartDictFactory.CreateDistinctByReplacement(str, Pattern);

            //Asert
            dict.Count.Should().Be(0);
        }


        [Fact]
        public void CreateInseartDictEmptyStrTest()
        {
            //Arrange
            var str = String.Empty;

            //Act
            var dict=  InseartDictFactory.CreateDistinctByReplacement(str, Pattern);

            //Asert
            dict.Count.Should().Be(0);
        }


        [Fact]
        public void CreateInseartDictNullStrTest()
        {
            //Arrange
            string str = null;

            //Act & Asert
            var exception = Assert.Throws<ArgumentNullException>(() => InseartDictFactory.CreateDistinctByReplacement(str, Pattern));
            exception.Message.Should().Contain("Невозможно создать словарь вставок из NULL строки");
        }
    }
}