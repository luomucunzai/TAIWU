using System;
using Config;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Character;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Kongsangpai.FistAndPalm
{
	// Token: 0x0200048E RID: 1166
	public class DaXueShanZhangFa : CombatSkillEffectBase
	{
		// Token: 0x06003C08 RID: 15368 RVA: 0x0024BB76 File Offset: 0x00249D76
		public DaXueShanZhangFa()
		{
		}

		// Token: 0x06003C09 RID: 15369 RVA: 0x0024BB80 File Offset: 0x00249D80
		public DaXueShanZhangFa(CombatSkillKey skillKey) : base(skillKey, 10101, -1)
		{
		}

		// Token: 0x06003C0A RID: 15370 RVA: 0x0024BB91 File Offset: 0x00249D91
		public override void OnEnable(DataContext context)
		{
			Events.RegisterHandler_PrepareSkillBegin(new Events.OnPrepareSkillBegin(this.OnPrepareSkillBegin));
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
			Events.RegisterHandler_SkillEffectChange(new Events.OnSkillEffectChange(this.OnSkillEffectChange));
		}

		// Token: 0x06003C0B RID: 15371 RVA: 0x0024BBCA File Offset: 0x00249DCA
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_PrepareSkillBegin(new Events.OnPrepareSkillBegin(this.OnPrepareSkillBegin));
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
			Events.UnRegisterHandler_SkillEffectChange(new Events.OnSkillEffectChange(this.OnSkillEffectChange));
		}

		// Token: 0x06003C0C RID: 15372 RVA: 0x0024BC04 File Offset: 0x00249E04
		private unsafe void OnPrepareSkillBegin(DataContext context, int charId, bool isAlly, short skillId)
		{
			bool flag = !this.IsSrcSkillPerformed || charId != base.CharacterId || Config.CombatSkill.Instance[skillId].EquipType != 1;
			if (!flag)
			{
				MainAttributes selfMainAttributes = this.CharObj.GetCurrMainAttributes();
				MainAttributes enemyMainAttributes = DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly, true).GetCharacter().GetCurrMainAttributes();
				this._affectingSkillId = skillId;
				this._addPower = 0;
				for (sbyte type = 0; type < 6; type += 1)
				{
					bool flag2 = base.IsDirect ? (*(ref selfMainAttributes.Items.FixedElementField + (IntPtr)type * 2) > *(ref enemyMainAttributes.Items.FixedElementField + (IntPtr)type * 2)) : (*(ref selfMainAttributes.Items.FixedElementField + (IntPtr)type * 2) < *(ref enemyMainAttributes.Items.FixedElementField + (IntPtr)type * 2));
					if (flag2)
					{
						this._addPower += 10;
					}
				}
				bool flag3 = this._addPower > 0;
				if (flag3)
				{
					base.AppendAffectedData(context, charId, 199, EDataModifyType.AddPercent, skillId);
					base.ShowSpecialEffectTips(0);
				}
			}
		}

		// Token: 0x06003C0D RID: 15373 RVA: 0x0024BD30 File Offset: 0x00249F30
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = !this.IsSrcSkillPerformed;
			if (flag)
			{
				bool flag2 = charId == base.CharacterId && skillId == base.SkillTemplateId;
				if (flag2)
				{
					this.IsSrcSkillPerformed = true;
					bool flag3 = base.PowerMatchAffectRequire((int)power, 0);
					if (flag3)
					{
						base.AddMaxEffectCount(true);
					}
					else
					{
						base.RemoveSelf(context);
					}
				}
			}
			else
			{
				bool flag4 = charId == base.CharacterId;
				if (flag4)
				{
					bool flag5 = skillId == base.SkillTemplateId && base.PowerMatchAffectRequire((int)power, 0);
					if (flag5)
					{
						base.RemoveSelf(context);
					}
					else
					{
						bool flag6 = Config.CombatSkill.Instance[skillId].EquipType == 1;
						if (flag6)
						{
							base.ClearAffectedData(context);
							base.ReduceEffectCount(1);
						}
					}
				}
			}
		}

		// Token: 0x06003C0E RID: 15374 RVA: 0x0024BDF4 File Offset: 0x00249FF4
		private void OnSkillEffectChange(DataContext context, int charId, SkillEffectKey key, short oldCount, short newCount, bool removed)
		{
			bool flag = removed && this.IsSrcSkillPerformed && charId == base.CharacterId && key.SkillId == base.SkillTemplateId && key.IsDirect == base.IsDirect;
			if (flag)
			{
				base.RemoveSelf(context);
			}
		}

		// Token: 0x06003C0F RID: 15375 RVA: 0x0024BE44 File Offset: 0x0024A044
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || dataKey.CombatSkillId != this._affectingSkillId;
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
					result = this._addPower;
				}
				else
				{
					result = 0;
				}
			}
			return result;
		}

		// Token: 0x040011A7 RID: 4519
		private const int AddPowerUnit = 10;

		// Token: 0x040011A8 RID: 4520
		private short _affectingSkillId;

		// Token: 0x040011A9 RID: 4521
		private int _addPower;
	}
}
