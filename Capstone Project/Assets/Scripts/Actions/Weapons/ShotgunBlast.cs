using UnityEngine;
using System.Collections;

public class ShotgunBlast : MonoBehaviour {

    [SerializeField]
    private Vector2 _widths;
    [SerializeField]
    private float _growthDuration;
    [SerializeField]
    private Vector4 _endPosition;
    [SerializeField]
    private float _dissipationRate;

    private DigitalRuby.LightningBolt.LightningBoltScript[] _lightning;
    private Vector3[] _endPositions = new Vector3[5];

    private void Start() {

        //Get all the prefab lightning bolts into one array
        _lightning = GetComponentsInChildren<DigitalRuby.LightningBolt.LightningBoltScript>();

        // Set the widths for all each bolt of lightning
        foreach (DigitalRuby.LightningBolt.LightningBoltScript bolt in _lightning) {

            bolt.Width = _widths;
        }

        //Generate random lengths and heights for each bolt to make it more chaotic
        for (int i = 0; i < _lightning.Length; ++i) {
            _endPositions[i] = new Vector2(Random.Range(_endPosition.x, _endPosition.y), Random.Range(_endPosition.z, _endPosition.w));
        }

        StartCoroutine(Extend());
    }
    

    private IEnumerator Extend() {

        //while (_lightning[_lightning.Length-1].EndPosition.x < _endPositions[_lightning.Length - 1].x &&
        //    _lightning[_lightning.Length-1].EndPosition.y < _endPositions[_lightning.Length - 1].y) {

        while(_lightning[_lightning.Length-1].EndPosition != _endPositions[_lightning.Length-1]) {

            int i = 0;
            foreach (DigitalRuby.LightningBolt.LightningBoltScript bolt in _lightning) {

                bolt.EndPosition = Vector2.MoveTowards(bolt.EndPosition, _endPositions[i++], _growthDuration * Time.deltaTime);
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
