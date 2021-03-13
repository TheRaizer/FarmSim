using System;

namespace FarmSim.Utility
{
    public static class UniqueIdGenerator
    {
        public static string IdFromDate()
        {
            string id = DateTime.Now.ToLongDateString() + DateTime.Now.ToLongTimeString() + UnityEngine.Random.Range(0, int.MaxValue).ToString();

            return id;
        }
    }
}