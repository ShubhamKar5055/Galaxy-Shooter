using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {
    public void loadGame() {
        SceneManager.LoadScene(1); // Main Game Scene
    }

    public void quitGame() {
        Application.Quit();
    }
}
