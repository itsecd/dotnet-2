using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameControl : MonoBehaviour
{

    public RaceClient client;
    public GameObject LoadMenu;
    public GameObject ResultText;

    private void Awake()
    {
        var obj = GameObject.FindWithTag("CLIENT_CREATED");
        client = obj.GetComponent<RaceClient>();
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

    private void Update()
    {
        if (client.ResultMatch == RaceClient.Result.Win)
            ResultText.GetComponent<Text>().text = "You win, congratulation";
        if (client.ResultMatch == RaceClient.Result.Lose)
            ResultText.GetComponent<Text>().text = "You lose :c";
        if (client.ResultMatch != RaceClient.Result.None)
            LoadResultMenu();
    }
}
