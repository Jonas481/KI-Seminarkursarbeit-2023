using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class irgendwas : MonoBehaviour
{
    public GameObject dasSpiel;

    private bool[,,] Stellung;
    private int Höhe;
    private int Breite;

    private int xZugVorher;
    private int xZugNachher;
    private int yZugVorher;
    private int yZugNachher;

    // private bool SpielIstVorbei;

    private bool ersterZug;

    //public GameObject[] momentanWeiß;
    //public GameObject[] momentanSchwarz;

    // Start is called before the first frame update
    void Start()
    {
        dasSpiel = GameObject.FindGameObjectWithTag("GameController");
        Breite = dasSpiel.GetComponent<Game>().getBreite();
        Höhe = dasSpiel.GetComponent<Game>().getHöhe();
        Stellung = new bool[2, Breite, Höhe];

        ersterZug = true;
        // SpielIstVorbei = false;

        xZugVorher = -1;
        xZugNachher = -1;
        yZugVorher = -1;
        yZugNachher = -1;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void machwas()
    {
        Game spiel = dasSpiel.GetComponent<Game>();

        GameObject Figur = null;

        // setze die Werte in Stellung
        // Stellung[a,b,c] = true heißt Spieler a hat Figur an Position b,c
        for (int x = 0; x < Breite; x++)
        {
            for (int y = 0; y < Höhe; y++)
            {
                if (spiel.GetPosition(x, y) == null)
                {
                    Stellung[0, x, y] = false;
                    Stellung[1, x, y] = false;
                    continue;
                }

                if (spiel.GetPosition(x, y).GetComponent<Chessmen>().getPlayer() == "Weiß")
                {
                    Stellung[0, x, y] = true;
                    Stellung[1, x, y] = false;
                }
                else
                {
                    Stellung[0, x, y] = false;
                    Stellung[1, x, y] = true;
                }                
            }
        }

        GameObject.FindGameObjectWithTag("GewinnerText").GetComponent<TMPro.TextMeshProUGUI>().enabled = true;

        int SpielerDran;
        if (spiel.GetCurrentPlayer() == "Weiß")
        {
            SpielerDran = 0;
        }
        else
        {
            SpielerDran = 1;
        }

        ersterZug = true;
        xZugVorher = -1;
        yZugVorher = -1;

        
        if ( istGewonnen(0, Stellung) )
        {
            GameObject.FindGameObjectWithTag("GewinnerText").GetComponent<TMPro.TextMeshProUGUI>().text = "Weiß hat schon gewonnen!";
            return;
        }

        if ( istGewonnen(1, Stellung) )
        {
            GameObject.FindGameObjectWithTag("GewinnerText").GetComponent<TMPro.TextMeshProUGUI>().text = "Schwarz hat schon gewonnen!";
            return;
        }

        if ( EsIstSchonVorbei() )
        {
            GameObject.FindGameObjectWithTag("GewinnerText").GetComponent<TMPro.TextMeshProUGUI>().text = "Spiel ist schon unentschieden!";
            return;
        }

        int Ergebnis = gewinntSpieler(SpielerDran, Stellung);
        if ( Ergebnis == 0 )
        {
            GameObject.FindGameObjectWithTag("GewinnerText").GetComponent<TMPro.TextMeshProUGUI>().text = spiel.GetCurrentPlayer() + " kann unentschieden erreichen :|";
            Figur = dasSpiel.GetComponent<Game>().GetPosition(xZugVorher, yZugVorher);
            Figur.GetComponent<Chessmen>().DestroyMovePlates();
            Figur.GetComponent<Chessmen>().MovePlateOptimalVonSpawn(xZugVorher, yZugVorher);
            Figur.GetComponent<Chessmen>().MovePlateOptimalZuSpawn(xZugNachher, yZugNachher);
        }

        if ( Ergebnis == 1 )
        {
            GameObject.FindGameObjectWithTag("GewinnerText").GetComponent<TMPro.TextMeshProUGUI>().text = spiel.GetCurrentPlayer() + " kann Sieg erreichen :)";
            // GameObject.FindGameObjectWithTag("GewinnerText").GetComponent<TMPro.TextMeshProUGUI>().text = "Sieg für " + spiel.GetCurrentPlayer() + " Zug " + xZugVorher + "," + yZugVorher + " nach " + xZugNachher + "," + yZugNachher;
            Figur = dasSpiel.GetComponent<Game>().GetPosition(xZugVorher, yZugVorher);
            Figur.GetComponent<Chessmen>().DestroyMovePlates();
            Figur.GetComponent<Chessmen>().MovePlateOptimalVonSpawn(xZugVorher, yZugVorher);
            Figur.GetComponent<Chessmen>().MovePlateOptimalZuSpawn(xZugNachher, yZugNachher);
        }

        if ( Ergebnis == -1 )
        {
            GameObject.FindGameObjectWithTag("GewinnerText").GetComponent<TMPro.TextMeshProUGUI>().text = spiel.GetCurrentPlayer() + " kann nichts erreichen :(";
        }


    }

    private bool EsIstSchonVorbei()
    {
        if ( istGewonnen(0,Stellung) || istGewonnen(1,Stellung) )
        {
            return true;
        }
        for(int x=0; x<Breite; x++)
        {
            for(int y=0; y < Höhe; y++)
            {
                // Spieler 0 könnte nach oben ziehen
                if ( Stellung[0,x,y] && !Stellung[0,x,y+1] && !Stellung[1,x,y+1] )
                {
                    return false;
                }
                // Spieler 0 könnte nach rechts schlagen
                if (Stellung[0, x, y] && x+1 < Breite && Stellung[1, x+1, y + 1] )
                {
                    return false;
                }
                // Spieler 0 könnte nach links schlagen
                if (Stellung[0, x, y] && x - 1 >= 0 && Stellung[1, x - 1, y + 1])
                {
                    return false;
                }
                // Spieler 1 könnte nach unten ziehen
                if (Stellung[1, x, y] && !Stellung[0, x, y - 1] && !Stellung[1, x, y - 1])
                {
                    return false;
                }
                // Spieler 1 könnte nach rechts schlagen
                if (Stellung[1, x, y] && x + 1 < Breite && Stellung[0, x + 1, y - 1])
                {
                    return false;
                }
                // Spieler 1 könnte nach links schlagen
                if (Stellung[1, x, y] && x - 1 >= 0 && Stellung[0, x - 1, y - 1])
                {
                    return false;
                }
            }
        }
        return true;
    }


    private bool istGewonnen(int Spieler, bool[,,] Feld)
    {
        int ZielZeile;

        if( Spieler == 0)
        {
            ZielZeile = Höhe-1;
        }
        else
        {
            ZielZeile = 0;
        }

        for(int x = 0; x < Breite; x++)
        {
            if( Feld[Spieler,x,ZielZeile] )
            {
                //Debug.Log("Spieler " + Spieler + "steht auf" + x +"," + ZielZeile);
                return true;
            }
        }

        return false;
    }


    // Berechnet ob 'Spieler' auf 'Feld' gewinnt
    private int gewinntSpieler(int Spieler, bool[,,] Feld)
    {
        // Ausgabe: +1 Sieg, 0 Unentschieden, -1 Niederlage

        // wenn das der erste Zug ist, müssen wir uns das später merken
        bool IstErsterZug = ersterZug;
        // Alles danach ist nicht mehr der erste Zug
        ersterZug = false;

        // Spieler hat schon gewonnen
        if (istGewonnen(Spieler, Feld))
        {
            //if (IstErsterZug )
            //{
            //    SpielIstVorbei = true;
            //}
            return +1;
        }

        // Also hat Spieler noch nicht gewonnen
        int ZielRichtung;
        int Gegenspieler = 1 - Spieler;

        if (Spieler == 0)  // Spieler ist Weiß
        {
            ZielRichtung = 1;
        }
        else  // Spieler ist Schwarz
        {
            ZielRichtung = -1;
        }

        // Kopiere Feld in eigene Kopie
        bool[,,] neuesFeld = Feld.Clone() as bool[,,];

        // Spieler verliert wenn man keine gültigen Zug findet oder alle gültigen Züge verlieren
        bool habenUnentschiedenGesehen = false;
        bool habenNiederlageGesehen = false;

        // Debug.Log("Teste Spieler " + Spieler);
        // Debug.Log("erster Zug = " + ersterZug);

        // Gehe alle Spielfelder x,y durch
        for (int x = 0; x < Breite; x++)
        {
            for(int y = 0; y < Höhe; y++)
            {
                // Auf Feld x,y steht KEINE Figur vom Spieler
                if (!neuesFeld[Spieler,x,y])
                {
                    continue;
                }

                // Also steht jetzt auf x,y eine Figur vom Spieler
                
                // Teste Schritt nach vorne
                if ( y+ZielRichtung >= 0 && y+ZielRichtung < Höhe && !neuesFeld[Spieler,x,y+ZielRichtung] && !neuesFeld[Gegenspieler,x,y+ZielRichtung])
                {
                    // mache den Zug nach vorne im NEUEN FELD
                    neuesFeld[Spieler, x, y] = false;
                    neuesFeld[Spieler, x, y+ZielRichtung] = true;
                    int testWert = gewinntSpieler(Gegenspieler, neuesFeld);
                    if ( testWert == -1 )  // Gegenspieler verliert. Wir geben SIEG zurück.
                    {
                        if ( IstErsterZug )
                        {
                            xZugVorher = x;
                            xZugNachher = x;
                            yZugVorher = y;
                            yZugNachher = y + ZielRichtung;
                        }
                        return +1;
                    }
                    if (testWert == 0)  // Situtation wird unentschieden. Der Zug wird gemerkt.
                    {
                        if (IstErsterZug)
                        {
                            xZugVorher = x;
                            xZugNachher = x;
                            yZugVorher = y;
                            yZugNachher = y + ZielRichtung;
                        }
                        // bestesErgebnis = 0;
                        habenUnentschiedenGesehen = true;
                    }
                    if (testWert == 1)
                    {
                        habenNiederlageGesehen = true;
                    }
                    // mache den Zug wieder rückgängig im NEUEN FELD
                    neuesFeld[Spieler, x, y] = true;
                    neuesFeld[Spieler, x, y + ZielRichtung] = false;
                }

                // Teste Schlag nach links
                if (y + ZielRichtung >= 0 && y + ZielRichtung < Höhe && x-1 >= 0 && !neuesFeld[Spieler, x-1, y + ZielRichtung] && neuesFeld[Gegenspieler, x-1, y + ZielRichtung])
                {
                    // schlage die Figur links im NEUEN FELD
                    neuesFeld[Spieler, x, y] = false;
                    neuesFeld[Spieler, x-1, y + ZielRichtung] = true;
                    neuesFeld[Gegenspieler, x - 1, y + ZielRichtung] = false;
                    int testWert = gewinntSpieler(Gegenspieler, neuesFeld);
                    if (testWert == -1)  // Gegenspieler verliert. Wir geben SIEG zurück.
                    {
                        if (IstErsterZug)
                        {
                            xZugVorher = x;
                            xZugNachher = x-1;
                            yZugVorher = y;
                            yZugNachher = y + ZielRichtung;
                        }
                        return +1;
                    }
                    if (testWert == 0)  // Situtation wird unentschieden. Der Zug wird gemerkt.
                    {
                        if (IstErsterZug)
                        {
                            xZugVorher = x;
                            xZugNachher = x - 1;
                            yZugVorher = y;
                            yZugNachher = y + ZielRichtung;
                        }
                        // bestesErgebnis = 0;
                        habenUnentschiedenGesehen = true;
                    }
                    if (testWert == 1)
                    {
                        habenNiederlageGesehen = true;
                    }
                    // mache den Zug wieder rückgängig im NEUEN FELD
                    neuesFeld[Spieler, x, y] = true;
                    neuesFeld[Spieler, x - 1, y + ZielRichtung] = false;
                    neuesFeld[Gegenspieler, x - 1, y + ZielRichtung] = true;
                }

                // Teste Schlag nach rechts
                if (y + ZielRichtung >= 0 && y + ZielRichtung < Höhe && x + 1 < Breite && !neuesFeld[Spieler, x + 1, y + ZielRichtung] && neuesFeld[Gegenspieler, x + 1, y + ZielRichtung])
                {
                    // schlage die Figur rechts im NEUEN FELD
                    neuesFeld[Spieler, x, y] = false;
                    neuesFeld[Spieler, x + 1, y + ZielRichtung] = true;
                    neuesFeld[Gegenspieler, x + 1, y + ZielRichtung] = false;
                    int testWert = gewinntSpieler(Gegenspieler, neuesFeld);
                    if (testWert == -1)  // Gegenspieler verliert. Wir geben SIEG zurück.
                    {
                        if (IstErsterZug)
                        {
                            xZugVorher = x;
                            xZugNachher = x + 1;
                            yZugVorher = y;
                            yZugNachher = y + ZielRichtung;
                        }
                        return +1;
                    }
                    if (testWert == 0)  // Situtation wird unentschieden. Der Zug wird gemerkt.
                    {
                        if (IstErsterZug)
                        {
                            xZugVorher = x;
                            xZugNachher = x + 1;
                            yZugVorher = y;
                            yZugNachher = y + ZielRichtung;
                        }
                        // bestesErgebnis = 0;
                        habenUnentschiedenGesehen = true;
                    }
                    if (testWert == 1)
                    {
                        habenNiederlageGesehen = true;
                    }
                    // mache den Zug wieder rückgängig im NEUEN FELD
                    neuesFeld[Spieler, x, y] = true;
                    neuesFeld[Spieler, x + 1, y + ZielRichtung] = false;
                    neuesFeld[Gegenspieler, x + 1, y + ZielRichtung] = true;
                }
            }
        }

        if( habenUnentschiedenGesehen )
        {
            return 0;
        }
        if( habenNiederlageGesehen )
        {
            return -1;
        }
        return 0;
    }

    // Diese Methode soll einen Zug im wirklichen Spiel ausführen
    // wir benutzen das zur Zeit nicht
    private void macheZugImSpiel(int vorherX, int vorherY, int nachherX, int nachherY)
    {
        GameObject FigurVorher = dasSpiel.GetComponent<Game>().GetPosition(vorherX, vorherY);
        GameObject FigurNachher = dasSpiel.GetComponent<Game>().GetPosition(nachherX, nachherY);

        if (FigurNachher != null)
        {
            Destroy(FigurNachher);
        }

        dasSpiel.GetComponent<Game>().SetPositionEmpty(vorherX, vorherY);

        FigurVorher.GetComponent<Chessmen>().SetXBoard(nachherX);
        FigurVorher.GetComponent<Chessmen>().SetYBoard(nachherY);
        FigurVorher.GetComponent<Chessmen>().SetCoords();

        dasSpiel.GetComponent<Game>().SetPosition(FigurVorher);

        dasSpiel.GetComponent<Game>().NextTurn();
    }
}
