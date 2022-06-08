using UnityEngine;
using UnityEngine.SceneManagement;


public class MenuControler : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKey("escape"))  
        {
            Application.Quit();   
        }
    }
    public void PlayPressed()
    {
        SceneManager.LoadScene(ScenesName.MenuLogin);
    }

    public void ExitPressed()
    {
        Application.Quit();
    }
}
