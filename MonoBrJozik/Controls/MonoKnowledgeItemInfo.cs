namespace MonoBrJozik.Controls
{
    internal class MonoKnowledgeItemInfo:MonoItemInfoBase
    {
        public uint Start { get; set; }
        public uint Current { get; set; }
        public uint End { get; set; }

        public MonoKnowledgeItemInfo(string text, uint start, uint end, uint current) : base(text)
        {
            Start = start;
            End = end;
            Current = current;
        }
    }
}
