using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private GamePhase currentPhase = GamePhase.choosingChunk;

    // Lecture seule pour les autres scripts
    public GamePhase CurrentPhase => currentPhase;

    public event Action<GamePhase> OnPhaseChanged;

    private void Start()
    {
        // Previent les abonnes de la phase de depart
        OnPhaseChanged?.Invoke(currentPhase);
    }

    public void SetPhase(GamePhase newPhase)
    {
        if (currentPhase == newPhase)
        {
            return;
        }

        currentPhase = newPhase;
        Debug.Log($"Phase : {currentPhase}");
        OnPhaseChanged?.Invoke(currentPhase);
    }
}
