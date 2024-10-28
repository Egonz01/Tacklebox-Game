using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FishSpawner : MonoBehaviour
{
    [Header("Fishes to spawn")]
    [SerializeField] List<Fish> fishToSpawn;

    [Header("Spawn Parameters")]
    [SerializeField] float Range = 1f;
    [SerializeField] float spawnTime = 1f;


    // Start is called before the first frame update
    void Start()
    {
        SpawnFish();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Spawn() {
        Fish newFish = Instantiate(fishToSpawn[Random.Range(0, fishToSpawn.Count)], transform.position + new Vector3(0, Random.Range(-Range, Range), 0), Quaternion.identity);
        newFish.GetComponent<Rigidbody2D>().velocity = -transform.right * newFish.swimSpeed;
        StartCoroutine(DestroyFishRoutine(newFish));
    }

    void SpawnFish() {
        StartCoroutine(SpawnFishRoutine());
        IEnumerator SpawnFishRoutine()
        {
            while(true) {
                yield return new WaitForSeconds(spawnTime);
                Spawn();
            }
        }
    }

    IEnumerator DestroyFishRoutine(Fish fish){
            yield return new WaitForSeconds(fish.despawnTime);
            if(fish.isCaught == false) {
                Destroy(fish.gameObject);
            }
        }
}
