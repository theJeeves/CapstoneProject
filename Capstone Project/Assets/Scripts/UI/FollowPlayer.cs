using UnityEngine;
using System.Collections;

public class FollowPlayer : MonoBehaviour {

    private GameObject _player;

    // Use this for initialization
    void Awake() {
        _player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update() {
        if (_player.transform.localScale.x > 0) {
            transform.position = _player.transform.position + new Vector3(0, 25, 0);
        }
        else {
            transform.position = _player.transform.position + new Vector3(10, 25, 0);
        }
    }
}
