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
        private FinishedActionResult()
        {
            
        }

        public static FinishedActionResult Instance => new FinishedActionResult();

        public bool IsFinished => true;
        public void Apply(Hero hero) { }
    }

    class UnFinishedActionResult : IActionResult
    {
        public static UnFinishedActionResult Instance => new UnFinishedActionResult();

        private UnFinishedActionResult()
        {
            
        }

        public bool IsFinished => false;
        public void Apply(Hero hero) { }
    }
}
