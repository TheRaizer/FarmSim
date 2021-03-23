using UnityEngine;

namespace FarmSim.Utility 
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private float cameraSpeed;
        [SerializeField] private float targetMoveSpeed;

        private Transform target;

        private void Awake()
        {
            target.position = transform.position;
        }

        private void FixedUpdate()
        {
            Vector2.Lerp(transform.position, target.position, cameraSpeed);
        }

        private void LateUpdate()
        {
            KeyHandler();
        }

        private void KeyHandler()
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                target.position += Vector3.left * 5 * Time.deltaTime;
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                target.position += Vector3.left * 5 * Time.deltaTime;
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                target.position += Vector3.left * 5 * Time.deltaTime;
            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                target.position += Vector3.left * 5 * Time.deltaTime;
            }
        }
    }
}
