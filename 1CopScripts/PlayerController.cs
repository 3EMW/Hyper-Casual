using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject bulletGO;
    public Transform bulletSpawnTransform;
    private float bulletSpeed = 13f;
    bool isShootingOn;
    Animator playerAnimator;
    Transform playerSpawnerCenter;
    float goingToCenterSpeed = 1f;
    // Start is called before the first frame update
    void Start()
    {
        playerSpawnerCenter = transform.parent.gameObject.transform;
        playerAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = Vector3.MoveTowards(transform.position, playerSpawnerCenter.position, Time.fixedDeltaTime * goingToCenterSpeed);
    }
    public void StartShooting()
    {
        StartShootingAnima();      //silah doðrultma animasyonu
        isShootingOn = true;
        StartCoroutine(Shooting());
    }
    public void StopShooting()
    {
        isShootingOn = false;
        StartRunAnima();
    }
    IEnumerator Shooting() //bullet spawnlama
    {
        while (isShootingOn)  //true ise
        {
            yield return new WaitForSeconds(0.5f); //X Saniye bekle sonra ...
            Shoot(); //Ateþ et
            yield return new WaitForSeconds(2f); //X Saniye bekle ve tekrar ateþ et.
        }
    }
    private void Shoot()
    {
        GameObject bullet = Instantiate(bulletGO, bulletSpawnTransform.position, Quaternion.identity);
        Rigidbody bulletRB = bullet.GetComponent<Rigidbody>();
        bulletRB.velocity = transform.forward * bulletSpeed;
    }
    private void StartShootingAnima()
    {
        playerAnimator.SetBool("isShootingOn", true);
        playerAnimator.SetBool("isRunning", false);
    }
    private void StartRunAnima()
    {
        playerAnimator.SetBool("isShootingOn", false);
        playerAnimator.SetBool("isRunning", true);
    }
    public void StartIdleAnima()
    {
        playerAnimator.SetBool("isRunning", false);
        playerAnimator.SetBool("isLevelFinished", true);
    }
}
