using UnityEngine;

public class Bobbing : MonoBehaviour
{
    [SerializeField] private float amplitude;
    [SerializeField] private float frequency;

    private float initialY;
    private bool isBobbing = false;

    public void StartBob()
    {
        initialY = transform.position.y;
        isBobbing = true;
    }

    public void StopBob()
    {
        isBobbing = false;
    }

    void Update()
    {
        if (isBobbing)
        {
            float y = Mathf.Sin(Time.time * frequency) * amplitude;
            transform.position = new Vector2(transform.position.x, y + initialY);
        }
    }
}
