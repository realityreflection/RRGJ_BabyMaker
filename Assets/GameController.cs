using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

    public float TurnScore = 20;
    public float TurnTime = 20;

    public GameGauge ScoreGauge;
    public Slider TimerSlider;
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


    float currentScore = 100;
    float currentTime = 0;
    bool isTurnEnd = false;

    // Use this for initialization
    void Start()
    {
        SkillText.enabled = false;
        FailedText.enabled = false;
        MyCmdLayer.GameController = this;
        OpponentCmdLayer.GameController = this;

        StartCoroutine(FirstStart());
    }

    public void CommandResult(int manCmdIdx, int womanCmdIdx)
    {
        int combiIdx = womanCmdIdx * 5 + manCmdIdx;
        var combiData = combinationData[combiIdx];
        SexPanel.SetPos(combiIdx);

        if (combiData.IsSuccess)
        {
            SkillText.text = combiData.SkillName;
        }

        FailedText.enabled = !combiData.IsSuccess;
        SkillText.enabled = combiData.IsSuccess;

        float deltaDir = combiData.IsSuccess ? 1 : -1;
        currentScore += deltaDir * TurnScore;
        ScoreGauge.SetSliderValue(currentScore);
    }

    public void TryToTurnEnd()
    {
        if(MyCmdLayer.SelectedCmdIdx * OpponentCmdLayer.SelectedCmdIdx > 0)
        {
            OnTurnEnd();
        }
    }

    void OnTurnStart()
    {
        isTurnEnd = false;
        MyCmdLayer.OnTurnStart();
        OpponentCmdLayer.OnTurnStart();
        StartCoroutine(StartTurn());
    }

    void OnTurnEnd()
    {
        MyCmdLayer.OnTurnEnd();
        OpponentCmdLayer.OnTurnEnd();
        CommandResult(MyCmdLayer.SelectedCmdIdx, OpponentCmdLayer.SelectedCmdIdx);
        StartCoroutine(EndTurn());
    }

    IEnumerator FirstStart()
    {
        yield return new WaitForSeconds(1.0f);
        OnTurnStart();
    }

    IEnumerator StartTurn()
    {
        currentTime = 0.0f;
        while(currentTime < TurnTime && !isTurnEnd)
        {
            currentTime += 0.5f;
            TimerSlider.normalizedValue = currentTime / TurnTime;
            yield return new WaitForSeconds(0.5f);
        }
        if(OpponentCmdLayer.SelectedCmdIdx < 0)
        {
            OpponentCmdLayer.OnClickCommand(Random.Range(0, 4));
        }
        
        if(MyCmdLayer.SelectedCmdIdx < 0)
        {
            MyCmdLayer.OnClickCommand(Random.Range(0, 4));
        }

        OnTurnEnd();
    }

    IEnumerator EndTurn()
    {
        yield return new WaitForSeconds(3.0f);
        OnTurnStart();
    }
}


public class CombinationData
{
    public bool IsSuccess { get; set; }
    public string SkillName { get; set; }
}