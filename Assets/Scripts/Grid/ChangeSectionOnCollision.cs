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

        private void Awake()
        {
            dataSaver = FindObjectOfType<DataSaver>();
        }

        public void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                // save the current section
                dataSaver.SaveSectionVoid(NodeGrid.Instance.IsSavableSection, NodeGrid.Instance.SectionNum);

                // get the new sections number
                NodeGrid.Instance.SectionNum = scene - 1;

                // load the new section
                SectionData sect = (SectionData)SerializationManager.LoadSave(SavePaths.SECTION_PREFIX + NodeGrid.Instance.SectionNum, SavePaths.SECTIONS_DIRECTORY);

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
    }
}
