using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CommandLayer : MonoBehaviour
{
    List<Command> Commands;
    public GameController GameController;
    public int SelectedCmdIdx = -1;

	// Use this for initialization
	void Start () {
        Commands = GetComponentsInChildren<Command>().ToList();
        SetCommandInputEnable(false);
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
            command.OnTurnStart();
        }
        SelectedCmdIdx = -1;
    }

    public void OnTurnEnd()
    {
        foreach (var command in Commands)
        {
            command.OnTurnEnd();
        }
    }

    public void OnClickCommand(int commandIdx)
    {
        SelectedCmdIdx = commandIdx;
        SetCommandInputEnable(false);
        GameController.TryToTurnEnd();
    }
}
