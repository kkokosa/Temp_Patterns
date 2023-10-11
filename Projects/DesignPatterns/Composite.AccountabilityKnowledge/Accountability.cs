using System;
using System.Collections.Generic;

namespace Composite.AccountabilityKnowledge
{
    public class Accountability
    {
        private static List<ConnectionRule> rules = new List<ConnectionRule>();
        private readonly AccountabilityType type;
        private readonly Party parent;
        private readonly Party child;

        public Accountability(AccountabilityType type, Party child, Party parent)
        {
            if (!rules.Exists(x => x.Type == type &&
                                  x.AllowedParent == parent.Type &&
                                  x.AllowedChild == child.Type))
            {
                throw new InvalidOperationException();
            }

            this.type = type;
            this.parent = parent;
            this.parent.RegisterAsParent(this);
            this.child = child;
            this.child.RegisterAsChild(this);
        }

        public AccountabilityType Type => type;

        public Party Parent => parent;

        public Party Child => child;

        public static void RegisterRule(AccountabilityType type, PartyType allowedChild, PartyType allowedParent)
        {
            rules.Add(new ConnectionRule(type, allowedChild, allowedParent));
        }
    }
}