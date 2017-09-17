using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class IntroSceneController : MonoBehaviour
{
    public Button StartButton;
    public Animation Anim;
    public GameObject Sound;

    void Start()
    {
        bool isDediServer = false;
        string[] args = System.Environment.GetCommandLineArgs();
        for (int i = 0; i < args.Length; i++)
        {
            if (args[i] == "-server")
                isDediServer = true;
        }

        if (!isDediServer)
        {
            Screen.SetResolution(720, 1280, false);
        }
    }

    public void OnClickStartButton()
    {
        StartButton.interactable = false;
        Anim.Play();
        Sound.SetActive(true);
        GameObject.FindGameObjectWithTag("NetworkManager").GetComponent<NetworkController>().StartClient();
    }
}
