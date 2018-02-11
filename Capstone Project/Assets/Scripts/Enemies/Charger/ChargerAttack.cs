using UnityEngine;

public class ChargerAttack : MonoBehaviour {

    #region Private Methods
    private void OnCollisionEnter2D(Collision2D otherObject) {

        if(otherObject.gameObject.tag == StringConstantUtility.PLAYER_TAG) {

        }
    }

    #endregion Private Methods
}
