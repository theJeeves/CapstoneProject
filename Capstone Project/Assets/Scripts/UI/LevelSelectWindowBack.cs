using UnityEngine;
using UnityEngine.UI;

public class LevelSelectWindowBack : MonoBehaviour {

    #region Constants
    private const string LEVEL_ONE_BUTTON = "LEVEL 1 BUTTON";

    #endregion Constants

    #region Private Methods
    private void OnEnable() {

        Navigation navigation = GetComponent<Button>().navigation;
        navigation.selectOnLeft = GameObject.Find(LEVEL_ONE_BUTTON).GetComponent<Selectable>();
        navigation.selectOnRight = GameObject.Find(LEVEL_ONE_BUTTON).GetComponent<Selectable>();

        GetComponent<Button>().navigation = navigation;
    }

    #endregion Private Methods
}
