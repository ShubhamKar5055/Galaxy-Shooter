using UnityEngine;

public class AudioManager : MonoBehaviour {
    [SerializeField]
    private GameObject _laserAudioGameObject;
    private AudioSource _laserAudio;
    [SerializeField]
    private GameObject _explosionAudioGameObject;
    private AudioSource _explosionAudio;
    [SerializeField]
    private GameObject _powerUpAudioGameObject;
    private AudioSource _powerUpAudio;
    [SerializeField]
    private GameObject _surviveAudioBackgroundGameObject;
    private AudioSource _surviveAudioBackground;

    // Start is called before the first frame update
    void Start() {
        _laserAudio = _laserAudioGameObject.GetComponent<AudioSource>();
        if(_laserAudio == null) {
                Debug.LogError("The Laser Audio Component is NULL.");
        }

        _explosionAudio = _explosionAudioGameObject.GetComponent<AudioSource>();
        if(_explosionAudio == null) {
                Debug.LogError("The Explosion Audio Component is NULL.");
        }

        _powerUpAudio = _powerUpAudioGameObject.GetComponent<AudioSource>();
        if(_powerUpAudio == null) {
                Debug.LogError("The Power Up Audio Component is NULL.");
        }

        _surviveAudioBackground = _surviveAudioBackgroundGameObject.GetComponent<AudioSource>();
        if(_surviveAudioBackground == null) {
                Debug.LogError("The Survive Audio Component is NULL.");
        }
    }
    
    public void playLaserAudio() {
        _laserAudio.Play();
    }

    public void playExplosionAudio() {
        _explosionAudio.Play();
    }

    public void playPowerUpAudio() {
        _powerUpAudio.Play();
    }

    public void playSurviveAudioBackground() {
        _surviveAudioBackground.Play();
    }

    public void stopSurviveAudioBackground() {
        _surviveAudioBackground.Stop();
    }
}
