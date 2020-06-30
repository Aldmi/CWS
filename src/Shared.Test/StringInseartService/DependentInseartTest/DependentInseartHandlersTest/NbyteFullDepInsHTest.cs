﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CSharpFunctionalExtensions;
using Domain.InputDataModel.Shared.StringInseartService.DependentInseart.DependentInseartHandlers;
using Domain.InputDataModel.Shared.StringInseartService.Model;
using FluentAssertions;
using Shared.Test.StringInseartService.Datas;
using Xunit;

namespace Shared.Test.StringInseartService.DependentInseartTest.DependentInseartHandlersTest
{
    public class NbyteFullDepInsHTest
    {
        private readonly NbyteFullDepInsH _handler;
        private readonly IReadOnlyDictionary<string, StringInsertModelExt> _extDict;

        public NbyteFullDepInsHTest()
        {
            _extDict = GetStringInsertModelExtDict.SimpleDictionary;
            var requiredModel = StringInsertModelFactory.CreateList("{NbyteFull:X2}", _extDict).First();
            var crcModel = StringInsertModelFactory.CreateList("{CRCMod256:X2}", _extDict).First();
            _handler = new NbyteFullDepInsH(requiredModel, crcModel);
        }


        //[Fact]
        //public void Calc_Normal_With_Border_Test()
        //{
        //    //Arrange
        //    var requiredModel = StringInsertModelFactory.CreateList("{NbyteFull:X2_Border[0x03-10]}", _extDict).First();
        //    var crcModel = StringInsertModelFactory.CreateList("{CRCMod256:X2}", _extDict).First();
        //    var handler = new NbyteFullDepInsH(requiredModel, crcModel);
        //    var sb = new StringBuilder("0x050x{NbyteFull:X2_Border[0x03-10]}0x03^52^ 10:250x{CRCMod256:X2}");  //??? изменить ключ X2_Border[0x03-10] 
        //    var format = "cp866";
        //    //Act
        //    var (isSuccess, _, value) = handler.CalcInsert(sb, format);
        //    //Assert
        //    isSuccess.Should().BeTrue();
        //    value.ToString().Should().Be("0x050x0E0x03^52^ 10:250x{CRCMod256:X2}");
        //}



        [Fact]
        public void Calc_Normal_Test()
        {
            //Arrange
            var sb = new StringBuilder("0x050x{NbyteFull:X2}0x03^52^ 10:250x{CRCMod256:X2}");
            var format = "cp866";
            //Act
            var (isSuccess, _, value) = _handler.CalcInsert(sb, format);
            //Assert
            isSuccess.Should().BeTrue();
            value.ToString().Should().Be("0x050x0E0x03^52^ 10:250x{CRCMod256:X2}");
        }


        [Fact]
        public void Calc_Zero_NbyteFull_Test()
        {
            //Arrange
            var sb = new StringBuilder("\u000201{NbyteFull:X2}{CRCMod256:X2}\u0003");
            //Act
            var (isSuccess, _, value) = _handler.CalcInsert(sb);
            //Assert
            isSuccess.Should().BeTrue();
            value.ToString().Should().Be("\u00020103{CRCMod256:X2}\u0003");  //27
        }


        [Fact]
        public void Not_NbyteFull_Marker_In_Str_Test()
        {
            //Arrange
            var sb = new StringBuilder("\u000201%010C60EF03B0470000001E%110{CRCMod256:X2}\u0003");
            //Act
            var (isSuccess, _, _, error) = _handler.CalcInsert(sb);
            //Assert
            isSuccess.Should().BeFalse();
            error.Should().Be("Обработчик Dependency Inseart не может найти Replacement переменную {NbyteFull:X2} в строке \u000201%010C60EF03B0470000001E%110{CRCMod256:X2}\u0003");
        }


        [Fact]
        public void Not_Crc_Marker_in_str_Test()
        {
            //Arrange
            var sb = new StringBuilder("\u000201{NbyteFull:X2}%010C60EF03B0470000001E%110\u0003");
            //Act
            var (isSuccess, _, _, error) = _handler.CalcInsert(sb);
            //Assert
            isSuccess.Should().BeFalse();
            error.Should().Be("Невозможно выделить подстроку из строки \u000201{NbyteFull:X2}%010C60EF03B0470000001E%110\u0003 используя паттерн {NbyteFull:X2}(.*){CRCMod256:X2}");
        }


        [Fact]
        public void Not_Crc_optionalModel_Test()
        {
            //Arrange
            var requiredModel = StringInsertModelFactory.CreateList("{NbyteFull:X2}", _extDict).First();
            StringInsertModel optionalModel = null;

            //Act & Asert
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() => new NbyteFullDepInsH(requiredModel, optionalModel));
            exception.Should().BeOfType<ArgumentNullException>();
            exception.Message.Should().Contain("Value cannot be null. (Parameter 'crcModel')");
        }
    }
}