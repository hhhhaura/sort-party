using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AmplifySliderController : MonoBehaviour
{
    private Text countDownText;
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
        countDownText = gameObject.GetComponentInChildren<Text>();
        actualAmplifyRectTransform = gameObject.transform.GetChild(1).gameObject.GetComponent<RectTransform>();
        systemController = GameObject.Find("System").GetComponent<SystemController>();
        systemController.amplitude = 0.0f;

        MAX_Y = 240;
        MIN_Y = -240;
        OFFSET = MAX_Y - MIN_Y;

        TIMER = 3.2f;
        timeRemaining = TIMER;
    }

    // Update is called once per frame
    void Update() {
        float posY = (systemController.amplitude * OFFSET) + MIN_Y;
        actualAmplifyRectTransform.anchoredPosition = new Vector2(0, posY);

        if (systemController.isWithinRange()) {
            timeRemaining -= Time.deltaTime;
            countDownText.text = "" + (int)timeRemaining;
        } else {
            timeRemaining = TIMER;
            countDownText.text = "";
        }

        if (timeRemaining <= 0f) {
            gameObject.SetActive(false);
            systemController.started = true;
        }
    }
}
