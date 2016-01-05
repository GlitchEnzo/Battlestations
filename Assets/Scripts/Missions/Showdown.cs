namespace Battlestations
{
    /// <summary>
    /// Page 84.
    /// </summary>
    public class Showdown : Mission
    {
        public override void Assemble()
        {
            base.Assemble();

            // add planet at center

            // add ship orbiting it
        }

        public override EndMissionState CheckEndOfMission()
        {
            // Success = enemy ship destroyed, retreated, or captured
            // Fail = players all killed or player ship destroyed

            EndMissionState end = EndMissionState.MissionInProgress;

            if (CurrentRound >= 100)
            {
                end = EndMissionState.MissionFail;
            }

            return end;
        }

        public override void ResolveEndOfMission()
        {
            // Success = enemy ship destroyed or retreated
            // Overwhelming = enemy ship captured
            // Fail = players all killed or player ship destroyed

            base.ResolveEndOfMission();
        }
    }
}