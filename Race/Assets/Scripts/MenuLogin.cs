using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuLogin : MonoBehaviour
{

    public RaceClient Client;

    private void Update()
    {
        if (Client.SuccessLogin)
            LoadLobby();
    }
    public void LoadLobby() => SceneManager.LoadSceneAsync("Lobby");

    public void BackPressed() => SceneManager.LoadScene("Menu");
}
