
using System.Collections.Generic;

namespace FarmSim.Utility
{
    public interface IItemRefsGUID
    {
        string itemGuid { set; }
    }

    public interface IWaterSourceRefsGUID
    {
        List<string> waterSrcGuids { get; }
    }
}