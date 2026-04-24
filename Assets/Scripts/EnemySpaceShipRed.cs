using UnityEngine;

public class EnemySpaceShipRed : MonoBehaviour
{
    [SerializeField] float speed = 3f;
    [SerializeField] float enemyDuration = 5f;
    [SerializeField] int points = 100; //Puntos que gana el jugador al eliminar a este enemigo

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Queremos que se mueva directamente hacia la izquierda
        transform.Translate(Vector2.left * speed * Time.deltaTime);

        //Al igual que las balas, necesitamos que este gameobject muera al cabo de unos segundos para ir liberando la escena de Objetos:
        Destroy(gameObject, enemyDuration);
    }

    //private void OnCollisionEnter2D(Collision2D elOtro)
    //{
    //    //El enemigo se tiene que destruir cuando le impacte la bala
    //    if (elOtro.collider.CompareTag("PlayerShot"))
    //    {
    //        Destroy(gameObject);
    //        ClsGlobales.scorePlayer += this.points;
    //    }
    //}

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
