using System.Runtime.InteropServices;
/*
 * a bunch of global stuff that everyone needs put into the same place
 * everything in here is jank as hell, do not question
 */
public class Helper
{
    public const int swarmLayer = 8;
    public const int ktpLayer = 9;
    public const int environmentLayer = 11;
    public const int groundLayer = 12;
    public const int ktpAttackLayer = 13;
    public const int obstacleLayer = 14;
    public const int explosionLayer = 15;

    public static bool FiftyFifty { get { return UnityEngine.Random.value > 0.5f; } }
    /*
     * hacky but efficient Fast inverse square root algorithm
     */
    public static float FastSqrt(float z)
    {
        if (z == 0) return 0;
        FloatIntUnion u;
        u.tmp = 0;
        u.f = z;
        u.tmp -= 1 << 23; /* Subtract 2^m. */
        u.tmp >>= 1; /* Divide by 2. */
        u.tmp += 1 << 29; /* Add ((b + 1) / 2) * 2^m. */
        return u.f;
    }

    // C style union what could go wrong?
    [StructLayout(LayoutKind.Explicit)]
    private struct FloatIntUnion
    {
        [FieldOffset(0)]
        public float f;

        [FieldOffset(0)]
        public int tmp;
    }
}