using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class IntroControllComponent : MonoBehaviour
{
    public Button startButton;
    public Animation anim;
    public void OnClickStartButton()
    {
        NetworkWorker.Connect();
        startButton.interactable = false;
        anim.Play();
    }
}
