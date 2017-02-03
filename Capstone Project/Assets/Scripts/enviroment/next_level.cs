using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class next_level : MonoBehaviour {
    [SerializeField]
    private int level_number;

    private void OnCollisionEnter2D(Collision2D otherGO)
    {

        string tag = otherGO.gameObject.tag;

        if (tag == "Player")
        {
            SceneManager.LoadScene(level_number);
        }

    }
}
