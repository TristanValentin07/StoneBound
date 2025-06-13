using UnityEngine;

public class PlayerInventory : Inventory
{
    private Canvas inventoryUI;           // Canvas de l'inventaire
    public MonoBehaviour activeCameraScript;
    private Move_Player movePlayer;         // Script de déplacement du joueur

    protected override void Start()
    {
        base.Start();
        if (inventoryUI == null)
        {
            inventoryUI = GameObject.Find("InventoryUI").GetComponent<Canvas>();
            if (inventoryUI == null)
            {
                Debug.LogError("❌ Canvas introuvable sur InventoryUI !");
                return;
            }
        }

        inventoryUI.gameObject.SetActive(false); // Inventaire fermé par défaut
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Récupérer le script de mouvement s'il n'est pas assigné dans l'inspecteur
        if (movePlayer == null)
            movePlayer = GetComponent<Move_Player>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab)) // Ouvrir/fermer l'inventaire avec Tab
        {
            bool isOpen = !inventoryUI.gameObject.activeSelf;
            inventoryUI.gameObject.SetActive(isOpen);

            if (isOpen)
            {
                // Débloquer et afficher le curseur
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;

                // Désactiver la rotation de la caméra
                if (activeCameraScript != null)
                    activeCameraScript.enabled = false;

                // Désactiver le déplacement du joueur
                if (movePlayer != null)
                    movePlayer.enabled = false;
            }
            else
            {
                // Rebloquer et masquer le curseur
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;

                // Réactiver la rotation de la caméra
                if (activeCameraScript != null)
                    activeCameraScript.enabled = true;

                // Réactiver le déplacement du joueur
                if (movePlayer != null)
                    movePlayer.enabled = true;
            }
        }
    }
}
