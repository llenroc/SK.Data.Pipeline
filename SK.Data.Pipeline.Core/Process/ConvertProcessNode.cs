﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SK.Data.Pipeline.Core
{
    public class ConvertProcessNode : ProcessNode
    {
        public Func<Entity, Entity> ConvertFunc { get; set; }

        public ConvertProcessNode(DataNode parent, Func<Entity, Entity> convertFunc)
            : base(parent)
        {
            ConvertFunc = convertFunc;
        }

        protected override IEnumerable<Entity> GetEntities()
        {
            foreach (var entity in Parent.Entities)
            {
                yield return ConvertFunc(entity);
            }
        }
    }
}
