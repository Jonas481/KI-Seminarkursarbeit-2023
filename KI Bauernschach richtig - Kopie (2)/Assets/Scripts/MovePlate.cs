using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlate : MonoBehaviour
{
    public GameObject controller;

    GameObject reference = null;

    int matrixX;
    int matrixY;

    public bool attack = false;
    public bool optimalVon = false;
    public bool optimalZu = false;

    public void Start()
    {
        if (attack)
        {
            gameObject.GetComponent<SpriteRenderer>().color = new Color(1.0f, 0.0f, 0.0f, 1.0f);
        }

        if (optimalVon)
        {
            gameObject.GetComponent<SpriteRenderer>().color = new Color(0.0f, 1.0f, 0.0f, 1.0f);
        }

        if (optimalZu)
        {
            gameObject.GetComponent<SpriteRenderer>().color = new Color(0.0f, 0.5f, 0.0f, 1.0f);
        }
    }

    public void OnMouseUp()
    {
        controller = GameObject.FindGameObjectWithTag("GameController");

        GameObject cp = controller.GetComponent<Game>().GetPosition(matrixX, matrixY);

        if (cp != null && !optimalVon)
        {
            Destroy(cp);
        }

        if (!optimalVon)
        {
            if (controller.GetComponent<Game>().GetCurrentPlayer() == "Weiß" && matrixY == controller.GetComponent<Game>().getHöhe()-1)
            {
                controller.GetComponent<Game>().Winner("Weiß ");
            }

            if (controller.GetComponent<Game>().GetCurrentPlayer() == "Schwarz" && matrixY == 0)
            {
                controller.GetComponent<Game>().Winner("Schwarz ");
            }
        }
        
        controller.GetComponent<Game>().SetPositionEmpty(reference.GetComponent<Chessmen>().GetXBoard(),reference.GetComponent<Chessmen>().GetYBoard());

        reference.GetComponent<Chessmen>().SetXBoard(matrixX);
        reference.GetComponent<Chessmen>().SetYBoard(matrixY);
        reference.GetComponent<Chessmen>().SetCoords();

        controller.GetComponent<Game>().SetPosition(reference);

        if (!optimalVon)
        {
            controller.GetComponent<Game>().NextTurn();
        }
        
        reference.GetComponent<Chessmen>().DestroyMovePlates();
    }

    public void SetCoords(int x, int y)
    {
        matrixX = x;
        matrixY = y;
    }

    public void SetReference(GameObject obj)
    {
        reference = obj;
    }

    public GameObject GetReference()
    {
        return reference;
    }
}
