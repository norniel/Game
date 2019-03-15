using System.Collections.Generic;
using System.Linq;
using Engine.Interfaces;
using Engine.ObjectStates;

namespace Engine.Objects
{
    public class ObjectWithStateContext : FixedObjectContext
    {
        public Dictionary<ObjectStates.ObjectStates, ObjStateProperties> ObjectStateProps { get; set; } =
            new Dictionary<ObjectStates.ObjectStates, ObjStateProperties>();

        public Dictionary<ObjectStates.ObjectStates, uint>
            BaseIds = new Dictionary<ObjectStates.ObjectStates, uint>();

        public override GameObject Produce()
        {
            return new FixedObjectWithState(this);
        }
    }

    class FixedObjectWithState: FixedObject, IWithObjectWithState
    {
        public FixedObjectWithState(ObjectWithStateContext context)
        :base(context)
        {
            var props = context.ObjectStateProps.Select(pair => new ObjectState(pair.Key, pair.Value)).ToList();
            ObjectWithState =
                new ObjectWithState(
                    props,
                    false,
                    OnLastStateFinished);
        }

        public ObjectWithState ObjectWithState { get; }
        private Dictionary<ObjectStates.ObjectStates, uint> BaseIds { get; set; }

        private void OnLastStateFinished()
        {
            RemoveFromContainer?.Invoke();
        }

        public override uint GetDrawingCode()
        {
            return DrawingCode();
        }

        private uint DrawingCode()
        {
            return ObjectWithState.CurrentState?.Id ?? Id;
        }

        public override uint GetBaseCode()
        {
            if (ObjectWithState.CurrentState == null)
                return DrawingCode();

            var baseIdInDic = BaseIds.TryGetValue(ObjectWithState.CurrentState.Name, out var baseId);
            return baseIdInDic ? baseId : DrawingCode();
        }
    }
}
