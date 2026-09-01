using UnityEngine;
using System.Collections;

public class TutorialManager : MonoBehaviour
{
    [Header("Tutorial Elements")]
    [SerializeField] private GameObject tutorialFingerObject;
    [SerializeField] private SpriteRenderer tutorialFingerRenderer;
    [SerializeField] private LineRenderer dashedLineRenderer;
    
    [Header("Animation Settings")]
    [SerializeField] private float animationDuration = 2f;
    [SerializeField] private float dashLength = 0.2f;
    [SerializeField] private float dashGap = 0.1f;
    
    private const string TUTORIAL_PREF_KEY = "tutorialSwipeDone";
    private bool tutorialActive = false;
    private bool tutorialCompleted = false;
    
    private Colony playerColony;
    private Colony enemyColony;
    
    private void Start()
    {
        if (IsTutorialCompleted())
        {
            HideTutorial();
            return;
        }
        
        if (BattleManager.Instance != null && BattleManager.Instance.Config.currentLevel != 1)
        {
            HideTutorial();
            return;
        }
        
        StartCoroutine(WaitForColoniesAndShowTutorial());
    }
    
    private bool IsTutorialCompleted()
    {
        return PlayerPrefs.GetInt(TUTORIAL_PREF_KEY, 0) == 1;
    }
    
    private IEnumerator WaitForColoniesAndShowTutorial()
    {
        while (BattleManager.Instance == null)
        {
            yield return null;
        }
        
        while (playerColony == null || enemyColony == null)
        {
            var colonies = BattleManager.Instance.GetAllColonies();
            foreach (var colony in colonies)
            {
                if (colony.Owner == ColonyOwner.Player)
                    playerColony = colony;
                else if (colony.Owner == ColonyOwner.Enemy)
                    enemyColony = colony;
            }
            
            yield return null;
        }
        
        SetupTutorial();
        ShowTutorial();
    }
    
    private void SetupTutorial()
    {
        if (dashedLineRenderer == null && tutorialFingerObject != null)
        {
            GameObject lineObj = new GameObject("TutorialDashedLine");
            lineObj.transform.SetParent(tutorialFingerObject.transform);
            dashedLineRenderer = lineObj.AddComponent<LineRenderer>();
            
            dashedLineRenderer.startWidth = 0.05f;
            dashedLineRenderer.endWidth = 0.05f;
            dashedLineRenderer.material = new Material(Shader.Find("Sprites/Default"));
            dashedLineRenderer.startColor = new Color(1f, 1f, 1f, 0.6f);
            dashedLineRenderer.endColor = new Color(1f, 1f, 1f, 0.6f);
            dashedLineRenderer.sortingOrder = 9;
            dashedLineRenderer.useWorldSpace = true;
        }
    }
    
    private void ShowTutorial()
    {
        if (tutorialFingerObject != null)
        {
            tutorialFingerObject.SetActive(true);
            tutorialActive = true;
            StartCoroutine(AnimateTutorial());
        }
    }
    
    private void HideTutorial()
    {
        tutorialActive = false;
        if (tutorialFingerObject != null)
        {
            tutorialFingerObject.SetActive(false);
        }
    }
    
    private IEnumerator AnimateTutorial()
    {
        while (tutorialActive && playerColony != null && enemyColony != null)
        {
            float elapsed = 0f;
            
            while (elapsed < animationDuration && tutorialActive)
            {
                float t = elapsed / animationDuration;
                Vector3 startPos = playerColony.Position;
                Vector3 endPos = enemyColony.Position;
                Vector3 currentPos = Vector3.Lerp(startPos, endPos, t);
                
                if (tutorialFingerObject != null)
                {
                    tutorialFingerObject.transform.position = new Vector3(currentPos.x, currentPos.y, -1f);
                }
                
                DrawDashedLine(startPos, currentPos);
                
                elapsed += Time.deltaTime;
                yield return null;
            }
            
            yield return new WaitForSeconds(0.5f);
        }
        
        if (dashedLineRenderer != null)
        {
            dashedLineRenderer.positionCount = 0;
        }
    }
    
    private void DrawDashedLine(Vector3 start, Vector3 end)
    {
        if (dashedLineRenderer == null) return;
        
        Vector3 direction = end - start;
        float totalDistance = direction.magnitude;
        direction.Normalize();
        
        int segmentCount = Mathf.CeilToInt(totalDistance / (dashLength + dashGap));
        dashedLineRenderer.positionCount = segmentCount * 2;
        
        int posIndex = 0;
        float currentDistance = 0f;
        
        while (currentDistance < totalDistance && posIndex < dashedLineRenderer.positionCount - 1)
        {
            Vector3 dashStart = start + direction * currentDistance;
            float remainingDistance = totalDistance - currentDistance;
            float actualDashLength = Mathf.Min(dashLength, remainingDistance);
            Vector3 dashEnd = dashStart + direction * actualDashLength;
            
            dashStart.z = -0.5f;
            dashEnd.z = -0.5f;
            
            dashedLineRenderer.SetPosition(posIndex, dashStart);
            dashedLineRenderer.SetPosition(posIndex + 1, dashEnd);
            
            posIndex += 2;
            currentDistance += dashLength + dashGap;
        }
    }
    
    public void OnTentacleLaunchRequested()
    {
        if (!tutorialCompleted && tutorialActive)
        {
            tutorialCompleted = true;
            PlayerPrefs.SetInt(TUTORIAL_PREF_KEY, 1);
            PlayerPrefs.Save();
            HideTutorial();
        }
    }
}
