using RandN;
using RandN.Rngs;

namespace Api
{
    public class Random : Component
    {
        public Random()
        {
            System.Random tempRandom = new System.Random();
            SystemRandom = tempRandom;
            ThreadLocalRng seeder = ThreadLocalRng.Instance;
            // Create the seed (Seeds can also be created manually)
            ChaCha.Factory8 factory = ChaCha.GetChaCha8Factory();
            ChaCha.Seed seed = factory.CreateSeed(seeder);

            // Create the RNG from the seed
            Rng = factory.Create(seed);
        }

        public System.Random SystemRandom { get; }
        public ChaCha Rng { get; }
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