using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class codigp : MonoBehaviour
{
    // Start is called before the first frame update
     public Vector3 initialScale = new Vector3(0.1f, 0.1f, 0.1f); // Escala inicial pequena
    public Vector3 finalScale = new Vector3(1f, 1f, 1f); // Escala final
    public float duration = 2f; // Duração da animação em segundos

    private float elapsedTime = 0f; // Tempo decorrido desde o início da animação

    void Start()
    {
        // Configurar a escala inicial
        transform.localScale = initialScale;
    }

    void Update()
    {
        // Verificar se a animação está completa
        if (elapsedTime < duration)
        {
            // Incrementar o tempo decorrido
            elapsedTime += Time.deltaTime;

            // Calcular a fração completada da animação
            float t = elapsedTime / duration;

            // Interpolação linear da escala
            transform.localScale = Vector3.Lerp(initialScale, finalScale, t);
        }
        else
        {
            // Garantir que a escala final seja exatamente a escala desejada
            transform.localScale = finalScale;
            SceneManager.LoadScene("MainMenu");
        }
        }
}
