using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformsMove : MonoBehaviour
{
    private float speed = 2.0f;
    private bool isFloating = false;
    private float floatingSpeed = 1f;
    private float direction = 1f;   // default right

    private const int SCREEN_WIDTH = 400;

    // Update is called once per frame
    void Update()
    {
        if (PlayerPrefs.GetInt("status", 1) == 0)
            return;
        transform.Translate(Vector3.up * Time.deltaTime * speed);
        /*
         * Apr. 18, 2020 Xin Huang
         */
        if (isFloating)
        {
            if (transform.localPosition.x < -SCREEN_WIDTH + 20)
            {
                direction = 1f;
            }
            else if (transform.localPosition.x > SCREEN_WIDTH - 20)
            {
                direction = -1f;
            }
            Debug.Log("cloud x position: " + transform.position.x + ", dir: " + direction);
            transform.Translate(Vector3.right * Time.deltaTime * floatingSpeed * direction);
        }
        if (transform.localPosition.y >= 700)
            Destroy(gameObject);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        //collision.collider.transform.SetParent(null);
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        ContactPoint2D contact = collision.contacts[0];
        if (contact.normal.Equals(Vector2.left) || contact.normal.Equals(Vector2.right))
            GetComponent<Collider2D>().enabled = false;
        //collision.collider.transform.SetParent(transform);
    }

    public void setSpeed(float speed_)
    {
        speed = speed_;
    }

    public void setFloating()
    {
        isFloating = true;
        // set up range
        // currently use screen width

        // decide direction left or right
        if (Random.Range(0, 2) < 1)
        {
            direction = 1f;
        }
        else
        {
            direction = -1f;
        }
        // decide floating speed (must be called after setSpeed)
        floatingSpeed = Random.Range(floatingSpeed, speed);
    }

    public void setFloatingSpeed(float speed)
    {
        floatingSpeed = speed;
    }
}
