/*
Author: Sunny Dinh
Date: 9/16/24
Description: This is created to track the player object, when the player moves it follows the 2D player object.
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player;
    private Vector3 offset;

    // Start is called before the first frame update
    void Start()
    { 
        offset = transform.position - player.transform.position; // offset vector from initial config
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = player.transform.position + offset; // makes sure to keep main camera with player movement
    }
}
