﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SK.Data.Pipeline.Core
{
    public class FilterProcessNode : ProcessNode
    {
        public Predicate<Entity> ShouldFilter { get; set; }

        public FilterProcessNode(DataNode parent, Predicate<Entity> shouldFilter)
            : base(parent)
        {
            ShouldFilter = shouldFilter;
        }

        protected override IEnumerable<Entity> GetEntities()
        {
            foreach (var entity in Parent.Entities)
            {
                if (ShouldFilter(entity)) continue;

                yield return entity;
            }
        }
    }
}
