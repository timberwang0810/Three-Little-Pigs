using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager S;
    public GameObject moneyFlashText;
    public GameObject pausePanel;

    private void Awake()
    {
        // Singleton Definition
        if (UIManager.S)
        {
            // singleton exists, delete this object
            Destroy(this.gameObject);
        }
        else
        {
            S = this;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this);
        pausePanel.SetActive(false);
    }

    public void ShowPausePanel()
    {
        pausePanel.SetActive(true);
    }

    public void HidePausePanel()
    {
        pausePanel.SetActive(false);
    }

    public void HideAll()
    {
        HidePausePanel();
    }

    public void ShowMoneyFlashText(int amount, Vector3 location)
    {
        Vector3 offsetLocation = new Vector3(location.x, location.y + 0.1f, location.z);
        GameObject moneyFlashTextObject = Instantiate(moneyFlashText, offsetLocation, Quaternion.identity);
        moneyFlashTextObject.GetComponent<TextMeshPro>().text = "+ " + amount;
        StartCoroutine(FlashMoneyText(moneyFlashTextObject));
    }

    private IEnumerator FlashMoneyText(GameObject moneyFlashTextObject)
    {
        Color opaque = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        Color transparent = new Color(1.0f, 1.0f, 1.0f, 0.0f);
        TextMeshPro text = moneyFlashTextObject.GetComponent<TextMeshPro>();
        float timer = 0;
        while (timer < 1)
        {
            timer += Time.deltaTime;
            text.color = Color.Lerp(opaque, transparent, timer / 2);
            Vector3 location = moneyFlashTextObject.transform.position;
            location.y += 0.01f;
            moneyFlashTextObject.transform.position = location;
            yield return null;
        }
        Destroy(moneyFlashTextObject);
    }
}
