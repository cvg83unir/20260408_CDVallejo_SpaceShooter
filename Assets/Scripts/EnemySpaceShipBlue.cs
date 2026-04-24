using UnityEngine;

public class EnemySpaceShipBlue : MonoBehaviour
{
    [SerializeField] float speed = 3f;
    [SerializeField] float leftLimitCoordinate = -3f;
    [SerializeField] float rightScreenLimit = 3f;
    bool isGoingLeft = true;
    [SerializeField] int points = 150; //Puntos que gana el jugador al eliminar a este enemigo


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 direction = isGoingLeft ? Vector2.left : Vector2.right;
        transform.Translate(direction * speed * Time.deltaTime);

        if (isGoingLeft && transform.position.x < leftLimitCoordinate)
        {
            this.isGoingLeft = false;
        }

        if (!isGoingLeft && transform.position.x > rightScreenLimit)
        {
            Destroy(gameObject);
        }

    }

    //private void OnCollisionEnter2D(Collision2D elOtro)
    //{
    //    //El enemigo se tiene que destruir cuando le impacte la bala
    //    if(elOtro.collider.CompareTag("PlayerShot"))
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
