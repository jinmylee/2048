using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class StageLock : MonoBehaviour
{
    public int StageAt;
    int nowStage;
    public GameObject StagesOb;
    void Start()
    {
        Button[] stages = StagesOb.GetComponentsInChildren<Button>();

        StageAt = PlayerPrefs.GetInt("StageReached");
        Debug.Log(StageAt);

        for(int i = StageAt + 1; i < stages.Length; i++)
        {
            stages[i].interactable = false;
        }


    }
}
