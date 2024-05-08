using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerFire : MonoBehaviour
{
    [SerializeField]
    private GameObject projectilePrefab;

    [SerializeField]
    private float fireRate = 1.0f;
    private IEnumerator firingCoroutine;

    private float timeSinceLastFire = 0.0f;
    private bool isAttemptingFire = false;
    private bool canFire = false;

    // Start is called before the first frame update
    void Start()
    {
        firingCoroutine = FireProjectiles();
    }

    // Update is called once per frame
    void Update()
    {
        timeSinceLastFire += Time.deltaTime;

        if(timeSinceLastFire >= 1 / fireRate)
        {
            canFire = true;
        }
        else
        {
            canFire = false;
        }

        if(isAttemptingFire && canFire)
        {
            Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            AudioManager.Instance.PlaySFX("Shoot");

            timeSinceLastFire = 0.0f;
        }
    }

    private void OnFire(InputValue value)
    {
        isAttemptingFire = value.Get<float>() == 1;
    } 

    IEnumerator FireProjectiles()
    {
        WaitForSeconds wait = new WaitForSeconds(1 / fireRate);

        while (true)
        {
            
            yield return wait;
        }
    }
}
