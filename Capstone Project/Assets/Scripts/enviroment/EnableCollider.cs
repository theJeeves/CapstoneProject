using UnityEngine;

public class EnableCollider : MonoBehaviour {

    #region Constants
    private const float MIN_GRAVITY_SCALE = 35.0f;
    private const float MAX_GRAVITY_SCALE = 45.0f;

    #endregion Constants

    #region Fields

    #region Public Fields
    public bool mainMenu = false;

    #endregion Public Fields

    #region Private Fields
    private float m_delayTime = 0.25f;

    #endregion Private Fields

    #endregion Fields

    #region Private Initializers
    private void Start() {

        if (!mainMenu) 
        {
            GetComponent<Rigidbody2D>().gravityScale = Random.Range(MIN_GRAVITY_SCALE, MAX_GRAVITY_SCALE);
        }
    }

    #endregion Private Initializers

    #region Private Methods
    // Update is called once per frame
    private void Update () {
	
        if (TimeTools.TimeExpired(ref m_delayTime)) {
            GetComponent<PolygonCollider2D>().enabled = true;
        }
	}

    #endregion Private Methods
}
