using UnityEngine;
using System.Collections;

public class testMovement : MonoBehaviour {

    private bool _canMove = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (_canMove) {
            transform.position += Vector3.up;
        }
        else {
        }
	}

    private void OnTriggerEnter2D(Collider2D other) {

        if (other.gameObject.GetComponent<Rigidbody2D>().velocity.y <= 0 && 
            other.gameObject.tag == "Player") {

            _canMove = true;
            other.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(400, 100);
        }
    }

    private void OnTriggerExit2D(Collider2D other) {

        if (other.gameObject.tag == "Player") {
            _canMove = false;
        }
    }
}
