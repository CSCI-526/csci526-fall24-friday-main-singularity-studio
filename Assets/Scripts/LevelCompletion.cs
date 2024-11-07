using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.VFX;
using UnityEngine.Analytics;


public class LevelCompletion : MonoBehaviour
{
    private int currentLevel = 0;
    public Image ProgressBarImg;
    private RectTransform rt;
    private float ProgressBarWidth;
    public CameraMovement cameraMovement;
    public SceneRotation sceneRotation;
    public ParticleSystem confetti1;
    public ParticleSystem confetti2;
    public ParticleSystem confetti3;
    private Vector2 mapPosition;
    private Vector2 mapScale;
    private Vector2 topLeftCorner;
    private Vector2 bottomRightCorner;
    public float distanceFromStart;
    private float mapLength;
    private Scene currentScene;
    
    private float startTime; // Track the game start time
    private bool isGameStarted = false; // Track if the game has started

    private void Start()
    {
        ProgressBarImg = GameObject.Find("Progress").GetComponent<Image>();
        rt = ProgressBarImg.GetComponent<RectTransform>();
        ProgressBarWidth = rt.rect.width;
        rt.anchorMin = new Vector2(0, 0.5f);
        rt.anchorMax = new Vector2(0, 0.5f);
        rt.sizeDelta = new Vector2(1, rt.sizeDelta.y);

        currentScene = SceneManager.GetActiveScene();
        if (currentScene.name != "Tutorial"){
            mapLength = 415f;
            confetti1.Stop();
            confetti2.Stop();
            // confetti3.Stop();
        }else{
            mapLength = 150f;
        }
        

        mapPosition = sceneRotation.transform.position;
        mapScale = sceneRotation.transform.localScale;
        topLeftCorner = mapPosition + new Vector2(-mapScale.x / 2, mapScale.y / 2);
        bottomRightCorner = mapPosition + new Vector2(mapScale.x / 2, -mapScale.y);
    }

    private void Update(){
        // Calculate the distance from the player to the top-left corner
        distanceFromStart = Vector2.Distance(transform.position, topLeftCorner);
        float progress = (mapLength - distanceFromStart)*100 / mapLength;
        rt.sizeDelta = new Vector2(ProgressBarWidth * (100 - progress)/100, rt.sizeDelta.y);
    }

    // Method to start tracking playtime
    public void StartGameTimer()
    {
        startTime = Time.time;
        isGameStarted = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.gameObject.name == "EndPhase1" && currentScene.name != "Tutorial")
        {
            confetti1.Play();
            // rt.sizeDelta = new Vector2(ProgressBarWidth / 3, rt.sizeDelta.y);
        }
        else if (collision.gameObject.name == "EndPhase2" && currentScene.name != "Tutorial")
        {
            confetti2.Play();
            // rt.sizeDelta = new Vector2(2 * ProgressBarWidth / 3, rt.sizeDelta.y);
        }
        else if (collision.gameObject.CompareTag("WinTrigger"))
        {
            // rt.sizeDelta = new Vector2(ProgressBarWidth, rt.sizeDelta.y);
            confetti3.Play();
            Win();
        }
        else if (collision.gameObject.CompareTag("LevelTrigger"))
        {
            Debug.Log("Leveled up");
            Destroy(collision.gameObject);
            currentLevel++;
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("WinTrigger")){
            GameObject cautionTape = GameObject.Find("Caution Tape");
            if (cautionTape != null){
                cautionTape.SetActive(false);
            }
            GameObject wallDamage = GameObject.Find("Wall Damage Trigger");
            if (wallDamage != null){
                wallDamage.SetActive(false);
            }
        }
    }

    private void Win()
    {
        Scene currentScene = SceneManager.GetActiveScene();

        string sceneName = currentScene.name;
        if (sceneName == "Tutorial"){
            cameraMovement.StopCamera();
            GameObject cautionTape = GameObject.Find("Caution Tape");
            if (cautionTape != null){
                cautionTape.SetActive(false);
            }
            GameObject wallDamage = GameObject.Find("Wall Damage Trigger");
            if (wallDamage != null){
                wallDamage.SetActive(false);
            }
            FindObjectOfType<EventControl>().ShowWinTutorial();
            
        }
        else
        {
            FindObjectOfType<EventControl>().ShowWinPanel();
            Debug.Log("Winner Winner Chicken Dinner!");
            
            float playTime = Time.time - startTime; // Calculate the play duration
            AnalyticsManager.trackProgress(currentLevel, true, playTime); // Pass playtime to AnalyticsManager
            
            cameraMovement.StopCamera();
            sceneRotation.StopRotation();
            isGameStarted = false; // Reset the game state
        }
    }
}
