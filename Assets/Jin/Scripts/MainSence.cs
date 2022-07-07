using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainSence : MonoBehaviour
{
       public void StageMenu()
    {
        SceneManager.LoadScene("StageMenu");
    }

    public void Stage1()
    {
        SceneManager.LoadScene("Exstage1");
    }
    public void Stage2()
    {
        SceneManager.LoadScene("Exstage2");
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
