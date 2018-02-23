using System;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;
using Engine.BridgeObjects;

namespace MonoBrJozik.Controls
{
    internal class MonoInvItemInfo: MonoItemInfo
    {
        public readonly List<MonoItemInfo> ClientActions;
        public MonoInvItemInfo(Texture2D texture, Texture2D innerTexture, string text, Action action, List<MonoItemInfo> clientActions) : base(texture, innerTexture, text, action)
        {
            ClientActions = clientActions;
        }
    }
}
