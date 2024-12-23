using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WoodRockController : MonoBehaviour
{
    public static int woodCount = 0;
    public static int rockCount = 0;
    public TextMeshProUGUI woodText;
    public TextMeshProUGUI rockText;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        woodText.text=woodCount.ToString();
        rockText.text=rockCount.ToString();
    }
}
