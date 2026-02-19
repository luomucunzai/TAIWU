using System;
using System.Collections.Generic;
using Config;
using GameData.Domains.Character.AvatarSystem.AvatarRes;
using GameData.Utilities;
using Redzen.Random;

namespace GameData.Domains.Character.AvatarSystem;

public class AvatarManager
{
	public static AvatarManager Instance;

	private List<AvatarGroup> _avatarGroupList;

	public List<byte[]> SkinColorsWeight;

	public List<byte[]> FeatureColorsWeight;

	public List<byte[]> LipColorsWeight;

	public List<byte[]> ClothColorsWeight;

	public List<byte[]> EyeballColorsWeight;

	public List<byte[]> HairColorsWeight;

	private bool _avatarReady;

	public bool DisplayMode { get; private set; }

	public bool AvatarSystemReady => _avatarReady;

	public AvatarGroup GetAvatarGroup(int avatarId)
	{
		return _avatarGroupList.Find((AvatarGroup e) => e.Id == avatarId);
	}

	public List<AvatarGroup> GetAvatarGroupList(Predicate<AvatarGroup> predicate)
	{
		return _avatarGroupList.FindAll(predicate);
	}

	public int GetCanUseAvatarGroupCount()
	{
		int count = 0;
		_avatarGroupList.ForEach(delegate(AvatarGroup e)
		{
			if (e.HasAsset)
			{
				count++;
			}
		});
		return count;
	}

	public int GetMaxAvatarId()
	{
		return _avatarGroupList[_avatarGroupList.Count - 1].Id;
	}

	public AvatarAsset GetAsset(int avatarId, EAvatarElementsType elemType, params short[] elemIds)
	{
		AvatarGroup avatarGroup = _avatarGroupList.Find((AvatarGroup e) => e.Id == avatarId);
		if (avatarGroup == null)
		{
			throw new Exception($"Avatar {avatarId} is not found!");
		}
		AvatarAsset avatarAsset = avatarGroup.Get(elemType, elemIds);
		if (elemType != EAvatarElementsType.Hair1Part && elemType != EAvatarElementsType.Hair2Part && elemType != EAvatarElementsType.MouthPart && elemType != EAvatarElementsType.EyeBall && elemType != EAvatarElementsType.Beard1 && elemType != EAvatarElementsType.Beard2 && (elemIds[0] != 0 || elemType != EAvatarElementsType.ClothColor) && avatarAsset == null)
		{
			AdaptableLog.TagError("AvatarManager", string.Format("Avatar {0} failed to get asset {1}:Id = {2} {3}", avatarId, elemType, elemIds[0], (elemIds.Length > 1) ? ("SubId = " + elemIds[1]) : ""));
		}
		return avatarAsset;
	}

	public bool HasAsset(int avatarId, EAvatarElementsType elemType, params short[] elemIds)
	{
		AvatarGroup avatarGroup = _avatarGroupList.Find((AvatarGroup e) => e.Id == avatarId);
		if (avatarGroup == null)
		{
			throw new Exception($"Avatar {avatarId} is not found!");
		}
		AvatarAsset avatarAsset = avatarGroup.Get(elemType, elemIds);
		return avatarAsset != null;
	}

	public sbyte GetAvatarIdByBodyTypeAndGender(sbyte bodyType, sbyte gender)
	{
		sbyte b = 0;
		switch (bodyType)
		{
		case 0:
			b = 1;
			break;
		case 1:
			b = 3;
			break;
		case 2:
			b = 5;
			break;
		}
		if (gender == 0)
		{
			b++;
		}
		return b;
	}

	public AvatarData GetRandomAvatar(IRandomSource random, sbyte gender = -1, bool transgender = false, sbyte bodyType = -1, AvatarData father = null, AvatarData mother = null)
	{
		if (gender == -1)
		{
			gender = Gender.GetRandom(random);
		}
		if (father == null && mother == null)
		{
			return GetTotalRandomAvatar(random, gender, transgender, bodyType);
		}
		return new AvatarInherit(father, mother, gender, bodyType, random).GetInheritAvatar();
	}

	public AvatarData GetRandomAvatar(IRandomSource random, sbyte gender, bool transgender, sbyte bodyType, short baseAttraction)
	{
		AvatarData totalRandomAvatar = GetTotalRandomAvatar(random, gender, transgender, bodyType);
		totalRandomAvatar.AdjustToBaseCharm(random, baseAttraction);
		return totalRandomAvatar;
	}

	public byte GetChildAvatarIdByAvatarId(byte avatarId)
	{
		byte index = 6;
		if (avatarId % 6 <= 0 || avatarId % 6 >= 5)
		{
			index = (byte)((avatarId % 2 != 0) ? 8 : 9);
		}
		else if (avatarId % 2 == 0)
		{
			index = 7;
		}
		return AvatarHead.Instance[index].AvatarId;
	}

	public short GetRandomChildClothIdByAvatarId(IRandomSource random, byte avatarId)
	{
		byte childAvatarIdByAvatarId = GetChildAvatarIdByAvatarId(avatarId);
		return GetAvatarGroup(childAvatarIdByAvatarId)?.GetRandomCloth(random, canCreateOnly: false) ?? 255;
	}

	private AvatarData GetTotalRandomAvatar(IRandomSource random, sbyte gender, bool transgender, sbyte bodyType)
	{
		if (transgender)
		{
			gender = Gender.Flip(gender);
		}
		sbyte preferredId = -1;
		if (bodyType != -1)
		{
			preferredId = GetAvatarIdByBodyTypeAndGender(bodyType, gender);
		}
		List<AvatarGroup> list = new List<AvatarGroup>();
		list.AddRange(_avatarGroupList.FindAll(FindFunc));
		AvatarData avatarData = null;
		AvatarGroup random2 = list.GetRandom(random);
		if (DisplayMode)
		{
			(avatarData, avatarData.ClothDisplayId) = random2.GetRandomAvatar(random, randColor: true, randCloth: true);
		}
		else
		{
			avatarData = random2.GetRandomAvatar(random).avatar;
		}
		return avatarData;
		bool FindFunc(AvatarGroup e)
		{
			if (e.BodyRes.Count <= 0)
			{
				return false;
			}
			if (-1 != preferredId)
			{
				return e.Id == preferredId;
			}
			if (e.Id >= AvatarHead.Instance[(byte)6].AvatarId)
			{
				return false;
			}
			if (gender == 1)
			{
				return e.Id % 2 == 1;
			}
			if (gender == 0)
			{
				return e.Id % 2 == 0;
			}
			return false;
		}
	}

	public void AvatarDataClamp(AvatarData avatarData)
	{
		IRandomSource random = ExternalDataBridge.Context.Random;
		AvatarGroup avatarGroup = GetAvatarGroup(avatarData.AvatarId);
		if (avatarGroup == null)
		{
			avatarGroup = _avatarGroupList.GetRandom(random);
		}
		avatarData.AvatarId = avatarGroup.Id;
		if (!HasAsset(avatarData.AvatarId, EAvatarElementsType.Head, avatarData.HeadId))
		{
			avatarData.HeadId = (byte)avatarGroup.GetRandomHead(random);
		}
		if (!HasAsset(avatarData.AvatarId, EAvatarElementsType.Cloth, avatarData.ClothDisplayId))
		{
			avatarData.ClothPartId = 0;
			avatarData.ClothDisplayId = avatarGroup.GetRandomCloth(random, canCreateOnly: true);
		}
		if (!HasAsset(avatarData.AvatarId, EAvatarElementsType.Eye, avatarData.EyesMainId, avatarData.EyesLeftId))
		{
			(avatarData.EyesMainId, avatarData.EyesLeftId, avatarData.EyesRightId, avatarData.EyebrowId) = avatarGroup.GetRandomEyes(random);
		}
		if (!HasAsset(avatarData.AvatarId, EAvatarElementsType.Nose, avatarData.NoseId))
		{
			avatarData.NoseId = avatarGroup.GetRandomNose(random);
		}
		if (!HasAsset(avatarData.AvatarId, EAvatarElementsType.Mouth, avatarData.MouthId))
		{
			avatarData.MouthId = avatarGroup.GetRandomMouth(random);
		}
		if (!HasAsset(avatarData.AvatarId, EAvatarElementsType.Beard1, avatarData.Beard1Id))
		{
			avatarData.Beard1Id = avatarGroup.GetRandomBeards(random).id1;
		}
		if (!HasAsset(avatarData.AvatarId, EAvatarElementsType.Beard2, avatarData.Beard2Id))
		{
			avatarData.Beard2Id = avatarGroup.GetRandomBeards(random).id2;
		}
		if (!HasAsset(avatarData.AvatarId, EAvatarElementsType.Hair1, avatarData.FrontHairId))
		{
			avatarData.FrontHairId = avatarGroup.GetRandomHairs(random).frontId;
		}
		if (!HasAsset(avatarData.AvatarId, EAvatarElementsType.Hair2, avatarData.BackHairId))
		{
			avatarData.BackHairId = avatarGroup.GetRandomHairs(random).backId;
		}
		var (feature1Id, feature2Id, wrinkle1Id, wrinkle2Id, wrinkle3Id) = avatarGroup.GetRandomMaskElems(random);
		if (!HasAsset(avatarData.AvatarId, EAvatarElementsType.Feature1, avatarData.Feature1Id))
		{
			avatarData.Feature1Id = feature1Id;
		}
		if (!HasAsset(avatarData.AvatarId, EAvatarElementsType.Feature2, avatarData.Feature2Id))
		{
			avatarData.Feature2Id = feature2Id;
		}
		if (!HasAsset(avatarData.AvatarId, EAvatarElementsType.Wrinkle1, avatarData.Wrinkle1Id))
		{
			avatarData.Wrinkle1Id = wrinkle1Id;
		}
		if (!HasAsset(avatarData.AvatarId, EAvatarElementsType.Wrinkle2, avatarData.Wrinkle2Id))
		{
			avatarData.Wrinkle2Id = wrinkle2Id;
		}
		if (!HasAsset(avatarData.AvatarId, EAvatarElementsType.Wrinkle3, avatarData.Wrinkle3Id))
		{
			avatarData.Wrinkle3Id = wrinkle3Id;
		}
	}

	public int GetColorWeightIndex()
	{
		return 1;
	}

	public void InitAvatarCore(bool displayMode, Action loadExternalAvatars = null, bool forceInit = false)
	{
		if (_avatarReady && !forceInit)
		{
			return;
		}
		DisplayMode = displayMode;
		SkinColorsWeight = new List<byte[]>();
		AvatarSkinColors.Instance.Iterate(delegate(AvatarSkinColorsItem colorItem)
		{
			SkinColorsWeight.Add(new byte[5] { colorItem.TemplateId, colorItem.ObbCn, colorItem.ObbChn, colorItem.ObbJp, colorItem.ObbEn });
			return true;
		});
		FeatureColorsWeight = new List<byte[]>();
		AvatarFeatureColors.Instance.Iterate(delegate(AvatarFeatureColorsItem colorItem)
		{
			FeatureColorsWeight.Add(new byte[5] { colorItem.TemplateId, colorItem.ObbCn, colorItem.ObbChn, colorItem.ObbJp, colorItem.ObbEn });
			return true;
		});
		LipColorsWeight = new List<byte[]>();
		AvatarLipColors.Instance.Iterate(delegate(AvatarLipColorsItem colorItem)
		{
			LipColorsWeight.Add(new byte[5] { colorItem.TemplateId, colorItem.ObbCn, colorItem.ObbChn, colorItem.ObbJp, colorItem.ObbEn });
			return true;
		});
		ClothColorsWeight = new List<byte[]>();
		AvatarClothColors.Instance.Iterate(delegate(AvatarClothColorsItem colorItem)
		{
			ClothColorsWeight.Add(new byte[5] { colorItem.TemplateId, colorItem.ObbCn, colorItem.ObbChn, colorItem.ObbJp, colorItem.ObbEn });
			return true;
		});
		EyeballColorsWeight = new List<byte[]>();
		AvatarEyeballColors.Instance.Iterate(delegate(AvatarEyeballColorsItem colorItem)
		{
			EyeballColorsWeight.Add(new byte[5] { colorItem.TemplateId, colorItem.ObbCn, colorItem.ObbChn, colorItem.ObbJp, colorItem.ObbEn });
			return true;
		});
		HairColorsWeight = new List<byte[]>();
		AvatarHairColors.Instance.Iterate(delegate(AvatarHairColorsItem colorItem)
		{
			HairColorsWeight.Add(new byte[5] { colorItem.TemplateId, colorItem.ObbCn, colorItem.ObbChn, colorItem.ObbJp, colorItem.ObbEn });
			return true;
		});
		_avatarGroupList = new List<AvatarGroup>();
		List<byte> allKeys = AvatarHead.Instance.GetAllKeys();
		for (int num = 0; num < allKeys.Count; num++)
		{
			AvatarHeadItem avatarHeadItem = AvatarHead.Instance[allKeys[num]];
			if (avatarHeadItem != null)
			{
				AvatarAsset avatarAsset = new AvatarAsset(avatarHeadItem);
				AvatarGroup avatarGroup = GetAvatarGroup(avatarAsset.AvatarId);
				if (avatarGroup == null)
				{
					avatarGroup = new AvatarGroup();
					avatarGroup.Id = avatarAsset.AvatarId;
					_avatarGroupList.Add(avatarGroup);
				}
				avatarGroup.Add(avatarAsset);
			}
		}
		List<uint> allKeys2 = AvatarElements.Instance.GetAllKeys();
		for (int num2 = 0; num2 < allKeys2.Count; num2++)
		{
			AvatarAsset avatarAsset2 = new AvatarAsset(AvatarElements.Instance[allKeys2[num2]]);
			AvatarGroup avatarGroup2 = GetAvatarGroup(avatarAsset2.AvatarId);
			if (avatarGroup2 == null)
			{
				avatarGroup2 = new AvatarGroup();
				avatarGroup2.Id = avatarAsset2.AvatarId;
				_avatarGroupList.Add(avatarGroup2);
			}
			avatarGroup2.Add(avatarAsset2);
		}
		loadExternalAvatars?.Invoke();
		byte childAvatarIdByAvatarId = GetChildAvatarIdByAvatarId(1);
		List<AvatarGroup> list = new List<AvatarGroup>();
		_avatarGroupList.Sort((AvatarGroup left, AvatarGroup right) => left.Id - right.Id);
		for (int num3 = 0; num3 < _avatarGroupList.Count; num3++)
		{
			AvatarGroup avatarGroup3 = _avatarGroupList[num3];
			avatarGroup3.Sort();
			avatarGroup3.ConstructEyesGroup();
			if (avatarGroup3.Id > 6 && avatarGroup3.Id < childAvatarIdByAvatarId)
			{
				string text = ModUsabilityCheck(avatarGroup3);
				if (!string.IsNullOrEmpty(text))
				{
					list.Add(avatarGroup3);
					AdaptableLog.TagError("AvatarManager", text);
				}
			}
		}
		for (int num4 = 0; num4 < list.Count; num4++)
		{
			_avatarGroupList.Remove(list[num4]);
		}
		_avatarReady = true;
	}

	private string ModUsabilityCheck(AvatarGroup checkGroup)
	{
		return string.Empty;
	}
}
