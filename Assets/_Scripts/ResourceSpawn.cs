using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceSpawn : MonoBehaviour
{

    private static ObjectPool myPool;

    float woodSpawnTimer = 0;
    float woodSmallSpawnTimer = 0;
    float stoneSpawnTimer = 0;
    float stoneSmallSpawnTimer = 0;
    float crystalSpawnTimer = 0;
    float crystalSmallSpawnTimer = 0;

    float speedBoostSpawnTimer = 0;

    int spawnPointX;
    int spawnPointZ;

    public float woodLargeSpawnTime = 6;
    public float stoneLargeSpawnTime = 8;
    public float crystalLargeSpawnTime = 10;
    public float woodSmallSpawnTime = 3;
    public float stoneSmallSpawnTime = 5;
    public float crystalSmallSpawnTime = 8;
    // Start is called before the first frame update
    void Start()
    {
        myPool = ObjectPool.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        
        woodSpawnTimer += Time.deltaTime;
        stoneSpawnTimer += Time.deltaTime;
        crystalSpawnTimer += Time.deltaTime;
        woodSmallSpawnTimer += Time.deltaTime;
        stoneSmallSpawnTimer += Time.deltaTime;
        crystalSmallSpawnTimer += Time.deltaTime;
        speedBoostSpawnTimer += Time.deltaTime;


        if(woodSpawnTimer > woodLargeSpawnTime)
        {
            spawnPointX = Random.Range(-46, -70);
            spawnPointZ = Random.Range(-9, 15);
            myPool.SpawnObject("Resource(Wood)", new Vector3(spawnPointX, 124.5f, spawnPointZ), transform.rotation, true);
           ////Debug.Log("Spawn Wood");
            woodSpawnTimer = 0;
        }

        if(stoneSpawnTimer > stoneLargeSpawnTime)
        {
            spawnPointX = Random.Range(51, 73);
            spawnPointZ = Random.Range(-8, 11);
            myPool.SpawnObject("Resource(Stone)", new Vector3(spawnPointX, 124, spawnPointZ), transform.rotation, true);
           ////Debug.Log("Spawn Stone");
            stoneSpawnTimer = 0;
        }

        if(crystalSpawnTimer > crystalLargeSpawnTime)
        {
            spawnPointX = Random.Range(-9, 14);
            spawnPointZ = Random.Range(-9, 14);
            myPool.SpawnObject("Resource(Crystal)", new Vector3(spawnPointX, 114.8f, spawnPointZ), transform.rotation, true);
           ////Debug.Log("Spawn Crystal");
            crystalSpawnTimer = 0;
        }
         if(woodSmallSpawnTimer > woodSmallSpawnTime)
        {
            spawnPointX = Random.Range(-46, -70);
            spawnPointZ = Random.Range(-9, 15);
            myPool.SpawnObject("Resource(WoodSmall)", new Vector3(spawnPointX, 124.45f, spawnPointZ), transform.rotation, true);
            //Debug.Log("Spawn Small Wood");
            woodSmallSpawnTimer = 0;
        }
         if(stoneSmallSpawnTimer > stoneSmallSpawnTime)
        {
            spawnPointX = Random.Range(51, 73);
            spawnPointZ = Random.Range(-8, 11);
            myPool.SpawnObject("Resource(StoneSmall)", new Vector3(spawnPointX, 123.5f, spawnPointZ), transform.rotation, true);
            //Debug.Log("Spawn Small Stone");
            stoneSmallSpawnTimer = 0;
        }
         if(crystalSmallSpawnTimer > crystalSmallSpawnTime)
        {
            spawnPointX = Random.Range(-9, 14);
            spawnPointZ = Random.Range(-9, 14);
            myPool.SpawnObject("Resource(CrystalSmall)", new Vector3(spawnPointX, 115, spawnPointZ), transform.rotation, true);
           ////Debug.Log("Spawn Small Crystal");
            crystalSmallSpawnTimer = 0;
        }
    }
}
