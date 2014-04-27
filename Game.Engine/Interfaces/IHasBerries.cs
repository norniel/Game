using System.Security.Cryptography.X509Certificates;
using Game.Engine.Objects;

namespace Game.Engine.Interfaces
{
    interface IHasBerries
    {
        int BerriesPerCollectCount { get; set; }

        int BerriesCount { get; set; }

        Berry GetBerry();
    }
}
