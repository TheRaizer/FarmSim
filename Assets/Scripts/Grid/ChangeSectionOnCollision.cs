using FarmSim.Serialization;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FarmSim.Grid
{
    public class ChangeSectionOnCollision : MonoBehaviour
    {
        [SerializeField] private int scene;
        private DataSaver dataSaver;

        private void Awake()
        {
            dataSaver = FindObjectOfType<DataSaver>();
        }

        public void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                dataSaver.SaveSectionVoid(NodeGrid.Instance.IsSavableSection, NodeGrid.Instance.SectionNum);

                NodeGrid.Instance.SectionNum = scene - 1;
                SectionData sect = (SectionData)SerializationManager.Load(Application.persistentDataPath + "/saves/Section_" + NodeGrid.Instance.SectionNum + ".save");

                // if there is no save
                if (sect == null)
                {
                    // create empty section data
                    SectionData.Current = new SectionData();
                }
                else
                {
                    // otherwise use the previous section data.s
                    SectionData.Current = sect;
                }
                SceneManager.LoadScene(scene);
            }
        }
    }
}
