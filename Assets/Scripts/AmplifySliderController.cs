using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AmplifySliderController : MonoBehaviour
{
    public List<Sprite> CDSprite;
    public Image CDTextImage;
    public Image CDBGImage;
    public GradeCounter gradeCounter;
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
			CDTextImage.sprite = CDSprite[(int) timeRemaining + 1];
			CDTextImage.color = new Color(255f, 255f, 255f);
            CDBGImage.fillAmount = timeRemaining - (int) timeRemaining;
		} else {
            timeRemaining = TIMER;
            countDownText.text = "";
			CDTextImage.sprite = null;
            CDTextImage.color = new Color(255f, 255f, 255f, 0f);
            CDBGImage.fillAmount = 0f;
            gradeCounter.AllDandelionDead();
		}

        if (timeRemaining <= 0f) {
            gameObject.SetActive(false);
            systemController.started = true;
        }
    }
}
