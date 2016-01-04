namespace Battlestations
{
    using System.Collections.Generic;
    using UnityEngine;

    public class Mission : MonoBehaviour
    {
        public int CurrentRound = 1;

        public MissionState CurrentMissionState = MissionState.Assemble;

        public RoundStep CurrentRoundStep = RoundStep.PowerGeneration;

        public PhaseStep CurrentPhaseStep = PhaseStep.ShipMovement;

        public Ship PlayerShip;

        public List<Ship> NPCShips;

        public List<Character> PlayerCharacters;

        public List<Character> NonPlayerCharacters;

        // TODO: Space map

        public virtual EndMissionState CheckEndOfMission()
        {
            return EndMissionState.MissionInProgress;
        }

        void Update()
        {
            switch (CurrentMissionState)
            {
                case MissionState.MissionLoop:
                    UpdateMissionLoop();
                    break;
            }
        }

        void UpdateMissionLoop()
        {
            switch (CurrentRoundStep)
            {
                case RoundStep.PowerGeneration:
                    // TODO: Show animation for player ship generating power?
                    PlayerShip.GeneratePower();

                    foreach (var ship in NPCShips)
                    {
                        ship.GeneratePower();
                    }

                    CurrentRoundStep = RoundStep.PhaseOne;
                    break;
                case RoundStep.PhaseOne:
                case RoundStep.PhaseTwo:
                case RoundStep.PhaseThree:
                case RoundStep.PhaseFour:
                case RoundStep.PhaseFive:
                case RoundStep.PhaseSix:
                    UpdatePhase();
                    break;
                case RoundStep.Bookkeeping:
                    PlayerShip.EndRound();

                    foreach (var ship in NPCShips)
                    {
                        ship.EndRound();
                    }

                    EndMissionState ended = CheckEndOfMission();
                    if (ended != EndMissionState.MissionInProgress)
                    {
                        // TODO: mission is complete, so resolve mission
                    }

                    CurrentRoundStep = RoundStep.PowerGeneration;
                    break;
            }
        }

        void UpdatePhase()
        {
            switch (CurrentPhaseStep)
            {
                case PhaseStep.ShipMovement:
                    break;
                case PhaseStep.Collisions:
                    break;
                case PhaseStep.MissleMovement:
                    break;
                case PhaseStep.HeroActions:
                    break;
                case PhaseStep.EnemyGrenadeDetonation:
                    break;
                case PhaseStep.EnemyActions:
                    break;
                case PhaseStep.HeroGrenadeDetonation:
                    break;
                case PhaseStep.CharacterEffects:
                    break;
                case PhaseStep.ControlRecovery:
                    CurrentRoundStep++;
                    break;
            }
        }
    }
}
