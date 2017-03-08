using UnityEngine;
using System.Collections;

public class Singleton<Type> : MonoBehaviour where Type : MonoBehaviour {

    private static Type _instance;

    public static Type Instance {
        get {
            // If the instance is null, look to see if there is already an instance in the scene.
            if (_instance == null) {
                _instance = GameObject.FindObjectOfType<Type>();

                // If an instance could not be found, create one.
                if (_instance == null) {
                    GameObject singleton = new GameObject(typeof(Type).Name);
                    _instance = singleton.AddComponent<Type>();
                }
            }
            return _instance;
        }
    }

	protected virtual void Awake() {

        // If there is already an instance in the scene, destory everything else.
        if (_instance == null) {
            _instance = this as Type;
            DontDestroyOnLoad(gameObject);
        }
        //else {
        //    Destroy(gameObject);
        //}
    }
}
