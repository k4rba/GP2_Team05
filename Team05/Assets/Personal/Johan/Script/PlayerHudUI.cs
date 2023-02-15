using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class PlayerHudUI : MonoBehaviour
{
    public int player1 = 0;
    public int player2 = 0;


    [SerializeField] private Image[] player1AbilityFillImage;
    [SerializeField] private Image[] player2AbilityFillImage;

    public float[] player1abilityCooldown;
    public float[] player2abilityCooldown;

    private float[] player1abilityCooldownMax;
    private float[] player2abilityCooldownMax;

    [SerializeField] private Image[] rangerAbility;
    [SerializeField] private Image[] tankAbility;

    // Start is called before the first frame update
    public void SetUi()
    {

        // 1== ranger  2== tank
        if (player1==1)
        {
            for (int i = 0; i < 2; i++)
            {
                player1AbilityFillImage[i] = rangerAbility[i];
                player2AbilityFillImage[i] = tankAbility[i];

            }
            player1abilityCooldownMax[1] = 5;
            player1abilityCooldownMax[2] = 5;
            player2abilityCooldownMax[1] = 5;
            player2abilityCooldownMax[2] = 5;
         
        }
        else if (player1 == 2)
        {
            for (int i = 0; i < 2; i++)
            {
                player1AbilityFillImage[i] = tankAbility[i];
                player2AbilityFillImage[i] = rangerAbility[i];

            }
            player1abilityCooldownMax[1] = 5;
            player1abilityCooldownMax[2] = 5;
            player2abilityCooldownMax[1] = 5;
            player2abilityCooldownMax[2] = 5;
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < 2; i++)
        {
            player1AbilityFillImage[i].fillAmount = player1abilityCooldown[i] / player1abilityCooldownMax[i];
            player2AbilityFillImage[i].fillAmount = player2abilityCooldown[i] / player2abilityCooldownMax[i];
        }

        //test
         if (Input.GetKeyDown(KeyCode.Space))
         {
            SetUi();
            Debug.Log("saf");
         }

    }
}
