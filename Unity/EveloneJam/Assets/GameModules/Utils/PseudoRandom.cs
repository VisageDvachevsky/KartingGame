namespace Project.Utils
{
    public class PseudoRandom
    {
        private readonly float _chance;
        private readonly float _chanceModifier;

        private float _currentChance;

        public PseudoRandom(float chance, float chanceModifier)
        {
            _chance = chance;
            _chanceModifier = chanceModifier;
            _currentChance = chance;
        }

        public bool Decide()
        {
            float decision = UnityEngine.Random.Range(0f, 1f);
            
            if (decision <  _currentChance)
            {
                Reset();
                return true;
            }

            _currentChance += _chanceModifier;
            return false;
        }

        public void Reset()
        {
            _currentChance = _chance;
        }
    }
}
