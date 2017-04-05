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
    static string playerId;
    static NetworkState state = NetworkState.None;
    static GameController gameController = null;
    static bool IsHost = false;

    // Use this for initialization
    public static void Connect()
    {
        if (state != NetworkState.None)
            return;
        state = NetworkState.Ready;

        playerId = (new System.Random()).Next().ToString();

        client = EzClient.Connect(
            "ws://192.168.1.13:9916",
            playerId,
            new Dictionary<string, object>()
            {

            });

        client.onWorldInfo += OnWorldInfo;
        client.onJoinPlayer += OnJoinPlayer;
        client.onModifyWorldProperty += OnModifyWorldProperty;
        client.onModifyPlayerProperty += OnModifyPlayerProperty;
        client.onDesignatedRootPlayer += OnDesignatedRootPlayer; 
    }

    public static void Disconnect()
    {
        client.Disconnect();
        state = NetworkState.None;
        client = null;
        gameController = null;
        IsHost = false;
    }

    public static void OnGameStart(GameController _gameController)
    {
        if (state != NetworkState.Ready)
            return;

        state = NetworkState.InGame;
        gameController = _gameController;
    }

    public static void RequestModifyGameModel(GameModel model)
    {
        if (state != NetworkState.InGame)
            return;

        client.SetWorldProperty("GameModel", model.ToEzObject());
    }

    public static void RequestModifyPlayerModel(PlayerModel model)
    {
        if (state != NetworkState.InGame)
            return;

        client.SetPlayerProperty("PlayerModel", model.ToEzObject());
    }

    public static bool IsRootPlayer() { return client.isRootPlayer; }

    public static GameModel GetGameModel() { return client.worldProperty["GameModel"].ToGameObject<GameModel>(); }

    private static void OnWorldInfo(WorldInfo worldInfo)
    {
        //client.SetWorldProperty("score", 1000);
        //client.worldProperty;
        IsHost = client.isRootPlayer;
        if (!IsHost)
        {
            SceneManager.LoadScene("InGame");
        }
    }

    private static void OnJoinPlayer(JoinPlayer player)
    {
        if (state != NetworkState.Ready)
            return;

        SceneManager.LoadScene("InGame");
    }

    private static void OnModifyWorldProperty(ModifyWorldProperty packet)
    {
        //client.worldProperty;
        var newModel = packet.Property["GameModel"].ToGameObject<GameModel>();
        if(newModel != null && gameController != null && state == NetworkState.InGame)
        {
            gameController.OnGameModelUpdate(newModel);
        }
    }

    private static void OnModifyPlayerProperty(ModifyPlayerProperty packet)
    {
        var playerModel = packet.Property["PlayerModel"].ToGameObject<PlayerModel>();
        if (playerModel != null && gameController != null && state == NetworkState.InGame)
        {
            bool isMe = packet.Player.PlayerId == playerId;
            gameController.OnPlayerModelUpdate(playerModel, isMe);
        }
    }

    private static void OnDesignatedRootPlayer()
    {
        if (!IsHost)
            SceneManager.LoadScene("InGame");
    }

    private static void OnReceivePacket(BroadcastPacket packet)
    {
    }
}

public class GameModel
{
    public bool isTurnEnd = false;
    public int maxTurnCount = 2;
    public int currentTurnCount = 0;
    public float score = 100;
}

public class PlayerModel
{
    public int selectedCommandIndex = -1;
    public bool isReady = false;
}