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

        public virtual void Assemble()
        {

        }

        public virtual EndMissionState CheckEndOfMission()
        {
            return EndMissionState.MissionInProgress;
        }

        public virtual void ResolveEndOfMission()
        {
            
        }

        void Update()
        {
            switch (CurrentMissionState)
            {
                case MissionState.Assemble:
                    // TODO: Allow the selection of player characters, and player ship
                    break;
                case MissionState.DetermineUpgrades:
                    // TODO: Place players in appropriate modules for upgrades
                    // NOTE: One player or bot MUST be at the hyperdrive battlestation
                    break;
                case MissionState.WarpIn:
                    // TODO: Determine the warp in location and orientation based on dice rolls
                    break;
                case MissionState.ResolveUpgrades:
                    // TODO: Resolve any upgrades based on dice rolls
                    break;
                case MissionState.MissionLoop:
                    UpdateRound();
                    break;
                case MissionState.EndOfMission:
                    CurrentMissionState++;
                    break;
                case MissionState.ResolveEndOfMission:
                    // TODO: Handle any other business (for example, checking for delivery of smuggled goods)
                    ResolveEndOfMission();
                    CurrentMissionState++;
                    break;
            }
        }

        void UpdateRound()
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

                    CurrentPhaseStep = PhaseStep.ShipMovement;
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
                        CurrentMissionState = MissionState.EndOfMission;
                    }
                    else
                    {
                        CurrentRound++;
                        CurrentRoundStep = RoundStep.PowerGeneration;
                    }
                    
                    break;
            }
        }

        void UpdatePhase()
        {
            switch (CurrentPhaseStep)
            {
                case PhaseStep.ShipMovement:
                    CurrentPhaseStep++;
                    break;
                case PhaseStep.Collisions:
                    CurrentPhaseStep++;
                    break;
                case PhaseStep.MissleMovement:
                    CurrentPhaseStep++;
                    break;
                case PhaseStep.HeroActions:
                    CurrentPhaseStep++;
                    break;
                case PhaseStep.EnemyGrenadeDetonation:
                    CurrentPhaseStep++;
                    break;
                case PhaseStep.EnemyActions:
                    CurrentPhaseStep++;
                    break;
                case PhaseStep.HeroGrenadeDetonation:
                    CurrentPhaseStep++;
                    break;
                case PhaseStep.CharacterEffects:
                    CurrentPhaseStep++;
                    break;
                case PhaseStep.ControlRecovery:
                    CurrentRoundStep++;
                    break;
            }
        }
    }
}
