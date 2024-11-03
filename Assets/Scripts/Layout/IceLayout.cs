using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IceLayout : MonoBehaviour
{
    [SerializeField]
    Image background;

    [SerializeField]
    Image ice;
    // Start is called before the first frame update
    public void SetIceLevel(int level)
    {
        Color iceColor = ice.color;
        iceColor.a = (level * 50) / 255;
        ice.color = iceColor;//�ִ밡 255

        Color backColor = background.color;
        backColor.a = ((float)level * 2) / 255;
        background.color = backColor;
    }
}
