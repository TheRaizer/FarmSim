public class CurrencyManager : AmountManager
{
    public bool Subtractable(int amt)
    {
        return CurrentAmt - amt >= 0;
    }
}
