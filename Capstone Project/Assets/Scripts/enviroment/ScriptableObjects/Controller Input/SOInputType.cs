using UnityEngine;

[CreateAssetMenu(menuName ="SO Input Type/New Input Type")]
public class SOInputType : ScriptableObject {

    #region Public Fields
    public InputAxisState[] inputs;               // An array of all the inputs which can be used by the player

    #endregion Public Fields
}
