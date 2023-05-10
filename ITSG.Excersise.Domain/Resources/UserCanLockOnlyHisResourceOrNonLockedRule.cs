namespace ITSG.Excersise.Domain.Resources
{
    public class UserCanLockOnlyHisResourceOrNonLockedRule : IDomainRuleChecker
    {

        private readonly Resource resource;
        private readonly long _userId;

        public UserCanLockOnlyHisResourceOrNonLockedRule(Resource resource, long userId)
        {
            this.resource = resource;
            _userId = userId;
        }

        public bool IsOK()
        {
            if (resource.LockedBy == null && resource.LockedTo == null)
                return true;

            if (resource.LockedBy == _userId)
                return true;

            if (resource.LockedTo < DateTime.Now)
                return true;

            return false;

        }
    }
}
