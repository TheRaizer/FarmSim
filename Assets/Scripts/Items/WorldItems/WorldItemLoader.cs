using FarmSim.Serialization;
using UnityEngine;

public class WorldItemLoader : MonoBehaviour, IOccurPostLoad
{
    private const string WRLD_ITEMS_PREFAB_FOLDER = "Prefabs/WorldItems/";
    public void PostLoad()
    {
        if (SectionData.Current.WorldItemDatas != null)
        {
            foreach (WorldItemData data in SectionData.Current.WorldItemDatas)
            {
                // get the tech prefab
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
