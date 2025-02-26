using UnityEngine;

public class Explosion : MonoBehaviour {
    private AudioManager _audioManager;
    // Start is called before the first frame update
    void Start() {
        _audioManager = GameObject.Find("Audio Manager").GetComponent<AudioManager>();
        if(_audioManager == null) {
            Debug.LogError("The Audio Manager is NULL.");
        } else {
            _audioManager.playExplosionAudio();
        }
        
        Destroy(this.gameObject, 2.8f); // Here "this" refers to the Explosion Animation Prefab instance for the Asteroid
    }
}
