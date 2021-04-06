using UnityEngine;

namespace FarmSim.Utility
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private float cameraSpeed;
        [SerializeField] private float targetMoveSpeed;

        [SerializeField] private Transform target;

        private void Awake()
        {
            target.position = new Vector3(target.position.x, target.position.y, transform.position.z);
        }

        private void LateUpdate()
        {
            Vector3 newPos = Vector3.Lerp(transform.position, target.position, cameraSpeed);
            transform.position = newPos;
        }
    }
}
