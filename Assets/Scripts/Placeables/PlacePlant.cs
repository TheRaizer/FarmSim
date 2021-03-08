
using FarmSim.Enums;

namespace FarmSim.Placeable
{
    public class PlacePlant : Placeable
    {
        protected override void OnPlace()
        {
            Node.Interactable.OnInteract(ToolTypes.OTHER, objectToPlace);
        }
    }
}
