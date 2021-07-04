using FarmSim.SavableData;
using FarmSim.Serialization;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FarmSim.Grid
{
    /// <class name="ChangeSectionOnCollision">
    ///     <summary>
    ///         Changes the grid section and scene of the map.
    ///     </summary>
    /// </class>
    public class ChangeSectionOnCollision : MonoBehaviour
    {
        [SerializeField] private int scene;
        private DataSaver dataSaver;
        private NodeGrid nodeGrid;

        private void Awake()
        {
            dataSaver = FindObjectOfType<DataSaver>();
            nodeGrid = FindObjectOfType<NodeGrid>();
        }

        public void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                SaveCurrentSection();

                SectionData sect = LoadNextSection();

                // if there is no save
                if (sect == null)
                {
                    // create empty section data
                    SectionData.Current = new SectionData();
                }
                else
                {
                    // make the current section data to point to the new sections data.
                    SectionData.Current = sect;
                }
                SceneManager.LoadScene(scene);
            }
        }

        private void SaveCurrentSection()
        {
            dataSaver.SaveSection(nodeGrid.IsSavableSection, nodeGrid.SectionNum);
        }

        private SectionData LoadNextSection()
        {
            // assign the next sections number
            nodeGrid.SectionNum = scene - 1;

            // load the new section
            SectionData sect = (SectionData)SerializationManager.LoadSave(SavePaths.SECTION_PREFIX + nodeGrid.SectionNum, SavePaths.SECTIONS_DIRECTORY);

            return sect;
        }
    }
}
