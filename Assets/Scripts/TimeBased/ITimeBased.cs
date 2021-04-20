
namespace FarmSim.TimeBased
{
    public interface ITimeBased
    {
        /// <summary>
        ///     Runs on the pass of each in-game day.
        /// </summary>
        void OnTimePass(int daysPassed = 1);
    }
}
