using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour {
    AudioClip[] PosSound = null;
    AudioClip MaleSound = null;
    AudioClip FemaleSound = null;
    AudioSource Source;
    int playedIndex = -1;
	// Use this for initialization
	void Start () {
        PosSound = Resources.LoadAll<AudioClip>("Sounds/Pos");
        MaleSound = Resources.Load<AudioClip>("Sounds/Button/button_man");
        FemaleSound = Resources.Load<AudioClip>("Sounds/Button/button_woman");
        Source = GetComponent<AudioSource>();
        Source.Stop();
    }

    public void PlayPosSound(int posIdx)
    {
        Source.clip = PosSound[posIdx];
        Source.Play();
    }

    public void StopPosSound()
    {
        Source.Stop();
    }

    public void PlayButtonSound(bool isMale)
    {
        Source.Stop();

        if (isMale)
            Source.clip = MaleSound;
        else
            Source.clip = FemaleSound;

        Source.Play();
    }
}
