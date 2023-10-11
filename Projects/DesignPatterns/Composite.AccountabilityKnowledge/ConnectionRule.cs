namespace Composite.AccountabilityKnowledge
{
    public struct ConnectionRule
    {
        private readonly AccountabilityType accountabilityType;
        private readonly PartyType allowedParent;
        private readonly PartyType allowedChild;

        public ConnectionRule(AccountabilityType accountabilityType, PartyType allowedChild, PartyType allowedParent)
        {
            this.accountabilityType = accountabilityType;
            this.allowedParent = allowedParent;
            this.allowedChild = allowedChild;
        }

        public AccountabilityType Type => accountabilityType;

        public PartyType AllowedParent => allowedParent;

        public PartyType AllowedChild => allowedChild;
    }
}