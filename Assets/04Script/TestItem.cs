using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestItem : MonoBehaviour
{
    [SerializeField]
    InventoryItemData itemData;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) 
        {
            GameManager.Inst.LootingItem(itemData);
            Destroy(gameObject);
        }

    }
}
