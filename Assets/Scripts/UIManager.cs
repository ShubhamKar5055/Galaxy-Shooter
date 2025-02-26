using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {
    [SerializeField]
    private Text _scoreText;
    [SerializeField]
    private Text _difficultyText;
    [SerializeField]
    private Sprite[] _liveSprites;
    [SerializeField]
    private Image _livesDisplayImage;
    [SerializeField]
    private Text _gameOverText;
    [SerializeField]
    private Text _restartText;
    [SerializeField]
    private Text _quitText;
    [SerializeField]
    private Text _timerText;
    [SerializeField]
    private Text _playerVictory;
    private GameManager _gameManager;
    private AudioManager _audioManager;

    // Start is called before the first frame update
    void Start() {
        _scoreText.text = "Score: 0";
        _gameOverText.enabled = false;
        _restartText.enabled = false;
        _quitText.enabled = false;
        _timerText.enabled = false;

        _gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        if(_gameManager == null) {
            Debug.LogError("The Game Manager is NULL.");
        }

        _audioManager = GameObject.Find("Audio Manager").GetComponent<AudioManager>();
        if(_audioManager == null) {
            Debug.LogError("The Audio Manager is NULL.");
        }
    }

    // Update is called once per frame
    void Update() {
        if(_timerText.enabled == true) {
            displayTimerText();
        }
    }

    public void updateScoreText(int playerScore) {
        _scoreText.text = "Score: " + playerScore.ToString();
        if(playerScore == 500) {
            enableTimerText();
        }
    }

    public void updateDifficultyText() {
        _difficultyText.text = "Difficulty: " + _gameManager.getGameDifficulty().ToString() + "%";
    }

    public void updateLivesDisplayImage(int currentLives) {
        if(currentLives < 0) {
            return;
        }
        _livesDisplayImage.sprite = _liveSprites[currentLives];
    }

    void displayTimerText() {
        string timerText = _gameManager.timer();
        if(timerText == null) {
            _gameManager.playerVictorySequence();
        } else {
            _timerText.text = string.Format("Survive for {0} seconds", timerText);
        }
    }

    void enableTimerText() {
        _timerText.enabled = true;
        _audioManager.playSurviveAudioBackground();
    }

    void disableTimerText() {
        _timerText.enabled = false;
    }

    void enableGameConclusionText() {
        if(_gameManager.playerVictory()) {
            _playerVictory.enabled = true;
            StartCoroutine(textFlickeringRoutine("Victory"));
        } else {
            _gameOverText.enabled = true;
            StartCoroutine(textFlickeringRoutine("Lose"));
        }
    }

    IEnumerator textFlickeringRoutine(string conclusion) {
        if(conclusion.Equals("Victory")) {
            while(true) {
                yield return new WaitForSeconds(0.5f);
                _playerVictory.text = "";
                yield return new WaitForSeconds(0.5f);
                _playerVictory.text = "YOU WIN!";
            }
        } else if(conclusion.Equals("Lose")) {
            while(true) {
                yield return new WaitForSeconds(0.5f);
                _gameOverText.text = "";
                yield return new WaitForSeconds(0.5f);
                _gameOverText.text = "GAME OVER";
            }
        }
    }

    void enableRestartText() {
        _restartText.enabled = true;
    }

    void enableQuitText() {
        _quitText.enabled = true;
    }

    public void gameOverSequence() {
        disableTimerText();
        _audioManager.stopSurviveAudioBackground();
        enableGameConclusionText();
        enableRestartText();
        enableQuitText();
        _gameManager.gameOver();
    }
}
