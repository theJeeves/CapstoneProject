using UnityEngine;

public class FollowPlayer : MonoBehaviour {

    private GameObject _player;

    // Use this for initialization
    void Awake() {
        _player = GameObject.FindGameObjectWithTag(Tags.PlayerTag);
    }

    private void Update() {
        if (_player == null) {
            _player = GameObject.FindGameObjectWithTag(Tags.PlayerTag);
        }
    }

    private void LateUpdate() {
        if (_player.transform.localScale.x > 0) {
            transform.position = _player.transform.position + new Vector3(0, 50, 0);
        }
        else {
            transform.position = _player.transform.position + new Vector3(0, 50, 0);
        }
    }
}
