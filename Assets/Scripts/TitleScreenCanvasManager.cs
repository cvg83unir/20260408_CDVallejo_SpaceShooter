using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreenCanvasManager : MonoBehaviour
{
    [SerializeField] private int lifesNumberEasyMode = 10;
    [SerializeField] private int lifesNumberNormalMode = 3;
    [SerializeField] private int lifesNumberHardMode = 1;

    /// <summary>
    /// Tenemos que hacer este método como público para que se pueda usar desde el canvas.
    /// </summary>
    public void ClickBotonModoFacil()
    {
        loadGamePlayScene(this.lifesNumberEasyMode);
    }

    /// <summary>
    /// Tenemos que hacer este método como público para que se pueda usar desde el canvas.
    /// </summary>
    public void ClickBotonModoNormal()
    {
        loadGamePlayScene(this.lifesNumberNormalMode);
    }

    /// <summary>
    /// Tenemos que hacer este método como público para que se pueda usar desde el canvas.
    /// </summary>
    public void ClickBotonModoDificil()
    {
        loadGamePlayScene(this.lifesNumberHardMode);
    }

    private void loadGamePlayScene(int lifesNumber)
    {
        ClsGlobales.initialLifesNumber = lifesNumber;
        //Debug.Log("Pulsado el botón con número de vidas: " + lifesNumber);
        ClsGlobales.startScrollStageOne = true;
        SceneManager.LoadScene("GameplayScene");
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
