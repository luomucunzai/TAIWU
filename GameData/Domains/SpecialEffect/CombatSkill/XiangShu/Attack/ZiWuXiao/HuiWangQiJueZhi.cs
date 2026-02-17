using System;
using System.Collections.Generic;
using Config;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.XiangShu.Attack.ZiWuXiao
{
	// Token: 0x020002B7 RID: 695
	public class HuiWangQiJueZhi : CombatSkillEffectBase
	{
		// Token: 0x06003224 RID: 12836 RVA: 0x0021DFD3 File Offset: 0x0021C1D3
		public HuiWangQiJueZhi()
		{
		}

		// Token: 0x06003225 RID: 12837 RVA: 0x0021DFDD File Offset: 0x0021C1DD
		public HuiWangQiJueZhi(CombatSkillKey skillKey) : base(skillKey, 17114, -1)
		{
		}

		// Token: 0x06003226 RID: 12838 RVA: 0x0021DFF0 File Offset: 0x0021C1F0
		public override void OnEnable(DataContext context)
		{
			SkillEffectKey effectKey = new SkillEffectKey(875, true);
			Dictionary<SkillEffectKey, short> effectDict = base.CombatChar.GetSkillEffectCollection().EffectDict;
			short value;
			this._addPower = (int)((effectDict != null && effectDict.TryGetValue(effectKey, out value)) ? (30 * value) : 0);
			bool flag = this._addPower > 0;
			if (flag)
			{
				base.CreateAffectedData(199, EDataModifyType.AddPercent, base.SkillTemplateId);
				DomainManager.Combat.ChangeSkillEffectCount(context, base.CombatChar, effectKey, -effectDict[effectKey], true, false);
				base.ShowSpecialEffectTips(0);
			}
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06003227 RID: 12839 RVA: 0x0021E091 File Offset: 0x0021C291
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06003228 RID: 12840 RVA: 0x0021E0A8 File Offset: 0x0021C2A8
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				bool flag2 = base.PowerMatchAffectRequire((int)power, 0);
				if (flag2)
				{
					SkillEffectKey effectKey = new SkillEffectKey(876, true);
					Dictionary<SkillEffectKey, short> effectDict = base.CombatChar.GetSkillEffectCollection().EffectDict;
					bool flag3 = effectDict != null && effectDict.ContainsKey(effectKey);
					if (flag3)
					{
						DomainManager.Combat.CastSkillFree(context, base.CombatChar, 879, ECombatCastFreePriority.Normal);
						base.ShowSpecialEffectTips(1);
					}
					else
					{
						CombatCharacter enemyChar = DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly, false);
						IReadOnlyDictionary<int, sbyte> trickDict = enemyChar.GetTricks().Tricks;
						List<NeedTrick> removeTricks = ObjectPool<List<NeedTrick>>.Instance.Get();
						removeTricks.Clear();
						foreach (sbyte trickType in trickDict.Values)
						{
							bool flag4 = enemyChar.IsTrickUseless(trickType);
							if (!flag4)
							{
								removeTricks.Add(new NeedTrick(trickType, 1));
							}
						}
						DomainManager.Combat.RemoveTrick(context, enemyChar, removeTricks, false, false, -1);
						ObjectPool<List<NeedTrick>>.Instance.Return(removeTricks);
						base.ShowSpecialEffectTips(2);
					}
				}
				base.RemoveSelf(context);
			}
		}

		// Token: 0x06003229 RID: 12841 RVA: 0x0021E218 File Offset: 0x0021C418
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || dataKey.CombatSkillId != base.SkillTemplateId;
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

		// Token: 0x04000ED9 RID: 3801
		private const sbyte AddPowerUnit = 30;

		// Token: 0x04000EDA RID: 3802
		private int _addPower;
	}
}
