using UnityEngine;
using UnityEngine.UI;

/* Class: InGameDisplay
 * Handles in game display via a canvas.
 */
public class InGameDisplay : MonoBehaviour {
    [SerializeField]
    Text gameOverText;

    // Use this for initialization
    void OnEnable() {
        Medusa.MedusaKilled += DisplayEndGame;
        Medusa.MedusaWon += DisplayEndGame;
    }

    void OnDisable() {
        Medusa.MedusaKilled -= DisplayEndGame;
        Medusa.MedusaWon -= DisplayEndGame;
    }

    /* DisplayEndGame
     * If Medusa is killed or kills the  player, display game over text.
     */
    void DisplayEndGame() {
        gameOverText.enabled = true;
    }
}
