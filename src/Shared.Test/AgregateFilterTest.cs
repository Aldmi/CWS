using System.Collections.Generic;
using System.Linq;
using Domain.InputDataModel.Autodictor.Model;
using FluentAssertions;
using Shared.Extensions;
using Shared.Test.StringInseartService.Datas;
using Shared.Types;
using Xunit;

namespace Shared.Test
{
    public class AgregateFilterTest
    {
        private readonly string _defaultItemJson;

        public AgregateFilterTest()
        {
             _defaultItemJson = "{}";
        }


        [Fact]
        public void Filter_NumberOfTrain_2Even_2Odd_Filter()
        {
            var inputs = GetListAdInputType.GenerateListInputType(4, 1);
            var filterEvenOdd = new AgregateFilter
            {
                Filters = new List<Predicate>
                {
                    //2 ЧЕТН.
                    new Predicate
                    {
                        Where = "(NumberOfTrain != null) && (NumberOfTrain != \"\") && (int.Parse(NumberOfTrain) % 2) == 0",
                        OrderBy = string.Empty,
                        Take = 2,
                        AlwaysTake = false
                    },
                    //2 НЕ ЧЕТН.
                    new Predicate
                    {
                        Where = "(NumberOfTrain != null) && (NumberOfTrain != \"\") && (int.Parse(NumberOfTrain) % 2) == 1",
                        OrderBy = string.Empty,
                        Take = 2,
                        AlwaysTake = false
                    }
                }
            };

            //Act
            var res= inputs.Filter(filterEvenOdd, _defaultItemJson).ToList();

           //Assert
           res.Count.Should().Be(4);
           res[0].NumberOfTrain.Should().Be("0");
           res[1].NumberOfTrain.Should().Be("2");
           res[2].NumberOfTrain.Should().Be("1");
           res[3].NumberOfTrain.Should().Be("3");
        }


        [Fact]
        public void Filter_NumberOfTrain_2Even_0Odd_Filter()
        {
            var inputs = GetListAdInputType.GenerateListInputType(4, 2);
            var filterEvenOdd = new AgregateFilter
            {
                Filters = new List<Predicate>
                {
                    //2 ЧЕТН.
                    new Predicate
                    {
                        Where = "(NumberOfTrain != null) && (NumberOfTrain != \"\") && (int.Parse(NumberOfTrain) % 2) == 0",
                        OrderBy = string.Empty,
                        Take = 2,
                        AlwaysTake = false
                    },
                    //2 НЕ ЧЕТН.
                    new Predicate
                    {
                        Where = "(NumberOfTrain != null) && (NumberOfTrain != \"\") && (int.Parse(NumberOfTrain) % 2) == 1",
                        OrderBy = string.Empty,
                        Take = 2,
                        AlwaysTake = false
                    }
                }
            };

            //Act
            var res = inputs.Filter(filterEvenOdd, _defaultItemJson).ToList();

            //Assert
            res.Count.Should().Be(2);
            res[0].NumberOfTrain.Should().Be("0");
            res[1].NumberOfTrain.Should().Be("2");
        }


        [Fact]
        public void Filter_NumberOfTrain_2Even_2OddDefault_Filter()
        {
            var inputs = GetListAdInputType.GenerateListInputType(4, 2);
            var filterEvenOdd = new AgregateFilter
            {
                Filters = new List<Predicate>
                {
                    //2 ЧЕТН.
                    new Predicate
                    {
                        Where = "(NumberOfTrain != null) && (NumberOfTrain != \"\") && (int.Parse(NumberOfTrain) % 2) == 0",
                        OrderBy = string.Empty,
                        Take = 2,
                        AlwaysTake = true
                    },
                    //2 НЕ ЧЕТН.
                    new Predicate
                    {
                        Where = "(NumberOfTrain != null) && (NumberOfTrain != \"\") && (int.Parse(NumberOfTrain) % 2) == 1",
                        OrderBy = string.Empty,
                        Take = 2,
                        AlwaysTake = true
                    }
                }
            };
            
            //Act
            var res = inputs.Filter(filterEvenOdd, _defaultItemJson).ToList();

            //Assert
            res.Count.Should().Be(4);
            res[0].NumberOfTrain.Should().Be("0");
            res[1].NumberOfTrain.Should().Be("2");
            res[2].NumberOfTrain.Should().Be(null);
            res[3].NumberOfTrain.Should().Be(null);
        }


        [Fact]
        public void Filter_NumberOfTrain_1Even_1Odd_Filter()
        {
            var inputs = GetListAdInputType.GenerateListInputType(2,1);
            var filterEvenOdd = new AgregateFilter
            {
                Filters = new List<Predicate>
                {
                    //2 ЧЕТН.
                    new Predicate
                    {
                        Where = "(NumberOfTrain != null) && (NumberOfTrain != \"\") && (int.Parse(NumberOfTrain) % 2) == 0",
                        OrderBy = string.Empty,
                        Take = 2,
                        AlwaysTake = true
                    },
                    //2 НЕ ЧЕТН.
                    new Predicate
                    {
                        Where = "(NumberOfTrain != null) && (NumberOfTrain != \"\") && (int.Parse(NumberOfTrain) % 2) == 1",
                        OrderBy = string.Empty,
                        Take = 2,
                        AlwaysTake = true
                    }
                }
            };

            //Act
            var res = inputs.Filter(filterEvenOdd, _defaultItemJson).ToList();

            //Assert
            res.Count.Should().Be(4);
            res[0].NumberOfTrain.Should().Be("0");
            res[1].NumberOfTrain.Should().Be(null);
            res[2].NumberOfTrain.Should().Be("1");
            res[3].NumberOfTrain.Should().Be(null);
        }


        [Fact]
        public void Filter_NumberOfTrain_2EvenDefault_2OddDefault_Filter()
        {
            var inputs = GetListAdInputType.GenerateDefaultListInputType(10);
            var filterEvenOdd = new AgregateFilter
            {
                Filters = new List<Predicate>
                {
                    //2 ЧЕТН.
                    new Predicate
                    {
                        Where = "(NumberOfTrain != null) && (NumberOfTrain != \"\") && (int.Parse(NumberOfTrain) % 2) == 0",
                        OrderBy = string.Empty,
                        Take = 2,
                        AlwaysTake = true
                    },
                    //2 НЕ ЧЕТН.
                    new Predicate
                    {
                        Where = "(NumberOfTrain != null) && (NumberOfTrain != \"\") && (int.Parse(NumberOfTrain) % 2) == 1",
                        OrderBy = string.Empty,
                        Take = 2,
                        AlwaysTake = true
                    }
                }
            };

            //Act
            var res = inputs.Filter(filterEvenOdd, _defaultItemJson).ToList();

            //Assert
            res.Count.Should().Be(4);
            res[0].NumberOfTrain.Should().Be(null);
            res[1].NumberOfTrain.Should().Be(null);
            res[2].NumberOfTrain.Should().Be(null);
            res[3].NumberOfTrain.Should().Be(null);
        }


    }
}