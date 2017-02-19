using UnityEngine;
using System.Collections;

public class RespawnPlayer : MonoBehaviour {

    [SerializeField]
    private SOCheckpoint _SOCheckpointHandler;
    [SerializeField]
    private SOEffects _SOEffectHandler;
    [SerializeField]
    private SOWeaponManager _SOWeaponManager;

    private GameObject _otherGo;
    private bool _respawned = false;

    private void Update() {
        
        if (_respawned) {

            _otherGo.GetComponent<Rigidbody2D>().velocity = new Vector3(0.0f, 0.0f, 0.0f);
            _otherGo.GetComponent<Rigidbody2D>().gravityScale = 0.0f;
            _otherGo.transform.position = _SOCheckpointHandler.checkpointPosition;

            if (Camera.main.WorldToViewportPoint(_SOCheckpointHandler.checkpointPosition).x > 0.0f &&
                Camera.main.WorldToViewportPoint(_SOCheckpointHandler.checkpointPosition).x < 1.0f) {

                _otherGo.GetComponent<Rigidbody2D>().gravityScale = 40.0f;
                _SOEffectHandler.PlayEffect(EffectEnums.PlayerRespawn, _SOCheckpointHandler.checkpointPosition);
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
