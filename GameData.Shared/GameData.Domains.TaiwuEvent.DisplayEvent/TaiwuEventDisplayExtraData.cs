using System.Collections.Generic;
using GameData.Domains.Character.Display;
using GameData.Domains.Item.Display;
using GameData.Domains.Merchant;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.TaiwuEvent.DisplayEvent;

[SerializableGameData(NotRestrictCollectionSerializedSize = true)]
public class TaiwuEventDisplayExtraData : ISerializableGameData
{
	[SerializableGameDataField]
	public bool HideRightFavorability;

	[SerializableGameDataField]
	public bool HideLeftFavorability;

	[SerializableGameDataField]
	public bool ForbidViewCharacter;

	[SerializableGameDataField]
	public bool ForbidViewSelf;

	[SerializableGameDataField]
	public bool MainRoleUseAlternativeName;

	[SerializableGameDataField]
	public bool TargetRoleUseAlternativeName;

	[SerializableGameDataField]
	public bool MainRoleShyFlag;

	[SerializableGameDataField]
	public bool TargetRoleShyFlag;

	[SerializableGameDataField]
	public bool LeftRoleShowInjuryInfo;

	[SerializableGameDataField]
	public bool RightRoleShowInjuryInfo;

	[SerializableGameDataField]
	public short MainRoleAdjustClothDisplayId;

	[SerializableGameDataField]
	public short TargetRoleAdjustClothDisplayId;

	[SerializableGameDataField]
	public short HereticTemplateId;

	[SerializableGameDataField]
	public CaravanDisplayData CaravanData;

	[SerializableGameDataField(SubDataMaxCount = int.MaxValue)]
	public EventSelectItemData SelectItemData;

	[SerializableGameDataField(SubDataMaxCount = int.MaxValue)]
	public EventSelectCharacterData SelectCharacterData;

	[SerializableGameDataField]
	public EventSelectReadingBookCountData SelectReadingBookCountData;

	[SerializableGameDataField]
	public EventSelectNeigongLoopingCountData SelectNeigongLoopingCountData;

	[SerializableGameDataField]
	public EventSelectFuyuFaithCountData SelectFuyuFaithCountData;

	[SerializableGameDataField]
	public EventSelectFameData SelectFameData;

	[SerializableGameDataField]
	public EventInputRequestData InputRequestData;

	[SerializableGameDataField]
	public EventActorData ActorData;

	[SerializableGameDataField]
	public EventActorData LeftActorData;

	[SerializableGameDataField(SubDataMaxCount = int.MaxValue)]
	public List<AvatarRelatedData> SelectOneAvatarRelatedDataList;

	[SerializableGameDataField]
	public bool RightCharacterShadow;

	[SerializableGameDataField]
	public bool RightForbiddenConsummateLevel;

	[SerializableGameDataField]
	public bool LeftForbidShowFavorChangeEffect;

	[SerializableGameDataField]
	public bool RightForbidShowFavorChangeEffect;

	[SerializableGameDataField]
	public ItemDisplayData JiaoDisplayData;

	[SerializableGameDataField]
	public bool LeftActorShowMarriageLook1;

	[SerializableGameDataField]
	public bool LeftActorShowMarriageLook2;

	[SerializableGameDataField]
	public bool RightActorShowMarriageLook1;

	[SerializableGameDataField]
	public bool RightActorShowMarriageLook2;

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 24;
		num = ((CaravanData == null) ? (num + 2) : (num + (2 + CaravanData.GetSerializedSize())));
		num = ((SelectItemData == null) ? (num + 4) : (num + (4 + SelectItemData.GetSerializedSize())));
		num = ((SelectCharacterData == null) ? (num + 4) : (num + (4 + SelectCharacterData.GetSerializedSize())));
		num = ((SelectReadingBookCountData == null) ? (num + 2) : (num + (2 + SelectReadingBookCountData.GetSerializedSize())));
		num = ((SelectNeigongLoopingCountData == null) ? (num + 2) : (num + (2 + SelectNeigongLoopingCountData.GetSerializedSize())));
		num = ((SelectFuyuFaithCountData == null) ? (num + 2) : (num + (2 + SelectFuyuFaithCountData.GetSerializedSize())));
		num = ((SelectFameData == null) ? (num + 2) : (num + (2 + SelectFameData.GetSerializedSize())));
		num = ((InputRequestData == null) ? (num + 2) : (num + (2 + InputRequestData.GetSerializedSize())));
		num = ((ActorData == null) ? (num + 2) : (num + (2 + ActorData.GetSerializedSize())));
		num = ((LeftActorData == null) ? (num + 2) : (num + (2 + LeftActorData.GetSerializedSize())));
		num = ((SelectOneAvatarRelatedDataList == null) ? (num + 2) : (num + (2 + 84 * SelectOneAvatarRelatedDataList.Count)));
		num = ((JiaoDisplayData == null) ? (num + 2) : (num + (2 + JiaoDisplayData.GetSerializedSize())));
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		*ptr = (HideRightFavorability ? ((byte)1) : ((byte)0));
		ptr++;
		*ptr = (HideLeftFavorability ? ((byte)1) : ((byte)0));
		ptr++;
		*ptr = (ForbidViewCharacter ? ((byte)1) : ((byte)0));
		ptr++;
		*ptr = (ForbidViewSelf ? ((byte)1) : ((byte)0));
		ptr++;
		*ptr = (MainRoleUseAlternativeName ? ((byte)1) : ((byte)0));
		ptr++;
		*ptr = (TargetRoleUseAlternativeName ? ((byte)1) : ((byte)0));
		ptr++;
		*ptr = (MainRoleShyFlag ? ((byte)1) : ((byte)0));
		ptr++;
		*ptr = (TargetRoleShyFlag ? ((byte)1) : ((byte)0));
		ptr++;
		*ptr = (LeftRoleShowInjuryInfo ? ((byte)1) : ((byte)0));
		ptr++;
		*ptr = (RightRoleShowInjuryInfo ? ((byte)1) : ((byte)0));
		ptr++;
		*(short*)ptr = MainRoleAdjustClothDisplayId;
		ptr += 2;
		*(short*)ptr = TargetRoleAdjustClothDisplayId;
		ptr += 2;
		*(short*)ptr = HereticTemplateId;
		ptr += 2;
		if (CaravanData != null)
		{
			byte* intPtr = ptr;
			ptr += 2;
			int num = CaravanData.Serialize(ptr);
			ptr += num;
			Tester.Assert(num <= 65535);
			*(ushort*)intPtr = (ushort)num;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		if (SelectItemData != null)
		{
			byte* intPtr2 = ptr;
			ptr += 4;
			int num2 = SelectItemData.Serialize(ptr);
			ptr += num2;
			Tester.Assert(num2 <= int.MaxValue);
			*(int*)intPtr2 = num2;
		}
		else
		{
			*(int*)ptr = 0;
			ptr += 4;
		}
		if (SelectCharacterData != null)
		{
			byte* intPtr3 = ptr;
			ptr += 4;
			int num3 = SelectCharacterData.Serialize(ptr);
			ptr += num3;
			Tester.Assert(num3 <= int.MaxValue);
			*(int*)intPtr3 = num3;
		}
		else
		{
			*(int*)ptr = 0;
			ptr += 4;
		}
		if (SelectReadingBookCountData != null)
		{
			byte* intPtr4 = ptr;
			ptr += 2;
			int num4 = SelectReadingBookCountData.Serialize(ptr);
			ptr += num4;
			Tester.Assert(num4 <= 65535);
			*(ushort*)intPtr4 = (ushort)num4;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		if (SelectNeigongLoopingCountData != null)
		{
			byte* intPtr5 = ptr;
			ptr += 2;
			int num5 = SelectNeigongLoopingCountData.Serialize(ptr);
			ptr += num5;
			Tester.Assert(num5 <= 65535);
			*(ushort*)intPtr5 = (ushort)num5;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		if (SelectFuyuFaithCountData != null)
		{
			byte* intPtr6 = ptr;
			ptr += 2;
			int num6 = SelectFuyuFaithCountData.Serialize(ptr);
			ptr += num6;
			Tester.Assert(num6 <= 65535);
			*(ushort*)intPtr6 = (ushort)num6;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		if (SelectFameData != null)
		{
			byte* intPtr7 = ptr;
			ptr += 2;
			int num7 = SelectFameData.Serialize(ptr);
			ptr += num7;
			Tester.Assert(num7 <= 65535);
			*(ushort*)intPtr7 = (ushort)num7;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		if (InputRequestData != null)
		{
			byte* intPtr8 = ptr;
			ptr += 2;
			int num8 = InputRequestData.Serialize(ptr);
			ptr += num8;
			Tester.Assert(num8 <= 65535);
			*(ushort*)intPtr8 = (ushort)num8;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		if (ActorData != null)
		{
			byte* intPtr9 = ptr;
			ptr += 2;
			int num9 = ActorData.Serialize(ptr);
			ptr += num9;
			Tester.Assert(num9 <= 65535);
			*(ushort*)intPtr9 = (ushort)num9;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		if (LeftActorData != null)
		{
			byte* intPtr10 = ptr;
			ptr += 2;
			int num10 = LeftActorData.Serialize(ptr);
			ptr += num10;
			Tester.Assert(num10 <= 65535);
			*(ushort*)intPtr10 = (ushort)num10;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		if (SelectOneAvatarRelatedDataList != null)
		{
			int count = SelectOneAvatarRelatedDataList.Count;
			Tester.Assert(count <= 65535);
			*(ushort*)ptr = (ushort)count;
			ptr += 2;
			for (int i = 0; i < count; i++)
			{
				ptr += SelectOneAvatarRelatedDataList[i].Serialize(ptr);
			}
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		*ptr = (RightCharacterShadow ? ((byte)1) : ((byte)0));
		ptr++;
		*ptr = (RightForbiddenConsummateLevel ? ((byte)1) : ((byte)0));
		ptr++;
		*ptr = (LeftForbidShowFavorChangeEffect ? ((byte)1) : ((byte)0));
		ptr++;
		*ptr = (RightForbidShowFavorChangeEffect ? ((byte)1) : ((byte)0));
		ptr++;
		if (JiaoDisplayData != null)
		{
			byte* intPtr11 = ptr;
			ptr += 2;
			int num11 = JiaoDisplayData.Serialize(ptr);
			ptr += num11;
			Tester.Assert(num11 <= 65535);
			*(ushort*)intPtr11 = (ushort)num11;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		*ptr = (LeftActorShowMarriageLook1 ? ((byte)1) : ((byte)0));
		ptr++;
		*ptr = (LeftActorShowMarriageLook2 ? ((byte)1) : ((byte)0));
		ptr++;
		*ptr = (RightActorShowMarriageLook1 ? ((byte)1) : ((byte)0));
		ptr++;
		*ptr = (RightActorShowMarriageLook2 ? ((byte)1) : ((byte)0));
		ptr++;
		int num12 = (int)(ptr - pData);
		if (num12 > 4)
		{
			return (num12 + 3) / 4 * 4;
		}
		return num12;
	}

	public unsafe int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		HideRightFavorability = *ptr != 0;
		ptr++;
		HideLeftFavorability = *ptr != 0;
		ptr++;
		ForbidViewCharacter = *ptr != 0;
		ptr++;
		ForbidViewSelf = *ptr != 0;
		ptr++;
		MainRoleUseAlternativeName = *ptr != 0;
		ptr++;
		TargetRoleUseAlternativeName = *ptr != 0;
		ptr++;
		MainRoleShyFlag = *ptr != 0;
		ptr++;
		TargetRoleShyFlag = *ptr != 0;
		ptr++;
		LeftRoleShowInjuryInfo = *ptr != 0;
		ptr++;
		RightRoleShowInjuryInfo = *ptr != 0;
		ptr++;
		MainRoleAdjustClothDisplayId = *(short*)ptr;
		ptr += 2;
		TargetRoleAdjustClothDisplayId = *(short*)ptr;
		ptr += 2;
		HereticTemplateId = *(short*)ptr;
		ptr += 2;
		ushort num = *(ushort*)ptr;
		ptr += 2;
		if (num > 0)
		{
			if (CaravanData == null)
			{
				CaravanData = new CaravanDisplayData();
			}
			ptr += CaravanData.Deserialize(ptr);
		}
		else
		{
			CaravanData = null;
		}
		int num2 = *(int*)ptr;
		ptr += 4;
		if (num2 > 0)
		{
			if (SelectItemData == null)
			{
				SelectItemData = new EventSelectItemData();
			}
			ptr += SelectItemData.Deserialize(ptr);
		}
		else
		{
			SelectItemData = null;
		}
		int num3 = *(int*)ptr;
		ptr += 4;
		if (num3 > 0)
		{
			if (SelectCharacterData == null)
			{
				SelectCharacterData = new EventSelectCharacterData();
			}
			ptr += SelectCharacterData.Deserialize(ptr);
		}
		else
		{
			SelectCharacterData = null;
		}
		ushort num4 = *(ushort*)ptr;
		ptr += 2;
		if (num4 > 0)
		{
			if (SelectReadingBookCountData == null)
			{
				SelectReadingBookCountData = new EventSelectReadingBookCountData();
			}
			ptr += SelectReadingBookCountData.Deserialize(ptr);
		}
		else
		{
			SelectReadingBookCountData = null;
		}
		ushort num5 = *(ushort*)ptr;
		ptr += 2;
		if (num5 > 0)
		{
			if (SelectNeigongLoopingCountData == null)
			{
				SelectNeigongLoopingCountData = new EventSelectNeigongLoopingCountData();
			}
			ptr += SelectNeigongLoopingCountData.Deserialize(ptr);
		}
		else
		{
			SelectNeigongLoopingCountData = null;
		}
		ushort num6 = *(ushort*)ptr;
		ptr += 2;
		if (num6 > 0)
		{
			if (SelectFuyuFaithCountData == null)
			{
				SelectFuyuFaithCountData = new EventSelectFuyuFaithCountData();
			}
			ptr += SelectFuyuFaithCountData.Deserialize(ptr);
		}
		else
		{
			SelectFuyuFaithCountData = null;
		}
		ushort num7 = *(ushort*)ptr;
		ptr += 2;
		if (num7 > 0)
		{
			if (SelectFameData == null)
			{
				SelectFameData = new EventSelectFameData();
			}
			ptr += SelectFameData.Deserialize(ptr);
		}
		else
		{
			SelectFameData = null;
		}
		ushort num8 = *(ushort*)ptr;
		ptr += 2;
		if (num8 > 0)
		{
			if (InputRequestData == null)
			{
				InputRequestData = new EventInputRequestData();
			}
			ptr += InputRequestData.Deserialize(ptr);
		}
		else
		{
			InputRequestData = null;
		}
		ushort num9 = *(ushort*)ptr;
		ptr += 2;
		if (num9 > 0)
		{
			if (ActorData == null)
			{
				ActorData = new EventActorData();
			}
			ptr += ActorData.Deserialize(ptr);
		}
		else
		{
			ActorData = null;
		}
		ushort num10 = *(ushort*)ptr;
		ptr += 2;
		if (num10 > 0)
		{
			if (LeftActorData == null)
			{
				LeftActorData = new EventActorData();
			}
			ptr += LeftActorData.Deserialize(ptr);
		}
		else
		{
			LeftActorData = null;
		}
		ushort num11 = *(ushort*)ptr;
		ptr += 2;
		if (num11 > 0)
		{
			if (SelectOneAvatarRelatedDataList == null)
			{
				SelectOneAvatarRelatedDataList = new List<AvatarRelatedData>(num11);
			}
			else
			{
				SelectOneAvatarRelatedDataList.Clear();
			}
			for (int i = 0; i < num11; i++)
			{
				AvatarRelatedData avatarRelatedData = new AvatarRelatedData();
				ptr += avatarRelatedData.Deserialize(ptr);
				SelectOneAvatarRelatedDataList.Add(avatarRelatedData);
			}
		}
		else
		{
			SelectOneAvatarRelatedDataList?.Clear();
		}
		RightCharacterShadow = *ptr != 0;
		ptr++;
		RightForbiddenConsummateLevel = *ptr != 0;
		ptr++;
		LeftForbidShowFavorChangeEffect = *ptr != 0;
		ptr++;
		RightForbidShowFavorChangeEffect = *ptr != 0;
		ptr++;
		ushort num12 = *(ushort*)ptr;
		ptr += 2;
		if (num12 > 0)
		{
			if (JiaoDisplayData == null)
			{
				JiaoDisplayData = new ItemDisplayData();
			}
			ptr += JiaoDisplayData.Deserialize(ptr);
		}
		else
		{
			JiaoDisplayData = null;
		}
		LeftActorShowMarriageLook1 = *ptr != 0;
		ptr++;
		LeftActorShowMarriageLook2 = *ptr != 0;
		ptr++;
		RightActorShowMarriageLook1 = *ptr != 0;
		ptr++;
		RightActorShowMarriageLook2 = *ptr != 0;
		ptr++;
		int num13 = (int)(ptr - pData);
		if (num13 > 4)
		{
			return (num13 + 3) / 4 * 4;
		}
		return num13;
	}
}
