using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spotlightBlink : MonoBehaviour
{
    private Light lightRef = null;
    [SerializeField] private float blinkInterval;
    [SerializeField] private float blinkDuration;
    [SerializeField] private float blinkFrequency;

    private bool isBlinking = false;
    private float timer;
    private float frequency;

    // Start is called before the first frame update
    void Start()
    {
        lightRef = gameObject.GetComponent<Light>();
        timer = blinkInterval;
    }

    // Update is called once per frame
    void Update()
    {
        if(!isBlinking)
        {
            if(timer > 0)
            {
                timer -= Time.deltaTime;
            } 
            else
            {
                isBlinking = !isBlinking;
                timer = blinkDuration;
            }
        } 
        else
        {
            //if still blinking
            if(timer > 0)
            {
                //it is time to blink on or off
                if(frequency < 0)
                {
                    lightRef.enabled = !lightRef.enabled;
                    frequency = blinkFrequency;
                }

                timer -= Time.deltaTime;
                frequency -= Time.deltaTime;
            }
            else
            {
                isBlinking = !isBlinking;
                lightRef.enabled = true;
                timer = blinkInterval;
                frequency = blinkFrequency;
            }
        }
    }
}
