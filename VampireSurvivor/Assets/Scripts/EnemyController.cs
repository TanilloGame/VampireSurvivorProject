using UnityEngine;

public class EnemyController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Invoke(nameof(Kill), 5f);
    }

    void Kill()
    {
        Destroy(gameObject);
    }
}


