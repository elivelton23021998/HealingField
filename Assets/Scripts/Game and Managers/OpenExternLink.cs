using UnityEngine;

public class OpenExternLink : MonoBehaviour
{
    // MÉTODOS DOS BOTÕES
    public void OpenMsfDonatePage()
    {
        Application.OpenURL("https://www.msf.org.br/doador-sem-fronteiras/");
    }

    public void OpenMsfMainPage()
    {
        Application.OpenURL("https://www.msf.org.br/");
    }

}
