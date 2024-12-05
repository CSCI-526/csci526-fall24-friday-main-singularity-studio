using UnityEngine;

public class RandomMovement : MonoBehaviour
{
    public float radius = .01f;      // Radius for random movement
    public float speed = .5f;       // Speed of movement
    public float waitTime = 2f;    // Time to wait at each point

    private Vector3 startPosition; // Original position
    private Vector3 targetPosition;
    private LevelCompletion levelCompletion;

    private void Start()
    {
        Random.InitState(System.DateTime.Now.Millisecond + GetInstanceID());
        startPosition = transform.position;
        StartCoroutine(MoveRandomly());
        GameObject player = GameObject.FindWithTag("Player");
        levelCompletion = player.GetComponent<LevelCompletion>();
    }

    private void Update(){
        if(levelCompletion.isWon){
            startPosition = transform.position;
            StartCoroutine(MoveRandomly());
        }
    }

    private System.Collections.IEnumerator MoveRandomly()
    {
        while (true)
        {
            // Generate a random point within the radius
            Vector2 randomPoint = Random.insideUnitCircle * radius;
            targetPosition = startPosition + new Vector3(randomPoint.x, randomPoint.y, 0f);

            // Move towards the target position
            while (Vector3.Distance(transform.position, targetPosition) > 0.01f)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
                yield return null;
            }

            // Wait for a moment at the target position
            yield return new WaitForSeconds(waitTime);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(startPosition != Vector3.zero ? startPosition : transform.position, radius);
    }
}
