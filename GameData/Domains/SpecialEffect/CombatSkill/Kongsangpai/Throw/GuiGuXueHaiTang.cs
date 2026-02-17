using System;
using System.Collections.Generic;
using System.Linq;
using Config;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Character;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.GameDataBridge;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.Kongsangpai.Throw
{
	// Token: 0x02000479 RID: 1145
	public class GuiGuXueHaiTang : CombatSkillEffectBase
	{
		// Token: 0x06003B7A RID: 15226 RVA: 0x0024821D File Offset: 0x0024641D
		public GuiGuXueHaiTang()
		{
		}

		// Token: 0x06003B7B RID: 15227 RVA: 0x00248227 File Offset: 0x00246427
		public GuiGuXueHaiTang(CombatSkillKey skillKey) : base(skillKey, 10408, -1)
		{
		}

		// Token: 0x06003B7C RID: 15228 RVA: 0x00248238 File Offset: 0x00246438
		public override void OnEnable(DataContext context)
		{
			Events.RegisterHandler_CombatBegin(new Events.OnCombatBegin(this.OnCombatBegin));
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06003B7D RID: 15229 RVA: 0x00248260 File Offset: 0x00246460
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_CombatBegin(new Events.OnCombatBegin(this.OnCombatBegin));
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
			bool flag = this._allCharMarkUid != null;
			if (flag)
			{
				foreach (DataUid dataUid in this._allCharMarkUid)
				{
					GameDataBridge.RemovePostDataModificationHandler(dataUid, base.DataHandlerKey);
				}
			}
		}

		// Token: 0x06003B7E RID: 15230 RVA: 0x002482F0 File Offset: 0x002464F0
		private void OnCombatBegin(DataContext context)
		{
			base.AppendAffectedAllCombatCharData(context, 199, EDataModifyType.AddPercent, -1);
			bool isDirect = base.IsDirect;
			if (isDirect)
			{
				base.AppendAffectedAllCombatCharData(context, 245, EDataModifyType.AddPercent, -1);
			}
			else
			{
				base.AppendAffectedData(context, base.CharacterId, 162, EDataModifyType.Custom, -1);
			}
			this._allCharMarkUid = new List<DataUid>();
			List<int> allCharList = ObjectPool<List<int>>.Instance.Get();
			allCharList.Clear();
			DomainManager.Combat.GetAllCharInCombat(allCharList);
			for (int i = 0; i < allCharList.Count; i++)
			{
				DataUid markUid = new DataUid(8, 10, (ulong)((long)allCharList[i]), 50U);
				this._allCharMarkUid.Add(markUid);
				GameDataBridge.AddPostDataModificationHandler(markUid, base.DataHandlerKey, new Action<DataContext, DataUid>(this.OnMarkChanged));
			}
			ObjectPool<List<int>>.Instance.Return(allCharList);
		}

		// Token: 0x06003B7F RID: 15231 RVA: 0x002483C8 File Offset: 0x002465C8
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			if (!interrupted)
			{
				bool flag = this.SkillKey.IsMatch(charId, skillId) && base.PowerMatchAffectRequire((int)power, 0);
				if (flag)
				{
					bool flag2 = base.EffectCount != (int)base.MaxEffectCount;
					if (flag2)
					{
						bool flag3 = base.EffectCount > 0;
						if (flag3)
						{
							DomainManager.Combat.ChangeSkillEffectToMaxCount(context, base.CombatChar, base.EffectKey);
						}
						else
						{
							base.AddMaxEffectCount(true);
						}
					}
					bool isDirect = base.IsDirect;
					if (isDirect)
					{
						base.InvalidateAllEnemyCache(context, 245);
					}
				}
				bool flag4 = Config.CombatSkill.Instance[skillId].EquipType != 1;
				if (!flag4)
				{
					CombatCharacter affectChar = DomainManager.Combat.GetElement_CombatCharacterDict(charId);
					bool flag5 = base.IsDirect ? (affectChar.IsAlly == base.CombatChar.IsAlly) : (affectChar.GetId() != base.CharacterId);
					if (!flag5)
					{
						bool flag6 = !affectChar.GetDefeatMarkCollection().PoisonMarkList.Exist((byte count) => count > 0) || base.EffectCount <= 0;
						if (!flag6)
						{
							DomainManager.Combat.AppendDieDefeatMark(context, affectChar, this.SkillKey, 1);
							base.ShowSpecialEffectTips(0);
							base.ReduceEffectCount(1);
							bool flag7 = affectChar.GetDefeatMarkCollection().DieMarkList.Count != (int)GameData.Domains.Combat.SharedConstValue.DefeatNeedDieMarkCount;
							if (!flag7)
							{
								bool flag8 = DomainManager.Combat.CheckHealthImmunity(context, affectChar);
								if (!flag8)
								{
									GameData.Domains.Character.Character character = affectChar.GetCharacter();
									DomainManager.SpecialEffect.AddAddMaxHealthEffect(context, character.GetId(), (int)(-(int)character.GetLeftMaxHealth(false)));
									character.ChangeHealth(context, 0);
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x06003B80 RID: 15232 RVA: 0x0024859C File Offset: 0x0024679C
		private void OnMarkChanged(DataContext context, DataUid dataUid)
		{
			base.InvalidateCache(context, (int)dataUid.SubId0, 199);
			base.InvalidateCache(context, (int)dataUid.SubId0, 232);
		}

		// Token: 0x06003B81 RID: 15233 RVA: 0x002485C8 File Offset: 0x002467C8
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.CharId != base.CharacterId && dataKey.FieldId == 245 && base.EffectCount > 0;
			int result;
			if (flag)
			{
				result = -50;
			}
			else
			{
				ushort fieldId = dataKey.FieldId;
				if (!true)
				{
				}
				int num;
				if (fieldId != 199)
				{
					num = 0;
				}
				else
				{
					num = (base.IsDirect ? 20 : 40);
				}
				if (!true)
				{
				}
				int unit = num;
				bool flag2 = unit == 0;
				if (flag2)
				{
					result = 0;
				}
				else
				{
					DefeatMarkCollection markCollection = DomainManager.Combat.GetElement_CombatCharacterDict(dataKey.CharId).GetDefeatMarkCollection();
					result = unit * markCollection.DieMarkList.Count((CombatSkillKey t) => t.Equals(this.SkillKey));
				}
			}
			return result;
		}

		// Token: 0x06003B82 RID: 15234 RVA: 0x0024867C File Offset: 0x0024687C
		public override bool GetModifiedValue(AffectedDataKey dataKey, bool dataValue)
		{
			bool flag = dataKey.CharId == base.CharacterId && dataKey.FieldId == 162 && base.EffectCount > 0;
			bool result;
			if (flag)
			{
				base.ShowSpecialEffectTipsOnceInFrame(1);
				result = false;
			}
			else
			{
				result = dataValue;
			}
			return result;
		}

		// Token: 0x0400116C RID: 4460
		private const sbyte DirectAddPowerUnit = 20;

		// Token: 0x0400116D RID: 4461
		private const sbyte ReverseAddPowerUnit = 40;

		// Token: 0x0400116E RID: 4462
		private const int DirectPoisonResistAddPercent = -50;

		// Token: 0x0400116F RID: 4463
		private List<DataUid> _allCharMarkUid;
	}
}
