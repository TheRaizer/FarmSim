
namespace FarmSim.Entity
{
    /// <class name="CurrencyManager">
    ///     <summary>
    ///         Manages specifically the amount of currency an entity has.
    ///     </summary>
    /// </class>
    public class CurrencyManager : AmountManager
    {
        public bool Subtractable(int amt)
        {
            return CurrentAmt - amt >= 0;
        }
    }
}
