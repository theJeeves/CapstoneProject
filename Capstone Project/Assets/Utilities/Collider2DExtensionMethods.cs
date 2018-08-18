using UnityEngine;

public static class Collider2DExtensionMethods
{
    /// <summary>
    /// Returns true if the Collider2D's instance does NOT have a 
    /// parent gameObject with a tag equalling the player's tag. False otherwise.
    /// </summary>
    /// <param name="collider"></param>
    /// <returns></returns>
    public static bool IsNotPlayer(this Collider2D collider)
    {
        return collider.gameObject.tag != Tags.PlayerTag ? true : false;
    }
}
