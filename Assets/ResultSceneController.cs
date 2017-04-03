using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResultSceneController : MonoBehaviour
{
    public void OnClickOk()
    {
        SceneManager.LoadScene("Intro");
    }
}
