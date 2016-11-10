using UnityEngine;
using System.Collections;

public class ShotgunBlast : AbstractBullet {

    public static event AbstractBulletEvent DamageEnemy;

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
    }
    

    protected override IEnumerator Shoot() {

        while (_lightning[_lightning.Length - 1].EndPosition.magnitude < _magnitudes[_lightning.Length - 1]) {

            for (int i = 0; i < _lightning.Length; ++i) {

                if (_lightning[i].EndPosition.magnitude < _magnitudes[i]) {
                    _lightning[i].EndPosition = Vector2.MoveTowards(_lightning[i].EndPosition, _lightning[i].EndPosition + (_directions[i] * _magnitudes[i]), _shotSpeed * Time.deltaTime);
                }
                else {
                    _magnitudes[i] = _lightning[i].EndPosition.magnitude;
                }
            }
            yield return 0;
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
                DamageEnemy(_damageAmount, hit.collider.gameObject);
            }
        }
    }
}
