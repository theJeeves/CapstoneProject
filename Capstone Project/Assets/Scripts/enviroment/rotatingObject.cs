using UnityEngine;
using System.Collections;

public class rotatingObject : MonoBehaviour {
    [SerializeField]
    private int _rotationSpeed;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(0, 0, _rotationSpeed * Time.deltaTime);
    }
}
