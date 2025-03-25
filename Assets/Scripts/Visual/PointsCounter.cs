using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PointsCounter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI PlayerOnePointsText;
    [SerializeField] private TextMeshProUGUI PlayerTwoPointsText;
    private float points = 0;

    private void Start()
    {
        PlayerOnePointsText.text = "0";
        PlayerTwoPointsText.text = "0";
    }

    public void IncreasePoints(PlayerType playerType)
    {
        TextMeshProUGUI textMeshPro;
        switch (playerType)
        {
            case PlayerType.PlayerOne:
                textMeshPro = PlayerOnePointsText;
                break;
            case PlayerType.PlayerTwo:
                textMeshPro = PlayerTwoPointsText;
                break;
            default:
                return;
        }        

        points++;
        textMeshPro.text = $"{points}";
    }
}
