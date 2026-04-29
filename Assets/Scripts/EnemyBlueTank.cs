using System.Collections;
using UnityEngine;

public class EnemyBlueTank : MonoBehaviour
{
    [SerializeField] int points = 200; //Puntos que gana el jugador al eliminar a este enemigo

    //Variables para determinar el disparo del enemigo
    [SerializeField] GameObject prefabRegularEnemyShot;
    [SerializeField] Transform shootingPoint;
    [SerializeField] float timeBetweenShots = 1f;

    private int oldSection=0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        this.oldSection = ClsGlobales.sectionStage1;
    }

    // Update is called once per frame
    void Update()
    {
        //El movimiento y el disparo del jefe final se activan cuando el jugador ha llegado a la zona del jefe final:
        if (this.oldSection != 2 && ClsGlobales.sectionStage1 == 2)
        {
            StartCoroutine(Shoot());

            this.oldSection = ClsGlobales.sectionStage1;
        }
        else
        {
            this.oldSection = ClsGlobales.sectionStage1;
        }

    }

    IEnumerator Shoot()
    {

        while (ClsGlobales.sectionStage1 == 2)
        {
            //Para que no todos los tanques disparn exactamente a la vez, añadimos número
            //aleatorio de menos de 1 segundo
            yield return new WaitForSeconds(timeBetweenShots + Random.Range(0f, 1f));
            Instantiate(this.prefabRegularEnemyShot, shootingPoint.position, Quaternion.identity);
        }
    }

    private void OnTriggerEnter2D(Collider2D elOtro)
    {
        //El enemigo se tiene que destruir cuando le impacte la bala o el propio player
        if (elOtro.CompareTag("PlayerShot") || elOtro.CompareTag("Player"))
        {

            Destroy(gameObject);
            ClsGlobales.scorePlayer += this.points;

        }
    }
}
