using FarmSim.SavableData;
using FarmSim.Serialization;
using UnityEngine;

/// <class name="WorldItemLoader">
///     <summary>
///         Loads/Instantiates all the world item objects that were saved in the given section while data is being injected into the other objects.
///     </summary>
/// </class>
public class WorldItemLoader : MonoBehaviour, ILoadable
{
    private const string WRLD_ITEMS_PREFAB_FOLDER = "Prefabs/WorldItems/";

    public void Load()
    {
        if (SectionData.Current.WorldItemDatas != null)
        {
            foreach (WorldItemData data in SectionData.Current.WorldItemDatas)
            {
                // get the wrld item prefab
                var prefab = Resources.Load(WRLD_ITEMS_PREFAB_FOLDER + data.prefabName) as GameObject;

                if (prefab == null)
                {
                    Debug.LogError("There is no tech prefab at path: " + WRLD_ITEMS_PREFAB_FOLDER + data.prefabName);
                }

                var gameObject = Instantiate(prefab);
                gameObject.transform.position = data.Pos;

                gameObject.GetComponent<IWorldItem>().Data = data;
            }
        }
    }
}
