using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using CSharpFunctionalExtensions;
using DAL.Abstract.Entities.Options.MiddleWare;
using DeviceForExchange.MiddleWares;
using DeviceForExchnage.Test.Datas;
using FluentAssertions;
using InputDataModel.Autodictor.Model;
using InputDataModel.Base;
using Moq;
using Serilog;
using Xunit;
using Xunit.Abstractions;

namespace DeviceForExchnage.Test
{
    public class MiddleWareInDataTest
    {
        private readonly MiddleWareInDataOption _middleWareInDataOption;
        private ILogger _logger;


        public MiddleWareInDataTest()
        {
             _middleWareInDataOption = InDataSourse.GetMiddleWareInDataOption();

             //Loger Moq-----------------------------------------------
             var mock = new Mock<ILogger>();
             mock.Setup(loger => loger.Debug(""));
             mock.Setup(loger => loger.Error(""));
             mock.Setup(loger => loger.Warning(""));
             _logger = mock.Object;
        }


        [Fact]
        public async Task OnePropertyByStringTypeTest()
        {
            //Arrage
            var inData = InDataSourse.GetData(1);
            var middleWareinData = new MiddleWareInData<AdInputType>(_middleWareInDataOption, _logger);

            Result<InputData<AdInputType>> result= new Result<InputData<AdInputType>>();
            middleWareinData.InvokeReadyRx.Subscribe(data => result = data);

            //Act
            await middleWareinData.InputSet(inData);
            var handledData = result.Value.Data.FirstOrDefault();

            //Asert
            result.IsSuccess.Should().BeTrue();
            handledData.Should().NotBeNull();
            handledData.Note.NameRu.Should().Be("jhjhj");

        }
    }

}