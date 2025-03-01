using UnityEngine;

public class Laser : MonoBehaviour {
    [SerializeField]
    private float _speed = 8.0f;

    // Update is called once per frame
    void Update() {
        transform.Translate(Vector3.up * _speed * Time.deltaTime);
        if(transform.position.y > 7.5f) {
            if(transform.parent != null) {
                Destroy(transform.parent.gameObject);
            } else {
                Destroy(this.gameObject);
            }
        }
    }
}
