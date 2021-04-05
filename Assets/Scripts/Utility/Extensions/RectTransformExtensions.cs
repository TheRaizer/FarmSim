using UnityEngine;

namespace FarmSim.Utility
{
    /// <class name="RectTransformExtensions">
    ///     <summary>
    ///         Extensions that assist the use of <see cref="RectTransform"/>'s
    ///     </summary>
    /// </class>
    public static class RectTransformExtensions
    {
        public static void SetLeft(this RectTransform rt, float left)
        {
            rt.offsetMin = new Vector2(left, rt.offsetMin.y);
        }

        public static void SetRight(this RectTransform rt, float right)
        {
            rt.offsetMax = new Vector2(-right, rt.offsetMax.y);
        }

        public static void SetTop(this RectTransform rt, float top)
        {
            rt.offsetMax = new Vector2(rt.offsetMax.x, -top);
        }

        public static void SetBottom(this RectTransform rt, float bottom)
        {
            rt.offsetMin = new Vector2(rt.offsetMin.x, bottom);
        }

        public static void Center(this RectTransform rt)
        {
            // reset its position to the origin
            rt.SetLeft(0);
            rt.SetRight(0);
            rt.SetTop(0);
            rt.SetBottom(0);
        }

        public static void SetToMouse(this RectTransform rt, Canvas canvas)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, Input.mousePosition, canvas.worldCamera, out Vector2 pos);
            rt.transform.position = canvas.transform.TransformPoint(pos);
        }
    }
}