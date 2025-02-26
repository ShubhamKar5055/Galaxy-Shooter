using UnityEngine;

public class PowerUp : MonoBehaviour {
    [SerializeField]
    private float _speed = 3.0f;
    [SerializeField]
    private int _powerUpId; // 0: Triple Shot, 1: Speed Boost, 2: Shield
    private AudioManager _audioManager;

    // Start is called before the first frame update
    void Start() {
        _audioManager = GameObject.Find("Audio Manager").GetComponent<AudioManager>();
        if(_audioManager == null) {
            Debug.LogError("The Audio Manager is NULL.");
        }
    }

    // Update is called once per frame
    void Update() {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
        if(transform.position.y < -5.5f) {
            Destroy(this.gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Player") {
            Player player = other.transform.GetComponent<Player>();
            if(player != null) {
                switch(_powerUpId) {
                    case 0: player.tripleShotActive();
                    break;
                    case 1: player.speedBoost();
                    break;
                    case 2: player.shieldActive();
                    break;
                    default: Debug.Log("Unidentifiable Power-Up ID");
                    break;
                }
                _audioManager.playPowerUpAudio();
                Destroy(this.gameObject);
            }
        }
    }
}
