using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SystemController : MonoBehaviour {
    public float amplify;
    public bool started;
    public bool finished;

    public Text startText; 
    public Text finishText;

    // timer
    public float TIMER;
    private float timeRemaining;

    // amplify range
    public float MIN_AMPLIFY;
    public float MAX_AMPLIFY;

    // Start is called before the first frame update
    void Start() {
        started = false;
        finished = false;

        TIMER = 4f;
        timeRemaining = TIMER;

        MAX_AMPLIFY = 0.3f;
        MIN_AMPLIFY = 0.2f;
    }

    // Update is called once per frame
    void Update() {
        startText.enabled = started;
        finishText.enabled = finished;

        if (!started || finished) {
            return;
        }

        if (isWithinRange()) {
            timeRemaining -= Time.deltaTime;
        } else {
            timeRemaining = TIMER;
        }

        if (timeRemaining <= 0f) {
            finished = true;
        }
    }

    public bool isWithinRange() {
        return (amplify >= MIN_AMPLIFY && amplify <= MAX_AMPLIFY);
    }
}
