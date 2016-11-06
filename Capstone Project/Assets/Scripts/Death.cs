using UnityEngine;
using System.Collections;

public class Death : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    if(transform.position.y < -200)
        {
            transform.position = new Vector3(0, -49, 0);
        }
	}
}
