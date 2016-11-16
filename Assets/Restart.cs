using UnityEngine;
using UnityEngine.SceneManagement;

/* Class: Restart
 * Loads sceneName if the touchpad is pressed down.
 */
[RequireComponent(typeof(SteamVR_TrackedObject))]
public class Restart : MonoBehaviour {
    public string sceneName;

    SteamVR_TrackedObject trackedObj;
    SteamVR_Controller.Device controller;

    void Start() {
        trackedObj = GetComponent<SteamVR_TrackedObject>();
        controller = SteamVR_Controller.Input((int)trackedObj.index);
    }
	
	// Update is called once per frame
	void Update () {
	    if (controller.GetPressDown(SteamVR_Controller.ButtonMask.Touchpad))
            SceneManager.LoadScene(sceneName);
	}
}
