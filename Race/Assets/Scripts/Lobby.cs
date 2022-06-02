using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Lobby : MonoBehaviour
{
    public RaceClient client;

    public GameObject PlayerText;

    private void Awake()
    {
        var obj = GameObject.FindWithTag("CLIENT_CREATED");
        client = obj.GetComponent<RaceClient>();
    }
    void Start()
    {
        PlayerText.GetComponent<Text>().text = client.PlayerLogin;
    }

    private void Update()
    {
        if (client.OpponentFound)
            SceneManager.LoadScene("Game");
    }

    public void FindOpponent()
    {
        client.FindOpponent();
    }
}
