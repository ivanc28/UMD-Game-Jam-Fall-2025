using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GemTracker : MonoBehaviour
{
    public BigLamp lamp;
    private TextMeshProUGUI text;
    // Start is called before the first frame update
    void Start()
    {
        if(lamp == null)
        {
            lamp = FindObjectOfType<BigLamp>();
        }    
        text = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        text.text = $"{lamp.curGemCount}/{BigLamp.GEM_COUNT}";
        
    }
}
