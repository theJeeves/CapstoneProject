using UnityEngine;

public class LaserSights : MonoBehaviour {

    #region Constant Fields
    private const float Y_OFFSET = 7.5f;
    private const float LINE_RENDER_WIDTH = 2.5f;

    #endregion Constant Fields

    #region Private Fields
    [SerializeField]
    private SOEffects _SOEffectHandler;
    [SerializeField]
    private LayerMask _whatToHit;
    [SerializeField]
    private Transform _endOfBarrel;

    private BoxCollider2D _playerBox;
    private Vector3 _playerPos;
    private Vector2 _obstruction;
    private LineRenderer _renderer;
    private GameObject _laserEffect;

    #endregion Private Fields

    #region Private Initializers
    private void Awake() {
        _renderer = GetComponent<LineRenderer>();
    }

    private void OnEnable() {
        _laserEffect = _SOEffectHandler.PlayEffect(EffectEnums.SniperLaserEffect, _endOfBarrel.position);
    }

    #endregion Private Initializers

    #region Private Finalizers
    private void OnDisable() {
        _SOEffectHandler.StopEffect(_laserEffect);
    }

    #endregion Private Finalizers

    #region Private Methods
    private void Update() {

        if (_playerBox == null) {
            _playerBox = GameObject.FindGameObjectWithTag(StringConstantUtility.PLAYER_TAG).GetComponent<BoxCollider2D>();
        }

        _playerPos = new Vector2(_playerBox.bounds.center.x, _playerBox.bounds.center.y + Y_OFFSET);

        CheckCollisions();

        //Set the Start Position
        _renderer.SetPosition(0, _endOfBarrel.position);

        //Set the End Position
        if (_obstruction == Vector2.zero) {
            _renderer.SetPosition(1, _playerPos);
            _laserEffect.transform.position = _playerPos;
        }
        else {
            _renderer.SetPosition(1, _obstruction);
            _laserEffect.transform.position = _obstruction;
        }

        //Set the width of the line renderer
        _renderer.startWidth = LINE_RENDER_WIDTH;
        _renderer.endWidth = LINE_RENDER_WIDTH;
        _renderer.startColor = Color.red;
        _renderer.endColor = Color.red;
    }

    private void CheckCollisions() {

        // Send out a raycast to determine if the laser should be touching the player or a world object.
        RaycastHit2D hit = Physics2D.Raycast(_endOfBarrel.position, _playerPos - _endOfBarrel.position, 
            Vector3.Magnitude(_playerPos - _endOfBarrel.position), _whatToHit);

        if (hit.collider != null) {
            _obstruction = hit.collider.gameObject.tag == StringConstantUtility.PLAYER_TAG ? Vector2.zero : hit.point;
        }
    }

    #endregion Private Methods
}
