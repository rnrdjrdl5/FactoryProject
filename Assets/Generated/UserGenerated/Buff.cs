namespace Tables
{
    public partial class Buff : IIconSprite
    {
        public bool IsInfiniteDuration => duration <= 0f;
    }
}
