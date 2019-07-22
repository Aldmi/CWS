using System.Collections.Generic;
using Serilog.Events;

namespace WebApiSwc.Settings
{
    public class LoggerSettings
    {
        public readonly LogEventLevel MinLevel;
        public readonly FileSinkSetting FileSinkSetting;
        public readonly ElasticsearchSinkSetting ElasticsearchSinkSetting;

        public LoggerSettings(LogEventLevel minLevel, FileSinkSetting fileSinkSetting, ElasticsearchSinkSetting elasticsearchSinkSetting)
        {
            MinLevel = minLevel;
            FileSinkSetting = fileSinkSetting;
            ElasticsearchSinkSetting = elasticsearchSinkSetting;
        }

    }


    public abstract class SinkSetting
    {
        public readonly bool Enable;


        protected SinkSetting(bool enable)
        {
            Enable = enable;
        }
    }



    public class FileSinkSetting : SinkSetting
    {
        public FileSinkSetting(bool enable) : base(enable)
        {
        }
    }


    public class ElasticsearchSinkSetting : SinkSetting
    {
        public ElasticsearchSinkSetting(bool enable) : base(enable)
        {
        }
    }
}