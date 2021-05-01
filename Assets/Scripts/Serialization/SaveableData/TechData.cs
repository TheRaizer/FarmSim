using UnityEngine;

[System.Serializable]
public class TechData
{
    public Vector2 pos;
    public string prefabName;
    public string guid;

    public TechData(Vector2 pos, string prefabName, string guid)
    {
        this.pos = pos;
        this.prefabName = prefabName;
        this.guid = guid;
    }
}

public interface ITechData
{
    /// <summary>
    ///     Allows the data to be assigned from the techs creator.
    ///     <remarks>
    ///         <para>The data should not assigned from elsewhere.</para>
    ///     </remarks>
    /// </summary>
    TechData Data { set; }
} 
