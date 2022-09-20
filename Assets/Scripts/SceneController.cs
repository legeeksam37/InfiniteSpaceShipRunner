using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneController : MonoBehaviour
{

public static SceneController Instance;
private GameObject SpaceShip;
[SerializeField]
private GameObject[] grounds;
[SerializeField]
private GameObject[] groundsOnScene;
[SerializeField]
private float groundSize;
float pos;
[SerializeField]
private Image redBar;
[SerializeField]
private Image greenBar;
private ParticleSystem Flame1;
private ParticleSystem Flame2;
[SerializeField]
public int health; 
private int score; 
[SerializeField]
private Text healthText;
[SerializeField]
public Text gameOver;
[SerializeField]
public Text warning;



private void Awake(){

    if(Instance != null){

        Destroy(gameObject);

    }
    Instance = this;
    
 
}
   
   
   
   
   
    // Start is called before the first frame update
    void Start()
    {


        gameOver.gameObject.SetActive(false);
        warning.gameObject.SetActive(false);
        SpaceShip = GameObject.Find("SpaceShip");
        pos = SpaceShip.transform.position.z * groundSize/2 - 5f;
        
         health =100;
        groundsOnScene = new GameObject[grounds.Length];

        for(int i = 0; i < grounds.Length; i++){
            
            int n = Random.Range(0,grounds.Length);
            groundsOnScene[i] = Instantiate(grounds[n]);
        }

        groundSize = groundsOnScene[0].GetComponentInChildren<Transform>().Find("Runway").localScale.z;


        foreach (var ground in groundsOnScene)
        {
            ground.transform.position = new Vector3(0,0,pos);
            pos += groundSize;
        }

    }

    // Update is called once per frame
    void Update()
    {
        groundManagement();   
        healthManagement();
        scoreManagement();
        
    }

    private void groundManagement(){
         for (int i = groundsOnScene.Length - 1; i >=0; i--){
                 GameObject groundy = groundsOnScene[i];

                    if(groundy.transform.position.z + groundSize < SpaceShip.transform.position.z -10f)
                    {
                        float z = groundy.transform.position.z;
                        Destroy(groundy);
                        int j = Random.Range(0,grounds.Length);
                        //create new ground 
                        groundy = Instantiate(grounds[j]);
                        groundy.transform.position = new Vector3(0,0,z+groundSize*groundsOnScene.Length);
                        //adding it to the list
                        groundsOnScene[i] = groundy;
                    }
         } 
    }


    private void healthManagement(){
        greenBar.rectTransform.sizeDelta = new Vector2(redBar.rectTransform.sizeDelta.x * (health/100f), redBar.rectTransform.sizeDelta.y);
    }

    private void OnDestroy() {
        Instance = null;
    }

    private void scoreManagement(){
        healthText.text = health.ToString();
    }

}
