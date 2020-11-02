using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Shared.MiddleWares.Converters.StringConverters;
using Shared.MiddleWares.ConvertersOption.StringConvertersOption;
using Xunit;

namespace DeviceForExchnage.Test.ConverterTests
{
    public class SubStringMemConverterTest
    {
        private SubStringMemConverterOption Option { get; }
        public string InStr => "С остановками: Волочаевская, Климская, Октябрьская, Новосибирская, Красноярская, 25 Километр, Волховские холмы, Ленинско кузнецкие золотые сопки верхней пыжмы, Куйбышевская, Казахстанская, Свердлолвская, Московская, Горьковская";


        public SubStringMemConverterTest()
        {
            Option = new SubStringMemConverterOption()
            {
                Lenght = 60,
                InitPharases = new List<string> { "Без остановок: ", "С остановками: " },
                Separator = ',',
                BanTime = 0
            };
        }



        [Fact]
        public void NormalUse_8Step()
        {
            //Arrage
            var converter = new SubStringMemConverter(Option);

            //Act
            var resStep1 = converter.Convert(InStr, 1);
            var resStep2 = converter.Convert(InStr, 1);
            var resStep3 = converter.Convert(InStr, 1);
            var resStep4 = converter.Convert(InStr, 1);
            var resStep5 = converter.Convert(InStr, 1);
            var resStep6 = converter.Convert(InStr, 1);
            var resStep7 = converter.Convert(InStr, 1);
            var resStep8 = converter.Convert(InStr, 1);

            //Asert
            resStep1.Should().Be("С остановками: Волочаевская, Климская, Октябрьская,"); 
            resStep2.Should().Be("С остановками: Новосибирская, Красноярская, 25 Километр,");
            resStep3.Should().Be("С остановками: Волховские холмы,"); 
            resStep4.Should().Be("Ленинско кузнецкие золотые сопки верхней пыж");
            resStep5.Should().Be("С остановками: Куйбышевская, Казахстанская, Свердлолвская,");
            resStep6.Should().Be("С остановками: Московская, Горьковская"); 
            resStep7.Should().Be("С остановками: Волочаевская, Климская, Октябрьская,");
            resStep8.Should().Be("С остановками: Новосибирская, Красноярская, 25 Километр,");
        }


        [Fact]
        public void SubStringLenghtHightStringLenght_3Step()
        {
            //Arrage
            var option = new SubStringMemConverterOption
            {
                Lenght = 500,
                InitPharases = new List<string> { "Без остановок: ", "С остановками: " },
                Separator = ','
            };
            var converter = new SubStringMemConverter(option);


            //Act
            var resStep1 = converter.Convert(InStr,1);
            var resStep2 = converter.Convert(InStr, 1);
            var resStep3 = converter.Convert(InStr, 1);

            //Asert
            resStep1.Should().Be(InStr);
            resStep2.Should().Be(InStr);
            resStep3.Should().Be(InStr);
        }


        [Fact]
        public void NewStringAfter2Step_5Step_SeparatorSpace()
        {
            //Arrage
            var option = new SubStringMemConverterOption
            {
                Lenght = 30,
                InitPharases = new List<string> { "Без остановок: ", "С остановками: " },
                Separator = ' '
            };
            var converter = new SubStringMemConverter(option);
            var str = "С остановками: серпухово, балаково, Свободное";

            //Act
            var resStep1 = converter.Convert(str,1);
            var resStep2 = converter.Convert(str,1);
            str = "Новая строка 11 22 33 44 55 66 7777777 8888888";
            var resStep3 = converter.Convert(str,1);
            var resStep4 = converter.Convert(str,1);
            var resStep5 = converter.Convert(str,1);

            //Asert
            resStep1.Should().Be("С остановками: серпухово,");
            resStep2.Should().Be("С остановками: балаково,");

            resStep3.Should().Be("Новая строка 11 22 33 44 55");
            resStep4.Should().Be("66 7777777 8888888"); 
            resStep5.Should().Be("Новая строка 11 22 33 44 55"); 
        }


        [Fact]
        public void NewStringAfter2Step_5Step_Separator_comma()
        {
            //Arrage
            var option = new SubStringMemConverterOption
            {
                Lenght = 40,
                InitPharases = new List<string> { "Без остановок: ", "С остановками: " },
                Separator = ','
            };
            var converter = new SubStringMemConverter(option);
            var str = "С остановками: серпухово, балаково, Свободное";

            //Act
            var resStep1 = converter.Convert(str, 1);
            var resStep2 = converter.Convert(str, 1);
            str = "Без остановок: Московское, южное, Северное сияние";
            var resStep3 = converter.Convert(str, 1);
            var resStep4 = converter.Convert(str, 1);
            var resStep5 = converter.Convert(str, 1);

            //Asert
            resStep1.Should().Be("С остановками: серпухово, балаково,");
            resStep2.Should().Be("С остановками: Свободное");

            resStep3.Should().Be("Без остановок: Московское, южное,");
            resStep4.Should().Be("Без остановок: Северное сияние");
            resStep5.Should().Be("Без остановок: Московское, южное,");
        }



        [Fact]
        public void NewStringAfter1Step2Step3Step_7Step()
        {
            //Arrage
            var option = new SubStringMemConverterOption
            {
                Lenght = 40,
                InitPharases = new List<string> { "Без остановок: ", "С остановками: " },
                Separator = ','
            };
            var converter = new SubStringMemConverter(option);
            var str = "С остановками: серпухово, балаково, Свободное";

            //Act
            var resStep1 = converter.Convert(str, 1);
            str = "Без остановок: Московское, южное, Северное сияние";
            var resStep2 = converter.Convert(str, 1);
            str = "Без остановок: Корейская, Ясное, Рыбий Глаз желтый";
            var resStep3 = converter.Convert(str, 1);
            str = "С остановками: серпухово, балаково, Свободное";
            var resStep4 = converter.Convert(str, 1);
            var resStep5 = converter.Convert(str, 1);
            var resStep6 = converter.Convert(str, 1);
            var resStep7 = converter.Convert(str, 1);

            //Asert
            resStep1.Should().Be("С остановками: серпухово, балаково,");
            resStep2.Should().Be("Без остановок: Московское, южное,");

            resStep3.Should().Be("Без остановок: Корейская, Ясное,");
            resStep4.Should().Be("С остановками: серпухово, балаково,");
            resStep5.Should().Be("С остановками: Свободное");
            resStep6.Should().Be("С остановками: серпухово, балаково,");
            resStep7.Should().Be("С остановками: Свободное");
        }


        [Fact]
        public void EmptyStringConvert()
        {
            //Arrage
            var option = new SubStringMemConverterOption
            {
                Lenght = 5
            };
            var converter = new SubStringMemConverter(option);
            var str = String.Empty;

            //Act
            var resStep1 = converter.Convert(str,1);
            var resStep2 = converter.Convert(str,1);
            var resStep3 = converter.Convert(str,1);

            //Asert
            resStep1.Should().Be(expected: string.Empty);
            resStep2.Should().Be(string.Empty);
            resStep3.Should().Be(string.Empty);
        }


        [Fact]
        public void NullStringConvert()
        {
            //Arrage
            var option = new SubStringMemConverterOption
            {
                Lenght = 25
            };
            var converter = new SubStringMemConverter(option);
            string str = null;

            //Act
            var resStep1 = converter.Convert(str,1);
            var resStep2 = converter.Convert(str,1);
            var resStep3 = converter.Convert(str,1);

            //Asert
            resStep1.Should().Be(expected:null);
            resStep2.Should().Be(null);
            resStep3.Should().Be(null);
        }



        [Fact]
        public void BanTime_Always_3Step()
        {
            //Arrage
            var option = new SubStringMemConverterOption()
            {
                Lenght = 60,
                InitPharases = new List<string> { "Без остановок: ", "С остановками: " },
                Separator = ',',
                BanTime = 1000
            };
            var converter = new SubStringMemConverter(option);

            //Act
            var resStep1 = converter.Convert(InStr, 1);
            var resStep2 = converter.Convert(InStr, 1);
            var resStep3 = converter.Convert(InStr, 1);


            //Asert
            resStep1.Should().Be("С остановками: Волочаевская, Климская, Октябрьская,");
            resStep2.Should().Be("С остановками: Волочаевская, Климская, Октябрьская,");
            resStep3.Should().Be("С остановками: Волочаевская, Климская, Октябрьская,");
        }


        [Fact]
        public async Task BanTime_Every_2_Steps()
        {
            //Arrage
            var option = new SubStringMemConverterOption()
            {
                Lenght = 60,
                InitPharases = new List<string> { "Без остановок: ", "С остановками: " },
                Separator = ',',
                BanTime = 200
            };
            var converter = new SubStringMemConverter(option);

            //Act
            var resStep1 = converter.Convert(InStr, 1); //Первая выдача данных, запускает таймер тргерра

            await Task.Delay(100);
            var resStep2 = converter.Convert(InStr, 1); //Вторая уже на работающем таймере прошло 100 мс

            await Task.Delay(100);
            var resStep3 = converter.Convert(InStr, 1);  //Третья тожн на работающем таймере прошло 200 мс

            await Task.Delay(100);
            var resStep4 = converter.Convert(InStr, 1);

            await Task.Delay(100);
            var resStep5 = converter.Convert(InStr, 1);

            await Task.Delay(100);
            var resStep6 = converter.Convert(InStr, 1);

            await Task.Delay(100);
            var resStep7 = converter.Convert(InStr, 1);

            await Task.Delay(100);
            var resStep8 = converter.Convert(InStr, 1);

            await Task.Delay(100);
            var resStep9 = converter.Convert(InStr, 1);

            await Task.Delay(100);
            var resStep10 = converter.Convert(InStr, 1);


            //Asert
            resStep1.Should().Be("С остановками: Волочаевская, Климская, Октябрьская,");
            resStep2.Should().Be("С остановками: Волочаевская, Климская, Октябрьская,");
            resStep3.Should().Be("С остановками: Волочаевская, Климская, Октябрьская,");
        }


        [Fact]
        public async Task BanTime_Every_2Steps_After_5Step_New_data()
        {
            //Arrage
            var option = new SubStringMemConverterOption()
            {
                Lenght = 60,
                InitPharases = new List<string> { "Без остановок: ", "С остановками: " },
                Separator = ',',
                BanTime = 200
            };
            var converter = new SubStringMemConverter(option);

            //Act
            var resStep1 = converter.Convert(InStr, 1);

            await Task.Delay(100);
            var resStep2 = converter.Convert(InStr, 1);

            await Task.Delay(100);
            var resStep3 = converter.Convert(InStr, 1);

            await Task.Delay(100);
            var resStep4 = converter.Convert(InStr, 1);

            await Task.Delay(100);
            var resStep5 = converter.Convert(InStr, 1);

            await Task.Delay(100);
            var resStep6 = converter.Convert(InStr, 1);

            var str = "С остановками: Станция1, Станция2, Станция4, Станция5, Станция6, Станция7, Станция8, Станция9, Станция10, Станция11, Станция12";
            await Task.Delay(100);
            var resStep7 = converter.Convert(str, 1);

            await Task.Delay(100);
            var resStep8 = converter.Convert(str, 1);

            await Task.Delay(100);
            var resStep9 = converter.Convert(str, 1);

            await Task.Delay(100);
            var resStep10 = converter.Convert(str, 1);

            await Task.Delay(100);
            var resStep11 = converter.Convert(str, 1);

            await Task.Delay(100);
            var resStep12 = converter.Convert(str, 1);


            //Asert
            resStep1.Should().Be("С остановками: Волочаевская, Климская, Октябрьская,");
            resStep2.Should().Be("С остановками: Волочаевская, Климская, Октябрьская,");
            resStep3.Should().Be("С остановками: Волочаевская, Климская, Октябрьская,");
        }
    }
}