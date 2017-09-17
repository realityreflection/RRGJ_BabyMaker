using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MatchInfo
{
    public bool IsSuccess { get; set; }
    public string SkillName { get; set; }
}

public class GameController : NetworkBehaviour
{
    public int TurnScore = 20;
    public float TurnTime = 20;
    public int MaxTurnCount = 10;

    PlayerController malePlayer;
    PlayerController femalePlayer;
    GameViewController gameView;

    int currentTurn = 0;
    int currentScore = 100;
    float currentTime = 0;
    bool isTurnOver = true;

    int maleCommand = -1;
    int femaleCommand = -1;

    // Use this for initialization
    void Start ()
    {
        StartCoroutine(DelayGameStart());
    }

    void GameStart()
    {
        bool ladyFirst = (Random.Range(-1, 1) >= 0.0);
        int maleIndex = ladyFirst ? 1 : 0;
        int femaleIndex = ladyFirst ? 0 : 1;

        gameView = GameObject.FindGameObjectWithTag("GameView").GetComponent<GameViewController>();
        var players = GameObject.FindGameObjectsWithTag("Player");
        malePlayer = players[maleIndex].GetComponent<PlayerController>();
        femalePlayer = players[femaleIndex].GetComponent<PlayerController>();

        gameView.Rpc_OnGameStart();
        malePlayer.OnGameStart(true);
        femalePlayer.OnGameStart(false);

        StartCoroutine(DelayFirstStart());
    }

    void TurnStart()
    {
        if (CheckGameEnd())
            return;

        isTurnOver = false;
        maleCommand = -1;
        femaleCommand = -1;

        gameView.Rpc_OnTurnStart(currentTurn, MaxTurnCount);
        malePlayer.OnTurnStart();
        femalePlayer.OnTurnStart();

        StartCoroutine(TurnTick());
    }

    public void OnPlayerCommand(bool isMale, int selectedCommandIndex)
    {
        if (isMale)
        {
            maleCommand = selectedCommandIndex;
            femalePlayer.OnOtherCommand(selectedCommandIndex);
        }
        else
        {
            femaleCommand = selectedCommandIndex;
            malePlayer.OnOtherCommand(selectedCommandIndex);
        }

        CheckTurnEnd();
    }

    void TurnEnd()
    {
        isTurnOver = true;
        StartCoroutine(DelayCalculatMatchResult());
        malePlayer.OnTurnEnd(maleCommand, femaleCommand);
        femalePlayer.OnTurnEnd(femaleCommand, maleCommand);
    }

    void CheckTurnEnd()
    {
        if(maleCommand != -1 && femaleCommand != -1)
        {
            TurnEnd();
        }
    }

    void CalculateMatchResult()
    {
        int matchIndex = maleCommand * 5 + femaleCommand;
        var matchInfo = matchTable[matchIndex];
        int deltaDir = matchInfo.IsSuccess ? 1 : -1;
        currentScore += deltaDir * TurnScore;
        currentTurn++;

        gameView.Rpc_ShowMatchResult(matchIndex, matchInfo.IsSuccess, matchInfo.SkillName, currentScore, currentTurn, MaxTurnCount);

        StartCoroutine(DelayTurnStart());
    }

    bool CheckGameEnd()
    {
        if (currentTurn >= MaxTurnCount || currentScore <= 0)
        {
            gameView.Rpc_ShowGameResult(currentScore >= 100, currentScore);
            return true;
        }
        return false;
    }


    IEnumerator DelayGameStart()
    {
        yield return new WaitForSeconds(1.0f);
        GameStart();
    }

    IEnumerator DelayFirstStart()
    {
        yield return new WaitForSeconds(3.0f);
        TurnStart();
    }

    IEnumerator DelayTurnStart()
    {
        yield return new WaitForSeconds(5.0f);
        TurnStart();
    }

    IEnumerator DelayCalculatMatchResult()
    {
        yield return new WaitForSeconds(1.0f);
        CalculateMatchResult();
    }

    IEnumerator TurnTick()
    {
        currentTime = 0.0f;
        while (!isTurnOver && currentTime <= TurnTime)
        {
            currentTime += 0.5f;
            gameView.Rpc_UpdateTime(currentTime / TurnTime);
            yield return new WaitForSeconds(0.5f);
        }

        if (maleCommand == -1)
        {
            malePlayer.Rpc_OnTimesUpWithoutCommand();
        }

        if (femaleCommand == -1)
        {
            femalePlayer.Rpc_OnTimesUpWithoutCommand();
        }
    }


    MatchInfo[] matchTable =
    {
        new MatchInfo{ IsSuccess = true, SkillName = "나무젓가락 쪼개기" },
        new MatchInfo{ IsSuccess = true, SkillName = "뒤덮치기" },
        new MatchInfo{ IsSuccess = false, SkillName = ""},
        new MatchInfo{ IsSuccess = false, SkillName = "" },
        new MatchInfo{ IsSuccess = false, SkillName = "" },

        new MatchInfo{ IsSuccess = true, SkillName = "허리들기" },
        new MatchInfo{ IsSuccess = true, SkillName = "개싸움" },
        new MatchInfo{ IsSuccess = false, SkillName = "" },
        new MatchInfo{ IsSuccess = false, SkillName = "" },
        new MatchInfo{ IsSuccess = false, SkillName = "" },

        new MatchInfo{ IsSuccess = false, SkillName = "" },
        new MatchInfo{ IsSuccess = false, SkillName = "" },
        new MatchInfo{ IsSuccess = true, SkillName = "절구찧기" },
        new MatchInfo{ IsSuccess = true, SkillName = "레이디스 머신" },
        new MatchInfo{ IsSuccess = false, SkillName = "" },

        new MatchInfo{ IsSuccess = false, SkillName = "" },
        new MatchInfo{ IsSuccess = false, SkillName = "" },
        new MatchInfo{ IsSuccess = false, SkillName = "" },
        new MatchInfo{ IsSuccess = true, SkillName = "불륜매미" },
        new MatchInfo{ IsSuccess = true, SkillName = "북치기" },

        new MatchInfo{ IsSuccess = false, SkillName = "" },
        new MatchInfo{ IsSuccess = false, SkillName = "" },
        new MatchInfo{ IsSuccess = true, SkillName = "스테이션 런치" },
        new MatchInfo{ IsSuccess = false, SkillName = "" },
        new MatchInfo{ IsSuccess = true, SkillName = "잔디깎이" },
    };
}
