using UnityEngine;
using System.Collections;

public class ShotgunBlast : AbstractBullet {

    [SerializeField]
    private SOEffects _SOEffectHandler;

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
    protected float[] _magnitudes = new float[5];
    private Vector2[] _directions = new Vector2[5];

    public override void Fire(Vector2 direction) {

        base.Fire(direction);

        _endOfBarrel = _direction.x == 0.0f && _direction.y == -1.0f ? GameObject.FindGameObjectWithTag("StartAlt") : GameObject.FindGameObjectWithTag("StartNorm");

        //Get all the prefab lightning bolts into one array
        _lightning = GetComponentsInChildren<DigitalRuby.LightningBolt.LightningBoltScript>();

        foreach (DigitalRuby.LightningBolt.LightningBoltScript bolt in _lightning) {
            bolt.Width = _widths;
        }

        //Generate random lengths and heights for each bolt to make it more chaotic
        GenLengthsAndHeights();

        StartCoroutine(Shoot());
    }

    private void Update() {
        for (int i = 0; i < _lightning.Length; ++i) {
            _lightning[i].StartPosition = _endOfBarrel.transform.position - transform.position;
        }
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


    protected override IEnumerator Shoot() {

        CheckCollisions();

        for (int i = 0; i < _lightning.Length; ++i) {
            float magnitude = _magnitudes[i];
            _lightning[i].EndPosition += new Vector3(_directions[i].x * magnitude, _directions[i].y * magnitude, 0.0f);
        }

        _magnitudes[_lightning.Length - 1] = _lightning[_lightning.Length - 1].EndPosition.magnitude;

        while (_lightning[_lightning.Length - 1].Width.x > 0 &&
            _lightning[_lightning.Length - 1].Width.y > 0) {

            foreach (DigitalRuby.LightningBolt.LightningBoltScript bolt in _lightning) {

                bolt.Width = Vector2.MoveTowards(bolt.Width, Vector2.zero, _dissipationRate * Time.deltaTime);
            }
            yield return 0;
        }

        Destroy(gameObject, 0.8f);
    }

    // User Raycasting on each lightning bolt to determine if a collision has occured.
    private void CheckCollisions() {

        for (int i = 0; i < _lightning.Length; ++i) {

            RaycastHit2D hit = Physics2D.Raycast((Vector2)_endOfBarrel.transform.position, _directions[i], _magnitudes[i], _whatToHit);

            if (hit.collider != null) {
                string hitTagName = hit.collider.gameObject.tag;

                Debug.Log(hitTagName);

                if (hitTagName == "Enemy") {
                    hit.collider.gameObject.GetComponentInParent<EnemyBasicBehaviors>().DecrementHealth(_damageAmount);
                }
                else {
                    _magnitudes[i] = Vector3.Magnitude(hit.point - (Vector2)_endOfBarrel.transform.position);
                }
                if (hitTagName == "SwarmerPodBattery") {
                    hit.collider.gameObject.GetComponentInParent<SwarmPodSpawner>().DestroyPod();
                }
                else if (hitTagName == "AcidBall") {
                    hit.collider.gameObject.GetComponent<AcidBallBehavior>().Splatter();
                }
                else if (hitTagName == "SniperBullet") {
                    hit.collider.gameObject.GetComponent<SniperDealDamage>().Destroy();
                }

                _SOEffectHandler.PlayEffect(EffectEnums.LightningImpact, hit.point);
            }
        }
    }
}
