using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager G_instance;
    [SerializeField] Text Candle_text;



    public int CandleCounter;
    public bool isGameover = false;
    void Awake()
    {
        if (G_instance == null)
            G_instance = this;
        else if (G_instance != this)
            Destroy(G_instance);
        DontDestroyOnLoad(G_instance);



        Candle_text = GameObject.Find("PlayerUi").transform.GetChild(3).GetChild(0).GetChild(0).GetComponent<Text>();
    }


    public void CanndleCounter(int counter)
    {
        CandleCounter += counter;
        Candle_text.text = $"{CandleCounter.ToString()}/6";
        print(CandleCounter);
    }

}
