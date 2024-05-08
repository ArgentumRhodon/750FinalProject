using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartsManager : MonoBehaviour
{
    public Health playerHealth;
    [SerializeField]
    private GameObject heartPrefab;

    List<GameObject> hearts;

    // Start is called before the first frame update
    void Start()
    {
        hearts = new List<GameObject>();

        for(int i = 0; i < playerHealth.maxHealth; i++)
        {
            Vector3 heartPosition = new Vector3(-GameManager.Instance.cameraWorldBounds.x + 0.5f * (i + 1), GameManager.Instance.cameraWorldBounds.y - 0.5f, -1);
            hearts.Add(Instantiate(heartPrefab, heartPosition, Quaternion.identity));
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (playerHealth.health < hearts.Count && playerHealth.health >= 0)
        {
            for(int i = hearts.Count - 1; i > playerHealth.health - 1; i--)
            {
                GameObject heart = hearts[i];
                hearts.RemoveAt(i);
                Destroy(heart);
            }
        }

        if(playerHealth.health <= 0)
        {
            GameManager.Instance.hasLost = true;
        }
    }
}
