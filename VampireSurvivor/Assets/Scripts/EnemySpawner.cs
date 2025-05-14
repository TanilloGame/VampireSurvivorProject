using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefabA;
    [SerializeField] private GameObject enemyPrefabB;

    private IEnumerator Start()
    {
        UnityWebRequest request = UnityWebRequestAssetBundle.GetAssetBundle("https://npqpmvwvkzwsulwlyjuc.supabase.co/storage/v1/object/public/gameassetbundles//enemigos");
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError("Error downloading AssetBundle: " + request.error);
        }
        else
        {
            AssetBundle bundle = DownloadHandlerAssetBundle.GetContent(request);
            enemyPrefabA = bundle.LoadAsset<GameObject>("EnemyA");
            enemyPrefabB = bundle.LoadAsset<GameObject>("EnemyB");
            bundle.Unload(false);

        }

        request.Dispose();





    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Instantiate(enemyPrefabA, transform.position, transform.rotation);
        }

        
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Instantiate(enemyPrefabB, transform.position, transform.rotation);
        }
    }
}

