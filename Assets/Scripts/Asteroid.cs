using UnityEngine;

public class Asteroid : MonoBehaviour {
    [SerializeField]
    private float _speed = 1.5f;
    [SerializeField]
    private float _angularSpeed = 20.0f;
    [SerializeField]
    private GameObject _explosionAnimationPrefab;
    private SpawnManager _spawnManager;

    // Start is called before the first frame update
    void Start() {
       _spawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();
       if(_spawnManager == null) {
            Debug.LogError("The Spawn Manager is NULL.");
       }
    }

    // Update is called once per frame
    void Update() {
        handleMovement();
    }

    void handleMovement() {
        /* Space.World forces the asteroid's translation to be applied in the global/world space rather than the 
           object's local space which is continuously altering due to it's rotation */
        transform.Translate(Vector3.down * _speed * Time.deltaTime, Space.World);
        transform.Rotate(Vector3.forward * _angularSpeed * Time.deltaTime); // 20 degrees per second
        
        if(transform.position.y < -6.5f) {
            if(!_spawnManager.hasSpawningStarted()) {
                transform.position = new Vector3(Random.Range(-8.0f, 8.0f), 8.0f, 0);
                return;
            }
            Destroy(this.gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Laser") {
            Destroy(other.gameObject);
            Instantiate(_explosionAnimationPrefab, transform.position, Quaternion.identity);
            if(!_spawnManager.hasSpawningStarted()) {
                _spawnManager.startSpawning();
            }
            Destroy(GetComponent<Rigidbody2D>());
            Destroy(this.gameObject, 0.25f);
        } else if(other.tag == "Player") {
            Player player = other.transform.GetComponent<Player>();
            if(player != null) {
                for(int i = 1; i <= 3; i++) {
                    player.damage();
                }
                Instantiate(_explosionAnimationPrefab, transform.position, Quaternion.identity);
                Destroy(GetComponent<Rigidbody2D>());
                Destroy(this.gameObject, 0.25f);
            }
        }
    }
}
