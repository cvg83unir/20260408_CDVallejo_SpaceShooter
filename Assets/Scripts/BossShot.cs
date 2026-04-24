using UnityEngine;

public class BossShot : MonoBehaviour
{
    [SerializeField] float bossShotSpeed = 3f;
    private Rigidbody2D rb2D;

    private void Awake()
    {
        //Cacheamos el rigidbody:
        this.rb2D = GetComponent<Rigidbody2D>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        this.rb2D.linearVelocity = Vector2.left * bossShotSpeed;
        //this.rb2D.linearVelocity = Vector2.left * bossShotSpeed * Time.deltaTime;

        //Necesitamos que el GameObject (el disparo) muera al cabo de unos segundos para ir liberando la escena de Objetos:
        Destroy(gameObject, 5f);
    }
}
