using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ⁠ https://www.youtube.com/watch?v=mldjoVDhKc4 ⁠ Reference
/// </summary>
public class PlayerMovement : MonoBehaviour
{
    public SceneRotation sceneRotation;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpForceLandscape = 6.0f; 
    [SerializeField] private float fallSpeedLandscape = 0.09f; 
    [SerializeField] private float jetpackForce = 3.0f; 
    [SerializeField] private float normalFallSpeed = 0.05f; 
    private Rigidbody2D rb;
    private bool useJet;
    public CameraMovement cameraMovement;
    private Health health;
    private int healthDamage = 1;
    private Vector3 playerOriginalPosition;
    public GameObject cam; 
    public Vector3 CameraOriginalPosition;
    public GameObject scene;
    public Vector3 sceneOriginalPosition; 

    private bool isGameStated = false;

    private HashSet<GameObject> damagedSpikes = new HashSet<GameObject>();

    private bool isTouchWall = false;
    private float stayOnWallTime = 0.0f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        health = GetComponent<Health>();

        isGameStated = false;
    }

    void Update()
    {
        if(isGameStated){
            float moveLR = Input.GetAxis("Horizontal"); // Left/Righ Movement
            Vector2 vel= new Vector2(moveLR * speed, rb.velocity.y);
            rb.velocity = vel;

            if (sceneRotation.isVertical) //Check vert
            {
                if (Input.GetKey(KeyCode.Space)) // disable jump, enable jet
                {
                    useJet = true;
                    rb.velocity = new Vector2(rb.velocity.x, jetpackForce);
                }
                else
                {
                    useJet = false;
                }

                if (!useJet && rb.velocity.y < 0) // Use normal gravity in vertical mode
                {
                    rb.velocity += new Vector2(0, -normalFallSpeed); // Normal falling speed
                }
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.Space)) // input spaceBar
                {
                    Vector2 spac = new Vector2(rb.velocity.x, jumpForceLandscape);
                    rb.velocity = spac;
                }

                if (rb.velocity.y < 0) // Slow landscape fall
                {
                    Vector2 slo = new Vector2(0, -fallSpeedLandscape);
                    rb.velocity += slo;
                }

                useJet = false;
            }
        }
        
    }

    
    void OnCollisionEnter2D(Collision2D collision) // If collided with spike
    {
        if (collision.gameObject.CompareTag("Wall")){
            Debug.Log("YOU LOST!");
            PlayerPrefs.DeleteAll();
            // Time.timeScale = 0f; //Pause the game (for now)

            FindObjectOfType<EventControl>().ShowGameOverPanel();
            isTouchWall = true;
        }
        if (collision.gameObject.CompareTag("Spike") && !sceneRotation.isRotating) //check if it is spike
        {
            if (!damagedSpikes.Contains(collision.gameObject))
            {
                damagedSpikes.Add(collision.gameObject);

                playerOriginalPosition = gameObject.transform.position;

                cam = GameObject.Find("Main Camera");
                CameraOriginalPosition = cam.transform.position;

                scene = GameObject.Find("GameView");
                sceneOriginalPosition = scene.transform.position;

                Die();
            }
        }
        
    }

    void OnCollisionStay2D(Collision2D collision){
        print("in stay function");
        if (collision.gameObject.CompareTag("Wall")){
            print("on the wall");
            stayOnWallTime += Time.deltaTime;

            if(stayOnWallTime >= 1){
                health.TakeDamage(healthDamage);
                stayOnWallTime = 0.0f;
            }
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Spike"))
        {
            damagedSpikes.Remove(collision.gameObject);
        }
        if (collision.gameObject.CompareTag("Wall")){
            isTouchWall = false;
            stayOnWallTime = 0.0f;
        }
    }

    public void StartGame(){
        isGameStated = true;
    }

    
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("WinTrigger")) // If it is winTrigger
        {
            Win();
            cameraMovement.StopCamera();
            sceneRotation.StopRotation();
        }
    }

    void Die()
    {
        Debug.Log("Player has died :("); //print death
        health.TakeDamage(healthDamage);

        // // Position the player and camera and sceneView at the origial position instead of reloading the scene!
        // gameObject.transform.position = playerOriginalPosition;
        // cam.transform.position = CameraOriginalPosition;
        // scene.transform.position = sceneOriginalPosition;
        // scene.transform.rotation = Quaternion.identity;
        // sceneRotation.setCameraOrientation();

        // UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }

    void Win()
    {
        Debug.Log("Winner Winner Chicken Dinner!"); //print win
    }
}
