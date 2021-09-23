using AutoMapper;
using Domain.InputDataModel.Shared.StringInseartService.Model;
using FluentAssertions;
using Infrastructure.Dal.EfCore.Entities.StringInsertModelExt;
using Shared.Mathematic;
using Shared.Types;
using WebApiSwc.AutoMapperConfig;
using WebApiSwc.DTO.JSON.InputTypesDto;
using WebApiSwc.DTO.JSON.OptionsDto.StringInsertModelExt;
using WebApiSwc.DTO.JSON.Shared;
using Xunit;

namespace MappingTest
{
    public class StringInsertModelExtTest
    {
        private readonly IMapper _mapper;
        private readonly StringInsertModelExt _model;
        private readonly StringInsertModelExtDto _dto;
        private readonly EfStringInseartModelExt _ef;

        public StringInsertModelExtTest()
        {
            var myProfile = new MappingProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
            _mapper = new Mapper(configuration);

            _model = new StringInsertModelExt("X2", ":X2", null, null, new MathematicFormula("x+10"));

            _dto = new StringInsertModelExtDto
            {
                Key = "X2",
                Format = ":X2",
                BorderSubString = null,
                StringHandlerMiddleWareOption = null,
                MathematicFormula = new MathematicFormulaDto {Expr = "x+10"}
            };

            _ef = new EfStringInseartModelExt()
            {
                Key = "X2",
                Format = ":X2",
                BorderSubString = null,
                StringHandlerMiddleWareOption = null,
                MathematicFormula = new EfMathematicFormula { Expr = "x+10" }
            };
        }



        [Fact]
        public void Model_2_Dto_MapTest()
        {
            //Act
            var dto = _mapper.Map<StringInsertModelExtDto>(_model);

            //Asert
            dto.MathematicFormula.Expr.Should().Be("x+10");
        }


        [Fact]
        public void Dto_2_Model_MapTest()
        {
            //Act
            var model = _mapper.Map<StringInsertModelExt>(_dto);

            //Asert
            model.MathematicFormula.Expr.Should().Be("x+10");
        }


        [Fact]
        public void Model_2_Ef_MapTest()
        {
            //Act
            var ef = _mapper.Map<EfStringInseartModelExt>(_model);

            //Asert
            ef.MathematicFormula.Expr.Should().Be("x+10");
        }


        [Fact]
        public void Ef_2_Model_MapTest()
        {
            //Act
            var model = _mapper.Map<StringInsertModelExt>(_ef);

            //Asert
            model.MathematicFormula.Expr.Should().Be("x+10");
        }

    }
}