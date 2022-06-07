using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MenuControler : MonoBehaviour
{

    public void PlayPressed()
    {
        SceneManager.LoadScene(ScenesName.MenuLogin);
    }

    public void ExitPressed()
    {
        Application.Quit();
    }
}
