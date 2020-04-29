using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.UI;

public class PlayerControl : MonoBehaviour
{
    private int score;
    public Text Score;

    private int life;
    public Text Life;

    public Text GameOverScore;
    public Text highScore;

    public GameObject canvas;
    public bool gameOver;

    public AudioClip coinSound;
    public AudioClip lifeSound;
    public AudioClip shielfSound;

    /*
     * Xin Huang Mar 2, 2020
     * add speed control
     */
    public float moveSpeed;

    /* 
     * Xin Huang Mar 2, 2020
     * skill status
     */
    public bool skill_shield;
    private float begin;

    private bool isDataUpload;

    private const int SCREEN_WIDTH = 400;

    private float tempTime;

    public GameObject shield;

    //public Slider playerSpeedSlider;

    // Start is called before the first frame update
    void Start()
    {
        PlayerPrefs.SetInt("status", 1);
        Time.timeScale = 1;
        score = 0;
        life = 3;
        gameOver = false;
        GameOverScore.text = "0";
        Score.text = "0";
        canvas.SetActive(false);

        /*
         * Xin Huang added Mar 2, 2020
         * skill status
         */
        skill_shield = false;

        begin = Time.time;
        isDataUpload = false;

        // Debug.Log("Screen Width : " + Screen.width);
        // Debug.Log(MetaInfo.version);

        moveSpeed = PlayerPrefs.GetFloat("palyerSpeed", 5f);
        shield.GetComponent<Image>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
       
        if (PlayerPrefs.GetInt("status", 1) == 0)
            return;
        if (transform.localPosition.y >= 700 || transform.localPosition.y <= -750)
        {
            life = 0;
        }
        if (skill_shield)
        {
            // transform.GetComponent<Image>().color = new Color32(0, 255, 0, 255);
            shield.GetComponent<Image>().enabled = true;
        }
        else
        {
            shield.GetComponent<Image>().enabled = false;
            if (life == 1)
                transform.GetComponent<Image>().color = new Color32(255, 0, 0, 255);
            else
                transform.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        }
        if (life <= 0)
        {
            gameOver = true;
            Time.timeScale = 0;
            PlayerPrefs.SetInt("status", 0);
            canvas.SetActive(true);
            GameOverScore.text = score.ToString();
            // Save and load data of highest score
            if (score > PlayerPrefs.GetInt("highscore", 0))
                PlayerPrefs.SetInt("highscore", score);
            highScore.text = PlayerPrefs.GetInt("highscore", 0).ToString();
            shield.GetComponent<Image>().enabled = false;
            Destroy(gameObject);
            if (gameOver && !isDataUpload)
            {
                Analytics.CustomEvent("gameOver" + MetaInfo.version, new Dictionary<string, object>
            {
                {"score", score},
                {"highscore", PlayerPrefs.GetInt("highscore", 0)},
                {"duration", Time.time - begin}
            });
                isDataUpload = true;
            }
        }
        
        Move();
        Score.text = score.ToString();
        Life.text = life.ToString();

    }

    /*
     * Modified by Xin Huang Mar 2, 2020
     * add mobile control
     * 
     * Mar 30, 2020 Xin Huang
     * If the player go out of bound on one side, appear on the other side
     */
    private void Move()
    {
        // Debug.Log("Player position: " + transform.position);
        if (transform.localPosition.x > SCREEN_WIDTH)
        {
            transform.localPosition = new Vector3(-SCREEN_WIDTH + 10, transform.localPosition.y, transform.localPosition.z);
        } 
        else if (transform.localPosition.x < -SCREEN_WIDTH)
        {
            transform.localPosition = new Vector3(SCREEN_WIDTH - 10, transform.localPosition.y, transform.localPosition.z);
        }
        // Debug.Log("Player position: " + transform.position);
        float h = Input.acceleration.x;
        if (SystemInfo.deviceType == DeviceType.Desktop)
        {
            h = Input.GetAxis("Horizontal");
        }
        
        if (Mathf.Abs(h) > 0.1f)
        {
            transform.Translate(h * Vector3.right * moveSpeed * Time.deltaTime);
        }
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Coin")
        {
            score += 1;
            Destroy(other.gameObject);
            AudioSource.PlayClipAtPoint(coinSound, transform.position);
        }
        else if (other.tag == "Life")
        {
            life++;
            Destroy(other.gameObject);
            AudioSource.PlayClipAtPoint(lifeSound, transform.position);
        }else if(other.tag == "Enemy"){
            life--;
            Destroy(other.gameObject);
        }
    }

    /*
     *  Modified by Xin Huang, Mar 2, 2020
     *  add skill shield effect
     */
    public void OnCollisionEnter2D(Collision2D other)
    {
        if (!skill_shield && other.gameObject.CompareTag("Enemy"))
        {
            life--;
            Destroy(other.gameObject);
        }

        ContactPoint2D contact = other.contacts[0];
        if (contact.normal.Equals(Vector2.left) || contact.normal.Equals(Vector2.right))    // invalid collision
            return;
        if (!skill_shield && other.gameObject.tag == "Saw")
        {
            life--;
            //Destroy(other.gameObject);
        }
        /*
         * Modified by Chenlu
         * Add Break Cloud Effect
         */
        if (other.gameObject.tag == "BreakCloud")
        {
            float time = 0.5f;
            Destroy(other.gameObject,time);
        }
        /*
         * Modified by Xin
         * Move with Cloud
         */
        //transform.parent = other.gameObject.transform;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        //transform.parent = null;
    }

    /*
     *  Added by Xin Huang, Mar 2, 2020
     *  for external use
     */
    public int getScore()
     {
        return score;
     }

    public void callSkill(string name, bool status, int cost)
    {
        switch (name)
        {
            case "Shield":
                if (status)
                {
                    skill_shield = true;
                    score -= cost;
                    AudioSource.PlayClipAtPoint(shielfSound, transform.position);
                } 
                else
                {
                    skill_shield = false;
                }
                break;

            case "Cloud":
                score -= cost;
                break;

            default:
                break;
        }
    }
    

    
}



