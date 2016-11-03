using UnityEngine;
using System.Collections;

public class ShotgunBlast : MonoBehaviour {

    [SerializeField]
    private Vector2 _widths;
    [SerializeField]
    private float _growthDuration;
    [SerializeField]
    private Vector2 _magnitudeRange;
    [SerializeField]
    private Vector2 _directionRange;


    [SerializeField]
    private float _dissipationRate;

    private Vector2 _barrel;
    private Vector2 _target;
    private Vector3 _direction;

    private DigitalRuby.LightningBolt.LightningBoltScript[] _lightning;
    private float[] _magnitudes = new float[5];
    private Vector3[] _directions = new Vector3[5];

    private void Start() {

        _barrel = GameObject.FindGameObjectWithTag("Barrel").transform.position;
        _target = GameObject.FindGameObjectWithTag("Target").transform.position;

        _direction = (_target - _barrel).normalized;

        Debug.Log("direction = " + _direction);


        //Get all the prefab lightning bolts into one array
        _lightning = GetComponentsInChildren<DigitalRuby.LightningBolt.LightningBoltScript>();

        // Set the widths for all each bolt of lightning
        foreach (DigitalRuby.LightningBolt.LightningBoltScript bolt in _lightning) {

            bolt.Width = _widths;
        }

        //Generate random lengths and heights for each bolt to make it more chaotic
        for (int i = 0; i < _lightning.Length; ++i) {

            if (i == _lightning.Length - 1) {
                _magnitudes[i] = _magnitudeRange.y;
                _directions[i] = _direction;
            }
            else {
                _magnitudes[i] = Random.Range(_magnitudeRange.x, _magnitudeRange.y);
                _directions[i].x += Random.Range(_directionRange.x, _directionRange.y);
                _directions[i].y += Random.Range(_directionRange.x, _directionRange.y);

                _directions[i] += _direction;
            }
        }

        StartCoroutine(Extend());
    }
    

    private IEnumerator Extend() {

        while (_lightning[_lightning.Length - 1].EndPosition.magnitude <= _magnitudes[_lightning.Length - 1]) {

            int i = 0;
            foreach (DigitalRuby.LightningBolt.LightningBoltScript bolt in _lightning) {

                bolt.EndPosition = Vector2.MoveTowards(bolt.EndPosition, (bolt.EndPosition + (_directions[i++] * 10.0f)), _growthDuration * Time.deltaTime);
            }

            yield return 0;
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
