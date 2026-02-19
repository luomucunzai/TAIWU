using System;
using System.Collections.Generic;
using GameData.Utilities;
using Redzen.Random;

namespace GameData.Domains.Character.AvatarSystem.AvatarRes;

public class AvatarGroup
{
	public byte Id;

	public bool HasAsset;

	public bool AvatarAvailable;

	public List<BodyRes> BodyRes;

	public List<AvatarAsset> HeadRes;

	public List<EyeRes> EyesGroup;

	public List<AvatarAsset> EyesRes;

	private readonly Dictionary<string, AvatarAsset> _eyeballsRes;

	public List<AvatarAsset> EyeBrowRes;

	public List<AvatarAsset> NoseRes;

	public List<MouthRes> MouthRes;

	public List<HairRes> Hair1Res;

	public List<HairRes> Hair2Res;

	public List<AvatarAsset> Beard1Res;

	public List<AvatarAsset> Beard2Res;

	public List<AvatarAsset> Feature1Res;

	public List<AvatarAsset> Feature2Res;

	public List<AvatarAsset> Wrinkle1Res;

	public List<AvatarAsset> Wrinkle2Res;

	public List<AvatarAsset> Wrinkle3Res;

	public AvatarAsset WorstFeature2;

	public const short ObsoletedFeatureId = 6;

	public AvatarGroup()
	{
		BodyRes = new List<BodyRes>();
		HeadRes = new List<AvatarAsset>();
		EyesRes = new List<AvatarAsset>();
		_eyeballsRes = new Dictionary<string, AvatarAsset>();
		EyeBrowRes = new List<AvatarAsset>();
		NoseRes = new List<AvatarAsset>();
		MouthRes = new List<MouthRes>();
		Beard1Res = new List<AvatarAsset>();
		Beard2Res = new List<AvatarAsset>();
		Hair1Res = new List<HairRes>();
		Hair2Res = new List<HairRes>();
		Feature1Res = new List<AvatarAsset>();
		Feature2Res = new List<AvatarAsset>();
		Wrinkle1Res = new List<AvatarAsset>();
		Wrinkle2Res = new List<AvatarAsset>();
		Wrinkle3Res = new List<AvatarAsset>();
	}

	private BodyRes EnsureBody(short bodyId)
	{
		BodyRes bodyRes = BodyRes.Find((BodyRes e) => e.Id == bodyId);
		if (bodyRes == null)
		{
			bodyRes = new BodyRes
			{
				Id = bodyId,
				ClothParts = new List<AvatarAsset>()
			};
			BodyRes.Add(bodyRes);
		}
		return bodyRes;
	}

	private MouthRes EnsureMouth(short mouthId)
	{
		MouthRes mouthRes = MouthRes.Find((MouthRes e) => e.Id == mouthId);
		if (mouthRes == null)
		{
			mouthRes = new MouthRes
			{
				Id = mouthId
			};
			MouthRes.Add(mouthRes);
		}
		return mouthRes;
	}

	private HairRes EnsureHair1(short hairId)
	{
		HairRes hairRes = Hair1Res.Find((HairRes e) => e.Id == hairId);
		if (hairRes == null)
		{
			hairRes = new HairRes
			{
				Id = hairId
			};
			Hair1Res.Add(hairRes);
		}
		return hairRes;
	}

	private HairRes EnsureHair2(short hairId)
	{
		HairRes hairRes = Hair2Res.Find((HairRes e) => e.Id == hairId);
		if (hairRes == null)
		{
			hairRes = new HairRes
			{
				Id = hairId
			};
			Hair2Res.Add(hairRes);
		}
		return hairRes;
	}

	public void Add(AvatarAsset asset)
	{
		if (asset == null)
		{
			return;
		}
		HasAsset = true;
		switch (asset.Type)
		{
		case EAvatarElementsType.Cloth:
			EnsureBody(asset.Id).Cloth = asset;
			break;
		case EAvatarElementsType.ClothColor:
			EnsureBody(asset.Id).Color = asset;
			break;
		case EAvatarElementsType.ClothSkin:
			EnsureBody(asset.Id).Skin = asset;
			break;
		case EAvatarElementsType.ClothPart:
		{
			List<AvatarAsset> clothParts = EnsureBody(asset.Id).ClothParts;
			AvatarAsset avatarAsset8 = clothParts.Find((AvatarAsset e) => e.Id == asset.Id);
			if (avatarAsset8 == null)
			{
				clothParts.Add(asset);
			}
			else
			{
				clothParts[clothParts.IndexOf(avatarAsset8)] = asset;
			}
			break;
		}
		case EAvatarElementsType.Head:
		{
			AvatarAsset avatarAsset12 = HeadRes.Find((AvatarAsset e) => e.Id == asset.Id);
			if (avatarAsset12 == null)
			{
				HeadRes.Add(asset);
			}
			else
			{
				HeadRes[HeadRes.IndexOf(avatarAsset12)] = asset;
			}
			break;
		}
		case EAvatarElementsType.Eye:
		{
			AvatarAsset avatarAsset7 = EyesRes.Find((AvatarAsset e) => e.Id == asset.Id && e.SubId == asset.SubId);
			if (avatarAsset7 == null)
			{
				EyesRes.Add(asset);
			}
			else
			{
				EyesRes[EyesRes.IndexOf(avatarAsset7)] = asset;
			}
			break;
		}
		case EAvatarElementsType.EyeBall:
		{
			string key = $"{asset.Id}_{asset.SubId}";
			if (!_eyeballsRes.ContainsKey(key))
			{
				_eyeballsRes.Add(key, asset);
			}
			else
			{
				_eyeballsRes[key] = asset;
			}
			break;
		}
		case EAvatarElementsType.EyeBrow:
		{
			AvatarAsset avatarAsset10 = EyeBrowRes.Find((AvatarAsset e) => e.Id == asset.Id);
			if (avatarAsset10 == null)
			{
				EyeBrowRes.Add(asset);
			}
			else
			{
				EyeBrowRes[EyeBrowRes.IndexOf(avatarAsset10)] = asset;
			}
			break;
		}
		case EAvatarElementsType.Nose:
		{
			AvatarAsset avatarAsset5 = NoseRes.Find((AvatarAsset e) => e.Id == asset.Id);
			if (avatarAsset5 == null)
			{
				NoseRes.Add(asset);
			}
			else
			{
				NoseRes[NoseRes.IndexOf(avatarAsset5)] = asset;
			}
			break;
		}
		case EAvatarElementsType.Mouth:
			EnsureMouth(asset.Id).Mouth = asset;
			break;
		case EAvatarElementsType.MouthPart:
			EnsureMouth(asset.Id).MouthPart = asset;
			break;
		case EAvatarElementsType.Beard1:
		{
			AvatarAsset avatarAsset2 = Beard1Res.Find((AvatarAsset e) => e.Id == asset.Id);
			if (avatarAsset2 == null)
			{
				Beard1Res.Add(asset);
			}
			else
			{
				Beard1Res[Beard1Res.IndexOf(avatarAsset2)] = asset;
			}
			break;
		}
		case EAvatarElementsType.Beard2:
		{
			AvatarAsset avatarAsset11 = Beard2Res.Find((AvatarAsset e) => e.Id == asset.Id);
			if (avatarAsset11 == null)
			{
				Beard2Res.Add(asset);
			}
			else
			{
				Beard2Res[Beard2Res.IndexOf(avatarAsset11)] = asset;
			}
			break;
		}
		case EAvatarElementsType.Hair1:
			EnsureHair1(asset.Id).Hair = asset;
			break;
		case EAvatarElementsType.Hair1Part:
			EnsureHair1(asset.Id).HairPart = asset;
			break;
		case EAvatarElementsType.Hair2:
			EnsureHair2(asset.Id).Hair = asset;
			break;
		case EAvatarElementsType.Hair2Part:
			EnsureHair2(asset.Id).HairPart = asset;
			break;
		case EAvatarElementsType.Feature1:
		{
			AvatarAsset avatarAsset9 = Feature1Res.Find((AvatarAsset e) => e.Id == asset.Id);
			if (avatarAsset9 == null)
			{
				Feature1Res.Add(asset);
			}
			else
			{
				Feature1Res[Feature1Res.IndexOf(avatarAsset9)] = asset;
			}
			break;
		}
		case EAvatarElementsType.Feature2:
		{
			AvatarAsset avatarAsset6 = Feature2Res.Find((AvatarAsset e) => e.Id == asset.Id);
			if (avatarAsset6 == null)
			{
				Feature2Res.Add(asset);
			}
			else
			{
				Feature2Res[Feature2Res.IndexOf(avatarAsset6)] = asset;
			}
			break;
		}
		case EAvatarElementsType.Wrinkle1:
		{
			AvatarAsset avatarAsset4 = Wrinkle1Res.Find((AvatarAsset e) => e.Id == asset.Id);
			if (avatarAsset4 == null)
			{
				Wrinkle1Res.Add(asset);
			}
			else
			{
				Wrinkle1Res[Wrinkle1Res.IndexOf(avatarAsset4)] = asset;
			}
			break;
		}
		case EAvatarElementsType.Wrinkle2:
		{
			AvatarAsset avatarAsset3 = Wrinkle2Res.Find((AvatarAsset e) => e.Id == asset.Id);
			if (avatarAsset3 == null)
			{
				Wrinkle2Res.Add(asset);
			}
			else
			{
				Wrinkle2Res[Wrinkle2Res.IndexOf(avatarAsset3)] = asset;
			}
			break;
		}
		case EAvatarElementsType.Wrinkle3:
		{
			AvatarAsset avatarAsset = Wrinkle3Res.Find((AvatarAsset e) => e.Id == asset.Id);
			if (avatarAsset == null)
			{
				Wrinkle3Res.Add(asset);
			}
			else
			{
				Wrinkle3Res[Wrinkle3Res.IndexOf(avatarAsset)] = asset;
			}
			break;
		}
		}
	}

	public void ConstructEyesGroup()
	{
		EyesGroup = new List<EyeRes>();
		int i = 0;
		while (i < EyesRes.Count)
		{
			AvatarAsset avatarAsset = EyesRes[i];
			string key = $"{avatarAsset.Id}_{avatarAsset.SubId}";
			_eyeballsRes.TryGetValue(key, out var value);
			EyeRes eyeRes = new EyeRes();
			eyeRes.Id = avatarAsset.Id;
			eyeRes.LeftEye = EyesRes[i];
			eyeRes.RightEye = EyesRes[i];
			eyeRes.LeftEyeball = value;
			eyeRes.RightEyeball = value;
			EyesGroup.Add(eyeRes);
			for (i++; i < EyesRes.Count && EyesRes[i].Id == avatarAsset.Id; i++)
			{
				AvatarAsset avatarAsset2 = EyesRes[i];
				string key2 = $"{avatarAsset2.Id}_{avatarAsset2.SubId}";
				_eyeballsRes.TryGetValue(key2, out var value2);
				EyeRes eyeRes2 = new EyeRes();
				eyeRes2.Id = avatarAsset.Id;
				eyeRes2.LeftEye = avatarAsset;
				eyeRes2.RightEye = avatarAsset2;
				eyeRes2.LeftEyeball = value;
				eyeRes2.RightEyeball = value2;
				EyesGroup.Add(eyeRes2);
				EyeRes eyeRes3 = new EyeRes();
				eyeRes3.Id = avatarAsset.Id;
				eyeRes3.LeftEye = avatarAsset2;
				eyeRes3.RightEye = avatarAsset;
				eyeRes3.LeftEyeball = value2;
				eyeRes3.RightEyeball = value;
				EyesGroup.Add(eyeRes3);
			}
		}
	}

	public void Sort()
	{
		BodyRes.Sort((BodyRes l, BodyRes r) => l.Id - r.Id);
		HeadRes.Sort((AvatarAsset l, AvatarAsset r) => l.Id - r.Id);
		EyesRes.Sort((AvatarAsset l, AvatarAsset r) => (l.Id == r.Id) ? (l.SubId - r.SubId) : (l.Id - r.Id));
		NoseRes.Sort((AvatarAsset l, AvatarAsset r) => l.Id - r.Id);
		MouthRes.Sort((MouthRes l, MouthRes r) => l.Id - r.Id);
		Hair1Res.Sort((HairRes l, HairRes r) => l.Id - r.Id);
		Hair2Res.Sort((HairRes l, HairRes r) => l.Id - r.Id);
		Beard1Res.Sort((AvatarAsset l, AvatarAsset r) => l.Id - r.Id);
		Beard2Res.Sort((AvatarAsset l, AvatarAsset r) => l.Id - r.Id);
		Feature1Res.Sort((AvatarAsset l, AvatarAsset r) => l.Id - r.Id);
		Feature2Res.Sort((AvatarAsset l, AvatarAsset r) => l.Id - r.Id);
		Wrinkle1Res.Sort((AvatarAsset l, AvatarAsset r) => l.Id - r.Id);
		Wrinkle2Res.Sort((AvatarAsset l, AvatarAsset r) => l.Id - r.Id);
		Wrinkle3Res.Sort((AvatarAsset l, AvatarAsset r) => l.Id - r.Id);
		WorstFeature2 = null;
		foreach (AvatarAsset feature2Re in Feature2Res)
		{
			if (WorstFeature2 == null || WorstFeature2.Config.CharmExtraArg > feature2Re.Config.CharmExtraArg)
			{
				WorstFeature2 = feature2Re;
			}
		}
	}

	private float GetRandomFloat(IRandomSource random, float min, float max)
	{
		return min + (float)random.NextDouble() * (max - min);
	}

	private int GetRandomInt(IRandomSource random, int min, int max)
	{
		return random.Next(min, max);
	}

	public AvatarAsset Get(EAvatarElementsType elemType, params short[] ids)
	{
		AvatarAsset result = null;
		int id = ids[0];
		int subId = ((ids.Length == 2) ? ids[1] : 0);
		switch (elemType)
		{
		case EAvatarElementsType.Cloth:
			result = BodyRes.Find((BodyRes e) => e.Id == id)?.Cloth;
			break;
		case EAvatarElementsType.ClothColor:
			result = BodyRes.Find((BodyRes e) => e.Id == id)?.Color;
			break;
		case EAvatarElementsType.ClothSkin:
			result = BodyRes.Find((BodyRes e) => e.Id == id)?.Skin;
			break;
		case EAvatarElementsType.ClothPart:
		{
			BodyRes bodyRes = BodyRes.Find((BodyRes e) => e.Id == id);
			if (bodyRes != null && subId != 0)
			{
				result = bodyRes.ClothParts.Find(FuncFind);
			}
			break;
		}
		case EAvatarElementsType.Head:
			result = HeadRes.Find(FuncFind);
			break;
		case EAvatarElementsType.Eye:
			result = ((subId != 0) ? EyesRes.Find((AvatarAsset e) => e.Id == id && e.SubId == subId) : EyesRes.Find(FuncFind));
			break;
		case EAvatarElementsType.EyeBall:
		{
			_eyeballsRes.TryGetValue($"{id}_{subId}", out var value);
			result = value;
			break;
		}
		case EAvatarElementsType.EyeBrow:
			result = EyeBrowRes.Find(FuncFind);
			break;
		case EAvatarElementsType.Nose:
			result = NoseRes.Find(FuncFind);
			break;
		case EAvatarElementsType.Mouth:
			result = MouthRes.Find((MouthRes e) => e.Id == id)?.Mouth;
			break;
		case EAvatarElementsType.MouthPart:
			result = MouthRes.Find((MouthRes e) => e.Id == id)?.MouthPart;
			break;
		case EAvatarElementsType.Beard1:
			result = Beard1Res.Find(FuncFind);
			break;
		case EAvatarElementsType.Beard2:
			result = Beard2Res.Find(FuncFind);
			break;
		case EAvatarElementsType.Hair1:
			result = Hair1Res.Find((HairRes e) => e.Id == id)?.Hair;
			break;
		case EAvatarElementsType.Hair1Part:
			result = Hair1Res.Find((HairRes e) => e.Id == id)?.HairPart;
			break;
		case EAvatarElementsType.Hair2:
			result = Hair2Res.Find((HairRes e) => e.Id == id)?.Hair;
			break;
		case EAvatarElementsType.Hair2Part:
			result = Hair2Res.Find((HairRes e) => e.Id == id)?.HairPart;
			break;
		case EAvatarElementsType.Feature1:
			result = Feature1Res.Find(FuncFind);
			break;
		case EAvatarElementsType.Feature2:
			result = Feature2Res.Find(FuncFind);
			break;
		case EAvatarElementsType.Wrinkle1:
			result = Wrinkle1Res.Find(FuncFind);
			break;
		case EAvatarElementsType.Wrinkle2:
			result = Wrinkle2Res.Find(FuncFind);
			break;
		case EAvatarElementsType.Wrinkle3:
			result = Wrinkle3Res.Find(FuncFind);
			break;
		default:
			return null;
		}
		return result;
		bool FuncFind(AvatarAsset asset)
		{
			return asset.Id == id;
		}
	}

	public int GetTypeCount(EAvatarElementsType type)
	{
		int result = 0;
		switch (type)
		{
		case EAvatarElementsType.Cloth:
			result = BodyRes.Count;
			break;
		case EAvatarElementsType.Head:
			result = HeadRes.Count;
			break;
		case EAvatarElementsType.Eye:
			result = EyesGroup.Count;
			break;
		case EAvatarElementsType.EyeBrow:
			result = EyeBrowRes.Count;
			break;
		case EAvatarElementsType.Nose:
			result = NoseRes.Count;
			break;
		case EAvatarElementsType.Mouth:
			result = MouthRes.Count;
			break;
		case EAvatarElementsType.Beard1:
			result = Beard1Res.Count;
			break;
		case EAvatarElementsType.Beard2:
			result = Beard2Res.Count;
			break;
		case EAvatarElementsType.Hair1:
			result = Hair1Res.Count;
			break;
		case EAvatarElementsType.Hair2:
			result = Hair2Res.Count;
			break;
		case EAvatarElementsType.Feature1:
			result = Feature1Res.Count;
			break;
		case EAvatarElementsType.Feature2:
			result = Feature2Res.Count;
			break;
		case EAvatarElementsType.Wrinkle1:
			result = Wrinkle1Res.Count;
			break;
		case EAvatarElementsType.Wrinkle2:
			result = Wrinkle2Res.Count;
			break;
		case EAvatarElementsType.Wrinkle3:
			result = Wrinkle3Res.Count;
			break;
		}
		return result;
	}

	public int GetClothNPartCount(int clothId)
	{
		return BodyRes.Find((BodyRes e) => e.Id == clothId)?.ClothParts.Count ?? 0;
	}

	public (AvatarData avatar, short clothId) GetRandomAvatar(IRandomSource random, bool randColor = true, bool randCloth = false, bool canCreateOnly = false, bool canNaked = false)
	{
		AvatarData avatarData = new AvatarData();
		avatarData.AvatarId = Id;
		if (!AvatarManager.Instance.DisplayMode)
		{
			avatarData.ChildClothId = AvatarManager.Instance.GetRandomChildClothIdByAvatarId(random, Id);
		}
		short num = 0;
		if (HasAsset)
		{
			if (randCloth)
			{
				num = GetRandomCloth(random, canCreateOnly, canNaked);
				avatarData.ClothPartId = GetRandomClothPart(random, num);
			}
			avatarData.HeadId = (byte)GetRandomHead(random);
			AvatarAsset headAsset = HeadRes.Find((AvatarAsset e) => e.Id == avatarData.HeadId);
			(short, short, short, short) randomEyes = GetRandomEyes(random);
			avatarData.EyesMainId = randomEyes.Item1;
			avatarData.EyesLeftId = randomEyes.Item2;
			avatarData.EyesRightId = randomEyes.Item3;
			avatarData.EyebrowId = randomEyes.Item4;
			(float, float, float, float, float, float, float, float) randomEyeInfos = GetRandomEyeInfos(random, headAsset, EyesGroup.Find((EyeRes e) => e.Id == avatarData.EyesMainId));
			avatarData.EyesHeightPercent = (short)(randomEyeInfos.Item1 * 100f);
			avatarData.EyesDistancePercent = (short)(randomEyeInfos.Item2 * 100f);
			avatarData.EyesAngle = (short)(randomEyeInfos.Item3 * 100f);
			avatarData.EyesScale = (short)(randomEyeInfos.Item4 * 100f);
			avatarData.EyebrowHeight = (short)(randomEyeInfos.Item5 * 100f);
			avatarData.EyebrowDistancePercent = (short)(randomEyeInfos.Item6 * 100f);
			avatarData.EyebrowAngle = (short)(randomEyeInfos.Item7 * 100f);
			avatarData.EyebrowScale = (short)(randomEyeInfos.Rest.Item1 * 100f);
			avatarData.MouthId = GetRandomMouth(random);
			(float, float) randomMouthInfos = GetRandomMouthInfos(random, headAsset, MouthRes.Find((MouthRes e) => e.Id == avatarData.MouthId)?.Mouth);
			avatarData.MouthHeightPercent = (short)(randomMouthInfos.Item1 * 100f);
			avatarData.MouthScale = (short)(randomMouthInfos.Item2 * 100f);
			avatarData.NoseId = GetRandomNose(random);
			(float, float) randomNoseInfos = GetRandomNoseInfos(random, avatarData, headAsset);
			avatarData.NoseHeightPercent = (short)(randomNoseInfos.Item1 * 100f);
			avatarData.NoseScale = (short)(randomNoseInfos.Item2 * 100f);
			(short, short) randomBeards = GetRandomBeards(random);
			avatarData.Beard1Id = randomBeards.Item1;
			avatarData.Beard2Id = randomBeards.Item2;
			(short, short) randomHairs = GetRandomHairs(random);
			avatarData.FrontHairId = randomHairs.Item1;
			avatarData.BackHairId = randomHairs.Item2;
			(short, short, short, short, short) randomMaskElems = GetRandomMaskElems(random);
			avatarData.Feature1Id = randomMaskElems.Item1;
			avatarData.Feature2Id = randomMaskElems.Item2;
			avatarData.Wrinkle1Id = randomMaskElems.Item3;
			avatarData.Wrinkle2Id = randomMaskElems.Item4;
			avatarData.Wrinkle3Id = randomMaskElems.Item5;
			if (avatarData.Gender == 1 && (avatarData.Beard1Id == 0 || avatarData.Beard2Id == 0))
			{
				throw new Exception($"AvatarGroup {avatarData.AvatarId} generate error:Get beard id error! beardId_1 = {avatarData.Beard1Id}  beardId_2 = {avatarData.Beard2Id}");
			}
			if (randColor)
			{
				AvatarManager instance = AvatarManager.Instance;
				Func<IRandomSource, IList<byte[]>, int, byte[]> func = RandomUtils.GenerateRandomWeightCell;
				int colorWeightIndex = instance.GetColorWeightIndex();
				byte[] array = func(random, instance.SkinColorsWeight, colorWeightIndex);
				avatarData.ColorSkinId = array[0];
				byte[] array2 = func(random, instance.LipColorsWeight, colorWeightIndex);
				avatarData.ColorMouthId = array2[0];
				byte[] array3 = func(random, instance.HairColorsWeight, colorWeightIndex);
				avatarData.ColorFrontHairId = array3[0];
				avatarData.ColorBackHairId = avatarData.ColorFrontHairId;
				avatarData.ColorBeard1Id = avatarData.ColorFrontHairId;
				avatarData.ColorBeard2Id = avatarData.ColorFrontHairId;
				avatarData.ColorEyebrowId = avatarData.ColorFrontHairId;
				if (random.CheckPercentProb(GlobalConfig.Instance.AvatarFurColorSplitObb))
				{
					int num2 = random.Next(100);
					byte[] avatarFurColorSplitObbArray = GlobalConfig.Instance.AvatarFurColorSplitObbArray;
					if (num2 < avatarFurColorSplitObbArray[0])
					{
						array3 = func(random, instance.HairColorsWeight, colorWeightIndex);
						avatarData.ColorBeard1Id = array3[0];
						avatarData.ColorBeard2Id = array3[0];
					}
					else if (num2 < avatarFurColorSplitObbArray[0] + avatarFurColorSplitObbArray[1])
					{
						byte[] array4 = func(random, instance.HairColorsWeight, colorWeightIndex);
						avatarData.ColorEyebrowId = array4[0];
					}
					else if (num2 < avatarFurColorSplitObbArray[0] + avatarFurColorSplitObbArray[1] + avatarFurColorSplitObbArray[2])
					{
						array3 = func(random, instance.HairColorsWeight, colorWeightIndex);
						avatarData.ColorFrontHairId = array3[0];
						avatarData.ColorBackHairId = avatarData.ColorFrontHairId;
					}
					else
					{
						avatarData.ColorBackHairId = func(random, instance.HairColorsWeight, colorWeightIndex)[0];
						avatarData.ColorBeard1Id = func(random, instance.HairColorsWeight, colorWeightIndex)[0];
						avatarData.ColorBeard2Id = func(random, instance.HairColorsWeight, colorWeightIndex)[0];
						avatarData.ColorEyebrowId = func(random, instance.HairColorsWeight, colorWeightIndex)[0];
					}
				}
				byte[] array5 = func(random, instance.FeatureColorsWeight, colorWeightIndex);
				avatarData.ColorFeature1Id = array5[0];
				byte[] array6 = func(random, instance.EyeballColorsWeight, colorWeightIndex);
				avatarData.ColorEyeballId = array6[0];
				byte[] array7 = func(random, instance.ClothColorsWeight, colorWeightIndex);
				avatarData.ColorClothId = array7[0];
			}
		}
		return (avatar: avatarData, clothId: num);
	}

	public short GetRandomHead(IRandomSource random)
	{
		List<AvatarAsset> list = new List<AvatarAsset>();
		int i = 0;
		for (int count = HeadRes.Count; i < count; i++)
		{
			AvatarAsset avatarAsset = HeadRes[i];
			if (avatarAsset.HeadConfig.CanRandom)
			{
				list.Add(avatarAsset);
			}
		}
		return list.GetRandom(random)?.Id ?? 0;
	}

	public short GetRandomCloth(IRandomSource random, bool canCreateOnly, bool canNaked = false)
	{
		return BodyRes.FindAll(delegate(BodyRes e)
		{
			bool result = true;
			if (!canNaked && e.Cloth.Config.ElementId == 0)
			{
				result = false;
			}
			if (canCreateOnly && !e.Cloth.Config.CanCreate)
			{
				result = false;
			}
			if (e.Cloth.Config.ElementId >= 10000)
			{
				result = false;
			}
			return result;
		}).GetRandom(random)?.Id ?? 0;
	}

	public byte GetRandomClothPart(IRandomSource random, int clothId)
	{
		BodyRes bodyRes = BodyRes.Find((BodyRes e) => e.Id == clothId);
		if (bodyRes != null && bodyRes.ClothParts != null && bodyRes.ClothParts.Count > 0)
		{
			return (byte)bodyRes.ClothParts.GetRandom(random).SubId;
		}
		return 0;
	}

	public short GetRandomMouth(IRandomSource random)
	{
		return MouthRes.GetRandom(random)?.Id ?? 0;
	}

	public (float height, float scale) GetRandomMouthInfos(IRandomSource random, AvatarAsset headAsset, AvatarAsset mouthAsset)
	{
		float item = 0f;
		float num = 0f;
		if (mouthAsset != null && headAsset != null)
		{
			float num2 = 0f;
			List<float> avatarMouthScaleRange = GlobalConfig.Instance.AvatarMouthScaleRange;
			num = GetRandomFloat(random, avatarMouthScaleRange[0], avatarMouthScaleRange[1]);
			float num3 = (float)mouthAsset.Config.SpriteSize[1] * num;
			float num4 = Math.Max((float)(headAsset.HeadConfig.MouthYmax - headAsset.HeadConfig.MouthYmin) - num3, 0f);
			item = GetRandomFloat(random, num2, num4);
			item = (item - num2) / (num4 - num2);
		}
		return (height: item, scale: num);
	}

	public (short mainId, short leftId, short rightId, short eyebrowId) GetRandomEyes(IRandomSource random)
	{
		short item = 0;
		short item2 = 0;
		short item3 = 0;
		short num = 0;
		EyeRes random2 = EyesGroup.GetRandom(random);
		if (random2 != null)
		{
			item = random2.Id;
			item2 = random2.LeftEye.SubId;
			item3 = random2.RightEye.SubId;
		}
		num = EyeBrowRes.GetRandom(random)?.Id ?? 0;
		return (mainId: item, leftId: item2, rightId: item3, eyebrowId: num);
	}

	public (float height, float distance, float angle, float scale, float eyebrowHeight, float eyebrowDistance, float eyebrowAngle, float eyebrowScale) GetRandomEyeInfos(IRandomSource random, AvatarAsset headAsset, EyeRes eyeRes)
	{
		float item = 0f;
		float num = 0f;
		float item2 = 0f;
		float item3 = 0f;
		float item4 = 0f;
		float item5 = 0f;
		float item6 = 0f;
		float item7 = 0f;
		if (headAsset != null && eyeRes != null)
		{
			List<int> avatarEyeRotateRange = GlobalConfig.Instance.AvatarEyeRotateRange;
			item2 = GetRandomInt(random, avatarEyeRotateRange[0], avatarEyeRotateRange[1]);
			List<float> avatarEyesScaleRange = GlobalConfig.Instance.AvatarEyesScaleRange;
			item3 = GetRandomFloat(random, avatarEyesScaleRange[0], avatarEyesScaleRange[1]);
			item = GetRandomFloat(random, 0f, 1f);
			num = GetRandomFloat(random, 0f, 1f);
			List<int> avatarEyebrowRotateRange = GlobalConfig.Instance.AvatarEyebrowRotateRange;
			item6 = GetRandomInt(random, avatarEyebrowRotateRange[0], avatarEyebrowRotateRange[1]);
			List<float> avatarEyebrowScaleRange = GlobalConfig.Instance.AvatarEyebrowScaleRange;
			item7 = GetRandomFloat(random, avatarEyebrowScaleRange[0], avatarEyebrowScaleRange[1]);
			List<float> avatarEyebrowOffsetRange = GlobalConfig.Instance.AvatarEyebrowOffsetRange;
			item4 = GetRandomFloat(random, avatarEyebrowOffsetRange[0], avatarEyebrowOffsetRange[1]);
			item5 = num;
		}
		return (height: item, distance: num, angle: item2, scale: item3, eyebrowHeight: item4, eyebrowDistance: item5, eyebrowAngle: item6, eyebrowScale: item7);
	}

	public short GetRandomNose(IRandomSource random)
	{
		return NoseRes.GetRandom(random)?.Id ?? 0;
	}

	public (float hegiht, float scale) GetRandomNoseInfos(IRandomSource random, AvatarData avatarData, AvatarAsset headAsset)
	{
		AvatarAsset avatarAsset = Get(EAvatarElementsType.Mouth, avatarData.MouthId);
		AvatarAsset avatarAsset2 = Get(EAvatarElementsType.Eye, avatarData.EyesMainId, avatarData.EyesLeftId);
		AvatarAsset avatarAsset3 = Get(EAvatarElementsType.Eye, avatarData.EyesMainId, avatarData.EyesRightId);
		AvatarAsset avatarAsset4 = Get(EAvatarElementsType.Nose, avatarData.NoseId);
		float item = 0f;
		float num = 0f;
		if (headAsset != null)
		{
			List<float> avatarNoseScaleRange = GlobalConfig.Instance.AvatarNoseScaleRange;
			num = GetRandomFloat(random, avatarNoseScaleRange[0], avatarNoseScaleRange[1]);
			List<int[]> avatarNoseHeightRange = GlobalConfig.Instance.AvatarNoseHeightRange;
			int[] array = RandomUtils.GenerateRandomWeightCell(random, avatarNoseHeightRange);
			float num2 = (float)headAsset.HeadConfig.MouthYmin + (float)avatarData.MouthHeightPercent * 0.01f + (float)(avatarAsset.Config.SpriteSize[1] * avatarData.MouthScale) * 0.01f;
			float val = (float)headAsset.HeadConfig.EyesYmin + (float)avatarData.EyesHeightPercent * 0.01f - (float)avatarAsset2.Config.SpriteSize[1] * 0.5f * (float)avatarData.EyesScale * 0.01f;
			float val2 = (float)headAsset.HeadConfig.EyesYmin + (float)avatarData.EyesHeightPercent * 0.01f - (float)avatarAsset3.Config.SpriteSize[1] * 0.5f * (float)avatarData.EyesScale * 0.01f;
			float num3 = Math.Min(val, val2) - (float)avatarAsset4.Config.SpriteSize[1] * 0.5f * num - num2;
			if (num3 > 0f)
			{
				float num4 = num3 / 5f;
				item = num2 + num4 * (float)(array[0] - 1) + GetRandomFloat(random, 0f, num4);
				item = (item - num2) / num3;
			}
		}
		return (hegiht: item, scale: num);
	}

	public (short id1, short id2) GetRandomBeards(IRandomSource random)
	{
		short item = 1;
		short item2 = 1;
		if (random.CheckProb(GlobalConfig.Instance.AvatarNoneBeardObb, 10000))
		{
			return (id1: item, id2: item2);
		}
		List<AvatarAsset> list = ((Beard1Res.Count > 0 || Id % 2 == 0) ? Beard1Res : GetBackupAvatarGroup().Beard1Res);
		item = (short)((list.Count <= 1) ? 1 : list[GetRandomInt(random, 1, list.Count)].Id);
		List<AvatarAsset> list2 = ((Beard2Res.Count > 0 || Id % 2 == 0) ? Beard2Res : GetBackupAvatarGroup().Beard2Res);
		item2 = (short)((list2.Count <= 1) ? 1 : list2[GetRandomInt(random, 1, list2.Count)].Id);
		return (id1: item, id2: item2);
	}

	public (short id1, short id2) GetRandomBeardsWithCondition(IRandomSource random, Predicate<AvatarAsset> condition)
	{
		short item = 1;
		short item2 = 1;
		if (random.CheckProb(GlobalConfig.Instance.AvatarNoneBeardObb, 10000))
		{
			return (id1: item, id2: item2);
		}
		List<AvatarAsset> selectableBeardResList = new List<AvatarAsset>();
		List<AvatarAsset> resList = ((Beard1Res.Count > 0 || Id % 2 == 0) ? Beard1Res : GetBackupAvatarGroup().Beard1Res);
		item = SelectBeardInList(resList);
		List<AvatarAsset> resList2 = ((Beard2Res.Count > 0 || Id % 2 == 0) ? Beard2Res : GetBackupAvatarGroup().Beard2Res);
		item2 = SelectBeardInList(resList2);
		return (id1: item, id2: item2);
		short SelectBeardInList(List<AvatarAsset> list)
		{
			selectableBeardResList.Clear();
			for (int i = 1; i < list.Count; i++)
			{
				AvatarAsset avatarAsset = list[i];
				if (condition(avatarAsset))
				{
					selectableBeardResList.Add(avatarAsset);
				}
			}
			if (selectableBeardResList.Count == 0)
			{
				return 1;
			}
			return selectableBeardResList[GetRandomInt(random, 0, selectableBeardResList.Count)].Id;
		}
	}

	private AvatarGroup GetBackupAvatarGroup()
	{
		return AvatarManager.Instance.GetAvatarGroup(Id - 1);
	}

	public (short frontId, short backId) GetRandomHairs(IRandomSource random)
	{
		return GetRandomHairsWithCondition(random, null);
	}

	public (short frontId, short backId) GetRandomHairsNoSkinHead(IRandomSource random)
	{
		return GetRandomHairsWithCondition(random, (HairRes res) => res.Id != Hair1Res[0].Id && res.Id != Hair2Res[0].Id);
	}

	public (short frontId, short backId) GetRandomHairsWithCondition(IRandomSource random, Predicate<HairRes> condition)
	{
		List<HairRes> list = new List<HairRes>();
		for (int i = 1; i < Hair1Res.Count; i++)
		{
			HairRes hairRes = Hair1Res[i];
			if (hairRes.Hair.Config.CanCreate && (condition == null || condition(hairRes)))
			{
				list.Add(hairRes);
			}
		}
		short num = (short)((list.Count <= 0) ? 1 : list[GetRandomInt(random, 0, list.Count)].Id);
		AvatarAsset avatarAsset = Get(EAvatarElementsType.Hair1, num);
		if (avatarAsset.Config.DisableRelativeType)
		{
			return (frontId: num, backId: 1);
		}
		list.Clear();
		foreach (HairRes hair2Re in Hair2Res)
		{
			if (hair2Re.Hair.Config.CanCreate && !avatarAsset.Config.BanElements.Exist(hair2Re.Hair.Config.TemplateId) && (condition == null || condition(hair2Re)))
			{
				list.Add(hair2Re);
			}
		}
		short item = (short)((list.Count <= 0) ? 1 : list[GetRandomInt(random, 0, list.Count)].Id);
		return (frontId: num, backId: item);
	}

	public bool IsHairless(short frontId, short backId)
	{
		if (frontId == Hair1Res[0].Id)
		{
			return backId == Hair2Res[0].Id;
		}
		return false;
	}

	public bool IsBeardless(short beard1Id, short beard2Id)
	{
		if (beard1Id == Beard1Res[0].Id)
		{
			return beard2Id == Beard2Res[0].Id;
		}
		return false;
	}

	public (short feature1Id, short feature2Id, short wrinkle1Id, short wrinkle2Id, short wrinkle3Id) GetRandomMaskElems(IRandomSource random)
	{
		short item = 1;
		short item2 = 1;
		short num = 0;
		short num2 = 0;
		short num3 = 0;
		if (!random.CheckProb(GlobalConfig.Instance.AvatarNoneFeatureObb, 10000))
		{
			if (random.CheckPercentProb(GlobalConfig.Instance.AvatarHasFeature1Obb) && Feature1Res.Count > 0)
			{
				List<AvatarAsset> featureResExcludeDelete = GetFeatureResExcludeDelete(Feature1Res);
				item = featureResExcludeDelete[GetRandomInt(random, 1, featureResExcludeDelete.Count)].Id;
			}
			if (random.CheckPercentProb(GlobalConfig.Instance.AvatarHasFeature2Obb) && Feature2Res.Count > 0)
			{
				List<AvatarAsset> featureResExcludeDelete2 = GetFeatureResExcludeDelete(Feature2Res);
				item2 = featureResExcludeDelete2[GetRandomInt(random, 1, featureResExcludeDelete2.Count)].Id;
			}
		}
		num = Wrinkle1Res.GetRandom(random)?.Id ?? 0;
		num2 = Wrinkle2Res.GetRandom(random)?.Id ?? 0;
		num3 = Wrinkle3Res.GetRandom(random)?.Id ?? 0;
		return (feature1Id: item, feature2Id: item2, wrinkle1Id: num, wrinkle2Id: num2, wrinkle3Id: num3);
	}

	public static List<AvatarAsset> GetFeatureResExcludeDelete(List<AvatarAsset> featureRes)
	{
		List<AvatarAsset> list = new List<AvatarAsset>(featureRes);
		int num = list.FindIndex((AvatarAsset e) => e.Config.ElementId == 6);
		if (num >= 0)
		{
			list.RemoveAt(num);
		}
		return list;
	}

	public static short GetUsefulFeatureId(short featureId)
	{
		if (featureId == 6)
		{
			return 7;
		}
		return featureId;
	}
}
