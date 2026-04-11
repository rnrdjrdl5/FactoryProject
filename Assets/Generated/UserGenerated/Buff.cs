namespace Tables
{
    public partial class Buff : IIconAtlasSprite
    {
        public bool IsInfiniteDuration => duration <= 0f;
    }
}
