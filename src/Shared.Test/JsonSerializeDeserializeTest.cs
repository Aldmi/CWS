using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Domain.InputDataModel.Base.ProvidersConcrete.ByRuleDataProviders.Rules;
using FluentAssertions;
using Newtonsoft.Json;
using Shared.Helpers;
using Shared.MiddleWares.HandlersOption;
using Shared.Services.StringInseartService;
using Xunit;

namespace Shared.Test
{
   public class JsonSerializeDeserializeTest
    {

        [Fact]
        public void StringMiddleWareOption_Deserialize_Test()
        {
            //Arrage
            var str = "{\"converters\":[{\"PadRightStringConverterOption\":{\"Lenght\": 10}}]}";


            //Act
            try
            {
                var converters = JsonConvert.DeserializeObject<StringMiddleWareOption>(str);
            }
            catch (Exception ex)
            {

            }

            //Asert
            //res.Should().Be("Строка вот такая то 555 69");
        }



        [Fact]
        public void Test()
        {
            //Arrage
            var header = "\u0002{AddressDevice:X2} {Nchar:X2}";
            var body = "{StationsCut({\"converters\":[{\"PadRightStringConverterOption\":{\"Lenght\": 10}}]})}";
            //var footer = "0x10{CRCMod256(10):X2}0x03";
            var footer = "0x10\u0003{CRCMod256([\u0002-\u0003]|Hex):X2}";
            var str = header + body + footer;


            //Act
            var matches = Regex.Matches(str, ViewRule<int>.Pattern)
                .Select(match => new StringInsertModel(match.Groups[0].Value, match.Groups[1].Value, match.Groups[2].Value, match.Groups[3].Value)).ToList();

            //Asert
            //res.Should().Be("Строка вот такая то 555 69");
        }

    }
}
