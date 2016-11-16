using UnityEngine;
using System.Collections;

/* Class: Medusa
 * Encapsulates Medusa-specific actions. Medusa is deemed as in a victim's gaze if Medusa
 * is in view (ie in screen space) and there is a line of sight (ie unobstructed raycast).
 */
public class Medusa : MonoBehaviour {
    // Alert other objects when Medusa is killed or wins.
    public delegate void MedusaAction();
    public static event MedusaAction MedusaKilled;
    public static event MedusaAction MedusaWon;

    // The camera for the gaze of a Medusa victim.
    [SerializeField]
    Camera victimGaze;
    // Transforms representing the victim and Medusa eyes.
    [SerializeField]
    Transform victimEyes;
    [SerializeField]
    Transform medusaEyes;
    // Margins for padding the detection of Medusa in screen space.
    [SerializeField]
    float viewMarginX;
    [SerializeField]
    float viewMarginY;

    private bool alive = true;

    void Update() {
        if (alive) { 
            // Only rotate along the y-axis.
            Vector3 lookAtPoint = new Vector3(victimEyes.position.x, transform.position.y, victimEyes.position.z);
            transform.LookAt(lookAtPoint);

            bool victimDied = IsMedusaOnScreen() && IsMedusaInLineOfSight();
            if (victimDied) {
                EndGame();
                MedusaWon();
            }
        }
    }
	
    /* Function: IsMedusaInLineOfSight
     * Casts a ray from Medusa's eye transform to the victim's eye transform and
     * returns true if there is an unobstructed ray, ie clear line of sight.
     */
    private bool IsMedusaInLineOfSight() {
        Vector3 eyeDifferenceVector = victimEyes.position - medusaEyes.position;
        float distance = eyeDifferenceVector.magnitude;

        RaycastHit hit;
        Ray ray = new Ray(medusaEyes.position, eyeDifferenceVector.normalized);

        return (Physics.Raycast(ray, out hit, distance) && hit.collider.gameObject.tag == "Player");
    }

    /* Function: IsMedusaOnScreen
     * Converts Medusa's eye transform to screen space and returns true if in screen space.
     * The z coordinate must be positive (negatives are behind the screen) and the x,y coordinates
     * must be within the screen width and height margins.
     */
    private bool IsMedusaOnScreen() {
        Vector3 medusaScreenPoint = victimGaze.WorldToScreenPoint(medusaEyes.position);

        return medusaScreenPoint.z > 0f &&
            medusaScreenPoint.x > viewMarginX && medusaScreenPoint.x < (victimGaze.pixelWidth - viewMarginX) &&
            medusaScreenPoint.y > viewMarginY && medusaScreenPoint.y < (victimGaze.pixelHeight - viewMarginY);
    }

    void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.name == "Sword") {
            EndGame();
            MedusaKilled();
        }
    }

    /* Function: EndGame
     * Disables Medusa's gaze-checking and movement.
     */
    void EndGame() {
        GetComponent<NavMeshAgent>().enabled = false;
        GetComponent<WalkInsideCircle>().enabled = false;
        alive = false;
    }
}
