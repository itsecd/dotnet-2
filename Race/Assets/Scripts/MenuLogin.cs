using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuLogin : MonoBehaviour
{

    public RaceClient client;

    private void Update()
    {
        if (client.SuccessLogin)
            LoadLobby();
    }
    public void LoadLobby() => SceneManager.LoadSceneAsync("Lobby");

    public void BackPressed() => SceneManager.LoadScene("Menu");
}
