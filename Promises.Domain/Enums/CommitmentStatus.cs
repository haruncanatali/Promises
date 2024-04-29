using System.ComponentModel;

namespace Promises.Domain.Enums;

public enum CommitmentStatus
{
    [Description("Söz Verildi")]
    Promised = 1,
    [Description("Söz Alındı")]
    Committed
}