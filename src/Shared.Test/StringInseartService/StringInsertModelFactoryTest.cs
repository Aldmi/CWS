using System;
using System.Collections.Generic;
using Domain.InputDataModel.Shared.StringInseartService.Model;
using FluentAssertions;
using Shared.Test.StringInseartService.Datas;
using Shared.Types;
using Xunit;

namespace Shared.Test.StringInseartService
{
    public class StringInsertModelFactoryTest
    {   
        #region TheoryData
        public static IEnumerable<object[]>  CreateInseartDictDatas => new[]
        {
            new object[]
            {
                "\u0002{AddressDevice:X2} {Nbyte:D3}",
                new List<StringInsertModel>
                {
                    new StringInsertModel("{AddressDevice:X2}", "AddressDevice", String.Empty, new StringInsertModelExt("X2", ":X2", null, null)),
                    new StringInsertModel("{Nbyte:D3}", "Nbyte", String.Empty , new StringInsertModelExt("D3", ":D3", null, null)),
                }
            },
            new object[]
            {
                "\u0002{Stations} {ArrivalTime:t}",
                new List<StringInsertModel>
                {
                    new StringInsertModel("{Stations}", "Stations", String.Empty, new StringInsertModelExt("default", string.Empty, null, null)),
                    new StringInsertModel("{ArrivalTime:t}", "ArrivalTime", String.Empty , new StringInsertModelExt("t", ":t", null, null)),
                }
            },
            new object[]
            {
                "0x57{Nbyte:X2 fff {CRCXor:X2}",
                new List<StringInsertModel>
                {
                    new StringInsertModel("{CRCXor:X2}", "CRCXor", String.Empty, new StringInsertModelExt("X2", ":X2", null, null))
                }
            },
            new object[]
            {
                "0x57{Nbyte:X2 fff {CRCXor:X2_Border}",
                new List<StringInsertModel>
                {
                    new StringInsertModel("{CRCXor:X2_Border}", "CRCXor", String.Empty, new StringInsertModelExt("X2_Border", ":X2", new BorderSubString{StartCh = "0x02", EndCh = "0x03", IncludeBorder = true}, null))
                }
            },
            new object[]
            {
                "0x57{Nbyte:X2 {} {CRCXor:X2_Border}",
                new List<StringInsertModel>
                {
                    new StringInsertModel("{CRCXor:X2_Border}", "CRCXor", String.Empty, new StringInsertModelExt("X2_Border", ":X2", new BorderSubString{StartCh = "0x02", EndCh = "0x03", IncludeBorder = true}, null))
                }
            },
            new object[]
            {
                "0x57{ fff {{{ {Nbyte:D3} }gfgf:X2",
                new List<StringInsertModel>
                {
                    new StringInsertModel("{Nbyte:D3}", "Nbyte", String.Empty , new StringInsertModelExt("D3", ":D3", null, null))
                }
            },
            new object[]
            {
                "0x{MATH((rowNumber+64)-(rowNumber*1)):X1}0bb",
                new List<StringInsertModel>
                {
                    new StringInsertModel("{MATH((rowNumber+64)-(rowNumber*1)):X1}", "MATH", "((rowNumber+64)-(rowNumber*1))", new StringInsertModelExt("X1", ":X1", null, null)),
                }
            },

        };
        #endregion

   

        [Theory]
        [MemberData(nameof(CreateInseartDictDatas))]
        public void NormalReplacementTest(string str, List<StringInsertModel> expectedInsertModels)
        {
            //Act
            var dictExt = GetStringInsertModelExtDict.SimpleDictionary;
            var list=  StringInsertModelFactory.CreateListDistinctByReplacement(str, dictExt);

            //Asert
            var moldels= list.ToArray();
            for (int i = 0; i < moldels.Length; i++)
            {
                var model = moldels[i];
                var expectedModel = expectedInsertModels[i];
                model.Replacement.Should().Be(expectedModel.Replacement);
                model.VarName.Should().Be(expectedModel.VarName);
                model.Option.Should().Be(expectedModel.Option);

                model.Ext.Key.Should().Be(expectedModel.Ext.Key);
            }
        }


        [Fact]
        public void CreateInseartDictWithoutReplacementTest()
        {
            //Arrange
            var dictExt = GetStringInsertModelExtDict.SimpleDictionary;
            var str = "0x570xAA0xFF";

            //Act
            var dict= StringInsertModelFactory.CreateListDistinctByReplacement(str, dictExt);

            //Asert
            dict.Count.Should().Be(0);
        }


        [Fact]
        public void CreateInseartDictEmptyStrTest()
        {
            //Arrange
            var dictExt = GetStringInsertModelExtDict.SimpleDictionary;
            var str = String.Empty;

            //Act
            var dict=  StringInsertModelFactory.CreateListDistinctByReplacement(str, dictExt);

            //Asert
            dict.Count.Should().Be(0);
        }


        [Fact]
        public void CreateInseartDictNullStrTest()
        {
            //Arrange
            var dictExt = GetStringInsertModelExtDict.SimpleDictionary;
            string str = null;

            //Act & Asert
            var exception = Assert.Throws<ArgumentNullException>(() => StringInsertModelFactory.CreateListDistinctByReplacement(str, dictExt));
            exception.Message.Should().Contain("Невозможно создать словарь вставок из NULL строки");
        }
    }
}