using UnityEngine;
using UnityEngine.UI;

public class LevelSelectWindowBack : MonoBehaviour {

	private void OnEnable() {

        Navigation navigation = GetComponent<Button>().navigation;
        navigation.selectOnLeft = GameObject.Find("LEVEL 1 BUTTON").GetComponent<Selectable>();
        navigation.selectOnRight = GameObject.Find("LEVEL 1 BUTTON").GetComponent<Selectable>();

        GetComponent<Button>().navigation = navigation;
    }
}
