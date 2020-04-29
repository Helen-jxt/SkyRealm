using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchDetect : MonoBehaviour
{
    private float lastEndTime, curEndTime;
    public static bool doubleClick, longPress;
    Rect notouchable_area;

    void Start()
    {
        lastEndTime = -1f;
        curEndTime = -1f;
        doubleClick = false;
        longPress = false;
    }

    Vector2 startPos, endPos;
    float curBeginTime;
    void Update()
    {
        if (doubleClick)
            doubleClick = false;
        if (longPress)
            longPress = false;
        if (Input.touches.Length > 0)
        {
            Touch t = Input.GetTouch(0);
            
            if (t.phase == TouchPhase.Began)
            {
                startPos = new Vector2(t.position.x / (float)Screen.width, t.position.y / (float)Screen.width);
                curBeginTime = Time.time;
            }
            if (t.phase == TouchPhase.Stationary)
            {
                if (Time.time - curBeginTime >= 0.5f)
                    longPress = true;
                else
                    longPress = false;
            }
            if (t.phase == TouchPhase.Ended)
            {
                curEndTime = Time.time;
                //if (curBeginTime >= 0 && curEndTime - curBeginTime > 0.5f)
                //{
                //    endPos = new Vector2(t.position.x / (float)Screen.width, t.position.y / (float)Screen.width);
                //    Vector2 swipe = new Vector2(endPos.x - startPos.x, endPos.y - startPos.y);
                //    if (swipe.magnitude < 0.15f)
                //        longPress = true;

                //}
                endPos = new Vector2(t.position.x / (float)Screen.width, t.position.y / (float)Screen.width);
                Vector2 swipe = new Vector2(endPos.x - startPos.x, endPos.y - startPos.y);
                if (lastEndTime >= 0)
                {
                    if (curEndTime - lastEndTime <= 0.5f && (endPos.x < 0.75f && endPos.x > 0.25f) && (startPos.x < 0.75f && startPos.x > 0.25f))
                    {
                        doubleClick = true;
                    }
                    else
                        doubleClick = false;
                }
                lastEndTime = curEndTime;
            }
        }
    }

    // Show debug information
    //void OnGUI()
    //{
    //    GUI.Label(new Rect(20, 20, 300, 400), "doubleClick: " + doubleClick.ToString());
    //    GUI.Label(new Rect(20, 320, 300, 400), startPos.ToString() + "\n" + endPos.ToString());
    //}
}
