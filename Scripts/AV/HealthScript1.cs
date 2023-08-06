using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class HealthScript1 : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> heartsPrefab; //List of hearts to visualize deaths
    private MazeRenderer1 onDeath; // Use this to trigger a death transition
    private int count = 0;
    private int maxCount = 3;

    // Start is called before the first frame update
    void Start()
    {
        onDeath = FindObjectOfType<MazeRenderer1>();
        // Subscribe to win event for smooth health update
        onDeath.triggerDeathEvent.AddListener(() => subscribe2Death());
    }

    void subscribe2Death()
    {
        if (count != maxCount)
        {
            Destroy(heartsPrefab[count]);
            count += 1;
        }
    }
}
