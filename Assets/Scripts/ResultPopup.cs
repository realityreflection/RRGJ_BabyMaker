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
    }


    public void Show(bool _isSuccess, int _score)
    {
        isSuccess = _isSuccess;
        score = _score;

        IsSuccessText.text = isSuccess ? "Baby Success!" : "Baby Failed...";
        ScoreText.text = score.ToString();

        gameObject.SetActive(true);
    }
    
}
