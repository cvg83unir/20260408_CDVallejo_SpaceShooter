using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

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
    [SerializeField] private float segundosTiempoRecuperacion = 3;
    private bool bRecuperando = false;
    private float secondsRecoveryPlayer = 0f;
    [SerializeField] private TextMeshProUGUI textLifes;
    [SerializeField] private TextMeshProUGUI textScore;

    private Rigidbody2D rb2D;
    private Vector2 rawMove = Vector2.zero; 

    private void Awake()
    {
        Application.targetFrameRate = 60;
        this.lifes = ClsGlobales.initialLifesNumber;

        //Cachear el RigidBody
        this.rb2D = GetComponent<Rigidbody2D>();

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
        if(bRecuperando)
        {
            this.secondsRecoveryPlayer += Time.deltaTime;
            if (this.secondsRecoveryPlayer > this.segundosTiempoRecuperacion)
            {
                this.bRecuperando = false;
                this.secondsRecoveryPlayer = 0;
            }
        }
        else
        {
            this.secondsRecoveryPlayer = 0;
        }

        //Delimitamos los valores de la Y dentro de las cuales se puede mover nuestra nave:
        float yDelimitada = Mathf.Clamp(transform.position.y, - this.maximoValorY, this.maximoValorY);

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
        if (!elOtro.CompareTag("PlayerShot"))
        {
            //Debug.Log("Golpe recibido. Variable bRecuperando: " + this.bRecuperando + ". Mis Vidas = " + this.lifes);
            //Cada vez que algo colisione con mi nave, nos quitamos una vida, salvo que este en
            //tiempo de recuperación:
            if (this.bRecuperando == false)
            {
                this.lifes--;
                this.textLifes.text = "VIDAS: " + this.lifes;
            }

            //Si hemos llegado a 0 vidas, gameover:
            if (this.lifes <= 0)
            {
                Destroy(gameObject);

                ClsGlobales.gameCompleted = false;
                System.Threading.Thread.Sleep(1000);
                SceneManager.LoadScene("EndingScene");
            }
            //Si no hemos muestro, activamos el recovery time
            else
            {
                this.bRecuperando = true;
            }
        }

    }

}
