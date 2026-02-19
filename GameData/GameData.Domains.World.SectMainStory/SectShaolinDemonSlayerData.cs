using System.Collections.Generic;
using Config;
using GameData.Domains.SpecialEffect;
using GameData.Serializer;
using GameData.Utilities;
using Redzen.Random;

namespace GameData.Domains.World.SectMainStory;

[SerializableGameData(IsExtensible = true)]
public class SectShaolinDemonSlayerData : ISerializableGameData
{
	public const int DemonPerLevel = 2;

	public const int MaxRestrictCount = 3;

	private static readonly List<int> RestrictCacheIndexes = new List<int>();

	private static readonly List<short> RestrictCacheWeights = new List<short>();

	private static readonly HashSet<int> RestrictCacheGroups = new HashSet<int>();

	public List<SpecialEffectBase> TrialingRestrictEffects;

	[SerializableGameDataField(FieldIndex = 0)]
	private uint _demonFlags0;

	[SerializableGameDataField(FieldIndex = 1)]
	private List<int> _trialingDemons;

	[SerializableGameDataField(FieldIndex = 2)]
	private int _trialingLevel;

	[SerializableGameDataField(FieldIndex = 3)]
	private List<IntList> _trailingRestricts;

	[SerializableGameDataField(FieldIndex = 4)]
	private List<int> _trailingRegenerateRestrictCount;

	public bool Trialing
	{
		get
		{
			List<int> trialingDemons = _trialingDemons;
			return trialingDemons != null && trialingDemons.Count > 0 && _trialingDemons.Count - _trialingLevel * 2 >= 2;
		}
	}

	public DemonSlayerTrialLevelItem TrialingLevel
	{
		get
		{
			if (!Trialing)
			{
				return null;
			}
			int index = MathUtils.Clamp(_trialingLevel, 0, DemonSlayerTrialLevel.Instance.Count - 1);
			return DemonSlayerTrialLevel.Instance[index];
		}
	}

	public static IntList GenerateRestricts(IRandomSource random, int demonId, int totalPower)
	{
		IntList result = IntList.Create();
		RestrictCacheGroups.Clear();
		int num = totalPower / 3 + ((totalPower % 3 != 0) ? 1 : 0);
		for (int i = 0; i < 3; i++)
		{
			if (totalPower < num)
			{
				num = totalPower;
				totalPower = num + 2;
			}
			RestrictCacheIndexes.Clear();
			RestrictCacheWeights.Clear();
			foreach (DemonSlayerTrialRestrictItem item in (IEnumerable<DemonSlayerTrialRestrictItem>)DemonSlayerTrialRestrict.Instance)
			{
				if (!item.MutexDemonId.Contains(demonId) && !RestrictCacheGroups.Contains(item.MutexGroupId) && item.Power <= totalPower && item.Power >= num && item.Weight > 0)
				{
					RestrictCacheIndexes.Add(item.TemplateId);
					RestrictCacheWeights.Add(item.Weight);
				}
			}
			if (RestrictCacheIndexes.Count != 0)
			{
				int randomIndex = RandomUtils.GetRandomIndex(RestrictCacheWeights, random);
				int num2 = RestrictCacheIndexes[randomIndex];
				result.Items.Add(num2);
				DemonSlayerTrialRestrictItem demonSlayerTrialRestrictItem = DemonSlayerTrialRestrict.Instance[num2];
				RestrictCacheGroups.Add(demonSlayerTrialRestrictItem.MutexGroupId);
				totalPower -= demonSlayerTrialRestrictItem.Power;
				if (totalPower <= 0)
				{
					break;
				}
			}
		}
		return result;
	}

	public int GetRegenerateRestrictCount()
	{
		return Trialing ? _trailingRegenerateRestrictCount[_trialingLevel] : 0;
	}

	public DemonSlayerTrialItem GetTrialingDemon(int index)
	{
		bool flag = ((index < 0 || index >= 2) ? true : false);
		if (flag || !Trialing)
		{
			return null;
		}
		int num = _trialingLevel * 2;
		int index2 = _trialingDemons[num + index];
		return DemonSlayerTrial.Instance[index2];
	}

	public IEnumerable<DemonSlayerTrialRestrictItem> GetTrialingRestricts(int index)
	{
		bool flag = ((index < 0 || index >= 2) ? true : false);
		if (flag || !Trialing)
		{
			yield break;
		}
		int baseIndex = _trialingLevel * 2;
		IntList restricts = _trailingRestricts[baseIndex + index];
		List<int> items = restricts.Items;
		if (items == null || items.Count <= 0)
		{
			yield break;
		}
		foreach (int restrictId in restricts.Items)
		{
			yield return DemonSlayerTrialRestrict.Instance[restrictId];
		}
	}

	public bool IsDemonDefeated(int templateId)
	{
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		if (templateId < 0 || templateId >= DemonSlayerTrial.Instance.Count)
		{
			return false;
		}
		Tester.Assert(templateId < 32, "templateId < 32");
		BoolArray32 val = BoolArray32.op_Implicit(_demonFlags0);
		return ((BoolArray32)(ref val))[templateId];
	}

	public bool GenerateDemons(IRandomSource random)
	{
		if (Trialing)
		{
			return false;
		}
		if (_trialingDemons == null)
		{
			_trialingDemons = new List<int>();
		}
		_trialingDemons.Clear();
		foreach (DemonSlayerTrialItem item in (IEnumerable<DemonSlayerTrialItem>)DemonSlayerTrial.Instance)
		{
			_trialingDemons.Add(item.TemplateId);
		}
		CollectionUtils.Shuffle(random, _trialingDemons);
		if (_trailingRestricts == null)
		{
			_trailingRestricts = new List<IntList>();
		}
		_trailingRestricts.Clear();
		for (int i = 0; i < _trialingDemons.Count; i++)
		{
			int demonId = _trialingDemons[i];
			int index = i / 2;
			DemonSlayerTrialLevelItem demonSlayerTrialLevelItem = DemonSlayerTrialLevel.Instance[index];
			_trailingRestricts.Add(GenerateRestricts(random, demonId, demonSlayerTrialLevelItem.TotalPower));
		}
		if (_trailingRegenerateRestrictCount == null)
		{
			_trailingRegenerateRestrictCount = new List<int>();
		}
		_trailingRegenerateRestrictCount.Clear();
		foreach (DemonSlayerTrialLevelItem item2 in (IEnumerable<DemonSlayerTrialLevelItem>)DemonSlayerTrialLevel.Instance)
		{
			_trailingRegenerateRestrictCount.Add(item2.RestrictRandomCount);
		}
		_trialingLevel = 0;
		return true;
	}

	public bool ClearDemons()
	{
		List<int> trialingDemons = _trialingDemons;
		if (trialingDemons == null || trialingDemons.Count <= 0)
		{
			List<IntList> trailingRestricts = _trailingRestricts;
			if (trailingRestricts == null || trailingRestricts.Count <= 0)
			{
				trialingDemons = _trailingRegenerateRestrictCount;
				if ((trialingDemons == null || trialingDemons.Count <= 0) && _trialingLevel == 0)
				{
					return false;
				}
			}
		}
		_trialingDemons?.Clear();
		_trailingRestricts?.Clear();
		_trailingRegenerateRestrictCount?.Clear();
		_trialingLevel = 0;
		return true;
	}

	public bool ReGenerateRestricts(IRandomSource random)
	{
		if (GetRegenerateRestrictCount() <= 0)
		{
			return false;
		}
		_trailingRegenerateRestrictCount[_trialingLevel]--;
		int num = _trialingLevel * 2;
		for (int i = 0; i < 2; i++)
		{
			int demonId = _trialingDemons[num + i];
			_trailingRestricts[num + i] = GenerateRestricts(random, demonId, TrialingLevel.TotalPower);
		}
		return true;
	}

	public bool ToNextLevel()
	{
		if (!Trialing)
		{
			return false;
		}
		_trialingLevel++;
		if (!Trialing)
		{
			ClearDemons();
		}
		return true;
	}

	public bool MarkDemonAsDefeated(int templateId)
	{
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		if (templateId < 0 || templateId >= DemonSlayerTrial.Instance.Count)
		{
			return false;
		}
		Tester.Assert(templateId < 32, "templateId < 32");
		BoolArray32 val = BoolArray32.op_Implicit(_demonFlags0);
		((BoolArray32)(ref val))[templateId] = true;
		_demonFlags0 = BoolArray32.op_Implicit(val);
		return true;
	}

	public SectShaolinDemonSlayerData()
	{
	}

	public SectShaolinDemonSlayerData(SectShaolinDemonSlayerData other)
	{
		_demonFlags0 = other._demonFlags0;
		_trialingDemons = ((other._trialingDemons == null) ? null : new List<int>(other._trialingDemons));
		_trialingLevel = other._trialingLevel;
		if (other._trailingRestricts != null)
		{
			List<IntList> trailingRestricts = other._trailingRestricts;
			int count = trailingRestricts.Count;
			_trailingRestricts = new List<IntList>(count);
			for (int i = 0; i < count; i++)
			{
				_trailingRestricts.Add(new IntList(trailingRestricts[i]));
			}
		}
		else
		{
			_trailingRestricts = null;
		}
		_trailingRegenerateRestrictCount = ((other._trailingRegenerateRestrictCount == null) ? null : new List<int>(other._trailingRegenerateRestrictCount));
	}

	public void Assign(SectShaolinDemonSlayerData other)
	{
		_demonFlags0 = other._demonFlags0;
		_trialingDemons = ((other._trialingDemons == null) ? null : new List<int>(other._trialingDemons));
		_trialingLevel = other._trialingLevel;
		if (other._trailingRestricts != null)
		{
			List<IntList> trailingRestricts = other._trailingRestricts;
			int count = trailingRestricts.Count;
			_trailingRestricts = new List<IntList>(count);
			for (int i = 0; i < count; i++)
			{
				_trailingRestricts.Add(new IntList(trailingRestricts[i]));
			}
		}
		else
		{
			_trailingRestricts = null;
		}
		_trailingRegenerateRestrictCount = ((other._trailingRegenerateRestrictCount == null) ? null : new List<int>(other._trailingRegenerateRestrictCount));
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 8;
		num = ((_trialingDemons == null) ? (num + 2) : (num + (2 + 4 * _trialingDemons.Count)));
		if (_trailingRestricts != null)
		{
			num += 2;
			int count = _trailingRestricts.Count;
			for (int i = 0; i < count; i++)
			{
				num += _trailingRestricts[i].GetSerializedSize();
			}
		}
		else
		{
			num += 2;
		}
		num = ((_trailingRegenerateRestrictCount == null) ? (num + 2) : (num + (2 + 4 * _trailingRegenerateRestrictCount.Count)));
		return (num <= 4) ? num : ((num + 3) / 4 * 4);
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		*(uint*)ptr = _demonFlags0;
		ptr += 4;
		if (_trialingDemons != null)
		{
			int count = _trialingDemons.Count;
			Tester.Assert(count <= 65535);
			*(ushort*)ptr = (ushort)count;
			ptr += 2;
			for (int i = 0; i < count; i++)
			{
				((int*)ptr)[i] = _trialingDemons[i];
			}
			ptr += 4 * count;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		*(int*)ptr = _trialingLevel;
		ptr += 4;
		if (_trailingRestricts != null)
		{
			int count2 = _trailingRestricts.Count;
			Tester.Assert(count2 <= 65535);
			*(ushort*)ptr = (ushort)count2;
			ptr += 2;
			for (int j = 0; j < count2; j++)
			{
				int num = _trailingRestricts[j].Serialize(ptr);
				ptr += num;
				Tester.Assert(num <= 65535);
			}
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		if (_trailingRegenerateRestrictCount != null)
		{
			int count3 = _trailingRegenerateRestrictCount.Count;
			Tester.Assert(count3 <= 65535);
			*(ushort*)ptr = (ushort)count3;
			ptr += 2;
			for (int k = 0; k < count3; k++)
			{
				((int*)ptr)[k] = _trailingRegenerateRestrictCount[k];
			}
			ptr += 4 * count3;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		int num2 = (int)(ptr - pData);
		return (num2 <= 4) ? num2 : ((num2 + 3) / 4 * 4);
	}

	public unsafe int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		_demonFlags0 = *(uint*)ptr;
		ptr += 4;
		ushort num = *(ushort*)ptr;
		ptr += 2;
		if (num > 0)
		{
			if (_trialingDemons == null)
			{
				_trialingDemons = new List<int>(num);
			}
			else
			{
				_trialingDemons.Clear();
			}
			for (int i = 0; i < num; i++)
			{
				_trialingDemons.Add(((int*)ptr)[i]);
			}
			ptr += 4 * num;
		}
		else
		{
			_trialingDemons?.Clear();
		}
		_trialingLevel = *(int*)ptr;
		ptr += 4;
		ushort num2 = *(ushort*)ptr;
		ptr += 2;
		if (num2 > 0)
		{
			if (_trailingRestricts == null)
			{
				_trailingRestricts = new List<IntList>(num2);
			}
			else
			{
				_trailingRestricts.Clear();
			}
			for (int j = 0; j < num2; j++)
			{
				IntList item = default(IntList);
				ptr += item.Deserialize(ptr);
				_trailingRestricts.Add(item);
			}
		}
		else
		{
			_trailingRestricts?.Clear();
		}
		ushort num3 = *(ushort*)ptr;
		ptr += 2;
		if (num3 > 0)
		{
			if (_trailingRegenerateRestrictCount == null)
			{
				_trailingRegenerateRestrictCount = new List<int>(num3);
			}
			else
			{
				_trailingRegenerateRestrictCount.Clear();
			}
			for (int k = 0; k < num3; k++)
			{
				_trailingRegenerateRestrictCount.Add(((int*)ptr)[k]);
			}
			ptr += 4 * num3;
		}
		else
		{
			_trailingRegenerateRestrictCount?.Clear();
		}
		int num4 = (int)(ptr - pData);
		return (num4 <= 4) ? num4 : ((num4 + 3) / 4 * 4);
	}
}
