using UnityEngine;
using System.Collections;

public class CrystalBullet : AbstractBullet {

    public static event AbstractBulletEvent DamageEnemy;

    [SerializeField]
    protected string _whatToHit;

    private ControllableObject _controller;
    private Transform _gun;

    private void OnEnable() {
        _controller = GameObject.FindGameObjectWithTag("Player").GetComponent<ControllableObject>();
        _gun = GameObject.FindGameObjectWithTag("Gun").GetComponent<Transform>();
    }

    protected override void Start() {
        base.Start();

        //transform.localEulerAngles = new Vector3(0, 0, Vector3.Angle(Vector2.zero, _direction));
        transform.localEulerAngles = new Vector3(0, 0, (float)_controller.FacingDirection * _gun.localRotation.z + 90);

        _direction += new Vector3(Random.Range(_directionRange.Min, _directionRange.Max), Random.Range(_directionRange.Min, _directionRange.Max), 0);
        Debug.Log(_direction);

        StartCoroutine(Shoot());
    }


    protected override IEnumerator Shoot() {

        while (true) {
            transform.position = Vector2.MoveTowards(transform.position, transform.position + (_direction * _shotSpeed), (_shotSpeed * Time.deltaTime));
            yield return 0;
        }
    }

    private void OnCollisionEnter2D(Collision2D otherObject) {

        if (otherObject.gameObject.tag == _whatToHit && DamageEnemy != null) {
            DamageEnemy(_damageAmount, otherObject.gameObject);
            Destroy(gameObject);
        }
    }
}
