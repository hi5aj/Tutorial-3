using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KillCount : MonoBehaviour
{
    Projectile bots;
    public Text fixedAmount;
    // Start is called before the first frame update
    void Start()
    {
        fixedAmount.text = "Robots Fixed: " + bots.fixedBots.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        fixedAmount.text = "Robots Fixed: " + bots.fixedBots.ToString();
    }
}
