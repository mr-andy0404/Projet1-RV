using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GestionCourse : MonoBehaviour
{
    [Header("Joueur")]
    public GameObject avion;
    private float vitesseMax;

    [Header("HUD")]
    public GameObject hud;
    private Hud affichage;

    [Header("Bullet")]
    public GameObject bullet;

    [Header("Anneaux")]
    [Range(50, 200)]
    [Tooltip("Hauteur minimale du premier anneau par rapport au sol.")]
    public int hauteurMin = 50;
    [Range(100, 1000)]
    [Tooltip("Hauteur maximale du premier anneau par rapport au sol.")]
    public int hauteurMax = 100;
    [Range(2, 100)]
    public int nbAnneaux = 30;
    [Range(10, 1000)]
    [Tooltip("Distance entre les anneaux sur le plan de l'horizontale.")]
    public int distanceEntreAnneaux = 100;
    [Range(0, 180)]
    [Tooltip("Angle maximal selon l'axe des lacets que la trajectoire de l'avion doit effectuer pour arriver au nouvel anneau.")]
    public int angleMax = 70;

    // Position de d?part de l'avion
    public Vector3 startPos;

    // Mod?les d'un anneau et d'un morceau de piste a?rienne ? suivre
    public GameObject anneau;
    private List<GameObject> listeAnneaux;

    [Space(10)]
    [Header("Calcul du score")]
    public float poidsAngleScore = 10;
    private List<int> listeScores;
    private int score = 0;
    private int scoreMax;
    [Range(0, 1)]
    public float poidsScoreTemps;



    [Space(10)]
    [Header("Piste")]
    public GameObject piste;
    public Color couleurPiste;
    private List<GameObject> listePiste;

    [Space(10)]
    [Header("Obstacles")]
    [Range(0, 20)]
    [Tooltip("Nombre maximum d'obstacles par tron?on de piste, le premier tron?on est libre")]
    public int nbObstacles;
    [Range(0, 10)]
    public float distanceALaPiste;
    public List<GameObject> listeObstacles;
    private int nbTypesObstacles;


    private GameObject anneauCourant;

    private float temps = 0f;


    // G?n?ration de la course
    void Start()
    {
        // Si la hauteur maximale est inf?rieure ? la hauteur minimale, on ?galise les deux et la course se fera ? altitude constante
        hauteurMax = Mathf.Max(hauteurMin, hauteurMax);
        startPos = avion.transform.position;
        listeAnneaux = new List<GameObject>();
        listePiste = new List<GameObject>();
        listeScores = new List<int>();
        nbTypesObstacles = listeObstacles.Count;
        genererAnneaux();
        genererPiste();
        genererObstacles();

        affichage = hud.GetComponent<Hud>();
        affichage.RafraichirHUDComplet(nbAnneaux, 0, 0, 0, 0);
        // La course contient au minimum un anneau par d?faut
        anneauCourant = listeAnneaux[0];
        // On annonce que le premier anneau est la cible de l?avion
        anneauCourant.GetComponent<CollisionAnneau>().target = true;

        vitesseMax = 100;
        scoreMax = listeScores.Sum();

        DemarrerChrono();
    }




    // G?n?re une liste d?anneaux ? partir des param?tres choisis dans l??diteur Unity. On part d?un premier anneau avec une position connue.
    // Pour obtenir l?anneau suivant, on duplique l?anneau pr?c?dent, on le tourne d?un angle al?atoire (autour de y) et on le pousse vers l?avant selon
    // la distance d?finie. On r?gle la hauteur ensuite.
    private void genererAnneaux()
    {
        // Le premier anneau sera toujours ? la m?me position par rapport ? la position de d?part
        Vector3 posPremierAnneau = new Vector3(startPos.x, startPos.y, startPos.z + distanceEntreAnneaux);
        // On place l?anneau ? la hauteur minimale par rapport ? la hauteur du terrain en-dessous
        posPremierAnneau.y = Terrain.activeTerrain.SampleHeight(startPos) + hauteurMin;
        listeAnneaux.Add(Instantiate<GameObject>(anneau, posPremierAnneau, new Quaternion()));
        listeAnneaux[0].GetComponent<CollisionAnneau>().avion = avion;

        listeScores.Add((int)(hauteurMin / poidsAngleScore));

        for (int i = 1; i < nbAnneaux; i++)
        {
            GameObject nouvelAnneau;
            int alpha = Random.Range(-angleMax, angleMax);
            // ?Duplication? de l?anneau pr?c?dent
            nouvelAnneau = Instantiate<GameObject>(anneau, listeAnneaux[i-1].transform.position, listeAnneaux[i-1].transform.rotation);
            // On tourne le nouvel anneau et on le pousse
            nouvelAnneau.transform.Rotate(0, alpha, 0);
            nouvelAnneau.transform.Translate(new Vector3(0, 0, distanceEntreAnneaux));

            // On choisit une hauteur al?atoire pour le nouvel anneau
            float hauteurTerrain = Terrain.activeTerrain.SampleHeight(nouvelAnneau.transform.position);
            float dH = hauteurTerrain + hauteurMin + Random.value * (hauteurMax - hauteurMin) - listeAnneaux[i-1].transform.position.y;
            nouvelAnneau.transform.Translate(new Vector3(0, dH, 0));

            nouvelAnneau.GetComponent<CollisionAnneau>().avion = avion;
            listeAnneaux.Add(nouvelAnneau);

            listeScores.Add((int)Mathf.Pow(Mathf.Abs(alpha) + Mathf.Abs(dH) / poidsAngleScore, 2));
        }
    }




    // G?n?re une piste qui relie les anneaux entre eux pour marquer visullement le parcours.
    // Chaque morceau de piste est une ligne droite entre deux anneaux successifs.
    private void genererPiste()
    {
        GameObject nouvellePiste;
        LineRenderer lineRenderer;

        // Cr?ation des morceaux de piste entre le d?part et les anneaux successifs
        for (int i = 0; i < listeAnneaux.Count; i++)
        {
            nouvellePiste = Instantiate<GameObject>(piste);
            lineRenderer = nouvellePiste.GetComponent<LineRenderer>();
            if (i==0)
            {
                lineRenderer.SetPositions(new Vector3[2] { startPos, listeAnneaux[0].transform.position });
            } else
            {
                lineRenderer.SetPositions(new Vector3[2] { listeAnneaux[i-1].transform.position, listeAnneaux[i].transform.position });
            }
            lineRenderer.startColor = couleurPiste;
            lineRenderer.endColor = couleurPiste;
            listePiste.Add(nouvellePiste);
        }
    }



    // G?n?re un nombre d'obstacles al?atoire sur chaque tron?on de piste, chaque obstacle est lui-m?me pris au hasard dans la liste fournie
    private void genererObstacles()
    {
        Vector3 elementDirection, depart, arrivee, pos;
        int nbObstaclesTroncon;
        GameObject obstacle;
        // On place des obstacles entre chaque anneau mais pas entre le d?part et le premier
        for (int i = 1; i < nbAnneaux; i++)
        {
            // On choisit un nombre d'obstacles pour le tron?on, puis on d?coupe le tron?on en fonction du nombre d'obstacles
            depart = listeAnneaux[i-1].transform.position;
            arrivee = listeAnneaux[i].transform.position;
            nbObstaclesTroncon = Random.Range(0, nbObstacles);
            elementDirection = (arrivee - depart) / (nbObstaclesTroncon + 1);

            // On place chaque obstacle ? l'int?rieur de son morceau de tron?on, et on le d?place dans un disque orthogonal ? la piste al?atoirement
            for (int j = 1; j < nbObstaclesTroncon+1; j++)
            {
                obstacle = Instantiate<GameObject>(listeObstacles[Random.Range(0, nbTypesObstacles)]);
                obstacle.GetComponent<Obstacle>().avion = avion;
                obstacle.GetComponent<Obstacle>().bullet = bullet;
                pos = depart + Random.Range(j - 1f, j) * elementDirection;
                obstacle.transform.position = pos;
                obstacle.transform.Translate(new Vector3(Random.Range(-distanceALaPiste, distanceALaPiste), Random.Range(-distanceALaPiste, distanceALaPiste), 0));
                obstacle.transform.Rotate(new Vector3(0, Random.Range(0, 360), 0));
            }
        }
    }


    private bool chronoDemarre = false;
    public void DemarrerChrono()
    {
        chronoDemarre = true;
    }
    public void ArreterChrono()
    {
        chronoDemarre = false;
        temps = 0f;
    }

    private int indiceAnneauCourant = 0;
    private LineRenderer rendererPiste;
    private Color couleurValidation = new Color(0, 1.0f, 0, 0.75f);
    // On v?rifie si l?anneau ? franchir a ?t? franchi. Si c?est le cas on met la course ? jour (score, indicateurs visuels) et on change l?anneau ? franchir.
    void Update()
    {
        if (chronoDemarre)
        {
            temps += Time.deltaTime;
            affichage.Chronometre(temps);
        }

        if (anneauCourant.GetComponent<CollisionAnneau>().IsHit())
        {
            // Affiche le morceau de piste valid? en vert
            rendererPiste = listePiste[indiceAnneauCourant].GetComponent<LineRenderer>();
            rendererPiste.startColor = couleurValidation;
            rendererPiste.endColor = couleurValidation;

            // Affiche l'anneau valid? en vert
            anneauCourant.GetComponentInChildren<Renderer>().material.color = couleurValidation;

            // On augmente le score
            score += listeScores[indiceAnneauCourant];


            // L?anneau courant n?est plus la cible
            anneauCourant.GetComponent<CollisionAnneau>().target = false;

            // Mise ? jour de l?affichage
            affichage.PassageAnneau(indiceAnneauCourant + 1, score);

            // On choisit le nouvel anneau courant et on le marque comme cible
            indiceAnneauCourant++;
            // Si c??tait le dernier anneau on arr?te la course et on d?clenche l?animation de victoire
            if (indiceAnneauCourant == nbAnneaux)
            {
                DeclencherVictoire();
            } else
            {
                anneauCourant = listeAnneaux[indiceAnneauCourant];
                anneauCourant.GetComponent<CollisionAnneau>().target = true;
            }
        }
    }


    private bool gagne = false;
    private bool perdu = false;
    public void DeclencherVictoire()
    {
        if (!perdu)
        {
            chronoDemarre = false;
            gagne = true;
            avion.GetComponent<AvionGyroscope2>().bloque = true;
            float tempsMinimal = nbAnneaux * distanceEntreAnneaux / vitesseMax;
            int scoreTemps = (int)(poidsScoreTemps * score * tempsMinimal / temps);
            affichage.AfficherVictoire(score, temps, scoreTemps);
        }
    }

    public void DeclencherDefaite() {
        if (!gagne)
        {
            chronoDemarre = false;
            perdu = true;
            //avion.GetComponent<AvionGyroscope2>().bloque = true;
            affichage.AfficherDefaite(temps);
        }
    }
}
