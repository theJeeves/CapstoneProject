using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName ="SO Input Type/New Input Type")]
public class SOInputType : ScriptableObject {

    public InputAxisState[] inputs;               // An array of all the inputs which can be used by the player
}
