using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour {
    [SerializeField]
    private float _speed = 4.0f;
    [SerializeField]
    private int _speedMultiplier = 1;
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private float _fireRate = 0.15f;
    private float _canFire = 0.0f ;
    [SerializeField]
    private int _lives = 3;
    private SpawnManager _spawnManager;
    [SerializeField]
    private GameObject _tripleShotPrefab;
    private bool _isTripleShotActive = false;
    private Coroutine _tripleShotPowerDownRoutine;
    private bool _isShieldActive = false;
    private Coroutine _shieldPowerDownRoutine;
    [SerializeField]
    private GameObject _shieldVisualizer;
    [SerializeField]
    private GameObject[] _playerHurtVisualizers;
    private bool _isLeftEngineDamaged = false, _isRightEngineDamaged = false;
    [SerializeField]
    private int _score = 0;
    private UIManager _uiManager;
    private AudioManager _audioManager;
    private Animator _anim;
    private GameManager _gameManager;
    
    // Start is called before the first frame update
    void Start() {
        // Starting position for the Player
        transform.position = new Vector3(0, 0, 0); 

        _spawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();
        if(_spawnManager == null) {
            Debug.LogError("The Spawn Manager is NULL.");
        }

        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        if(_uiManager == null) {
            Debug.LogError("The UI Manager is NULL.");
        }

        _audioManager = GameObject.Find("Audio Manager").GetComponent<AudioManager>();
        if(_audioManager == null) {
            Debug.LogError("The Audio Manager is NULL.");
        }

        _anim = GetComponent<Animator>();
        if(_anim == null) {
            Debug.LogError("The Animator is NULL.");
        }

        _gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        if(_gameManager == null) {
            Debug.LogError("The Game Manager is NULL.");
        }
    }

    // Update is called once per frame
    void Update() {
        handleMovement();

        if(Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire) {
            instantiateLaser();
        }
    }

    void handleMovement() {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);

        handleMovementAnimation(horizontalInput);

        transform.Translate(direction * (_speed * _speedMultiplier) * Time.deltaTime);

        if(transform.position.x > 11.3f) {
            transform.position = new Vector3(-11.3f, transform.position.y, 0);
        } else if(transform.position.x < -11.3f) {
            transform.position = new Vector3(11.3f, transform.position.y, 0);
        }

        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -3.8f, 2.5f), 0);
    }

    void handleMovementAnimation(float horizontalInput) {
        if (horizontalInput < 0) {
            _anim.SetBool("isMovingLeft", true);
            _anim.SetBool("isMovingRight", false);
        } else if (horizontalInput > 0) {
            _anim.SetBool("isMovingLeft", false);
            _anim.SetBool("isMovingRight", true);
        } else {
            _anim.SetBool("isMovingLeft", false);
            _anim.SetBool("isMovingRight", false);
        }
    }

    void instantiateLaser() {
        if(_isTripleShotActive) {
            Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
        } else {
            Vector3 position = transform.position + new Vector3(0, 1.05f, 0);
            Instantiate(_laserPrefab, position, Quaternion.identity);
        }   

        _audioManager.playLaserAudio();

        _canFire = Time.time + _fireRate;
    }

    void playerDamageAnimation() {
        if(_lives <= 0){
            return;
        } else if(_isLeftEngineDamaged == false && _isRightEngineDamaged == false) {
            int index = Random.Range(0, 2);
            _playerHurtVisualizers[index].SetActive(true);
            if(index == 0) {
                _isLeftEngineDamaged = true;
            } else {
                _isRightEngineDamaged = true;
            }
        } else if (_isLeftEngineDamaged == false) {
            _playerHurtVisualizers[0].SetActive(true);
            _isLeftEngineDamaged = true;
        } else if (_isRightEngineDamaged == false) {
            _playerHurtVisualizers[1].SetActive(true);
            _isRightEngineDamaged = true;
        }
    }

    public void damage() {
        if(_isShieldActive) {
            _isShieldActive = false;
            _shieldVisualizer.SetActive(false);
            StopCoroutine(_shieldPowerDownRoutine);
            return;
        } else if(_gameManager.playerVictory()) {
            return;
        }
        
        _lives--;

        playerDamageAnimation();

        _uiManager.updateLivesDisplayImage(_lives);

        if(_lives == 0) {
            _spawnManager.onPlayerDeathOrVictory();
            _uiManager.gameOverSequence();
            Destroy(this.gameObject);
        }
    }

    public void tripleShotActive() {
        if(_isTripleShotActive) {
            StopCoroutine(_tripleShotPowerDownRoutine);
        } else {
            _isTripleShotActive = true;
        }

        _tripleShotPowerDownRoutine = StartCoroutine(tripleShotPowerDownRoutine());
    }

    IEnumerator tripleShotPowerDownRoutine() {
        yield return new WaitForSeconds(5.0f);
        _isTripleShotActive = false;
    }

    public void speedBoost() {
        _speedMultiplier++;
        StartCoroutine(speedDownRoutine());
    }

    IEnumerator speedDownRoutine() {
        yield return new WaitForSeconds(5.0f);
        _speedMultiplier--;
    }

    public void shieldActive() {
        if(_isShieldActive) {
            StopCoroutine(_shieldPowerDownRoutine);
        } else {
            _isShieldActive = true;
            _shieldVisualizer.SetActive(true);
        }

        _shieldPowerDownRoutine = StartCoroutine(shieldPowerDownRoutine());
    }

    IEnumerator shieldPowerDownRoutine() {
        yield return new WaitForSeconds(10.0f);
        _isShieldActive = false;
        _shieldVisualizer.SetActive(false);
    }

    public void updateScore(int points) {
        _score += points;
        _uiManager.updateScoreText(_score);
        if((_score % 100) == 0 && _score != 0 && _score <= 500) {
            _gameManager.incrementGameDifficulty();
        }
    }
}
