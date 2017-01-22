using UnityEngine;
using System.Collections;

public class RespawnPlayer : MonoBehaviour {

    [SerializeField]
    private SORespawn _respawnContainer;
    [SerializeField]
    private SOEffects _SOEffect;

    private void OnTriggerEnter2D(Collider2D otherGO) {

        if (otherGO.tag == "Player") {
            _SOEffect.PlayEffect(EffectEnum.PlayerRespawn, _respawnContainer.respawnPos);
            otherGO.GetComponent<Rigidbody2D>().velocity = new Vector3(0.0f, 0.0f, 0.0f);
            otherGO.transform.position = _respawnContainer.respawnPos;
        }
    }
}
