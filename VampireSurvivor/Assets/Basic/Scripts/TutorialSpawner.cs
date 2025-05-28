using UnityEngine;

public class TutorialSpawner : MonoBehaviour
{
    public GameObject prefab;
    public Vector3 position;
    public GameObject wanderArea;
    public GameObject player;
    void Start()
    {
        var instance = Instantiate(prefab, position, Quaternion.identity);
        var be = instance.GetComponent<BehaviorExecutor>();
        if (be != null)
        {
            be.SetBehaviorParam("wanderArea", wanderArea);
            be.SetBehaviorParam("player", player);
        }
    }
}