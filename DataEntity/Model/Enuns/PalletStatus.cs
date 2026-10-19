using System.ComponentModel;

namespace DataEntity;

public enum PalletStatus
{
    [Description("Zaskladněno")]
    Stored = 0,
    [Description("Vyskladněno")]
    Dispatched = 1,
    [Description("V dopravě")]
    InTransit = 2
}
