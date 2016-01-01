namespace Battlestations
{
    using UnityEngine;

    public class Mission : MonoBehaviour
    {
        public RoundStep CurrentRoundStep = RoundStep.PowerGeneration;

        public PhaseStep CurrentPhaseStep = PhaseStep.ShipMovement;
    }
}
