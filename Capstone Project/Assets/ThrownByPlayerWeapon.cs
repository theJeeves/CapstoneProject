using UnityEngine;
using System.Collections;

public class ThrownByPlayerWeapon : MonoBehaviour {

    [SerializeField]
    private float _xForce;
    [SerializeField]
    private float _yForce;

    private Rigidbody2D _body2d;

    private void Awake() {
        _body2d = GetComponent<Rigidbody2D>();
    }

    private void OnEnable() {
        ShotgunBlast.PushEnemy += PushThisGameObject;
    }

    private void OnDisable() {
        ShotgunBlast.PushEnemy -= PushThisGameObject;
    }

    private void PushThisGameObject(GameObject whatGotHit, Vector3 direction) {

        if (whatGotHit == gameObject) {
            if (direction.x == 1.0f && direction.y == 0.0f) {
                Debug.Log(direction.x * _xForce);
                _body2d.AddForce(new Vector2(_xForce * direction.x, _yForce * direction.x), ForceMode2D.Impulse);
            }
        }
    }
}
