using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour {
    AudioClip[] PosSound = null;
    AudioClip MaleSound = null;
    AudioClip FemaleSound = null;
    AudioSource EffectSound;

    public AudioSource BGM;

	// Use this for initialization
	void Start ()
    {
        PosSound = Resources.LoadAll<AudioClip>("Sounds/Pos");
        MaleSound = Resources.Load<AudioClip>("Sounds/Button/button_man");
        FemaleSound = Resources.Load<AudioClip>("Sounds/Button/button_woman");
        EffectSound = GetComponent<AudioSource>();
        EffectSound.Stop();
    }

    public void PlayPosSound(int posIdx)
    {
        EffectSound.clip = PosSound[posIdx];
        EffectSound.Play();
    }

    public void StopPosSound()
    {
        EffectSound.Stop();
    }

    public void PlayBGM()
    {
        BGM.loop = true;
        BGM.Play();
    }

    public void PlayButtonSound(bool isMale)
    {
        EffectSound.Stop();

        if (isMale)
            EffectSound.clip = MaleSound;
        else
            EffectSound.clip = FemaleSound;

        EffectSound.Play();
    }
}
