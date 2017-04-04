using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CommandLayer : MonoBehaviour {
    List<Command> Commands;
    public int SelectedCmdIdx = -1;

	// Use this for initialization
	void Start () {
        Commands = GetComponentsInChildren<Command>().ToList();
    }

    void SetCommandInputEnable(bool isEnable)
    {
        foreach (var command in Commands)
        {
            command.SetInputEnable(isEnable);
        }
    }

    public void OnTurnStart()
    {
        SetCommandInputEnable(true);
        SelectedCmdIdx = -1;
    }

    public void OnTurnEnd()
    {

    }

    public void OnClickCommand(int commandIdx)
    {
        SelectedCmdIdx = commandIdx;
        SetCommandInputEnable(false);
    }
}
