using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultPopup : MonoBehaviour
{
    public GameObject Button;
    public Image PanelBg;
    public Text IsSuccessText;
    public Text ScoreText;
    public Text ScoreLabelText;

    bool isSuccess = false;
    int score = 0;

    void Start()
    {
        IsSuccessText.enabled = false;
        ScoreText.enabled = false;
        ScoreLabelText.enabled = false;
    }


    public void Show(bool _isSuccess, int _score)
    {
        enabled = true;
        isSuccess = _isSuccess;
        score = _score;
        StartCoroutine(Showing());
    }

    void ShowStuff()
    {
        IsSuccessText.text = isSuccess ? "BABY SUCCESS!" : "BABY FAILED...";
        ScoreText.text = score.ToString();

        IsSuccessText.enabled = true;
        ScoreText.enabled = true;
        ScoreLabelText.enabled = true;
        Button.SetActive(true);
    }



    IEnumerator Showing()
    {
        float totalTime = 1.0f;
        float accTIme = 0.0f;
        while (accTIme < totalTime)
        {
            PanelBg.color = new Color(PanelBg.color.r, PanelBg.color.g, PanelBg.color.b, accTIme / totalTime);
            accTIme += 0.1f;
            yield return new WaitForSeconds(0.1f);
        }

        ShowStuff();
    }
}
