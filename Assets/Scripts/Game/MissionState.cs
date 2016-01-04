namespace Battlestations
{
    /// <summary>
    /// Page 59. 
    /// </summary>
    public enum MissionState
    {
        // --------- PRE MISSION ---------------

        /// <summary>
        /// Assemble crew & equipment for both heroes and enemies.
        /// </summary>
        Assemble,

        DetermineUpgrades,
        WarpIn,
        ResolveUpgrades,

        // -----------  MISSION ---------------

        /// <summary>
        /// Loops through round by round, phase by phase, step by step, until the mission is over.
        /// </summary>
        MissionLoop,

        // --------- POST MISSION ---------------

        EndOfMission,
        ResolveEndOfMission,
        SpoilsOfWar,
        ReviveDeadCharacters,

        // ------- CHARACTER DEVELOPMENT ---------------

        RewardExperience,
        RepairAndHeal,
        Requisitioning,
        ReturnCapturedCrew,
        PurchaseEquipment,
        RestoreLuckAndAbilityPools,

        MissionComplete
    }
}
