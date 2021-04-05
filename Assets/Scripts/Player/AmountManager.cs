using UnityEngine;

public class AmountManager : MonoBehaviour
{
    [SerializeField] protected int maxAmt;
    public int CurrentAmt { get; private set; }

    public void IncreaseMax(int newMax) => maxAmt = newMax;
    public void IncreaseAmt(int amt) => CurrentAmt = Mathf.Clamp(CurrentAmt + amt, 0, maxAmt);
    public void DecreaseAmt(int amt) => CurrentAmt = Mathf.Clamp(CurrentAmt - amt, 0, maxAmt);
    public void ZeroOut() => CurrentAmt = 0;
    public void MaxOut() => CurrentAmt = maxAmt;
}
