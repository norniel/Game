using System.Collections.Generic;
using Engine.Interfaces;

namespace Engine.Objects.Food
{
    public class NutKernel : FixedObject, IEatable
    {
        public NutKernel()
        {
            IsPassable = true;

            Size = new Size(1, 1);

            Id = 0x00002600;
        }

        public override int Weight => 1;

        public override void InitializeProperties()
        {
            this.Properties = new HashSet<Property>
            {
                Property.Pickable,
                Property.Eatable
            };
        }

        public override string Name => "Nut Kernel";
        public int Poisoness => 0;

        public int Satiety => 4;
    }
}