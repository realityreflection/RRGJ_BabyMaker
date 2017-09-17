using UnityEngine;
using UnityEngine.Networking;

public class PlayerController : NetworkBehaviour
{
    CommandController myCommands;
    CommandController otherCommands;
    SoundController soundController;
    GameController gameController;

    [SyncVar]
    bool sync_isMale;

    // Use this for initialization
    void Start()
    {
        myCommands = GameObject.FindGameObjectWithTag("MyCommand").GetComponent<CommandController>();
        otherCommands = GameObject.FindGameObjectWithTag("OtherCommand").GetComponent<CommandController>();
        soundController = GameObject.FindGameObjectWithTag("SoundController").GetComponent<SoundController>();
    }

    public void OnGameStart(bool isMale)
    {
        sync_isMale = isMale;
        Rpc_OnGameStart(isMale);
    }

    [ClientRpc]
    void Rpc_OnGameStart(bool isMale)
    {
        if (!isLocalPlayer)
            return;

        myCommands.OnGameStart(this, true, isMale);
        otherCommands.OnGameStart(this, false, !isMale);
        soundController.PlayBGM();
    }

    public void OnTurnStart()
    {
        Rpc_OnTurnStart();
    }

    [ClientRpc]
    public void Rpc_OnTurnStart()
    {
        if (!isLocalPlayer)
            return;

        myCommands.OnTurnStart();
        otherCommands.OnTurnStart();
    }

    public void OnTurnEnd(int selectedCommand, int otherCommand)
    {
        Rpc_OnTurnEnd(selectedCommand, otherCommand);
    }

    [ClientRpc]
    public void Rpc_OnTurnEnd(int selectedCommand, int otherCommand)
    {
        if (!isLocalPlayer)
            return;

        myCommands.OnTurnEnd(selectedCommand);
        otherCommands.OnTurnEnd(otherCommand);
    }

    public void OnMyCommand(int commandIndex)
    {
        if (!isLocalPlayer)
            return;

        soundController.PlayButtonSound(sync_isMale);
        Cmd_OnMyCommand(commandIndex);
    }

    [Command]
    public void Cmd_OnMyCommand(int commandIndex)
    {
        if(!gameController)
            gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();

        gameController.OnPlayerCommand(sync_isMale, commandIndex);
    }

    public void OnOtherCommand(int commandIndex)
    {
        Rpc_OnOtherCommand(commandIndex);
    }

    [ClientRpc]
    public void Rpc_OnOtherCommand(int commandIndex)
    {
        if (!isLocalPlayer)
            return;

        soundController.PlayButtonSound(!sync_isMale);
    }

    [ClientRpc]
    public void Rpc_OnTimesUpWithoutCommand()
    {
        if (!isLocalPlayer)
            return;

        //if no input, set random command
        OnMyCommand(Random.Range(0, 5));
    }
}
