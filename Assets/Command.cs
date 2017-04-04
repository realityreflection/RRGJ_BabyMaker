using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Command : MonoBehaviour
{
    public bool IsMaleCommand;
    public int CommandIndex;

    CommandLayer parentLayer;
    Button button;
    Text text;
    int CooldownedTurn = 0;

	// Use this for initialization
	void Start ()
    {
        button = GetComponentInChildren<Button>();
        text = GetComponentInChildren<Text>();
        parentLayer = GetComponentInParent<CommandLayer>();
        text.enabled = false;

    }

    public void SetInputEnable(bool isEnable)
    {
        button.interactable = isEnable;
    }

    public void OnClickButton()
    {
        parentLayer.OnClickCommand(CommandIndex);
        CooldownedTurn = 1;
    }

    public void OnTurnStart()
    {
        if(CooldownedTurn > 0)
        {
            CooldownedTurn--;
            text.enabled = true;

        }
        else
        {
            text.enabled = false;
            button.interactable = true;
        }
    }

    public void OnTurnEnd()
    {
        button.interactable = false;
    }
}