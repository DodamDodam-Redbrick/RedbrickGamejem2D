using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class EditDeckPanel : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI discriptionText;
    [SerializeField]
    GameObject selectDectPanel;
    [SerializeField]
    GameObject[] editDectPopUp;

    [SerializeField] TextMeshProUGUI goldText;
    public List<UnitInfo> deckInfo = new List<UnitInfo>();
    public List<GameObject> spriteObj = new List<GameObject>();
    public int maxDeckIndex = 5;
    private void OnEnable()
    {
        goldText.text = $"{Player.Instance.gold}";
        foreach (GameObject popup in editDectPopUp)
        {
            Transform popUpTrans = popup.transform;
            foreach (Transform child in popUpTrans)
            {
                Destroy(child.gameObject);
            }
        }
        deckInfo.Clear();
        // 중복 유닛 추적을 위한 HashSet
        HashSet<UnitType> addedUnits = new HashSet<UnitType>();

        List<UnitInfo> playerUnitList = Player.Instance.unitList;
        int deckSize = Mathf.Min(maxDeckIndex, playerUnitList.Count);

        for (int i = 0; i < deckSize; i++)
        {
            if (playerUnitList[i] == null)
                continue;

            if (playerUnitList.Count < i)
                break;

            // 중복 유닛을 제외하기 위해 체크
            if (addedUnits.Contains(playerUnitList[i].unitType))
                continue;

            // 중복이 아니면 추가
            deckInfo.Add(playerUnitList[i]);
            addedUnits.Add(playerUnitList[i].unitType);

            // UI 오브젝트 생성
            GameObject unitObj = new GameObject();
            unitObj.name = ($"{deckInfo[deckInfo.Count - 1].unitType}");
            Image unitImage = unitObj.AddComponent<Image>();
            unitImage.sprite = deckInfo[deckInfo.Count - 1].thumbnail;
            TextMeshProUGUI unitText = Instantiate(discriptionText);
            unitText.text = $"{DataManager.Instance.rewardData[(RewardType)deckInfo[deckInfo.Count - 1].unitType].description}";
            unitText.transform.SetParent(editDectPopUp[deckInfo.Count - 1].transform, false);
            unitText.transform.localPosition = new Vector3(0, -160, 0);
            unitObj.transform.SetParent(editDectPopUp[deckInfo.Count - 1].transform, false);
            unitObj.transform.localScale = new Vector3(4, 4, 4);
            unitObj.transform.localPosition += new Vector3(0, 130, 0);
        }
    }

}

