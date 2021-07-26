using UnityEngine;

public class Pause : MonoBehaviour
{
    public void Pausar()
    {
        Time.timeScale = 0;
    }

    public void Despausar()
    {
        Time.timeScale = 1;
    }
}
