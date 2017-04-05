using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerComponent : MonoBehaviour
{
    List<Command> Commands;
    public GameController GameController;
    public int selectedCmdIdx = -1;
    public bool isReady = false;
    public bool isMale = false;
    bool isMe = false;

	// Use this for initialization
	void Start () {
        Commands = GetComponentsInChildren<Command>().ToList();
        SetCommandInputEnable(false);
    }

    public void Init(bool _isMe)
    {
        isMe = _isMe;
        isMale = !(NetworkWorker.IsRootPlayer() ^ isMe);
        for (int i = 0; i < Commands.Count; ++i)
        {
            Commands[i].Init(isMale, i, isMe);
        }

        if(isMe)
        {
            var model = new PlayerModel();
            model.IsHost = NetworkWorker.IsRootPlayer();
            model.isReady = true;
            model.selectedCommandIndex = -1;
            NetworkWorker.RequestModifyPlayerModel(model);
        }
    }

    public void UpdateWithModel(PlayerModel model)
    {
        selectedCmdIdx = model.selectedCommandIndex;
        isReady = model.isReady;
    }


    public void SetCommandInputEnable(bool isEnable)
    {
        foreach (var command in Commands)
        {
            command.SetInputEnable(isEnable);
        }
    }

    public void OnTurnStart()
    {
        foreach (var command in Commands)
        {
            command.OnTurnStart(isMe);
        }
        if (isMe)
        {
            var model = new PlayerModel();
            model.IsHost = NetworkWorker.IsRootPlayer();
            model.isReady = false;
            model.selectedCommandIndex = -1;
            NetworkWorker.RequestModifyPlayerModel(model);
        }
    }

    public void OnTurnEnd()
    {
        foreach (var command in Commands)
        {
            command.OnTurnEnd(isMe);
        }
        Commands[selectedCmdIdx].SetCooldown();
        if (isMe)
        {
            StartCoroutine(DelayForReady());
        }
    }

    public void OnClickCommand(int commandIdx)
    {
        GameController.SoundController.PlayButtonSound(isMale);
        SetCommandInputEnable(false);
        if (isMe)
        {
            var model = new PlayerModel();
            model.IsHost = NetworkWorker.IsRootPlayer();
            model.isReady = false;
            model.selectedCommandIndex = commandIdx;
            NetworkWorker.RequestModifyPlayerModel(model);
        }
    }

    IEnumerator DelayForReady()
    {
        yield return new WaitForSeconds(5.0f);
        var model = new PlayerModel();
        model.IsHost = NetworkWorker.IsRootPlayer();
        model.isReady = true;
        model.selectedCommandIndex = -1;
        NetworkWorker.RequestModifyPlayerModel(model);
    }
}
