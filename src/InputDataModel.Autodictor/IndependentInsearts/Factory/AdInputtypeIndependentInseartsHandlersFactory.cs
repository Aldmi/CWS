using Domain.InputDataModel.Autodictor.IndependentInsearts.Handlers;
using Shared.Services.StringInseartService;

namespace Domain.InputDataModel.Autodictor.IndependentInsearts.Factory
{
    public class AdInputTypeIndependentInseartsHandlersFactory : IIndependentInseartsHandlersFactory
    {
        public IIndependentInsertsHandler Create(StringInsertModel insertModel)
        {
            switch (insertModel.VarName)
            {
                case "TypeName":
                    return new TypeNameInsH(insertModel);

                case "TypeAlias":
                    return new TypeAliasInsH(insertModel);

                case "NumberOfTrain":
                    return new NumberOfTrainInsH(insertModel);

                case "PathNumber":
                    return new PathNumberInsH(insertModel);

                case "Platform":
                    return new PlatformInsH(insertModel);

                case "Event":
                    return new EventInsH(insertModel);

                case "Addition":
                    return new AdditionInsH(insertModel);

                case "Stations":
                    return new StationsInsH(insertModel);

                case "StationsCut":
                    return new StationsCutInsH(insertModel);

                case "StationArrival":
                    return new StationArrivalInsH(insertModel);

                case "StationDeparture":
                    return new StationDepartureInsH(insertModel);

                case "Note":
                    return new NoteInsH(insertModel);

                case "DaysFollowing":
                    return new DaysFollowingInsH(insertModel);

                case "DaysFollowingAlias":
                    return new DaysFollowingAliasInsH(insertModel);

                case "TArrival":
                    return new TArrivalInsH(insertModel);

                case "TDepart":
                    return new TDepartInsH(insertModel);

                case "DelayTime":
                    return new DelayTimeInsH(insertModel);

                case "Time":
                    return new TimeInsH(insertModel);

                case "ExpectedTime":
                    return new ExpectedTimeInsH(insertModel);

                case "SyncTInSec":
                    return new SyncTInSecInsH(insertModel);

                case "Year":
                    return new YearInsH(insertModel);

                case "Month":
                    return new MonthInsH(insertModel);

                case "Day":
                    return new DayInsH(insertModel);

                case "Hour":
                    return new HourInsH(insertModel);

                case "Minute":
                    return new MinuteInsH(insertModel);

                case "Second":
                    return new SecondInsH(insertModel);

                default: return null;
            }
        }
    }
}