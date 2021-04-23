using UnityEngine;

public class WaitForTrueBool : CustomYieldInstruction
{
    private readonly BoolWrapper boolean;
    private readonly float timeout;
    private readonly float startTime;
    private bool timedOut;

    public bool TimedOut => timedOut;

    public override bool keepWaiting
    {
        get
        {
            if (Time.realtimeSinceStartup - startTime >= timeout)
            {
                timedOut = true;
            }

            return !boolean.GetValue() && !timedOut;
        }
    }

    public WaitForTrueBool(BoolWrapper _boolean, float _timeout = 10)
    {
        boolean = _boolean;
        timeout = _timeout;
        startTime = Time.realtimeSinceStartup;
    }
}

public class BoolWrapper
{
    private bool value;

    public bool GetValue()
    {
        return value;
    }

    public bool SetValue(bool newValue) => value = newValue;

    public BoolWrapper(bool value) { this.value = value; }
}
