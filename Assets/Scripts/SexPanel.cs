using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SexPanel : MonoBehaviour
{
    List<GameObject> posList = null;
	// Use this for initialization
	void Start ()
    {
        posList = new List<GameObject>();
        for (int i = 0; i < transform.childCount; ++i)
        {
            var targetObj = transform.GetChild(i).gameObject;
            posList.Add(targetObj);
            targetObj.SetActive(false);
        }
	}
	

    public void SetPos(int combinationIdx)
    {
        for (int i = 0; i < posList.Count; ++i)
        {
            if(i == combinationIdx)
            {
                posList[i].SetActive(true);
            }
            else
            {
                posList[i].SetActive(false);
            }
        }
    }

    public void Reset()
    {
        for (int i = 0; i < posList.Count; ++i)
        {
            posList[i].SetActive(false);
        }
    }
}
