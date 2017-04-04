using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GSF.Ez;
using GSF.Ez.Packet;
using UnityEngine.SceneManagement;

enum NetworkState
{
    None,
    Ready,
    InGame,
}


public class NetworkWorker
{
    private static EzClient client;
    static NetworkState state = NetworkState.None;
    static GameController gameController = null;

    // Use this for initialization
    public static void Connect()
    {
        if (state != NetworkState.None)
            return;
        state = NetworkState.Ready;

        var rd = (new System.Random()).Next();

        client = EzClient.Connect(
            "ws://localhost:9916",
            rd.ToString(),
            new Dictionary<string, object>()
            {

            });

        client.onWorldInfo += OnWorldInfo;
        client.onJoinPlayer += OnJoinPlayer;
        client.onModifyWorldProperty += OnModifyWorldProperty;

        //client.onCustomPacket += OnReceivePacket;
    }

    public static void OnGameStart(GameController _gameController)
    {
        gameController = _gameController;
    }

    private static void OnWorldInfo(WorldInfo worldInfo)
    {
        //client.SetWorldProperty("score", 1000);
        //client.worldProperty;

        if(client.players.Count > 1)
        {
            SceneManager.LoadScene("InGame");
        }
    }

    private static void OnJoinPlayer(JoinPlayer player)
    {
        if (state != NetworkState.Ready)
            return;

        var id = player.Player.PlayerId;
        SceneManager.LoadScene("InGame");
    }

    private static void OnModifyWorldProperty(ModifyWorldProperty packet)
    {
        //client.worldProperty;
        if(gameController)
        {
            //gameController.
        }
    }

    private static void OnModifyPlayerProperty(ModifyPlayerProperty packet)
    {
    }

    private static void OnReceivePacket(BroadcastPacket packet)
    {
        var senderId = packet.Sender.PlayerId;
        var packetData = packet.Data;
    }
}

public class GameModel
{
    public bool isTurnEnd = false;
    public int maxTurnCount = 20;
    public int currentTurnCount = 0;
    public float score = 100;
    public int rootSelectedCmdIndex = -1;
    public int guestSelectedCmdIndex = -1;
    public bool rootReady = false;
    public bool guestReady = false;
}