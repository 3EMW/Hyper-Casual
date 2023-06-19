using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawnerController : MonoBehaviour
{
    public GameObject playerGO;
    float playerSpeed = 5;
    float xSpeed;
    float maxXPosition = 3f;
    public List<GameObject> playersList = new List<GameObject>();
    bool isPlayersMoving;

    public AudioSource playerSpawnerAudioSource;
    public AudioClip gateClip, congClip, failClip, shootClip;
    // Start is called before the first frame update
    void Start()
    {
        
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isPlayersMoving == false)
        {
            return;
        }
        float touchX = 0;
        float newXValue = 0;
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
        {
            xSpeed = 300f;
            touchX = Input.GetTouch(0).deltaPosition.x / Screen.width;
        }
        else if (Input.GetMouseButton(0))
        {
            xSpeed = 500f;
            touchX = Input.GetAxis("Mouse X");
        }
        newXValue = transform.position.x + xSpeed * touchX * Time.deltaTime;
        newXValue = Mathf.Clamp(newXValue, -maxXPosition, maxXPosition);
        Vector3 playerNewPosition = new Vector3(newXValue, transform.position.y, transform.position.z + playerSpeed * Time.deltaTime);
        transform.position = playerNewPosition;
    }
    public void SpawnPlayer(int gateValue,GateType gateType)
    {
        PlayAudio(gateClip);
        if (gateType == GateType.additionType)
        {
            for (int i = 0; i < gateValue; i++)
            {
                GameObject newPlayerGO = Instantiate(playerGO, GetPlayerPosition(), Quaternion.identity, transform);
                playersList.Add(newPlayerGO);
            }
        }
        else if (gateType == GateType.multiplyType)
        {
            int newPlayerCount = (playersList.Count * gateValue) - playersList.Count;
            for (int i = 0; i < newPlayerCount; i++)
            {
                GameObject newPlayerGO = Instantiate(playerGO, GetPlayerPosition(), Quaternion.identity, transform);
                playersList.Add(newPlayerGO);
            }
        }   
    }
    public Vector3 GetPlayerPosition()
    {
        Vector3 position = Random.insideUnitSphere * 0.1f;
        Vector3 newPos = transform.position + position;
        newPos.y = 0.5f;
        return newPos;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "FinishLine")
        {
            Debug.Log("Finish");
            StopBackgroundMusic();
            PlayAudio(congClip);
            StartAllCopsIdleAnima();
            isPlayersMoving = false;
            GameManager.instance.ShowSuccPanel();
            //Stop moving.
        }
    }

    public void PlayerGotKilled(GameObject playerGO)
    {
        playersList.Remove(playerGO);
        Destroy(playerGO);
        CheckPlayerCount();
    }
    public void CheckPlayerCount()
    {
        if (playersList.Count <= 0)
        {
            StopBackgroundMusic();
            PlayAudio(failClip);
            StopPlayer();
            GameManager.instance.ShowFailPanel();
        }
    }

    public void ZombieDetected(GameObject target)
    {
        isPlayersMoving = false;
        LookAtZombies(target);
        StartAllCopsShooting();
        PlayAudio(shootClip);
        //look at zombies
    }

    private void LookAtZombies(GameObject target)
    {
        Vector3 dir = target.transform.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(dir);
        lookRotation.x = 0;
        lookRotation.z = 0;
        transform.rotation = lookRotation;
    }
    private void LookAtForward()
    {
        transform.rotation = Quaternion.identity;
    }
    public void AllZombiesKilled()
    {
        LookAtForward();
        MovePlayer();
        //polisler tekrar iletiye bakacak
        //yürümeye devam edecekler
    }
    public void MovePlayer()
    {
        StartAllCopsRunAnima();
        isPlayersMoving = true;
    }
    public void StopPlayer()
    {
        isPlayersMoving = false;
    }
    public void StartAllCopsShooting()
    {
        for(int i = 0; i < playersList.Count; i++)
        {
            PlayerController cop = playersList[i].GetComponent<PlayerController>();
            cop.StartShooting();
        }
    }
    public void StartAllCopsRunAnima()
    {
        for(int i = 0; i < playersList.Count; i++)
        {
            PlayerController cop = playersList[i].GetComponent<PlayerController>();
            cop.StopShooting();
        }
    }
    public void StartAllCopsIdleAnima()
    {
        for (int i = 0; i < playersList.Count; i++)
        {
            PlayerController cop = playersList[i].GetComponent<PlayerController>();
            cop.StartIdleAnima();
        }
    }
    private void PlayAudio(AudioClip clip)
    {
        if (playerSpawnerAudioSource!=null)
        {
            playerSpawnerAudioSource.PlayOneShot(clip, 0.3f);
        }
    }
    private void StopBackgroundMusic()
    {
        Camera.main.GetComponent<AudioSource>().Stop();
    }
}
