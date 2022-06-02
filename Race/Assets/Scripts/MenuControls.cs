using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuControls : MonoBehaviour
{

    private void Awake()
    {
        GameObject obj = GameObject.FindWithTag("CLIENT_CREATED");
        if (obj != null)
        {
            Destroy(obj);
        }
    }
    public void PlayPressed()
    {
        SceneManager.LoadScene("Login");
    }

    public void ExitPressed()
    {
        Application.Quit();
    }
}
