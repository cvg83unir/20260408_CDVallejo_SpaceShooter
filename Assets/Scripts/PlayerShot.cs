using UnityEngine;

public class PlayerShot : MonoBehaviour
{
    [SerializeField]float shotSpeed = 3f;
    private Rigidbody2D rd2D;

    private void Awake()
    {
        //Cachear el rigidBody
        this.rd2D = GetComponent<Rigidbody2D>();
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Establecemos la velocidad a la que saldrá el disparo a la derecha:

        //¿no habría que poner el * Time.deltaTime?
        this.rd2D.linearVelocity = Vector2.right * this.shotSpeed;
        //this.rd2D.linearVelocity = Vector2.right * this.shotSpeed * Time.deltaTime;

        //Necesitamos que el GameObject (el disparo) muera al cabo de unos segundos para ir liberando la escena de Objetos:
        Destroy(gameObject, 5f);
        
    }

    //private void OnCollisionEnter2D(Collision2D elOtro)
    //{
    //    //La bala se tiene que destruir siempre que se choque con algo
    //    Destroy(gameObject);
    //}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //La bala se tiene que destruir siempre que se choque con algo
        Destroy(gameObject);
    }

}
