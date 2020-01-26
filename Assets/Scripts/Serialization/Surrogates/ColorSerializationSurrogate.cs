using System.Runtime.Serialization;
using UnityEngine;

public class ColorSerializationSurrogate : ISerializationSurrogate {
    public void GetObjectData (object obj, SerializationInfo info, StreamingContext context) {
        var c = (Color) obj;
        info.AddValue ("r", c.r);
        info.AddValue ("g", c.g);
        info.AddValue ("b", c.b);
        info.AddValue ("a", c.a);
    }

    public object SetObjectData (object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector) {
        var c = (Color) obj;
        c.r = (float) info.GetValue ("r", typeof (float));
        c.g = (float) info.GetValue ("g", typeof (float));
        c.b = (float) info.GetValue ("b", typeof (float));
        c.a = (float) info.GetValue ("a", typeof (float));
        obj = c;
        return obj;
    }
}