using UnityEngine;
using System.Collections;

public class CrystalBullet : AbstractBullet {

    public static event AbstractBulletEvent DamageEnemy;

    [SerializeField]
    protected string _whatToHit;

    private ControllableObject _controller;

    private void OnEnable() {
        _controller = GameObject.FindGameObjectWithTag("Player").GetComponent<ControllableObject>();
    }

    protected override void Start() {
        base.Start();

        transform.localEulerAngles = new Vector3(0, 0, _controller.AimDirection);

        _direction += new Vector3(Random.Range(_directionRange.Min, _directionRange.Max), Random.Range(_directionRange.Min, _directionRange.Max), 0);

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
