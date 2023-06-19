using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameObject StartMenuPanel;
    public GameObject SuccPanel;
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
        StartMenuPanel.gameObject.SetActive(false);
        GameObject playerGO = GameObject.FindGameObjectWithTag("Player");
        PlayerController player = playerGO.GetComponent<PlayerController>();
        player.GameStarted();
    }
    public void NextButtonTappped()
    {
        SuccPanel.gameObject.SetActive(false);
        LevelController.instance.NextLevel();
    }
    public void ShowSuccMenu()
    {
        SuccPanel.gameObject.SetActive(true);
    }
}
