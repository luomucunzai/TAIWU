using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.Character;
using GameData.Domains.CombatSkill;
using GameData.GameDataBridge;

namespace GameData.Domains.SpecialEffect.CombatSkill.Emeipai.Neigong
{
	// Token: 0x02000547 RID: 1351
	public class DaBoNiePanFa : CombatSkillEffectBase
	{
		// Token: 0x06004014 RID: 16404 RVA: 0x0025CDD4 File Offset: 0x0025AFD4
		public DaBoNiePanFa()
		{
		}

		// Token: 0x06004015 RID: 16405 RVA: 0x0025CDDE File Offset: 0x0025AFDE
		public DaBoNiePanFa(CombatSkillKey skillKey) : base(skillKey, 2007, -1)
		{
		}

		// Token: 0x06004016 RID: 16406 RVA: 0x0025CDF0 File Offset: 0x0025AFF0
		public override void OnEnable(DataContext context)
		{
			base.CreateAffectedData(199, EDataModifyType.AddPercent, -1);
			this._featureUid = base.ParseCharDataUid(17);
			GameDataBridge.AddPostDataModificationHandler(this._featureUid, base.DataHandlerKey, new Action<DataContext, DataUid>(this.OnFeaturesChange));
			this.UpdateAddPowerValue();
		}

		// Token: 0x06004017 RID: 16407 RVA: 0x0025CE3F File Offset: 0x0025B03F
		public override void OnDisable(DataContext context)
		{
			GameDataBridge.RemovePostDataModificationHandler(this._featureUid, base.DataHandlerKey);
		}

		// Token: 0x06004018 RID: 16408 RVA: 0x0025CE54 File Offset: 0x0025B054
		private void UpdateAddPowerValue()
		{
			this._addPowerValue = 0;
			List<short> featureIds = this.CharObj.GetFeatureIds();
			bool flag = this.AnyReincarnationBonusFeature(featureIds);
			if (flag)
			{
				this._addPowerValue += 20;
			}
			bool flag2 = this.AnyProfessionReincarnationBonusFeature(featureIds);
			if (flag2)
			{
				this._addPowerValue += 10;
			}
			List<DeadCharacter> deadCharList = DomainManager.Character.GetCharacterSamsaraData(base.CharacterId).DeadCharacters;
			sbyte fameThreshold = base.IsDirect ? 4 : 2;
			for (int i = 0; i < deadCharList.Count; i++)
			{
				bool flag3 = deadCharList[i] == null;
				if (!flag3)
				{
					sbyte fameType = deadCharList[i].FameType;
					bool flag4 = fameType >= 0 && (base.IsDirect ? (fameType >= fameThreshold) : (fameType <= fameThreshold));
					if (flag4)
					{
						this._addPowerValue += DaBoNiePanFa.AddPower[(int)(base.IsDirect ? (fameType - fameThreshold) : (fameThreshold - fameType))];
					}
				}
			}
		}

		// Token: 0x06004019 RID: 16409 RVA: 0x0025CF60 File Offset: 0x0025B160
		private bool AnyReincarnationBonusFeature(List<short> featureIds)
		{
			return featureIds.Exists(base.IsDirect ? new Predicate<short>(Character.IsPositiveReincarnationBonusFeature) : new Predicate<short>(Character.IsNegativeReincarnationBonusFeature));
		}

		// Token: 0x0600401A RID: 16410 RVA: 0x0025CF9C File Offset: 0x0025B19C
		private bool AnyProfessionReincarnationBonusFeature(List<short> featureIds)
		{
			return featureIds.Exists(base.IsDirect ? new Predicate<short>(Character.IsProfessionPositiveReincarnationBonusFeature) : new Predicate<short>(Character.IsProfessionNegativeReincarnationBonusFeature));
		}

		// Token: 0x0600401B RID: 16411 RVA: 0x0025CFD6 File Offset: 0x0025B1D6
		private void OnFeaturesChange(DataContext context, DataUid dataUid)
		{
			this.UpdateAddPowerValue();
			DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, 199);
		}

		// Token: 0x0600401C RID: 16412 RVA: 0x0025CFF8 File Offset: 0x0025B1F8
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.CharId != base.CharacterId;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				bool flag2 = dataKey.FieldId == 199;
				if (flag2)
				{
					result = (int)this._addPowerValue;
				}
				else
				{
					result = 0;
				}
			}
			return result;
		}

		// Token: 0x0600401D RID: 16413 RVA: 0x0025D040 File Offset: 0x0025B240
		protected override int GetSubClassSerializedSize()
		{
			return base.GetSubClassSerializedSize() + 1;
		}

		// Token: 0x0600401E RID: 16414 RVA: 0x0025D05C File Offset: 0x0025B25C
		protected unsafe override int SerializeSubClass(byte* pData)
		{
			byte* pCurrData = pData + base.SerializeSubClass(pData);
			*pCurrData = (byte)this._addPowerValue;
			return this.GetSubClassSerializedSize();
		}

		// Token: 0x0600401F RID: 16415 RVA: 0x0025D088 File Offset: 0x0025B288
		protected unsafe override int DeserializeSubClass(byte* pData)
		{
			byte* pCurrData = pData + base.DeserializeSubClass(pData);
			this._addPowerValue = *(sbyte*)pCurrData;
			return this.GetSubClassSerializedSize();
		}

		// Token: 0x040012DA RID: 4826
		private static readonly sbyte[] AddPower = new sbyte[]
		{
			1,
			3,
			5
		};

		// Token: 0x040012DB RID: 4827
		private const sbyte ReincarnationBonusFeatureAddPower = 20;

		// Token: 0x040012DC RID: 4828
		private const sbyte ProfessionReincarnationBonusFeatureAddPower = 10;

		// Token: 0x040012DD RID: 4829
		private sbyte _addPowerValue;

		// Token: 0x040012DE RID: 4830
		private DataUid _featureUid;
	}
}
