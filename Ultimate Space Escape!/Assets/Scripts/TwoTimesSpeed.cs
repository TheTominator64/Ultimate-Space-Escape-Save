using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TwoTimesSpeed : MonoBehaviour
{
    public GameObject image1x;
    public GameObject image2x;
    public Text textSpeed;
    
    void Start()
    {
        image2x.SetActive(false);
        image1x.SetActive(true);
        textSpeed.enabled = false;
    }
    
    void Update()
    {
        //When 4 key is pressed, makes game play at "4x" speed (actually 3x speed, but 4x sounds better. 
        //Also makes more sense to the player because its twice as fast as the supposed "2x" speed. 
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            if (Time.timeScale == 0f)
            {
                return;
            }
            else
            {
                image2x.SetActive(true);
                textSpeed.enabled = true;
                textSpeed.text = "4x";
                image1x.SetActive(false);
                Time.timeScale = 3f;
            }
        }

        //When 2 key is pressed, makes game play at "2x" speed (actually 1.5x speed, but 2x sounds better)
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (Time.timeScale == 0f)
            {
                return;
            }
            else
            {
                image2x.SetActive(true);
                textSpeed.enabled = true;
                textSpeed.text = "2x";
                image1x.SetActive(false);
                Time.timeScale = 1.5f;
            } 
        }

        //When 1 key is pressed, makes game play at 1x speed
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (Time.timeScale == 0f)
            {
                return;
            }      
            else
            {
                image2x.SetActive(false);
                image1x.SetActive(true);
                textSpeed.enabled = false;
                Time.timeScale = 1f;
            }
        }

        //If you click the mouse scroll wheel down, it toggles the game speed.
        if (Input.GetMouseButtonDown(2))
        {
            if (Time.timeScale == 0f)
            {
                return;
            }
            else
            {

                if (Time.timeScale == 1.5f)
                {
                    image2x.SetActive(true);
                    image1x.SetActive(false);
                    textSpeed.enabled = true;
                    textSpeed.text = "4x";
                    Time.timeScale = 3f;
                }
                else if (Time.timeScale == 3f)
                {
                    image2x.SetActive(false);
                    image1x.SetActive(true);
                    textSpeed.enabled = false;
                    Time.timeScale = 1f;
                }
                else
                {
                    image2x.SetActive(true);
                    image1x.SetActive(false);
                    textSpeed.enabled = true;
                    textSpeed.text = "2x";
                    Time.timeScale = 1.5f;
                }
            }
        }
    }
}
