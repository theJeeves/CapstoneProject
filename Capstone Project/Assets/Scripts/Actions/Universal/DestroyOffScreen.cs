using UnityEngine;
using System.Collections;

public class DestroyOffScreen : MonoBehaviour {
	
	// Update is called once per frame
	void Update () {
	    if (Camera.main.WorldToViewportPoint(transform.position).x < 0 ||
            Camera.main.WorldToViewportPoint(transform.position).x > 1 ||
            Camera.main.WorldToViewportPoint(transform.position).y < 0 ||
            Camera.main.WorldToViewportPoint(transform.position).y > 1) {

            Destroy(gameObject);
        }
	}
}
