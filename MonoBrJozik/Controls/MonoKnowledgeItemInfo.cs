namespace MonoBrJozik.Controls
{
    internal class MonoKnowledgeItemInfo : MonoItemInfoBase
    {
        public int Start { get; private set; }
        public int Current { get; private set; }
        public int End { get; private set; }
        public bool IsDisabled { get; private set; }

        public MonoKnowledgeItemInfo(string text, int start, int end, int current, bool isDisabled) : base(text)
        {
            Start = start;
            End = end;
            Current = current;
            IsDisabled = isDisabled;
        }
    }
}