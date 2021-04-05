using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrencyManager : AmountManager
{
    public bool Subtractable(int amt)
    {
        return CurrentAmt - amt >= 0;
    }
}
