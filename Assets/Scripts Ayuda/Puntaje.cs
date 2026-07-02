
using TMPro;
using UnityEngine;

public class Puntaje : MonoBehaviour
{
    public int puntaje;
    public TextMeshProUGUI UIDePuntaje;//Variable de texto para UI en público para conectarlo con el que está en Canvas>Text(TMP)


    public void Start()
    {
        UIDePuntaje.text = puntaje.ToString(); 
    }
    public void SumarPuntaje(int puntos)
    {
        puntaje = puntaje + puntos;
        UIDePuntaje.text = puntaje.ToString();//Convierte el int de puntaje a string y reemplaza el texto del UI por ese string
    }
}
