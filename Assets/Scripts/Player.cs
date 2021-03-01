using UnityEngine;

public class Player : MonoBehaviour
{
    [field: SerializeField] public ITool Tool { get; set; }

    private void LateUpdate()
    {
        OnMouseClick();
    }

    private void OnMouseClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if(Tool != null)
            {
                Tool.OnUse();
            }
        }
    }
}
