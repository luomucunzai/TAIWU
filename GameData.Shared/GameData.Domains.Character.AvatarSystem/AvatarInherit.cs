using GameData.Domains.Character.AvatarSystem.AvatarRes;
using Redzen.Random;

namespace GameData.Domains.Character.AvatarSystem;

public class AvatarInherit
{
	private AvatarManager _avatarManager;

	private readonly AvatarData _father;

	private readonly AvatarData _mother;

	private readonly sbyte _specifiedBodyType;

	private sbyte _gender;

	private readonly int _avatarChanceMutation;

	private IRandomSource _customRandom;

	public AvatarInherit(AvatarData father, AvatarData mother, sbyte gender, sbyte bodyType, IRandomSource random)
	{
		_father = father;
		_mother = mother;
		_gender = gender;
		_specifiedBodyType = bodyType;
		_avatarChanceMutation = GlobalConfig.Instance.AvatarChanceMutation;
		_customRandom = random;
		_avatarManager = AvatarManager.Instance;
	}

	public AvatarData GetInheritAvatar()
	{
		if (_father == null && _mother == null)
		{
			return null;
		}
		int num = 0;
		if (_gender == 1)
		{
			if (_father != null)
			{
				num = _father.AvatarId;
			}
			else if (_mother != null)
			{
				num = (byte)(_mother.AvatarId - 1);
			}
		}
		else if (_gender == 0)
		{
			if (_mother != null)
			{
				num = _mother.AvatarId;
			}
			else if (_father != null)
			{
				num = (byte)(_father.AvatarId + 1);
			}
		}
		if (_gender == -1)
		{
			_gender = Gender.GetRandom(_customRandom);
		}
		if (_specifiedBodyType != -1)
		{
			num = _avatarManager.GetAvatarIdByBodyTypeAndGender(_specifiedBodyType, _gender);
		}
		if (num == 0)
		{
			num = _customRandom.Next(1, 4) * 2;
			if (_gender == 1)
			{
				num--;
			}
		}
		AvatarGroup avatarGroup = _avatarManager.GetAvatarGroup(num);
		AvatarData item = avatarGroup.GetRandomAvatar(_customRandom).avatar;
		InheritColorSkin(item);
		InheritEyesId(item);
		InheritEyesScale(item);
		InheritEyesHeight(item);
		InheritEyesDistance(item);
		InheritEyesRotate(item);
		InheritEyebrowsId(item);
		InheritEyebrowsScale(item);
		InheritEyebrowsHeight(item);
		InheritEyebrowsDistance(item);
		InheritEyebrowsRotate(item);
		InheritEyebrowsColor(item);
		InheritEyeballsColor(item);
		InheritMouthId(item);
		InheritMouthScale(item);
		InheritMouthHeight(item);
		InheritMouthColor(item);
		InheritNoseId(item);
		InheritNoseScale(item);
		InheritNoseHeight(item);
		InheritBeard1Id(item);
		InheritBeard2Id(item);
		InheritBeardColor(item);
		InheritFeature1Id(item);
		InheritFeature1Color(item);
		InheritFeature2Id(item);
		InheritFeature2Color(item);
		if (item.Gender == 1)
		{
			(short id1, short id2) randomBeards = _avatarManager.GetAvatarGroup(item.AvatarId).GetRandomBeards(_customRandom);
			short item2 = randomBeards.id1;
			short item3 = randomBeards.id2;
			AvatarAsset avatarAsset = avatarGroup.Get(EAvatarElementsType.Beard1, item.Beard1Id);
			if (avatarAsset == null)
			{
				item.Beard1Id = item2;
			}
			AvatarAsset avatarAsset2 = avatarGroup.Get(EAvatarElementsType.Beard2, item.Beard2Id);
			if (avatarAsset2 == null)
			{
				item.Beard2Id = item3;
			}
		}
		_avatarManager = null;
		_customRandom = null;
		return item;
	}

	private int GetInheritCode()
	{
		int num = _customRandom.Next(10000);
		if (num < _avatarChanceMutation)
		{
			return 0;
		}
		if (_father != null && _mother != null)
		{
			if (num >= 5000)
			{
				return 2;
			}
			return 1;
		}
		if (_father == null && _mother != null)
		{
			return 2;
		}
		if (_mother == null && _father != null)
		{
			return 1;
		}
		return 0;
	}

	private void InheritColorSkin(AvatarData avatarData)
	{
		int inheritCode = GetInheritCode();
		if (1 == inheritCode)
		{
			avatarData.ColorSkinId = _father.ColorSkinId;
		}
		else if (2 == inheritCode)
		{
			avatarData.ColorSkinId = _mother.ColorSkinId;
		}
	}

	private void InheritEyesId(AvatarData avatarData)
	{
		int inheritCode = GetInheritCode();
		AvatarData avatarData2 = _father;
		if (2 == inheritCode)
		{
			avatarData2 = _mother;
		}
		if (avatarData2 != null)
		{
			avatarData.EyesMainId = avatarData2.EyesMainId;
			if (_avatarManager.GetAsset(avatarData.AvatarId, EAvatarElementsType.Eye, avatarData.EyesMainId, avatarData2.EyesLeftId) != null)
			{
				avatarData.EyesLeftId = avatarData2.EyesLeftId;
			}
			if (_avatarManager.GetAsset(avatarData.AvatarId, EAvatarElementsType.Eye, avatarData.EyesMainId, avatarData2.EyesRightId) != null)
			{
				avatarData.EyesRightId = avatarData2.EyesRightId;
			}
		}
	}

	private void InheritEyesHeight(AvatarData avatarData)
	{
		int inheritCode = GetInheritCode();
		if (inheritCode != 0)
		{
			AvatarData avatarData2 = null;
			if (1 == inheritCode && _father != null)
			{
				avatarData2 = _father;
			}
			if (2 == inheritCode && _mother != null)
			{
				avatarData2 = _mother;
			}
			if (avatarData2 != null)
			{
				avatarData.EyesHeightPercent = avatarData2.EyesHeightPercent;
			}
		}
	}

	private void InheritEyesDistance(AvatarData avatarData)
	{
		int inheritCode = GetInheritCode();
		if (inheritCode != 0)
		{
			AvatarData avatarData2 = null;
			if (1 == inheritCode && _father != null)
			{
				avatarData2 = _father;
			}
			if (2 == inheritCode && _mother != null)
			{
				avatarData2 = _mother;
			}
			if (avatarData2 != null)
			{
				avatarData.EyesDistancePercent = avatarData2.EyesDistancePercent;
			}
		}
	}

	private void InheritEyesRotate(AvatarData avatarData)
	{
		int inheritCode = GetInheritCode();
		if (inheritCode != 0)
		{
			if (1 == inheritCode)
			{
				avatarData.EyesAngle = _father.EyesAngle;
			}
			else if (2 == inheritCode)
			{
				avatarData.EyesAngle = _mother.EyesAngle;
			}
		}
	}

	private void InheritEyesScale(AvatarData avatarData)
	{
		int inheritCode = GetInheritCode();
		if (inheritCode != 0)
		{
			if (1 == inheritCode)
			{
				avatarData.EyesScale = _father.EyesScale;
			}
			else if (2 == inheritCode)
			{
				avatarData.EyesScale = _mother.EyesScale;
			}
		}
	}

	private void InheritEyebrowsHeight(AvatarData avatarData)
	{
		int inheritCode = GetInheritCode();
		if (inheritCode != 0)
		{
			if (1 == inheritCode)
			{
				avatarData.EyebrowHeight = _father.EyebrowHeight;
			}
			else if (2 == inheritCode)
			{
				avatarData.EyebrowHeight = _mother.EyebrowHeight;
			}
		}
	}

	private void InheritEyebrowsDistance(AvatarData avatarData)
	{
		int inheritCode = GetInheritCode();
		if (inheritCode != 0)
		{
			AvatarData avatarData2 = null;
			if (1 == inheritCode && _father != null)
			{
				avatarData2 = _father;
			}
			if (2 == inheritCode && _mother != null)
			{
				avatarData2 = _mother;
			}
			if (avatarData2 != null)
			{
				avatarData.EyebrowDistancePercent = avatarData2.EyebrowDistancePercent;
			}
		}
	}

	private void InheritEyebrowsRotate(AvatarData avatarData)
	{
		int inheritCode = GetInheritCode();
		if (inheritCode != 0)
		{
			if (1 == inheritCode)
			{
				avatarData.EyebrowAngle = _father.EyebrowAngle;
			}
			else if (2 == inheritCode)
			{
				avatarData.EyebrowAngle = _mother.EyebrowAngle;
			}
		}
	}

	private void InheritEyebrowsScale(AvatarData avatarData)
	{
		int inheritCode = GetInheritCode();
		if (inheritCode != 0)
		{
			if (1 == inheritCode)
			{
				avatarData.EyebrowScale = _father.EyebrowScale;
			}
			else if (2 == inheritCode)
			{
				avatarData.EyebrowScale = _mother.EyebrowScale;
			}
		}
	}

	private void InheritEyebrowsId(AvatarData avatarData)
	{
		int inheritCode = GetInheritCode();
		if (inheritCode != 0)
		{
			if (1 == inheritCode)
			{
				avatarData.EyebrowId = _father.EyebrowId;
			}
			else if (2 == inheritCode)
			{
				avatarData.EyebrowId = _mother.EyebrowId;
			}
		}
	}

	private void InheritEyebrowsColor(AvatarData avatarData)
	{
		int inheritCode = GetInheritCode();
		if (inheritCode != 0)
		{
			if (1 == inheritCode)
			{
				avatarData.ColorEyebrowId = _father.ColorEyebrowId;
			}
			else if (2 == inheritCode)
			{
				avatarData.ColorEyebrowId = _mother.ColorEyebrowId;
			}
		}
	}

	private void InheritEyeballsColor(AvatarData avatarData)
	{
		int inheritCode = GetInheritCode();
		if (inheritCode != 0)
		{
			if (1 == inheritCode)
			{
				avatarData.ColorEyeballId = _father.ColorEyeballId;
			}
			else if (2 == inheritCode)
			{
				avatarData.ColorEyeballId = _mother.ColorEyeballId;
			}
		}
	}

	private void InheritNoseId(AvatarData avatarData)
	{
		int inheritCode = GetInheritCode();
		if (inheritCode != 0)
		{
			if (1 == inheritCode)
			{
				avatarData.NoseId = _father.NoseId;
			}
			else if (2 == inheritCode)
			{
				avatarData.NoseId = _mother.NoseId;
			}
		}
	}

	private void InheritNoseHeight(AvatarData avatarData)
	{
		int inheritCode = GetInheritCode();
		if (inheritCode != 0)
		{
			AvatarData avatarData2 = null;
			if (1 == inheritCode && _father != null)
			{
				avatarData2 = _father;
			}
			if (2 == inheritCode && _mother != null)
			{
				avatarData2 = _mother;
			}
			if (avatarData2 != null)
			{
				avatarData.NoseHeightPercent = avatarData2.NoseHeightPercent;
			}
		}
	}

	private void InheritNoseScale(AvatarData avatarData)
	{
		int inheritCode = GetInheritCode();
		if (inheritCode != 0)
		{
			if (1 == inheritCode)
			{
				avatarData.NoseScale = _father.NoseScale;
			}
			else if (2 == inheritCode)
			{
				avatarData.NoseScale = _mother.NoseScale;
			}
		}
	}

	private void InheritMouthId(AvatarData avatarData)
	{
		int inheritCode = GetInheritCode();
		if (inheritCode != 0)
		{
			if (1 == inheritCode)
			{
				avatarData.MouthId = _father.MouthId;
			}
			else if (2 == inheritCode)
			{
				avatarData.MouthId = _mother.MouthId;
			}
		}
	}

	private void InheritMouthHeight(AvatarData avatarData)
	{
		int inheritCode = GetInheritCode();
		if (inheritCode != 0)
		{
			AvatarData avatarData2 = null;
			if (1 == inheritCode && _father != null)
			{
				avatarData2 = _father;
			}
			if (2 == inheritCode && _mother != null)
			{
				avatarData2 = _mother;
			}
			if (avatarData2 != null)
			{
				avatarData.MouthHeightPercent = avatarData2.MouthHeightPercent;
			}
		}
	}

	private void InheritMouthScale(AvatarData avatarData)
	{
		int inheritCode = GetInheritCode();
		if (inheritCode != 0)
		{
			if (1 == inheritCode)
			{
				avatarData.MouthScale = _father.MouthScale;
			}
			else if (2 == inheritCode)
			{
				avatarData.MouthScale = _mother.MouthScale;
			}
		}
	}

	private void InheritMouthColor(AvatarData avatarData)
	{
		int inheritCode = GetInheritCode();
		if (inheritCode != 0)
		{
			if (1 == inheritCode)
			{
				avatarData.ColorMouthId = _father.ColorMouthId;
			}
			else if (2 == inheritCode)
			{
				avatarData.ColorMouthId = _mother.ColorMouthId;
			}
		}
	}

	private void InheritBeardColor(AvatarData avatarData)
	{
		int inheritCode = GetInheritCode();
		if (inheritCode == 0)
		{
			return;
		}
		if (1 == inheritCode)
		{
			avatarData.ColorBeard1Id = _father.ColorBeard1Id;
		}
		else if (2 == inheritCode)
		{
			avatarData.ColorBeard1Id = _mother.ColorBeard1Id;
		}
		inheritCode = GetInheritCode();
		if (inheritCode != 0)
		{
			if (1 == inheritCode)
			{
				avatarData.ColorBeard2Id = _father.ColorBeard2Id;
			}
			else if (2 == inheritCode)
			{
				avatarData.ColorBeard2Id = _mother.ColorBeard2Id;
			}
		}
	}

	private void InheritBeard1Id(AvatarData avatarData)
	{
		int inheritCode = GetInheritCode();
		if (inheritCode != 0)
		{
			if (1 == inheritCode)
			{
				avatarData.Beard1Id = _father.Beard1Id;
			}
			else if (2 == inheritCode)
			{
				avatarData.Beard1Id = _mother.Beard1Id;
			}
		}
	}

	private void InheritBeard2Id(AvatarData avatarData)
	{
		int inheritCode = GetInheritCode();
		if (inheritCode != 0)
		{
			if (1 == inheritCode)
			{
				avatarData.Beard2Id = _father.Beard2Id;
			}
			else if (2 == inheritCode)
			{
				avatarData.Beard2Id = _mother.Beard2Id;
			}
		}
	}

	private void InheritFeature1Id(AvatarData avatarData)
	{
		AvatarAsset avatarAsset = null;
		if (_father != null)
		{
			avatarAsset = _avatarManager.GetAsset(_father.AvatarId, EAvatarElementsType.Feature1, _father.Feature1Id);
		}
		AvatarAsset avatarAsset2 = null;
		if (_mother != null)
		{
			avatarAsset2 = _avatarManager.GetAsset(_mother.AvatarId, EAvatarElementsType.Feature1, _mother.Feature1Id);
		}
		bool flag = false;
		if (avatarAsset != null)
		{
			flag = avatarAsset.Config.Inherit;
		}
		bool flag2 = false;
		if (avatarAsset2 != null)
		{
			flag2 = avatarAsset2.Config.Inherit;
		}
		int inheritCode = GetInheritCode();
		if (flag != flag2)
		{
			if (flag)
			{
				avatarData.Feature1Id = _father.Feature1Id;
			}
			if (flag2)
			{
				avatarData.Feature1Id = _mother.Feature1Id;
			}
		}
		else if (flag || inheritCode != 0)
		{
			if (1 == inheritCode && _father != null)
			{
				avatarData.Feature1Id = _father.Feature1Id;
			}
			if (2 == inheritCode && _mother != null)
			{
				avatarData.Feature1Id = _mother.Feature1Id;
			}
		}
	}

	private void InheritFeature2Id(AvatarData avatarData)
	{
		AvatarAsset avatarAsset = null;
		if (_father != null)
		{
			avatarAsset = _avatarManager.GetAsset(_father.AvatarId, EAvatarElementsType.Feature2, _father.Feature2Id);
		}
		AvatarAsset avatarAsset2 = null;
		if (_mother != null)
		{
			avatarAsset2 = _avatarManager.GetAsset(_mother.AvatarId, EAvatarElementsType.Feature2, _mother.Feature2Id);
		}
		bool flag = false;
		if (avatarAsset != null)
		{
			flag = avatarAsset.Config.Inherit;
		}
		bool flag2 = false;
		if (avatarAsset2 != null)
		{
			flag2 = avatarAsset2.Config.Inherit;
		}
		int inheritCode = GetInheritCode();
		if (flag != flag2)
		{
			if (flag)
			{
				avatarData.Feature2Id = _father.Feature2Id;
			}
			if (flag2)
			{
				avatarData.Feature2Id = _mother.Feature2Id;
			}
		}
		else if (flag || inheritCode != 0)
		{
			if (1 == inheritCode && _father != null)
			{
				avatarData.Feature2Id = _father.Feature2Id;
			}
			if (2 == inheritCode && _mother != null)
			{
				avatarData.Feature2Id = _mother.Feature2Id;
			}
		}
	}

	private void InheritFeature1Color(AvatarData avatarData)
	{
		int inheritCode = GetInheritCode();
		if (inheritCode != 0)
		{
			if (1 == inheritCode)
			{
				avatarData.ColorFeature1Id = _father.ColorFeature1Id;
			}
			else if (2 == inheritCode)
			{
				avatarData.ColorFeature1Id = _mother.ColorFeature1Id;
			}
		}
	}

	private void InheritFeature2Color(AvatarData avatarData)
	{
		int inheritCode = GetInheritCode();
		if (inheritCode != 0)
		{
			if (1 == inheritCode)
			{
				avatarData.ColorFeature2Id = _father.ColorFeature2Id;
			}
			else if (2 == inheritCode)
			{
				avatarData.ColorFeature2Id = _mother.ColorFeature2Id;
			}
		}
	}
}
