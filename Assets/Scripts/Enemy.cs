/**
Author: Ruoqi
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public float speed;
    public bool MoveRight;
    
    
    // Update is called once per frame
    void Update()
    {   
        if (PlayerPrefs.GetInt("status", 1) == 0)
            return;
        // if move right, bool is true, which means he will move to the right
        if (MoveRight) {
            transform.Translate(2 * Time.deltaTime * speed, Time.deltaTime * 1, 0);
            transform.localScale = new Vector2(-120,120);

        } else {
            transform.Translate(-2 * Time.deltaTime * speed, Time.deltaTime * 1, 0);
            transform.localScale = new Vector2(120,120);
        }
        if (transform.localPosition.y >= 700)
            Destroy(gameObject);
    }
    
    void OnTriggerEnter2D(Collider2D trig){
        if (trig.gameObject.CompareTag("Turn")) {
            if (MoveRight) {
                MoveRight = true;
            } else {
                MoveRight = false;
            }
        }
    }
}

