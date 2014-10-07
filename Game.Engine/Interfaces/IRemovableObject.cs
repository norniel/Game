using System;

namespace Game.Engine.Interfaces
{
    interface IRemovableObject
    {
        Action RemoveFromContainer { get; set; }
    }
}
