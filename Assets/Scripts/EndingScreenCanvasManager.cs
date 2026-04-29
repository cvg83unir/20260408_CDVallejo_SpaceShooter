using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndingScreenCanvasManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textScore;
    [SerializeField] private TextMeshProUGUI textCongratulations;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StringBuilder sbCongratulationsText = new StringBuilder();

        if (ClsGlobales.gameCompleted)
        {
            sbCongratulationsText.AppendLine("¡ENHORABUENA!");
            sbCongratulationsText.AppendLine("");
            sbCongratulationsText.AppendLine("La galaxia está en paz.");

            this.textCongratulations.color = Color.white;
        }
        else
        {
            this.textCongratulations.color = Color.red;
            sbCongratulationsText.AppendLine("GAME OVER");
        }

        this.textCongratulations.text = sbCongratulationsText.ToString();
        this.textScore.text = "Puntuación Final: " + ClsGlobales.scorePlayer;
    }

    public void RetryButtonClick()
    {
        //Volvemos a la primera escena, reseteando primero las variables globales:
        ClsGlobales.sectionStage1 = 0;
        ClsGlobales.activateFinalBoss = false;
        ClsGlobales.scorePlayer = 0;
        ClsGlobales.initialLifesNumber = 3;
        ClsGlobales.startScrollStageOne = false;
        ClsGlobales.gameCompleted = false;
        SceneManager.LoadScene("TitleScene");
    }

    public void ExitButtonClick()
    {
        //Cerramos el juego:
        Application.Quit();
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
