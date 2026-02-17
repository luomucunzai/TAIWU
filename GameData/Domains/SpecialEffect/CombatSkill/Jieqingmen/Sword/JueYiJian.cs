using System;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Character;
using GameData.Domains.Character.Relation;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.Information;

namespace GameData.Domains.SpecialEffect.CombatSkill.Jieqingmen.Sword
{
	// Token: 0x020004E5 RID: 1253
	public class JueYiJian : CombatSkillEffectBase
	{
		// Token: 0x06003DED RID: 15853 RVA: 0x00253FB6 File Offset: 0x002521B6
		public JueYiJian()
		{
		}

		// Token: 0x06003DEE RID: 15854 RVA: 0x00253FC0 File Offset: 0x002521C0
		public JueYiJian(CombatSkillKey skillKey) : base(skillKey, 13201, -1)
		{
		}

		// Token: 0x06003DEF RID: 15855 RVA: 0x00253FD1 File Offset: 0x002521D1
		public override void OnEnable(DataContext context)
		{
			Events.RegisterHandler_CastAttackSkillBegin(new Events.OnCastAttackSkillBegin(this.OnCastAttackSkillBegin));
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06003DF0 RID: 15856 RVA: 0x00253FF8 File Offset: 0x002521F8
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_CastAttackSkillBegin(new Events.OnCastAttackSkillBegin(this.OnCastAttackSkillBegin));
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06003DF1 RID: 15857 RVA: 0x00254020 File Offset: 0x00252220
		private void OnCastAttackSkillBegin(DataContext context, CombatCharacter attacker, CombatCharacter defender, short skillId)
		{
			bool flag = attacker.GetId() != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				short favor = DomainManager.Character.GetFavorability(base.CurrEnemyChar.GetId(), base.CharacterId);
				sbyte favorType = FavorabilityType.GetFavorabilityType(favor);
				bool flag2 = favorType < 3;
				if (!flag2)
				{
					this._addDamage = (int)JueYiJian.AddDamage[(int)(favorType - 3)];
					base.AppendAffectedData(context, 69, EDataModifyType.AddPercent, base.SkillTemplateId);
					base.ShowSpecialEffectTips(0);
					bool flag3 = favorType < 4;
					if (!flag3)
					{
						this._affected = true;
						DomainManager.Combat.AddTrick(context, base.IsDirect ? base.CombatChar : base.EnemyChar, 19, 9, base.IsDirect, false);
						base.ShowSpecialEffectTips(1);
					}
				}
			}
		}

		// Token: 0x06003DF2 RID: 15858 RVA: 0x002540F4 File Offset: 0x002522F4
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				bool affected = this._affected;
				if (affected)
				{
					this._affected = false;
					this.MakeRelation(context);
					this.ChangeFavorability(context);
				}
				base.RemoveSelf(context);
			}
		}

		// Token: 0x06003DF3 RID: 15859 RVA: 0x0025414C File Offset: 0x0025234C
		private void MakeRelation(DataContext context)
		{
			bool flag = !RelationTypeHelper.AllowAddingRelation(base.CurrEnemyChar.GetId(), base.CharacterId, 32768);
			if (!flag)
			{
				InformationDomain informationDomain = DomainManager.Information;
				DomainManager.Character.AddRelation(context, base.CurrEnemyChar.GetId(), base.CharacterId, 32768, int.MinValue);
				int secretInformationMetaDataId = informationDomain.AddSecretInformationMetaData(context, informationDomain.GetSecretInformationCollection().AddBecomeEnemy(base.CharacterId, base.CurrEnemyChar.GetId()), false);
				informationDomain.ReceiveSecretInformation(context, secretInformationMetaDataId, base.CharacterId, -1);
				informationDomain.ReceiveSecretInformation(context, secretInformationMetaDataId, base.CurrEnemyChar.GetId(), -1);
				informationDomain.SetSecretInformationCollectionModified(context);
				base.ShowSpecialEffectTips(2);
			}
		}

		// Token: 0x06003DF4 RID: 15860 RVA: 0x00254208 File Offset: 0x00252408
		private void ChangeFavorability(DataContext context)
		{
			Character srcChar = base.CurrEnemyChar.GetCharacter();
			byte creatingType = srcChar.GetCreatingType();
			bool flag = creatingType <= 1;
			bool flag2 = !flag;
			if (!flag2)
			{
				Character dstChar = base.CombatChar.GetCharacter();
				creatingType = dstChar.GetCreatingType();
				flag = (creatingType <= 1);
				bool flag3 = !flag;
				if (!flag3)
				{
					DomainManager.Character.DirectlyChangeFavorabilityOptional(context, srcChar, dstChar, -10000, -1);
				}
			}
		}

		// Token: 0x06003DF5 RID: 15861 RVA: 0x00254280 File Offset: 0x00252480
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
				bool flag2 = dataKey.FieldId == 69;
				if (flag2)
				{
					result = this._addDamage;
				}
				else
				{
					result = 0;
				}
			}
			return result;
		}

		// Token: 0x04001246 RID: 4678
		private const int AddShaCount = 9;

		// Token: 0x04001247 RID: 4679
		private const int ChangeFavorabilityValue = -10000;

		// Token: 0x04001248 RID: 4680
		private static readonly short[] AddDamage = new short[]
		{
			20,
			40,
			80,
			160
		};

		// Token: 0x04001249 RID: 4681
		private bool _affected;

		// Token: 0x0400124A RID: 4682
		private int _addDamage;
	}
}
