using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeKeyboardInputHandler : MonoBehaviour
{
    [SerializeField]
    private KeyCode _leftKeyCode;

    [SerializeField]
    private KeyCode _rightKeyCode;

    [SerializeField]
    private KeyCode _downKeyCode;

    [SerializeField]
    private KeyCode _forwardKeyCode;


    private IObjectMover _objectMover;

    private void Start()
    {
        _objectMover = GetComponent<IObjectMover>();

        if (_objectMover == null)
            Debug.LogError("Вы забыли добавить компонент ObjectMover! Код не стабилен!");
        else
            _objectMover.MoveForward();
    }

    private void Update()
    {
        if (_objectMover == null)
            return;

        if (Input.GetKeyDown(_forwardKeyCode))
            _objectMover.Rotate(Quaternion.Euler(0, 0, 90));
        else if (Input.GetKeyDown(_downKeyCode))
            _objectMover.Rotate(Quaternion.Euler(0, 0, -90));
        else if (Input.GetKeyDown(_rightKeyCode))
            _objectMover.Rotate(Quaternion.Euler(0, 0, 0));
        else if (Input.GetKeyDown(_leftKeyCode))
            _objectMover.Rotate(Quaternion.Euler(0, 0, -180));
    }
}

