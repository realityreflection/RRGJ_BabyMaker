using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CommandLayer : MonoBehaviour {
    List<Command> Commands;
	// Use this for initialization
	void Start () {
        Commands = GetComponentsInChildren<Command>().ToList();
    }


    public void SetCommandInputEnable(bool isEnable)
    {
        foreach(var command in Commands)
        {
            command.SetInputEnable(isEnable);
        }
    }
}
