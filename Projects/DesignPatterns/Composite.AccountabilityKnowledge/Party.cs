using System;
using System.Collections.Generic;

namespace Composite.AccountabilityKnowledge
{
    public class Party
    {
        protected readonly string name;
        protected readonly PartyType type;
        protected readonly List<Accountability> parents;
        protected readonly List<Accountability> children;

        public Party(string name, PartyType type)
        {
            this.type = type;
            this.name = name;
            this.parents = new List<Accountability>();
            this.children = new List<Accountability>();
        }

        public void RegisterAsParent(Accountability accountability)
        {
            Children.Add(accountability);
        }

        public void RegisterAsChild(Accountability accountability)
        {
            Parents.Add(accountability);
        }

        public IEnumerable<Party> AllParents
        {
            get
            {
                foreach (var accountability in Parents)
                {
                    yield return accountability.Parent;
                    foreach (var parentAccountability in accountability.Parent.AllParents)
                    {
                        yield return parentAccountability;
                    }
                }
            }
        }
        public IEnumerable<Party> AllChildren
        {
            get
            {
                foreach (var accountability in Children)
                {
                    yield return accountability.Child;
                    foreach (var childAccountability in accountability.Child.AllChildren)
                    {
                        yield return childAccountability;
                    }
                }
            }
        }

        public List<Accountability> Parents => parents;

        public List<Accountability> Children => children;

        public PartyType Type
        {
            get => type;
        }
    }
}
