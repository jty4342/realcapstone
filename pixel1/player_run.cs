using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_run : MonoBehaviour
{
    public float rotationSpeed = 100.0f;
    public float m_fspeed;
    public float jumpForce = 400f;
    public int maxJumps = 2;
    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }
    void StopAndReset()
    {
       
    }
    // Update is called once per frame
    void Update()
    {


        float fMOveDist =  m_fspeed * Time.deltaTime;



       /* if (Input.GetKey(KeyCode.UpArrow))
        {
            transform.position += Vector3.up * fMOveDist;
            Debug.Log("up");
        }*/

        
      
        
       
        if (Input.GetKey(KeyCode.UpArrow))
        {
            transform.position += Vector3.up * fMOveDist;
            maxJumps++; 
            Debug.Log("up2");
            
        }
        else if (maxJumps == 2)
        {
             if (Input.GetKey(KeyCode.UpArrow))
              {
                  transform.position += Vector3.up * fMOveDist;
                   Debug.Log("up");
              }
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.position += Vector3.right * fMOveDist;
            Debug.Log("right");
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            transform.position += Vector3.down * fMOveDist;
            Debug.Log("down");
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.position += Vector3.left * fMOveDist;
            Debug.Log("left");
        }
        else if (Input.GetKey(KeyCode.R)) 
        {
           transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
        }
        


    }

   


}
