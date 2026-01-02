namespace PddJpegExtractor;

public readonly struct JpegStreamReader(Stream input)
{
    private readonly byte[] buffer = new byte[4096];
    public void Read()
    {
        ReadIntrodctorySegments();
        Console.WriteLine("Start of Scan reached");
        ScanForImageEnd();
    }

    private void ScanForImageEnd()
    {
        bool prior = false;
        for (int i = 0; i < 1024*1024*10;i++)
        {
            if (i%(1024*1024) == 0)
                Console.Write($".");
            int current = input.ReadByte();
            if (prior && current == 0xD9)
            {
                Console.WriteLine("End of Image reached");
                return;
            }
            prior = current == 0xFF;
        }
    }

    private void ReadIntrodctorySegments()
    {
        while (true)
        {
            input.ReadExactly(buffer.AsSpan(0, 2));
            if (buffer[0] != 0xFF)
                throw new InvalidOperationException("segment does not start with FF");
            if (buffer[1] == 0xD8) continue; // SOI
            if (buffer[1] == 0xDA) return; // SOS
            input.ReadExactly(buffer.AsSpan(2, 2));
            var length = (buffer[2] << 8) | buffer[3];
            Console.WriteLine($"Segment marker: FF{buffer[1]:X2} Length {length}");
            JumpAhead(length - 2);
        }
    }

    private void JumpAhead(int value)
    {
        while (value > 0)
        {
            var toRead = Math.Min(value, buffer.Length);
            var read = input.Read(buffer.AsSpan(0, toRead));
            if (read == 0)
                throw new EndOfStreamException("Unexpected end of JPEG stream");
            value -= read;
        }
    }
}
