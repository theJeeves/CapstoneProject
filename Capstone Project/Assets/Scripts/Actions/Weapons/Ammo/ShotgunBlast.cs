using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShotgunBlast : MonoBehaviour {

    public delegate void ShotgunBlastEvent(float damage);
    public static event ShotgunBlastEvent DamageEnemy;

    [SerializeField]
    private float _damageAmount;
    [SerializeField]
    private LayerMask enemies;

    [SerializeField]
    private Vector2 _widths;
    [SerializeField]
    private float _growthDuration;

    [System.Serializable]
    private struct Range {

        [SerializeField]
        private float _min;
        public float Min {
            get { return _min; }
            set { _min = value; }
        }

        [SerializeField]
        private float _max;
        public float Max {
            get { return _max; }
            set { _max = value; }
        }
    }

    [SerializeField]
    private Range _magnitudeRange;
    [SerializeField]
    private Range _directionRange;

[SerializeField]
    private float _dissipationRate;

    private Vector3 _start;
    private Vector3 _end;
    private Vector3 _direction;

    private DigitalRuby.LightningBolt.LightningBoltScript[] _lightning = new DigitalRuby.LightningBolt.LightningBoltScript[5];
    private float[] _magnitudes = new float[5];
    private Vector3[] _directions = new Vector3[5];

    private void Start() {

        _start = GameObject.FindGameObjectWithTag("Barrel").transform.position;
        _end = GameObject.FindGameObjectWithTag("Direction").transform.position;

        _direction = (_end - _start).normalized;


        //Get all the prefab lightning bolts into one array
        _lightning = GetComponentsInChildren<DigitalRuby.LightningBolt.LightningBoltScript>();

        foreach (DigitalRuby.LightningBolt.LightningBoltScript bolt in _lightning) {
            bolt.Width = _widths;
        }

        //Generate random lengths and heights for each bolt to make it more chaotic
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

        StartCoroutine(Extend());
    }
    

    private IEnumerator Extend() {

        while (_lightning[_lightning.Length - 1].EndPosition.magnitude < _magnitudes[_lightning.Length - 1]) {

            for (int i = 0; i < _lightning.Length; ++i) {

                if (_lightning[i].EndPosition.magnitude < _magnitudes[i]) {
                    _lightning[i].EndPosition = Vector2.MoveTowards(_lightning[i].EndPosition, _lightning[i].EndPosition + (_directions[i] * 10.0f), _growthDuration * Time.deltaTime);
                }
                else {
                    _magnitudes[i] = _lightning[i].EndPosition.magnitude;
                }
            }
            yield return 0;
        }

        _magnitudes[_lightning.Length - 1] = _lightning[_lightning.Length - 1].EndPosition.magnitude;

        //for (int i = 0; i < 5; ++i) {
        //    Debug.Log(i + 1 + " magnitude: " + _magnitudes[i]);
        //    Debug.Log(i + 1 + " bolt mag: " + _lightning[i].EndPosition.magnitude);
        //}

        for (int i = 0; i < _lightning.Length; ++i) {

            RaycastHit2D hit = Physics2D.Raycast(_start, _directions[i], _magnitudes[i], enemies);

            if (hit.collider != null && DamageEnemy != null) {
                Debug.DrawLine(_start, _start + _lightning[i].EndPosition, Color.white);
                DamageEnemy(_damageAmount);
            }
        }

        while (_lightning[_lightning.Length - 1].Width.x > 0 &&
            _lightning[_lightning.Length - 1].Width.y > 0) {

            foreach (DigitalRuby.LightningBolt.LightningBoltScript bolt in _lightning) {

                bolt.Width = Vector2.MoveTowards(bolt.Width, Vector2.zero, _dissipationRate * Time.deltaTime);
            }
            yield return 0;
        }

        Destroy(gameObject);
    }
}
