using ITSG.Excersise.Domain;

namespace ITSG.Excersise.Infrastructure.Domain
{
    public abstract class BaseRepository
    {
        protected bool CheckRule(IDomainRuleChecker domainRule)
        {
            return domainRule.IsOK();
        }
    }
}
