﻿using Engine.Objects;

namespace Engine
{
    public class FixedObject : GameObject
    {
        public bool IsPassable { get; protected set; }

        public Size Size { get; protected set; }

        public FixedObject()
        {
            IsPassable = true;
        }

        public FixedObject(Size size, uint id)
        {
            IsPassable = true;
            Size = size;
            Id = id;
        }

        public void Draw(){}

        public override string Name
        {
            get { return "Fixed objects"; }
        }

        public override uint GetDrawingCode()
        {
            return this.Id;
        }
    }

}