using System.Collections;
using UnityEngine;

public class EnemyGenerator : MonoBehaviour
{
    [SerializeField] Transform top;
    [SerializeField] Transform bottom;

    //Enemigos hechos durante la clase (los azules)
    [SerializeField] GameObject prefabBlueEnemy;
    [SerializeField] float timeBetweenBlueShips = 3f;

    //Enemigos rojos
    [SerializeField] GameObject prefabRedEnemy;
    [SerializeField] float timeBetweenRedShips = 3f;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(GenerateBlueEnemies());

        StartCoroutine(GenerateRedEnemies());
    }

    IEnumerator GenerateBlueEnemies()
    {
        while(ClsGlobales.activateFinalBoss == false)
        {
            yield return new WaitForSeconds(timeBetweenBlueShips);
            Vector3 position = Vector3.Lerp(top.position, bottom.position, Random.Range(0f, 1f));
            Instantiate(prefabBlueEnemy, position, Quaternion.identity);
        }
    }

    IEnumerator GenerateRedEnemies()
    {
        while (ClsGlobales.activateFinalBoss == false)
        {
            Vector3 position = Vector3.Lerp(top.position, bottom.position, Random.Range(0f, 1f));
            //Generamos 5 Enemigos consecutivos
            for (int i = 0; i<5; i++)
            {
                Instantiate(prefabRedEnemy, position, Quaternion.identity);
                yield return new WaitForSeconds(0.1f);
            }
            //Esperamos hasta la siguiente vez
            yield return new WaitForSeconds(timeBetweenRedShips);
            
           
        }
    }


}
