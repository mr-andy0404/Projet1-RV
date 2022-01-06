using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuVictoire : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RetourAccueil()
    {
        SceneManager.LoadScene("Accueil");
    }

    public void Rejouer()
    {
        Scene jeu = SceneManager.GetSceneByName("Jeu");
        SceneManager.LoadScene("Jeu");
        SceneManager.MoveGameObjectToScene(NetworkInfo.player,jeu);
    }
}
