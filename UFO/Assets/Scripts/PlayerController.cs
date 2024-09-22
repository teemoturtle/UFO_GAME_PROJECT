/*
Author: Sunny Dinh
Date: 9/16/24
Description: This is used to control the player, spawn random pickup gameobjects, set the timer, and dispaly end messages for the game
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb2d;
    public float speed;
    //private int count;
    private float timer;
    //public Text countText;
    public Text timerText;
    public Text youWinText;
    public Button restartButton;
    public Button startButton;
    public Button closeGameButton;
    private bool gameOver; // set for game play
    private float spawnMaxTime = 10f; // set spawn max time
    private float setSpawnTime = 0f; // initiate spawn timer
    private Vector2 randomSpawnRange = new Vector2(10f, 10f); // set spawn range
    public GameObject pickUp; // initiate pickup game object
    private GameObject initialPlayer; // initiate player gameObject

    // Start is called before the first frame update
    void Start()
    {
        //count = 0;
        timer = 60; // set start time
        rb2d = GetComponent<Rigidbody2D>();
        //countText.text = "Count: " + count.ToString();
        timerText.text = "";
        youWinText.text = "";
        restartButton.gameObject.SetActive(false);
        gameOver = true;
        closeGameButton.gameObject.SetActive(true);


        GameObject initialPickUp = GameObject.FindGameObjectWithTag("PickUp"); // find the pickup object
        initialPlayer = GameObject.FindGameObjectWithTag("Player"); // find the player object


        // this is used to hide the main pickup object so it can be cloned in void update() SpawnPickup()
        if (initialPickUp != null)
        {
            SpriteRenderer spriteRenderer = initialPickUp.GetComponent<SpriteRenderer>(); // Use this to edit the sprite
            Collider2D collider = initialPickUp.GetComponent<Collider2D>(); // Use this edit the Collider2D 

            if (spriteRenderer != null)
            {
                spriteRenderer.enabled = false;  // Hide the sprite
            }

            if (collider != null)
            {
                collider.enabled = false;  // Disable the collider to prevent interaction
            }
        }

        // set initial player sprite to false
        if (initialPlayer != null)
        {
            initialPlayer.SetActive(false);
        }

    }

    // FixedUpdate is in sync with physics engine
    void FixedUpdate()
    {
        // bool used to stop time and gameplay
        if (!gameOver)
        {
            float moveHorizontal = Input.GetAxis("Horizontal"); // player movement x-axis
            float moveVertical = Input.GetAxis("Vertical"); // player movement y-axis
            Vector2 movement = new Vector2(moveHorizontal, moveVertical); // player movement set
            rb2d.velocity = movement * speed; // player movement speed
            timer -= Time.deltaTime; // subtract timer by milliseconds
            int seconds = (int)timer % 60; // convert timer to seconds
            timerText.text = "Time: " + seconds.ToString();
            closeGameButton.gameObject.SetActive(false); // hide close button when game starts
            // if player reaches 0 seconds
            if (seconds == 0)
            {
                youWinText.text = "YOU WIN!";
                restartButton.gameObject.SetActive(true);
                gameObject.SetActive(false);
                timer = 0;
                gameOver = true;
            }

            // create spawn time to randomly spawn a pickup gameObject every 10 seconds
            setSpawnTime += Time.deltaTime; // add 1 second to spawn time
            if (setSpawnTime >= spawnMaxTime)
            {
                SpawnPickup();
                setSpawnTime = 0f;
            }

        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("PickUp"))
        {
            Debug.Log("OnCollisionEnter2D");
            youWinText.text = "GAME OVER!";
            restartButton.gameObject.SetActive(true);
            gameOver = true; // if player hit, stop gameplay
            gameObject.SetActive(false); // disable the player but keep data to play again
            closeGameButton.gameObject.SetActive(true); // give user option to close game
        }
    }

    private void SpawnPickup()
    {
        Vector2 randomPosition;
        float minDistanceFromPlayer = 5f; // default spawn distance from player object

        do
        {
            // set random position to spawn pickup object
            randomPosition = new Vector2(
            UnityEngine.Random.Range(-randomSpawnRange.x, randomSpawnRange.x),
            UnityEngine.Random.Range(-randomSpawnRange.y, randomSpawnRange.y)
        );

        }
        while (Vector2.Distance(randomPosition, transform.position) < minDistanceFromPlayer); // only spawn if found location from min player distance

        GameObject newPickup = Instantiate(pickUp, randomPosition, Quaternion.identity); // create new pickup gameObject value

        SpriteRenderer spriteRenderer = newPickup.GetComponent<SpriteRenderer>(); // used to edit sprite
        Collider2D collider = newPickup.GetComponent<Collider2D>(); // used to edit collider 2D

        // when a new pickUp gameObject is create from the main pickUp object
        // Must reactive sprite visibility and collider2D
        if (spriteRenderer != null)
        {
            spriteRenderer.enabled = true;
        }

        if (collider != null)
        {
            collider.enabled = true;
        }
    }

    // This method will be called when the Restart button is pressed
    public void OnRestartButtonPress()
    {
        SceneManager.LoadScene("SampleScene");
    }

    // This method will be called when the Start button is pressed
    public void OnStartButtonPress()
    {
        gameOver = false;  // Enable the game to start

        // Enable player physics
        rb2d.simulated = true;

        // Start the game timer
        timer = 60;
        timerText.text = "Time: " + timer.ToString();
        timerText.gameObject.SetActive(true);  // Show the timer

        // Hide the start button once the game starts
        startButton.gameObject.SetActive(false);

        // reactivate player sprite
        initialPlayer.SetActive(true);

        // Call SpawnPickup to create the initial pickup object
        SpawnPickup();
    }

    // This method will be called when the x button is pressed
    public void CloseGame()
    {
        Application.Quit();
    }
}
