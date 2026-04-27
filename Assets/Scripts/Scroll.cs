using NUnit.Framework.Internal;
using TMPro;
using UnityEngine;

public class Scroll : MonoBehaviour
{
    [SerializeField] private float velocidad;
    [SerializeField] private Vector3 direccion;
    //[SerializeField] private float anchoImagen;
    [SerializeField] private float posicion_X_FinalBoss;

    private Vector3 posicionInicial;
    private Vector3 nuevaPosicion;

    private float secondsCounter = 0f;

    private void Awake()
    {
        this.secondsCounter = 0;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Debug.Log("Start de Scroll. Paso 1. posicionInicial. X = " + posicionInicial.x + ", Y = " + posicionInicial.y);
        posicionInicial = transform.position;
        //Debug.Log("Start de Scroll. Paso 2. posicionInicial. X = " + posicionInicial.x + ", Y = " + posicionInicial.y);

    }

    // Update is called once per frame
    void Update()
    {

        //Debug.Log("Nueva posición. X = " + nuevaPosicion.x + ", posicion_X_FinalBoss = " + posicion_X_FinalBoss);
        if (nuevaPosicion.x >= posicion_X_FinalBoss && ClsGlobales.startScrollStageOne)
        {
            this.secondsCounter += Time.deltaTime;
            movimientoEscenario();
        }
        else if(nuevaPosicion.x < posicion_X_FinalBoss && ClsGlobales.startScrollStageOne)
        {
            //Debug.Log("El escenario se para. nueva posición. X = " + nuevaPosicion.x + ", Y = " + nuevaPosicion.y);

            //Cuando el escenario se para, tengo que activar al enemigo final y desactivar los enemigos aleatorios:
            ClsGlobales.activateFinalBoss = true;
            this.secondsCounter = 0;
        } 
    }

    public void movimientoEscenario()
    {
        //float espacio = velocidad * Time.time;

        float espacio = velocidad * this.secondsCounter;

        nuevaPosicion = posicionInicial + espacio * direccion;

        //Debug.Log("espacio: " + espacio + ", nueva posición. X = " + nuevaPosicion.x + ", Y = " + nuevaPosicion.y);

        //mi posición se va refrescando desde la inicial SUMANDO tanto como resto me quede en la dirección deseada
        transform.position = nuevaPosicion;

    }

    //public void movimientoScrollParallax()
    //{
    //    float espacio = velocidad * Time.time;

    //    //Resto: cuánto me queda de recorrido para alcanzar un nuevo ciclo.
    //    float resto = espacio % anchoImagen;

    //    //mi posición se va refrescando desde la inicial SUMANDO tanto como resto me quede en la dirección deseada
    //    transform.position = posicionInicial + resto * direccion;
    //}
}
