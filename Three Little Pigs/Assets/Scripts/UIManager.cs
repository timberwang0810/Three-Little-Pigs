using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager S;
    public GameObject moneyFlashText;
    public GameObject pausePanel;

    [Header("Sell Panel")]
    public GameObject sellPanel;
    public TextMeshProUGUI sellText;
    private Turret selectedTurret;

    [Header("Health Bar")]
    public Image healthBar;
    public Color minHealthColor;
    public Color maxHealthColor;

    public TextMeshProUGUI moneyText;

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
        //DontDestroyOnLoad(this); commented out to case for levels only allowing certain turrets
        pausePanel.SetActive(false);
        healthBar.fillAmount = 1;
        healthBar.color = maxHealthColor;
    }

    public void ShowPausePanel()
    {
        pausePanel.SetActive(true);
        sellPanel.SetActive(false);
    }

    public void HidePausePanel()
    {
        pausePanel.SetActive(false);
    }

    public void HideAll()
    {
        HidePausePanel();
        HideSellPanel();
    }

    public void AdjustHealthBar(float amount)
    {
        healthBar.fillAmount = amount;
        healthBar.color = Color.Lerp(minHealthColor, maxHealthColor, healthBar.fillAmount);
    }
    
    public void UpdateMoney(int money)
    {
        moneyText.text = money.ToString();
    }

    public void ShowMoneyFlashText(int amount, Vector3 location)
    {
        Vector3 offsetLocation = new Vector3(location.x, location.y + 0.1f, location.z);
        GameObject moneyFlashTextObject = Instantiate(moneyFlashText, offsetLocation, Quaternion.identity);
        moneyFlashTextObject.GetComponent<TextMeshPro>().text = "+ " + amount;
        StartCoroutine(FlashMoneyText(moneyFlashTextObject));
    }

    public void ShowSellPanel(Turret currTurret)
    {
        sellPanel.SetActive(true);
        sellText.text = "Sell this turret for $" + currTurret.cost + "?";
        selectedTurret = currTurret;
    }

    public void HideSellPanel()
    {
        sellPanel.SetActive(false);
    }

    public void btn_YesSell()
    {
        GameManager.S.AddMoney(selectedTurret.cost);
        SoundManager.S.OnUIConfirm();
        HideSellPanel();
        ShowMoneyFlashText(selectedTurret.cost, selectedTurret.gameObject.transform.position);
        Destroy(selectedTurret.gameObject);
    }

    public void btn_NoSell()
    {
        SoundManager.S.OnUIExit();
        HideSellPanel();
    }

    private IEnumerator FlashMoneyText(GameObject moneyFlashTextObject)
    {
        Color opaque = new Color(255.0f, 255.0f, 0.0f, 1.0f);
        Color transparent = new Color(255.0f, 255.0f, 0.0f, 0.0f);
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
