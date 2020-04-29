using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class GameManage : MonoBehaviour
{
	private Image bg;
	private GameObject [] platforms;
	private GameObject [] awards;
    private GameObject spikes;
	private Transform gameCanvas;
    private float begin;
    public GameObject player;
    private float speed = 2.0f;
    public Sprite phase2, phase3;
    
    private GameObject[] skills;

    private GameObject enemies;
    public bool MoveRight;

    private const float EAGLE_SHOW_TIME = 15f;
    private const float EAGLE_PERIOD = 10f;

    private bool shop;

    public GameObject pauseCanvas;


    /*
     * Modified by Chenlu
     */
    public GameObject tutorialTilt;
    public GameObject tutorialSpikes;
    public GameObject tutorialShield;
    public GameObject tutorialCloud;
    public GameObject tutorialSaw;
    public GameObject tutorialEagle;
    public GameObject tutorialContinue;
    
    public PlayerControl playercontrol;
   
    private static bool showTilt = false;
    private static bool showSpikes = false;
    private static bool showShield = false;
    private static bool showCloudCreator = false;
    private static bool showSaw = false;
    private static bool showEagle = false;

    public GameObject shield;
    public GameObject cloudCreator;
    private float timer = 0;


    void genCeil()
    {
        spikes = Resources.Load<GameObject>("Ceil/Spike");
        for (int i = 0; i < 10; i++)
        {
            GameObject spike = Instantiate(spikes, new Vector3(100 * i, -50, 0), Quaternion.identity);
            spike.transform.SetParent(gameCanvas, false);
        }
    }

    void Start()
    {
        begin = Time.time;
        gameCanvas = GameObject.Find("GameCanvas").transform;
        genCeil();
        bg = GameObject.Find("Background").GetComponent<Image>();
        bg.material.SetTextureOffset("_MainTex", new Vector2(0, 0));
        platforms = Resources.LoadAll<GameObject>("Platforms");
        awards = Resources.LoadAll<GameObject>("Awards");
        InvokeRepeating("CreateAll", 0, 0.5f);
        InvokeRepeating("speedChange", 0, 30.0f);
        skills = Resources.LoadAll<GameObject>("Skills");
        enemies = Resources.Load<GameObject>("Eagle/Enemy");
        shop = false;
        InvokeRepeating("enemy_created", EAGLE_SHOW_TIME, EAGLE_PERIOD);

        
    }

    // Update is called once per frame
    void Update()
    {
        
        if (TouchDetect.doubleClick)
        {
            if (PlayerPrefs.GetInt("status", 1) == 1)
            {
                PlayerPrefs.SetInt("status", 0);
                pauseCanvas.SetActive(true);
            }
        }
        // bg.material.SetTextureOffset("_MainTex", new Vector2(0, Time.time / 5));


        /*
         * Modified by Chenlu, Apr 12
         * show tutorial for first-time player
         */
        
        if (Time.time - begin >= 0.25 && !showTilt)
        {
            PlayerPrefs.SetInt("status", 0);
            player.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
            tutorialTilt.SetActive(true);
         
        }
        if (showTilt)
        {
            tutorialTilt.SetActive(false);
           
        }


        if (playercontrol.getScore() == 2 && !showShield)
        {

            PlayerPrefs.SetInt("status", 0);
            player.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
            tutorialShield.SetActive(true);
            Vector3 shieldScale = shield.transform.localScale;
            shieldScale.Set(1.5f, 1.5f, 1);
            shield.transform.localScale = shieldScale; 
           
        }
        if (showShield)
        {
            tutorialShield.SetActive(false);
        }


        if (player.transform.localPosition.y >= 250 && !showSpikes)
        {
            PlayerPrefs.SetInt("status", 0);
            player.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
            tutorialSpikes.SetActive(true);

        }
        if (showSpikes)
        {
            tutorialSpikes.SetActive(false);

        }


        if (playercontrol.getScore() == 6 && !showCloudCreator)
        {

            PlayerPrefs.SetInt("status", 0);
            player.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
            tutorialCloud.SetActive(true);
            Vector3 cloudScale = cloudCreator.transform.localScale;
            cloudScale.Set(1.5f, 1.5f, 1);
            cloudCreator.transform.localScale = cloudScale;

        }
        if (showCloudCreator)
        {
            tutorialCloud.SetActive(false);
        }

        if (GameObject.Find("Enemy(Clone)").transform.localPosition.y >= -550 && !showEagle)
        {

            PlayerPrefs.SetInt("status", 0);
            player.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
            tutorialEagle.SetActive(true);

        }
        if (showEagle)
        {
            tutorialEagle.SetActive(false);
        }


        if (GameObject.Find("2_Saw(Clone)").transform.localPosition.y >= -550 && !showSaw)
        {
            PlayerPrefs.SetInt("status", 0);
            player.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
            tutorialSaw.SetActive(true);

        }
        if (showSaw)
        {
            tutorialSaw.SetActive(false);
        }

        

    }

    private Vector3 Vector3(double v1, double v2, int v3)
    {
        throw new NotImplementedException();
    }

    /*
     * Modified by Chenlu, Apr 12
     * show tutorial only once
     */
    public void continuePress()
    {
        PlayerPrefs.SetInt("status", 1);
        player.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        if (tutorialTilt.activeSelf) { showTilt = true; }
        if (tutorialSpikes.activeSelf) { showSpikes = true; }
        if (tutorialShield.activeSelf) { showShield = true; }
        if (tutorialCloud.activeSelf) { showCloudCreator = true; }
        if (tutorialSaw.activeSelf) { showSaw = true; }
        if (tutorialEagle.activeSelf) { showEagle = true; }

        Vector3 shieldScale = shield.transform.localScale;
        shieldScale.Set(1, 1, 1);
        shield.transform.localScale = shieldScale;

        Vector3 cloudScale = cloudCreator.transform.localScale;
        cloudScale.Set(1, 1, 1);
        cloudCreator.transform.localScale = cloudScale;

    }


   
    private void genSpeed(float et)
    {
        speed += 0.1f;
        if (speed > 5)
            speed = 5;
    }

    private void speedChange()
    {
        float elapse = Time.time - begin;
        if (elapse >= 60)
        {
            bg.gameObject.GetComponent<Image>().sprite = phase3;
            genSpeed(elapse - 60);
        }
    }

    private void CreateAll()
    {
        // If pause or gameover, stop creating new platforms.
        if (PlayerPrefs.GetInt("status", 1) == 0)
            return;
        GameObject tempObject = null;
        float elapse = Time.time - begin;
        UnityEngine.Random.InitState((int)(Time.time * 60));
        float prob = UnityEngine.Random.Range(0f, 1f);
        if (elapse < 10)
        {
            tempObject = prob < 0.8f ? platforms[0] : platforms[1];
           
        }
        /*
         * modified by Chenlu
         * set prob for break cloud
         */
        else if (elapse < 30)
        {
            
            tempObject = prob < 0.6f ? platforms[0] : (prob < 0.8f ? platforms[1] : platforms[3]);

        }
        else
        {
            if (elapse < 60)
                bg.gameObject.GetComponent<Image>().sprite = phase2;
            tempObject = prob < 0.6f ? platforms[0] : (prob < 0.8f ? platforms[1] : (prob < 0.9f ? platforms[2] : platforms[3]));
        }
        UnityEngine.Random.InitState((int)(Time.time * 60));
        GameObject cloud = Instantiate(tempObject, new Vector3(UnityEngine.Random.Range(-290, 291), -1000, 0), Quaternion.identity);
        //cloud.tag = tempObject == platforms[2] ? "Saw" : (tempObject == platforms[1] ? "DarkCloud" : "Clouds");
        cloud.tag = tempObject == platforms[3] ? "BreakCloud" : (tempObject == platforms[2] ? "Saw" : (tempObject == platforms[1] ? "DarkCloud" : "Clouds"));
        cloud.AddComponent<PlatformsMove>();
        cloud.GetComponent<PlatformsMove>().setSpeed(speed);    // pass values between scripts
        if (UnityEngine.Random.Range(0, 10) < 2)
        {
            // set cloud float, prob = 0.1
            cloud.GetComponent<PlatformsMove>().setFloating();    
        }
        cloud.transform.SetParent(gameCanvas, false);
        if (tempObject == platforms[1])
            cloud.GetComponent<Collider2D>().enabled = false;
        GameObject award = CreateAward();
        if (award != null && tempObject != platforms[2])
        {
            Instantiate(award, new Vector3(0, 60, 0), Quaternion.identity).transform.SetParent(cloud.transform, false);
        }

        player.transform.SetAsLastSibling();    // make sure the player is always on top layer
    }

    private GameObject CreateAward()
    {
        UnityEngine.Random.InitState((int)(Time.time * 60));
        float prob = UnityEngine.Random.Range(0f, 1f);
        GameObject temp = null;
        if (prob < 0.5f)
        {
            temp = null;
        }
        else if (prob < 0.95f)
        {
            temp = awards[0]; //coin
        }
        else
        {
            temp = awards[1]; //life
        }
        return temp;
    }

    /*
     * Added by Xin Huang Mar 7, 2020
     * for external use, generate a gloden cloud
     * modified from createAll()
     */
        public void gen_gloden_cloud()
    {
        GameObject cloud = Instantiate(skills[0], new Vector3(UnityEngine.Random.Range(-290, 291), -800, 0), Quaternion.identity);
        cloud.tag = "Clouds";
        cloud.AddComponent<PlatformsMove>();
        cloud.GetComponent<PlatformsMove>().setSpeed(speed);    // pass values between scripts
        cloud.transform.SetParent(gameCanvas, false);

        player.transform.SetAsLastSibling();    // make sure the player is always on top layer
    }

    /*
     * Added by Ruoqi on Mar 7, 2020
     * generate an eagle
     */
    public void enemy_created()
    {
        if (PlayerPrefs.GetInt("status", 1) == 0)
            return;

        GameObject enemy = Instantiate(enemies, new Vector3(UnityEngine.Random.Range(-300, 300), UnityEngine.Random.Range(-1500, -800), 0), Quaternion.identity);
        enemy.tag = "Enemy";
        enemy.AddComponent<PlatformsMove>();
        enemy.GetComponent<PlatformsMove>().setSpeed(speed);    // pass values between scripts
        enemy.GetComponent<PlatformsMove>().setFloating();
        enemy.transform.SetParent(gameCanvas, false);

        player.transform.SetAsLastSibling();      // make sure the player is always on top layer
        
        if (transform.localPosition.y >= 700)
            Destroy(gameObject);

    }     

}
