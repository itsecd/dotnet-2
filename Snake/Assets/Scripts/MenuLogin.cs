using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuLogin : MonoBehaviour
{
    public SnakeClient Client;

    private void Update()
    {
        if (Client.SuccessLogin)
            LoadLobby();
    }
    public void LoadLobby() => SceneManager.LoadSceneAsync(ScenesName.Game);

    public void BackPressed() => SceneManager.LoadScene(ScenesName.Menu);
}
