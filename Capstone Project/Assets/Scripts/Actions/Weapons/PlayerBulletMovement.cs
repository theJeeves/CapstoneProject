using UnityEngine;
using System.Collections;

public class PlayerBulletMovement : MonoBehaviour {

    [SerializeField]
    private float _speed;

    private ControllableObject _controller;

	private void OnEnable()
    {
        _controller = GameObject.FindGameObjectWithTag("Player").GetComponent<ControllableObject>();
        StartCoroutine(movement());
    }

    private IEnumerator movement()
    {
        float direction = (float)_controller.Direction;
        while (true)
        {
            transform.Translate(Vector2.right  * _speed * Time.deltaTime);
            yield return 0;
        }
        
    }
}
