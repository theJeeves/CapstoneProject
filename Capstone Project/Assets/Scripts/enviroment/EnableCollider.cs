using UnityEngine;
using System.Collections;

public class EnableCollider : MonoBehaviour {

    private float _delayTime = 0.25f;
    private float _timer;

    private void Start() {
        _timer = Time.time;
        GetComponent<Rigidbody2D>().gravityScale = Random.Range(35.0f, 45.0f);
    }
	
	// Update is called once per frame
	private void Update () {
	
        if (Time.time - _timer > _delayTime) {
            GetComponent<PolygonCollider2D>().enabled = true;
        }
	}
}
