using System.Runtime.InteropServices;

namespace MedSolutions.Shared.Extensions;

public static class GuidExtensions
{
    private static long _counter = DateTime.UtcNow.Ticks;

    /// <summary>
    /// Generates a sequential GUID suitable for database indexing.
    /// </summary>
    public static Guid NewSequentialGuid()
    {
        Span<byte> guidBytes = stackalloc byte[16];
        Guid.NewGuid().TryWriteBytes(guidBytes);

        // Increment counter atomically
        var incrementedCounter = Interlocked.Increment(ref _counter);
        Span<byte> counterBytes = stackalloc byte[sizeof(long)];
        MemoryMarshal.Write(counterBytes, ref incrementedCounter);

        if (!BitConverter.IsLittleEndian)
        {
            counterBytes.Reverse();
        }

        // Embed counter into last 8 bytes
        guidBytes[8] = counterBytes[1];
        guidBytes[9] = counterBytes[0];
        guidBytes[10] = counterBytes[7];
        guidBytes[11] = counterBytes[6];
        guidBytes[12] = counterBytes[5];
        guidBytes[13] = counterBytes[4];
        guidBytes[14] = counterBytes[3];
        guidBytes[15] = counterBytes[2];

        return new Guid(guidBytes);
    }
}
