using UnityEngine;

public class CheatCodeMenu : MonoBehaviour
{
    public GameObject cheatPanel;
    private GameObject player;

    public Item itemAK47;
    public Item itemRailgun;
    public Item itemPistol;

    public Item itemSyringe;
    public Item itemRedFlask;
    public Item itemGreenFlask;

    public InventoryUI inventoryUI;

    private Inventory playerInventory;
    private Health_Manager healthManager;
    private Stamina_Manager staminaManager;
    
    private void Start()
    {
        if (cheatPanel != null)
            cheatPanel.SetActive(false);

        // 🔍 Récupération directe dans la scène (pas via le joueur)
        healthManager = FindAnyObjectByType<Health_Manager>();
        staminaManager = FindAnyObjectByType<Stamina_Manager>();

        if (healthManager == null) Debug.LogWarning("❌ Health_Manager non trouvé dans la scène !");
        if (staminaManager == null) Debug.LogWarning("❌ Stamina_Manager non trouvé dans la scène !");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            cheatPanel.SetActive(!cheatPanel.activeSelf);
            Cursor.lockState = cheatPanel.activeSelf ? CursorLockMode.None : CursorLockMode.Locked;
            Cursor.visible = cheatPanel.activeSelf;
            Debug.Log("📂 Menu de cheats " + (cheatPanel.activeSelf ? "ouvert" : "fermé"));
        }
    }

    private GameObject GetPlayer()
    {
        if (player == null)
        {
            player = GameObject.FindWithTag("Player");
            if (player != null)
            {
                Debug.Log("👤 Joueur trouvé dynamiquement : " + player.name);
                playerInventory = player.GetComponent<Inventory>();
            }
            else
            {
                Debug.LogWarning("⚠️ Aucun joueur trouvé avec le tag 'Player'");
            }
        }
        return player;
    }

    public void TeleportToForest()
    {
        Debug.Log("TP vers l'autel de la forêt");
        GameObject p = GetPlayer();
        if (p != null)
            p.transform.position = new Vector3(44.34f, 20.45f, 43.77f);
    }

    public void TeleportToChurch()
    {
        Debug.Log("TP vers l’église");
        GameObject p = GetPlayer();
        if (p != null)
            p.transform.position = new Vector3(222.3f, 20.90f, 237f);
    }

    public void GiveAK() { Debug.Log("Give AK47"); GiveItem(itemAK47); }
    public void GiveRailgun() { Debug.Log("Give Railgun"); GiveItem(itemRailgun); }
    public void GivePistol() { Debug.Log("Give Pistol"); GiveItem(itemPistol); }
    public void GiveSyringe() { Debug.Log("Give Seringue"); GiveItem(itemSyringe); }
    public void GiveRedFlask() { Debug.Log("Give Fiole rouge"); GiveItem(itemRedFlask); }
    public void GiveGreenFlask() { Debug.Log("Give Fiole verte"); GiveItem(itemGreenFlask); }

    private void GiveItem(Item item)
    {
        GameObject p = GetPlayer();
        if (p != null && item != null)
        {
            if (playerInventory == null)
                playerInventory = p.GetComponent<Inventory>();

            if (playerInventory != null)
            {
                Debug.Log("📦 Ajout : " + item.itemName);
                bool added = playerInventory.AddItem(item);
                if (added)
                {
                    Debug.Log("✔ Ajouté à l’inventaire.");
                    if (inventoryUI != null)
                    {
                        inventoryUI.UpdateUI();
                        Debug.Log("🔄 UI mise à jour.");
                    }
                    else Debug.LogWarning("❌ InventoryUI non assigné !");
                }
                else Debug.LogWarning("❌ Inventaire plein.");
            }
            else Debug.LogWarning("❌ Pas de script Inventory sur le joueur !");
        }
    }

    public void EnableGodMode()
    {
        if (healthManager != null)
        {
            healthManager.godMode = true;
            Debug.Log("🛡️ God Mode activé !");
        }
        else Debug.LogWarning("❌ Health_Manager introuvable !");
    }

    public void EnableInfiniteStamina()
    {
        staminaManager.infiniteStamina = true;
        Debug.Log("⚡ Stamina infinie activée !");
    }
}
