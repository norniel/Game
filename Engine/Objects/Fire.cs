using System.Collections.Generic;
using Engine.Interfaces;
using Engine.ObjectStates;

namespace Engine.Objects
{
    internal class Fire : FixedObject, IBurning, IWithObjectWithState
    {
        public ObjectWithState ObjectWithState { get; }

        public Fire()
        {
            IsPassable = false;

            Size = new Size(1, 1);

            Id = 0x00000600;

            ObjectWithState =
                new ObjectWithState(
                    new List<IObjectState>
                    {
                        new Firing {TickCount = 300, Distribution = 10, Eternal = false},
                        new Attenuating {TickCount = 150, Distribution = 10, Eternal = false}
                    }, 
                    false,
                    OnLastStateFinished);

            Name = "Fire";
        }

        public override void InitializeProperties()
        {
            Properties = new HashSet<Property>
            {Property.Burning};
        }
        
        public int TimeOfBurning {
            get
            {
                if (ObjectWithState.CurrentState is Firing)
                {
                    return ObjectWithState.TicksToNextState;
                }

                return 0;
            }
            set => ObjectWithState.ChangeState(0, value);
        }

        public int LightRadius {
            get
            {
                if (ObjectWithState.CurrentState is Attenuating)
                    return 1;

                return 3;
            }
        }

        public override uint GetDrawingCode()
        {
            if (ObjectWithState.CurrentState is Attenuating)
                return 0x00001500;

            return Id;
        }

        public void OnLastStateFinished()
        {
            RemoveFromContainer?.Invoke();
        }
    }
}
