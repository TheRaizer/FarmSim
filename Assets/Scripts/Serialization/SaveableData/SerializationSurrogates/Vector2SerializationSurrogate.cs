using System.Runtime.Serialization;
using UnityEngine;

namespace FarmSim.Serialization
{
    /// <class name="Vector2SerializationSurrogate">
    ///     <summary>
    ///         A surrogate of a <see cref="Vector2"/> that serializes it into 'x' and 'y' <see cref="float"/> values.
    ///     </summary>
    /// </class>
    public class Vector2SerializationSurrogate : ISerializationSurrogate
    {
        public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
        {
            Vector2 vector2 = (Vector2)obj;
            info.AddValue("x", vector2.x);
            info.AddValue("y", vector2.y);
        }

        public object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
        {
            Vector2 vector2 = (Vector2)obj;
            vector2.x = (float)info.GetValue("x", typeof(float));
            vector2.y = (float)info.GetValue("y", typeof(float));

            obj = vector2;
            return obj;
        }
    }
}