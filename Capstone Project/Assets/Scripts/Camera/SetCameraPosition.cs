using UnityEngine;

public class SetCameraPosition : MonoBehaviour {

    #region Private Fields
    [SerializeField]
    private Vector3 _newCameraPos;

    #endregion Private Fields

    #region Private Methods
    private void OnTriggerEnter2D(Collider2D otherGO) {

        if (otherGO.tag == Tags.PlayerTag)
        {
            GameObject.FindGameObjectWithTag(Tags.SmartCameraTag).transform.position = new Vector3(_newCameraPos.x, _newCameraPos.y, _newCameraPos.z);
        }
    }

    #endregion Private Methods
}
