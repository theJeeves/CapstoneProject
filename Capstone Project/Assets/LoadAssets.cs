using UnityEngine;
using System.Collections;

public class LoadAssets : MonoBehaviour {

    [SerializeField]
    private SOEffects _SOEffectHandler;
    [SerializeField]
    private MovementRequest _MovementRequest;

	// Use this for initialization
	private void Awake () {

        _SOEffectHandler.LoadEffects();
        _MovementRequest.LoadMovementRequests();
	}

}
