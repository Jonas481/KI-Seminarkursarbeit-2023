using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    public GameObject chesspiece;

    [SerializeField] private int FeldBreite = 4;
    [SerializeField] private int FeldHöhe = 4;

    private GameObject[,] positions;
    private GameObject[] playerBlack;
    private GameObject[] playerWhite;

    private string currentPlayer = "Weiß";

    private bool gameOver = false;

    // Start is called before the first frame update
    void Start()
    {
        positions = new GameObject[FeldBreite, FeldHöhe];
        playerBlack = new GameObject[FeldBreite];
        playerWhite = new GameObject[FeldBreite];

        for(int i = 0; i < FeldBreite; i++)
        {
            playerWhite[i] = Create("Weißer_Bauer", i, 0);
            playerBlack[i] = Create("Schwarzer_Bauer", i, FeldHöhe-1);
        }

        for (int i = 0; i < FeldBreite; i++)
        {
            SetPosition(playerBlack[i]);
            SetPosition(playerWhite[i]);
        }
    }

    public GameObject Create(string name, int x, int y)
    {
        GameObject obj = Instantiate(chesspiece, new Vector3(0, 0, -1), Quaternion.identity);
        Chessmen cm = obj.GetComponent<Chessmen>();
        cm.name = name;
        cm.SetXBoard(x);
        cm.SetYBoard(y);
        cm.Activate();
        return obj;
    }

    public int getBreite()
    {
        return FeldBreite;
    }

    public int getHöhe()
    {
        return FeldHöhe;
    }

    public void SetPosition(GameObject obj)
    {
        Chessmen cm = obj.GetComponent<Chessmen>();

        positions[cm.GetXBoard(), cm.GetYBoard()] = obj;
    }

    public void SetPositionEmpty(int x, int y)
    {
        positions[x, y] = null;
    }

    public GameObject GetPosition(int x, int y)
    {
        return positions[x, y];
    }

    public bool PositionOnBoard(int x, int y)
    {
        if (x < 0 || y < 0 || x >= FeldBreite || y >= FeldHöhe) return false;
        return true;
    }

    public string GetCurrentPlayer()
    {
        return currentPlayer;
    }

    public bool IsGameOver()
    {
        return gameOver;
    }

    public void SetGameOverFalse()
    {
        gameOver = false;
    }

    public void StarteNeuesSpiel()
    {
        gameOver = false;

        SceneManager.LoadScene("Game");
    }

    public void NextTurn()
    {
        if (currentPlayer == "Weiß")
        {
            currentPlayer = "Schwarz";
        }
        else
        {
            currentPlayer = "Weiß";
        }
    }

    public void Update()
    {
        //if (gameOver == true && Input.GetMouseButtonDown(0))
        //{
        //    gameOver = false;

        //    SceneManager.LoadScene("Game");
        //}
    }

    public void Winner(string playerWinner)
    {
        gameOver = true;

        GameObject.FindGameObjectWithTag("GewinnerText").GetComponent<TMPro.TextMeshProUGUI>().enabled = true;
        GameObject.FindGameObjectWithTag("GewinnerText").GetComponent<TMPro.TextMeshProUGUI>().text = playerWinner + "hat gewonnen";
    }
}
