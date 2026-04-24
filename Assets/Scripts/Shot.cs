using UnityEngine;

public class Shot : MonoBehaviour
{
    [SerializeField] float shotSpeed = 3f;
    [SerializeField] float shotDuration = 5f;//tiempo después del cual destruimos el GameObject (el disparo) muera al cabo de unos segundos para ir liberando la escena de Objetos:
    [SerializeField] Vector2 direccion;
    [SerializeField] bool playerShot = false;//para distinguir si son disparos del jugador o de los enemigos
    
    private Rigidbody2D rb2D;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Cacheamos el rigidbody:
        this.rb2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        this.rb2D.linearVelocity = direccion * shotSpeed;
        //this.rb2D.linearVelocity = direccion * shotSpeed * Time.deltaTime;

        //Necesitamos que el GameObject (el disparo) muera al cabo de unos segundos para ir liberando la escena de Objetos:
        Destroy(gameObject, shotDuration);

    }


    //private void OnCollisionEnter2D(Collision2D elOtro)
    //{
    //    //La bala se tiene que destruir en ciertas condiciones, por ejemplo.
    //    //Una bala del jugador se tiene que destruir en cuanto toque en cualquier cosa, salvo otros disparos:
    //    if (this.playerShot)
    //    {
    //        if (!elOtro.collider.CompareTag("EnemyShot"))
    //        {
    //            Destroy(gameObject);
    //        }
    //    }
    //    else
    //    {
    //        //Sin embargo, una bala enemiga, puede chocar con otras balas e incluso puede traspasar a otros enemigos:
    //        //Entonces, sólo se destruirá si toca al jugador:
    //        if (elOtro.collider.CompareTag("Player"))
    //        {
    //            Destroy(gameObject);
    //        }
    //    }
        
    //}

    private void OnTriggerEnter2D(Collider2D elOtro)
    {
        //La bala se tiene que destruir en ciertas condiciones, por ejemplo.
        //Una bala del jugador se tiene que destruir en cuanto toque en cualquier cosa, salvo otros disparos o mi propia nave:
        if (this.playerShot)
        {
            if (!elOtro.CompareTag("EnemyShot") && !elOtro.CompareTag("Player"))
            {
                Destroy(gameObject);
            }
        }
        else
        {
            //Sin embargo, una bala enemiga, puede chocar con otras balas e incluso puede traspasar a otros enemigos:
            //Entonces, sólo se destruirá si toca al jugador:
            if (elOtro.CompareTag("Player"))
            {
                Destroy(gameObject);
            }
        }
    }
}
