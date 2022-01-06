using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Net.NetworkInformation;
using System.Net;
using System.Net.Sockets;

public class Hud : MonoBehaviour
{
    private TextMeshProUGUI nbAnneauxTotal, noAnneauCourant, score, chrono;
    private Text vitesse, poussee;
    private RawImage ecranRouge;
    private GameObject ecranVictoire, ecranDefaite;
    private TextMeshProUGUI adresseIP;

    private float temps;

    private bool animationEcranRouge;
    private float dureeAnimationEcranRouge;


    // Start is called before the first frame update
    void Start()
    {
        nbAnneauxTotal = transform.GetChild(2).GetChild(2).GetComponent<TextMeshProUGUI>();
        noAnneauCourant = transform.GetChild(2).GetChild(1).GetComponent<TextMeshProUGUI>();
        score = transform.GetChild(4).GetComponent<TextMeshProUGUI>();
        chrono = transform.GetChild(3).GetChild(1).GetComponent<TextMeshProUGUI>();
        adresseIP = transform.GetChild(8).GetComponent<TextMeshProUGUI>();

        vitesse = transform.GetChild(0).GetChild(1).GetComponent<Text>();
        poussee = transform.GetChild(1).GetChild(1).GetComponent<Text>();

        ecranRouge = transform.GetChild(5).GetComponent<RawImage>();
        ecranRouge.color = new Color(0, 0, 0, 0);

        ecranVictoire = transform.GetChild(6).gameObject;
        ecranVictoire.SetActive(false);

        ecranDefaite = transform.GetChild(7).gameObject;
        ecranDefaite.SetActive(false);

        temps = 0f;

        var host = Dns.GetHostEntry(Dns.GetHostName());
        foreach (var ip in host.AddressList)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                adresseIP.text = ip.ToString();
            }
        }
    }

    int compteur = 0;
    float dureeAnimationEnCours = 0f;
    // Update is called once per frame
    void Update()
    {
        System.TimeSpan t = System.TimeSpan.FromSeconds(temps);
        chrono.text = t.ToString(@"mm\:ss\:ff");


        if (animationEcranRouge)
        {
            ecranRouge.color = new Color(1, 0, 0, 0.25f * Mathf.Cos(compteur / 10f) + 0.75f);
            compteur++;
            dureeAnimationEnCours += Time.deltaTime;
            if (dureeAnimationEnCours > dureeAnimationEcranRouge)
            {
                animationEcranRouge = false;
                ecranRouge.color = new Color(0, 0, 0, 0);
                compteur = 0;
                dureeAnimationEnCours = 0f;
            }
        }
    }

    // Pour mettre ? jour tous les champs de l?affichage sauf le chronom?tre qui est g?r? automatiquement
    public void RafraichirHUDComplet(int _nbAnneauxTotal, int _noAnneauCourant, int _score, float _vitesse, float _poussee)
    {
        nbAnneauxTotal.text = _nbAnneauxTotal.ToString();
        noAnneauCourant.text = _noAnneauCourant.ToString() + " /";
        score.text = _score.ToString();
        vitesse.text = ((int)_vitesse).ToString();
        poussee.text = ((int)_poussee).ToString() + "N";
    }

    // Appel? pour mettre ? jour l?affichage quand le joueur franchit un anneau
    public void PassageAnneau(int _noAnneauCourant, int _score)
    {
        noAnneauCourant.text = _noAnneauCourant.ToString() + " /";
        score.text = _score.ToString();
    }

    // Appel? pour mettre ? jour l?affichage des param?tres de vol de l?avion
    public void ParametresDeVol(float _vitesse, float _poussee)
    {
        vitesse.text = ((int)_vitesse).ToString();
        poussee.text = ((int)_poussee).ToString() + "N";
    }

    // Appel? pour mettre ? jour l?affichage du chronom?tre
    public void Chronometre(float _temps)
    {
        temps = _temps;
    }



    // Affiche une animation d??cran rouge
    public void AnimerEcranRouge(float duree)
    {
        animationEcranRouge = true;
        dureeAnimationEcranRouge = duree;
    }

    public void AfficherVictoire(int nScore, float nTemps, int nScoreChrono)
    {
        ecranVictoire.SetActive(true);
        Text score, tempsChrono, scoreChrono, scoreTotal;
        score = ecranVictoire.transform.GetChild(6).GetComponent<Text>();
        tempsChrono = ecranVictoire.transform.GetChild(7).GetComponent<Text>();
        scoreChrono = ecranVictoire.transform.GetChild(8).GetComponent<Text>();
        scoreTotal = ecranVictoire.transform.GetChild(9).GetComponent<Text>();

        score.text = nScore.ToString();
        tempsChrono.text = System.TimeSpan.FromSeconds(nTemps).ToString(@"mm\:ss\:ff") + "           +";
        scoreChrono.text = nScoreChrono.ToString();
        scoreTotal.text = (nScore + nScoreChrono).ToString();
    }

    public void AfficherDefaite(float nTemps)
    {
        ecranDefaite.SetActive(true);
        Text tempsChrono = ecranDefaite.transform.GetChild(7).GetComponent<Text>();
        tempsChrono.text = System.TimeSpan.FromSeconds(nTemps).ToString(@"mm\:ss\:ff") + "           +";
    }
}
