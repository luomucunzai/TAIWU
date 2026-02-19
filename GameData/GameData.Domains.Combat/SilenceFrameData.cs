using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Combat;

[SerializableGameData(NotForArchive = true)]
public struct SilenceFrameData : ISerializableGameData
{
	[SerializableGameDataField]
	private int _totalFrame;

	[SerializableGameDataField]
	private int _leftFrame;

	public int TotalFrame => _totalFrame;

	public int LeftFrame => _leftFrame;

	public float LeftProgress => Infinity ? 1f : (NotInSilencing ? 0f : ((float)LeftFrame / (float)MathUtils.Max(TotalFrame, 1)));

	public bool Silencing => LeftFrame != 0;

	public bool NotInSilencing => LeftFrame == 0;

	public bool Infinity => LeftFrame < 0;

	public static SilenceFrameData Create(int total)
	{
		return new SilenceFrameData
		{
			_totalFrame = total,
			_leftFrame = total
		};
	}

	public static SilenceFrameData Create(int total, int left)
	{
		return new SilenceFrameData
		{
			_totalFrame = total,
			_leftFrame = left
		};
	}

	public bool Cover(int newFrame)
	{
		if (Infinity)
		{
			return false;
		}
		if (NotInSilencing || newFrame < 0 || newFrame >= TotalFrame)
		{
			_totalFrame = (_leftFrame = newFrame);
			return true;
		}
		if (newFrame > LeftFrame)
		{
			_leftFrame = newFrame;
			return true;
		}
		return false;
	}

	public bool Tick(int deltaFrame = 1)
	{
		if (Infinity || NotInSilencing)
		{
			return false;
		}
		_leftFrame -= deltaFrame;
		return true;
	}

	public bool IsSerializedSizeFixed()
	{
		return true;
	}

	public int GetSerializedSize()
	{
		int num = 8;
		return (num <= 4) ? num : ((num + 3) / 4 * 4);
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		*(int*)ptr = _totalFrame;
		ptr += 4;
		*(int*)ptr = _leftFrame;
		ptr += 4;
		int num = (int)(ptr - pData);
		return (num <= 4) ? num : ((num + 3) / 4 * 4);
	}

	public unsafe int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		_totalFrame = *(int*)ptr;
		ptr += 4;
		_leftFrame = *(int*)ptr;
		ptr += 4;
		int num = (int)(ptr - pData);
		return (num <= 4) ? num : ((num + 3) / 4 * 4);
	}
}
