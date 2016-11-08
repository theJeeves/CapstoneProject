using UnityEngine;
using System.Collections;

public class SniperLockOn : LockOntoPlayer {

    public static event LockOntoPlayerEvent Attack;

    protected override IEnumerator LockOn() {

        while (true) {
            int startTime = 0;
            while (startTime < _timer) {
                yield return new WaitForSeconds(1.0f);
                ++startTime;
            } 

            if (Attack != null) {
                Attack();
            }
        }
    }
}
