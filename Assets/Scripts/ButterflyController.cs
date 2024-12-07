using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButterflyController : MonoBehaviour {

    public bool isPresent;
    public float timeToArrived;


    public bool setStartPosition;
    public Vector3 startPosition;
    public bool setTargetPosition;
    public Vector3 targetPosition;

    private float time;
    public GameObject butterflyGameObject;

    // Start is called before the first frame update
    void Start() {
        isPresent = false;
        setStartPosition = false;
        setTargetPosition = false;
        timeToArrived = 0.5f;

        butterflyGameObject = gameObject.transform.Find("Butterfly").gameObject;
    }

    // Update is called once per frame
    void Update() {
        if (setStartPosition) {
            setStartPositionWithNowPosition();
            setStartPosition = false;
        }
        if (setTargetPosition) {
            setTargetPositionWithNowPosition();
            setTargetPosition = false;
        }

        if (isPresent) {
            time += Time.deltaTime/timeToArrived;
            butterflyGameObject.transform.position = Vector3.Lerp(startPosition, targetPosition, time);
        }
    }

    void setStartPositionWithNowPosition() {
        startPosition = butterflyGameObject.transform.position;
    }

    void setTargetPositionWithNowPosition() {
        targetPosition = butterflyGameObject.transform.position;
    }
}
