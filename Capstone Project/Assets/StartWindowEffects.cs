using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StartWindowEffects : MonoBehaviour {

    [SerializeField]
    private Transform _playerSpawnPos;
    [SerializeField]
    private GameObject _player;
    [SerializeField]
    private SOEffects _SOEffectHandler;
    [SerializeField]
    private SOEnemyBodyParts _SOBodyPartsHandler;
    private float _timer = 0.0f;
    private float _deployTime = 0.0f;

    private List<GameObject> _bodyParts = new List<GameObject>();
    private float _resetTimer = 0.0f;
    private float _resetTime = 60.0f;

    private bool _start = false;

    private void OnEnable() {
        Reset();
    }

    private void Update() {

        if (_start && Time.time - _timer > _deployTime) {
            DeployBodyParts();
            _timer = Time.time;
            _deployTime = Random.Range(0.5f, 2.0f);
        }

        if (Time.time - _resetTimer > _resetTime) {
            Reset();
        }
    }

    private void DeployBodyParts() {

        GameObject effectInstance = _SOEffectHandler.PlayEffect(EffectEnums.SwarmerDeathExplosion, transform.position);
        effectInstance.transform.localScale = new Vector3(0.85f, 0.85f, 1.0f);
        effectInstance.GetComponent<AudioSource>().pitch = Random.Range(0.75f, 1.5f);

        for (int i = 0; i < _SOBodyPartsHandler.bodyParts.Length; ++i) {
            GameObject instance = Instantiate(_SOBodyPartsHandler.bodyParts[i], transform.position, Quaternion.identity) as GameObject;
            _bodyParts.Add(instance);
            instance.GetComponent<Rigidbody2D>().gravityScale = 2.0f;
            instance.transform.localScale = new Vector3(0.2f, 0.2f, 1.0f);
            instance.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-15.0f, -5.0f), Random.Range(2.0f, 10.0f)), ForceMode2D.Impulse);
            instance.GetComponent<Rigidbody2D>().AddTorque(Random.Range(-500.0f, 500.0f));
        }
    }

    public void SetStart() {
        _start = true;
        _timer = Time.time;
    }

    private void Reset() {
        _start = false;
        _deployTime = Random.Range(0.5f, 2.0f);
        _resetTimer = Time.time;
        foreach (GameObject part in _bodyParts) {
            Destroy(part);
        }
        _bodyParts.Clear();
        Instantiate(_player, _playerSpawnPos.position, Quaternion.identity);
    }
}
