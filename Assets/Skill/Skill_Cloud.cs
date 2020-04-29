using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * added by Xin Huang Mar 7, 2020
 * This skill generates a white cloud at the bottom
 * currently the lasting time is the same as cooling time
 */
public class Skill_Cloud : MonoBehaviour 
{ 
    public Button btn;
    public Image imageCoolDown;
    public Text costText;
    public float cooldownSpeed = 5;
    public PlayerControl player;
    public int cost = 5;
    public GameManage gm;

    bool isCooling = false;

    // Start is called before the first frame update
    void Start()
    {
        costText.text = cost + "";
        btn.onClick.AddListener(TaskOnClick);
        isCooling = false;
        imageCoolDown.fillAmount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (isCooling)
        {
            imageCoolDown.fillAmount += 1 / cooldownSpeed * Time.deltaTime;

            if (imageCoolDown.fillAmount >= 1)
            {
                imageCoolDown.fillAmount = 0;
                isCooling = false;
            }
        }
    }

    void TaskOnClick()
    {
        if (PlayerPrefs.GetInt("status", 0) != 0 && !isCooling && player.getScore() >= cost)
        {
            isCooling = true;
            player.callSkill("Cloud", true, cost);
            // generate a cloud
            gm.gen_gloden_cloud();
        }
    }

}
