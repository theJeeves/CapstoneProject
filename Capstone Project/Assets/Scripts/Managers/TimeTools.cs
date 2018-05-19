using UnityEngine;

public class TimeTools : MonoBehaviour {

	public static bool TimeExpired(ref float timeVar, float time = 0.0f) {

        DecreaseTime(ref timeVar);
        return timeVar <= time;
    }

    public static float TimeElapsed(ref float timeVar) {
        IncreaseTime(ref timeVar);
        return timeVar;
    }

    public static void DecreaseTime(ref float timeVar) {
        timeVar -= Time.deltaTime;
    }

    public static void IncreaseTime(ref float timeVar) {
        timeVar += Time.deltaTime;
    }
}
