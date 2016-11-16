using UnityEngine;
using System.Collections;

/* Class: WalkInsideCircle
 * Moves a NavMeshAgent around a circle.
 */
[RequireComponent (typeof (NavMeshAgent))]
public class WalkInsideCircle : MonoBehaviour {
    // The following are handled by a custom editor, WalkInsideCircleEditor.
    public bool randomlyPause;
    public bool randomizeSpeed;
    public float circleRadius;

    public float minSpeed, maxSpeed;
    public float minPause, maxPause;

    // The origin of the circle.
    public Vector3 origin;

    private float minCycleTime = 0.5f;
    private float maxCycleTime = 3.0f;

    private NavMeshAgent agent;
    private IEnumerator walkCycle;

    void OnDisable() {
        StopCoroutine(walkCycle);
    }

	// Use this for initialization
	void Start () {
        agent = GetComponent<NavMeshAgent>();
        // Randomize the first position of the agent.
        transform.position = GetDestination();

        Walk();
	}

    void Walk() {
        if (randomizeSpeed)
            agent.speed = Random.Range(minSpeed, maxSpeed);

        walkCycle = CycleDestinations();
        StartCoroutine(walkCycle);
    }

    IEnumerator CycleDestinations() {
        while(true) {
            agent.destination = GetDestination();

            yield return new WaitForSeconds(Random.Range(minCycleTime, maxCycleTime));
            if (randomlyPause) yield return new WaitForSeconds(Random.Range(minPause, maxPause));
            if (randomizeSpeed) agent.speed = Random.Range(minSpeed, maxSpeed);
        }
    }

    /* Function: GetDestination
     * Returns a Vector3 destination, where the x,z coordinates are a random point on a circle
     * and the y coordinate is equal to the gameObject's y position.
     */
    private Vector3 GetDestination() {
        Vector3 destination;
        Vector2 randomXZ = (Random.insideUnitCircle * circleRadius);
        destination.x = randomXZ.x;
        destination.y = transform.position.y;
        destination.z = randomXZ.y;

        return  destination + origin;
    }
}
