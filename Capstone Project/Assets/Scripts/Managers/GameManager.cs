using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    public SOSaveFile SOSaveHandler;
    public SOEffects SOEffectHandler;

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
            if (SOSaveHandler.SOCheckpointHandler.checkpointGO == null) {

                GameObject player = Instantiate( Resources.Load("MainCharacter/MainCharacter", typeof(GameObject)) as GameObject, transform.position, Quaternion.identity) as GameObject;
                SOEffectHandler.PlayEffect(EffectEnums.PlayerRespawn, SOSaveHandler.SOCheckpointHandler.checkpointPosition);
                player.transform.position = SOSaveHandler.SOCheckpointHandler.checkpointPosition;
            }
            else {
                GameObject player = Instantiate(Resources.Load("MainCharacter/MainCharacter", typeof(GameObject)) as GameObject, transform.position, Quaternion.identity) as GameObject;
                SOEffectHandler.PlayEffect(EffectEnums.PlayerRespawn, SOSaveHandler.SOCheckpointHandler.checkpointPosition);
                player.transform.position = SOSaveHandler.SOCheckpointHandler.checkpointPosition;
            }
        }
        else if (level == 2) {

        }
    }
}
