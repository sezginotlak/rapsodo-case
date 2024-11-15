using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCHealth : MonoBehaviour
{
    public float Health { get; private set; }
    public float Duration { private get; set; } = 60f;

    private void Awake()
    {
        Health = 1f;
    }

    public void DecreaseHealth(NPCCanvas npcCanvas)
    {
        DOTween.To(() => Health, x => Health = x, 0, Duration).SetEase(Ease.Linear).OnUpdate(() => 
        {
            npcCanvas.UpdateHealthBar(Health);
            MainCanvasManager.Instance.UpdateHealthText((int)(Health * 100f));
        });
    }

    public void CancelHealthUpdate()
    {
        DOTween.KillAll();
    }

    public bool IsHealthFinished()
    {
        return Health <= 0f;
    }
}
