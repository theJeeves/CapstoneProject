using UnityEngine;

public class DestroyOffScreen : MonoBehaviour {

    [SerializeField]
    private bool _bodyParts = false;

    Vector3 _GOpos;
	
	// Update is called once per frame
	void Update () {

        _GOpos = Camera.main.WorldToViewportPoint(transform.position);


        if (!_bodyParts) {
            if (_GOpos.x < -.26f || _GOpos.x > 1.26f || _GOpos.y < -0.26f || _GOpos.y > 1.26f) {

                Destroy(gameObject);
            }
        }
        else {
            if (_GOpos.x < -.26f || _GOpos.x > 1.26f || _GOpos.y < -0.26f) {

                Destroy(gameObject);
            }
        }
	}
}
