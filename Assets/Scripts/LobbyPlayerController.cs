using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class LobbyPlayerController : NetworkLobbyPlayer
{ 
    public override void OnClientEnterLobby()
    {
        base.OnClientEnterLobby();
        StartCoroutine(DelayReady());
    }

    IEnumerator DelayReady()
    {
        yield return new WaitForSeconds(1.0f);
        readyToBegin = true;
        SendReadyToBeginMessage();
    }
}
