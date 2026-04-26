using TMPro;
using UnityEngine;

public class EndingScreenCanvasManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textScore;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        this.textScore.text = "Puntuación Final: " + ClsGlobales.scorePlayer;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
