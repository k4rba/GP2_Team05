using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WaitForSeconds = UnityEngine.WaitForSeconds;


public class PlayerHudUI : MonoBehaviour {
    [SerializeField] private Image[] meleeAbilityFillImage;
    [SerializeField] private Image[] rangedAbilityFillImage;

    [SerializeField] private Image[] rangerAbility;
    [SerializeField] private Image[] tankAbility;


    public List<Player> Players = new List<Player>();


    private float meleeAbility1CDMax, meleeAbility2CDMax, rangedAbility1CDMax, rangedAbility2CDMax;


    public void SetUi() {
        foreach (var player in Players) {
            switch (player.cType) {
                case Player.CharacterType.Melee:
                    meleeAbility1CDMax = player.AbilityBCooldown;
                    meleeAbility2CDMax = player.AbilityACooldown;
                    break;
                case Player.CharacterType.Ranged:
                    rangedAbility1CDMax = player.AbilityBCooldown;
                    rangedAbility2CDMax = player.AbilityACooldown;
                    break;
            }
        }
    }

    public void SetCooldown(Player.CharacterType cType, int abilityNumber) {
        switch (cType) {
            case Player.CharacterType.Melee:
                switch (abilityNumber) {
                    case 0:
                        StartCoroutine(VisualizeCooldown(meleeAbilityFillImage[0], meleeAbility1CDMax));
                        break;
                    case 1:
                        StartCoroutine(VisualizeCooldown(meleeAbilityFillImage[1], meleeAbility2CDMax));
                        break;
                }

                break;

            case Player.CharacterType.Ranged:
                switch (abilityNumber) {
                    case 0:
                        StartCoroutine(VisualizeCooldown(rangedAbilityFillImage[0], rangedAbility1CDMax));
                        break;
                    case 1:
                        StartCoroutine(VisualizeCooldown(rangedAbilityFillImage[1], rangedAbility2CDMax));
                        break;
                }
                break;
        }
    }

    IEnumerator VisualizeCooldown(Image relevantFillImage, float maxCd) {
        relevantFillImage.fillAmount = 0;
        var fillAmountStep = 1 / (maxCd * 10);
        for (int i = 0; i < maxCd * 10; i++) {
            yield return new WaitForSeconds(0.1f);
            relevantFillImage.fillAmount += fillAmountStep;
        }
    }
}