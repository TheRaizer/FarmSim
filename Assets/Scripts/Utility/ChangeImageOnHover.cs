using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ChangeImageOnHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Sprite spriteToChangeToo;

    private Sprite baseSprite;
    private Image img;

    public void OnPointerEnter(PointerEventData eventData)
    {
        img.sprite = spriteToChangeToo;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        img.sprite = baseSprite;
    }

    private void Awake()
    {
        img = GetComponent<Image>();
        baseSprite = img.sprite;
    }
}
