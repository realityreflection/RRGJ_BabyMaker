using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Command : MonoBehaviour
{
    public Sprite Image;
    public bool IsMaleCommand;
    public int CommandIndex;

    Button button;
    Text text;
	// Use this for initialization
	void Start ()
    {
        var buttonImage = GetComponent<Image>();
        foreach (var image in GetComponentsInChildren<Image>())
        {
            if(image != buttonImage)
            {
                image.sprite = Image;
                break;
            }
        }

        button = GetComponentInChildren<Button>();
        text = GetComponentInChildren<Text>();

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
}