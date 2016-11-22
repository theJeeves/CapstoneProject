using UnityEngine;
using System.Collections;

public class DestroyOffScreen : MonoBehaviour {

    Vector3 _GOpos;
	
	// Update is called once per frame
	void Update () {

        _GOpos = Camera.main.WorldToViewportPoint(transform.position);


        if (_GOpos.x < -.15f || _GOpos.x > 1.15f || _GOpos.y < -0.15f || _GOpos.y > 1.15f) {

            Destroy(gameObject);
        }
	}
}
