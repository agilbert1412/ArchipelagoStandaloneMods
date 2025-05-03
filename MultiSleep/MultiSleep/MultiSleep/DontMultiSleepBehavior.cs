namespace MultiSleep.MultiSleep
{
    public class DontMultiSleepBehavior : MultiSleepBehavior
    {

        public DontMultiSleepBehavior()
        {
        }

        public override bool ShouldKeepSleeping()
        {
            return false;
        }
    }
}
