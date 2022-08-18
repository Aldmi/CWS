using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Domain.InputDataModel.Base.ProvidersAbstract;
using Domain.InputDataModel.Base.Response;
using FluentAssertions;
using Shared.Enums;
using Shared.Types;
using Xunit;

namespace InputDataModel.Autodictor.Test
{
    public class ResponsePieceOfDataWrapperTest
    {
        #region TheoryData
        public static IEnumerable<object[]> ResponsesItems => new[]
        {
            new object[]
            {
                new ResponseDataItem<int>
                {
                    Status = StatusDataExchange.End,
                    ProcessedItemsInBatch = new ProcessedItemsInBatch<int>(
                        0,
                        2,
                        new List<ProcessedItem<int>>
                        {
                            //1 переменная в батче по индексу 0
                            new ProcessedItem<int>(1, new ReadOnlyDictionary<string, Change<string>>(new Dictionary<string, Change<string>>
                            {
                                {"Addition", new Change<string>("Другое","Другое")},
                                {"Stations", new Change<string>("Москва-Омск","Москва-Омск")}
                            })),
                            //2 переменная в батче по индексу 1
                            new ProcessedItem<int>(2, new ReadOnlyDictionary<string, Change<string>>(new Dictionary<string, Change<string>>
                            {
                                {"Addition", new Change<string>("Другое","Другое")},
                                {"Stations", new Change<string>("Питер-Калиниград","Питер-Калиниград")}
                            }))
                        })
                },
                new List<int>{0,1}
            },
            new object[]
            {
                new ResponseDataItem<int>
                {
                    Status = StatusDataExchange.End,
                    ProcessedItemsInBatch = new ProcessedItemsInBatch<int>(
                        2,
                        1,
                        new List<ProcessedItem<int>>
                        {
                            //1 переменная в батче по индексу 2
                            new ProcessedItem<int>(1, new ReadOnlyDictionary<string, Change<string>>(new Dictionary<string, Change<string>>
                            {
                                {"Addition", new Change<string>("Другое","Другое")},
                                {"Stations", new Change<string>("Москва-Омск","Москва-Омск")}
                            }))
                        })
                },
                new List<int>{2}
            },
            new object[]
            {
                new ResponseDataItem<int>
                {
                    Status = StatusDataExchange.End,
                    ProcessedItemsInBatch = new ProcessedItemsInBatch<int>(
                        5,
                        3,
                        new List<ProcessedItem<int>>
                        {
                            //1 переменная в батче по индексу 5
                            new ProcessedItem<int>(1, new ReadOnlyDictionary<string, Change<string>>(new Dictionary<string, Change<string>>
                            {
                                {"Addition", new Change<string>("Другое","Другое")},
                                {"Stations", new Change<string>("Москва-Омск","Москва-Омск")}
                            })),
                            //2 переменная в батче по индексу 6
                            new ProcessedItem<int>(1, new ReadOnlyDictionary<string, Change<string>>(new Dictionary<string, Change<string>>
                            {
                                {"Addition", new Change<string>("Другое","Другое")},
                                {"Stations", new Change<string>("Питер-Калиниград","Питер-Калиниград")}
                            })),
                            //3 переменная в батче по индексу 7
                            new ProcessedItem<int>(3, new ReadOnlyDictionary<string, Change<string>>(new Dictionary<string, Change<string>>
                            {
                                {"Addition", new Change<string>("Другое","Другое")},
                                {"Stations", new Change<string>("Курск-Магадан","Курск-Магадан")}
                            }))
                        })
                },
                new List<int>{5,6,7}
            }
        };
        #endregion
        [Theory]
        [MemberData(nameof(ResponsesItems))]
        public void ResponseDataItem_SweepBatch_Test(ResponseDataItem<int> resp, List<int> expectedIndexes)
        {
            //Act
            var dict=resp.SweepBatch();

            //Assert
            dict.Count.Should().Be(resp.ProcessedItemsInBatch.BatchSize);
            for (int i = 0; i < expectedIndexes.Count; i++)
            {
                dict.Keys.ToList()[i].Should().Be(expectedIndexes[i]);
            }
        }



        [Fact]
        public void Analog_EvaluateResponsesItems_Test()
        {
            //string - аналог ProcessedItemsInBatch<TIn>, т.е. значение переданные за этот батч
            var inputData = new List<Dictionary<int, string>>
            {
                new Dictionary<int, string>
                {
                    {0, "Str0.0"},
                    {1, "Str1.0"},
                },
                new Dictionary<int, string>
                {
                    {0, "Str0.1"},
                    {1, "Str1.1"},
                },
                new Dictionary<int, string>
                {
                    {2, "Str2.0"},
                },
                new Dictionary<int, string>
                {
                    {8, "Str8.0"},
                },
                new Dictionary<int, string>
                {
                    {2, "Str2.1"},
                },
            };


            var linearList= new List<KeyValuePair<int, string>>();
            foreach (var hh in inputData.Select(dict => dict.Select(i => i)))
            {
                linearList.AddRange(hh);
            }

            var gr= linearList
                .GroupBy(item => item.Key)
                .ToDictionary(k=>k.Key, pairs =>pairs.Select(p=>p.Value).ToList());


            var expectedDict = new Dictionary<int, List<string>>
            {
                {0, new List<string>{"Str0.0", "Str0.1"}},   //Все строки под индексом 0
                {1, new List<string>{"Str1.0", "Str1.1"}},   //Все строки под индексом 1
                {2, new List<string>{"Str2.0", "Str2.1"}},   //Все строки под индексом 2
                {8, new List<string>{"Str8.0"}}              //Все строки под индексом 8
            };
        }



        [Fact]
        public void EvaluateResponsesItems_Test()
        {
            //Arrange
            var responsePieceOfDataWrapper = new ResponsePieceOfDataWrapper<int>
            {
                ResponsesItems = new List<ResponseDataItem<int>>
                {
                    new ResponseDataItem<int>
                    {
                        Status = StatusDataExchange.End,
                        ProcessedItemsInBatch = new ProcessedItemsInBatch<int>(
                            0,
                            2,
                            new List<ProcessedItem<int>>
                            {
                                //1 переменная в батче по индексу 0
                                new ProcessedItem<int>(1, new ReadOnlyDictionary<string, Change<string>>(new Dictionary<string, Change<string>>
                                {
                                    {"Addition", new Change<string>("Другое","Другое")},
                                    {"Stations", new Change<string>("Москва-Омск","Москва-Омск")}
                                })),
                                //2 переменная в батче по индексу 1
                                new ProcessedItem<int>(2, new ReadOnlyDictionary<string, Change<string>>(new Dictionary<string, Change<string>>
                                {
                                    {"Addition", new Change<string>("Другое","Другое")},
                                    {"Stations", new Change<string>("Питер-Калиниград","Питер-Калиниград")}
                                }))
                            })
                    },
                    new ResponseDataItem<int>
                    {
                        Status = StatusDataExchange.EndWithTimeout,
                        ProcessedItemsInBatch = new ProcessedItemsInBatch<int>(
                            0,
                            2,
                            new List<ProcessedItem<int>>
                            {
                                //1 переменная в батче по индексу 0
                                new ProcessedItem<int>(1, new ReadOnlyDictionary<string, Change<string>>(new Dictionary<string, Change<string>>
                                {
                                    {"Addition", new Change<string>("Другое","Другое")},
                                    {"Stations", new Change<string>("Женева-Милан","Женева-Милан")}
                                })),
                                //2 переменная в батче по индексу 1
                                new ProcessedItem<int>(2, new ReadOnlyDictionary<string, Change<string>>(new Dictionary<string, Change<string>>
                                {
                                    {"Addition", new Change<string>("Другое","Другое")},
                                    {"Stations", new Change<string>("Женева-Милан","Женева-Милан")}
                                }))
                            })
                    },
                    new ResponseDataItem<int>
                    {
                        Status = StatusDataExchange.End,
                        ProcessedItemsInBatch = new ProcessedItemsInBatch<int>(
                            2,
                            1,
                            new List<ProcessedItem<int>>
                            {
                                //1 переменная в батче по индексу 2
                                new ProcessedItem<int>(1, new ReadOnlyDictionary<string, Change<string>>(new Dictionary<string, Change<string>>
                                {
                                    {"Addition", new Change<string>("Другое","Другое")},
                                    {"Stations", new Change<string>("Москва-Омск","Москва-Омск")}
                                }))
                            })
                    },
                    new ResponseDataItem<int>
                    {
                        Status = StatusDataExchange.End,
                        ProcessedItemsInBatch = new ProcessedItemsInBatch<int>(
                            5,
                            3,
                            new List<ProcessedItem<int>>
                            {
                                //1 переменная в батче по индексу 5
                                new ProcessedItem<int>(1, new ReadOnlyDictionary<string, Change<string>>(new Dictionary<string, Change<string>>
                                {
                                    {"Addition", new Change<string>("Другое","Другое")},
                                    {"Stations", new Change<string>("Москва-Омск","Москва-Омск")}
                                })),
                                //2 переменная в батче по индексу 6
                                new ProcessedItem<int>(1, new ReadOnlyDictionary<string, Change<string>>(new Dictionary<string, Change<string>>
                                {
                                    {"Addition", new Change<string>("Другое","Другое")},
                                    {"Stations", new Change<string>("Питер-Калиниград","Питер-Калиниград")}
                                })),
                                //3 переменная в батче по индексу 7
                                new ProcessedItem<int>(3, new ReadOnlyDictionary<string, Change<string>>(new Dictionary<string, Change<string>>
                                {
                                    {"Addition", new Change<string>("Другое","Другое")},
                                    {"Stations", new Change<string>("Курск-Магадан","Курск-Магадан")}
                                }))
                            })
                    },
                }
            };

            //Act
            var res = responsePieceOfDataWrapper.LinearizationBatchs();


            //Assert
        }




    }
}