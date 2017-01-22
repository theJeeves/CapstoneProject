using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private Canvas _quitMenu;

    [SerializeField]
    private Button _play;

    [SerializeField]
    private Button _quit;

    // Use this for initialization
    void Start()
    {
        _quitMenu = _quitMenu.GetComponent<Canvas>();
        _play = _play.GetComponent<Button>();
        _quit = _quit.GetComponent<Button>();
        _quitMenu.enabled = false;
    }

    public void ExitPress()
    {
        _quitMenu.enabled = true;
        _play.enabled = false;
        _quit.enabled = false;
    }

    public void NoPress()
    {
        _quitMenu.enabled = false;
        _play.enabled = true;
        _quit.enabled = true;
    }

    public void PlayGame()
    {
        Application.LoadLevel(1);

    }

    public void ExitGame()
    {
        Application.Quit();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
