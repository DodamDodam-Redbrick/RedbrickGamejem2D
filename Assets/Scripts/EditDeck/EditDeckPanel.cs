using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class EditDeckPanel : MonoBehaviour
{
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
        foreach (GameObject popup in editDectPopUp)
        {
            Transform popUpTrans = popup.transform;
            foreach(Transform child in popUpTrans)
            {
                Destroy(child.gameObject);
            }
        }
        deckInfo.Clear();
        // �ߺ� ���� ������ ���� HashSet
        HashSet<UnitType> addedUnits = new HashSet<UnitType>();

        List<UnitInfo> playerUnitList = Player.Instance.unitList;
        int deckSize = Mathf.Min(maxDeckIndex, playerUnitList.Count);

        for (int i = 0; i < deckSize; i++)
        {
            if (playerUnitList[i] == null)
                continue;

            if (playerUnitList.Count < i)
                break;

            // �ߺ� ������ �����ϱ� ���� üũ
            if (addedUnits.Contains(playerUnitList[i].unitType))
                continue;

            // �ߺ��� �ƴϸ� �߰�
            deckInfo.Add(playerUnitList[i]);
            addedUnits.Add(playerUnitList[i].unitType);

            // UI ������Ʈ ����
            GameObject unitObj = new GameObject();
            unitObj.name = ($"{deckInfo[deckInfo.Count - 1].unitType}");
            Image unitImage = unitObj.AddComponent<Image>();
            unitImage.sprite = deckInfo[deckInfo.Count - 1].thumbnail;
            Button unitButton = unitObj.AddComponent<Button>();
            unitButton.onClick.AddListener(OnClickSelectDeckButton);
            unitObj.transform.SetParent(editDectPopUp[deckInfo.Count - 1].transform, false);
        }
    }

    public void OnClickSelectDeckButton()
    {

    }
}

