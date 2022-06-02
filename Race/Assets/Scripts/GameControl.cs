using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameControl : MonoBehaviour
{

    public RaceClient Client;
    public GameObject LoadMenu;
    public GameObject ResultText;

    private void Awake()
    {
        var obj = GameObject.FindWithTag("CLIENT_CREATED");
        Client = obj.GetComponent<RaceClient>();
    }
    private void LoadResultMenu()
    {
        this.gameObject.SetActive(false);
        LoadMenu.SetActive(true);
    }

    public void MenuPressed()
    {
        SceneManager.LoadScene("Menu");
    }

    public void EscPressed()
    {
        Client.CloseConnection();
        SceneManager.LoadScene("Menu");
    }

    private void Update()
    {
        if (Client.ResultMatch == RaceClient.Result.Win)
            ResultText.GetComponent<Text>().text = "You win, congratulation";
        if (Client.ResultMatch == RaceClient.Result.Lose)
            ResultText.GetComponent<Text>().text = "You lose :c";
        if (Client.ResultMatch != RaceClient.Result.None)
            LoadResultMenu();
    }
}
