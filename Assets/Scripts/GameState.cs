using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class GameState : MonoBehaviour {
    public static string SavePath => Application.persistentDataPath + "/saves/";
    public const string SaveExtension = ".save";

    public static GameState Current => st_current;
    static GameState st_current;
    static string st_currentStateFileName;

    Dictionary<string, object> _keyValueStore = new Dictionary<string, object> ();
    Dictionary<GameObject, string> _keyDictionary = new Dictionary<GameObject, string> ();

    static string GetKey (GameObject gameObject, string label) {
        if (!st_current._keyDictionary.ContainsKey (gameObject)) {
            st_current._keyDictionary.Add (gameObject, GlobalObjectId.GetGlobalObjectIdSlow (gameObject).ToString ());
        }

        return $"{gameObject.name}_{label}_{st_current._keyDictionary[gameObject]}";
    }

    void Awake () {
        if (st_current) {
            Destroy (this);
        } else {
            st_current = this;
            DontDestroyOnLoad (this);
        }
    }

    public static t LoadState<t> (GameObject gameObject, string label, t fallback = default (t)) {
        return LoadStateByKey (GetKey (gameObject, label), fallback);
    }

    public static void SaveState<t> (GameObject gameObject, string label, t value) {
        SaveStateByKey (GetKey (gameObject, label), value);
    }

    public static t LoadStateByKey<t> (string key, t fallback = default (t)) {
        if (!st_current._keyValueStore.ContainsKey (key))
            return fallback;

        return (t) st_current._keyValueStore[key];
    }

    public static void SaveStateByKey<t> (string key, t value) {
        if (st_current._keyValueStore.ContainsKey (key))
            st_current._keyValueStore[key] = value;
        else
            st_current._keyValueStore.Add (key, value);
    }

    public static void LoadStateFile (string name) {
        st_current._keyValueStore = (Dictionary<string, object>) SerializationManager.Load (SavePath + name + SaveExtension);
        st_currentStateFileName = name;
    }

    public static void SaveStateFile () {
        SaveStateFile (st_currentStateFileName);
    }

    public static void SaveStateFile (string name) {
        SerializationManager.Save (SavePath + name + SaveExtension, st_current._keyValueStore);
        st_currentStateFileName = name;
    }

    public static void NewStateFile (string name) {
        st_current._keyValueStore = new Dictionary<string, object> ();
        st_currentStateFileName = name;
    }

    public TMP_ENTRY[] TMP_ENTRIES;

    [System.Serializable]
    public class TMP_ENTRY {
        public string key;
        public string value;
        public TMP_ENTRY (string key, string value) {
            this.key = key;
            this.value = value;
        }
    }

    void Update () {
        TMP_ENTRIES = _keyValueStore.Select (e => new TMP_ENTRY (e.Key, e.Value.ToString ())).ToArray ();
    }
}