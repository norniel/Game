using Game.Engine.Objects;
using Microsoft.Practices.ObjectBuilder2;

namespace Game.Engine
{
    //Container- quantity of stacks and capacity of stack. MaxContainerCapacity = quantityOfStacks * capacityOfStack
    //sum of items weight = containerWeight
    //Container is full = containerWeight >= MaxContainerCapacity
    public class Container
    {
        private readonly int _stackQuantity;

        private readonly int _stackCapacity;

        private readonly int _maxContainerCapacity;
        public Container(int stackQuantity, int stackCapacity)
        {
            _stackQuantity = stackQuantity;
            _stackCapacity = stackCapacity;
            _maxContainerCapacity = _stackCapacity*_stackQuantity;
        }

        public int StackQuantity {
            get{ return _stackQuantity; }
        }

        public int StackCapacity {
            get { return _stackCapacity; }
        }

        public int MaxContainerCapacity
        {
            get { return _maxContainerCapacity; }
        }

        public int ContainerCapacity { get; set; }

        public bool IsFull {
            get { return ContainerCapacity >= MaxContainerCapacity; }
        }

        public virtual bool Add(GameObject gameObject)
        {
            if (!IsGameObjectFits(gameObject))
                return false;

            ContainerCapacity = gameObject.Weight + ContainerCapacity;

            return true;
        }

        public virtual void Remove(GameObject gameObject)
        {
            DecreaseContainerCapacity(gameObject);
        }

        public void DecreaseContainerCapacity(GameObject gameObject)
        {
            ContainerCapacity = ContainerCapacity >= gameObject.Weight ? ContainerCapacity - gameObject.Weight : 0;
        }

        public bool IsGameObjectFits(GameObject gameObject)
        {
            return (MaxContainerCapacity >= gameObject.Weight + ContainerCapacity);
        }
    }
}
