using UnityEngine;

public class RingActivationSupport : MonoBehaviour
{
    public string requiredItemName = "Power Ring";
    public Transform ringSlot;
    public GameObject uiDepose;
    public GameObject uiActivate;
    public GameObject secretDoor;
    public float doorOpenSpeed = 2f;

    private bool ringDeposited = false;
    private bool playerInRange = false;
    private WeaponManager playerWeaponManager;

    private GameObject placedRing;

    void Update()
    {
        if (!playerInRange || playerWeaponManager == null)
        {
            HideUI();
            return;
        }

        GameObject heldObject = playerWeaponManager.currentWeaponGO;

        if (!ringDeposited)
        {
            if (heldObject != null && heldObject.name.Contains(requiredItemName))
            {
                uiDepose.SetActive(true);
                uiActivate.SetActive(false);

                if (Input.GetKeyDown(KeyCode.E))
                {
                    placedRing = Instantiate(heldObject, ringSlot.position, ringSlot.rotation, transform);
                    placedRing.transform.localScale = ringSlot.lossyScale;
                    ringDeposited = true;

                    playerWeaponManager.UnEquipWeapon();
                    Destroy(heldObject);

                    HideUI();
                }
            }
            else
            {
                HideUI();
            }
        }
        else
        {
            uiDepose.SetActive(false);
            uiActivate.SetActive(true);

            if (Input.GetKeyDown(KeyCode.E))
            {
                HideUI();
                OpenSecretDoor();
            }
        }
    }

    void HideUI()
    {
        if (uiDepose != null) uiDepose.SetActive(false);
        if (uiActivate != null) uiActivate.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerWeaponManager = other.GetComponentInChildren<WeaponManager>();
            playerInRange = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            playerWeaponManager = null;
            HideUI();
        }
    }

    void OpenSecretDoor()
    {
        Debug.Log("Activation de la fonction OpenSecretDoor");
        if (secretDoor != null)
        {
            StopAllCoroutines();
            StartCoroutine(MoveDoor(secretDoor.transform));
        }
    }

    System.Collections.IEnumerator MoveDoor(Transform door)
    {
        float distanceToMove = 5f;
        float speed = doorOpenSpeed;
    
        Vector3 startPos = door.position;
        Vector3 targetPos = startPos + door.up * distanceToMove; // je prend l'axe local car sinon, il part en diagonale

        Debug.Log($"🚪 Déplacement de {startPos} vers {targetPos}");

        float elapsed = 0f;
        float duration = distanceToMove / speed; // temps basé sur la distance

        while (elapsed < duration)
        {
            float t = elapsed / duration;
            door.position = Vector3.Lerp(startPos, targetPos, t);
            elapsed += Time.deltaTime;
            yield return null;
        }

        door.position = targetPos;
        Debug.Log("✅ Déplacement terminé !");
    }

}
