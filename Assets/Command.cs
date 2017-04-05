using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Command : MonoBehaviour
{
    public bool IsMaleCommand;
    public int CommandIndex;

    PlayerComponent parentLayer;
    Button button;
    Text text;
    int CooldownedTurn = 0;

	// Use this for initialization
	void Start ()
    {
        button = GetComponentInChildren<Button>();
        text = GetComponentInChildren<Text>();
        parentLayer = GetComponentInParent<PlayerComponent>();
        text.enabled = false;
    }

    public void Init(bool isMale, int index, bool isMe)
    {
        var spriteName = string.Format("Button/Bt_{0}_0{1}", isMale ? "M" : "W", index + 1);
        var sprite = Resources.Load<Sprite>(spriteName);
        button.image.sprite = sprite;
        button.interactable = isMe;
    }

    public void SetInputEnable(bool isEnable)
    {
        button.interactable = isEnable;
    }

    public void OnClickButton()
    {
        parentLayer.OnClickCommand(CommandIndex);
    }

    public void SetCooldown()
    {
        CooldownedTurn = 1;
        text.enabled = true;
    }

    public void OnTurnStart(bool isMe)
    {
        if(CooldownedTurn > 0)
        {
            CooldownedTurn--;
            text.enabled = true;

        }
        else
        {
            text.enabled = false;
            button.interactable = isMe;
        }
    }

    public void OnTurnEnd(bool isMe)
    {
        button.interactable = false;
    }
}