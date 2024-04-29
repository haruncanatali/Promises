using NpgsqlTypes;
using Serilog.Events;
using Serilog.Sinks.PostgreSQL;

namespace Promises.Api.Configs.ColumnWriters;

public class StatusColorColumnWriter : ColumnWriterBase
{
    public StatusColorColumnWriter() : base(NpgsqlDbType.Varchar)
    {
    }

    public override object GetValue(LogEvent logEvent, IFormatProvider formatProvider = null)
    {
        var (username, value) = logEvent.Properties.FirstOrDefault(c => c.Key == "StatusColor");
        return value?.ToString();
    }
}