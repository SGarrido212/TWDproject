using TMPro;//Libreria de UI
using UnityEngine;
using UnityEngine.SceneManagement;

public class Salud : MonoBehaviour
{    
    int vida;
    public int vidaMaxima;
    public TextMeshProUGUI UIDeVida;

    public string nivelACargar;

    private void Start()
    {
        vida = vidaMaxima;
        UIDeVida.text = vida.ToString();
    }

    public void Curar(int puntosDeCura)
    {
        vida = vida + puntosDeCura;
        if (vida > vidaMaxima)
        {
            vida = vidaMaxima;
        }
        UIDeVida.text = vida.ToString();
    }

    public void Danar(int puntosDeDaño)
    {
        vida = vida - puntosDeDaño;
        UIDeVida.text = vida.ToString();
        if (vida <= 0)
        {
            Muerte();
        }
    }

    public void Muerte()
    {
        SceneManager.LoadScene(nivelACargar);
    }
}
