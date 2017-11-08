namespace Engine
{
    using Heros;
    class Standing : IState
    {
       // private Hero _hero;

        public Standing( /*Hero hero*/ )
        {
            //_hero = hero;
        }

        #region IState Members

        //public event StateHandler NextState;

        public void Act()
        {
        }

        public bool ShowActing => false;

        #endregion
    }
}
