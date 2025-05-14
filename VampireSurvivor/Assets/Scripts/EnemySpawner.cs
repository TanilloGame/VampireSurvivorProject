using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class EnemySpawner : MonoBehaviour
{

    [SerializeField] private GameObject enemy1Prefab;
    [SerializeField] private GameObject enemy2Prefab;

    [SerializeField] private List<GameObject> enemy1Pool = new List<GameObject>();
    [SerializeField] private List<GameObject> enemy2Pool = new List<GameObject>();

    IEnumerator Start()
    {
        UnityWebRequest request = UnityWebRequestAssetBundle.GetAssetBundle("https://gkgekuizxqkymecxpwme.supabase.co/storage/v1/object/public/vampire-survivor-bundle//enemigos");
        yield return request.SendWebRequest();
        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Failed to load asset bundle: " + request.error);
            yield break;
        }
        else
        {
            Debug.Log("Asset bundle loaded successfully.");
            AssetBundle assetBundle = DownloadHandlerAssetBundle.GetContent(request);
            enemy1Prefab = assetBundle.LoadAsset<GameObject>("Enemigo 1");
            enemy2Prefab = assetBundle.LoadAsset<GameObject>("Enemigo 2");
            assetBundle.Unload(false);
        }
        request.Dispose();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            GetEnemyFromPool("Enemy1");
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            GetEnemyFromPool("Enemy2");
        }
    }

    private void GetEnemyFromPool(string enemyType)
    {
        switch (enemyType)
        {
            case "Enemy1":
                GetSpecificEnemy(enemy1Pool, enemy1Prefab);
                break;
            case "Enemy2":
                GetSpecificEnemy(enemy2Pool, enemy2Prefab);
                break;
        }
    }

    private void GetSpecificEnemy(List<GameObject> enemyType, GameObject prefabEnemy)
    {
        GameObject enemy = null;
        if (enemyType.Count > 0)
        {
            for (int i = 0; i < enemyType.Count; i++)
            {
                if (enemyType[i].activeInHierarchy == false)
                {
                    enemy = enemyType[i];
                    enemy.SetActive(true);
                    enemy.transform.position = transform.position;
                    enemy.transform.rotation = Quaternion.identity;
                    break;
                }
            }
        }

        if (enemy == null)
        {
            enemy = Instantiate(prefabEnemy, transform.position, Quaternion.identity);
            enemyType.Add(enemy);
        }
    }
}