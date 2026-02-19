using System.Collections.Generic;
using Config;
using Config.Common;
using GameData.Serializer;

namespace GameData.Domains.Building;

[SerializableGameData(NotForArchive = true)]
public class BuildingFormulaContextBridge : ISerializableGameData, IFormulaContextBridge<EBuildingFormulaArgType>
{
	public delegate int CalcArgument(BuildingBlockKey blockKey, EBuildingFormulaArgType argType);

	private BuildingBlockKey _blockKey;

	private Dictionary<EBuildingFormulaArgType, int> _argValues;

	private CalcArgument _calcArg;

	public BuildingBlockKey BlockKey => _blockKey;

	public void Initialize(BuildingBlockKey blockKey, BuildingBlockItem configData, CalcArgument calcArgHandler, bool cacheAllArgs = false)
	{
		if (_argValues == null)
		{
			_argValues = new Dictionary<EBuildingFormulaArgType, int>();
		}
		_argValues.Clear();
		_blockKey = blockKey;
		_calcArg = calcArgHandler;
		if (!cacheAllArgs)
		{
			return;
		}
		List<short> expandInfos = configData.ExpandInfos;
		if (expandInfos == null || expandInfos.Count <= 0)
		{
			return;
		}
		foreach (short expandInfo in configData.ExpandInfos)
		{
			BuildingScaleItem buildingScaleItem = BuildingScale.Instance[expandInfo];
			if (buildingScaleItem.Formula < 0)
			{
				continue;
			}
			BuildingFormulaItem buildingFormulaItem = BuildingFormula.Instance[buildingScaleItem.Formula];
			EBuildingFormulaArgType[] arguments = buildingFormulaItem.Arguments;
			if (arguments != null && arguments.Length > 0)
			{
				arguments = buildingFormulaItem.Arguments;
				foreach (EBuildingFormulaArgType argType in arguments)
				{
					GetArgument(argType);
				}
			}
		}
	}

	public int GetArgument(EBuildingFormulaArgType argType)
	{
		if (_argValues.TryGetValue(argType, out var value))
		{
			return value;
		}
		value = _calcArg?.Invoke(_blockKey, argType) ?? 0;
		_argValues.Add(argType, value);
		return value;
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 8 + DictionaryOfBasicTypePair.GetSerializedSize<EBuildingFormulaArgType, int>((IReadOnlyDictionary<EBuildingFormulaArgType, int>)_argValues);
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		*(ulong*)pData = (ulong)_blockKey;
		byte* num = pData + 8;
		int num2 = (int)(num + DictionaryOfBasicTypePair.Serialize<EBuildingFormulaArgType, int>(num, ref _argValues) - pData);
		if (num2 > 4)
		{
			return (num2 + 3) / 4 * 4;
		}
		return num2;
	}

	public unsafe int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		_blockKey = (BuildingBlockKey)(*(ulong*)ptr);
		ptr += 8;
		ptr += DictionaryOfBasicTypePair.Deserialize<EBuildingFormulaArgType, int>(ptr, ref _argValues);
		int num = (int)(ptr - pData);
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}
}
