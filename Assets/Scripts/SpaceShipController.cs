using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using System.Collections;

public class SpaceShipController : MonoBehaviour
{
    [SerializeField] float linearVelocity = 3f;
    [SerializeField] InputActionReference move;
    [SerializeField] InputActionReference shoot;
    [SerializeField] GameObject prefabShot;
    [SerializeField] Transform shootingPoint;
    [SerializeField] float maximoValorY = 0.93f;
    [SerializeField] float maximoValorX = 1.64f;

    //Variables para determinar cada cuando puedo disparar
    [SerializeField] float ratioShot;
    private float secondsShotCounter = -1f;

    //Variables para el contador de vidas del usuario:
    [SerializeField] private int lifes = 3;
    [SerializeField] private float recoverySecondsLimit = 3f;
    [SerializeField] private float blinkingSecondsInterval = 0.2f;
    private bool recovering = false;
    private float actualRecoveringSeconds = 0f;
    [SerializeField] private TextMeshProUGUI textLifes;
    [SerializeField] private TextMeshProUGUI textScore;

    private Rigidbody2D rb2D;
    private SpriteRenderer spriteRenderer;
    private Sprite defaultSprite;
    private PolygonCollider2D pcollider;
    private Vector2 rawMove = Vector2.zero;

    private void Awake()
    {
        Application.targetFrameRate = 60;
        this.lifes = ClsGlobales.initialLifesNumber;

        //Cachear el RigidBody y el spriteRenderer
        this.rb2D = GetComponent<Rigidbody2D>();
        this.spriteRenderer = GetComponent<SpriteRenderer>();
        this.defaultSprite = this.spriteRenderer.sprite;
        this.pcollider = GetComponent<PolygonCollider2D>();

        //Así cogemos las acciones del personaje sólo cuando hay alguna interacción por parte del jugador
        move.action.started += OnMove;
        move.action.canceled += OnMove;
        move.action.performed += OnMove;
        shoot.action.started += OnShoot;
    }

    private void OnEnable()
    {
        move.action.Enable();
        shoot.action.Enable();
    }

    private void OnDisable()
    {
        move.action.Disable();
        shoot.action.Disable();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        this.textLifes.text = "VIDAS: " + this.lifes;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("Contador Segundos Disparo: " + this.secondsShotCounter + ". Ratio Shoot: " + this.ratioShot);

        //Temporizador para el disparo:
        if (this.secondsShotCounter != -1)
        {
            this.secondsShotCounter += Time.deltaTime;
        }

        //Temporizador del tiempo del recuperación del jugador:
        if (recovering)
        {
            this.actualRecoveringSeconds += Time.deltaTime;
            if (this.actualRecoveringSeconds > this.recoverySecondsLimit)
            {
                this.recovering = false;
                Debug.Log("Fin RECOVERY");
                this.actualRecoveringSeconds = 0;
            }
        }
        else
        {
            this.actualRecoveringSeconds = 0;
        }

        //Delimitamos los valores de la Y dentro de las cuales se puede mover nuestra nave:
        float yDelimitada = Mathf.Clamp(transform.position.y, -this.maximoValorY, this.maximoValorY);

        //Delimitamos los valores de la Y dentro de las cuales se puede mover nuestra nave:
        float xDelimitada = Mathf.Clamp(transform.position.x, -this.maximoValorX, this.maximoValorX);

        //Movimiento de nuestro personaje
        this.rb2D.linearVelocity = this.rawMove * this.linearVelocity;
        //this.rb2D.linearVelocity = this.rawMove * this.linearVelocity * Time.deltaTime;

        //Delimito Movimiento
        transform.position = new Vector3(xDelimitada, yDelimitada, transform.position.z);

        //Actualizamos los puntos;
        this.textScore.text = "PUNTOS: " + ClsGlobales.scorePlayer;
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        this.rawMove = context.action.ReadValue<Vector2>();
    }

    private void OnShoot(InputAction.CallbackContext context)
    {
        //Primerísimo disparo, no hay tiempo de espera
        if (this.secondsShotCounter == -1)
        {
            //Debug.Log("Primer Disparo");
            GenerateShoot();
        }
        else
        {
            //Si no es el primerísimo disparo sí que tenemos que superar el tiempo mínimo entre disparo, entonces, instanciamos un nuevo disparo
            if (this.secondsShotCounter > this.ratioShot)
            {
                GenerateShoot();
            }
        }


    }

    private void GenerateShoot()
    {
        GameObject newShoot = Instantiate(prefabShot, shootingPoint.position, Quaternion.identity);
        this.secondsShotCounter = 0;

        //Formas de destruir el disparo al cabo de 5 segundos. Pero es mejor hacerlo en la clase del tiro:
        //Destroy(newShoot, 5f);
        //Invoke(Destroy, 5f);
    }

    //private void OnCollisionEnter2D(Collision2D elOtro)
    //{

    //}

    private void OnTriggerEnter2D(Collider2D elOtro)
    {
        //No tenemos que perder vida si chocamos con nuestro propio disparo o con las banderas de sección.
        if (!elOtro.CompareTag("PlayerShot") && !elOtro.tag.StartsWith("FlagSection"))
        {
            //Cada vez que algo colisione con mi nave, nos quitamos una vida, salvo que este en
            //tiempo de recuperación:
            if (this.recovering == false)
            {
                this.lifes--;
                this.textLifes.text = "VIDAS: " + this.lifes;
            }

            //Si hemos llegado a 0 vidas, gameover:
            if (this.lifes <= 0)
            {
                Destroy(gameObject);

                ClsGlobales.gameCompleted = false;
                System.Threading.Thread.Sleep(500);
                SceneManager.LoadScene("EndingScene");
            }
            //Si no hemos muerto, activamos el tiempo de recuperación y la corrutina del parpadeo
            else
            {
                this.recovering = true;
                //Comenzamos el parpadeo del personaje:
                StartCoroutine(Blink());

            }
        }

    }

    private IEnumerator Blink()
    {
        //Primero desactivamos el collider del personaje para que mientras estamos parpadeando no nos
        //convirtamos en una arma de destrucción masiva
        this.pcollider.enabled = false;

        while (this.recovering)
        {
            //quitamos el renderer y esperamos un poco de tiempo...
            this.spriteRenderer.sprite = null;
            yield return new WaitForSeconds(this.blinkingSecondsInterval);

            //pasado ese tiempo, volvemos a poner el renderer
            this.spriteRenderer.sprite = this.defaultSprite;
            yield return new WaitForSeconds(this.blinkingSecondsInterval);

        }

        //Al acabar el tiempo de recuperación y quitar el parpadeo, volvemos a poner el collider
        this.pcollider.enabled = true;
    }

    private void OnTriggerExit2D(Collider2D elOtro)
    {
        if (elOtro.CompareTag("FlagSection1"))
        {
            ClsGlobales.sectionStage1 = 1;
            Debug.Log("Sección Escenario: " + ClsGlobales.sectionStage1);
        }
        else if (elOtro.CompareTag("FlagSection2"))
        {
            ClsGlobales.sectionStage1 = 2;
            Debug.Log("Sección Escenario: " + ClsGlobales.sectionStage1);
        }
        else if (elOtro.CompareTag("FlagSectionFinalBoss"))
        {
            ClsGlobales.sectionStage1 = 4;
            Debug.Log("Sección Escenario: " + ClsGlobales.sectionStage1);
        }

    }
}
