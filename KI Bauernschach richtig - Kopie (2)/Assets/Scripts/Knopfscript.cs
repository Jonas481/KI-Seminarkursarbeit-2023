using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knopfscript : MonoBehaviour
{
    public GameObject dasSpiel;

    // Start is called before the first frame update
    void Start()
    {
        dasSpiel = GameObject.FindGameObjectWithTag("GameController");

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Der Knopf wird gedrückt
    public void neuesSpiel()
    {
        Game spiel = dasSpiel.GetComponent<Game>();
        spiel.StarteNeuesSpiel();
    }
}
