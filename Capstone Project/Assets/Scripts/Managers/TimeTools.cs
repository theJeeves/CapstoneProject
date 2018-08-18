using UnityEngine;

public class TimeTools
{
    public static float TimeElapsed(ref float timeVar) {
        IncreaseTime(ref timeVar);
        return timeVar;
    }

    public static void IncreaseTime(ref float timeVar) {
        timeVar += Time.deltaTime;
    }
}
