using NpgsqlTypes;
using Serilog.Events;
using Serilog.Sinks.PostgreSQL;

namespace Promises.Api.Configs.ColumnWriters;

public class RequestMethodColumnWriter : ColumnWriterBase
{
    public RequestMethodColumnWriter() : base(NpgsqlDbType.Varchar)
    {
    }

    public override object GetValue(LogEvent logEvent, IFormatProvider formatProvider = null)
    {
        var (username, value) = logEvent.Properties.FirstOrDefault(c => c.Key == "RequestMethod");
        return value?.ToString();
    }
}