using UnityEngine;
using UnityEngine.SceneManagement;

namespace FarmSim.Grid
{
    public class ChangeSectionOnCollision : MonoBehaviour
    {
        [SerializeField] private int scene;

        public void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                NodeGrid.Instance.SectionNum = scene - 1;
                SceneManager.LoadScene(scene);
            }
        }
    }
}
