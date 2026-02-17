using System;
using Config;
using GameData.Domains.CombatSkill;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Combat
{
	// Token: 0x02000708 RID: 1800
	[SerializableGameData(NotForArchive = true)]
	public struct WeaponEffectDisplayData : ISerializableGameData
	{
		// Token: 0x06006811 RID: 26641 RVA: 0x003B3820 File Offset: 0x003B1A20
		public WeaponEffectDisplayData(SkillEffectKey effectKey, int charId)
		{
			this.EffectKey = effectKey;
			GameData.Domains.CombatSkill.CombatSkill skill;
			bool flag = DomainManager.CombatSkill.TryGetElement_CombatSkills(new ValueTuple<int, short>(charId, effectKey.SkillId), out skill);
			if (flag)
			{
				this.EffectDescription = DomainManager.CombatSkill.GetEffectDisplayData(skill);
			}
			else
			{
				bool flag2 = effectKey.SkillId >= 0;
				if (flag2)
				{
					this.EffectDescription = new CombatSkillEffectDescriptionDisplayData
					{
						EffectId = (effectKey.IsDirect ? Config.CombatSkill.Instance[effectKey.SkillId].DirectEffectID : Config.CombatSkill.Instance[effectKey.SkillId].ReverseEffectID)
					};
				}
				else
				{
					this.EffectDescription = CombatSkillEffectDescriptionDisplayData.Invalid;
				}
			}
		}

		// Token: 0x06006812 RID: 26642 RVA: 0x003B38D0 File Offset: 0x003B1AD0
		public bool IsSerializedSizeFixed()
		{
			return false;
		}

		// Token: 0x06006813 RID: 26643 RVA: 0x003B38E4 File Offset: 0x003B1AE4
		public int GetSerializedSize()
		{
			int totalSize = 3;
			totalSize += this.EffectDescription.GetSerializedSize();
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x06006814 RID: 26644 RVA: 0x003B3918 File Offset: 0x003B1B18
		public unsafe int Serialize(byte* pData)
		{
			byte* pCurrData = pData + this.EffectKey.Serialize(pData);
			int fieldSize = this.EffectDescription.Serialize(pCurrData);
			pCurrData += fieldSize;
			Tester.Assert(fieldSize <= 65535, "");
			int totalSize = (int)((long)(pCurrData - pData));
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x06006815 RID: 26645 RVA: 0x003B3980 File Offset: 0x003B1B80
		public unsafe int Deserialize(byte* pData)
		{
			byte* pCurrData = pData + this.EffectKey.Deserialize(pData);
			pCurrData += this.EffectDescription.Deserialize(pCurrData);
			int totalSize = (int)((long)(pCurrData - pData));
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x04001C79 RID: 7289
		[SerializableGameDataField]
		public SkillEffectKey EffectKey;

		// Token: 0x04001C7A RID: 7290
		[SerializableGameDataField]
		public CombatSkillEffectDescriptionDisplayData EffectDescription;
	}
}
