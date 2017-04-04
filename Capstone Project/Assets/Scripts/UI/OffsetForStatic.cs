using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class OffsetForStatic : MonoBehaviour {

    [SerializeField]
    private float _offsetSpeed = 0.0f;

    private Image _image;

    private void OnEnable() {
        _image = GetComponent<Image>();
        _image.material.mainTextureOffset = new Vector2(0.0f, 0.0f);
    }

    private void Update() {

        _image.material.mainTextureOffset = new Vector2(0.0f, _image.material.mainTextureOffset.y + _offsetSpeed);
    }
}
