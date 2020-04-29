using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonClick : MonoBehaviour
{
    /* Modified by Chenlu Zhang, Apr 6
     * Add tutorial images
     */
    public Image tutorialImg;
    int imgNum = 1;
    private string resImgPath = "Tutorial/";
    public GameObject canvas;
    public GameObject pauseCanvas;



    public void EnterGame ()
    {
        PlayerPrefs.SetInt("status", 1);
        SceneManager.LoadScene("MainScene");
    }

    public void EnterTutorial()
    {
        Dictionary<string, object> dict = new Dictionary<string, object>();
        if (PlayerPrefs.GetInt("tutorCnts", 0) == 0)
        {
            dict.Add("useTutor", 1);
        }
        PlayerPrefs.SetInt("tutorCnts", PlayerPrefs.GetInt("tutorCnts", 0) + 1);
        dict.Add("tutor", PlayerPrefs.GetInt("tutorCnts", 0));
        Analytics.CustomEvent("hitTutor" + MetaInfo.version, dict);
        canvas.SetActive(true);
    }

    public void Previous()
    {
        if (imgNum > 1)
        {
            imgNum--;
            tutorialImg.GetComponent<Image>().sprite = Resources.Load(resImgPath + imgNum, typeof(Sprite)) as Sprite;
        }
    }

    public void Next()
    {
        if (imgNum < 6)
        {
            imgNum++;
            tutorialImg.GetComponent<Image>().sprite = Resources.Load(resImgPath + imgNum, typeof(Sprite)) as Sprite;
        }
    }

    public void EnterSettings ()
    {
       
    }

    public void QuitGame ()
    {
        Application.Quit();
    }

    public void BackHome ()
    {
        SceneManager.LoadScene("StartScene");
    }

    public void Replay ()
    {
        // record total replay times
        PlayerPrefs.SetInt("replayCnts", PlayerPrefs.GetInt("replayCnts", 0) + 1);
        Analytics.CustomEvent("hitReplay" + MetaInfo.version, new Dictionary<string, object>
        {
            { "replay", PlayerPrefs.GetInt("replayCnts", 0)}
        });
        PlayerPrefs.SetInt("status", 1);
        SceneManager.LoadScene("MainScene");
    }

    public void pausePress ()
    {
        PlayerPrefs.SetInt("status", 1);
        pauseCanvas.SetActive(false);
    }


   

}
