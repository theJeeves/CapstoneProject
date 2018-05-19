﻿using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public enum DamageEnum {
    None,
    Acid,
    Explosion,
    LaserContinuous
}

public class PlayerHealth : MonoBehaviour {

    [Header("Health Variables")]
    [SerializeField]
    private int _maxHealth;                 // Max health the player can have at any time. Default value when the player respawns.
    [SerializeField]
    private int _health;                    // The health the player current has.
    [SerializeField]
    private float _recoveryTime;            // How long before the player can receive damage again.

    [Space]
    [Header("Effects")]

    [SerializeField]
    private GameObject _playerBody;
    [SerializeField]
    private SOEffects _SOEffectHandler;                 // Reference to the SOEffectHandler so this gameobject can request effects.
    [SerializeField]
    private ScreenShakeRequest _scrnShkRequest;         // Screenshake request to show damage has been done to the player.

    [Space]
    [Header("Game State")]
    [SerializeField]
    private SOWeaponManager _SOWeaponManager;
    private GameManager _GM;                 // Reference to the checkpoint system. This is where the player will respawn on death.

    [Space]
    [Header("Body Parts")]
    [SerializeField]
    private GameObject[] _bodyParts;

    private Transform[] _effectPositions;           // Positions where effects will be played.
    private GameObject _effect;                     // GameObject to reference a called effect. This is so the effect may be manipulated.
    private DamageEnum _damageType;                 // Type of damage so this script can call different effect types.
    private SpriterDotNetUnity.ChildData _entity;
    private bool _canTakeDamage;                    // Bool determining if the player is able to take damage or not. Tied with _recoveryTime.
    private float _timer = 0.0f;                    // Timer is used to determine how much time has passed for effect durations.
    //private int _damageReceived = 0;                // How much damage has the player received.
    private float _duration = 0.0f;                 // Duration for an effect to last.
    private bool _deathAnimationPlayed = false;     // Bool to ensure the death explosions only play once per death.
    private bool _killPlayerCalled = false;

    private InputManager _inputManager;
    private PlayerMovementManager _movementManager;
    private PlayerCollisionState _collisionState;

    // Getter/Setter for other scripts to refernce the player's health.
    public int Health {
        get { return _health; }
        set { _health = value; }
    }

    private void Awake() {
        _canTakeDamage = true;
    }

    private void OnEnable() {
        _GM = GameManager.Instance.GetComponent<GameManager>();
        _effectPositions = GetComponentsInChildren<Transform>();
        _movementManager = GetComponent<PlayerMovementManager>();
        _collisionState = GetComponent<PlayerCollisionState>();
    }

    void Start() {
        _health = _maxHealth;

        UpdateHealth?.Invoke(_health);
    }

    #region Events
    public static event UnityAction<int> UpdateHealth;
    public static event UnityAction OnPlayerDeath;
    #endregion Events

    #region Public Methods
    public void KillPlayer() {

        _killPlayerCalled = true;

        if (!_deathAnimationPlayed) {

            if (_inputManager == null) { _inputManager = InputManager.Instance.GetComponent<InputManager>(); }

            _playerBody.SetActive(false);
            _inputManager.PauseInput(60.0f);

            if (!_collisionState.OnSolidGround) {
                _SOEffectHandler.PlayEffect(EffectEnums.Player_Death00, transform.position);
            }
            else {
                _SOEffectHandler.PlayEffect(EffectEnums.Player_Death01, transform.position);
            }
            _deathAnimationPlayed = true;
            DeployBodyParts();
            _timer = Time.time;

            // This tells the save file to add to the number of deaths.
            OnPlayerDeath?.Invoke();
        }

        GetComponent<Rigidbody2D>().velocity = new Vector3(0.0f, 0.0f, 0.0f);
        GetComponent<Rigidbody2D>().gravityScale = 0.0f;
        GetComponent<PlayerMovementManager>().ClearQueue();

        // If an effect is playing and then the player dies, stop the effect at once.
        _duration = 0.0f;
        _SOEffectHandler.StopEffect(_effect);

        foreach (GameObject sprite in _entity.Sprites) {
            //Color spriteColor = sprite.GetComponent<SpriteRenderer>().color;
            sprite.GetComponent<SpriteRenderer>().color = Color.white;
        }

        if (Time.time - _timer > 1.0f) {

            transform.position = _GM.SOSaveHandler.CheckpointPosition;
            GetComponent<PlayerMovementManager>().ClearQueue();

            if (Camera.main.WorldToViewportPoint(_GM.SOSaveHandler.CheckpointPosition).x > 0.1f &&
                Camera.main.WorldToViewportPoint(_GM.SOSaveHandler.CheckpointPosition).x < 0.9f) {

                // Delay the player from any input for 1.5 seconds to ensure they do not immediate fly to their death
                // Call the respawn effect and restore all the default values for the player.
                _inputManager.PauseInput(1.25f);
                GetComponent<Rigidbody2D>().gravityScale = 40.0f;
                _SOEffectHandler.PlayEffect(EffectEnums.PlayerRespawn, _GM.SOSaveHandler.CheckpointPosition);
                _SOWeaponManager.Reload();
                _playerBody.SetActive(true);
                _deathAnimationPlayed = false;
                _killPlayerCalled = false;
            }
        }
    }

    /// <summary>
    /// The function is generally usesd by other game objects which can damage the player. This function handles various damage types.
    /// Some attacks will have an instant damage on the health while other may have a damage over time. 
    /// Each one will damage the player and, if applicable, call the SOEffectHandler to play the appropriate effect.
    /// </summary>
    public void DecrementPlayerHealth(int damage, float duration = 0.0f, DamageEnum damageType = DamageEnum.None) {
        if (damageType != DamageEnum.LaserContinuous) {
            // Tell the input manager to stop taking in input for 0.5 seconds
            // To ensure there are no conflicting movement requests, clear the player movement queue of all requests.
            if (_inputManager == null) { _inputManager = InputManager.Instance.GetComponent<InputManager>(); }
            _inputManager.PauseInput(0.5f);
            _movementManager.ClearQueue();
        }

        // If duration is passed in for damages which last for X amount of time
        if (duration > 0.0f) {
            //_damageReceived = damage;
            _duration = duration;

            // For each of the damage effects, call the appropriate functions to give the selected effect.
            if (damageType == DamageEnum.Acid) {
                AcidDamageEffect();
                _health -= damage;
                if (_effect != null) { _SOEffectHandler.StopEffect(_effect); }
                _effect = _SOEffectHandler.PlayEffect(EffectEnums.AcidDamageEffect, _effectPositions[1].position);
                _damageType = damageType;
            }
            else if (damageType == DamageEnum.Explosion) {
                ExplosionDamageEffect();
                if (_effect != null) { _SOEffectHandler.StopEffect(_effect); }
                _effect = _SOEffectHandler.PlayEffect(EffectEnums.ExplosionDamageEffect, _effectPositions[1].position);
                _damageType = damageType;
                _health -= damage;
                _scrnShkRequest.ShakeRequest();
            }

            _timer = Time.time;
        }
        else {
            _health -= damage;
            _scrnShkRequest.ShakeRequest();
        }

        // UPDATE HEALTH UI
        UpdateHealth?.Invoke(_health);
    }
    #endregion Public Methods

    #region Private Methods
    private void Update() {

        if (_entity == null) {
            _entity = GetComponent<SpriterDotNetUnity.SpriterDotNetBehaviour>().ChildData;
        }

        // if the player dies, stop any velocity the player had, place them at the last checkpoint, and reset their health to 100%.
        if (_health <= 0) {

            if (!_deathAnimationPlayed) {

                if (_inputManager == null) { _inputManager = InputManager.Instance.GetComponent<InputManager>(); }
                _inputManager.StopInput();

                GetComponent<BoxCollider2D>().enabled = false;
                GetComponent<PolygonCollider2D>().enabled = false;

                _playerBody.SetActive(false);

                if (!_collisionState.OnSolidGround) {
                    _SOEffectHandler.PlayEffect(EffectEnums.Player_Death00, transform.position);
                }
                else {
                    _SOEffectHandler.PlayEffect(EffectEnums.Player_Death01, transform.position);
                }
                _deathAnimationPlayed = true;
                DeployBodyParts();
                _timer = Time.time;

                // This tells the save file to add to the number of deaths.
                OnPlayerDeath?.Invoke();
            }

            GetComponent<Rigidbody2D>().velocity = new Vector3(0.0f, 0.0f, 0.0f);
            GetComponent<Rigidbody2D>().gravityScale = 0.0f;
            GetComponent<PlayerMovementManager>().ClearQueue();

            StartCoroutine(ReloadLevelDelay());

        }

        if (_killPlayerCalled) {
            KillPlayer();
        }

        if (_effect != null) {

            // If an effect is playing and then the player dies, stop the effect at once.
            if (_health < 0) {
                _duration = 0.0f;
                _SOEffectHandler.StopEffect(_effect);

                foreach (GameObject sprite in _entity.Sprites) {
                    //Color spriteColor = sprite.GetComponent<SpriteRenderer>().color;
                    sprite.GetComponent<SpriteRenderer>().color = Color.white;
                }
            }
            // if there is any type of effect associated with the player, ensure it is following them with a slight offset.
            _effect.transform.position = transform.position + new Vector3(0.0f, 25.0f, 0.0f);
        }

        // If an enemy has damaged the player and their is an effect to be played,
        // keep calling the damage and effect to do damage over time.
        if (_duration != 0.0f) {
            if (Time.time - _timer < _duration) {

                if (_damageType == DamageEnum.Acid && _canTakeDamage) {
                    _scrnShkRequest.ShakeRequest();
                    StartCoroutine(RecoveryDelay());
                }

                ReturnToNormalColor();
            }
            else {
                _duration = 0.0f;
                _SOEffectHandler.StopEffect(_effect);
            }
        }
    }

    // Go through all the sprites for the player and render them green
    // for an acid damage effect.
    private void AcidDamageEffect() {
        foreach (GameObject sprite in _entity.Sprites) {
            sprite.GetComponent<SpriteRenderer>().color = Color.green;
        }
    }

    // Go through all the sprites for the player and render them
    // dark grey for an explosion effect
    private void ExplosionDamageEffect() {
        foreach (GameObject sprite in _entity.Sprites) {
            sprite.GetComponent<SpriteRenderer>().color = new Color(0.18f, 0.18f, 0.18f);
        }
    }

    // Regardless of what color a damage effect has turned the player,
    // this goes through all the sprites for the player and renders them
    // back to their normal state.
    private void ReturnToNormalColor() {

        float t = Time.deltaTime * (1.0f / _duration);

        foreach (GameObject sprite in _entity.Sprites) {
            Color spriteColor = sprite.GetComponent<SpriteRenderer>().color;
            sprite.GetComponent<SpriteRenderer>().color = new Color(Mathf.Clamp01(spriteColor.r + t), 
                Mathf.Clamp01(spriteColor.g + t), Mathf.Clamp01(spriteColor.b + t));
        }
    }

    private IEnumerator RecoveryDelay() {
        _canTakeDamage = false;

        yield return new WaitForSeconds(_recoveryTime);

        _canTakeDamage = true;
    }

    private IEnumerator ReloadLevelDelay() {
        yield return new WaitForSeconds(5.0f);
        _GM.OnContinue(WindowIDs.None, WindowIDs.None);
    }

    private void DeployBodyParts() {

        foreach(GameObject bodyPart in _bodyParts) {
            GameObject instance = Instantiate(bodyPart, transform.position, Quaternion.identity) as GameObject;
            instance.GetComponent<Rigidbody2D>().AddForce(new Vector2(UnityEngine.Random.Range(-75.0f, 75.0f), UnityEngine.Random.Range(200.0f, 400.0f)), ForceMode2D.Impulse );
            instance.GetComponent<Rigidbody2D>().AddTorque(UnityEngine.Random.Range(-500.0f, 500.0f));
        }
    }

    #endregion Private Methods
}
