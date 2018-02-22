using UnityEngine;

public class rotatingObject : MonoBehaviour {

    #region Private Fields
    [SerializeField]
    private int _rotationSpeed;

    #endregion Private Fields

    #region Private Methods
    // Update is called once per frame
    private void Update () {
        transform.Rotate(0, 0, _rotationSpeed * Time.deltaTime);
    }

    #endregion Private Methods
}
