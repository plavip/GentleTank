﻿using UnityEngine;

public class GameInformation : MonoBehaviour
{
    //static public GameInformation Instance { get; private set; }
    static private GameInformation instance;
    static public GameInformation Instance
    {
        get
        {
            if (instance == null)
            {
                GameInformation gameInformation = FindObjectOfType<GameInformation>();
                if (gameInformation == null)
                {
                    gameInformation = new GameObject("GameInformation").AddComponent<GameInformation>();
                    gameInformation.players = Resources.Load<AllPlayerManager>("AllPlayerManager");
                }
                instance = gameInformation;
            }
            return instance;
        }
        private set { instance = value; }
    }

    public AllPlayerManager players;

    private void Awake()
    {
        Instance = this;
        players.SetupInstance();
        DontDestroyOnLoad(this);
    }

}