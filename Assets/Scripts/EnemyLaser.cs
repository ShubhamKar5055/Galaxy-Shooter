using UnityEngine;

public class EnemyLaser : MonoBehaviour {
    [SerializeField]
    private float _speed = 8.0f;

    // Update is called once per frame
    void Update() {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
        if(transform.position.y < -5.5f) {
            if(transform.parent != null) {
                Destroy(transform.parent.gameObject);
            } else {
                Destroy(this.gameObject);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Player") {
            Player player = other.transform.GetComponent<Player>();
            if(player != null) {
                player.damage();
                Destroy(this.gameObject);
            }
        } else if(other.tag == "Laser") {
            Destroy(other.gameObject);
            Destroy(this.gameObject);
        }
    }
}
