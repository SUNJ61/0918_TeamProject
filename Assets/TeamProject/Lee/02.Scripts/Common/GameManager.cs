using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager G_instance;

    public bool isGameover = false;
    void Awake()
    {
        if (G_instance == null)
            G_instance = this;
        else if (G_instance != this)
            Destroy(G_instance);
        DontDestroyOnLoad(G_instance);
    }
}
