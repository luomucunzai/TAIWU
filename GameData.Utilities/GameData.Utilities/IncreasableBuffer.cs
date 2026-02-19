namespace GameData.Utilities;

public class IncreasableBuffer
{
	private byte[] _buffer;

	private readonly int _initialSize;

	public IncreasableBuffer(int initialSize)
	{
		_buffer = new byte[initialSize];
		_initialSize = initialSize;
	}

	public byte[] Get(int minSize)
	{
		if (_buffer.Length >= minSize)
		{
			return _buffer;
		}
		int num = _buffer.Length * 2;
		if ((uint)num > 2147483647u)
		{
			num = int.MaxValue;
		}
		if (num < minSize)
		{
			num = minSize;
		}
		_buffer = new byte[num];
		return _buffer;
	}

	public void Shrink()
	{
		_buffer = new byte[_initialSize];
	}
}
