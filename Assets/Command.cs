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

	// Use this for initialization
	void Start ()
    {
        button = GetComponentInChildren<Button>();
        text = GetComponentInChildren<Text>();
        parentLayer = GetComponentInParent<CommandLayer>();
        text.enabled = false;
    }

    public void SetCoolDown(bool isCoolDown)
    {
        button.interactable = isCoolDown;
        text.enabled = isCoolDown;
    }

    public void SetInputEnable(bool isEnable)
    {
        button.interactable = isEnable;
    }

    public void OnClickButton()
    {
        parentLayer.OnClickCommand(CommandIndex);
    }
}