using FarmSim.Utility;
using UnityEngine;

namespace FarmSim.Placeable {
    public class SpawnPlaceableOnClick : MonoBehaviour
    {
        [SerializeField] private string objectId;
        private MoveObject moveObject = null;
        private readonly ObjectPooler objectPooler = null;

        private void Awake()
        {
            moveObject = FindObjectOfType<MoveObject>();
        }

        private void OnMouseDown()
        {
            GameObject obj = objectPooler.SpawnGameObject(objectId, Vector2.zero, Quaternion.identity);
            if(obj.TryGetComponent(out Placeable placeable))
            {
                moveObject.AttachedObject = placeable;
            }
            Debug.Log("Mouse down");
        }
    }
}
