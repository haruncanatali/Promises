using NpgsqlTypes;
using Serilog.Events;
using Serilog.Sinks.PostgreSQL;

namespace Promises.Api.Configs.ColumnWriters;

public class UserIdColumnWriter : ColumnWriterBase
{
    public UserIdColumnWriter() : base(NpgsqlDbType.Varchar)
    {
    }

    public override object GetValue(LogEvent logEvent, IFormatProvider formatProvider = null)
    {
        var (username, value) = logEvent.Properties.FirstOrDefault(c => c.Key == "UserId");
        return value?.ToString();
    }
}