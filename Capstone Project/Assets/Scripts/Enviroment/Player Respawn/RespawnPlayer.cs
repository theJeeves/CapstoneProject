using UnityEngine;
using System.Collections;

public class RespawnPlayer : MonoBehaviour {

    [SerializeField]
    private SORespawn _respawnContainer;
    [SerializeField]
    private SOEffects _SOEffect;
    [SerializeField]
    private SOWeaponManager _SOWeaponManager;
    [SerializeField]
    private SOAudio _SOAudioManager;

    private AudioSource _audioSource;

    private GameObject _otherGo;
    private bool _respawned = false;

    private void OnEnable() {
        _audioSource = GetComponent<AudioSource>();
    }

    private void Update() {
        
        if (_respawned) {

            _otherGo.GetComponent<Rigidbody2D>().velocity = new Vector3(0.0f, 0.0f, 0.0f);
            _otherGo.GetComponent<Rigidbody2D>().gravityScale = 0.0f;
            _otherGo.transform.position = _respawnContainer.respawnPos;
            _audioSource.transform.position = _respawnContainer.respawnPos;

            if (Camera.main.WorldToViewportPoint(_respawnContainer.respawnPos).x > 0.0f &&
                Camera.main.WorldToViewportPoint(_respawnContainer.respawnPos).x < 1.0f) {

                _otherGo.GetComponent<Rigidbody2D>().gravityScale = 40.0f;
                _SOEffect.PlayEffect(EffectEnum.PlayerRespawn, _respawnContainer.respawnPos);
                _SOAudioManager.Play(_audioSource);
                _SOWeaponManager.Reload();
                _respawned = false;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D otherGO) {

        if (otherGO.tag == "Player") {
            _otherGo = otherGO.gameObject;
            _respawned = true;
        }
    }
}
