using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShopController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI AlertText;
    [SerializeField] public int healthPrice = 5;
    [SerializeField] public int damagePrice = 3;
    [SerializeField] public int speedPrice = 2;


    public void SetText(int area) {
        if (area == 1) {
            AlertText.text = "Press E to fish";
        }
        else if (area == 2) {
            AlertText.text = "Press E to upgrade health for " + healthPrice;
        }
        else if (area == 3) {
            AlertText.text = "Press E to upgrade damage for " + damagePrice;
        }
        else if (area == 4) {
            AlertText.text = "Press E to upgrade bobber speed for " + speedPrice;
        }
    }

    public void RemoveText() {
        AlertText.text = "";
    }

    public void IncreasePrice(int choice) {
        if (choice == 1) {
            healthPrice += healthPrice;
            NDSaveLoad.SaveInt("HealthPrice", healthPrice);
            NDSaveLoad.Flush();
        }
        if (choice == 2) {
            damagePrice += damagePrice;
            NDSaveLoad.SaveInt("DamagePrice", damagePrice);
            NDSaveLoad.Flush();
        }
        if (choice == 3) {
            speedPrice += speedPrice;
            NDSaveLoad.SaveInt("SpeedPrice", speedPrice);
            NDSaveLoad.Flush();
        }
    }

    public int GetPrice(int choice) {
        if (choice == 1) {
            return healthPrice;
        }
        if (choice == 2) {
            return damagePrice;
        }
        if (choice == 3) {
            return speedPrice;
        }

        return 0;
    }

    public void setPrice(int choice, int price) {
        if (choice == 1) {
            healthPrice = price;
        }
        if (choice == 2) {
            damagePrice = price;
        }
        if (choice == 3) {
            speedPrice = price;
        }
    }
}
