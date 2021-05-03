using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InstructionsText : MonoBehaviour
{
    public TextMeshProUGUI instructionsText;

    List<string> pages = new List<string>();
    private int listLength;
    private int curPage = 0;

    // Start is called before the first frame update
    void Start()
    {
        pages.Add("Build turrets to defend your hut against the wolves!");
        pages.Add("Click to choose turret and place on map on valid locations.");
        pages.Add("Click on turret to sell turret.\n\nPress ESC to pause game.");
        pages.Add("Survive as long as possible in first two levels and defeat all enemies in final level to win the game!");

        listLength = pages.Count;
    }

    public void btn_Prev()
    {
        curPage--;
        if (curPage < 0) curPage = listLength - 1;

        instructionsText.text = pages[curPage];
    }

    public void btn_Next()
    {
        curPage++;
        if (curPage >= listLength) curPage = 0;

        instructionsText.text = pages[curPage];
    }
}
