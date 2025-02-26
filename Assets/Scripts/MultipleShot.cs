using UnityEngine;

public class MultipleShot : MonoBehaviour {
    // Update is called once per frame
    void Update() {
        if(transform.childCount == 0) {
            Destroy(this.gameObject);
        }
    }
}
