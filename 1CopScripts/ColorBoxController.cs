using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorBoxController : MonoBehaviour
{
    public Material boxMat;
    private GameObject playerGO;
    private PlayerController player;
    // Start is called before the first frame update
    void Start()
    {
        playerGO = GameObject.FindGameObjectWithTag("Player");
        player = playerGO.GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag=="Player")
        {
            player.TouchedToColorBox(boxMat);
            Destroy(gameObject);
        }
    }
}
