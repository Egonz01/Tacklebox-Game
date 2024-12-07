using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MoneyCounter : MonoBehaviour
{
    [SerializeField] Fisherman man;
    [SerializeField] TextMeshProUGUI moneyText;
    [SerializeField] TextMeshProUGUI healthText;
    [SerializeField] TextMeshProUGUI damageText;


    // Update is called once per frame
    void Update()
    {
        moneyText.text = man.GetMoney().ToString();
        healthText.text = man.GetHealth().ToString();
        damageText.text = man.GetDamage().ToString();
    }
}
