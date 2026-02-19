using System;
using System.Text;

namespace GameData.GameDataBridge.VnPipe;

public class Master : NativeObject, IPipe
{
	public static Master Create(string name)
	{
		byte[] bytes = Encoding.UTF8.GetBytes(name);
		IntPtr intPtr = Bridge.master_create(bytes, bytes.Length);
		if (intPtr == IntPtr.Zero)
		{
			return null;
		}
		return new Master(intPtr);
	}

	public bool Wait()
	{
		if (base.disposed)
		{
			return false;
		}
		return Bridge.master_wait(m_ptr) != 0;
	}

	public unsafe int Read(byte[] buf, int off, int len)
	{
		if (base.disposed)
		{
			throw new Exception("Pipe broken: trying to read data after disposed.");
		}
		if (off < 0 || buf.Length < off + len)
		{
			throw new ArgumentException($"Offset {off} should be in range of [0, {buf.Length}(buffer length) + {len}(data size)]");
		}
		int num;
		fixed (byte* ptr = buf)
		{
			num = Bridge.master_read(m_ptr, ptr + off, len);
		}
		if (num <= 0)
		{
			throw new Exception($"Pipe broken: {num} received.");
		}
		return num;
	}

	public unsafe int Write(byte[] buf, int off, int len)
	{
		if (base.disposed)
		{
			return -1;
		}
		if (off < 0 || buf.Length < off + len)
		{
			throw new ArgumentException($"Offset {off} should be in range of [0, {buf.Length}(buffer length) + {len}(data size)]");
		}
		int num;
		fixed (byte* ptr = buf)
		{
			num = Bridge.master_write(m_ptr, ptr + off, len);
		}
		if (num < 0)
		{
			throw new Exception($"Pipe broken: {num} written.");
		}
		return num;
	}

	public bool Flush()
	{
		if (base.disposed)
		{
			return false;
		}
		return Bridge.master_flush(m_ptr) != 0;
	}

	protected override void will_dispose()
	{
		Bridge.master_release(m_ptr);
	}

	private Master(IntPtr ptr)
		: base(ptr)
	{
	}
}
