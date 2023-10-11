using System;
using System.Collections.Generic;

namespace Composite.AccountabilityKnowledge.Refactored
{
    public class AccountabilityService
    {
        public void RegisterParent(AccountabilityType type, Party child, Party parent)
        {
            CheckRelationshipAllowed(type, child, parent);

            var accountability = new Accountability(type, child, parent);
            child.Parents.Add(accountability);
            parent.Children.Add(accountability);
        }
        public void RegisterChild(AccountabilityType type, Party child, Party parent)
        {
            CheckRelationshipAllowed(type, child, parent);

            var accountability = new Accountability(type, child, parent);
            parent.Children.Add(accountability);
            child.Parents.Add(accountability);
        }

        private void CheckRelationshipAllowed(AccountabilityType type, Party child, Party parent)
        {
            if (!rules.Exists(x => x.Type == type &&
                                         x.AllowedParent == parent.Type &&
                                         x.AllowedChild == child.Type))
            {
                throw new InvalidOperationException();
            }
        }

        public List<ConnectionRule> rules = new List<ConnectionRule>();

        public void RegisterRule(AccountabilityType type, PartyType allowedChild, PartyType allowedParent)
        {
            rules.Add(new ConnectionRule(type, allowedChild, allowedParent));
        }
    }
}