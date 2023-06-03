using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chessmen : MonoBehaviour
{
    public GameObject controller;
    public GameObject movePlate;

    private int xBoard = -1;
    private int yBoard = -1;

    private string player;

    public Sprite Schwarzer_Bauer;
    public Sprite Weiﬂer_Bauer;

    public void Activate()
    {
        controller = GameObject.FindGameObjectWithTag("GameController");

        SetCoords();

        switch (this.name)
        {
            case "Schwarzer_Bauer": this.GetComponent<SpriteRenderer>().sprite = Schwarzer_Bauer; player = "Schwarz"; break;
            
            case "Weiﬂer_Bauer": this.GetComponent<SpriteRenderer>().sprite = Weiﬂer_Bauer; player = "Weiﬂ"; break;
        }
    }

    public void SetCoords() {
        float x = xBoard;
        float y = yBoard;

        x *= 1.9f;
        y *= 1.8f;

        x += -1.9f;
        y += -1.9f;

        this.transform.position = new Vector3(x, y, -1.0f);
    }

    public int GetXBoard()
    {
        return xBoard;
    }

    public int GetYBoard()
    {
        return yBoard;
    }

    public string getPlayer()
    {
        return player;
    }

    public void SetXBoard(int x)
    {
        xBoard = x;
    }

    public void SetYBoard(int y)
    {
        yBoard = y;
    }

    private void OnMouseUp()
    {
        if (!controller.GetComponent<Game>().IsGameOver() && controller.GetComponent<Game>().GetCurrentPlayer() == player)
        {
            DestroyMovePlates();

            InitiateMovePlates();
        }
    }

    public void DestroyMovePlates()
    {
        GameObject[] movePlates = GameObject.FindGameObjectsWithTag("MovePlate");
        for (int i = 0; i < movePlates.Length; i++)
        {
            Destroy(movePlates[i]);
        }
    }

    public void InitiateMovePlates()
    {
        switch (this.name)
        {
            case "Schwarzer_Bauer":
                PawnMovePlate(xBoard, yBoard - 1);
                break;
            case "Weiﬂer_Bauer":
                PawnMovePlate(xBoard, yBoard + 1);
                break;
        }
    }

    public void PawnMovePlate(int x, int y)
    {
        Game sc = controller.GetComponent<Game>();
        if(sc.PositionOnBoard(x, y))
        {
            if (sc.GetPosition(x,y) == null)
            {
                MovePlateSpawn(x, y);
            }

            if (sc.PositionOnBoard(x + 1, y) && sc.GetPosition(x+1, y) != null && sc.GetPosition(x+1,y).GetComponent<Chessmen>().player != player)
            {
                MovePlateAttackSpawn(x + 1, y);
            }

            if (sc.PositionOnBoard(x - 1, y) && sc.GetPosition(x - 1, y) != null && sc.GetPosition(x - 1, y).GetComponent<Chessmen>().player != player)
            {
                MovePlateAttackSpawn(x - 1, y);
            }
        }
    }

    public void MovePlateSpawn(int matrixX, int matrixY)
    {
        float x = matrixX;
        float y = matrixY;

        x *= 1.9f;
        y *= 1.8f;

        x += -1.9f;
        y += -1.9f;

        GameObject mp = Instantiate(movePlate, new Vector3(x, y, -3.0f), Quaternion.identity);

        MovePlate mpScript = mp.GetComponent<MovePlate>();
        mpScript.SetReference(gameObject);
        mpScript.SetCoords(matrixX, matrixY);
    }

    public void MovePlateAttackSpawn(int matrixX, int matrixY)
    {
        float x = matrixX;
        float y = matrixY;

        x *= 1.9f;
        y *= 1.8f;

        x += -1.9f;
        y += -1.9f;

        GameObject mp = Instantiate(movePlate, new Vector3(x, y, -3.0f), Quaternion.identity);

        MovePlate mpScript = mp.GetComponent<MovePlate>();
        mpScript.attack = true;
        mpScript.SetReference(gameObject);
        mpScript.SetCoords(matrixX, matrixY);
    }

    public void MovePlateOptimalVonSpawn(int matrixX, int matrixY)
    {
        float x = matrixX;
        float y = matrixY;

        x *= 1.9f;
        y *= 1.8f;

        x += -1.9f;
        y += -1.9f;

        GameObject mp = Instantiate(movePlate, new Vector3(x, y, -3.0f), Quaternion.identity);

        MovePlate mpScript = mp.GetComponent<MovePlate>();
        mpScript.optimalVon = true;
        mpScript.SetReference(gameObject);
        mpScript.SetCoords(matrixX, matrixY);
    }

    public void MovePlateOptimalZuSpawn(int matrixX, int matrixY)
    {
        float x = matrixX;
        float y = matrixY;

        x *= 1.9f;
        y *= 1.8f;

        x += -1.9f;
        y += -1.9f;

        GameObject mp = Instantiate(movePlate, new Vector3(x, y, -3.0f), Quaternion.identity);

        MovePlate mpScript = mp.GetComponent<MovePlate>();
        mpScript.optimalZu = true;
        mpScript.SetReference(gameObject);
        mpScript.SetCoords(matrixX, matrixY);
    }

}




























//public void LineMovePlate(int xIncrement, int yIncrement)
//{
//    Game sc = controller.GetComponent<Game>();

//    int x = xBoard + xIncrement;
//    int y = yBoard + yIncrement;

//    while (sc.PositionOnBoard(x,y) && sc.GetPosition(x,y) == null)
//    {
//        MovePlateSpawn(x, y);
//        x += xIncrement;
//        y += yIncrement;
//    }

//    if (sc.PositionOnBoard(x,y) && sc.GetPosition(x,y).GetComponent<Chessmen>().player != player)
//    {
//        MovePlateAttackSpawn(x, y);
//    }
//}

//public void pointmoveplate(int x, int y)
//{
//    game sc = controller.getcomponent<game>();
//    if (sc.positiononboard(x,y))
//    {
//        gameobject cp = sc.getposition(x, y);

//        if (cp == null)
//        {
//            moveplatespawn(x, y);
//        }

//        else if (cp.getcomponent<chessmen>().player != player)
//        {
//            moveplateattackspawn(x, y);
//        }
//    }
//}