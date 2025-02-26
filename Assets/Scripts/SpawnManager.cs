using System.Collections;
using UnityEngine;

public class SpawnManager : MonoBehaviour {
    [SerializeField]
    private GameObject _enemyPrefab;
    [SerializeField]
    private GameObject _enemyContainer;
    [SerializeField]
    private GameObject[] _powerUpPrefabs;
    [SerializeField]
    private GameObject _asteroidPrefab;
    private GameManager _gameManager;
    private bool _hasSpawningStarted = false;
    private bool _stopSpawning = false;

    // Start is called before the first frame update
    void Start() {
        _gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        if(_gameManager == null) {
            Debug.LogError("The Game Manager is Null.");
        }
    }

    public bool hasSpawningStarted() {
        return _hasSpawningStarted;
    }

    public void startSpawning() {
        StartCoroutine(spawnEnemyRoutine()); 
        StartCoroutine(spawnPowerUpRoutine());
        StartCoroutine(spawnAsteroidRoutine());
        _hasSpawningStarted = true;
    }

    IEnumerator spawnEnemyRoutine() {
        yield return new WaitForSeconds(2.0f);

        while(!_stopSpawning) {
            Vector3 posToSpawn = new Vector3(Random.Range(-8.0f, 8.0f), 7.0f, 0);
            GameObject newEnemy = Instantiate(_enemyPrefab, posToSpawn, Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
            // Game Difficulty is proportionals to enemy spawning rate
            yield return new WaitForSeconds(300.0f/_gameManager.getGameDifficulty());
        }
    }

    IEnumerator spawnPowerUpRoutine() {
        yield return new WaitForSeconds(2.0f);

        while(!_stopSpawning) {
            GameObject powerUpPrefab = _powerUpPrefabs[Random.Range(0,3)];
            Vector3 posToSpawn = new Vector3(Random.Range(-8.0f, 8.0f), 7.0f, 0);
            Instantiate(powerUpPrefab, posToSpawn, Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(3.0f, 8.0f));
        }
    }

     IEnumerator spawnAsteroidRoutine() {
        yield return new WaitForSeconds(20.0f);

        while(!_stopSpawning) {
            Vector3 posToSpawn = new Vector3(Random.Range(-8.0f, 8.0f), 8.0f, 0);
            Instantiate(_asteroidPrefab, posToSpawn, Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(20.0f, 30.0f));
        }
    }

    public void onPlayerDeathOrVictory() {
        _stopSpawning = true;
    }
}
