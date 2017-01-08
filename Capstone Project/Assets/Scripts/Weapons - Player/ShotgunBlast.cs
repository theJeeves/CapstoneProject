using UnityEngine;
using System.Collections;

public class ShotgunBlast : AbstractBullet {

    public static event AbstractBulletEvent DamageEnemy;
    public static event AbstractBulletEvent2 PushEnemy;

    [SerializeField]
    private GameObject _endOfBarrel;

    [SerializeField]
    private Vector2 _widths;

    [SerializeField]
    private Range _magnitudeRange;

    [SerializeField]
    private float _dissipationRate;

    [SerializeField]
    protected LayerMask _whatToHit;

    private DigitalRuby.LightningBolt.LightningBoltScript[] _lightning = new DigitalRuby.LightningBolt.LightningBoltScript[5];
    private float[] _magnitudes = new float[5];
    private Vector3[] _directions = new Vector3[5];

    protected override void Start() {

        base.Start();

        //Get all the prefab lightning bolts into one array
        _lightning = GetComponentsInChildren<DigitalRuby.LightningBolt.LightningBoltScript>();

        foreach (DigitalRuby.LightningBolt.LightningBoltScript bolt in _lightning) {
            bolt.Width = _widths;
        }

        //Generate random lengths and heights for each bolt to make it more chaotic
        GenLengthsAndHeights();

        StartCoroutine(Shoot());

        _endOfBarrel = GameObject.FindGameObjectWithTag("Start");
    }

    private void Update() {
        for (int i = 0; i < _lightning.Length; ++i) {
            _lightning[i].StartPosition = _endOfBarrel.transform.position - transform.position;
        }
    }


    protected override IEnumerator Shoot() {

        // ASK THE GROUP IF THEY LIKE THIS LOOK BETTER
        for (int i = 0; i < _lightning.Length; ++i) {
            _lightning[i].EndPosition += _directions[i] * _magnitudes[i];
        }

        _magnitudes[_lightning.Length - 1] = _lightning[_lightning.Length - 1].EndPosition.magnitude;

        CheckCollisions();

        while (_lightning[_lightning.Length - 1].Width.x > 0 &&
            _lightning[_lightning.Length - 1].Width.y > 0) {

            foreach (DigitalRuby.LightningBolt.LightningBoltScript bolt in _lightning) {

                bolt.Width = Vector2.MoveTowards(bolt.Width, Vector2.zero, _dissipationRate * Time.deltaTime);
            }
            yield return 0;
        }

        Destroy(gameObject);
    }

    // Generates a random length and a random direction between a specified range.
    // The last lightning bolt will always have the longest length and have the exact aiming direction.
    private void GenLengthsAndHeights() {
        for (int i = 0; i < _lightning.Length; ++i) {

            if (i != _lightning.Length - 1) {

                _magnitudes[i] = Random.Range(_magnitudeRange.Min, _magnitudeRange.Max);
                _directions[i].x += Random.Range(_directionRange.Min, _directionRange.Max);
                _directions[i].y += Random.Range(_directionRange.Min, _directionRange.Max);
                _directions[i] += _direction;
            }
            else {
                _magnitudes[i] = _magnitudeRange.Max;
                _directions[i] = _direction;
            }
        }
    }

    // User Raycasting on each lightning bolt to determine if a collision has occured.
    private void CheckCollisions() {

        for (int i = 0; i < _lightning.Length; ++i) {
            
            RaycastHit2D hit = Physics2D.Raycast(_start, _directions[i], _magnitudes[i], _whatToHit);

            if (hit.collider != null && DamageEnemy != null) {
                Debug.DrawLine(_start, _start + _lightning[i].EndPosition, Color.white);
                if (PushEnemy != null) {
                    PushEnemy(hit.collider.gameObject, _direction);
                }
                if (DamageEnemy != null) {
                    DamageEnemy(_damageAmount, hit.collider.gameObject);
                }
            }
        }
    }
}
