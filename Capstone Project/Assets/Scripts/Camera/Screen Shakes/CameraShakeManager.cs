using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraShakeManager : MonoBehaviour {

    [SerializeField]
    private float _decreaseRate = 0.0f;

    private Queue<ScreenShakeRequest> _shakeQ = new Queue<ScreenShakeRequest>();

    private ControllableObject _controller;
    private int _key;
    private Vector3 _defaultPosition = new Vector3(0.0f, 0.0f, 0.0f);

    private void OnEnable() {
        //_controller = GameObject.FindGameObjectWithTag("Player").GetComponent<ControllableObject>();
    }

    private void Update() {
        if (_controller == null) {
            _controller = GameObject.FindGameObjectWithTag("Player").GetComponent<ControllableObject>();
        }
    }

    private void LateUpdate() {

        _key = _controller.CurrentKey;

        while (_shakeQ.Count > 0) {
            transform.localPosition = _shakeQ.Dequeue().Shake(_key);
        }

        if (transform.position != _defaultPosition) {
            transform.localPosition = new Vector3(Mathf.MoveTowards(transform.localPosition.x, 0.0f, _decreaseRate),
                Mathf.MoveTowards(transform.localPosition.y, 0.0f, _decreaseRate), 0.0f);
        }
    }

    public void Enqueue(ScreenShakeRequest request) {
        _shakeQ.Enqueue(request);
    }
}
