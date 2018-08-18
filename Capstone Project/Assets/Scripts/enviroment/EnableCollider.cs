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
    private XFloat m_Delay = 0.25f;

    #endregion Private Fields

    #endregion Fields

    #region Private Initializers
    private void Start()
    {
        if (!mainMenu) 
        {
            GetComponent<Rigidbody2D>().gravityScale = Random.Range(MIN_GRAVITY_SCALE, MAX_GRAVITY_SCALE);
        }
    }

    #endregion Private Initializers

    #region Private Methods
    // Update is called once per frame
    private void Update ()
    {	
        if (m_Delay != null && m_Delay.IsExpired)
        {
            GetComponent<PolygonCollider2D>().enabled = true;
            m_Delay = null;
        }
	}

    #endregion Private Methods
}
