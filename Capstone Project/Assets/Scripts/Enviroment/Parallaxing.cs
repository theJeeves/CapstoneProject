using UnityEngine;
using System.Collections;

public class Parallaxing : MonoBehaviour {

    [SerializeField]
    private Material[] _grounds;               // Array of the backgrounds/foregrounds to receive the parallaxing effect.
    [SerializeField]
    private float _smoothing;                   // How smooth the parallax is going to look.

    //private float[] _parallaxScales;            // Camera to background movement

    private Camera _mainCam;
    private Vector3 _previousCamPos;

    private SpriteRenderer _renderer;

    private void Awake() {
        _mainCam = Camera.main;
    }

    private void Start () {
        _previousCamPos = _mainCam.transform.position;
	}

    private void Update () {
	
        for (int i = 0; i < _grounds.Length; ++i) {
            Vector2 parallax = (_previousCamPos - _mainCam.transform.position);
            Vector2 bgTargetOffset  = _grounds[i].mainTextureOffset + parallax * -0.25f;

            //Vector2 bgTargetOffset = new Vector2(bgTargetOffsetX, _grounds[i].mainTextureOffset.y);

            _grounds[i].mainTextureOffset = Vector2.Lerp(_grounds[i].mainTextureOffset, bgTargetOffset, _smoothing * Time.deltaTime);
        }

        _previousCamPos = _mainCam.transform.position;
	}
}
