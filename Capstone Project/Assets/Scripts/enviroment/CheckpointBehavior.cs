using UnityEngine;
using SpriterDotNetUnity;
using System.Collections.Generic;
using System.Linq;

public class CheckpointBehavior : MonoBehaviour {

    #region Fields

    #region Public Fields
    [Header("Checkpoint Properties")]
    public int currentLevel = 0;
    public int ID = 0;

    [Space]
    [Header("Camera Properties")]

    public Vector3 cameraPos;
    public bool smartXEnabled;
    public bool smartYEnabled;

    [Space]

    #endregion Public Fields

    #region Private Fields
    [Header("Tools")]

    [SerializeField]
    private GameManager _GM;             // Reference to the save file to set and get values from it.
    [SerializeField]
    private Transform _respawnPoint;


    private UnityAnimator _animator;                // Refernce to the SpriterDotNetUnity animator to animate the checkpoint
    private bool _activated = false;                // Determines if the player has reached hit a given checkpoint

    #endregion Private Fields

    #endregion Fields

    #region Private Initializers
    private void OnEnable() {
        _GM = GameManager.Instance.GetComponent<GameManager>();
    }

    #endregion Private Initializers

    #region Public Methods
    // If the player has reached a new checkpoint, set the checkpoint to a checked state and record their position.
    // Also, record which checkpoint the player has just reached.
    public void Activate() {
        _activated = true;

        if (_animator != null) {
            _animator.Play(GetAnimation(1));
            _GM.SOSaveHandler.CheckpointPosition = _respawnPoint.position;
            _GM.SOSaveHandler.CheckpointID = ID;
            _GM.SOSaveHandler.CurrentLevel = currentLevel;

            if (currentLevel == 1 && ID >= 7) {
                _GM.SOSaveHandler.JouleEnabled = 1;
            }

            GetComponent<AudioSource>().Play();
        }
    }

    #endregion Public Methods

    #region Private Methods
    private void Update() {
        // Keep looking for the animator component unit it is found.
        if (_animator == null) {
            _animator = GetComponent<SpriterDotNetBehaviour>().Animator;
            if (_activated) {
                Activate();
            }
        }

        // If a player has reached a new checkpoint, reset the old checkpoint to a ready state (not a checked state)
        bool allowAction = _activated;
        allowAction &= _GM.SOSaveHandler.CheckpointID != ID;

        if (allowAction) {
            _activated = false;
            _animator.Play(GetAnimation(-1));
        }
    }

    private void OnTriggerEnter2D(Collider2D otherGo) {

        bool allowAction = _activated == false;
        allowAction &= otherGo.gameObject.tag == StringConstantUtility.PLAYER_TAG;

        if (allowAction) {
            Activate();
        }
    }

    // This is not Unity. This is SpriterDotNetUnity. This code is based on an example
    // straight from the developer. 1. generate a list of all the avaialbe animations.
    // 2. get the index of the animation you are looking for. 3. Offset the index to find the
    // correct animation.
    private string GetAnimation(int offset) {

        if (_animator != null) {
            List<string> animations = _animator.GetAnimations().ToList();
            int index = animations.IndexOf(_animator.CurrentAnimation.Name);
            index += offset;
            if (index >= animations.Count) index = 0;
            if (index < 0) index = animations.Count - 1;
            return animations[index];
        }

        return "error";
    }

    #endregion Private Methods
}
