namespace Matchmaker;

using System.Security.Cryptography;

public static class Id
{
  private const long Min = 0x00038D7EA4C68000; // 10^15
  private const long Max = 0x001FFFFFFFFFFFFF; // 2^53 - 1

  private const long Range = Max - Min + 1;

  public static long Create()
  {
    Span<byte> bytes = stackalloc byte[8];

    RandomNumberGenerator.Fill(bytes);

    long random = BitConverter.ToInt64(bytes) & long.MaxValue;

    return Min + (random % Range);
  }

  public static bool IsValid(long input)
  {
    return input is >= Min and <= Max;
  }
}
