namespace ITSG.Excersise.Domain.Resources
{
    public class UserCanUnlockOnlyHisResourceRule : IDomainRuleChecker
    {
        private readonly Resource _resource;
        private readonly long _userId;

        public UserCanUnlockOnlyHisResourceRule(Resource resource, long userId)
        {
            _resource = resource;
            _userId = userId;
        }

        public bool IsOK()
        {
            if (!_resource.LockedBy.HasValue)
                return false;

            return _resource.LockedBy.Value == _userId;
        }
    }
}
