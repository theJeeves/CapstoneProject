using UnityEngine;
using System.Collections;

public class CamaraShift : MonoBehaviour {

    private int gameWindow;
    public GameObject cameraPosition;
    private Rigidbody2D _body2D;

    private Vector3 cameraMove = new Vector3(420, 0, 0);

    void Awake()
    {
        _body2D = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>(); 
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player") {
            if (_body2D.velocity.x > 0) {
                cameraPosition.transform.position += cameraMove;
            }
            else if (_body2D.velocity.x < 0) {
                cameraPosition.transform.position -= cameraMove;
            }
        }        
    }

}
