using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMenu : MonoBehaviour
{
    [SerializeField] string s_targetScene;

    public void Save() {
        GameState.SaveStateByKey("Current Scene", SceneManager.GetActiveScene().name);
        GameState.SaveStateFile();
    }

    public void ChangeScene() {
        SceneManager.LoadScene(s_targetScene);
    }
}
