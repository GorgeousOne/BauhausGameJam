using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Highscore : MonoBehaviour {
    
    [SerializeField] private TMP_Text label;
    
    public void UpdateScore(int score) {
        label.SetText(score.ToString().PadLeft(4, '0'));
    }
}
