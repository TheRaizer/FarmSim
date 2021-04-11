using UnityEngine;

/// <class name="PlayerUIManager">
///     <summary>
///         Manages the activeness of each UI the player can control.
///     </summary>
/// </class>
public class PlayerUIManager : MonoBehaviour
{
    [SerializeField] private GameObject inventoryUI;

    private void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            inventoryUI.SetActive(!inventoryUI.activeInHierarchy);
        }
    }
}
