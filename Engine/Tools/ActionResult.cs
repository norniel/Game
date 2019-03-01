using Engine.Heros;

namespace Engine.Tools
{
    public interface IActionResult
    {
        bool IsFinished { get; }
        void Apply(Hero hero);
    }
    
    class FinishedActionResult : IActionResult
    {
        public bool IsFinished => true;
        public void Apply(Hero hero) { }
    }

    class UnFinishedActionResult : IActionResult
    {
        public bool IsFinished => false;
        public void Apply(Hero hero) { }
    }
}
