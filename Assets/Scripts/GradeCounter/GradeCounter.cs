using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GradeCounter : MonoBehaviour
{
    public int SeedPerDandelion = 50;
    int AliveDandelion = 0;
    public TextMeshProUGUI GradeText;

	public void NewDandelion()
    {
        Debug.Log("NewDandelion");
        AliveDandelion++;
	}

    public void AllDandelionDead()
    {
        Debug.Log("AllDandelionDead");
        AliveDandelion = 0;
	}

    public int GetGrade()
    {
        return AliveDandelion * SeedPerDandelion;
    }

    public void SetGradeUI()
    {
		GradeText.text = "" + GetGrade();
	}
}
