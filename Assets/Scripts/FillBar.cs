using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FillBar : MonoBehaviour
{
    Image i;
    Selector s;

    // Start is called before the first frame update
    void Start()
    {
        s = FindObjectOfType<Selector>();
        i = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        i.fillAmount = 1 - s.PercentUsed;
    }
}
