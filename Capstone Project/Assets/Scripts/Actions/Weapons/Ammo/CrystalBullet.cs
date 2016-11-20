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

        // Angle the crystal according the the angle of the gun's direction
        transform.localEulerAngles = new Vector3(0, 0, _controller.AimDirection);

        // Have the crystals fire out from the MG at slightly off directions to give a more chaotic feel.
        _direction += new Vector3(Random.Range(_directionRange.Min, _directionRange.Max), Random.Range(_directionRange.Min, _directionRange.Max), 0);

        StartCoroutine(Shoot());
    }


    protected override IEnumerator Shoot() {

        // While the bullet is alive, move in the direction in which it was shot.
        while (true) {
            transform.position = Vector2.MoveTowards(transform.position, transform.position + (_direction * _shotSpeed), (_shotSpeed * Time.deltaTime));
            yield return 0;
        }
    }

    // If it collides with an enemy, destroy itself.
    private void OnTriggerEnter2D(Collider2D otherObject) {

        if (otherObject.gameObject.tag == _whatToHit && DamageEnemy != null) {
            DamageEnemy(_damageAmount, otherObject.gameObject);
            Destroy(gameObject);
        }
    }
}
