using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkController : NetworkLobbyManager
{
    bool isPlaying = false;
    // Use this for initialization
    void Start()
    {
        string[] args = System.Environment.GetCommandLineArgs();
        for (int i = 0; i < args.Length; i++)
        {
            if (args[i] == "-server")
                GetComponent<NetworkManager>().StartServer();
        }
    }

    public override void OnLobbyServerConnect(NetworkConnection conn)
    {
        base.OnLobbyServerConnect(conn);
        CheckReadyToBegin();
    }

    public override void OnLobbyServerDisconnect(NetworkConnection conn)
    {
        base.OnLobbyServerDisconnect(conn);
        if (isPlaying)
        {
            StopServer();
            Application.Quit();
        }
    }
    
    public override void OnLobbyServerSceneChanged(string sceneName)
    {
        isPlaying = sceneName == playScene;
    }

}

