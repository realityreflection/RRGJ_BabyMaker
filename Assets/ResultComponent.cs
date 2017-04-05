using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultComponent : MonoBehaviour {
    public GameObject SuccessView;
    public GameObject FailedView;
    public Image Dim;
    public ResultPopup Popup;
    bool isSuccess = false;
    int score = 0;
    public void ShowResult(bool _isSuccess, int _score)
    {
        Dim.color = new Color(Dim.color.r, Dim.color.g, Dim.color.b, 0.5f);
        isSuccess = _isSuccess;
        score = _score;
        StartCoroutine(ShowResulting());
    }


    IEnumerator ShowResulting()
    {
        SuccessView.SetActive(isSuccess);
        FailedView.SetActive(!isSuccess);
        yield return new WaitForSeconds(1.0f);
        Popup.Show(isSuccess, score);
    }
}
