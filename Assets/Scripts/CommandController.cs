using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CommandController : MonoBehaviour
{
    List<CommandButton> commands;
    PlayerController playerController;
    bool isOwned;
	// Use this for initialization
	void Start ()
    {
        commands = GetComponentsInChildren<CommandButton>().ToList();
    }

    public void OnGameStart(PlayerController _playerController, bool _isOwned, bool isMale)
    {
        playerController = _playerController;
        isOwned = _isOwned;
        for (int i = 0; i < commands.Count; ++i)
        {
            commands[i].Init(isMale, i, isOwned);
        }
    }


    public void OnTurnStart()
    {
        foreach (var command in commands)
        {
            command.OnTurnStart();
        }
    }

    public void OnTurnEnd(int selectedCmdIdx)
    {
        foreach (var command in commands)
        {
            command.OnTurnEnd();
        }
        commands[selectedCmdIdx].SetCooldown();
    }

    public void OnClickCommand(int commandIdx)
    {
        foreach (var command in commands)
        {
            command.SetInputEnable(false);
        }
        playerController.Cmd_OnMyCommand(commandIdx);
    }

}
