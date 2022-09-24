using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceShipControl : MonoBehaviour
{

  [SerializeField]
  private float translationSpeed;
  private const float MIN_X = -9.7f;
  private const float MAX_X = 7.4f;
  private const float MIN_Y = -2.27541f;
  private const float MAX_Y = 2f;
  [SerializeField]
  private float maxSpeed = 250;
  private float currentSpeed = 0;
  private ParticleSystem Flame1;
  private ParticleSystem Flame2;
  public float fuelConsumption;
  

  private bool bAccelerate;
  


    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(0 ,MIN_Y,4);
        
        Flame1 = GameObject.Find("Reactor1").GetComponent<ParticleSystem>();
        Flame2 = GameObject.Find("Reactor2").GetComponent<ParticleSystem>();
        var main = Flame1.main;
        var main2 = Flame2.main;
        bAccelerate = false;

        
    }

    // Update is called once per frame
    void Update()
    {
      moveHorizontally();
      moveVertically();       
      moveForward();

    }

    void moveHorizontally(){
        float horizontalMove = Input.GetAxis("Horizontal") * 
              translationSpeed * Time.deltaTime;
              

                if(currentSpeed >= 0.1f){
                    transform.position += transform.right * horizontalMove*currentSpeed;

                }
                if(transform.position.x <=  MIN_X){
                    transform.position = new Vector3( MIN_X,transform.position.y, transform.position.z);
                }
                if(transform.position.x >=  MAX_X){
                    transform.position = new Vector3(MAX_X, transform.position.y, transform.position.z);
                }
    }

    void rotate(){

      
      
      if(Input.GetAxis("Horizontal") == -1 ){

        if(transform.localRotation.eulerAngles.z <= 10){
          transform.Rotate(new Vector3(0, 0, -0.1f*Time.deltaTime));
          //Debug.Log("inferior");
        }else {
          transform.Rotate(new Vector3(0, 0, 10));
          //Debug.Log("superior");
        }
        
      }

      if(Input.GetAxis("Horizontal") == 1 ){
 
          Debug.Log(transform.localRotation.eulerAngles.z );

        if(transform.localRotation.eulerAngles.z >= -10){
          //transform.Rotate(new Vector3(0, 0, 0.1f*Time.deltaTime));
          //Debug.Log("superior");
        }else {
          //transform.Rotate(new Vector3(0, 0, -10));
          Debug.Log("inferior");
        }

      }

    
    }

    void moveVertically(){


              float verticalMove = Input.GetAxis("Vertical") * translationSpeed * Time.deltaTime;
              

               if(currentSpeed >= 0.1f ){
                     transform.position += 2*transform.up 
                    * verticalMove; 
                    
               }     
                   
              
                if(transform.position.y >  MAX_Y){
                    transform.position = new Vector3(transform.position.x, MAX_Y, transform.position.z);
                }

                if(transform.position.y <=  MIN_Y){
                    transform.position = new Vector3(transform.position.x, MIN_Y, transform.position.z);
                }

                if(transform.position.y > MIN_Y && currentSpeed <= 0.1f){
                  transform.position -= 2f*transform.up  * Time.deltaTime; 
                }
    }


    void moveForward(){
             
                if(Input.GetButton("Fire1") && SceneController.Instance.health > 0 && SceneController.Instance.fuel >= 1){
                    currentSpeed += 0.1f * Time.deltaTime;
                    bAccelerate =true;
                    changeFlameOnAcceleration(Flame1, bAccelerate);
                     changeFlameOnAcceleration(Flame2, bAccelerate);
                     
                  if(SceneController.Instance.fuel >= 1){
                        fuelConsumming(7);
                  }else {
                    Debug.Log("You ran out of fuel");
                  }
                    

                     
                      if(currentSpeed > maxSpeed){
                        currentSpeed = maxSpeed;
                      }

                } else{
                  bAccelerate = false;
                   changeFlameOnAcceleration(Flame1, bAccelerate);
                   changeFlameOnAcceleration(Flame2, bAccelerate);
                  currentSpeed -= 0.1f * Time.deltaTime;
                      if(currentSpeed <= 0){
                          currentSpeed = 0;
                      }
                     
                }
               

                 transform.position += transform.forward * currentSpeed;
    }


    void changeFlameOnAcceleration(ParticleSystem system, bool bAccelerate){

                    if(bAccelerate){
                        bAccelerate = true;
                        var main = system.main;
                        main.startSpeed = 0.5f;
                    }else {
                      var main = system.main;
                        main.startSpeed = 0.1f;
                    }
      }


    private void OnTriggerEnter(Collider other) {


        
        if(SceneController.Instance.health > 0){
          SceneController.Instance.health -= 10;
          //SceneController.Instance.health = 100; for testing only
        }
        if(SceneController.Instance.health == 50){
          SceneController.Instance.warning.gameObject.SetActive(true);
        }else{
          SceneController.Instance.warning.gameObject.SetActive(false);
        }

        if(SceneController.Instance.health <=0){
           SceneController.Instance.gameOver.gameObject.SetActive(true);
        }
    }


    private void fuelConsumming(float c){
      SceneController.Instance.fuel -= c *Time.deltaTime;
}



}
