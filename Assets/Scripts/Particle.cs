using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle : MonoBehaviour
{

    public float opacity = 255f;
    public float opacityDecreaseSpeed = 10f;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        opacity -= opacityDecreaseSpeed * Time.deltaTime;
        Color curColor = GetComponent<SpriteRenderer>().color;
        GetComponent<SpriteRenderer>().color = new Color(curColor.r, curColor.g, curColor.b, opacity);
    }
}
