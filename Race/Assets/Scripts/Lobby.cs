using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Lobby : MonoBehaviour
{
    public RaceClient Client;

    public GameObject PlayerText;

    private void Awake()
    {
        var obj = GameObject.FindWithTag("CLIENT_CREATED");
        Client = obj.GetComponent<RaceClient>();
    }
    void Start()
    {
        PlayerText.GetComponent<Text>().text = Client.PlayerLogin;
    }

    private void Update()
    {
        if (Client.OpponentFound)
            SceneManager.LoadScene(ScenesName.Game);
    }

    public void BackButtonPressed()
    {
        Client.CloseConnection();
        SceneManager.LoadScene(ScenesName.Menu);
    }

    public void FindOpponent()
    {
        Client.FindOpponent();
    }
}
