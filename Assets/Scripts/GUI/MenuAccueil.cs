using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuAccueil : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject networkManager;
    void Start()
    {
        NetworkInfo.networkManager = networkManager;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Demarrer()
    {
        SceneManager.LoadScene("Jeu");
    }

    public void Quitter()
    {
        Application.Quit();
    }
}
