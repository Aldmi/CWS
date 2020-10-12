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
        private CreepingLineRunningConvertertOption Option { get; }


        public CreepingLineRunningConvertertTest()
        {
            Option = new CreepingLineRunningConvertertOption
            {
                String4Reset = String.Empty,
                Separator = ' ',
                Lenght = 10
            };
        }



        [Fact]
        public void Init()
        {
            //Arrage
            var convertert = new CreepingLineRunningConvertert(Option);
            var cl= new CreepingLine("С остановками: Волочаевская", null, TimeSpan.FromSeconds(3));

            //Act
            var initRes = (CreepingLine)convertert.Convert(cl, 1);

            //Assert
            initRes.NameRu.Should().Be("С остановками: Волочаевская");
        }


        [Fact]
        public void Three_Step_For_Same_Data()
        {
            //Arrage
            var convertert = new CreepingLineRunningConvertert(Option);
            var cl = new CreepingLine("С остановками: Волочаевская", null, TimeSpan.FromSeconds(3));

            //Act
            var step1 = (CreepingLine)convertert.Convert(cl, 1);
            var step2 = (CreepingLine)convertert.Convert(cl, 1);
            var step3 = (CreepingLine)convertert.Convert(cl, 1);

            //Assert
            step1.NameRu.Should().Be("С остановками: Волочаевская");
        }


        [Fact]
        public async Task Three_Step_For_Same_Data_Emulate_Delay()
        {
            //Arrage
            var convertert = new CreepingLineRunningConvertert(Option);
            var cl = new CreepingLine("С остановками: Волочаевская", null, TimeSpan.FromSeconds(3));

            //Act
            var step1 = ((CreepingLine)convertert.Convert(cl, 1)).NameRu;
            await Task.Delay(4000);
            var step2 = ((CreepingLine)convertert.Convert(cl, 1)).NameRu;
            await Task.Delay(500);
            var step3 = ((CreepingLine)convertert.Convert(cl, 1)).NameRu;


            //Assert
            step1.Should().Be("С остановками: Волочаевская");
        }



        [Fact]
        public async Task Other_Data()
        {
            //Arrage
            var convertert = new CreepingLineRunningConvertert(Option);
            var cl1 = new CreepingLine("С остановками: Волочаевская", null, TimeSpan.FromSeconds(3));
            var cl2= new CreepingLine("Другая строка", null, TimeSpan.FromSeconds(2));

            //Act
            var step1 = ((CreepingLine)convertert.Convert(cl1, 1)).NameRu;

            await Task.Delay(1000);
            var step2 = ((CreepingLine)convertert.Convert(cl2, 1)).NameRu;

            await Task.Delay(3000);
            var step3 = ((CreepingLine)convertert.Convert(cl2, 1)).NameRu;


            //Assert
            step1.Should().Be("С остановками: Волочаевская");
        }
    }
}