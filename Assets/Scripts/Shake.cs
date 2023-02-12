using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shake : MonoBehaviour
{
    public Vector3 anchor;
    public float shakeIntensity;
    public float shakeDecreaseSpeed = 1f;
    public bool doShakeVertical = true;
    public bool doShakeHorizontal = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 newPos = new Vector3(0, 0, anchor.z);
        if(doShakeHorizontal){
            newPos += Vector3.right * Random.Range(-shakeIntensity, shakeIntensity);
        }
        if(doShakeVertical){
            newPos += Vector3.up * Random.Range(-shakeIntensity, shakeIntensity);
        }
        transform.localPosition = newPos;
        shakeIntensity -= shakeDecreaseSpeed * Time.deltaTime;
        if(shakeIntensity < 0){
            shakeIntensity = 0;
        }
    }
}
