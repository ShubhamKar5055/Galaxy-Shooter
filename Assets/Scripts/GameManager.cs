using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    [SerializeField]
    private float _gameDifficulty = 50.0f;
    private bool _isDifficultyIncremented;
    private Player _player;
    private UIManager _uiManager;
    private SpawnManager _spawnManager;
    private float remainingTime = 30.0f; // 30 seconds
    private bool _playerVictory = false;
    private bool _isGameOver = false;

    // Start is called before the first frame update
    void Start() {
        _player = GameObject.Find("Player").GetComponent<Player>();
        if(_player == null) {
            Debug.LogError("The Player is NULL.");
        }

        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        if(_uiManager == null) {
            Debug.LogError("The UI Manager is NULL.");
        }

        _spawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();
        if(_spawnManager == null) {
            Debug.LogError("The Spawn Manager is NULL.");
        }
    }

    // Update is called once per frame
    void Update() {
        int playerScore = _player.getScore();
        if((playerScore % 100) == 0 && playerScore != 0 && playerScore <= 500) {
            if(!_isDifficultyIncremented) {
                incrementGameDifficulty(); // Game Difficulty Increments for every 100 points
                _uiManager.updateDifficultyText();
                _isDifficultyIncremented = true;
            }
        } else {
            _isDifficultyIncremented = false;
        }

        if(Input.GetKeyDown(KeyCode.R) && _isGameOver == true) {
            SceneManager.LoadScene(1); // Current Game Scene
        }

        if(Input.GetKey("escape")) {
            Application.Quit();
        }
    }

    public float getGameDifficulty() {
        return _gameDifficulty;
    }

    void incrementGameDifficulty() {
        _gameDifficulty += 10.0f;
    }

    public string timer() {
        remainingTime -= Time.deltaTime;
        int remainingTimeInt = Mathf.FloorToInt(remainingTime);

        if(remainingTimeInt < 0) {
            return null;
        }

        return string.Format("00:{0:D2}", remainingTimeInt); // D2 ensures that the number is displayed as at least two digits
    }

    public bool playerVictory() {
        return _playerVictory;
    }

    public void playerVictorySequence() {
        _playerVictory = true;
        _spawnManager.onPlayerDeathOrVictory();
        _uiManager.gameOverSequence();
        destroyAllEnemiesLasersAndAsteroids();
    }

    void destroyAllEnemiesLasersAndAsteroids() {
        List<GameObject> allObjectsToDestroy = new List<GameObject>();

        // Find all objects with the "Enemy" tag and add them to the list
        allObjectsToDestroy.AddRange(GameObject.FindGameObjectsWithTag("Enemy"));

        // Find all objects with the "Enemy Laser" tag and add them to the list
        allObjectsToDestroy.AddRange(GameObject.FindGameObjectsWithTag("Enemy Laser"));

        // Find all objects with the "Asteroid" tag and add them to the list
        allObjectsToDestroy.AddRange(GameObject.FindGameObjectsWithTag("Asteroid"));

        foreach (GameObject obj in allObjectsToDestroy) {
            Destroy(obj);
        }
    }

    public void gameOver() {
        _isGameOver = true;
    }
}
