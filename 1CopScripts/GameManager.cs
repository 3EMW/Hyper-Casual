using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public GameObject MainPanel;
    public GameObject FailPanel;
    public GameObject SuccPanel;
    public static GameManager instance;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void StartButtonTapped()
    {
        MainPanel.gameObject.SetActive(false);
        GameObject playerSpawnerGO = GameObject.FindGameObjectWithTag("PlayerSpawner");
        PlayerSpawnerController playerSpawner = playerSpawnerGO.GetComponent<PlayerSpawnerController>();
        playerSpawner.MovePlayer();
    }
    public void ShowFailPanel()
    {
        FailPanel.gameObject.SetActive(true);
    }
    public void RestartButtonTapped()
    {
        LevelLoader.instance.GetLevel();
    }
    public void ShowSuccPanel()
    {
        SuccPanel.gameObject.SetActive(true);

    }
    public void NextLevelButtonTapped()
    {
        LevelLoader.instance.NextLevel();
 
    }
}
