using UnityEngine;
using System.Collections.Generic;

public class CameraShakeManager : MonoBehaviour {

    #region Private Fields
    [SerializeField]
    private float _decreaseRate = 0.0f;

    private Queue<ScreenShakeRequest> _shakeQ = new Queue<ScreenShakeRequest>();
    private ControllableObject _controller;
    private int _key;
    private Vector3 _defaultPosition = new Vector3(0.0f, 0.0f, 0.0f);

    #endregion Private Fields

    #region Public Methods
    /// <summary>
    /// Enqueue a camera screen shake.
    /// </summary>
    /// <param name="request"></param>
    public void Enqueue(ScreenShakeRequest request) {
        _shakeQ.Enqueue(request);
    }

    #endregion Public Methods

    #region Private Methods
    private void Update() {

        // If _controller == null, the find the player game object.
        if (_controller == null) {
            _controller = GameObject.FindGameObjectWithTag(StringConstantUtility.PLAYER_TAG).GetComponent<ControllableObject>();
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

    #endregion Private Methods
}
