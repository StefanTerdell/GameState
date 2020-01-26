using UnityEngine;

public class StatefulThing : MonoBehaviour {
    Color[] _colors;
    int _colorIndex;
    Renderer _renderer;

    void OnDestroy () {
        GameState.SaveState (gameObject, "ThingyColor", _colorIndex);
    }

    void Start () {
        _colorIndex = GameState.LoadState (gameObject, "ThingyColor", _colorIndex);

        _colors = new Color[] {
            Color.red,
            Color.blue,
            Color.green,
            Color.yellow
        };

        _renderer = GetComponent<Renderer> ();

        SetColor (_colorIndex);
    }

    void Update () {
        if (Input.GetKeyDown (KeyCode.Space))
            SetColor (++_colorIndex);
    }

    void SetColor (int index) {
        GameState.SaveState(gameObject, "ThingyColor", index);
        _renderer.material.color = _colors[index % _colors.Length];
    }
}