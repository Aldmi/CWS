using System;
using System.Threading.Tasks;
using Domain.InputDataModel.Autodictor.Entities;
using Domain.InputDataModel.Autodictor.MIddleWare.ObjectConverters;
using FluentAssertions;
using Shared.MiddleWares.Converters;
using Xunit;

namespace DeviceForExchnage.Test.ConverterTests
{
    public class CreepingLineRunningConvertertTest
    {
        public string InStr => "С остановками: Волочаевская, Климская, Октябрьская, Новосибирская, Красноярская, 25 Километр, Волховские холмы, Ленинско кузнецкие золотые сопки верхней пыжмы, Куйбышевская, Казахстанская, Свердлолвская, Московская, Горьковская";
        private CreepingLineRunningConvertertOption Option { get; }


        public CreepingLineRunningConvertertTest()
        {
            Option = new CreepingLineRunningConvertertOption
            {
                String4Reset = String.Empty,
                Separator = ' ',
                Lenght = 30
            };
        }



        [Fact]
        public void Three_Step_For_Same_Data()
        {
            //Arrage
            var convertert = new CreepingLineRunningConvertert(Option);

            //Act
            var step1 = ((CreepingLine)convertert.Convert(new CreepingLine(InStr, null, TimeSpan.FromSeconds(300)), 1)).NameRu;
            var step2 = ((CreepingLine)convertert.Convert(new CreepingLine(InStr, null, TimeSpan.FromSeconds(300)), 1)).NameRu;
            var step3 = ((CreepingLine)convertert.Convert(new CreepingLine(InStr, null, TimeSpan.FromSeconds(300)), 1)).NameRu;

            //Assert
            step1.Should().Be("С остановками: Волочаевская,");
            step2.Should().Be("Климская, Октябрьская,");
            step3.Should().Be("Новосибирская, Красноярская,");
        }


        //Постоянный сброс тригера новым временем
        [Fact]
        public async Task Reset_Trigger_Always_New_Time_Income()
        {
            //Arrage
            var convertert = new CreepingLineRunningConvertert(Option);

            //Act
            var step1 = ((CreepingLine)convertert.Convert(new CreepingLine(InStr, null, TimeSpan.FromSeconds(2)), 1)).NameRu;
            await Task.Delay(1000);
            var step2 = ((CreepingLine)convertert.Convert(new CreepingLine(InStr, null, TimeSpan.FromSeconds(3)), 1)).NameRu;
            await Task.Delay(1000);
            var step3 = ((CreepingLine)convertert.Convert(new CreepingLine(InStr, null, TimeSpan.FromSeconds(1)), 1)).NameRu;
            await Task.Delay(500);
            var step4 = ((CreepingLine)convertert.Convert(new CreepingLine(InStr, null, TimeSpan.FromSeconds(2)), 1)).NameRu;
            
            //Assert
            step1.Should().Be("С остановками: Волочаевская,");
            step2.Should().Be("Климская, Октябрьская,");
            step3.Should().Be("Новосибирская, Красноярская,");
            step4.Should().Be("25 Километр, Волховские");
        }

        //Постоянный сброс тригера новой строкой
        [Fact]
        public async Task Reset_Trigger_Always_New_String_Income()
        {
            //Arrage
            var convertert = new CreepingLineRunningConvertert(Option);

            //Act
            var step1 = ((CreepingLine)convertert.Convert(new CreepingLine(InStr, null, TimeSpan.FromSeconds(2)), 1)).NameRu;
            await Task.Delay(1500);
            var step2 = ((CreepingLine)convertert.Convert(new CreepingLine("Новая строка 1 ооооочень большая строка !!! !!!!", null, TimeSpan.FromSeconds(2)), 1)).NameRu;
            await Task.Delay(1500);
            var step3 = ((CreepingLine)convertert.Convert(new CreepingLine("Новая строка 2 ооооочень большая строка !!! !!!!", null, TimeSpan.FromSeconds(2)), 1)).NameRu;
            await Task.Delay(500);
            var step4 = ((CreepingLine)convertert.Convert(new CreepingLine("Новая строка 3 ооооочень большая строка !!! !!!!", null, TimeSpan.FromSeconds(2)), 1)).NameRu;
            
            //Assert
            step1.Should().Be("С остановками: Волочаевская,");
            step2.Should().Be("Новая строка 1 ооооочень");
            step3.Should().Be("Новая строка 2 ооооочень");
            step4.Should().Be("Новая строка 3 ооооочень");
        }


        //Сработка тригерра
        [Fact]
        public async Task Fire_Trigger_After_2_Step()
        {
            //Arrage
            var convertert = new CreepingLineRunningConvertert(Option);
            int timerInvoke = (int)TimeSpan.FromSeconds(0.5).TotalMilliseconds;

            //Act
            var step1 = ((CreepingLine)convertert.Convert(new CreepingLine(InStr, null, TimeSpan.FromSeconds(1)), 1)).NameRu;
            await Task.Delay(timerInvoke);
            var step2 = ((CreepingLine)convertert.Convert(new CreepingLine(InStr, null, TimeSpan.FromSeconds(1)), 1)).NameRu;
            await Task.Delay(timerInvoke);
            var step3 = ((CreepingLine)convertert.Convert(new CreepingLine(InStr, null, TimeSpan.FromSeconds(1)), 1)).NameRu;
            await Task.Delay(timerInvoke);
            var step4 = ((CreepingLine)convertert.Convert(new CreepingLine(InStr, null, TimeSpan.FromSeconds(1)), 1)).NameRu;
            await Task.Delay(timerInvoke);
            var step5 = ((CreepingLine)convertert.Convert(new CreepingLine(InStr, null, TimeSpan.FromSeconds(1)), 1)).NameRu;

            //Assert
            step1.Should().Be("С остановками: Волочаевская,");
            step2.Should().Be("Климская, Октябрьская,");
            step3.Should().Be("");
            step4.Should().Be("");
        }



        [Fact]
        public async Task Emittation_Invoke_ByTimerTest()
        {
            //Arrage
            var convertert = new CreepingLineRunningConvertert(Option);
            int timerInvoke = (int) TimeSpan.FromSeconds(0.5).TotalMilliseconds;


            //Act
            //Поступают данные на вход. step1, step2, step3, step4 - данные не меняются и не вышло время сбросса тригера, поэтому работает Paging.
            convertert.SendCommand(MemConverterCommand.Reset);
            var step1 = ((CreepingLine)convertert.Convert(new CreepingLine(InStr, null, TimeSpan.FromSeconds(2)), 1)).NameRu;

            await Task.Delay(timerInvoke);
            var step2 = ((CreepingLine)convertert.Convert(new CreepingLine(InStr, null, TimeSpan.FromSeconds(2)), 1)).NameRu;

            await Task.Delay(timerInvoke);
            var step3 = ((CreepingLine)convertert.Convert(new CreepingLine(InStr, null, TimeSpan.FromSeconds(2)), 1)).NameRu;

            await Task.Delay(timerInvoke);
            var step4 = ((CreepingLine)convertert.Convert(new CreepingLine(InStr, null, TimeSpan.FromSeconds(2)), 1)).NameRu;

            //Время триггера (2сек) вышло, данные на вход идут все те же, на step5 и step6 уже сброшенное значение.
            await Task.Delay(timerInvoke);
            var step5 = ((CreepingLine)convertert.Convert(new CreepingLine(InStr, null, TimeSpan.FromSeconds(2)), 1)).NameRu;

            await Task.Delay(timerInvoke);
            var step6 = ((CreepingLine)convertert.Convert(new CreepingLine(InStr, null, TimeSpan.FromSeconds(2)), 1)).NameRu;

            //Поступили новые данные на вход, перезапустился триггер и Pagging конвертор. step7, step8 - работает Paging
            await Task.Delay(timerInvoke);
            convertert.SendCommand(MemConverterCommand.Reset);
            var step7 = ((CreepingLine)convertert.Convert(new CreepingLine("Новая строка 1 ооооочень большая строка !!! !!!!", null, TimeSpan.FromSeconds(1)), 1)).NameRu;

            await Task.Delay(timerInvoke);
            var step8 = ((CreepingLine)convertert.Convert(new CreepingLine("Новая строка 1 ооооочень большая строка !!! !!!!", null, TimeSpan.FromSeconds(1)), 1)).NameRu;

            //Время триггера (1сек) вышло, данные на вход идут все те же, на step9 и step10 уже сброшенное значение.
            await Task.Delay(timerInvoke);
            var step9 = ((CreepingLine)convertert.Convert(new CreepingLine("Новая строка 1 ооооочень большая строка !!! !!!!", null, TimeSpan.FromSeconds(1)), 1)).NameRu;

            await Task.Delay(timerInvoke);
            var step10 = ((CreepingLine)convertert.Convert(new CreepingLine("Новая строка 1 ооооочень большая строка !!! !!!!", null, TimeSpan.FromSeconds(1)), 1)).NameRu;

            //На вход поступают старые данные, при этом отправили команду запроса, что принудительно перезапустила триггер. Эмитация повторной отправки тех же данных (строка и время), но с другим Id, что заставит выполнится SendCommand.
            await Task.Delay(timerInvoke);
            convertert.SendCommand(MemConverterCommand.Reset);
            var step11 = ((CreepingLine)convertert.Convert(new CreepingLine("Новая строка 1 ооооочень большая строка !!! !!!!", null, TimeSpan.FromSeconds(1)), 1)).NameRu;


            //Assert
            step1.Should().Be("С остановками: Волочаевская,");
            step2.Should().Be("Климская, Октябрьская,");
            step3.Should().Be("Новосибирская, Красноярская,");
            step4.Should().Be("25 Километр, Волховские");
            step5.Should().Be("");
            step6.Should().Be("");
            step7.Should().Be("Новая строка 1 ооооочень");
            step8.Should().Be("большая строка !!! !!!!");
            step9.Should().Be("");
            step10.Should().Be("");
            step11.Should().Be("Новая строка 1 ооооочень");
        }
    }
}