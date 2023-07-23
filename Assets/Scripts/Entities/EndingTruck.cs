using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndingTruck : MonoBehaviour
{
    [SerializeField] private GameObject speechBubble;
    [SerializeField] private TextMeshProUGUI speechText;

    private bool isEnding = false;
    // Start is called before the first frame update
    void Start()
    {
        speechBubble.gameObject.SetActive(false);
        speechText.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (GameManager.Instance.GetGold() >= GameManager.Instance.MaxGold)
            {
                speechText.text = "end the game? (Y)";
                isEnding = true;
            }
            else
            {
                speechText.text = "Not Enough!";
                isEnding = false;
            }
            speechText.gameObject.SetActive(true);
            speechBubble.gameObject.SetActive(true);
        }
    }
    public void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!isEnding) return;
            if (Input.GetKeyDown(KeyCode.Y))
            {
                Debug.Log("EndingTruck - Ended");
                GameSceneManager.Instance.EndGame();
                if (GameSceneManager.Instance.Map == MapType.Farm)
                {
                    SceneManager.LoadScene("EndingSceneFarm");
                }
                else
                {
                    SceneManager.LoadScene("EndingSceneFish");
                }
            }
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            speechBubble.gameObject.SetActive(false);
            speechText.gameObject.SetActive(false);
        }
    }
}
