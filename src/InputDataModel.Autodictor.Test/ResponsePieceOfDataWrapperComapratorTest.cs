using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Domain.InputDataModel.Autodictor.Entities;
using Domain.InputDataModel.Autodictor.Model;
using Domain.InputDataModel.Base.ProvidersAbstract;
using Domain.InputDataModel.Base.Response;
using Domain.InputDataModel.Base.Response.Comparators;
using Domain.InputDataModel.Base.Response.ResponseValidators;
using FluentAssertions;
using Shared.Enums;
using Shared.Types;
using Xunit;

namespace InputDataModel.Autodictor.Test
{
    public class ResponsePieceOfDataWrapperComapratorTest
    {
        [Fact]
        public void LenghtResponseValidator_ByteArray_InData_Test()
        {
            //Arrange
            var cmp = new ResponsePieceOfDataWrapperComparator<AdInputType>();
            var valueStep1 = new ResponsePieceOfDataWrapper<AdInputType>()
            {
                KeyExchange = "1111",
                DeviceName = "3222",
                ResponsesItems = new List<ResponseDataItem<AdInputType>>
                {
                    new ResponseDataItem<AdInputType>
                    {
                        Status = StatusDataExchange.End,
                        TransportException = null,
                        ProcessedItemsInBatch = new ProcessedItemsInBatch<AdInputType>(0,1, new List<ProcessedItem<AdInputType>>
                        {
                            new ProcessedItem<AdInputType>(
                                new AdInputType(1, "6090", new Note(), "2", new EventTrain(1), new TypeTrain("", "", "", "", 1), new Station(), new Station(), DateTime.Now, null, Lang.Ru), 
                                new ReadOnlyDictionary<string, Change<string>>(new Dictionary<string, Change<string>>
                                            {
                                                {"NumberOfTrain", new Change<string>("6090", "6090") },
                                                {"StationArrival", new Change<string>("Воронеж", "Воронеж") }
                                            }))
                        })
                    }
                }
            };


            var valueStep2 = new ResponsePieceOfDataWrapper<AdInputType>()
            {
                KeyExchange = "1111",
                DeviceName = "3222",
                ResponsesItems = new List<ResponseDataItem<AdInputType>>
                {
                    new ResponseDataItem<AdInputType>
                    {
                        Status = StatusDataExchange.End,
                        TransportException = null,
                        ProcessedItemsInBatch = new ProcessedItemsInBatch<AdInputType>(0,1, new List<ProcessedItem<AdInputType>>
                        {
                            new ProcessedItem<AdInputType>(
                                new AdInputType(1, "6091", new Note(), "3", new EventTrain(1), new TypeTrain("", "", "", "", 1), new Station(), new Station(), DateTime.Now, null, Lang.Ru),
                                new ReadOnlyDictionary<string, Change<string>>(new Dictionary<string, Change<string>>
                                {
                                    {"NumberOfTrain", new Change<string>("6091", "6091") },
                                    {"StationArrival", new Change<string>("Воронеж", "Воронеж") }
                                }))
                        })
                    }
                }
            };

            valueStep1.EvaluateResponsesItems();
            valueStep2.EvaluateResponsesItems();


            //Act
            var resStep1 = cmp.Equals(valueStep1);
            var resStep2 = cmp.Equals(valueStep2);
            var resStep3 = cmp.Equals(valueStep2);

            //Asert
            resStep1.Should().BeTrue();
            resStep2.Should().BeFalse();
            resStep3.Should().BeTrue();
        }

    }
}