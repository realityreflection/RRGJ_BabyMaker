using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class IntroControllComponent : MonoBehaviour
{
    public void OnClickStartButton()
    {
        SceneManager.LoadScene("InGame");
    }
}
