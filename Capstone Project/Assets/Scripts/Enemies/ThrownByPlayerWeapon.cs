using UnityEngine;

public class ThrownByPlayerWeapon : MonoBehaviour {

    #region Constants
    private const float X_CONSTRAINTS = 1.0f;
    private const float Y_CONSTRAINTS = 0.0f;
    #endregion Constants

    #region Private Fields
    [SerializeField]
    private float _xForce;
    [SerializeField]
    private float _yForce;

    private Rigidbody2D _body2d;

    #endregion Private Fields

    #region Private Initializers
    private void Awake() {
        _body2d = GetComponent<Rigidbody2D>();
    }

    #endregion Private Initializers

    #region Private Methods
    private void PushThisGameObject(GameObject whatGotHit, Vector3 direction) {

        bool allowAction = whatGotHit == gameObject;
        allowAction &= direction.x == X_CONSTRAINTS;
        allowAction &= direction.y == Y_CONSTRAINTS;

        if (allowAction) {
            _body2d.AddForce(new Vector2(_xForce * direction.x, _yForce * direction.x), ForceMode2D.Impulse);
        }
    }

    #endregion Private Methods
}
