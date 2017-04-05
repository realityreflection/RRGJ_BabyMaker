using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public GameModel gameModel;
    public float TurnScore = 20;
    public float TurnTime = 20;
    public int maxTurnCount = 2;

    public GameGauge ScoreGauge;
    public Slider TimerSlider;
    public Text SkillText;
    public Text FailedText;
    public Text TurnText;
    public SexPanel SexPanel;
    public PlayerComponent MyPlayer;
    public PlayerComponent OtherPlayer;
    public SoundController SoundController;
    public ResultComponent ResultComponent;
    public GameObject GameStartEffect;

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

    int currentTurn = 0;
    float currentScore = 100;
    float currentTime = 0;
    bool isTurnEnd = true;

    // Use this for initialization
    void Start()
    {
        GameStartEffect.SetActive(true);
        SkillText.enabled = false;
        FailedText.enabled = false;
        StartCoroutine(FirstStart());
    }

    public void OnGameModelUpdate(GameModel model)
    {
        currentTurn = model.currentTurnCount;
        isTurnEnd = model.isTurnEnd;
        maxTurnCount = model.maxTurnCount;
        currentScore = model.score;

        if(isTurnEnd)
        {
            OnTurnEnd();
        }
        else
        {
            OnTurnStart();
        }
    }

    public void OnPlayerModelUpdate(PlayerModel newPlayerModel, bool isMe)
    {
        if(isMe)
            MyPlayer.UpdateWithModel(newPlayerModel);
        else
            OtherPlayer.UpdateWithModel(newPlayerModel);

        if(isTurnEnd)
        {
            CheckTurnStart();
        }
        else
        {
            CheckTurnEnd();
        }
    }

    void CheckTurnStart()
    {
        bool isToStart = MyPlayer.isReady && OtherPlayer.isReady;
        if(isToStart && NetworkWorker.IsRootPlayer())
        {
            var model = new GameModel();
            model.currentTurnCount = currentTurn;
            model.isTurnEnd = false;
            model.maxTurnCount = maxTurnCount;
            model.score = currentScore;

            NetworkWorker.RequestModifyGameModel(model);
        }
    }

    void CheckTurnEnd()
    {
        bool isToEnd = (MyPlayer.selectedCmdIdx > -1 && OtherPlayer.selectedCmdIdx > -1);
        if (isToEnd && NetworkWorker.IsRootPlayer())
        {
            //game model update
            int manCmdIdx = MyPlayer.isMale ? MyPlayer.selectedCmdIdx : OtherPlayer.selectedCmdIdx;
            int womanCmdIdx = OtherPlayer.isMale ? MyPlayer.selectedCmdIdx : OtherPlayer.selectedCmdIdx;
            int combiIdx = manCmdIdx * 5 + womanCmdIdx;
            var combiData = combinationData[combiIdx];
            float deltaDir = combiData.IsSuccess ? 1 : -1;
            currentScore += deltaDir * TurnScore;
            currentTurn++;

            var model = new GameModel();
            model.currentTurnCount = currentTurn;
            model.isTurnEnd = true;
            model.maxTurnCount = maxTurnCount;
            model.score = currentScore;

            NetworkWorker.RequestModifyGameModel(model);
        }
    }

    bool CheckGameEnd()
    {
        if(currentTurn >= maxTurnCount || currentScore <= 0)
        {
            ResultComponent.ShowResult(currentScore >= 100, (int)currentScore);
            return true;
        }
        return false;
    }

    public void UpdateResult(int manCmdIdx, int womanCmdIdx)
    {
        int combiIdx = manCmdIdx * 5 + womanCmdIdx;
        var combiData = combinationData[combiIdx];
        SexPanel.SetPos(combiIdx);

        if (combiData.IsSuccess)
        {
            SkillText.text = combiData.SkillName;
        }

        FailedText.enabled = !combiData.IsSuccess;
        SkillText.enabled = combiData.IsSuccess;

        TurnText.text = string.Format("{0} / {1}", currentTurn, maxTurnCount);

        ScoreGauge.SetSliderValue(currentScore);
        SoundController.PlayPosSound(combiIdx);
    }

    public void OnGameEnd()
    {
        NetworkWorker.Disconnect();
        SceneManager.LoadScene("Intro");
    }

    void OnTurnStart()
    {
        GameStartEffect.SetActive(false);

        SexPanel.Reset();
        SoundController.StopPosSound();

        SkillText.enabled = false;
        FailedText.enabled = false;
        TurnText.text = string.Format("{0} / {1}", currentTurn, maxTurnCount);

        if (CheckGameEnd())
            return;

        MyPlayer.OnTurnStart();
        OtherPlayer.OnTurnStart();
        StartCoroutine(StartTurn());
    }

    void OnTurnEnd()
    {
        StartCoroutine(DelayedTurnEndEffect());

        MyPlayer.OnTurnEnd();
        OtherPlayer.OnTurnEnd();
    }

    IEnumerator FirstStart()
    {
        yield return new WaitForSeconds(2.0f);
        NetworkWorker.OnGameStart(this);
        MyPlayer.Init(true);
        OtherPlayer.Init(false);
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
        if(OtherPlayer.selectedCmdIdx < 0)
        {
            OtherPlayer.OnClickCommand(Random.Range(0, 4));
        }
        
        if(MyPlayer.selectedCmdIdx < 0)
        {
            MyPlayer.OnClickCommand(Random.Range(0, 4));
        }
    }

    IEnumerator DelayedTurnEndEffect()
    {
        yield return new WaitForSeconds(0.5f);
        int manCmdIdx = MyPlayer.isMale ? MyPlayer.selectedCmdIdx : OtherPlayer.selectedCmdIdx;
        int womanCmdIdx = OtherPlayer.isMale ? MyPlayer.selectedCmdIdx : OtherPlayer.selectedCmdIdx;
        UpdateResult(manCmdIdx, womanCmdIdx);
    }
}


public class CombinationData
{
    public bool IsSuccess { get; set; }
    public string SkillName { get; set; }
}