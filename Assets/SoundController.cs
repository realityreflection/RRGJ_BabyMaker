using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour {
    AudioSource[] sounds = null;
    int playedIndex = -1;
	// Use this for initialization
	void Start () {
        sounds = GetComponentsInChildren<AudioSource>();
        foreach(var sound in sounds)
        {
            sound.Stop();
        }
	}

    public void PlaySound(int soundIdx)
    {
        sounds[soundIdx].Play();
        playedIndex = soundIdx;
    }

    public void StopSound()
    {
        if(playedIndex >=0 && playedIndex < sounds.Length)
            sounds[playedIndex].Stop();
    }
}
