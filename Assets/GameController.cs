using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

    public GameGauge gauge;
    public Slider timerSlider;
    public Text SkillText;
    public Text FailedText;
    public SexPanel SexPanel;
    public CommandLayer MyCmdLayer;
    public CommandLayer OpponentCmdLayer;

    // Use this for initialization
    void Start ()
    {
        SkillText.enabled = false;
        FailedText.enabled = false;
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}


}
