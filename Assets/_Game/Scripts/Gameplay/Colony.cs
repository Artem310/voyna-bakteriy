using UnityEngine;
using TMPro;

public enum ColonyOwner
{
    Neutral,
    Player,
    Enemy
}

public class Colony : MonoBehaviour
{
    [Header("Colony Data")]
    [SerializeField] private ColonyOwner owner = ColonyOwner.Neutral;
    [SerializeField] private float units = 10f;
    
    [Header("Visual")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private TextMeshPro unitsText;
    [SerializeField] private Color playerColor = new Color(0f, 0.8f, 1f);
    [SerializeField] private Color enemyColor = new Color(1f, 0.2f, 0.5f);
    [SerializeField] private Color neutralColor = new Color(0.5f, 0.5f, 0.5f);
    
    private GameConfig config;
    
    public ColonyOwner Owner => owner;
    public float Units => units;
    public Vector3 Position => transform.position;
    
    private void Start()
    {
        config = BattleManager.Instance.Config;
        UpdateVisuals();
    }
    
    private void Update()
    {
        if (owner != ColonyOwner.Neutral)
        {
            units += config.growthPerSecond * Time.deltaTime;
        }
        
        UpdateUnitsDisplay();
    }
    
    public void Initialize(ColonyOwner owner, float startUnits)
    {
        this.owner = owner;
        this.units = Mathf.Max(startUnits, 0f);
        UpdateVisuals();
    }
    
    public bool TryRemoveUnits(float amount)
    {
        if (units >= amount)
        {
            units -= amount;
            UpdateUnitsDisplay();
            return true;
        }
        return false;
    }
    
    public void ReceiveUnits(float amount, ColonyOwner attacker)
    {
        if (owner == attacker)
        {
            units += amount;
        }
        else
        {
            units -= amount;
            
            if (units <= 0)
            {
                units = Mathf.Abs(units);
                SetOwner(attacker);
            }
        }
        
        UpdateUnitsDisplay();
    }
    
    private void SetOwner(ColonyOwner newOwner)
    {
        owner = newOwner;
        UpdateVisuals();
        
        if (owner == ColonyOwner.Player || owner == ColonyOwner.Enemy)
        {
            BattleManager.Instance?.CheckVictoryConditions();
        }
    }
    
    private void UpdateVisuals()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = owner switch
            {
                ColonyOwner.Player => playerColor,
                ColonyOwner.Enemy => enemyColor,
                _ => neutralColor
            };
        }
        UpdateUnitsDisplay();
    }
    
    private void UpdateUnitsDisplay()
    {
        if (unitsText != null)
        {
            unitsText.text = Mathf.FloorToInt(units).ToString();
        }
    }
}
