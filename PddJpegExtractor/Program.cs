
using System.Drawing;
using Melville.FileSystem.BlockFile.ByteSinks;
using PddJpegExtractor;

Console.WriteLine(args[0]);

using var handle = new FileByteSink(args[0]);

const int blockSize = 4096;
const int blockDiskSize = blockSize + 4;
int index = 0;

Span<byte> buffer = stackalloc byte[2];
for (uint i = 0; BlockOffset(i) is var offset && offset < handle.Length ; i++)
{
    handle.Read(buffer, offset);
    if (buffer[0] != 0xFF || buffer[1] != 0xD8 ) continue;
    Console.WriteLine($"Block {i} : {buffer[0]:X2}{buffer[1]:X2}");
    using var source = new BlockRecoveryStream(handle, i);
    using var dest = File.Create($"{args[0]}.img{index++:D4}.jpg");
    new JpegStreamReader(new DivertingStream(source, dest)).Read();
}

static long BlockOffset(uint block) => 16 + (block * blockDiskSize);