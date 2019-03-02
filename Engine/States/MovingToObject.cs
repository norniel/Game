using Engine.Objects;

namespace Engine.States
{
    class MovingToObject:Moving
    {
        private readonly GameObject _destinationObject;

        public MovingToObject(MobileObject mobileObject, Point destination, GameObject destinationObject) : base(mobileObject, destination)
        {
            _destinationObject = destinationObject;
        }

        public override void Act()
        {
            if (Game.Map.GetObjectFromCell(Map.PointToCell(_destination)) != _destinationObject)
            {
                _mobileObject.StateEvent.FireEvent();
                return;
            }

            base.Act();
        }
    }
}
