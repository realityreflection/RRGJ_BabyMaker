using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

    public float TurnValue = 20;
    public GameGauge gauge;
    public Slider timerSlider;
    public Text SkillText;
    public Text FailedText;
    public SexPanel SexPanel;
    public CommandLayer MyCmdLayer;
    public CommandLayer OpponentCmdLayer;

    CombinationData[] combinationData = {
        new CombinationData{ IsSuccess = true, SkillName = "나무젓가락 쪼개기" },
        new CombinationData{ IsSuccess = true, SkillName = "뒤덮치기" },
        new CombinationData{ IsSuccess = false, SkillName = ""},
        new CombinationData{ IsSuccess = false, SkillName = "" },
        new CombinationData{ IsSuccess = false, SkillName = "" },

        new CombinationData{ IsSuccess = true, SkillName = "허리들기" },
        new CombinationData{ IsSuccess = true, SkillName = "개싸움" },
        new CombinationData{ IsSuccess = false, SkillName = "" },
        new CombinationData{ IsSuccess = false, SkillName = "" },
        new CombinationData{ IsSuccess = false, SkillName = "" },

        new CombinationData{ IsSuccess = false, SkillName = "" },
        new CombinationData{ IsSuccess = false, SkillName = "" },
        new CombinationData{ IsSuccess = true, SkillName = "절구찧기" },
        new CombinationData{ IsSuccess = true, SkillName = "레이디스 머신" },
        new CombinationData{ IsSuccess = false, SkillName = "" },

        new CombinationData{ IsSuccess = false, SkillName = "" },
        new CombinationData{ IsSuccess = false, SkillName = "" },
        new CombinationData{ IsSuccess = false, SkillName = "" },
        new CombinationData{ IsSuccess = true, SkillName = "불륜매미" },
        new CombinationData{ IsSuccess = true, SkillName = "북치기" },

        new CombinationData{ IsSuccess = false, SkillName = "" },
        new CombinationData{ IsSuccess = false, SkillName = "" },
        new CombinationData{ IsSuccess = true, SkillName = "스테이션 런치" },
        new CombinationData{ IsSuccess = false, SkillName = "" },
        new CombinationData{ IsSuccess = true, SkillName = "잔디깎이" },
    };

    // Use this for initialization
    void Start ()
    {
        SkillText.enabled = false;
        FailedText.enabled = false;
    }
	

    public void CommandResult(int manCmdIdx, int womanCmdIdx)
    {
        int combiIdx = womanCmdIdx * 5 + manCmdIdx;
        var combiData = combinationData[combiIdx];
        SexPanel.SetPos(combiIdx);

        if(combiData.IsSuccess)
        {
            SkillText.text = combiData.SkillName;
        }

        FailedText.enabled = !combiData.IsSuccess;
        SkillText.enabled = combiData.IsSuccess;

        float deltaScore
    }
}


public class CombinationData
{
    public bool IsSuccess { get; set; }
    public string SkillName { get; set; }
}