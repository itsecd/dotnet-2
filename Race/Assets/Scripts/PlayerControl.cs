using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerControl : MonoBehaviour
{
    [SerializeField]
    Car2D car;

    float pressedBrake = 0f;

    void Update()
    {
        car.throttleInput = Input.GetAxis("Vertical");
        car.steerInput = -Input.GetAxis("Horizontal");

        if (Input.GetKey(KeyCode.Escape))
            SceneManager.LoadScene("Menu");

        if (Input.GetKey(KeyCode.Space))
            pressedBrake = Mathf.Min(pressedBrake + 0.25f, 1f);
        else
            pressedBrake = 0f;
        car.brakeInput = pressedBrake;
    }
}
