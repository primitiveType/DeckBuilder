using RandN;
using RandN.Rngs;

namespace Api
{
    public class Random : Component
    {
        public System.Random SystemRandom { get; private set; }
        public ChaCha Rng { get; }

        public Random()
        {
            var tempRandom = new System.Random();
            SystemRandom = tempRandom;
            var seeder = ThreadLocalRng.Instance;
            // Create the seed (Seeds can also be created manually)
            var factory = ChaCha.GetChaCha8Factory();
            var seed = factory.CreateSeed(seeder);

            // Create the RNG from the seed
            Rng = factory.Create(seed);
        }
    }

    // public static class Random
    // {
    //     private static Randomizer Instance { get; set; }
    //
    //     public static void Shuffle<T>(List<T> list)
    //     {
    //         Instance.Rng.ShuffleInPlace(list);
    //     }
    // }
}