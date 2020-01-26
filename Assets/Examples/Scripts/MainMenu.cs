using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {

    public Dropdown c_dropdown;
    public InputField c_newFileName;

    void Start () {
        c_dropdown.options = GetLoadFiles ();
    }

    public List<Dropdown.OptionData> GetLoadFiles () {
        if (!Directory.Exists (GameState.SavePath)) {
            Directory.CreateDirectory (GameState.SavePath);
        }

        return Directory.GetFiles (GameState.SavePath).Select (longName => {
            var lastSlash = longName.LastIndexOf ('/') + 1;
            var lastDot = longName.LastIndexOf ('.');
            var shortName = longName.Substring (lastSlash, lastDot - lastSlash);

            return new Dropdown.OptionData (shortName);
        }).ToList ();
    }

    public void Load () {
        GameState.LoadStateFile (c_dropdown.options[c_dropdown.value].text);
        SceneManager.LoadScene (GameState.LoadStateByKey<string> ("Current Scene"));
    }

    public void New () {
        GameState.NewStateFile (c_newFileName.text);
        GameState.SaveStateByKey ("Current Scene", "Scene A");
        SceneManager.LoadScene ("Scene A");
    }
}