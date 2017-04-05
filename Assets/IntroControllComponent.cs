using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class IntroControllComponent : MonoBehaviour
{
    public Button startButton;
    public Animation anim;
    public GameObject sound;

    void Start()
    {
        Screen.SetResolution(720, 1280, true);
    }

    public void OnClickStartButton()
    {
        startButton.interactable = false;
        anim.Play();
        sound.SetActive(true);
        NetworkWorker.Connect();
    }
}
