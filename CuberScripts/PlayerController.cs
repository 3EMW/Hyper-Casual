using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    float playerSpeed = 10f;
    float xSpeed;
    float maxXValue = 4f;
    bool isPlayerMoving;
    public GameObject headBoxGO;
    private ScaleCalculator scaleCalculator;
    Renderer headBoxRenderer;
    private Material CurrentHead;
    public Material Warning;
    Animator playerAnim;
    public AudioSource playerAudioSource;
    public AudioClip gateClip, colorBoxClip, obstacleClip, successClip;
    // Start is called before the first frame update
    void Start()
    {
        
        scaleCalculator = new ScaleCalculator();
        headBoxRenderer = headBoxGO.transform.GetChild(0).gameObject.GetComponent<Renderer>();
        CurrentHead = headBoxRenderer.material;
        playerAnim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isPlayerMoving == false)
        {
            return;
        }
        float touchX = 0;
        float newXValue;
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
        {
            xSpeed = 300;
            touchX = Input.GetTouch(0).deltaPosition.x / Screen.width;
        }
        else if (Input.GetMouseButton(0))
        {
            xSpeed = 400f;
            touchX = Input.GetAxis("Mouse X");
        }
        newXValue = transform.position.x + xSpeed * touchX * Time.deltaTime;
        newXValue = Mathf.Clamp(newXValue, -maxXValue, maxXValue);
        Vector3 playerNewPosition = new Vector3(newXValue, transform.position.y, transform.position.z + playerSpeed * Time.deltaTime);
        transform.position = playerNewPosition;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag=="Finish")
        {
            isPlayerMoving = false;  //karakteri durdur
            StartIdleAnim();
            StopBackgroundMusic();
            GameManager.instance.ShowSuccMenu();
            PlayAudio(successClip, 1f);
        }
    }
    public void PassedGate(GateType gateType, int gateValue)
    {
        PlayAudio(gateClip, 1f);
        headBoxGO.transform.localScale = scaleCalculator.CalculatePlayerHeadSize(gateType, gateValue, headBoxGO.transform);
        Debug.Log("kafan1milyon");
    }
    public void TouchedToColorBox(Material boxMat)
    {
        PlayAudio(obstacleClip, 0.7f);
        headBoxRenderer.material = boxMat;
        CurrentHead = boxMat;
    }
    public void TouchedToObstacle()
    {
        headBoxGO.transform.localScale = scaleCalculator.DecreasePlayerHeadSize(headBoxGO.transform);
        StartCoroutine(StartRedBlink());
    }
    private IEnumerator StartRedBlink()
    {
        headBoxGO.transform.GetChild(0).GetComponent<MeshRenderer>().material = Warning;
        yield return new WaitForSeconds(0.5f);
        headBoxGO.transform.GetChild(0).GetComponent<MeshRenderer>().material = CurrentHead;
    }
    public void GameStarted()
    {
        isPlayerMoving = true;
        StartRunAnim();
    }
    private void StartRunAnim()
    {
        playerAnim.SetBool("isIdleOn", false);
        playerAnim.SetBool("isRunningOn", true);
    }
    private void StartIdleAnim()
    {
        playerAnim.SetBool("isIdleOn", true);
        playerAnim.SetBool("isRunningOn", false);
    }
    private void PlayAudio(AudioClip clip,float volume)
    {
        playerAudioSource.PlayOneShot(clip, volume);
    }
    private void StopBackgroundMusic()
    {
        Camera.main.GetComponent<AudioSource>().Stop();
    }
}
