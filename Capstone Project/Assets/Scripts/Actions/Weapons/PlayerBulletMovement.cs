using UnityEngine;
using System.Collections;

public class PlayerBulletMovement : MonoBehaviour {

    [SerializeField]
    private float _speed;

    private Vector2 _barrel;
    private Vector2 _target;
    private Vector3 _direction;

    private void Start()
    {
        _barrel = GameObject.FindGameObjectWithTag("Barrel").transform.position;
        _target = GameObject.FindGameObjectWithTag("Direction").transform.position;

        _direction = (_target - _barrel).normalized;

        transform.localEulerAngles = new Vector3(transform.localRotation.x, transform.localRotation.y, Vector3.Angle(_direction, Vector3.up));

        Debug.Log(Vector2.Angle(_direction, Vector2.up));

        StartCoroutine(movement());
    }

    private IEnumerator movement()
    {
        while (true) {
            transform.position = Vector2.MoveTowards(transform.position, transform.position + (_direction * _speed), (_speed * Time.deltaTime));

            yield return 0;
        }
    }
}
