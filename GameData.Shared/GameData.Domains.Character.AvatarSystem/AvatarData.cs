using System;
using System.Collections.Generic;
using Config;
using GameData.Domains.Character.AvatarSystem.AvatarRes;
using GameData.Serializer;
using GameData.Utilities;
using Redzen.Random;

namespace GameData.Domains.Character.AvatarSystem;

[Serializable]
public class AvatarData : ISerializableGameData
{
	[SerializableGameDataField]
	public bool ShowVeil;

	[SerializableGameDataField]
	public byte AvatarId;

	[SerializableGameDataField]
	public byte ColorSkinId;

	[SerializableGameDataField]
	public byte ColorClothId;

	[SerializableGameDataField]
	public short ChildClothId;

	public short ClothDisplayId;

	public bool ShowBlush;

	public bool ShowJieqingMask;

	[SerializableGameDataField]
	public byte ClothPartId;

	[SerializableGameDataField]
	public byte HeadId = 1;

	[SerializableGameDataField]
	public short EyesMainId;

	[SerializableGameDataField]
	public short EyesLeftId;

	[SerializableGameDataField]
	public short EyesRightId;

	[SerializableGameDataField]
	public short EyebrowId;

	[SerializableGameDataField]
	public byte ColorEyeballId;

	[SerializableGameDataField]
	public byte ColorEyebrowId;

	[SerializableGameDataField]
	public short EyesHeightPercent;

	[SerializableGameDataField]
	public short EyesDistancePercent;

	[SerializableGameDataField]
	public short EyesAngle;

	[SerializableGameDataField]
	public short EyesScale;

	[SerializableGameDataField]
	public short EyebrowHeight;

	[SerializableGameDataField]
	public short EyebrowDistancePercent;

	[SerializableGameDataField]
	public short EyebrowAngle;

	[SerializableGameDataField]
	public short EyebrowScale;

	[SerializableGameDataField]
	public short NoseId;

	[SerializableGameDataField]
	public short NoseHeightPercent;

	[SerializableGameDataField]
	public short NoseScale;

	[SerializableGameDataField]
	public short MouthId;

	[SerializableGameDataField]
	public short MouthHeightPercent;

	[SerializableGameDataField]
	public short MouthScale;

	[SerializableGameDataField]
	public byte ColorMouthId;

	[SerializableGameDataField]
	public short Beard1Id;

	[SerializableGameDataField]
	public short Beard2Id;

	[SerializableGameDataField]
	public byte ColorBeard1Id;

	[SerializableGameDataField]
	public byte ColorBeard2Id;

	[SerializableGameDataField]
	public short FrontHairId;

	[SerializableGameDataField]
	public short BackHairId;

	[SerializableGameDataField]
	public byte ColorFrontHairId;

	[SerializableGameDataField]
	public byte ColorBackHairId;

	[SerializableGameDataField]
	public short Feature1Id;

	[SerializableGameDataField]
	public short Feature2Id;

	[SerializableGameDataField]
	public short Wrinkle1Id;

	[SerializableGameDataField]
	public short Wrinkle2Id;

	[SerializableGameDataField]
	public short Wrinkle3Id;

	[SerializableGameDataField]
	public byte ColorFeature1Id;

	[SerializableGameDataField]
	public byte ColorFeature2Id;

	[SerializableGameDataField]
	private byte _growableElementsShowingAbilities;

	[SerializableGameDataField]
	private byte _growableElementsShowingStates;

	public static readonly short[] CharmLevel = new short[9] { 100, 200, 300, 400, 500, 600, 700, 800, 900 };

	[NonSerialized]
	private sbyte _eyesHeightIndex;

	[NonSerialized]
	private AvatarGroup _avatarGroup;

	[NonSerialized]
	private AvatarAsset _headAsset;

	public short BaseCharm => GetBaseCharm();

	public sbyte Gender => GetGender();

	public bool FaceVisible
	{
		get
		{
			if (!ShowVeil)
			{
				return !ShowMask();
			}
			return false;
		}
	}

	private AvatarManager AvatarManager => AvatarManager.Instance;

	public sbyte GetGender()
	{
		return (AvatarId % 2 == 1) ? ((sbyte)1) : ((sbyte)0);
	}

	public bool ShowMask()
	{
		short clothDisplayId = ClothDisplayId;
		return ShowMask(clothDisplayId);
	}

	public bool ShowMask(short clothDisplayId)
	{
		AvatarAsset asset = AvatarManager.GetAsset(AvatarId, EAvatarElementsType.Cloth, clothDisplayId);
		if (asset != null && asset.Config.RelativeExtraPart >= 6)
		{
			return asset.Config.RelativeExtraPart <= 23;
		}
		return false;
	}

	public void ChangeGender(sbyte gender)
	{
		int num = AvatarId % 2;
		if (num != gender)
		{
			if (num == 1)
			{
				AvatarId++;
			}
			else
			{
				AvatarId--;
			}
		}
	}

	public sbyte GetBodyType()
	{
		return (sbyte)((AvatarId - 1) / 2);
	}

	public void ChangeBodyType(sbyte bodyType)
	{
		int num = (AvatarId - 1) % 2;
		AvatarId = (byte)(bodyType * 2 + num + 1);
	}

	public AvatarData FormatDisabledElements()
	{
		AvatarManager instance = AvatarManager.Instance;
		AvatarAsset backHairAsset = instance.GetAsset(AvatarId, EAvatarElementsType.Hair2, BackHairId);
		AvatarAsset asset = instance.GetAsset(AvatarId, EAvatarElementsType.Hair1, FrontHairId);
		if (asset.Config.DisableRelativeType)
		{
			BackHairId = 1;
		}
		if (asset.Config.BanElements != null && Array.Exists(asset.Config.BanElements, (uint e) => e == backHairAsset.Config.TemplateId))
		{
			BackHairId = 1;
		}
		return this;
	}

	public void ConvertAvatarToSkeleton()
	{
		sbyte bodyType = GetBodyType();
		byte index = bodyType switch
		{
			0 => (byte)((Gender == 0) ? 11 : 10), 
			1 => (byte)((Gender == 0) ? 13 : 12), 
			2 => (byte)((Gender == 0) ? 15 : 14), 
			_ => throw new Exception($"invalid body type {bodyType}"), 
		};
		HeadId = AvatarHead.Instance[index].HeadId;
	}

	public void SetGrowableElementShowingAbility(sbyte growableElementType, bool showable)
	{
		if (showable)
		{
			_growableElementsShowingAbilities |= (byte)(1 << (int)growableElementType);
		}
		else
		{
			_growableElementsShowingAbilities &= (byte)(~(1 << (int)growableElementType));
		}
	}

	public void SetGrowableElementShowingAbility(sbyte growableElementType)
	{
		_growableElementsShowingAbilities |= (byte)(1 << (int)growableElementType);
	}

	public void ResetGrowableElementShowingAbility(sbyte growableElementType)
	{
		_growableElementsShowingAbilities &= (byte)(~(1 << (int)growableElementType));
	}

	public void ClearGrowableElementShowingAbilities()
	{
		_growableElementsShowingAbilities = 0;
	}

	public void FillGrowableElementsShowingStates()
	{
		_growableElementsShowingStates = byte.MaxValue;
	}

	public byte GetGrowableElementShowingAbilities()
	{
		return _growableElementsShowingAbilities;
	}

	public byte GetGrowableElementShowingStates()
	{
		return _growableElementsShowingStates;
	}

	public bool GetGrowableElementShowingAbility(sbyte growableElementType)
	{
		return (_growableElementsShowingAbilities & (1 << (int)growableElementType)) != 0;
	}

	public void SetGrowableElementShowingState(sbyte growableElementType, bool show)
	{
		if (show)
		{
			_growableElementsShowingStates |= (byte)(1 << (int)growableElementType);
		}
		else
		{
			_growableElementsShowingStates &= (byte)(~(1 << (int)growableElementType));
		}
	}

	public void SetGrowableElementShowingState(sbyte growableElementType)
	{
		_growableElementsShowingStates |= (byte)(1 << (int)growableElementType);
	}

	public void ResetGrowableElementShowingState(sbyte growableElementType)
	{
		_growableElementsShowingStates &= (byte)(~(1 << (int)growableElementType));
	}

	public bool GetGrowableElementShowingState(sbyte growableElementType)
	{
		return (_growableElementsShowingStates & (1 << (int)growableElementType)) != 0;
	}

	public AvatarData GenerateMultipleBirthChildAvatar(IRandomSource random, sbyte gender)
	{
		AvatarData avatarData = new AvatarData(this);
		avatarData.ChangeGender(gender);
		AvatarGroup avatarGroup = AvatarManager.Instance.GetAvatarGroup(avatarData.AvatarId);
		(avatarData.FrontHairId, avatarData.BackHairId) = avatarGroup.GetRandomHairs(random);
		return avatarData;
	}

	public void Copy(AvatarData other)
	{
		Assign(other);
		ClothDisplayId = other.ClothDisplayId;
	}

	public (string[], string[]) GetSkeletonSlotAndAttachment()
	{
		if (!GetGrowableElementShowingState(0) || !GetGrowableElementShowingAbility(0))
		{
			return (null, null);
		}
		AvatarManager instance = AvatarManager.Instance;
		AvatarAsset asset = instance.GetAsset(AvatarId, EAvatarElementsType.Hair1, FrontHairId);
		AvatarAsset asset2 = instance.GetAsset(AvatarId, EAvatarElementsType.Hair2, BackHairId);
		return (asset.Config.SkeletonSlotAndAttachment, asset2.Config.SkeletonSlotAndAttachment);
	}

	public void ChangeToMarriageStyle1()
	{
		ClothDisplayId = SharedConstValue.MarriageClothDisplayId;
		FrontHairId = SharedConstValue.MarriageHairHeadDressId;
		BackHairId = SharedConstValue.MarriageHairHeadDressId;
	}

	public void ChangeToMarriageStyle2()
	{
		ClothDisplayId = SharedConstValue.MarriageClothDisplayId;
		FrontHairId = SharedConstValue.MarriageHairPhoenixCoronetId;
		BackHairId = SharedConstValue.MarriageHairPhoenixCoronetId;
	}

	public void ChangeToShixiangBarbarianMaster()
	{
		ClothDisplayId = SharedConstValue.ShixiangBarbarianMasterClothDisplayId;
	}

	public AvatarData()
	{
	}

	public AvatarData(AvatarData other)
	{
		ShowVeil = other.ShowVeil;
		AvatarId = other.AvatarId;
		ColorSkinId = other.ColorSkinId;
		ColorClothId = other.ColorClothId;
		ChildClothId = other.ChildClothId;
		ClothPartId = other.ClothPartId;
		HeadId = other.HeadId;
		EyesMainId = other.EyesMainId;
		EyesLeftId = other.EyesLeftId;
		EyesRightId = other.EyesRightId;
		EyebrowId = other.EyebrowId;
		ColorEyeballId = other.ColorEyeballId;
		ColorEyebrowId = other.ColorEyebrowId;
		EyesHeightPercent = other.EyesHeightPercent;
		EyesDistancePercent = other.EyesDistancePercent;
		EyesAngle = other.EyesAngle;
		EyesScale = other.EyesScale;
		EyebrowHeight = other.EyebrowHeight;
		EyebrowDistancePercent = other.EyebrowDistancePercent;
		EyebrowAngle = other.EyebrowAngle;
		EyebrowScale = other.EyebrowScale;
		NoseId = other.NoseId;
		NoseHeightPercent = other.NoseHeightPercent;
		NoseScale = other.NoseScale;
		MouthId = other.MouthId;
		MouthHeightPercent = other.MouthHeightPercent;
		MouthScale = other.MouthScale;
		ColorMouthId = other.ColorMouthId;
		Beard1Id = other.Beard1Id;
		Beard2Id = other.Beard2Id;
		ColorBeard1Id = other.ColorBeard1Id;
		ColorBeard2Id = other.ColorBeard2Id;
		FrontHairId = other.FrontHairId;
		BackHairId = other.BackHairId;
		ColorFrontHairId = other.ColorFrontHairId;
		ColorBackHairId = other.ColorBackHairId;
		Feature1Id = other.Feature1Id;
		Feature2Id = other.Feature2Id;
		Wrinkle1Id = other.Wrinkle1Id;
		Wrinkle2Id = other.Wrinkle2Id;
		Wrinkle3Id = other.Wrinkle3Id;
		ColorFeature1Id = other.ColorFeature1Id;
		ColorFeature2Id = other.ColorFeature2Id;
		_growableElementsShowingAbilities = other._growableElementsShowingAbilities;
		_growableElementsShowingStates = other._growableElementsShowingStates;
	}

	public void Assign(AvatarData other)
	{
		ShowVeil = other.ShowVeil;
		AvatarId = other.AvatarId;
		ColorSkinId = other.ColorSkinId;
		ColorClothId = other.ColorClothId;
		ChildClothId = other.ChildClothId;
		ClothPartId = other.ClothPartId;
		HeadId = other.HeadId;
		EyesMainId = other.EyesMainId;
		EyesLeftId = other.EyesLeftId;
		EyesRightId = other.EyesRightId;
		EyebrowId = other.EyebrowId;
		ColorEyeballId = other.ColorEyeballId;
		ColorEyebrowId = other.ColorEyebrowId;
		EyesHeightPercent = other.EyesHeightPercent;
		EyesDistancePercent = other.EyesDistancePercent;
		EyesAngle = other.EyesAngle;
		EyesScale = other.EyesScale;
		EyebrowHeight = other.EyebrowHeight;
		EyebrowDistancePercent = other.EyebrowDistancePercent;
		EyebrowAngle = other.EyebrowAngle;
		EyebrowScale = other.EyebrowScale;
		NoseId = other.NoseId;
		NoseHeightPercent = other.NoseHeightPercent;
		NoseScale = other.NoseScale;
		MouthId = other.MouthId;
		MouthHeightPercent = other.MouthHeightPercent;
		MouthScale = other.MouthScale;
		ColorMouthId = other.ColorMouthId;
		Beard1Id = other.Beard1Id;
		Beard2Id = other.Beard2Id;
		ColorBeard1Id = other.ColorBeard1Id;
		ColorBeard2Id = other.ColorBeard2Id;
		FrontHairId = other.FrontHairId;
		BackHairId = other.BackHairId;
		ColorFrontHairId = other.ColorFrontHairId;
		ColorBackHairId = other.ColorBackHairId;
		Feature1Id = other.Feature1Id;
		Feature2Id = other.Feature2Id;
		Wrinkle1Id = other.Wrinkle1Id;
		Wrinkle2Id = other.Wrinkle2Id;
		Wrinkle3Id = other.Wrinkle3Id;
		ColorFeature1Id = other.ColorFeature1Id;
		ColorFeature2Id = other.ColorFeature2Id;
		_growableElementsShowingAbilities = other._growableElementsShowingAbilities;
		_growableElementsShowingStates = other._growableElementsShowingStates;
	}

	public bool IsSerializedSizeFixed()
	{
		return true;
	}

	public int GetSerializedSize()
	{
		int num = 73;
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		*pData = (ShowVeil ? ((byte)1) : ((byte)0));
		byte* num = pData + 1;
		*num = AvatarId;
		byte* num2 = num + 1;
		*num2 = ColorSkinId;
		byte* num3 = num2 + 1;
		*num3 = ColorClothId;
		byte* num4 = num3 + 1;
		*(short*)num4 = ChildClothId;
		byte* num5 = num4 + 2;
		*num5 = ClothPartId;
		byte* num6 = num5 + 1;
		*num6 = HeadId;
		byte* num7 = num6 + 1;
		*(short*)num7 = EyesMainId;
		byte* num8 = num7 + 2;
		*(short*)num8 = EyesLeftId;
		byte* num9 = num8 + 2;
		*(short*)num9 = EyesRightId;
		byte* num10 = num9 + 2;
		*(short*)num10 = EyebrowId;
		byte* num11 = num10 + 2;
		*num11 = ColorEyeballId;
		byte* num12 = num11 + 1;
		*num12 = ColorEyebrowId;
		byte* num13 = num12 + 1;
		*(short*)num13 = EyesHeightPercent;
		byte* num14 = num13 + 2;
		*(short*)num14 = EyesDistancePercent;
		byte* num15 = num14 + 2;
		*(short*)num15 = EyesAngle;
		byte* num16 = num15 + 2;
		*(short*)num16 = EyesScale;
		byte* num17 = num16 + 2;
		*(short*)num17 = EyebrowHeight;
		byte* num18 = num17 + 2;
		*(short*)num18 = EyebrowDistancePercent;
		byte* num19 = num18 + 2;
		*(short*)num19 = EyebrowAngle;
		byte* num20 = num19 + 2;
		*(short*)num20 = EyebrowScale;
		byte* num21 = num20 + 2;
		*(short*)num21 = NoseId;
		byte* num22 = num21 + 2;
		*(short*)num22 = NoseHeightPercent;
		byte* num23 = num22 + 2;
		*(short*)num23 = NoseScale;
		byte* num24 = num23 + 2;
		*(short*)num24 = MouthId;
		byte* num25 = num24 + 2;
		*(short*)num25 = MouthHeightPercent;
		byte* num26 = num25 + 2;
		*(short*)num26 = MouthScale;
		byte* num27 = num26 + 2;
		*num27 = ColorMouthId;
		byte* num28 = num27 + 1;
		*(short*)num28 = Beard1Id;
		byte* num29 = num28 + 2;
		*(short*)num29 = Beard2Id;
		byte* num30 = num29 + 2;
		*num30 = ColorBeard1Id;
		byte* num31 = num30 + 1;
		*num31 = ColorBeard2Id;
		byte* num32 = num31 + 1;
		*(short*)num32 = FrontHairId;
		byte* num33 = num32 + 2;
		*(short*)num33 = BackHairId;
		byte* num34 = num33 + 2;
		*num34 = ColorFrontHairId;
		byte* num35 = num34 + 1;
		*num35 = ColorBackHairId;
		byte* num36 = num35 + 1;
		*(short*)num36 = Feature1Id;
		byte* num37 = num36 + 2;
		*(short*)num37 = Feature2Id;
		byte* num38 = num37 + 2;
		*(short*)num38 = Wrinkle1Id;
		byte* num39 = num38 + 2;
		*(short*)num39 = Wrinkle2Id;
		byte* num40 = num39 + 2;
		*(short*)num40 = Wrinkle3Id;
		byte* num41 = num40 + 2;
		*num41 = ColorFeature1Id;
		byte* num42 = num41 + 1;
		*num42 = ColorFeature2Id;
		byte* num43 = num42 + 1;
		*num43 = _growableElementsShowingAbilities;
		byte* num44 = num43 + 1;
		*num44 = _growableElementsShowingStates;
		int num45 = (int)(num44 + 1 - pData);
		if (num45 > 4)
		{
			return (num45 + 3) / 4 * 4;
		}
		return num45;
	}

	public unsafe int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		ShowVeil = *ptr != 0;
		ptr++;
		AvatarId = *ptr;
		ptr++;
		ColorSkinId = *ptr;
		ptr++;
		ColorClothId = *ptr;
		ptr++;
		ChildClothId = *(short*)ptr;
		ptr += 2;
		ClothPartId = *ptr;
		ptr++;
		HeadId = *ptr;
		ptr++;
		EyesMainId = *(short*)ptr;
		ptr += 2;
		EyesLeftId = *(short*)ptr;
		ptr += 2;
		EyesRightId = *(short*)ptr;
		ptr += 2;
		EyebrowId = *(short*)ptr;
		ptr += 2;
		ColorEyeballId = *ptr;
		ptr++;
		ColorEyebrowId = *ptr;
		ptr++;
		EyesHeightPercent = *(short*)ptr;
		ptr += 2;
		EyesDistancePercent = *(short*)ptr;
		ptr += 2;
		EyesAngle = *(short*)ptr;
		ptr += 2;
		EyesScale = *(short*)ptr;
		ptr += 2;
		EyebrowHeight = *(short*)ptr;
		ptr += 2;
		EyebrowDistancePercent = *(short*)ptr;
		ptr += 2;
		EyebrowAngle = *(short*)ptr;
		ptr += 2;
		EyebrowScale = *(short*)ptr;
		ptr += 2;
		NoseId = *(short*)ptr;
		ptr += 2;
		NoseHeightPercent = *(short*)ptr;
		ptr += 2;
		NoseScale = *(short*)ptr;
		ptr += 2;
		MouthId = *(short*)ptr;
		ptr += 2;
		MouthHeightPercent = *(short*)ptr;
		ptr += 2;
		MouthScale = *(short*)ptr;
		ptr += 2;
		ColorMouthId = *ptr;
		ptr++;
		Beard1Id = *(short*)ptr;
		ptr += 2;
		Beard2Id = *(short*)ptr;
		ptr += 2;
		ColorBeard1Id = *ptr;
		ptr++;
		ColorBeard2Id = *ptr;
		ptr++;
		FrontHairId = *(short*)ptr;
		ptr += 2;
		BackHairId = *(short*)ptr;
		ptr += 2;
		ColorFrontHairId = *ptr;
		ptr++;
		ColorBackHairId = *ptr;
		ptr++;
		Feature1Id = *(short*)ptr;
		ptr += 2;
		Feature2Id = *(short*)ptr;
		ptr += 2;
		Wrinkle1Id = *(short*)ptr;
		ptr += 2;
		Wrinkle2Id = *(short*)ptr;
		ptr += 2;
		Wrinkle3Id = *(short*)ptr;
		ptr += 2;
		ColorFeature1Id = *ptr;
		ptr++;
		ColorFeature2Id = *ptr;
		ptr++;
		_growableElementsShowingAbilities = *ptr;
		ptr++;
		_growableElementsShowingStates = *ptr;
		ptr++;
		int num = (int)(ptr - pData);
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public short GetCharm(short characterAge, short clothId)
	{
		double num = GetBaseCharm();
		_headAsset = AvatarManager.GetAsset(AvatarId, EAvatarElementsType.Head, HeadId);
		_eyesHeightIndex = -1;
		double num2 = num * CalCharmRate() + GetWrinkleCharm(characterAge) + GetClothCharm(clothId);
		_headAsset = null;
		return (short)num2;
	}

	public short GetBaseCharm()
	{
		_headAsset = AvatarManager.GetAsset(AvatarId, EAvatarElementsType.Head, HeadId);
		double item = GetFeatureCharm().Item1;
		double num = GetEyebrowsCharm() * (double)GlobalConfig.Instance.EyebrowRatioInBaseCharm + GetEyesCharm() * (double)GlobalConfig.Instance.EyesRatioInBaseCharm + GetNoseCharm() * (double)GlobalConfig.Instance.NoseRatioInBaseCharm + GetMouthCharm() * (double)GlobalConfig.Instance.MouthRatioInBaseCharm + item;
		_headAsset = null;
		return (short)num;
	}

	public (double, double) GetFeatureCharm()
	{
		double item = 0.0;
		AvatarAsset asset = AvatarManager.GetAsset(AvatarId, EAvatarElementsType.Feature1, Feature1Id);
		if (asset != null)
		{
			item = asset.Config.ElemCharm;
		}
		double item2 = 1.0;
		AvatarAsset asset2 = AvatarManager.GetAsset(AvatarId, EAvatarElementsType.Feature2, Feature2Id);
		if (asset2 != null)
		{
			item2 = asset2.Config.CharmExtraArg;
		}
		return (item, item2);
	}

	public double GetWrinkleCharm(short age)
	{
		short num = 0;
		if (GetGrowableElementShowingState(3) && GetGrowableElementShowingAbility(3))
		{
			AvatarAsset asset = AvatarManager.GetAsset(AvatarId, EAvatarElementsType.Wrinkle1, Wrinkle1Id);
			if (asset != null)
			{
				num += asset.Config.ElemCharm;
			}
		}
		if (GetGrowableElementShowingState(4) && GetGrowableElementShowingAbility(4))
		{
			AvatarAsset asset2 = AvatarManager.GetAsset(AvatarId, EAvatarElementsType.Wrinkle2, Wrinkle2Id);
			if (asset2 != null)
			{
				num += asset2.Config.ElemCharm;
			}
		}
		if (GetGrowableElementShowingState(5) && GetGrowableElementShowingAbility(5))
		{
			AvatarAsset asset3 = AvatarManager.GetAsset(AvatarId, EAvatarElementsType.Wrinkle3, Wrinkle3Id);
			if (asset3 != null)
			{
				num += asset3.Config.ElemCharm;
			}
		}
		return num;
	}

	public double CalCharmRate()
	{
		short num = FrontHairId;
		short num2 = BackHairId;
		if (!GetGrowableElementShowingState(0) || !GetGrowableElementShowingAbility(0))
		{
			num = 1;
			num2 = 1;
		}
		float val = 1f;
		AvatarAsset asset = AvatarManager.GetAsset(AvatarId, EAvatarElementsType.Hair1, num);
		if (asset != null)
		{
			val = Math.Min(val, asset.Config.CharmExtraArg);
		}
		AvatarAsset asset2 = AvatarManager.GetAsset(AvatarId, EAvatarElementsType.Hair2, num2);
		if (asset2 != null)
		{
			val = Math.Min(val, asset2.Config.CharmExtraArg);
		}
		if (GetGrowableElementShowingAbility(1) && GetGrowableElementShowingState(1))
		{
			AvatarAsset asset3 = AvatarManager.GetAsset(AvatarId, EAvatarElementsType.Beard1, Beard1Id);
			if (asset3 != null)
			{
				val = Math.Min(val, asset3.Config.CharmExtraArg);
			}
		}
		if (GetGrowableElementShowingAbility(2) && GetGrowableElementShowingState(2))
		{
			AvatarAsset asset4 = AvatarManager.GetAsset(AvatarId, EAvatarElementsType.Beard2, Beard2Id);
			if (asset4 != null)
			{
				val = Math.Min(val, asset4.Config.CharmExtraArg);
			}
		}
		double item = GetFeatureCharm().Item2;
		val = Math.Min(val, (float)item);
		return val;
	}

	public double GetBeard1Charm(short age)
	{
		if (!GetGrowableElementShowingState(1) || !GetGrowableElementShowingAbility(1))
		{
			return 0.0;
		}
		short num = 0;
		AvatarAsset asset = AvatarManager.GetAsset(AvatarId, EAvatarElementsType.Beard1, Beard1Id);
		if (asset != null)
		{
			num = asset.Config.ElemCharm;
		}
		return num;
	}

	public double GetBeard2Charm(short age)
	{
		if (!GetGrowableElementShowingState(2) || !GetGrowableElementShowingAbility(2))
		{
			return 0.0;
		}
		short num = 0;
		AvatarAsset asset = AvatarManager.GetAsset(AvatarId, EAvatarElementsType.Beard2, Beard2Id);
		if (asset != null)
		{
			num = asset.Config.ElemCharm;
		}
		return num;
	}

	public double GetClothCharm(short clothId)
	{
		short num = 0;
		AvatarAsset asset = AvatarManager.GetAsset(AvatarId, EAvatarElementsType.Cloth, clothId);
		if (asset != null)
		{
			num = asset.Config.ElemCharm;
		}
		return num;
	}

	public double GetMouthCharm()
	{
		double result = 0.0;
		AvatarAsset asset = AvatarManager.GetAsset(AvatarId, EAvatarElementsType.Mouth, MouthId);
		if (asset != null)
		{
			List<float[]> charmMouthHeight = GlobalConfig.Instance.CharmMouthHeight;
			double charmRate = GetCharmRate(MouthHeightPercent, charmMouthHeight, 2);
			result = (double)asset.Config.ElemCharm * charmRate;
		}
		return result;
	}

	public double GetNoseCharm()
	{
		double result = 0.0;
		AvatarAsset asset = AvatarManager.GetAsset(AvatarId, EAvatarElementsType.Nose, NoseId);
		if (asset != null)
		{
			List<float[]> charmNoseHeight = GlobalConfig.Instance.CharmNoseHeight;
			double num = GetCharmRate(NoseHeightPercent, charmNoseHeight, 1);
			if ((int)Math.Floor((float)NoseHeightPercent / (100f / (float)charmNoseHeight.Count)) >= 4 && (_eyesHeightIndex == 0 || _eyesHeightIndex == 4))
			{
				num = 0.05000000074505806;
			}
			result = (double)asset.Config.ElemCharm * num;
		}
		return result;
	}

	public double GetEyesCharm()
	{
		AvatarAsset asset = AvatarManager.GetAsset(AvatarId, EAvatarElementsType.Eye, EyesMainId, EyesLeftId);
		AvatarAsset asset2 = AvatarManager.GetAsset(AvatarId, EAvatarElementsType.Eye, EyesMainId, EyesRightId);
		short num = Math.Min(asset.Config.ElemCharm, asset2.Config.ElemCharm);
		List<float[]> charmEyesHeight = GlobalConfig.Instance.CharmEyesHeight;
		double charmRate = GetCharmRate(EyesHeightPercent, charmEyesHeight, 2);
		_eyesHeightIndex = (sbyte)Math.Floor((float)EyesHeightPercent / (100f / (float)charmEyesHeight.Count));
		List<float[]> charmEyesDistance = GlobalConfig.Instance.CharmEyesDistance;
		double charmRate2 = GetCharmRate(EyesDistancePercent, charmEyesDistance, 2);
		sbyte b = (sbyte)Math.Floor((float)EyesDistancePercent / (100f / (float)charmEyesDistance.Count));
		List<int> avatarEyeRotateRange = GlobalConfig.Instance.AvatarEyeRotateRange;
		short num2 = (short)((double)((float)EyesAngle * 0.01f - (float)avatarEyeRotateRange[0]) / (double)(avatarEyeRotateRange[1] - avatarEyeRotateRange[0]) * 100.0);
		List<float[]> charmEyesRotate = GlobalConfig.Instance.CharmEyesRotate;
		double charmRate3 = GetCharmRate(num2, charmEyesRotate, 2);
		sbyte b2 = (sbyte)Math.Floor((float)num2 / (100f / (float)charmEyesRotate.Count));
		double num3 = (((_eyesHeightIndex != 0 && _eyesHeightIndex != 4) || (b != 0 && b != 4) || (b2 != 0 && b2 != 4)) ? Math.Min(Math.Min(charmRate, charmRate2), charmRate3) : (charmRate * charmRate2 * charmRate3));
		return (double)num * num3;
	}

	public double GetEyebrowsCharm()
	{
		double result = 0.0;
		AvatarAsset asset = AvatarManager.GetAsset(AvatarId, EAvatarElementsType.EyeBrow, EyebrowId);
		AvatarAsset asset2 = AvatarManager.GetAsset(AvatarId, EAvatarElementsType.Head, HeadId);
		if (asset != null && asset2 != null)
		{
			float num = (float)EyebrowHeight * 0.01f;
			List<float> avatarEyebrowOffsetRange = GlobalConfig.Instance.AvatarEyebrowOffsetRange;
			short charmPercent = (short)(100.0 * ((double)(num - avatarEyebrowOffsetRange[0]) / (double)(avatarEyebrowOffsetRange[1] - avatarEyebrowOffsetRange[0])));
			List<float[]> charmEyebrowHeight = GlobalConfig.Instance.CharmEyebrowHeight;
			double charmRate = GetCharmRate(charmPercent, charmEyebrowHeight, 2);
			double val = GetCharmRate(divideInfo: GlobalConfig.Instance.CharmEyebrowDistance, charmPercent: EyebrowDistancePercent, bestAreaIndex: 2);
			List<int> avatarEyebrowRotateRange = GlobalConfig.Instance.AvatarEyebrowRotateRange;
			short charmPercent2 = (short)((double)((float)EyebrowAngle * 0.01f - (float)avatarEyebrowRotateRange[0]) / (double)(avatarEyebrowRotateRange[1] - avatarEyebrowRotateRange[0]) * 100.0);
			List<float[]> charmEyebrowRotate = GlobalConfig.Instance.CharmEyebrowRotate;
			double num2 = Math.Min(val2: GetCharmRate(charmPercent2, charmEyebrowRotate, 2), val1: Math.Min(charmRate, val));
			result = (double)asset.Config.ElemCharm * num2;
		}
		return result;
	}

	public bool AdjustToBaseCharm(IRandomSource random, short targetBaseCharm)
	{
		_avatarGroup = AvatarManager.GetAvatarGroup(AvatarId);
		_headAsset = AvatarManager.GetAsset(AvatarId, EAvatarElementsType.Head, HeadId);
		double num = targetBaseCharm;
		if (random.CheckProb(GlobalConfig.Instance.AvatarBadFeatureObb, 10000) && num / (double)_avatarGroup.WorstFeature2.Config.CharmExtraArg < 900.0)
		{
			AvatarAsset avatarAsset = _avatarGroup.Feature2Res[random.Next(1, _avatarGroup.Feature2Res.Count)];
			Feature2Id = avatarAsset.Id;
			num /= (double)avatarAsset.Config.CharmExtraArg;
		}
		else
		{
			Feature2Id = 1;
		}
		AvatarAsset asset = AvatarManager.GetAsset(AvatarId, EAvatarElementsType.Feature1, Feature1Id);
		if (asset != null && asset.Config.ElemCharm != 0)
		{
			num -= (double)asset.Config.ElemCharm;
		}
		if (num <= 0.0)
		{
			num = targetBaseCharm;
			Feature1Id = 1;
		}
		double num2 = num * (double)GlobalConfig.Instance.EyesRatioInBaseCharm;
		double num3 = num * (double)GlobalConfig.Instance.MouthRatioInBaseCharm;
		double num4 = num * (double)GlobalConfig.Instance.NoseRatioInBaseCharm;
		double num5 = num - num2 - num3 - num4;
		(sbyte, sbyte) eyesIndexTuple;
		bool num6 = AdjustEyes(random, num2, out eyesIndexTuple);
		EyebrowDistancePercent = EyesDistancePercent;
		double num7 = GetEyesCharm() * (double)GlobalConfig.Instance.EyesRatioInBaseCharm;
		num3 = num - num7 - num4 - num5;
		bool flag = AdjustMouth(random, num3);
		double num8 = GetMouthCharm() * (double)GlobalConfig.Instance.MouthRatioInBaseCharm;
		num4 = num - num7 - num8 - num5;
		bool flag2 = AdjustNose(random, num4, eyesIndexTuple);
		double num9 = GetNoseCharm() * (double)GlobalConfig.Instance.NoseRatioInBaseCharm;
		num5 = num - num7 - num8 - num9;
		bool flag3 = AdjustEyebrow(random, num5);
		_avatarGroup = null;
		_headAsset = null;
		return num6 && flag2 && flag3 && flag;
	}

	private bool AdjustEyebrow(IRandomSource random, double charm)
	{
		charm /= (double)GlobalConfig.Instance.EyebrowRatioInBaseCharm;
		List<AvatarAsset> list = new List<AvatarAsset>();
		double num = double.MaxValue;
		for (int i = 0; i < _avatarGroup.EyeBrowRes.Count; i++)
		{
			double num2 = (double)_avatarGroup.EyeBrowRes[i].Config.ElemCharm - charm;
			if (list.Count <= 0)
			{
				num = num2;
				list.Add(_avatarGroup.EyeBrowRes[i]);
				continue;
			}
			if (Math.Abs(num2 - num) < 0.20000000298023224)
			{
				list.Add(_avatarGroup.EyeBrowRes[i]);
				continue;
			}
			bool flag = Math.Abs(num2) < Math.Abs(num);
			if (flag)
			{
				if (num2 < 0.0 && num > 0.0 && num < Math.Abs(num2) * 2.0)
				{
					flag = false;
				}
			}
			else if (num2 > 0.0 && num < 0.0 && num2 < Math.Abs(num) * 2.0)
			{
				flag = true;
			}
			if (flag)
			{
				list.Clear();
				num = num2;
				list.Add(_avatarGroup.EyeBrowRes[i]);
			}
		}
		if (list.Count <= 0)
		{
			return false;
		}
		AvatarAsset random2 = list.GetRandom(random);
		double charmPercent = charm / (double)random2.Config.ElemCharm;
		List<float[]> charmHeightDivideInfo = GlobalConfig.Instance.CharmEyebrowHeight;
		List<float[]> charmDistanceDivideInfo = GlobalConfig.Instance.CharmEyebrowDistance;
		List<float[]> charmRotateDivideInfo = GlobalConfig.Instance.CharmEyebrowRotate;
		List<int> rotateRange = GlobalConfig.Instance.AvatarEyebrowRotateRange;
		List<float> heightRange = GlobalConfig.Instance.AvatarEyebrowOffsetRange;
		EyebrowId = random2.Id;
		EyesAngle = 0;
		EyebrowDistancePercent = 50;
		EyebrowHeight = (short)((heightRange[0] + (heightRange[1] - heightRange[0]) * 0.5f) * 100f);
		if (charm >= (double)random2.Config.ElemCharm)
		{
			return true;
		}
		float[] array = new float[2]
		{
			charmHeightDivideInfo[2][1],
			charmHeightDivideInfo[1][0]
		};
		bool flag2 = IsInRange(array, charmPercent);
		array[0] = charmDistanceDivideInfo[2][1];
		array[1] = charmDistanceDivideInfo[0][0];
		bool flag3 = IsInRange(array, charmPercent);
		array[0] = charmRotateDivideInfo[2][1];
		array[1] = charmRotateDivideInfo[0][0];
		bool num3 = IsInRange(array, charmPercent);
		List<Action> list2 = new List<Action>();
		if (flag3)
		{
			list2.Add(AdjustBasedOnDistance);
		}
		if (flag2)
		{
			list2.Add(AdjustBasedOnHeight);
		}
		if (num3)
		{
			list2.Add(AdjustBasedOnRotate);
		}
		if (list2.Count > 0)
		{
			list2.GetRandom(random)();
			return true;
		}
		return false;
		void AdjustBasedOnDistance()
		{
			sbyte resultIndex;
			double areaLerpValueByPercent = GetAreaLerpValueByPercent(random, charmDistanceDivideInfo, charmPercent, 2, out resultIndex, -1);
			EyebrowDistancePercent = (short)(areaLerpValueByPercent * 100.0);
		}
		void AdjustBasedOnHeight()
		{
			sbyte resultIndex;
			double areaLerpValueByPercent = GetAreaLerpValueByPercent(random, charmHeightDivideInfo, charmPercent, 2, out resultIndex, -1);
			EyebrowHeight = (short)(((double)heightRange[0] + (double)(heightRange[1] - heightRange[0]) * areaLerpValueByPercent) * 100.0);
		}
		void AdjustBasedOnRotate()
		{
			sbyte resultIndex;
			double areaLerpValueByPercent = GetAreaLerpValueByPercent(random, charmRotateDivideInfo, charmPercent, 2, out resultIndex, -1);
			EyebrowAngle = (short)(((double)rotateRange[0] + (double)(rotateRange[1] - rotateRange[0]) * areaLerpValueByPercent) * 100.0);
		}
	}

	private bool AdjustEyes(IRandomSource random, double charm, out (sbyte, sbyte) eyesIndexTuple)
	{
		charm /= (double)GlobalConfig.Instance.EyesRatioInBaseCharm;
		eyesIndexTuple.Item1 = -1;
		eyesIndexTuple.Item2 = -1;
		List<EyeRes> list = new List<EyeRes>();
		double num = double.MaxValue;
		for (int i = 0; i < _avatarGroup.EyesGroup.Count; i++)
		{
			double num2 = (double)Math.Min(_avatarGroup.EyesGroup[i].LeftEye.Config.ElemCharm, _avatarGroup.EyesGroup[i].RightEye.Config.ElemCharm) - charm;
			if (list.Count <= 0)
			{
				list.Add(_avatarGroup.EyesGroup[i]);
				num = num2;
				continue;
			}
			if (Math.Abs(num2 - num) < 0.20000000298023224)
			{
				list.Add(_avatarGroup.EyesGroup[i]);
				continue;
			}
			bool flag = Math.Abs(num2) < Math.Abs(num);
			if (flag)
			{
				if (num2 < 0.0 && num > 0.0 && num < Math.Abs(num2) * 2.0)
				{
					flag = false;
				}
			}
			else if (num2 > 0.0 && num < 0.0 && num2 < Math.Abs(num) * 2.0)
			{
				flag = true;
			}
			if (flag)
			{
				num = num2;
				list.Clear();
				list.Add(_avatarGroup.EyesGroup[i]);
			}
		}
		if (list.Count <= 0)
		{
			return false;
		}
		EyeRes random2 = list.GetRandom(random);
		short num3 = Math.Min(random2.LeftEye.Config.ElemCharm, random2.RightEye.Config.ElemCharm);
		double charmPercent = charm / (double)num3;
		EyesMainId = random2.Id;
		EyesLeftId = random2.LeftEye.SubId;
		EyesRightId = random2.RightEye.SubId;
		List<float[]> charmHeightDivideInfo = GlobalConfig.Instance.CharmEyesHeight;
		List<float[]> charmDistanceDivideInfo = GlobalConfig.Instance.CharmEyesDistance;
		List<float[]> charmRotateDivideInfo = GlobalConfig.Instance.CharmEyesRotate;
		List<int> rotateRange = GlobalConfig.Instance.AvatarEyeRotateRange;
		EyesAngle = 0;
		if (charm >= (double)num3)
		{
			eyesIndexTuple.Item1 = 2;
			eyesIndexTuple.Item2 = 2;
			EyesDistancePercent = 50;
			EyesHeightPercent = 50;
			return true;
		}
		float[] array = new float[2]
		{
			charmHeightDivideInfo[2][1],
			charmHeightDivideInfo[1][0]
		};
		bool flag2 = IsInRange(array, charmPercent);
		array[0] = charmDistanceDivideInfo[2][1];
		array[1] = charmDistanceDivideInfo[0][0];
		bool flag3 = IsInRange(array, charmPercent);
		array[0] = charmRotateDivideInfo[2][1];
		array[1] = charmRotateDivideInfo[0][0];
		bool num4 = IsInRange(array, charmPercent);
		List<Func<(sbyte, sbyte)>> list2 = new List<Func<(sbyte, sbyte)>>();
		if (flag3)
		{
			list2.Add(AdjustBasedOnDistance);
		}
		if (flag2)
		{
			list2.Add(AdjustBasedOnHeight);
		}
		if (num4)
		{
			list2.Add(AdjustBasedOnRotate);
		}
		if (list2.Count > 0)
		{
			(sbyte, sbyte) tuple = list2.GetRandom(random)();
			sbyte item = tuple.Item1;
			sbyte item2 = tuple.Item2;
			eyesIndexTuple.Item1 = item;
			eyesIndexTuple.Item2 = item2;
			return true;
		}
		eyesIndexTuple.Item1 = (sbyte)((!random.CheckPercentProb(50)) ? 4 : 0);
		eyesIndexTuple.Item2 = (sbyte)((!random.CheckPercentProb(50)) ? 4 : 0);
		sbyte b = (sbyte)((!random.CheckPercentProb(50)) ? 4 : 0);
		float[] array2 = charmDistanceDivideInfo[eyesIndexTuple.Item1];
		float[] array3 = charmHeightDivideInfo[eyesIndexTuple.Item2];
		float[] array4 = charmRotateDivideInfo[b];
		float num5 = Math.Max(array2[0], array2[1]);
		float num6 = Math.Min(array2[0], array2[1]);
		float num7 = Math.Max(array3[0], array3[1]);
		float num8 = Math.Min(array3[0], array3[1]);
		float num9 = Math.Max(array4[0], array4[1]);
		float num10 = Math.Min(array4[0], array4[1]);
		float num11 = num5 * num7 * num9;
		float num12 = num6 * num8 * num10;
		if (IsInRange(new float[2] { num11, num12 }, charmPercent))
		{
			double num13 = random.NextDouble() * (double)(num5 - num6) + (double)num6;
			sbyte resultIndex;
			double areaLerpValueByPercent = GetAreaLerpValueByPercent(random, charmDistanceDivideInfo, num13, 2, out resultIndex, (eyesIndexTuple.Item1 != 0) ? ((sbyte)1) : ((sbyte)0));
			EyesDistancePercent = (short)(areaLerpValueByPercent * 100.0);
			double num14 = charmPercent / num13;
			double num15 = random.NextDouble() * (num14 - (double)num8) + (double)num8;
			double areaLerpValueByPercent2 = GetAreaLerpValueByPercent(random, charmHeightDivideInfo, num15, 2, out resultIndex, (eyesIndexTuple.Item2 != 0) ? ((sbyte)1) : ((sbyte)0));
			EyesHeightPercent = (short)(areaLerpValueByPercent2 * 100.0);
			double percent = num14 / num15;
			double areaLerpValueByPercent3 = GetAreaLerpValueByPercent(random, charmRotateDivideInfo, percent, 2, out resultIndex, (b != 0) ? ((sbyte)1) : ((sbyte)0));
			EyesAngle = (short)(((double)rotateRange[0] + (double)(rotateRange[1] - rotateRange[0]) * areaLerpValueByPercent3) * 100.0);
			return true;
		}
		return false;
		(sbyte, sbyte) AdjustBasedOnDistance()
		{
			sbyte resultIndex2;
			double areaLerpValueByPercent4 = GetAreaLerpValueByPercent(random, charmDistanceDivideInfo, charmPercent, 2, out resultIndex2, -1);
			EyesDistancePercent = (short)(areaLerpValueByPercent4 * 100.0);
			EyesHeightPercent = 50;
			return (resultIndex2, 2);
		}
		(sbyte, sbyte) AdjustBasedOnHeight()
		{
			sbyte resultIndex2;
			double areaLerpValueByPercent4 = GetAreaLerpValueByPercent(random, charmHeightDivideInfo, charmPercent, 2, out resultIndex2, -1);
			EyesHeightPercent = (short)(areaLerpValueByPercent4 * 100.0);
			EyesDistancePercent = 50;
			return (2, resultIndex2);
		}
		(sbyte, sbyte) AdjustBasedOnRotate()
		{
			sbyte resultIndex2;
			double areaLerpValueByPercent4 = GetAreaLerpValueByPercent(random, charmRotateDivideInfo, charmPercent, 2, out resultIndex2, -1);
			EyesAngle = (short)(((double)rotateRange[0] + (double)(rotateRange[1] - rotateRange[0]) * areaLerpValueByPercent4) * 100.0);
			EyesDistancePercent = 50;
			EyesHeightPercent = 50;
			return (2, 2);
		}
	}

	private bool AdjustNose(IRandomSource random, double charm, (sbyte heightIndex, sbyte distanceIndex) eyesIndexTuple)
	{
		charm /= (double)GlobalConfig.Instance.NoseRatioInBaseCharm;
		List<AvatarAsset> list = new List<AvatarAsset>();
		double num = double.MaxValue;
		for (int i = 0; i < _avatarGroup.NoseRes.Count; i++)
		{
			double num2 = (double)_avatarGroup.NoseRes[i].Config.ElemCharm - charm;
			if (list.Count <= 0)
			{
				list.Add(_avatarGroup.NoseRes[i]);
				num = num2;
				continue;
			}
			if (Math.Abs(num2 - num) < 0.20000000298023224)
			{
				list.Add(_avatarGroup.NoseRes[i]);
				continue;
			}
			bool flag = Math.Abs(num2) < Math.Abs(num);
			if (flag)
			{
				if (num2 < 0.0 && num > 0.0 && num < Math.Abs(num2) * 2.0)
				{
					flag = false;
				}
			}
			else if (num2 > 0.0 && num < 0.0 && num2 < Math.Abs(num) * 2.0)
			{
				flag = true;
			}
			if (flag)
			{
				num = num2;
				list.Clear();
				list.Add(_avatarGroup.NoseRes[i]);
			}
		}
		if (list.Count <= 0)
		{
			return false;
		}
		AvatarAsset random2 = list.GetRandom(random);
		double percent = charm / (double)random2.Config.ElemCharm;
		List<float[]> charmNoseHeight = GlobalConfig.Instance.CharmNoseHeight;
		sbyte resultIndex;
		double areaLerpValueByPercent = GetAreaLerpValueByPercent(random, charmNoseHeight, percent, 1, out resultIndex, -1);
		NoseId = random2.Id;
		NoseHeightPercent = (short)(areaLerpValueByPercent * 100.0);
		return true;
	}

	private bool AdjustMouth(IRandomSource random, double charm)
	{
		charm /= (double)GlobalConfig.Instance.MouthRatioInBaseCharm;
		List<MouthRes> list = new List<MouthRes>();
		double num = double.MaxValue;
		for (int i = 0; i < _avatarGroup.MouthRes.Count; i++)
		{
			double num2 = (double)_avatarGroup.MouthRes[i].Mouth.Config.ElemCharm - charm;
			if (list.Count <= 0)
			{
				num = num2;
				list.Add(_avatarGroup.MouthRes[i]);
				continue;
			}
			if (Math.Abs(num2 - num) < 0.20000000298023224)
			{
				list.Add(_avatarGroup.MouthRes[i]);
				continue;
			}
			bool flag = Math.Abs(num2) < Math.Abs(num);
			if (flag)
			{
				if (num2 < 0.0 && num > 0.0 && num < Math.Abs(num2) * 2.0)
				{
					flag = false;
				}
			}
			else if (num2 > 0.0 && num < 0.0 && num2 < Math.Abs(num) * 2.0)
			{
				flag = true;
			}
			if (flag)
			{
				list.Clear();
				num = num2;
				list.Add(_avatarGroup.MouthRes[i]);
			}
		}
		if (list.Count <= 0)
		{
			return false;
		}
		MouthRes random2 = list.GetRandom(random);
		double percent = charm / (double)random2.Mouth.Config.ElemCharm;
		List<float[]> charmMouthHeight = GlobalConfig.Instance.CharmMouthHeight;
		sbyte resultIndex;
		double areaLerpValueByPercent = GetAreaLerpValueByPercent(random, charmMouthHeight, percent, 2, out resultIndex, -1);
		MouthId = random2.Id;
		MouthHeightPercent = (short)(areaLerpValueByPercent * 100.0);
		return true;
	}

	public double GetCharmRate(short charmPercent, List<float[]> divideInfo, int bestAreaIndex)
	{
		double num = 100.0 / (double)divideInfo.Count;
		int num2 = (int)Math.Floor((double)charmPercent / num);
		if (num2 >= divideInfo.Count)
		{
			return divideInfo[divideInfo.Count - 1][1];
		}
		float num3 = Math.Abs(divideInfo[num2][1] - divideInfo[num2][0]);
		double num4 = ((double)charmPercent - num * (double)num2) / num;
		if (num2 == bestAreaIndex)
		{
			if (Math.Abs(num3) < 0.001f)
			{
				return divideInfo[num2][0];
			}
			if (num4 <= 0.5)
			{
				return (double)divideInfo[num2][0] + (double)num3 * (num4 * 2.0);
			}
			return (double)divideInfo[num2][1] - (double)num3 * (num4 - 0.5) * 2.0;
		}
		if (divideInfo[num2][0] <= divideInfo[num2][1])
		{
			return (double)divideInfo[num2][0] + (double)num3 * num4;
		}
		return (double)divideInfo[num2][0] - (double)num3 * num4;
	}

	private (sbyte, double) GetAreaPartInfo(float rangeMin, float rangeMax, float checkValue, int dividePartCount, int bestAreaIndex)
	{
		sbyte b = -1;
		double num = 0.0;
		double num2 = (rangeMax - rangeMin) / (float)dividePartCount;
		for (sbyte b2 = 0; b2 < dividePartCount; b2++)
		{
			double num3 = (double)rangeMin + num2 * (double)b2;
			double num4 = num3 + num2;
			if ((double)checkValue >= num3 && (double)checkValue <= num4)
			{
				b = b2;
				num = ((double)checkValue - num3) / num2;
				break;
			}
		}
		if (-1 == b)
		{
			b = (sbyte)(dividePartCount - 1);
			num = 1.0;
		}
		if (b == bestAreaIndex)
		{
			num = ((num < 0.47999998927116394) ? (num * 2.0) : ((!(num > 0.5199999809265137)) ? 1.0 : ((1.0 - num) * 0.5)));
		}
		return (b, num);
	}

	private bool IsInRange(float[] rangeInfo, double checkValue)
	{
		if (checkValue >= (double)rangeInfo[0] && checkValue < (double)rangeInfo[1])
		{
			return true;
		}
		if (checkValue >= (double)rangeInfo[1] && checkValue < (double)rangeInfo[0])
		{
			return true;
		}
		return false;
	}

	private double GetAreaLerpValueByPercent(IRandomSource random, List<float[]> divideInfo, double percent, sbyte bestAreaIndex, out sbyte resultIndex, sbyte searchDirection = -1)
	{
		if (divideInfo.CheckIndex(bestAreaIndex))
		{
			float num = Math.Max(divideInfo[bestAreaIndex][0], divideInfo[bestAreaIndex][1]);
			if (percent >= (double)num)
			{
				resultIndex = bestAreaIndex;
				return ((float)bestAreaIndex + 0.5f) / (float)divideInfo.Count;
			}
		}
		resultIndex = -1;
		double num2 = 0.0;
		bool odd = random.CheckPercentProb(50);
		if (-1 != searchDirection)
		{
			odd = searchDirection == 0;
		}
		sbyte index = (sbyte)((!odd) ? (divideInfo.Count - 1) : 0);
		sbyte passAreaCount = 0;
		while (LoopComplete())
		{
			if (IsInRange(divideInfo[index], percent))
			{
				resultIndex = index;
				float num3 = Math.Min(divideInfo[index][0], divideInfo[index][1]);
				float num4 = Math.Max(divideInfo[index][0], divideInfo[index][1]);
				if (index != bestAreaIndex)
				{
					if (Math.Abs(percent - (double)num3) < 0.0010000000474974513)
					{
						num2 = (double)passAreaCount / (double)divideInfo.Count;
						break;
					}
					if (Math.Abs(percent - (double)num4) < 0.0010000000474974513)
					{
						num2 = (double)(passAreaCount + 1) / (double)divideInfo.Count;
						break;
					}
				}
				num2 = (percent - (double)num3) / (double)(num4 - num3);
				if (num2 < 0.0)
				{
					AdaptableLog.Warning("算法错误！区域插值不可以是0！");
				}
				if (index == bestAreaIndex)
				{
					num2 *= 0.5;
				}
				num2 = ((double)passAreaCount + num2) / (double)divideInfo.Count;
				resultIndex = index;
				break;
			}
			LoopNext();
		}
		if (!odd)
		{
			num2 = 1.0 - num2;
		}
		return num2;
		bool LoopComplete()
		{
			if (odd)
			{
				return index < divideInfo.Count;
			}
			return index >= 0;
		}
		void LoopNext()
		{
			if (odd)
			{
				index++;
			}
			else
			{
				index--;
			}
			passAreaCount++;
		}
	}
}
