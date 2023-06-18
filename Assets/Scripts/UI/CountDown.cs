using UnityEngine;
using UnityEngine.UI;

public class CountDown : MonoBehaviour
{
    [SerializeField] private float timeInSeconds;
    [SerializeField] private Text timeDisplay;
    [SerializeField] private Text timeDisplayShadow;

    private float seconds;
    private float minutes;

    private void Update()
    {
        if (timeInSeconds > 0)
        {
            timeInSeconds -= Time.deltaTime;

            seconds = timeInSeconds;
            minutes = Mathf.FloorToInt(timeInSeconds / 60);

            timeDisplayShadow.text = timeDisplay.text = minutes.ToString("00") + ":" + Mathf.FloorToInt(seconds % 60f).ToString("00");
        }
        else //EndGame
        {
            UiGameScene.instance.EndDay();
        }
    }
    
}
