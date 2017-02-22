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

    private InputManager _inputManager;

    private void OnEnable() {
        _inputManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<InputManager>();
    }

    private void Update() {
        
        if (_respawned) {

            // If the player has been respawned: 1. set the velocity to zero so there is no movement when the player is moved.
            // 2. set the gravityScale to zero for the same reason.
            // 3. Move the player to the last known checkpoint position
            _otherGo.GetComponent<Rigidbody2D>().velocity = new Vector3(0.0f, 0.0f, 0.0f);
            _otherGo.GetComponent<Rigidbody2D>().gravityScale = 0.0f;
            _otherGo.transform.position = _SOCheckpointHandler.checkpointPosition;

            // Wait unit the player is within the frame of the screen
            if (Camera.main.WorldToViewportPoint(_SOCheckpointHandler.checkpointPosition).x > 0.1f &&
                Camera.main.WorldToViewportPoint(_SOCheckpointHandler.checkpointPosition).x < 0.9f) {

                // Set all the settings back to their defaults and play the respawn effect. Also reload the player's weapons
                // Lastly, pause input from the player to ensure they do not go flying off to their death immediately after they respawn
                _otherGo.GetComponent<Rigidbody2D>().gravityScale = 40.0f;
                _SOEffectHandler.PlayEffect(EffectEnums.PlayerRespawn, _SOCheckpointHandler.checkpointPosition);
                _SOWeaponManager.Reload();
                _inputManager.PauseInput(1.5f);
                _respawned = false;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D otherGO) {

        if (otherGO.tag == "Player") {
            _otherGo = otherGO.gameObject;
            _respawned = true;

            otherGO.GetComponent<PlayerMovementManager>().ClearQueue();
        }
    }
}
