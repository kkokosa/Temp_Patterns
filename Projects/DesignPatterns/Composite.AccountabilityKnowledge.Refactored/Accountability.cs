using System;
using System.Collections.Generic;

namespace Composite.AccountabilityKnowledge.Refactored
{
    public class Accountability
    {
        private readonly AccountabilityType type;
        private readonly Party parent;
        private readonly Party child;

        public Accountability(AccountabilityType type, Party child, Party parent)
        {
            this.type = type;
            this.parent = parent;
            this.child = child;
        }

        public AccountabilityType Type => type;

        public Party Parent => parent;

        public Party Child => child;
    }
}