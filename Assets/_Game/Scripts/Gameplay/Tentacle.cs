using UnityEngine;

public class Tentacle : MonoBehaviour
{
    private Colony sourceColony;
    private Colony targetColony;
    private float unitsToTransfer;
    private float transferredUnits;
    private GameConfig config;
    private LineRenderer lineRenderer;
    
    public void Initialize(Colony source, Colony target, float units, GameConfig gameConfig)
    {
        sourceColony = source;
        targetColony = target;
        unitsToTransfer = units;
        transferredUnits = 0f;
        config = gameConfig;
        
        transform.position = source.Position;
        
        if (sourceColony != null)
        {
            sourceColony.RegisterTentacle();
        }
        
        lineRenderer = GetComponent<LineRenderer>();
        if (lineRenderer != null)
        {
            lineRenderer.positionCount = 2;
            lineRenderer.SetPosition(0, source.Position);
            lineRenderer.SetPosition(1, source.Position);
            lineRenderer.startWidth = 0.1f;
            lineRenderer.endWidth = 0.1f;
            
            Color color = source.Owner == ColonyOwner.Player 
                ? new Color(0f, 0.8f, 1f, 0.8f) 
                : new Color(1f, 0.2f, 0.5f, 0.8f);
            lineRenderer.startColor = color;
            lineRenderer.endColor = color;
        }
    }
    
    private void Update()
    {
        if (sourceColony == null || targetColony == null)
        {
            CleanupAndDestroy();
            return;
        }
        
        Vector3 direction = (targetColony.Position - transform.position).normalized;
        transform.position += direction * config.tentacleSpeed * Time.deltaTime;
        
        if (lineRenderer != null)
        {
            lineRenderer.SetPosition(0, sourceColony.Position);
            lineRenderer.SetPosition(1, transform.position);
        }
        
        float distanceToTarget = Vector3.Distance(transform.position, targetColony.Position);
        if (distanceToTarget < 0.1f)
        {
            float unitsThisFrame = config.unitTransferRate * Time.deltaTime;
            float unitsToSend = Mathf.Min(unitsThisFrame, unitsToTransfer - transferredUnits);
            
            if (unitsToSend > 0)
            {
                targetColony.ReceiveUnits(unitsToSend, sourceColony.Owner);
                transferredUnits += unitsToSend;
            }
            
            if (transferredUnits >= unitsToTransfer)
            {
                CleanupAndDestroy();
            }
        }
    }
    
    private void CleanupAndDestroy()
    {
        if (sourceColony != null)
        {
            sourceColony.UnregisterTentacle();
        }
        Destroy(gameObject);
    }
    
    private void OnDestroy()
    {
        if (sourceColony != null)
        {
            sourceColony.UnregisterTentacle();
        }
    }
}
