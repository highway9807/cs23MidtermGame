/****************************************************************
*
*  MineIce.cs
*  One ice at a time. Player walks up, holds Space to mine; bar only
*  shows if they have inventory space (maxStack/maxCopies). On complete,
*  TryAdd(1) and one new ice respawns at the same position (via GameState).
*
*****************************************************************/

using UnityEngine;

public class MineIce : MonoBehaviour
{
    [Header("Assign Ice ScriptableObject (e.g. Assets/ScriptableObjects/Ice)")]
    [SerializeField] private ItemDefinition iceItem;

    [Header("Mining")]
    public GameObject progressBar;
    public float miningTime = 2f;

    private GameObject progressBarInstance;
    private float timer;
    private bool inContact;
    private const float HeightOffset = 0.6f;

    private static bool s_oneIceActive;
    private static bool s_respawning;
    private bool m_isTheOne;

    void Start()
    {
        if (s_oneIceActive)
        {
            Destroy(gameObject);
            return;
        }
        s_oneIceActive = true;
        m_isTheOne = true;

        MineIce[] all = FindObjectsByType<MineIce>(FindObjectsSortMode.None);
        for (int i = 0; i < all.Length; i++)
        {
            if (all[i] != this)
                Destroy(all[i].gameObject);
        }
    }

    void OnDestroy()
    {
        if (m_isTheOne && !s_respawning)
            s_oneIceActive = false;
        s_respawning = false;
    }

    void Update()
    {
        if (!inContact)
            return;

        if (!Input.GetKey(KeyCode.Space))
        {
            if (progressBarInstance != null)
            {
                Destroy(progressBarInstance);
                progressBarInstance = null;
            }
            timer = 0f;
            return;
        }

        if (iceItem == null || GameState.gs == null || GameState.gs.playerInv == null ||
            !GameState.gs.playerInv.CanAdd(iceItem, 1))
        {
            if (progressBarInstance != null)
            {
                Destroy(progressBarInstance);
                progressBarInstance = null;
            }
            timer = 0f;
            return;
        }

        if (progressBarInstance == null)
            SpawnProgressBar();

        timer += Time.deltaTime;

        float progress = 1f - (timer / miningTime);
        if (progressBarInstance != null)
            progressBarInstance.transform.localScale = new Vector2(Mathf.Clamp01(progress), 1f);

        if (timer >= miningTime)
        {
            if (progressBarInstance != null)
            {
                Destroy(progressBarInstance);
                progressBarInstance = null;
            }

            if (GameState.gs != null && GameState.gs.playerInv != null)
                GameState.gs.playerInv.TryAdd(iceItem, 1);

            Vector3 respawnPos = transform.position;
            s_respawning = true;
            s_oneIceActive = false;
            if (GameState.gs != null)
                GameState.gs.SpawnIceAt(respawnPos);

            Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
            inContact = true;
    }

    void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            inContact = false;
            timer = 0f;
            if (progressBarInstance != null)
            {
                Destroy(progressBarInstance);
                progressBarInstance = null;
            }
        }
    }

    void SpawnProgressBar()
    {
        if (progressBar == null) return;
        Vector2 spawnPosition = new Vector2(
            transform.position.x,
            transform.position.y + HeightOffset
        );
        progressBarInstance = Instantiate(progressBar, spawnPosition, Quaternion.identity);
    }
}
