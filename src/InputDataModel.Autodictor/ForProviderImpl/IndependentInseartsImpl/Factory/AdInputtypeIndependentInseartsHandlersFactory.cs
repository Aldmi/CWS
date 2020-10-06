using Domain.InputDataModel.Autodictor.Entities;
using Domain.InputDataModel.Autodictor.ForProviderImpl.IndependentInseartsImpl.Handlers;
using Domain.InputDataModel.Shared.StringInseartService.IndependentInseart.IndependentInseartHandlers;
using Domain.InputDataModel.Shared.StringInseartService.Model;

namespace Domain.InputDataModel.Autodictor.ForProviderImpl.IndependentInseartsImpl.Factory
{
    public class AdInputTypeIndependentInseartsHandlersFactory : IIndependentInseartsHandlersFactory
    {
        public IIndependentInsertsHandler Create(StringInsertModel insertModel)
        {
            return insertModel.VarName switch
            {
                "TypeName" => new TypeNameInsH(insertModel),
                "TypeAlias" => new TypeAliasInsH(insertModel),
                "NumberOfTrain" => new NumberOfTrainInsH(insertModel),
                "PathNumber" => new PathNumberInsH(insertModel),
                "Platform" => new PlatformInsH(insertModel),
                "Event" => new EventInsH(insertModel),
                "EventAlias" => new EventAliasInsH(insertModel),
                "Addition" => new AdditionInsH(insertModel),
                "Stations" => new StationsInsH(insertModel),
                "StationsCut" => new StationsCutInsH(insertModel),
                "StationArrival" => new StationArrivalInsH(insertModel),
                "StationDeparture" => new StationDepartureInsH(insertModel),
                "Note" => new NoteInsH(insertModel),
                "DaysFollowing" => new DaysFollowingInsH(insertModel),
                "DaysFollowingAlias" => new DaysFollowingAliasInsH(insertModel),
                "TArrival" => new TArrivalInsH(insertModel),
                "TDepart" => new TDepartInsH(insertModel),
                "DelayTime" => new DelayTimeInsH(insertModel),
                "Time" => new TimeInsH(insertModel),
                "ExpectedTime" => new ExpectedTimeInsH(insertModel),
                "StopTime" => new StopTimeInsH(insertModel),
                "SyncTInSec" => new SyncTInSecInsH(insertModel),
                "Year" => new YearInsH(insertModel),
                "Month" => new MonthInsH(insertModel),
                "Day" => new DayInsH(insertModel),
                "Hour" => new HourInsH(insertModel),
                "Minute" => new MinuteInsH(insertModel),
                "Second" => new SecondInsH(insertModel),
                "DayOfWeek" => new DayOfWeekInsH(insertModel),
                "Lang" => new LangInsH(insertModel),
                "VagonDirection" => new VagonDirectionInsH(insertModel),
                "Emergency" => new EmergencyInsH(insertModel),
                "Category"=> new CategoryInsH(insertModel),
                _ => null
            };
        }
    }
}