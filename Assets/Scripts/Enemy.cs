using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour {
    [SerializeField]
    private float _speed = 4.0f;
    [SerializeField]
    private GameObject[] _enemyLaserPrefabs; 
    private Coroutine _fireLaserRoutine;
    private Player _player;
    private Animator _anim;
    private AudioManager _audioManager;

    // Start is called before the first frame update
    void Start() {
        _player = GameObject.Find("Player").GetComponent<Player>();
        if(_player == null) {
            Debug.LogError("The Player is NULL.");
        }

        _anim = GetComponent<Animator>();
        if(_anim == null) {
            Debug.LogError("The Animator is NULL.");
        }

        _audioManager = GameObject.Find("Audio Manager").GetComponent<AudioManager>();
        if(_audioManager == null) {
            Debug.LogError("The Audio Manager is NULL.");
        }

        _fireLaserRoutine = StartCoroutine(fireLaserRoutine());
    }

    // Update is called once per frame
    void Update() {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
        if(transform.position.y < -5.5f) {
            float randomX = UnityEngine.Random.Range(-8.0f, 8.0f);
            transform.position = new Vector3(randomX, 7.0f, 0);
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Player") {
            Player player = other.transform.GetComponent<Player>();
            if(player != null) {
                player.damage();
                _speed = 0;
                _anim.SetTrigger("onEnemyDeath");
                _audioManager.playExplosionAudio();
                StopCoroutine(_fireLaserRoutine);
                Destroy(GetComponent<Rigidbody2D>());;
                Destroy(this.gameObject, 2.8f);
            }
        } else if(other.tag == "Laser") {
            Destroy(other.gameObject);

            if(_player != null) {
                _player.updateScore(10);
            }

            _speed = 0;
            _anim.SetTrigger("onEnemyDeath");
            _audioManager.playExplosionAudio();
            StopCoroutine(_fireLaserRoutine);
            Destroy(GetComponent<Rigidbody2D>());
            Destroy(this.gameObject, 2.8f);
        }
    }

    IEnumerator fireLaserRoutine() {
        while(true) {
            int index = Random.Range(0, 2); // index: 0 -> Single Shot, 1 -> Double Shot
            Vector3 position;
            if(index == 0) {
                position = transform.position - new Vector3(0, 0.95f, 0); 
            } else {
                position = transform.position; // index = 1
            }
            Instantiate(_enemyLaserPrefabs[index], position, Quaternion.identity);
            _audioManager.playLaserAudio();
            yield return new WaitForSeconds(5.0f);
        } 
    }
}
