using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SerializationManager {
    public static bool Save (string fullPath, object saveData) {

        var directory = fullPath.Substring (0, fullPath.LastIndexOf ("/"));

        if (!Directory.Exists (directory))
            Directory.CreateDirectory (directory);

        var formatter = GetBinaryFormatter ();

        FileStream file = File.Create (fullPath);
        formatter.Serialize (file, saveData);

        file.Close ();

        return true;
    }

    public static object Load (string fullPath) {

        if (!File.Exists (fullPath))
            return null;

        var formatter = GetBinaryFormatter ();

        FileStream file = File.Open (fullPath, FileMode.Open);

        try {
            var save = formatter.Deserialize (file);
            file.Close ();
            return save;
        } catch {
            Debug.LogErrorFormat ("Failed to load file at {0}", fullPath);
            return null;
        }
    }

    static BinaryFormatter GetBinaryFormatter () {
        var formatter = new BinaryFormatter ();
        var selector = new SurrogateSelector ();
        
        var v3Surrogate = new Vector3SerializationSurrogate ();
        var qSurrogate = new QuaternionSerializationSurrogate ();
        var cSurrogate = new ColorSerializationSurrogate ();

        selector.AddSurrogate (typeof (Vector3), new StreamingContext (StreamingContextStates.All), v3Surrogate);
        selector.AddSurrogate (typeof (Quaternion), new StreamingContext (StreamingContextStates.All), qSurrogate);
        selector.AddSurrogate (typeof (Color), new StreamingContext (StreamingContextStates.All), cSurrogate);

        formatter.SurrogateSelector = selector;

        return formatter;
    }
}