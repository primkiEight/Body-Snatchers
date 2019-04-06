using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Extensions
{
    public static float RandomValue(this Vector2 v2)
    {
        return Random.Range(v2.x, v2.y);
    }
}

public class ObjectSpawner : MonoBehaviour {

    /*
    [System.Serializable]
    public struct AudioSettings
    {
        public string Name;
        public GameObject AudioClip; //AudioClip varijabla, sad primjer s gameobjectom
    }

    public List<AudioSettings> AudioList = new List<AudioSettings>();

    //gornja lista je zapravo nešto što postoji u Unityju, a to je Dictionary
    public Dictionary<string, GameObject> AudioSettingsDictionary = new Dictionary<string,GameObject>();
    //samo, Dictionary se ne može prikazati u Inspektoru
    //pa se ponekad u kodovima može naći jedno i drugo...
    //...pa se u Awakeu Dictionary puni elementima iz Liste
    */

    public Vector2 SpawnInterval = Vector2.one;

    public Vector2 XRange = Vector2.one;
    public Vector2 YRange = Vector2.one;
    public Vector2 ZRange = Vector2.one;

    public Color GizmoColor = Color.white;

    public List<GameObject> PossiblePrefabsToSpawn = new List<GameObject>();

    public void Start()
    {
        StartCoroutine(SpawnCoroutine());
    }

    private void SpawnObject(GameObject objectToSpawn)
    {
        Vector3 randomSpawnPosition = new Vector3(XRange.RandomValue(), YRange.RandomValue(), ZRange.RandomValue());
        //bilo bi zgodno isprobati da se ovaj vector nadoda na poziciju playera, pa da se spawna uvijek oko njega, ili oko nekog drugog objekta u ovom rangeu

        GameObject objectClone = Instantiate(objectToSpawn, randomSpawnPosition, Quaternion.identity, transform);
    }

    private IEnumerator SpawnCoroutine()
    {
        while (GameManager.GM.CurrentState == GameManager.GameState.Playing)
        {
            int randomIndex = Random.Range(0, PossiblePrefabsToSpawn.Count); //uključuje 0 i max, nema potrebe smanjivati za 1
            SpawnObject(PossiblePrefabsToSpawn[randomIndex]);

            yield return new WaitForSeconds(SpawnInterval.RandomValue());
        }
    }

    private void OnDrawGizmos() //nacrtati prostor u kojem se spawnaju neprijatelji
    {
        Gizmos.color = Color.yellow;
        float _sizeX = XRange.y - XRange.x;
        float _sizeY = YRange.y - YRange.x;
        float _sizeZ = ZRange.y - ZRange.x;

        float _middleX = XRange.x + (XRange.y - XRange.x) / 2;
        float _middleY = YRange.x + (YRange.y - YRange.x) / 2;
        float _middleZ = ZRange.x + (ZRange.y - ZRange.x) / 2;

        Vector3 _middle = new Vector3(_middleX, _middleY, _middleZ);
        Vector3 _size = new Vector3(_sizeX, _sizeY, _sizeZ);
        Gizmos.DrawWireCube(_middle, _size);

        
    }
}
