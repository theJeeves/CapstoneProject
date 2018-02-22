using UnityEngine;

public class Parallaxing : MonoBehaviour {

    #region Constants
    private const float PARALLAX_OFFSET = -0.25f;

    #endregion Constants

    #region Private Fields
    [SerializeField]
    private Material[] _grounds;               // Array of the backgrounds/foregrounds to receive the parallaxing effect.
    [SerializeField]
    private float _smoothing;                   // How smooth the parallax is going to look.

    private Camera _mainCam;
    private Vector3 _previousCamPos;
    private SpriteRenderer _renderer;

    #endregion Private Fields

    #region Private Initializers
    private void Awake() {
        _mainCam = Camera.main;
    }

    private void Start () {
        _previousCamPos = _mainCam.transform.position;
	}

    #endregion Private Initializers

    #region Private Methods
    private void Update () {
	
        for (int i = 0; i < _grounds.Length; ++i) {
            Vector2 parallax = (_previousCamPos - _mainCam.transform.position);
            Vector2 bgTargetOffset  = _grounds[i].mainTextureOffset + parallax * PARALLAX_OFFSET;

            _grounds[i].mainTextureOffset = Vector2.Lerp(_grounds[i].mainTextureOffset, bgTargetOffset, _smoothing * Time.deltaTime);
        }

        _previousCamPos = _mainCam.transform.position;
	}

    #endregion Private Methods
}
