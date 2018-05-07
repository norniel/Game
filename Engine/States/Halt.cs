namespace Engine
{
    class Halt : IState
    {
       // private Hero _hero;

        #region IState Members

        //public event StateHandler NextState;

        public void Act()
        {
        }

        public bool ShowActing => false;

        #endregion
    }
}
