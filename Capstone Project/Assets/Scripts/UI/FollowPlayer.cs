using UnityEngine;
using System.Collections;

public class FollowPlayer : MonoBehaviour {

    private GameObject _player;

	// Use this for initialization
	void Awake () {
        _player = GameObject.FindGameObjectWithTag("Player");
	}
	

    private void Update() {
        transform.position = _player.transform.position + new Vector3(0, 20, 0);
    }
}
