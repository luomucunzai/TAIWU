using System;
using Config;
using GameData.Common;
using GameData.Domains.Item;
using GameData.Domains.Taiwu.Profession;

namespace GameData.Domains.Combat
{
	// Token: 0x02000693 RID: 1683
	public class CombatCharacterStateAnimalAttack : CombatCharacterStateBase
	{
		// Token: 0x060061CB RID: 25035 RVA: 0x003782D7 File Offset: 0x003764D7
		public CombatCharacterStateAnimalAttack(CombatDomain combatDomain, CombatCharacter combatChar) : base(combatDomain, combatChar, CombatCharacterStateType.AnimalAttack)
		{
			this.IsUpdateOnPause = true;
		}

		// Token: 0x060061CC RID: 25036 RVA: 0x003782EC File Offset: 0x003764EC
		public override void OnEnter()
		{
			DataContext context = this.CombatChar.GetDataContext();
			this._animalChar = this.CurrentCombatDomain.GetElement_CombatCharacterDict(this.CurrentCombatDomain.GetCarrierAnimalCombatCharId());
			ItemKey[] weapons = this._animalChar.GetWeapons();
			int maxWeaponIndex = 0;
			for (int i = 1; i < 3; i++)
			{
				bool flag = weapons[i].IsValid();
				if (!flag)
				{
					break;
				}
				maxWeaponIndex = i;
			}
			this.CurrentCombatDomain.ChangeWeapon(context, this._animalChar, context.Random.Next(maxWeaponIndex), false, false);
			sbyte[] weaponTricks = this._animalChar.GetWeaponTricks();
			this._trickType = weaponTricks[context.Random.Next(weaponTricks.Length)];
			GameData.Domains.Item.Weapon weapon = this._animalChar.GetWeaponData(-1).Item;
			sbyte displayDist = this._animalChar.AnimalConfig.AttackDistances[this._animalChar.GetUsingWeaponIndex()];
			int displayPos = this.CurrentCombatDomain.GetDisplayPosition(this.CombatChar.IsAlly, (short)displayDist);
			ValueTuple<string, string, string, string> attackEffect = this.CurrentCombatDomain.GetAttackEffect(this._animalChar, weapon, this._trickType);
			this._attackAni = attackEffect.Item1;
			this._attackParticle = attackEffect.Item3;
			this._attackSound = attackEffect.Item4;
			this._animalEnterFrame = 34;
			this._attackDamageFrame = (short)((double)this._animalEnterFrame + Math.Round((double)(AnimDataCollection.Data[attackEffect.Item2].Events["act0"][0] * 60f)));
			this._animalLeaveFrame = (short)((double)this._animalEnterFrame + Math.Round((double)(AnimDataCollection.Data[attackEffect.Item2].Duration * 60f)));
			this._stateTotalFrame = this._animalLeaveFrame + 24;
			this.CombatChar.NeedAnimalAttack = false;
			this._animalChar.SetVisible(true, context);
			this._animalChar.SetDisplayPosition(displayPos, context);
			this._animalChar.SetAnimationToLoop(this.CurrentCombatDomain.GetIdleAni(this._animalChar), context);
			short carrierId = this.CombatChar.GetCharacter().GetEquipment()[11].TemplateId;
			sbyte carrierGrade = Config.Carrier.Instance[carrierId].Grade;
			ProfessionFormulaItem formula = ProfessionFormula.Instance[13];
			int addSeniority = formula.Calculate((int)carrierGrade);
			DomainManager.Extra.ChangeProfessionSeniority(context, 1, addSeniority, true, false);
		}

		// Token: 0x060061CD RID: 25037 RVA: 0x00378560 File Offset: 0x00376760
		public override bool OnUpdate()
		{
			bool flag = !base.OnUpdate();
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				bool flag2 = this._animalEnterFrame > 0;
				if (flag2)
				{
					this._animalEnterFrame -= 1;
					bool flag3 = this._animalEnterFrame == 0;
					if (flag3)
					{
						DataContext context = this.CombatChar.GetDataContext();
						this._animalChar.SetAnimationToPlayOnce(this._attackAni, context);
						this._animalChar.SetParticleToPlay(this._attackParticle, context);
						this._animalChar.SetAttackSoundToPlay(this._attackSound, context);
					}
				}
				bool flag4 = this._attackDamageFrame > 0;
				if (flag4)
				{
					this._attackDamageFrame -= 1;
					bool flag5 = this._attackDamageFrame == 0;
					if (flag5)
					{
						CombatContext context2 = CombatContext.Create(this._animalChar, null, -1, -1, -1, null);
						CombatCharacter enemyChar = this.CurrentCombatDomain.GetCombatCharacter(!this.CombatChar.IsAlly, true);
						this._animalChar.NormalAttackHitType = this.CurrentCombatDomain.GetAttackHitType(this._animalChar, this._trickType);
						this._animalChar.NormalAttackBodyPart = this.CurrentCombatDomain.GetAttackBodyPart(this._animalChar, enemyChar, context2.Random, -1, this._trickType, -1);
						this.CurrentCombatDomain.UpdateDamageCompareData(context2);
						this.CurrentCombatDomain.CalcNormalAttack(context2, this._trickType);
					}
				}
				bool flag6 = this._animalLeaveFrame > 0;
				if (flag6)
				{
					this._animalLeaveFrame -= 1;
					bool flag7 = this._animalLeaveFrame == 0;
					if (flag7)
					{
						DataContext context3 = this.CombatChar.GetDataContext();
						this._animalChar.SetDisplayPosition(int.MinValue, context3);
					}
				}
				bool flag8 = this._stateTotalFrame > 0;
				if (flag8)
				{
					this._stateTotalFrame -= 1;
					bool flag9 = this._stateTotalFrame == 0;
					if (flag9)
					{
						this._animalChar.SetVisible(false, this.CombatChar.GetDataContext());
						this.CombatChar.StateMachine.TranslateState();
					}
				}
				result = false;
			}
			return result;
		}

		// Token: 0x04001A79 RID: 6777
		private CombatCharacter _animalChar;

		// Token: 0x04001A7A RID: 6778
		private sbyte _trickType;

		// Token: 0x04001A7B RID: 6779
		private string _attackAni;

		// Token: 0x04001A7C RID: 6780
		private string _attackParticle;

		// Token: 0x04001A7D RID: 6781
		private string _attackSound;

		// Token: 0x04001A7E RID: 6782
		private short _animalEnterFrame;

		// Token: 0x04001A7F RID: 6783
		private short _attackDamageFrame;

		// Token: 0x04001A80 RID: 6784
		private short _animalLeaveFrame;

		// Token: 0x04001A81 RID: 6785
		private short _stateTotalFrame;
	}
}
