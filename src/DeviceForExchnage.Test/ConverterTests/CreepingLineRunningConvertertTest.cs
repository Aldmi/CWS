using System;
using System.Threading.Tasks;
using Domain.InputDataModel.Autodictor.Entities;
using Domain.InputDataModel.Autodictor.MIddleWare.ObjectConverters;
using FluentAssertions;
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
                Length = 30,
            };
        }


        [Fact]
        public void Three_Step_For_Zero_Duration()
        {
            //Arrage
            var convertert = new CreepingLineRunningConvertert(Option);
            var duration = TimeSpan.Zero;

            //Act
            var step1 = ((CreepingLine)convertert.Convert(new CreepingLine(InStr, null, duration, DateTime.MaxValue), 1)).NameRu;
            var step2 = ((CreepingLine)convertert.Convert(new CreepingLine(InStr, null, duration, DateTime.MaxValue),1)).NameRu;
            var step3 = ((CreepingLine)convertert.Convert(new CreepingLine(InStr, null, duration, DateTime.MaxValue),1)).NameRu;

            //Assert
            step1.Should().Be("С остановками: Волочаевская,");
            step2.Should().Be("Климская, Октябрьская,");
            step3.Should().Be("Новосибирская, Красноярская,");
        }


        /// <summary>
        /// Имитация работы.
        /// 227/30 = 7 pages отобразить за 1сек, значит вермя на 1 page = 3000/7 = 142мс
        /// Если имитировать InvokeTime = 100мс
        /// 1,2 шаг - работает pagingConverter - page1 
        /// 2,3 шаг - работает pagingConverter - page2
        /// 10 шаг - работает  triggerConverter - пустая строка
        /// </summary>
        [Fact]
        public async Task Input_Data_Not_Change()
        {
            //Arrage
            var convertert = new CreepingLineRunningConvertert(Option);
            var duration = TimeSpan.FromSeconds(1);
            var cl = new CreepingLine(InStr, null, duration, DateTime.MaxValue);
            var invokeTime = 100;
            
            //Act
            var initStep = ((CreepingLine)convertert.Convert(cl, 1)).NameRu;
            await Task.Delay(invokeTime);

            var step1 = ((CreepingLine)convertert.Convert(cl, 1)).NameRu;
            await Task.Delay(invokeTime);

            var step2 = ((CreepingLine)convertert.Convert(cl, 1)).NameRu;
            await Task.Delay(invokeTime);

            var step3 = ((CreepingLine)convertert.Convert(cl, 1)).NameRu;
            await Task.Delay(invokeTime);

            var step4 = ((CreepingLine)convertert.Convert(cl, 1)).NameRu;
            await Task.Delay(invokeTime);

            var step5 = ((CreepingLine)convertert.Convert(cl, 1)).NameRu;
            await Task.Delay(invokeTime);

            var step6 = ((CreepingLine)convertert.Convert(cl, 1)).NameRu;
            await Task.Delay(invokeTime);

            var step7 = ((CreepingLine)convertert.Convert(cl, 1)).NameRu;
            await Task.Delay(invokeTime);

            var step8 = ((CreepingLine)convertert.Convert(cl, 1)).NameRu;
            await Task.Delay(invokeTime);

            var step9 = ((CreepingLine)convertert.Convert(cl, 1)).NameRu;
            await Task.Delay(invokeTime);

            var step10 = ((CreepingLine)convertert.Convert(cl, 1)).NameRu;
            await Task.Delay(invokeTime);

            var step11 = ((CreepingLine)convertert.Convert(cl, 1)).NameRu;
            await Task.Delay(invokeTime);

            var step12 = ((CreepingLine)convertert.Convert(cl, 1)).NameRu;

            //Assert
            initStep.Should().Be("С остановками: Волочаевская,");
            step1.Should().Be("С остановками: Волочаевская,");
            step2.Should().Be("С остановками: Волочаевская,");

            step3.Should().Be("Климская, Октябрьская,");
            step4.Should().Be("Климская, Октябрьская,");

            step5.Should().Be("Новосибирская, Красноярская,");
            step6.Should().Be("Новосибирская, Красноярская,");

            step7.Should().Be("25 Километр, Волховские");
            step8.Should().Be("25 Километр, Волховские");

            step9.Should().Be("холмы, Ленинско кузнецкие");

            step10.Should().Be("");
            step11.Should().Be("");
            step12.Should().Be("");
        }


        /// <summary>
        /// Имитация работы.
        /// Данные идут одни и те же, не дожидаясь сброса приходят новые данные.
        /// Новые данные прокручиваются и происходт сброс.
        /// </summary>
        [Fact]
        public async Task Input_Data_Change()
        {
            //Arrage
            var convertert = new CreepingLineRunningConvertert(Option);
            var duration = TimeSpan.FromSeconds(1);
            var cl = new CreepingLine(InStr, null, duration, DateTime.MaxValue);
            var newStr = "Станция1, Станция2, Станция3, Станция4, Станция5, Станция7, Станция8, Станция9, Станция10, Станция11, Станция12, Станция13, Станция14";
            var clNew = new CreepingLine(newStr, null, duration, DateTime.MaxValue);
            var invokeTime = 100;

            //Act
            var initStep = ((CreepingLine)convertert.Convert(cl, 1)).NameRu;
            await Task.Delay(invokeTime);

            var step1 = ((CreepingLine)convertert.Convert(cl, 1)).NameRu;
            await Task.Delay(invokeTime);

            var step2 = ((CreepingLine)convertert.Convert(cl, 1)).NameRu;
            await Task.Delay(invokeTime);

            var step3 = ((CreepingLine)convertert.Convert(cl, 1)).NameRu;
            await Task.Delay(invokeTime);

            var step4 = ((CreepingLine)convertert.Convert(cl, 1)).NameRu;
            await Task.Delay(invokeTime);

            var step5 = ((CreepingLine)convertert.Convert(cl, 1)).NameRu;
            await Task.Delay(invokeTime);

            var step6 = ((CreepingLine)convertert.Convert(cl, 1)).NameRu;
            await Task.Delay(invokeTime);

            var step7 = ((CreepingLine)convertert.Convert(cl, 1)).NameRu;
            await Task.Delay(invokeTime);

            var step8 = ((CreepingLine)convertert.Convert(clNew, 1)).NameRu;
            await Task.Delay(invokeTime);

            var step9 = ((CreepingLine)convertert.Convert(clNew, 1)).NameRu;
            await Task.Delay(invokeTime);

            var step10 = ((CreepingLine)convertert.Convert(clNew, 1)).NameRu;
            await Task.Delay(invokeTime);

            var step11 = ((CreepingLine)convertert.Convert(clNew, 1)).NameRu;
            await Task.Delay(invokeTime);

            var step12 = ((CreepingLine)convertert.Convert(clNew, 1)).NameRu;
            await Task.Delay(invokeTime);

            var step13= ((CreepingLine)convertert.Convert(clNew, 1)).NameRu;
            await Task.Delay(invokeTime);

            var step14= ((CreepingLine)convertert.Convert(clNew, 1)).NameRu;
            await Task.Delay(invokeTime);

            var step15 = ((CreepingLine)convertert.Convert(clNew, 1)).NameRu;
            await Task.Delay(invokeTime);

            var step16 = ((CreepingLine)convertert.Convert(clNew, 1)).NameRu;
            await Task.Delay(invokeTime);

            var step17 = ((CreepingLine)convertert.Convert(clNew, 1)).NameRu;
            await Task.Delay(invokeTime);

            var step18 = ((CreepingLine)convertert.Convert(clNew, 1)).NameRu;
            await Task.Delay(invokeTime);

            var step19 = ((CreepingLine)convertert.Convert(clNew, 1)).NameRu;
            await Task.Delay(invokeTime);


            //Assert
            step1.Should().Be("С остановками: Волочаевская,");
            step2.Should().Be("С остановками: Волочаевская,");

            step3.Should().Be("Климская, Октябрьская,");
            step4.Should().Be("Климская, Октябрьская,");

            step5.Should().Be("Новосибирская, Красноярская,");
            step6.Should().Be("Новосибирская, Красноярская,");

            step7.Should().Be("25 Километр, Волховские");
            step8.Should().Be("Станция1, Станция2, Станция3,");
            step9.Should().Be("Станция1, Станция2, Станция3,");

            step10.Should().Be("Станция1, Станция2, Станция3,");
            step11.Should().Be("Станция4, Станция5, Станция7,");
            step12.Should().Be("Станция4, Станция5, Станция7,");
            step13.Should().Be("Станция8, Станция9,");
            step14.Should().Be("Станция8, Станция9,");
            step15.Should().Be("Станция10, Станция11,");
            step16.Should().Be("Станция10, Станция11,");
            step17.Should().Be("Станция12, Станция13,");
            step18.Should().Be("");
            step19.Should().Be("");
        }



        //Постоянный сброс тригера новым временем
        [Fact]
        public async Task Reset_Trigger_Always_New_String_Income()
        {
            //Arrage
            var convertert = new CreepingLineRunningConvertert(Option);
            var duration = TimeSpan.FromSeconds(1);
            var cl = new CreepingLine(InStr, null, duration, DateTime.MaxValue);
            var invokeTime = 100;

            //Act
            var step1 = ((CreepingLine)convertert.Convert(new CreepingLine(InStr, null, duration, DateTime.MaxValue), 1)).NameRu;
            await Task.Delay(invokeTime);
            var step2 = ((CreepingLine)convertert.Convert(new CreepingLine("NewStr1", null, duration, DateTime.MaxValue), 1)).NameRu;
            await Task.Delay(invokeTime);
            var step3 = ((CreepingLine)convertert.Convert(new CreepingLine("NewStr2", null, duration, DateTime.MaxValue), 1)).NameRu;
            await Task.Delay(invokeTime);
            var step4 = ((CreepingLine)convertert.Convert(new CreepingLine("NewStr3", null, duration, DateTime.MaxValue), 1)).NameRu;
            
            //Assert
            step1.Should().Be("С остановками: Волочаевская,");
            step2.Should().Be("NewStr1");
            step3.Should().Be("NewStr2");
            step4.Should().Be("NewStr3");
        }
    }
}