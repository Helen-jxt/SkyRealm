using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follower : MonoBehaviour
{
    public GameObject player;
    private Vector3 diff;
    // Start is called before the first frame update
    void Start()
    {
        if (player != null)
        {
            transform.position = player.transform.position;
            diff = transform.position - player.transform.position;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null)
            transform.position = player.transform.position + diff;
    }

}
