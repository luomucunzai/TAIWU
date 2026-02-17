using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Config;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Character;
using GameData.Domains.CombatSkill;
using GameData.Utilities;

namespace GameData.Domains.Combat.Ai.Memory
{
	// Token: 0x02000731 RID: 1841
	public class AiMemory
	{
		// Token: 0x060068EC RID: 26860 RVA: 0x003B86A8 File Offset: 0x003B68A8
		public AiMemory(CombatCharacter combatChar)
		{
			this._combatCharacter = combatChar;
			int[] enemyTeam = this._combatCharacter.IsAlly ? DomainManager.Combat.GetEnemyTeam() : DomainManager.Combat.GetSelfTeam();
			foreach (int charId in enemyTeam)
			{
				bool flag = charId >= 0;
				if (flag)
				{
					this.EnemyRecordDict.Add(charId, new RecordCollection());
				}
			}
		}

		// Token: 0x060068ED RID: 26861 RVA: 0x003B8734 File Offset: 0x003B6934
		public void ClearMemories()
		{
			this.SelfRecord.ClearAll();
			foreach (RecordCollection enemyRecord in this.EnemyRecordDict.Values)
			{
				enemyRecord.ClearAll();
			}
		}

		// Token: 0x060068EE RID: 26862 RVA: 0x003B879C File Offset: 0x003B699C
		public void RegisterCallbacks()
		{
			Events.RegisterHandler_NormalAttackBegin(new Events.OnNormalAttackBegin(this.OnNormalAttackBegin));
			Events.RegisterHandler_NormalAttackEnd(new Events.OnNormalAttackEnd(this.OnNormalAttackEnd));
			Events.RegisterHandler_CastAttackSkillBegin(new Events.OnCastAttackSkillBegin(this.OnCastAttackSkillBegin));
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
			Events.RegisterHandler_AddDirectDamageValue(new Events.OnAddDirectDamageValue(this.OnAddDirectDamageValue));
			Events.RegisterHandler_AddMindDamage(new Events.OnAddMindDamage(this.OnAddMindDamageValue));
			Events.RegisterHandler_GetTrick(new Events.OnGetTrick(this.OnGetTrick));
		}

		// Token: 0x060068EF RID: 26863 RVA: 0x003B8828 File Offset: 0x003B6A28
		public void UnregisterCallbacks()
		{
			Events.UnRegisterHandler_NormalAttackBegin(new Events.OnNormalAttackBegin(this.OnNormalAttackBegin));
			Events.UnRegisterHandler_NormalAttackEnd(new Events.OnNormalAttackEnd(this.OnNormalAttackEnd));
			Events.UnRegisterHandler_CastAttackSkillBegin(new Events.OnCastAttackSkillBegin(this.OnCastAttackSkillBegin));
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
			Events.UnRegisterHandler_AddDirectDamageValue(new Events.OnAddDirectDamageValue(this.OnAddDirectDamageValue));
			Events.UnRegisterHandler_AddMindDamage(new Events.OnAddMindDamage(this.OnAddMindDamageValue));
			Events.UnRegisterHandler_GetTrick(new Events.OnGetTrick(this.OnGetTrick));
		}

		// Token: 0x060068F0 RID: 26864 RVA: 0x003B88B4 File Offset: 0x003B6AB4
		private void OnNormalAttackBegin(DataContext context, CombatCharacter attacker, CombatCharacter defender, sbyte trickType, int pursueIndex)
		{
			bool flag = attacker == this._combatCharacter;
			if (flag)
			{
				this.UpdateMaxValues(true);
			}
			else
			{
				bool flag2 = defender == this._combatCharacter;
				if (flag2)
				{
					this.UpdateMaxValues(false);
				}
			}
			this._attacker = attacker;
			this._defender = defender;
			this._normalAttackWeaponId = attacker.GetWeapons()[attacker.GetUsingWeaponIndex()].Id;
			this._singleAttackDamage.Outer = 0;
			this._singleAttackDamage.Inner = 0;
			this._singleAttackMindDamage = 0;
			this._gotSkillNeedTrick = false;
		}

		// Token: 0x060068F1 RID: 26865 RVA: 0x003B8940 File Offset: 0x003B6B40
		private void OnNormalAttackEnd(DataContext context, CombatCharacter attacker, CombatCharacter defender, sbyte trickType, int pursueIndex, bool hit, bool isFightBack)
		{
			bool flag = attacker != this._combatCharacter && !this.EnemyRecordDict.ContainsKey(attacker.GetId());
			if (!flag)
			{
				int score = this.GetAttackScore();
				bool gotSkillNeedTrick = this._gotSkillNeedTrick;
				if (gotSkillNeedTrick)
				{
					score += 400;
				}
				Dictionary<int, ValueTuple<int, int>> weaponRecord = (attacker == this._combatCharacter) ? this.SelfRecord.WeaponRecord : this.EnemyRecordDict[attacker.GetId()].WeaponRecord;
				this.UpdateWeaponScore(weaponRecord, this._normalAttackWeaponId, score);
			}
		}

		// Token: 0x060068F2 RID: 26866 RVA: 0x003B89CC File Offset: 0x003B6BCC
		private void OnCastAttackSkillBegin(DataContext context, CombatCharacter attacker, CombatCharacter defender, short skillId)
		{
			bool flag = attacker == this._combatCharacter;
			if (flag)
			{
				this.UpdateMaxValues(true);
			}
			else
			{
				bool flag2 = defender == this._combatCharacter;
				if (flag2)
				{
					this.UpdateMaxValues(false);
				}
			}
			this._attacker = attacker;
			this._defender = defender;
			this._singleAttackDamage.Outer = 0;
			this._singleAttackDamage.Inner = 0;
			this._singleAttackMindDamage = 0;
		}

		// Token: 0x060068F3 RID: 26867 RVA: 0x003B8A34 File Offset: 0x003B6C34
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			CombatSkillKey skillKey = new CombatSkillKey(charId, skillId);
			CombatSkillItem skillConfig = Config.CombatSkill.Instance[skillId];
			GameData.Domains.CombatSkill.CombatSkill skill;
			bool flag = skillConfig.EquipType != 1 || interrupted || (charId != this._combatCharacter.GetId() && !this.EnemyRecordDict.ContainsKey(charId)) || !DomainManager.CombatSkill.TryGetElement_CombatSkills(skillKey, out skill);
			if (!flag)
			{
				int score = this.GetAttackScore();
				int powerScore = DomainManager.SpecialEffect.ModifyData(charId, skillId, 306, (int)power, -1, -1, -1) * 2;
				bool flag2 = powerScore > 0;
				if (flag2)
				{
					sbyte direction = skill.GetDirection();
					bool flag3 = direction != -1;
					if (flag3)
					{
						short effectTemplateId = (short)((direction == 0) ? skillConfig.DirectEffectID : skillConfig.ReverseEffectID);
						SpecialEffectItem effectConfig = SpecialEffect.Instance[effectTemplateId];
						bool flag4 = effectConfig.RequireAttackPower < 0 || power >= effectConfig.RequireAttackPower;
						if (flag4)
						{
							powerScore *= 2;
						}
					}
				}
				score += powerScore;
				Dictionary<short, ValueTuple<int, int>> skillRecord = (charId == this._combatCharacter.GetId()) ? this.SelfRecord.SkillRecord : this.EnemyRecordDict[charId].SkillRecord;
				this.UpdateSkillScore(skillRecord, skillId, score);
			}
		}

		// Token: 0x060068F4 RID: 26868 RVA: 0x003B8B74 File Offset: 0x003B6D74
		private void OnAddDirectDamageValue(DataContext context, int attackerId, int defenderId, sbyte bodyPart, bool isInner, int damageValue, short combatSkillId)
		{
			bool flag = defenderId == this._combatCharacter.GetId() && attackerId != this._combatCharacter.GetId();
			if (flag)
			{
				int distanceIndex = RecordCollection.GetIndexByDistance(DomainManager.Combat.GetCurrentDistance());
				if (isInner)
				{
					this.SelfRecord.MaxDamages[distanceIndex].Inner = Math.Max(this.SelfRecord.MaxDamages[distanceIndex].Inner, damageValue);
				}
				else
				{
					this.SelfRecord.MaxDamages[distanceIndex].Outer = Math.Max(this.SelfRecord.MaxDamages[distanceIndex].Outer, damageValue);
				}
			}
			if (isInner)
			{
				this._singleAttackDamage.Inner = this._singleAttackDamage.Inner + damageValue;
			}
			else
			{
				this._singleAttackDamage.Outer = this._singleAttackDamage.Outer + damageValue;
			}
		}

		// Token: 0x060068F5 RID: 26869 RVA: 0x003B8C58 File Offset: 0x003B6E58
		private void OnAddMindDamageValue(DataContext context, int attackerId, int defenderId, int damageValue, int markCount, short combatSkillId)
		{
			bool flag = defenderId == this._combatCharacter.GetId() && attackerId != this._combatCharacter.GetId();
			if (flag)
			{
				int distanceIndex = RecordCollection.GetIndexByDistance(DomainManager.Combat.GetCurrentDistance());
				this.SelfRecord.MaxMindDamages[distanceIndex] = Math.Max(this.SelfRecord.MaxMindDamages[distanceIndex], damageValue);
			}
			this._singleAttackMindDamage += damageValue;
		}

		// Token: 0x060068F6 RID: 26870 RVA: 0x003B8CD0 File Offset: 0x003B6ED0
		private void OnGetTrick(DataContext context, int charId, bool isAlly, sbyte trickType, bool usable)
		{
			bool flag = this._attacker != null && charId == this._attacker.GetId();
			if (flag)
			{
				this._gotSkillNeedTrick = true;
			}
		}

		// Token: 0x060068F7 RID: 26871 RVA: 0x003B8D04 File Offset: 0x003B6F04
		private unsafe void UpdateMaxValues(bool isAttacker)
		{
			DamageCompareData damageData = DomainManager.Combat.GetDamageCompareData();
			short distance = DomainManager.Combat.GetCurrentDistance();
			int distanceIndex = RecordCollection.GetIndexByDistance(distance);
			if (isAttacker)
			{
				for (int i = 0; i < damageData.HitType.Length; i++)
				{
					sbyte hitType = damageData.HitType[i];
					bool flag = hitType >= 0;
					if (flag)
					{
						HitOrAvoidInts hitValues = this.SelfRecord.MaxHits[distanceIndex];
						*(ref hitValues.Items.FixedElementField + (IntPtr)hitType * 4) = Math.Max(*(ref hitValues.Items.FixedElementField + (IntPtr)hitType * 4), damageData.HitValue[i]);
					}
				}
				bool flag2 = damageData.OuterAttackValue >= 0;
				if (flag2)
				{
					this.SelfRecord.MaxPenetrates[distanceIndex].Outer = Math.Max(this.SelfRecord.MaxPenetrates[distanceIndex].Outer, damageData.OuterAttackValue);
				}
				bool flag3 = damageData.InnerAttackValue >= 0;
				if (flag3)
				{
					this.SelfRecord.MaxPenetrates[distanceIndex].Inner = Math.Max(this.SelfRecord.MaxPenetrates[distanceIndex].Inner, damageData.InnerAttackValue);
				}
			}
			else
			{
				for (int j = 0; j < damageData.HitType.Length; j++)
				{
					sbyte hitType2 = damageData.HitType[j];
					bool flag4 = hitType2 >= 0;
					if (flag4)
					{
						*(ref this.SelfRecord.MaxAvoid.Items.FixedElementField + (IntPtr)hitType2 * 4) = Math.Max(*(ref this.SelfRecord.MaxAvoid.Items.FixedElementField + (IntPtr)hitType2 * 4), damageData.AvoidValue[j]);
					}
				}
				bool flag5 = damageData.OuterDefendValue >= 0;
				if (flag5)
				{
					this.SelfRecord.MaxPenetrateResist.Outer = Math.Max(this.SelfRecord.MaxPenetrateResist.Outer, damageData.OuterDefendValue);
				}
				bool flag6 = damageData.InnerDefendValue >= 0;
				if (flag6)
				{
					this.SelfRecord.MaxPenetrateResist.Inner = Math.Max(this.SelfRecord.MaxPenetrateResist.Inner, damageData.InnerDefendValue);
				}
			}
		}

		// Token: 0x060068F8 RID: 26872 RVA: 0x003B8F4C File Offset: 0x003B714C
		private int GetAttackScore()
		{
			DamageStepCollection damageStepCollection = this._defender.GetDamageStepCollection();
			int score = 0;
			score += this._singleAttackDamage.Outer * 100 / (damageStepCollection.OuterDamageSteps.Sum() / 7);
			score += this._singleAttackDamage.Inner * 100 / (damageStepCollection.InnerDamageSteps.Sum() / 7);
			return score + this._singleAttackMindDamage * 100 / damageStepCollection.MindDamageStep;
		}

		// Token: 0x060068F9 RID: 26873 RVA: 0x003B8FC0 File Offset: 0x003B71C0
		private void UpdateWeaponScore([TupleElementNames(new string[]
		{
			"score",
			"zeroScoreCount"
		})] Dictionary<int, ValueTuple<int, int>> weaponRecord, int weaponId, int newScore)
		{
			bool flag = weaponRecord.ContainsKey(weaponId);
			if (flag)
			{
				ValueTuple<int, int> record = weaponRecord[weaponId];
				bool flag2 = newScore > 0;
				if (flag2)
				{
					record.Item1 = newScore;
					record.Item2 = 0;
				}
				else
				{
					record.Item2++;
				}
				weaponRecord[weaponId] = record;
			}
			else
			{
				weaponRecord.Add(weaponId, new ValueTuple<int, int>(newScore, (newScore > 0) ? 0 : 1));
			}
		}

		// Token: 0x060068FA RID: 26874 RVA: 0x003B9030 File Offset: 0x003B7230
		private void UpdateSkillScore([TupleElementNames(new string[]
		{
			"score",
			"zeroScoreCount"
		})] Dictionary<short, ValueTuple<int, int>> skillRecord, short skillId, int newScore)
		{
			bool flag = skillRecord.ContainsKey(skillId);
			if (flag)
			{
				ValueTuple<int, int> record = skillRecord[skillId];
				bool flag2 = newScore > 0;
				if (flag2)
				{
					record.Item1 = newScore;
					record.Item2 = 0;
				}
				else
				{
					record.Item2++;
				}
				skillRecord[skillId] = record;
			}
			else
			{
				skillRecord.Add(skillId, new ValueTuple<int, int>(newScore, (newScore > 0) ? 0 : 1));
			}
		}

		// Token: 0x04001CCF RID: 7375
		private readonly CombatCharacter _combatCharacter;

		// Token: 0x04001CD0 RID: 7376
		public readonly RecordCollection SelfRecord = new RecordCollection();

		// Token: 0x04001CD1 RID: 7377
		public readonly Dictionary<int, RecordCollection> EnemyRecordDict = new Dictionary<int, RecordCollection>();

		// Token: 0x04001CD2 RID: 7378
		private CombatCharacter _attacker;

		// Token: 0x04001CD3 RID: 7379
		private CombatCharacter _defender;

		// Token: 0x04001CD4 RID: 7380
		private int _normalAttackWeaponId;

		// Token: 0x04001CD5 RID: 7381
		private OuterAndInnerInts _singleAttackDamage;

		// Token: 0x04001CD6 RID: 7382
		private int _singleAttackMindDamage;

		// Token: 0x04001CD7 RID: 7383
		private bool _gotSkillNeedTrick;
	}
}
