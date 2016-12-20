using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraShakeManager : MonoBehaviour {

    [SerializeField]
    private float _decreaseRate = 0.0f;

    private Queue<ScreenShakeRequest> _shakeQ = new Queue<ScreenShakeRequest>();

    private Vector3 _defaultPosition = new Vector3(0.0f, 0.0f, 0.0f);

    private void LateUpdate() {
        if (_shakeQ.Count > 0) {
            transform.localPosition = _shakeQ.Dequeue().Shake();
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
