using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmplifySliderController : MonoBehaviour
{
    private SystemController systemController;

    private RectTransform actualAmplifyRectTransform;
    private float MAX_Y;
    private float MIN_Y;
    private float OFFSET;

    // timer
    private float TIMER;
    private float timeRemaining;

    // Start is called before the first frame update
    void Start() {
        actualAmplifyRectTransform = gameObject.transform.GetChild(1).gameObject.GetComponent<RectTransform>();
        systemController = GameObject.Find("System").GetComponent<SystemController>();
        systemController.amplify = 0.0f;

        MAX_Y = 240;
        MIN_Y = -240;
        OFFSET = MAX_Y - MIN_Y;

        TIMER = 1.5f;
        timeRemaining = TIMER;
    }

    // Update is called once per frame
    void Update() {
        float posY = (systemController.amplify * OFFSET) + MIN_Y;
        actualAmplifyRectTransform.anchoredPosition = new Vector2(0, posY);

        if (systemController.isWithinRange()) {
            timeRemaining -= Time.deltaTime;
        } else {
            timeRemaining = TIMER;
        }

        if (timeRemaining <= 0f) {
            gameObject.SetActive(false);
            systemController.started = true;
        }
    }
}
