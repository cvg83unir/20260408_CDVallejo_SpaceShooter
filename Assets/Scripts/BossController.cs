using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;

public class BossController : MonoBehaviour
{
    //Variables para determinar el disparo del enemigo
    [SerializeField] GameObject prefabBossShot;
    [SerializeField] Transform shootingPoint;
    [SerializeField] float timeBetweenShots = 1f;
    bool shotCoroutineEnabled = false;

    //Variables para determinar el movimiento del enemigo:
    [SerializeField] float linearVelocity = 3f;
    [SerializeField] float minimumTimeBetweenDirectionChange = 1f;
    [SerializeField] float timeBetweenDirectionChange = 3f;
    [SerializeField] int keepDirectionProbability = 90;
    private float randomSecondsCounter = 0f;
    private float directionSecondsCounter = 0f;
    [SerializeField] float maximiumValueY = 0.75f;
    private bool isGoingDown = true;

    //Variables para los puntos y las vidas del enemigo final:
    [SerializeField] int points = 5000; //Puntos que gana el jugador al eliminar a este enemigo
    [SerializeField] private int lifes = 20;
    [SerializeField] private TextMeshProUGUI bossText;

    private Rigidbody2D rb2D;

    private void Awake()
    {
        //Cachear el RigidBody
        this.rb2D = GetComponent<Rigidbody2D>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //StartCoroutine(Shoot());

        //StartCoroutine(Move());
    }

    // Update is called once per frame
    void Update()
    {
        //El movimiento y el disparo del jefe final se activan cuando el jugador ha llegado a la zona del jefe final:
        if (ClsGlobales.activateFinalBoss == true)
        {
            MostrarTextoBoss();
            Movimiento();

            if (shotCoroutineEnabled == false)
            {
                StartCoroutine(Shoot());
            }
        }
    }

    private void Movimiento()
    {
        Vector2 direction = this.isGoingDown ? Vector2.down : Vector2.up;
        transform.Translate(direction * this.linearVelocity * Time.deltaTime);

        //Delimitamos los valores de la Y dentro de las cuales se puede mover nuestra nave:
        float yDelimitada = Mathf.Clamp(transform.position.y, -this.maximiumValueY, this.maximiumValueY);
        transform.position = new Vector3(transform.position.x, yDelimitada, transform.position.z);

        this.directionSecondsCounter += Time.deltaTime;
        this.randomSecondsCounter += Time.deltaTime;

        int randomValue = 0;
        if (this.randomSecondsCounter > minimumTimeBetweenDirectionChange)
        {
            randomValue = Random.Range(0, 100);
            this.randomSecondsCounter = 0;

        }

        //Debug.Log("ContadorSegundos: " + this.directionSecondsCounter + ". randomSecondsCounter: " + this.randomSecondsCounter + ". Valor aleatorio: " + randomValue);

        //Cambio de dirección si llevo más de X segundos en la misma dirección
        if (this.directionSecondsCounter >= this.timeBetweenDirectionChange)
        {
            changeDirection();
        }
        //También voy a cambiar de dirección si llevo más de 1 segundo en la misma dirección y sale un número aleatorio entre X y 100
        //(por defecto entre 91 y 100, por ello hay un 10% de posibilidades de un cambio repentino de dirección)
        else if (this.directionSecondsCounter > minimumTimeBetweenDirectionChange && randomValue > keepDirectionProbability)
        {
            changeDirection();
        }

    }

    IEnumerator Shoot()
    {
        this.shotCoroutineEnabled = true;

        while (true)
        {
            yield return new WaitForSeconds(timeBetweenShots);
            Instantiate(prefabBossShot, shootingPoint.position, Quaternion.identity);
        }
    }



    private void changeDirection()
    {
        //Reseteamos el contador de segundos:
        this.directionSecondsCounter = 0;

        //Cambiamos la dirección para la siguiente ejecución el update
        this.isGoingDown = !this.isGoingDown;
        //Debug.Log("Cambio de dirección. ¿Hacia abajo? " + this.isGoingDown);
    }

    private void MostrarTextoBoss()
    {
        this.bossText.gameObject.SetActive(true);
        
        this.bossText.text = "JEFE: " + this.lifes;
    }

    //private void OnCollisionEnter2D(Collision2D elOtro)
    //{
    //    //El enemigo final tiene que perder vida cuando le impacte una bala del jugador:
    //    if (elOtro.collider.CompareTag("PlayerShot"))
    //    {
    //        this.lifes--;
    //        this.bossText.text = "JEFE: " + this.lifes;

    //        //Si llega a 0 vidas, destruimos al jefe:
    //        if(this.lifes <= 0)
    //        {
    //            Destroy(gameObject);
    //            ClsGlobales.scorePlayer += this.points;
    //        }


    //    }
    //}

    private void OnTriggerEnter2D(Collider2D elOtro)
    {
        //El enemigo final tiene que perder vida cuando le impacte una bala del jugador:
        if (elOtro.CompareTag("PlayerShot") && ClsGlobales.activateFinalBoss ==true)
        {
            this.lifes--;
            this.bossText.text = "JEFE: " + this.lifes;

            //Si llega a 0 vidas, destruimos al jefe:
            if (this.lifes <= 0)
            {
                Destroy(gameObject);
                ClsGlobales.scorePlayer += this.points;

                ClsGlobales.startScrollStageOne = false;
                SceneManager.LoadScene("EndingScene");
            }


        }

    }

}
