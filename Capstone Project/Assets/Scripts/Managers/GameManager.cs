using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    public SOSaveFile SOSaveHandler;
    public SOEffects SOEffectHandler;

    private GameObject _player;
    private bool _inGame = false;

    private void OnEnable() {
        StartWindow.OnNewGame += OnNewGame;
    }

    private void OnDisable() {
        StartWindow.OnNewGame -= OnNewGame;
    }

    private void OnNewGame() {
        SOSaveHandler.NewGame();
    }

    private void OnLevelWasLoaded(int level) {
        if (level == 1) {
            if (SOSaveHandler.checkpointID == 0) {
                SpawnPlayer();
            }
            else {
                SpawnPlayer();
                GameObject[] checkpoints = GameObject.FindGameObjectsWithTag("Checkpoint");
                foreach(GameObject ckpt in checkpoints) {

                    if (ckpt.GetComponent<CheckpointBehavior>().ID == SOSaveHandler.checkpointID) {
                        ckpt.GetComponent<CheckpointBehavior>().Activate();
                    }
                }
            }
        }
        else if (level == 2) {

        }
    }

    private void SpawnPlayer() {
        GameObject _player = Instantiate(Resources.Load("MainCharacter/MainCharacter", typeof(GameObject)) as GameObject, transform.position, Quaternion.identity) as GameObject;
        SOEffectHandler.PlayEffect(EffectEnums.PlayerRespawn, SOSaveHandler.checkpointPosition);
        _player.transform.position = SOSaveHandler.checkpointPosition;
    }
}
