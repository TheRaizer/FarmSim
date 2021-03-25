using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

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
