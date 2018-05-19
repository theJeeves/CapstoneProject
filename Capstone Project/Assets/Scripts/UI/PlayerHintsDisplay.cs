using UnityEngine;
using UnityEngine.UI;

public class PlayerHintsDisplay : MonoBehaviour {

    #region Constant Fields
    private const string TO_BE_REPLACED = " NL ";
    private const string NEW_LINE = "\n";

    #endregion Constant Fields

    #region Private Fields
    private Text m_Hints;

    #endregion Private Fields

    #region Initializers
    private void Awake()
    {
        m_Hints = GetComponent<Text>();
    }

    private void OnEnable() {
        InstructionText.DisplayHint += DisplayHint;
    }

    #endregion Initializers

    #region Finalizers
    private void OnDisable() {
        InstructionText.DisplayHint -= DisplayHint;
    }

    #endregion Finalizers

    #region Private Methods
    private void DisplayHint(string hint) {

        hint = hint.Replace(TO_BE_REPLACED, NEW_LINE);
        m_Hints.text = hint;
    }

    #endregion Private Methods
}
