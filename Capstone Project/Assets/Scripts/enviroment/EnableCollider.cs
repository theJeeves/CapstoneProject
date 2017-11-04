using UnityEngine;
using System.Collections;

public class EnableCollider : MonoBehaviour {

    public bool mainMenu = false;

    private float m_delayTime = 0.25f;

    private void Start() {

        if (!mainMenu) { GetComponent<Rigidbody2D>().gravityScale = Random.Range(35.0f, 45.0f); }
    }
	
	// Update is called once per frame
	private void Update () {
	
        if (TimeTools.TimeExpired(ref m_delayTime)) {
            GetComponent<PolygonCollider2D>().enabled = true;
        }
	}
}
