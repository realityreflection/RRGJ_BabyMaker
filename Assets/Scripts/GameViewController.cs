using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class GameViewController : NetworkBehaviour
{
    public GameGauge ScoreGauge;
    public Slider TimerSlider;
    public Text SkillText;
    public Text FailedText;
    public Text TurnText;
    public SexPanel SexPanel;
    public ResultPanel ResultPanel;
    public SoundController SoundController;
    public GameObject GameStartEffect;

    void Start()
    {
        SkillText.enabled = false;
        FailedText.enabled = false;
    }

    [ClientRpc]
    public void Rpc_OnGameStart()
    {
        GameStartEffect.SetActive(true);
    }
    
    [ClientRpc]
    public void Rpc_ShowMatchResult(int matchIndex, bool isSuccess, string skillName, int currentScore, int currentTurn, int maxTurn)
    {
        SexPanel.SetPos(matchIndex);

        if (isSuccess)
        {
            SkillText.text = skillName;
        }

        FailedText.enabled = !isSuccess;
        SkillText.enabled = isSuccess;

        TurnText.text = string.Format("{0} / {1}", currentTurn, maxTurn);

        ScoreGauge.SetSliderValue(currentScore);
        SoundController.PlayPosSound(matchIndex);
    }

    [ClientRpc]
    public void Rpc_ShowGameResult(bool isSuccess, int score)
    {
        ResultPanel.ShowResult(isSuccess, score);
    }
    
    [ClientRpc]
    public void Rpc_OnTurnStart(int currentTurnCount, int maxTurn)
    {
        GameStartEffect.SetActive(false);

        SexPanel.Reset();
        SoundController.StopPosSound();

        SkillText.enabled = false;
        FailedText.enabled = false;
        TurnText.text = string.Format("{0} / {1}", currentTurnCount, maxTurn);
    }
    

    [ClientRpc]
    public void Rpc_UpdateTime(float normalizedTime)
    {  
        TimerSlider.normalizedValue = normalizedTime;
    }

    public void OnGameEndButtonClick()
    {
        GameObject.FindGameObjectWithTag("NetworkManager").GetComponent<NetworkLobbyManager>().StopClient();
    }
}