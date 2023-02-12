using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateButton : MonoBehaviour
{
    public bool haveButtonDisplay = true;
    public bool doRotation = true;
    public bool stopButtonDisplayOnPress = true;
    public bool spawnOrbs = true;
    public bool propellButtonWhenLaunchingOrbs = true;
    public bool doSFX = true;
    public bool doParticles = true;
    public bool doScreenShake = true;
    public bool doConnector = true;
    public bool doDisplayBounce = true;
    public bool doButtonBounce = true;



    public float speed = 100f;
    
    public float minSpeed = 0f;
    public float maxSpeed = 100f;

    public float speedStopSpeed = 2f;
    public float speedStartSpeed = 2f;

    public Transform pivot;
    public GameObject buttonDisplay;

    public SpriteRenderer buttonDispSprite;

    public Sprite heldDisplay;
    public Sprite unHeldDisplay;

    public GameObject button;
    public Sprite heldButton;
    public Sprite unHeldButton;
    public SpriteRenderer buttonSprite;

    public GameObject orb;
    public float orbChargeSpeed = 1f;
    public float orbStartSize = 0f;
    public float orbMaxSize = .1f;
    public float orbLaunchSpeed = 1f;
    public GameObject currentOrb;
    public float currentOrbSize = 0;
    public float orbLaunchAngleOffset = 45;

    public float buttonLaunchSpeed = 1750;
    public float buttonLaunchAngleOffset = 45;

    public GameObject background;

    public AudioSource chargeUpSFX;
    public AudioSource shootSFX;
    public float shootPitchMultiplyer;
    public float shootPitchOffset = .8f;
    public float chargeUpVolumeMultiplyer;
    public float shootVolumeMultiplyer;

    public GameObject particle;
    public float particleNumberPerBurstSizeMultiplyer = 50;
    public float particleLaunchSpeed = 10f;

    public float opacityDecreaseMinMultiplyer = 10f;
    public float opacityDecreaseMaxMultiplyer = 5f;
    public float particleSizeMultiplyer = 1f;

    public float camShakeIntensityMultiplyer = 1f;
    public float camShakeLaunchIntensityMultiplyer = 5f;

    public GameObject connector;

    public Color[] connectorColors = {};
    public int curConnectorColor = 0;
    public float connectorGrowMultiplyer = 1;
    public Vector3 displayAnchor;
    public float displayDistance;
    public float displayDistFreq = 0;
    public float displayLaunchFreq = 10;
    public float time;
    public float displayDistFreqDecreaseSpeed = 1f;
    public float displaySecondaryDist = 0;
    public float displaySecondaryDistFreqDecreaseSpeed = 1f;
    public float disp2ndDistOnLaunch = 10f;
    
    
    public float anchorButtonSize;
    public float buttonSize;
    public float buttonSizeFreq = 0;
    public float buttonLaunchFreq = 10;
    public float buttonSizeFreqDecreaseSpeed = 1f;
    public float buttonSecondarySize = 0;
    public float buttonSecondarySizeFreqDecreaseSpeed = 1f;
    public float button2ndSizeOnLaunch = 10f;

    public GameObject mainButtonDisplay;

    // Start is called before the first frame update
    void Start()
    {
        pivot = transform.parent;
        buttonDispSprite = buttonDisplay.GetComponent<SpriteRenderer>();
        buttonSprite = mainButtonDisplay.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;

        if(doConnector){
            connector.SetActive(true);
        } else {
            connector.SetActive(false);
        }

        if(propellButtonWhenLaunchingOrbs){
            background.SetActive(true);
                background.GetComponent<MeshRenderer>().material.SetTextureOffset(Shader.PropertyToID("_MainTex"), button.transform.position);
        } else {
            background.SetActive(false);
        }

        if(Input.GetMouseButton(0) || Input.GetKey(KeyCode.Space)){
            buttonSprite.sprite = heldButton;
        } else {
            buttonSprite.sprite = unHeldButton;
        }
        if(haveButtonDisplay){
            buttonDisplay.SetActive(true);
            if(Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space)){
                if(doSFX){
                    chargeUpSFX.pitch = Random.Range(.9f, 1.1f);
                    chargeUpSFX.Play();
                }
                if(spawnOrbs){
                    currentOrb = Instantiate(orb,transform.position, pivot.transform.rotation);
                    currentOrbSize = orbStartSize;
                }
            }
            if(Input.GetMouseButton(0) || Input.GetKey(KeyCode.Space)){
                if(stopButtonDisplayOnPress){
                    speed -= speedStopSpeed * Time.deltaTime;
                }
                buttonDispSprite.sprite = heldDisplay;
                if(spawnOrbs){
                    
                    currentOrbSize += orbChargeSpeed * Time.deltaTime;
                    currentOrbSize = Mathf.Clamp(currentOrbSize, orbStartSize, orbMaxSize);
                    currentOrb.transform.position = transform.position;
                    currentOrb.transform.localScale = new Vector2(currentOrbSize, currentOrbSize);
                    if(doSFX){
                        chargeUpSFX.volume = currentOrbSize * chargeUpVolumeMultiplyer;
                    }
                }
                if(doScreenShake){
                    Camera.main.gameObject.GetComponent<Shake>().shakeIntensity = camShakeIntensityMultiplyer * currentOrbSize;
                }
                if(doConnector){
                    curConnectorColor = 0;
                }
            } else {
                speed += speedStartSpeed * Time.deltaTime;
                buttonDispSprite.sprite = unHeldDisplay;
            }

            if(Input.GetMouseButtonUp(0) || Input.GetKeyUp(KeyCode.Space)){
                if(spawnOrbs){
                    currentOrb.GetComponent<Orb>().launched = true;
                    float xChange = Mathf.Cos((pivot.rotation.eulerAngles.z + orbLaunchAngleOffset) * Mathf.PI / 180) * currentOrbSize;
                    float yChange = Mathf.Sin((pivot.rotation.eulerAngles.z  + orbLaunchAngleOffset) * Mathf.PI / 180) * currentOrbSize;
                    currentOrb.GetComponent<Rigidbody2D>().AddForce(new Vector2(xChange, yChange) * orbLaunchSpeed);
                    if(propellButtonWhenLaunchingOrbs){
                        button.GetComponent<Rigidbody2D>().AddForce(new Vector2(xChange, yChange) * -buttonLaunchSpeed);
                    }
                    if(doSFX){
                        chargeUpSFX.Stop();
                        float curShootPitch = currentOrbSize * shootPitchMultiplyer + shootPitchOffset + Random.Range(-.125f, .125f);
                        shootSFX.pitch = curShootPitch;
                        shootSFX.volume = currentOrbSize * shootVolumeMultiplyer;
                        shootSFX.Play();
                    }

                    if(doParticles){
                        for(int p = 0; p < Random.Range(2, currentOrbSize * particleNumberPerBurstSizeMultiplyer); p++){
                            GameObject curParticle = Instantiate(particle, transform.position, pivot.transform.rotation);
                            curParticle.transform.position += new Vector3(Random.Range(-currentOrbSize, currentOrbSize), Random.Range(-currentOrbSize * 10, currentOrbSize * 10), 0);
                            float curParticleSize = currentOrbSize * particleSizeMultiplyer * Random.Range(.75f, 1.25f);
                            curParticle.transform.localScale = Vector2.one * curParticleSize;
                            curParticle.GetComponent<Rigidbody2D>().AddForce(new Vector2(xChange, yChange) * particleLaunchSpeed);
                            curParticle.GetComponent<Particle>().opacityDecreaseSpeed = Random.Range(currentOrbSize * opacityDecreaseMinMultiplyer, currentOrbSize * opacityDecreaseMaxMultiplyer);
                        }
                    }
                    if(doScreenShake){
                        Camera.main.gameObject.GetComponent<Shake>().shakeIntensity = camShakeLaunchIntensityMultiplyer * currentOrbSize;
                    }
                    if(doConnector){
                        connector.GetComponent<SpriteRenderer>().enabled = true;
                        connector.GetComponent<SpriteRenderer>().color = connectorColors[curConnectorColor];
                        connector.transform.localScale = new Vector3(connector.transform.localScale.x, currentOrbSize * connectorGrowMultiplyer);
                        curConnectorColor += 1;
                    }
                    if(doDisplayBounce){
                        displayDistFreq = displayLaunchFreq * currentOrbSize;
                        displaySecondaryDist = disp2ndDistOnLaunch * currentOrbSize;
                        time = 0;
                    }
                    if(doButtonBounce){
                        buttonSizeFreq = buttonLaunchFreq * currentOrbSize;
                        buttonSecondarySize = button2ndSizeOnLaunch * currentOrbSize;
                        time = 0;
                    }
                }
            } else {
                if(doConnector){
                    connector.GetComponent<SpriteRenderer>().enabled = false;
                }
            }

            speed = Mathf.Clamp(speed, minSpeed, maxSpeed);
            if(doRotation){
                pivot.Rotate(Vector3.forward * speed * Time.deltaTime, Space.Self);
            }
            if(doDisplayBounce){
                displayDistance = Mathf.Sin(displayDistFreq);
                displayDistFreq -= displayDistFreqDecreaseSpeed * Time.deltaTime;
                if(displayDistFreq < 0){
                    displayDistFreq = 0;
                }
                displaySecondaryDist -= displaySecondaryDistFreqDecreaseSpeed * Time.deltaTime;
                if(displaySecondaryDist < 0){
                    displaySecondaryDist = 0;
                }
            }
            if(doButtonBounce){
                buttonSize = Mathf.Sin(buttonSizeFreq);
                buttonSizeFreq -=  buttonSizeFreqDecreaseSpeed * Time.deltaTime;
                if(buttonSizeFreq < 0){
                    buttonSizeFreq = 0;
                }
                buttonSecondarySize -= buttonSecondarySizeFreqDecreaseSpeed * Time.deltaTime;
                if(buttonSecondarySize < 0){
                    buttonSecondarySize = 0;
                }
            }
            transform.localPosition = new Vector3(displayAnchor.x + displayDistance + displaySecondaryDist, displayAnchor.y, displayAnchor.z);
            mainButtonDisplay.transform.localScale = new Vector3(anchorButtonSize + buttonSize + buttonSecondarySize, anchorButtonSize + buttonSize + buttonSecondarySize, 0);
            buttonDisplay.transform.position = transform.position;
            mainButtonDisplay.transform.position = button.transform.position;
        } else {
            buttonDisplay.SetActive(false);
        }
    }
}
