using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerControl : MonoBehaviour
{
    [SerializeField]
    Car2D car;

    public GameControl Menu;

    float pressedBrake = 0f;

    void Update()
    {
        car.ThrottleInput = Input.GetAxis("Vertical");
        car.SteerInput = -Input.GetAxis("Horizontal");

        if (Input.GetKey(KeyCode.Escape))
            Menu.EscPressed();

        if (Input.GetKey(KeyCode.Space))
            pressedBrake = Mathf.Min(pressedBrake + 0.25f, 1f);
        else
            pressedBrake = 0f;
        car.BrakeInput = pressedBrake;
    }
}
