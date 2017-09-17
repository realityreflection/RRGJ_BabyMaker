using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CommandButton : MonoBehaviour
{
    public int CommandIndex;

    CommandController controller;
    Button button;
    Text text;
    int cooldownedTurn = 0;
    bool isOwned = false;

	// Use this for initialization
	void Start ()
    {
        button = GetComponentInChildren<Button>();
        text = GetComponentInChildren<Text>();
        controller = GetComponentInParent<CommandController>();
        text.enabled = false;
    }

    public void Init(bool isMale, int index, bool _isOwned)
    {
        var spriteName = string.Format("Button/Bt_{0}_0{1}", isMale ? "M" : "W", index + 1);
        var sprite = Resources.Load<Sprite>(spriteName);
        isOwned = _isOwned;
        button.image.sprite = sprite;
        button.interactable = isOwned;
    }

    public void SetInputEnable(bool isEnable)
    {
        button.interactable = isEnable;
    }

    public void OnClickButton()
    {
        controller.OnClickCommand(CommandIndex);
    }

    public void SetCooldown()
    {
        cooldownedTurn = 1;
        text.enabled = true;
    }

    public void OnTurnStart()
    {
        if(cooldownedTurn > 0)
        {
            cooldownedTurn--;
            text.enabled = true;

        }
        else
        {
            text.enabled = false;
            button.interactable = isOwned;
        }
    }

    public void OnTurnEnd()
    {
        button.interactable = false;
    }
}