using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using Config;
using GameData.ArchiveData;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Common.SingleValueCollection;
using GameData.Dependencies;
using GameData.DomainEvents;
using GameData.Domains.Character;
using GameData.Domains.Character.Ai;
using GameData.Domains.Character.Display;
using GameData.Domains.Character.Relation;
using GameData.Domains.Combat.Ai;
using GameData.Domains.Combat.MixPoison;
using GameData.Domains.Combat.Profession;
using GameData.Domains.CombatSkill;
using GameData.Domains.Extra;
using GameData.Domains.Global;
using GameData.Domains.Item;
using GameData.Domains.Item.Display;
using GameData.Domains.Map;
using GameData.Domains.Organization;
using GameData.Domains.SpecialEffect;
using GameData.Domains.SpecialEffect.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Tutorial;
using GameData.Domains.SpecialEffect.CombatSkill.Wuxianjiao.WugEffect;
using GameData.Domains.SpecialEffect.CombatSkill.Xuehoujiao.BreakBodyEffect;
using GameData.Domains.SpecialEffect.SectStory.Baihua;
using GameData.Domains.SpecialEffect.SectStory.Fulong;
using GameData.Domains.Taiwu;
using GameData.Domains.TaiwuEvent;
using GameData.Domains.World;
using GameData.Domains.World.Notification;
using GameData.Domains.World.SectMainStory;
using GameData.GameDataBridge;
using GameData.Serializer;
using GameData.Utilities;
using Redzen.Random;

namespace GameData.Domains.Combat
{
	// Token: 0x020006B1 RID: 1713
	[GameDataDomain(8)]
	public class CombatDomain : BaseGameDataDomain
	{
		// Token: 0x170003CB RID: 971
		// (get) Token: 0x060062FC RID: 25340 RVA: 0x00382609 File Offset: 0x00380809
		private bool EnemyUnyieldingFallen
		{
			get
			{
				return this.GetIsPlaygroundCombat() && DomainManager.Extra.GetEnemyUnyieldingFallen();
			}
		}

		// Token: 0x170003CC RID: 972
		// (get) Token: 0x060062FD RID: 25341 RVA: 0x00382620 File Offset: 0x00380820
		private bool EnemyEnableAi
		{
			get
			{
				return this._enableEnemyAi && (!this.GetIsPlaygroundCombat() || !DomainManager.Extra.GetDisableEnemyAi());
			}
		}

		// Token: 0x170003CD RID: 973
		// (get) Token: 0x060062FE RID: 25342 RVA: 0x00382645 File Offset: 0x00380845
		// (set) Token: 0x060062FF RID: 25343 RVA: 0x0038264D File Offset: 0x0038084D
		public bool Started { get; private set; }

		// Token: 0x170003CE RID: 974
		// (get) Token: 0x06006300 RID: 25344 RVA: 0x00382656 File Offset: 0x00380856
		// (set) Token: 0x06006301 RID: 25345 RVA: 0x0038265E File Offset: 0x0038085E
		public bool Pause { get; private set; }

		// Token: 0x06006302 RID: 25346 RVA: 0x00382667 File Offset: 0x00380867
		private void OnInitializedDomainData()
		{
			CombatDomain._handlersCombatCharAboutToFall = null;
		}

		// Token: 0x06006303 RID: 25347 RVA: 0x00382670 File Offset: 0x00380870
		private void InitializeOnInitializeGameDataModule()
		{
			sbyte bossId = 0;
			while ((int)bossId < Boss.Instance.Count)
			{
				short[] charIdList = Boss.Instance[bossId].CharacterIdList;
				foreach (short charId in charIdList)
				{
					CombatDomain.CharId2BossId[charId] = bossId;
				}
				bossId += 1;
			}
			foreach (TeammateCommandItem cmd in ((IEnumerable<TeammateCommandItem>)TeammateCommand.Instance))
			{
				ETeammateCommandOption option;
				bool flag = CombatDomain.TeammateCommandOptions.TryGetValue(cmd.Implement, out option) && option != cmd.Option;
				if (flag)
				{
					short predefinedLogId = 8;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(39, 2);
					defaultInterpolatedStringHandler.AppendLiteral("cmd implement mapping to multi option ");
					defaultInterpolatedStringHandler.AppendFormatted<ETeammateCommandOption>(option);
					defaultInterpolatedStringHandler.AppendLiteral(" ");
					defaultInterpolatedStringHandler.AppendFormatted<ETeammateCommandOption>(cmd.Option);
					PredefinedLog.Show(predefinedLogId, defaultInterpolatedStringHandler.ToStringAndClear());
				}
				else
				{
					CombatDomain.TeammateCommandOptions[cmd.Implement] = cmd.Option;
				}
			}
			GameData.Domains.Combat.SharedConstValue.InitializeCharId2AnimalIdCache();
			CombatDomain.BindMixPoisonEffectImplements();
			AiNodeFactory.Register(base.GetType().Assembly);
			AiActionFactory.Register(base.GetType().Assembly);
			AiConditionFactory.Register(base.GetType().Assembly);
		}

		// Token: 0x06006304 RID: 25348 RVA: 0x003827EC File Offset: 0x003809EC
		private void InitializeOnEnterNewWorld()
		{
		}

		// Token: 0x06006305 RID: 25349 RVA: 0x003827EF File Offset: 0x003809EF
		private void OnLoadedArchiveData()
		{
		}

		// Token: 0x06006306 RID: 25350 RVA: 0x003827F4 File Offset: 0x003809F4
		[DomainMethod]
		public int SimulatePrepareCombat(DataContext context, short combatConfigId, int[] enemyTeam)
		{
			CombatConfigItem combatConfig = Config.CombatConfig.Instance[combatConfigId];
			bool flag = combatConfig == null;
			int result;
			if (flag)
			{
				result = -1;
			}
			else
			{
				int[] selfTeam;
				this.ProcessCombatTeam(context, combatConfigId, enemyTeam, out selfTeam);
				result = (int)CFormulaHelper.RandomPrepareResult(selfTeam, enemyTeam, null);
			}
			return result;
		}

		// Token: 0x06006307 RID: 25351 RVA: 0x00382834 File Offset: 0x00380A34
		private void ProcessCombatTeam(DataContext context, short combatConfigId, int[] enemyTeam, out int[] selfTeam)
		{
			CombatConfigItem combatConfig = Config.CombatConfig.Instance[combatConfigId];
			selfTeam = new int[4];
			selfTeam[0] = DomainManager.Taiwu.GetTaiwuCharId();
			bool allowGroupMember = combatConfig.AllowGroupMember;
			if (allowGroupMember)
			{
				int teamIndex = 1;
				for (int i = 0; i < 3; i++)
				{
					int teammateId = DomainManager.Taiwu.GetElement_CombatGroupCharIds(i);
					bool flag = teammateId < 0 || enemyTeam.Exist(teammateId);
					if (!flag)
					{
						selfTeam[teamIndex++] = teammateId;
					}
				}
				bool flag2;
				if (combatConfig.AllowVitalDemon)
				{
					Dictionary<SectStoryThreeVitalsCharacterType, int> vitalTeammateData = this._vitalTeammateData;
					flag2 = (vitalTeammateData != null && vitalTeammateData.Count > 0);
				}
				else
				{
					flag2 = false;
				}
				bool flag3 = flag2;
				if (flag3)
				{
					foreach (KeyValuePair<SectStoryThreeVitalsCharacterType, int> keyValuePair in this._vitalTeammateData)
					{
						SectStoryThreeVitalsCharacterType sectStoryThreeVitalsCharacterType;
						int num;
						keyValuePair.Deconstruct(out sectStoryThreeVitalsCharacterType, out num);
						SectStoryThreeVitalsCharacterType type = sectStoryThreeVitalsCharacterType;
						int index = num;
						int[] array = selfTeam;
						int num2;
						if (index >= teamIndex)
						{
							teamIndex = (num2 = teamIndex) + 1;
						}
						else
						{
							num2 = index;
						}
						array[num2] = DomainManager.Extra.GetVitalCharacterByType(context, type).GetId();
					}
				}
				for (int j = teamIndex; j < selfTeam.Length; j++)
				{
					selfTeam[j] = -1;
				}
			}
			else
			{
				for (int k = 1; k < 4; k++)
				{
					selfTeam[k] = -1;
					bool flag4 = enemyTeam.Length > k;
					if (flag4)
					{
						enemyTeam[k] = -1;
					}
				}
			}
		}

		// Token: 0x06006308 RID: 25352 RVA: 0x003829B8 File Offset: 0x00380BB8
		private static void ProcessTemporaryFavorability(DataContext context, IReadOnlyList<int> enemyTeam)
		{
			int mainCharId = enemyTeam[0];
			for (int i = 1; i < enemyTeam.Count; i++)
			{
				int enemyCharId = enemyTeam[i];
				bool flag = enemyCharId < 0;
				if (!flag)
				{
					GameData.Domains.Character.Character enemyChar = DomainManager.Character.GetElement_Objects(enemyCharId);
					bool flag2 = enemyChar.GetCreatingType() == 1;
					if (!flag2)
					{
						short existFavorability = DomainManager.Character.GetFavorability(mainCharId, enemyCharId);
						bool flag3 = existFavorability != short.MinValue;
						if (!flag3)
						{
							short favorability = enemyChar.GetRandomFavorability(context.Random);
							DomainManager.Character.DirectlySetFavorabilities(context, mainCharId, enemyCharId, favorability, favorability);
						}
					}
				}
			}
		}

		// Token: 0x06006309 RID: 25353 RVA: 0x00382A60 File Offset: 0x00380C60
		[DomainMethod]
		public TeammateCommandChangeData ProcessCombatTeammateCommands(DataContext context, short combatConfigId, int[] enemyTeam)
		{
			int[] selfTeam;
			this.ProcessCombatTeam(context, combatConfigId, enemyTeam, out selfTeam);
			CombatConfigItem config = Config.CombatConfig.Instance[combatConfigId];
			bool allowRandomFavorability = config.AllowRandomFavorability;
			if (allowRandomFavorability)
			{
				CombatDomain.ProcessTemporaryFavorability(context, enemyTeam);
			}
			this.PreRandomizedTeammateCommandReplaceData = new TeammateCommandChangeData
			{
				SelfTeam = CombatDomain.CalcTeammateBetrayData(context, combatConfigId, selfTeam, true),
				EnemyTeam = CombatDomain.CalcTeammateBetrayData(context, combatConfigId, enemyTeam, false)
			};
			bool allowVitalDemonBetray = config.AllowVitalDemonBetray;
			if (allowVitalDemonBetray)
			{
				this.ProcessVitalDemonBetray(context, this.PreRandomizedTeammateCommandReplaceData.EnemyTeam);
			}
			return this.PreRandomizedTeammateCommandReplaceData;
		}

		// Token: 0x0600630A RID: 25354 RVA: 0x00382AEC File Offset: 0x00380CEC
		[DomainMethod]
		public void PrepareEnemyEquipments(DataContext context, short combatConfigId, List<int> enemyList)
		{
			GameData.Domains.Character.Character mainChar = DomainManager.Character.GetElement_Objects(enemyList[0]);
			short charTemplateId = mainChar.GetTemplateId();
			bool isBoss = CombatDomain.CharId2BossId.ContainsKey(charTemplateId);
			bool isAnimal = GameData.Domains.Combat.SharedConstValue.CharId2AnimalId.ContainsKey(charTemplateId);
			bool flag = isBoss || isAnimal || DomainManager.Taiwu.GetGroupCharIds().Contains(mainChar.GetId());
			if (!flag)
			{
				for (int i = 0; i < enemyList.Count; i++)
				{
					GameData.Domains.Character.Character enemyChar = DomainManager.Character.GetElement_Objects(enemyList[i]);
					context.Equipping.SelectEquipmentsByCombatConfig(context, enemyChar, combatConfigId, true, false);
				}
			}
		}

		// Token: 0x0600630B RID: 25355 RVA: 0x00382B98 File Offset: 0x00380D98
		[DomainMethod]
		public CharacterDisplayData ApplyVitalOnTeammate(DataContext context, int typeInt, int index)
		{
			bool flag = index <= 0 || index >= 4;
			bool flag2 = flag;
			CharacterDisplayData result;
			if (flag2)
			{
				result = null;
			}
			else
			{
				bool flag3 = this.PreRandomizedTeammateCommandReplaceData == null;
				if (flag3)
				{
					result = null;
				}
				else
				{
					bool vitalIsDemon = DomainManager.Extra.AreVitalsDemon();
					SectStoryThreeVitalsCharacter vitalData = DomainManager.Extra.GetVitalByType((SectStoryThreeVitalsCharacterType)typeInt);
					bool flag4 = vitalData == null || !vitalData.AllowAsTeammate(vitalIsDemon);
					if (flag4)
					{
						result = null;
					}
					else
					{
						Dictionary<SectStoryThreeVitalsCharacterType, int> vitalTeammateData = this._vitalTeammateData;
						bool flag5 = vitalTeammateData != null && vitalTeammateData.ContainsValue(index);
						if (flag5)
						{
							result = null;
						}
						else
						{
							if (this._vitalTeammateData == null)
							{
								this._vitalTeammateData = new Dictionary<SectStoryThreeVitalsCharacterType, int>();
							}
							this._vitalTeammateData[(SectStoryThreeVitalsCharacterType)typeInt] = index;
							GameData.Domains.Character.Character vitalCharacter = DomainManager.Extra.GetVitalCharacterByType(context, (SectStoryThreeVitalsCharacterType)typeInt);
							this.JoinSpecialGroup(context, vitalCharacter.GetId());
							result = DomainManager.Character.GetCharacterDisplayData(vitalCharacter.GetId());
						}
					}
				}
			}
			return result;
		}

		// Token: 0x0600630C RID: 25356 RVA: 0x00382C84 File Offset: 0x00380E84
		[DomainMethod]
		public int RevertVitalOnTeammate(DataContext context, int typeInt)
		{
			bool flag = this._vitalTeammateData == null || this.PreRandomizedTeammateCommandReplaceData == null;
			int result;
			if (flag)
			{
				result = -1;
			}
			else
			{
				int index;
				bool flag2 = !this._vitalTeammateData.Remove((SectStoryThreeVitalsCharacterType)typeInt, out index);
				if (flag2)
				{
					result = -1;
				}
				else
				{
					GameData.Domains.Character.Character vitalCharacter = DomainManager.Extra.GetVitalCharacterByType(context, (SectStoryThreeVitalsCharacterType)typeInt);
					this.ExitSpecialGroup(context, vitalCharacter.GetId());
					foreach (SectStoryThreeVitalsCharacterType otherType in this._vitalTeammateData.Keys)
					{
						bool flag3 = this._vitalTeammateData[otherType] > index;
						if (flag3)
						{
							Dictionary<SectStoryThreeVitalsCharacterType, int> vitalTeammateData = this._vitalTeammateData;
							SectStoryThreeVitalsCharacterType key = otherType;
							vitalTeammateData[key]--;
						}
					}
					result = index;
				}
			}
			return result;
		}

		// Token: 0x0600630D RID: 25357 RVA: 0x00382D74 File Offset: 0x00380F74
		[DomainMethod]
		public sbyte PrepareCombat(DataContext context, short combatConfigId, int[] enemyTeam)
		{
			int[] selfTeam;
			this.ProcessCombatTeam(context, combatConfigId, enemyTeam, out selfTeam);
			return this.PrepareCombat(context, combatConfigId, selfTeam, enemyTeam);
		}

		// Token: 0x0600630E RID: 25358 RVA: 0x00382D9C File Offset: 0x00380F9C
		public sbyte PrepareCombat(DataContext context, short combatConfigId, int[] selfTeam, int[] enemyTeam)
		{
			this.CombatConfig = Config.CombatConfig.Instance[combatConfigId];
			this.SetCombatStatus(CombatStatusType.NotInCombat, context);
			this.SetWaitingDelaySettlement(false, context);
			this.SetTimeScale(0f, context);
			this.SetAutoCombat(false, context);
			this.SetCombatFrame(0UL, context);
			this.SetCombatType(this.CombatConfig.CombatType, context);
			this.SetShowMercyOption(context, EShowMercyOption.Invalid);
			this.SetSelectedMercyOption(context, EShowMercySelect.Unselected);
			this.SetSkillAttackedIndexAndHit(new IntPair(-1, 0), context);
			bool flag = this.CombatConfig.InitDistance < (sbyte)this.CombatConfig.MinDistance || this.CombatConfig.InitDistance > (sbyte)this.CombatConfig.MaxDistance;
			if (flag)
			{
				ValueTuple<byte, byte> distanceRange = this.GetDistanceRange();
				short midDistance = (short)((distanceRange.Item1 + distanceRange.Item2) / 2);
				List<short> distanceRandomPool = ObjectPool<List<short>>.Instance.Get();
				distanceRandomPool.Clear();
				for (int i = -2; i <= 2; i++)
				{
					short distance = (short)((int)midDistance + 10 * i);
					bool flag2 = distance < (short)distanceRange.Item1;
					if (flag2)
					{
						distance = (short)distanceRange.Item1;
					}
					else
					{
						bool flag3 = distance > (short)distanceRange.Item2;
						if (flag3)
						{
							distance = (short)distanceRange.Item2;
						}
					}
					bool flag4 = !distanceRandomPool.Contains(distance);
					if (flag4)
					{
						distanceRandomPool.Add(distance);
					}
				}
				this.SetCurrentDistance(distanceRandomPool.GetRandom(context.Random), context);
				ObjectPool<List<short>>.Instance.Return(distanceRandomPool);
			}
			else
			{
				this.SetCurrentDistance((short)this.CombatConfig.InitDistance, context);
			}
			this._frameTimeAccumulator = 0f;
			this._bgmIndex = 0;
			this.Started = false;
			this._inBulletTime = false;
			this._showUseGoldenWire = 0;
			this.Context = context;
			this._isTutorialCombat = (DomainManager.Character.GetElement_Objects(selfTeam[0]).GetTemplateId() == 444);
			this.ClearCombatCharacterDict();
			for (int j = 0; j < 4; j++)
			{
				this._selfTeam[j] = ((j < selfTeam.Length && selfTeam[j] >= 0) ? selfTeam[j] : -1);
				this._enemyTeam[j] = ((j < enemyTeam.Length && enemyTeam[j] >= 0) ? enemyTeam[j] : -1);
			}
			int taiwuCharId = DomainManager.Taiwu.GetTaiwuCharId();
			for (int k = 0; k < 4; k++)
			{
				int selfCharId = this._selfTeam[k];
				int enemyCharId = this._enemyTeam[k];
				bool flag5 = selfCharId >= 0;
				if (flag5)
				{
					CombatCharacter combatChar = new CombatCharacter();
					combatChar.IsAlly = true;
					combatChar.IsTaiwu = (taiwuCharId == selfCharId);
					this.AddElement_CombatCharacterDict(selfCharId, combatChar);
					combatChar.Init(this, selfCharId, context);
				}
				bool flag6 = enemyCharId >= 0;
				if (flag6)
				{
					CombatCharacter combatChar2 = new CombatCharacter();
					combatChar2.IsAlly = false;
					combatChar2.IsTaiwu = (taiwuCharId == enemyCharId);
					this.AddElement_CombatCharacterDict(enemyCharId, combatChar2);
					combatChar2.Init(this, enemyCharId, context);
				}
			}
			this.InitSkillData(context);
			this.InitWeaponData(context);
			this._selfChar = (this._enemyChar = null);
			this.SetCombatCharacter(context, true, this._selfTeam[0]);
			this.SetCombatCharacter(context, false, this._enemyTeam[0]);
			this.InitEquipmentDurability();
			for (sbyte skillType = 0; skillType < 14; skillType += 1)
			{
				int bookCharId = DomainManager.LegendaryBook.GetOwner(skillType);
				bool flag7 = bookCharId >= 0 && bookCharId != this._selfTeam[0] && this._combatCharacterDict.ContainsKey(bookCharId);
				if (flag7)
				{
					this.AddCombatState(context, this._combatCharacterDict[bookCharId], 0, (short)(117 + skillType), 100, false, true, bookCharId);
				}
			}
			foreach (CombatCharacter combatChar3 in this._combatCharacterDict.Values)
			{
				combatChar3.AiController = new AiController(combatChar3);
				combatChar3.AiController.Init();
			}
			this.PrepareCombatSpecial(context);
			this.PrepareCombatProfession(context);
			this.PrepareCombatSectStory(context, combatConfigId);
			int selfWisdom = this.GetTeamWisdomCount(true);
			int enemyWisdom = this.GetTeamWisdomCount(false);
			this.SetSelfTeamWisdomType((selfWisdom > 0) ? 0 : ((selfWisdom < 0) ? 1 : -1), context);
			this.SetSelfTeamWisdomCount((short)Math.Abs(selfWisdom), context);
			this.SetEnemyTeamWisdomType((enemyWisdom > 0) ? 0 : ((enemyWisdom < 0) ? 1 : -1), context);
			this.SetEnemyTeamWisdomCount((short)Math.Abs(enemyWisdom), context);
			EPrepareCombatResult firstMoveType = CFormulaHelper.RandomPrepareResult(this._selfTeam, this._enemyTeam, context.Random);
			bool firstMoveIsAlly = firstMoveType == EPrepareCombatResult.SelfFirst;
			this._selfChar.SetBreathValue(30000 * (firstMoveIsAlly ? 30 : 0) / 100, context);
			this._selfChar.SetStanceValue(4000 * (firstMoveIsAlly ? 30 : 0) / 100, context);
			this._enemyChar.SetBreathValue(30000 * ((!firstMoveIsAlly) ? 30 : 0) / 100, context);
			this._enemyChar.SetStanceValue(4000 * ((!firstMoveIsAlly) ? 30 : 0) / 100, context);
			CombatCharacter afterMoveChar = firstMoveIsAlly ? this._enemyChar : this._selfChar;
			this.ChangeMobilityValue(context, afterMoveChar, -MoveSpecialConstants.MaxMobility * 50 / 100, false, null, false);
			foreach (CombatCharacter combatChar4 in this._combatCharacterDict.Values)
			{
				bool flag8 = !this.IsMainCharacter(combatChar4);
				if (flag8)
				{
					combatChar4.InitTeammateCommand(context, combatChar4.IsAlly == firstMoveIsAlly);
				}
			}
			this.UpdateAllCommandAvailability(context, this._selfChar);
			this.UpdateAllCommandAvailability(context, this._enemyChar);
			this._combatResultData.Reset();
			this._skillCastTimes.Clear();
			this._lootCharList.Clear();
			this.SelfMaxSkillGrade = -1;
			this.EnemyMaxSkillGrade = -1;
			this.SelfAvgEquipGrade = 0f;
			this.EnemyAvgEquipGrade = 0f;
			int selfEquipCount = 0;
			int enemyEquipCount = 0;
			foreach (CombatCharacter combatChar5 in this._combatCharacterDict.Values)
			{
				ItemKey[] equips = combatChar5.GetCharacter().GetEquipment();
				for (sbyte slot = 0; slot < 12; slot += 1)
				{
					bool flag9 = slot == 4 || slot == 11;
					if (!flag9)
					{
						ItemKey equipKey = equips[(int)slot];
						bool flag10 = !equipKey.IsValid();
						if (!flag10)
						{
							ItemBase equipItem = DomainManager.Item.GetBaseItem(equipKey);
							bool flag11 = equipItem.GetMaxDurability() >= 0 && equipItem.GetCurrDurability() <= 0;
							if (!flag11)
							{
								sbyte grade = ItemTemplateHelper.GetGrade(equipKey.ItemType, equipKey.TemplateId);
								bool isAlly = combatChar5.IsAlly;
								if (isAlly)
								{
									this.SelfAvgEquipGrade += (float)(grade + 1);
									selfEquipCount++;
								}
								else
								{
									this.EnemyAvgEquipGrade += (float)(grade + 1);
									enemyEquipCount++;
								}
							}
						}
					}
				}
			}
			this.SelfAvgEquipGrade /= (float)Math.Max(1, selfEquipCount);
			this.EnemyAvgEquipGrade /= (float)Math.Max(1, enemyEquipCount);
			bool isTutorialCombat = this._isTutorialCombat;
			if (isTutorialCombat)
			{
				this._selfChar.SetBreathValue(30000, context);
				this._selfChar.SetStanceValue(4000, context);
			}
			bool flag12 = combatConfigId == 125;
			if (flag12)
			{
				this.ClearMobilityAndForbidRecover(context, this._enemyCharId);
			}
			this.SetCombatStatus(CombatStatusType.InCombat, context);
			foreach (CombatCharacter combatChar6 in this._combatCharacterDict.Values)
			{
				this.UpdateBodyDefeatMark(context, combatChar6);
				this.UpdatePoisonDefeatMark(context, combatChar6);
				this.UpdateOtherDefeatMark(context, combatChar6);
			}
			this.UpdateAllTeammateCommandUsable(context, true, -1);
			this.UpdateAllTeammateCommandUsable(context, false, -1);
			this.PreRandomizedTeammateCommandReplaceData = null;
			this._vitalTeammateData = null;
			this._expectRatioData.Clear();
			this.SetExpectRatioData(this._expectRatioData, context);
			return (sbyte)firstMoveType;
		}

		// Token: 0x0600630F RID: 25359 RVA: 0x00383688 File Offset: 0x00381888
		private void PrepareCombatSpecial(DataContext context)
		{
			foreach (CombatCharacter combatChar in this._combatCharacterDict.Values)
			{
				ItemKey carrierItemKey = combatChar.GetCharacter().GetEquipment()[11];
				bool flag = !DomainManager.Extra.IsCarrierFullTamePoint(carrierItemKey);
				if (!flag)
				{
					GameData.Domains.Item.Carrier carrier;
					bool flag2 = !DomainManager.Item.TryGetElement_Carriers(carrierItemKey.Id, out carrier) || carrier.GetCurrDurability() <= 0;
					if (!flag2)
					{
						short carrierId = carrierItemKey.TemplateId;
						string effectName;
						bool flag3 = GameData.Domains.Combat.SharedConstValue.AnimalCarrier2Effect.TryGetValue(carrierId, out effectName);
						if (flag3)
						{
							DomainManager.SpecialEffect.Add(context, combatChar.GetId(), effectName);
						}
					}
				}
			}
		}

		// Token: 0x06006310 RID: 25360 RVA: 0x00383768 File Offset: 0x00381968
		private void PrepareCombatProfession(DataContext context)
		{
			bool flag = DomainManager.Extra.IsProfessionalSkillUnlockedAndEquipped(3) && this._enemyChar.IsAnimal;
			if (flag)
			{
				DomainManager.SpecialEffect.Add(context, this._selfChar.GetId(), "Profession.Hunter.HuntingBeasts");
			}
			ItemKey carrier = this._selfChar.GetCharacter().GetEquipment()[11];
			short carrierId = carrier.TemplateId;
			bool canUse = carrier.IsValid() && !DomainManager.Item.GetBaseItem(carrier).IsDurabilityRunningOut();
			bool flag2 = DomainManager.Extra.IsProfessionalSkillUnlockedAndEquipped(4) && carrierId >= 0 && Config.Carrier.Instance[carrierId].CharacterIdInCombat >= 0 && canUse;
			if (flag2)
			{
				GameData.Domains.Character.Character animalChar = DomainManager.Character.CreateFixedEnemy(context, Config.Carrier.Instance[carrierId].CharacterIdInCombat);
				DomainManager.Character.CompleteCreatingCharacter(animalChar.GetId());
				CombatCharacter animalCombatChar = new CombatCharacter();
				animalCombatChar.IsAlly = true;
				animalCombatChar.IsTaiwu = false;
				this.AddElement_CombatCharacterDict(animalChar.GetId(), animalCombatChar);
				animalCombatChar.Init(this, animalChar.GetId(), context);
				animalCombatChar.Immortal = true;
				animalCombatChar.SetVisible(false, context);
				animalCombatChar.SetCanAttackOutRange(true, context);
				animalCombatChar.SetUsingWeaponIndex(0, context);
				animalCombatChar.SetAnimationToLoop(this.GetIdleAni(animalCombatChar), context);
				for (int i = 0; i < 3; i++)
				{
					ItemKey weaponKey = animalCombatChar.GetWeapons()[i];
					bool flag3 = weaponKey.IsValid();
					if (flag3)
					{
						List<sbyte> weaponTricks = DomainManager.Item.GetWeaponTricks(weaponKey);
						CombatWeaponData weaponData = new CombatWeaponData(weaponKey, animalCombatChar);
						sbyte[] trickList = weaponData.GetWeaponTricks();
						this.AddElement_WeaponDataDict(weaponKey.Id, weaponData);
						weaponData.Init(context, i);
						for (int j = 0; j < weaponTricks.Count; j++)
						{
							trickList[j] = weaponTricks[j];
						}
					}
				}
				this.SetCarrierAnimalCombatCharId(animalCombatChar.GetId(), context);
			}
			else
			{
				this.SetCarrierAnimalCombatCharId(-1, context);
			}
			Location location = this._selfChar.GetCharacter().GetLocation();
			bool flag4 = DomainManager.Extra.IsProfessionalSkillUnlockedAndEquipped(2) && location.IsValid();
			if (flag4)
			{
				List<MapBlockData> neighborBlocks = ObjectPool<List<MapBlockData>>.Instance.Get();
				int steps = 1;
				DomainManager.Map.GetRealNeighborBlocks(location.AreaId, location.BlockId, neighborBlocks, steps, true);
				HashSet<string> savageEffects = ObjectPool<HashSet<string>>.Instance.Get();
				savageEffects.Clear();
				foreach (MapBlockData block in neighborBlocks)
				{
					string effect;
					bool flag5 = GameData.Domains.Combat.SharedConstValue.MapBlockSubType2SavageEffect.TryGetValue(block.BlockSubType, out effect);
					if (flag5)
					{
						savageEffects.Add(effect);
					}
				}
				foreach (string effect2 in savageEffects)
				{
					DomainManager.SpecialEffect.Add(context, this._selfChar.GetId(), effect2);
				}
				ObjectPool<List<MapBlockData>>.Instance.Return(neighborBlocks);
				ObjectPool<HashSet<string>>.Instance.Return(savageEffects);
			}
		}

		// Token: 0x06006311 RID: 25361 RVA: 0x00383ACC File Offset: 0x00381CCC
		private void PrepareCombatSectStory(DataContext context, short combatConfigId)
		{
			bool flag = combatConfigId >= 164 && combatConfigId <= 166;
			if (flag)
			{
				GameData.Domains.Character.Character liaoWumingChar = DomainManager.Character.CreateFixedCharacter(context, 515);
				DomainManager.Character.CompleteCreatingCharacter(liaoWumingChar.GetId());
				CombatCharacter liaoWumingCharCombatChar = new CombatCharacter();
				liaoWumingCharCombatChar.IsAlly = true;
				liaoWumingCharCombatChar.IsTaiwu = false;
				this.AddElement_CombatCharacterDict(liaoWumingChar.GetId(), liaoWumingCharCombatChar);
				liaoWumingCharCombatChar.Init(this, liaoWumingChar.GetId(), context);
				liaoWumingCharCombatChar.SetVisible(false, context);
				this.SetSpecialShowCombatCharId(liaoWumingCharCombatChar.GetId(), context);
				this._selfChar.NeedEnterSpecialShow = true;
			}
			else
			{
				this.SetSpecialShowCombatCharId(-1, context);
			}
			short enemyTemplateId = this._enemyChar.GetCharacter().GetTemplateId();
			bool flag2 = 514 <= enemyTemplateId && enemyTemplateId <= 518;
			if (flag2)
			{
				this.AddCombatState(context, this._enemyChar, 0, 145);
			}
			bool flag3 = combatConfigId == 198;
			if (flag3)
			{
				int charId = this._selfTeam[0];
				XiongZhongSiQi effect = new XiongZhongSiQi(charId);
				DomainManager.SpecialEffect.Add(context, effect);
			}
			bool flag4 = combatConfigId == 203;
			if (flag4)
			{
				int charId2 = this._selfTeam[0];
				SiQiDuoHun effect2 = new SiQiDuoHun(charId2);
				DomainManager.SpecialEffect.Add(context, effect2);
			}
			bool flag5 = combatConfigId == 204;
			if (flag5)
			{
				EventArgBox sectArgBox = DomainManager.Extra.GetSectMainStoryEventArgBox(3);
				SByteList list;
				sectArgBox.Get<SByteList>("ConchShip_PresetKey_BaihuaAdventureFinialWinSect", out list);
				List<sbyte> items = list.Items;
				bool flag6 = items != null && items.Count > 0;
				if (flag6)
				{
					foreach (sbyte orgTemplateId in list.Items)
					{
						this.PrepareCombatSectStoryTryCreateBaihuaLegacyPower(context, orgTemplateId);
					}
				}
			}
			bool flag7 = combatConfigId == 205;
			if (flag7)
			{
				int charId3 = this._selfTeam[0];
				SoulWitheringBell effect3 = new SoulWitheringBell(charId3);
				DomainManager.SpecialEffect.Add(context, effect3);
			}
			SectShaolinDemonSlayerData slayerTrial = DomainManager.Extra.GetSectShaolinDemonSlayerData();
			List<SpecialEffectBase> trialingRestrictEffects = slayerTrial.TrialingRestrictEffects;
			bool flag8 = trialingRestrictEffects != null && trialingRestrictEffects.Count > 0;
			if (flag8)
			{
				foreach (SpecialEffectBase effect4 in slayerTrial.TrialingRestrictEffects)
				{
					DomainManager.SpecialEffect.Add(context, effect4);
				}
			}
			slayerTrial.TrialingRestrictEffects = null;
		}

		// Token: 0x06006312 RID: 25362 RVA: 0x00383D70 File Offset: 0x00381F70
		private void PrepareCombatSectStoryTryCreateBaihuaLegacyPower(DataContext context, sbyte orgTemplateId)
		{
			int taiwuCharId = this._selfTeam[0];
			if (!true)
			{
			}
			SpecialEffectBase specialEffectBase;
			switch (orgTemplateId)
			{
			case 1:
				specialEffectBase = new LegacyPowerShaolin(taiwuCharId);
				break;
			case 2:
				specialEffectBase = new LegacyPowerEmei(taiwuCharId);
				break;
			case 3:
				specialEffectBase = new LegacyPowerBaihua(taiwuCharId);
				break;
			case 4:
				specialEffectBase = new LegacyPowerWudang(taiwuCharId);
				break;
			case 5:
				specialEffectBase = new LegacyPowerYuanshan(taiwuCharId);
				break;
			case 6:
				specialEffectBase = new LegacyPowerShixiang(taiwuCharId);
				break;
			case 7:
				specialEffectBase = new LegacyPowerRanshan(taiwuCharId);
				break;
			case 8:
				specialEffectBase = new LegacyPowerXuannv(taiwuCharId);
				break;
			case 9:
				specialEffectBase = new LegacyPowerZhujian(taiwuCharId);
				break;
			case 10:
				specialEffectBase = new LegacyPowerKongsang(taiwuCharId);
				break;
			case 11:
				specialEffectBase = new LegacyPowerJingang(taiwuCharId);
				break;
			case 12:
				specialEffectBase = new LegacyPowerWuxian(taiwuCharId);
				break;
			case 13:
				specialEffectBase = new LegacyPowerJieqing(taiwuCharId);
				break;
			case 14:
				specialEffectBase = new LegacyPowerFulong(taiwuCharId);
				break;
			case 15:
				specialEffectBase = new LegacyPowerXuehou(taiwuCharId);
				break;
			default:
				specialEffectBase = null;
				break;
			}
			if (!true)
			{
			}
			SpecialEffectBase effect = specialEffectBase;
			bool flag = effect == null;
			if (!flag)
			{
				DomainManager.SpecialEffect.Add(context, effect);
			}
		}

		// Token: 0x06006313 RID: 25363 RVA: 0x00383E80 File Offset: 0x00382080
		[DomainMethod]
		public bool StartCombat(DataContext context)
		{
			this.SetTimeScale(this._timeScale, context);
			bool flag = this.CombatConfig.StartInSecondPhase && this._enemyChar.BossConfig != null;
			if (flag)
			{
				CombatDomain.RaiseCombatCharAboutToFall(context, this._enemyChar, 3);
			}
			this._needCheckFallenCharSet.Clear();
			this.AddToCheckFallenSet(this._selfChar.GetId());
			this.AddToCheckFallenSet(this._enemyChar.GetId());
			Events.RaiseCombatBegin(context);
			Events.RaiseChangeNeiliAllocationAfterCombatBegin(context, this._selfChar, this._selfChar.GetNeiliAllocation());
			Events.RaiseChangeNeiliAllocationAfterCombatBegin(context, this._enemyChar, this._enemyChar.GetNeiliAllocation());
			Events.RaiseCreateGangqiAfterChangeNeiliAllocation(context, this._selfChar);
			Events.RaiseCreateGangqiAfterChangeNeiliAllocation(context, this._enemyChar);
			DomainManager.TaiwuEvent.OnEvent_CombatOpening(this._enemyCharId);
			this.EnsurePauseState();
			this.TestSkillCounter = 0;
			this.Started = true;
			bool flag2 = this.AiOptions.SaveMoveTarget && DomainManager.Extra.GetLastTargetDistance() > 0;
			if (flag2)
			{
				this.SetTargetDistance(context, DomainManager.Extra.GetLastTargetDistance(), true);
			}
			return true;
		}

		// Token: 0x06006314 RID: 25364 RVA: 0x00383FB0 File Offset: 0x003821B0
		[DomainMethod]
		public void SetTimeScale(DataContext context, float timeScale)
		{
			bool flag = !this.IsInCombat() || this.CombatAboutToOver();
			if (!flag)
			{
				bool inBulletTime = this._inBulletTime;
				if (inBulletTime)
				{
					this._timeScaleSaveInBulletTime = timeScale;
					this.SetTimeScale((timeScale == 0f) ? 0f : 0.2f, context);
				}
				else
				{
					this.SetTimeScale(timeScale, context);
				}
			}
		}

		// Token: 0x06006315 RID: 25365 RVA: 0x00384011 File Offset: 0x00382211
		[DomainMethod]
		public void SetPlayerAutoCombat(DataContext context, bool autoCombat)
		{
			this.SetAutoCombat(autoCombat, context);
			this.SetMoveState(0, true, false);
		}

		// Token: 0x06006316 RID: 25366 RVA: 0x00384028 File Offset: 0x00382228
		[DomainMethod]
		public void EnterBossPuppetCombat(DataContext context, short puppetCharTemplateId, sbyte consummateLevel, bool playground = false)
		{
			int bossLevel = (int)(consummateLevel / 2 - 1);
			bool flag = !CombatDomain.BossPuppet2BossId.ContainsKey(puppetCharTemplateId) || bossLevel < 0 || bossLevel > 8;
			if (!flag)
			{
				short charTemplateId = (short)((int)CombatDomain.BossPuppet2BossId[puppetCharTemplateId] + bossLevel);
				GameData.Domains.Character.Character fixedCharacter;
				bool flag2 = DomainManager.Character.TryGetFixedCharacterByTemplateId(charTemplateId, out fixedCharacter);
				if (flag2)
				{
					DomainManager.Character.RemoveNonIntelligentCharacter(context, fixedCharacter);
				}
				GameData.Domains.Character.Character character = DomainManager.Character.CreateFixedCharacter(context, charTemplateId);
				character.OfflineSetXiangshuType(4);
				DomainManager.LegendaryBook.UpdateBossCharacterLegendaryBookFeatures(context, character);
				DomainManager.Character.CompleteCreatingCharacter(character.GetId());
				this.CombatEntry(context, new List<int>
				{
					character.GetId()
				}, Boss.Instance[CombatDomain.CharId2BossId[charTemplateId]].CombatConfig);
				this.SetIsPuppetCombat(true, context);
				this.SetIsPlaygroundCombat(playground, context);
			}
		}

		// Token: 0x06006317 RID: 25367 RVA: 0x00384108 File Offset: 0x00382308
		[DomainMethod]
		public void EnableBulletTime(DataContext context, bool enable)
		{
			bool flag = this._inBulletTime == enable;
			if (!flag)
			{
				this._inBulletTime = enable;
				if (enable)
				{
					this._timeScaleSaveInBulletTime = this._timeScale;
					bool flag2 = this._timeScale > 0f;
					if (flag2)
					{
						this.SetTimeScale(0.2f, context);
					}
				}
				else
				{
					this.SetTimeScale(this._timeScaleSaveInBulletTime, context);
				}
			}
		}

		// Token: 0x06006318 RID: 25368 RVA: 0x00384170 File Offset: 0x00382370
		public override void OnUpdate(DataContext context)
		{
			bool flag = this._timeScale <= 0f || !this.IsInCombat() || !this.Started;
			if (!flag)
			{
				this.Context = context;
				this._selfChar.OnFrameBegin();
				bool flag2 = this._selfChar.TeammateBeforeMainChar >= 0;
				if (flag2)
				{
					this._combatCharacterDict[this._selfChar.TeammateBeforeMainChar].OnFrameBegin();
				}
				this._enemyChar.OnFrameBegin();
				bool flag3 = this._enemyChar.TeammateBeforeMainChar >= 0;
				if (flag3)
				{
					this._combatCharacterDict[this._enemyChar.TeammateBeforeMainChar].OnFrameBegin();
				}
				this._frameTimeAccumulator += this._timeScale;
				bool flag4 = this._frameTimeAccumulator >= 1f;
				if (flag4)
				{
					int frameTimes = (int)this._frameTimeAccumulator;
					this._frameTimeAccumulator %= 1f;
					for (int i = 0; i < frameTimes; i++)
					{
						bool flag5 = this.IsInCombat();
						if (flag5)
						{
							this.CombatLoop(context);
						}
					}
				}
				this._selfChar.OnFrameEnd();
				this._enemyChar.OnFrameEnd();
			}
		}

		// Token: 0x06006319 RID: 25369 RVA: 0x003842B0 File Offset: 0x003824B0
		private void CombatLoop(DataContext context)
		{
			bool skipCombatLoop = this._skipCombatLoop;
			if (skipCombatLoop)
			{
				this._skipCombatLoop = false;
			}
			else
			{
				this._saveDyingEffectTriggerd = false;
				bool flag = this.CheckFallen(context);
				if (!flag)
				{
					bool flag2 = this.Pause == this._selfChar.StateMachine.GetCurrentState().IsUpdateOnPause;
					if (flag2)
					{
						bool flag3 = !this.Pause;
						if (flag3)
						{
							bool flag4 = this._autoCombat && !this._isTutorialCombat;
							if (flag4)
							{
								this._selfChar.AiController.Update(context);
							}
							else
							{
								this._selfChar.AiController.UpdateOnlyMove(context);
							}
						}
						this._selfChar.StateMachine.OnUpdate();
					}
					bool flag5 = this.IsInCombat() && this.Pause == this._enemyChar.StateMachine.GetCurrentState().IsUpdateOnPause;
					if (flag5)
					{
						bool flag6 = !this.Pause && this.EnemyEnableAi && (!this._isTutorialCombat || DomainManager.TutorialChapter.IsInTutorialChapter(7));
						if (flag6)
						{
							this._enemyChar.AiController.Update(context);
						}
						this._enemyChar.StateMachine.OnUpdate();
					}
					bool pause = this.Pause;
					if (!pause)
					{
						this.SetCombatFrame(this._combatFrame + 1UL, context);
						bool flag7 = this.CombatConfig.SelfForceDefeatFrame > 0U && this._combatFrame >= (ulong)this.CombatConfig.SelfForceDefeatFrame;
						if (flag7)
						{
							this.ForceDefeat(this.GetMainCharacter(true).GetId());
						}
						bool flag8 = this.CombatConfig.EnemyForceDefeatFrame > 0U && this._combatFrame >= (ulong)this.CombatConfig.EnemyForceDefeatFrame;
						if (flag8)
						{
							this.ForceDefeat(this.GetMainCharacter(false).GetId());
						}
					}
				}
			}
		}

		// Token: 0x0600631A RID: 25370 RVA: 0x00384498 File Offset: 0x00382698
		private bool CheckFallen(DataContext context)
		{
			bool flag = this._selfChar.StateMachine.GetCurrentState().RequireDelayFallen || this._enemyChar.StateMachine.GetCurrentState().RequireDelayFallen;
			return !flag && this.CheckFallenImmediate(context);
		}

		// Token: 0x0600631B RID: 25371 RVA: 0x003844E8 File Offset: 0x003826E8
		public bool CheckFallenImmediate(DataContext context)
		{
			for (int i = 0; i < this._selfTeam.Length; i++)
			{
				int charId = this._selfTeam[i];
				bool flag = charId >= 0 && this._needCheckFallenCharSet.Contains(charId);
				if (flag)
				{
					bool flag2 = this.CheckCurrCharDangerOrFallen(context, this._combatCharacterDict[charId]);
					if (flag2)
					{
						break;
					}
				}
			}
			bool flag3 = this.IsInCombat() && !this.CombatAboutToOver();
			if (flag3)
			{
				for (int j = 0; j < this._enemyTeam.Length; j++)
				{
					int charId2 = this._enemyTeam[j];
					bool flag4 = charId2 >= 0 && this._needCheckFallenCharSet.Contains(charId2);
					if (flag4)
					{
						bool flag5 = this.CheckCurrCharDangerOrFallen(context, this._combatCharacterDict[charId2]);
						if (flag5)
						{
							break;
						}
					}
				}
			}
			this._needCheckFallenCharSet.Clear();
			return !this.IsInCombat();
		}

		// Token: 0x0600631C RID: 25372 RVA: 0x003845E4 File Offset: 0x003827E4
		public void EnsurePauseState()
		{
			bool prevPause = this.Pause;
			this.Pause = (this._selfChar.StateMachine.GetCurrentState().IsUpdateOnPause || this._enemyChar.StateMachine.GetCurrentState().IsUpdateOnPause);
			bool flag = prevPause == this.Pause;
			if (!flag)
			{
				this.UpdateAllTeammateCommandUsable(this.Context, true, -1);
				this.UpdateAllTeammateCommandUsable(this.Context, false, -1);
			}
		}

		// Token: 0x0600631D RID: 25373 RVA: 0x0038465C File Offset: 0x0038285C
		[DomainMethod]
		[Obsolete("This method is obsolete, and will be removed in future.")]
		public void RemoveTeammateCommand(DataContext context, int charId, int index)
		{
		}

		// Token: 0x0600631E RID: 25374 RVA: 0x0038465F File Offset: 0x0038285F
		[Obsolete("This method is obsolete, use AddCombatResultLegacy or ClearCombatResultLegacies instead.")]
		public void SetCombatResultLegacy(short legacy)
		{
		}

		// Token: 0x0600631F RID: 25375 RVA: 0x00384662 File Offset: 0x00382862
		[Obsolete("This method is obsolete, and will be removed in future.")]
		public void ExecuteTeammateCommandImplement(DataContext context, bool isAlly, ETeammateCommandImplement implement, int charId)
		{
		}

		// Token: 0x06006320 RID: 25376 RVA: 0x00384665 File Offset: 0x00382865
		[Obsolete("This method is obsolete, and will be removed in future.")]
		public void GetUsableTeammateCommands(List<ValueTuple<int, int>> teammateId2CmdIndexes, CombatCharacter combatChar, ETeammateCommandImplement targetImplement)
		{
		}

		// Token: 0x06006321 RID: 25377 RVA: 0x00384668 File Offset: 0x00382868
		[Obsolete("This method is obsolete, and will be removed in future.")]
		public void ChangeNpcWeaponInnerRatio(DataContext context, int charId, int weaponIndex, sbyte innerRatio)
		{
		}

		// Token: 0x06006322 RID: 25378 RVA: 0x0038466B File Offset: 0x0038286B
		[Obsolete("This method is obsolete, and will be removed in future. Use character.GetMaxTrickCount instead.")]
		public int GetMaxTrickCount(CombatCharacter character)
		{
			return character.GetMaxTrickCount();
		}

		// Token: 0x06006323 RID: 25379 RVA: 0x00384673 File Offset: 0x00382873
		[Obsolete("This method is obsolete, and will be removed in future. Use character.IsTrickUsable instead.")]
		public bool IsTrickUsable(CombatCharacter character, sbyte trickType)
		{
			return character.IsTrickUsable(trickType);
		}

		// Token: 0x06006324 RID: 25380 RVA: 0x0038467C File Offset: 0x0038287C
		[Obsolete("This method is obsolete, and will be removed in future. Use character.AnyUsableTrick instead.")]
		public bool HasAnyUsableTrick(CombatCharacter character)
		{
			return character.AnyUsableTrick;
		}

		// Token: 0x06006325 RID: 25381 RVA: 0x00384684 File Offset: 0x00382884
		[Obsolete("This method is obsolete, and will be removed in future. Use character.UsableTrickCount instead.")]
		public int GetUsableTrickCount(CombatCharacter character)
		{
			return character.UsableTrickCount;
		}

		// Token: 0x06006326 RID: 25382 RVA: 0x0038468C File Offset: 0x0038288C
		[Obsolete("This method is obsolete, and will be removed in future. Use character.ReplaceUsableTrick instead.")]
		public int ReplaceUsableTrick(DataContext context, CombatCharacter character, sbyte trickType, int count = -1)
		{
			return character.ReplaceUsableTrick(context, trickType, count);
		}

		// Token: 0x06006327 RID: 25383 RVA: 0x00384698 File Offset: 0x00382898
		[Obsolete("This method is obsolete, and will be removed in future. Use character.UselessTrickCount instead.")]
		public int GetUselessTrickCount(CombatCharacter character)
		{
			return character.UselessTrickCount;
		}

		// Token: 0x06006328 RID: 25384 RVA: 0x003846A0 File Offset: 0x003828A0
		[Obsolete("This method is obsolete, and will be removed in future. Use character.TrickEquals instead.")]
		public bool TrickEquals(CombatCharacter character, sbyte trick1, sbyte trick2)
		{
			return character.TrickEquals(trick1, trick2);
		}

		// Token: 0x06006329 RID: 25385 RVA: 0x003846AA File Offset: 0x003828AA
		[Obsolete("This method is obsolete, and will be removed in future. Use character.GetContinueTricks instead.")]
		public void GetContinueTricks(CombatCharacter character, sbyte trickType, List<int> indexList = null)
		{
			character.GetContinueTricks(trickType, indexList);
		}

		// Token: 0x0600632A RID: 25386 RVA: 0x003846B5 File Offset: 0x003828B5
		[Obsolete("This method is obsolete, and will be removed in future. Use character.CalcNormalAttackStartupFrames instead.")]
		public int GetAttackPrepareFrame(CombatCharacter character)
		{
			return character.CalcNormalAttackStartupFrames();
		}

		// Token: 0x0600632B RID: 25387 RVA: 0x003846BD File Offset: 0x003828BD
		[Obsolete("This method is obsolete, and will be removed in future. Use character.CalcNormalAttackStartupFrames instead.")]
		public int GetAttackPrepareFrame(CombatCharacter character, GameData.Domains.Item.Weapon weapon)
		{
			return character.CalcNormalAttackStartupFrames(weapon);
		}

		// Token: 0x0600632C RID: 25388 RVA: 0x003846C6 File Offset: 0x003828C6
		[Obsolete("This method is obsolete, and will be removed in future. Use character.CalcNormalAttackAnimationFrames instead.")]
		public short GetAttackAnimationFrame(CombatCharacter character, float animDuration)
		{
			return character.CalcNormalAttackAnimationFrames(animDuration);
		}

		// Token: 0x170003CF RID: 975
		// (get) Token: 0x0600632D RID: 25389 RVA: 0x003846CF File Offset: 0x003828CF
		public bool IsAiMoving
		{
			get
			{
				return this._autoCombat && this.AiOptions.AutoMove;
			}
		}

		// Token: 0x0600632E RID: 25390 RVA: 0x003846E8 File Offset: 0x003828E8
		[DomainMethod]
		public TestAiData GetAiTestData(bool isAlly)
		{
			return new TestAiData();
		}

		// Token: 0x0600632F RID: 25391 RVA: 0x003846FF File Offset: 0x003828FF
		[DomainMethod]
		public void SetAiOptions(AiOptions aiOptions)
		{
			this.AiOptions = aiOptions;
		}

		// Token: 0x06006330 RID: 25392 RVA: 0x0038470C File Offset: 0x0038290C
		public bool CanRecoverBreath(CombatCharacter character)
		{
			bool canRecover = true;
			return DomainManager.SpecialEffect.ModifyData(character.GetId(), -1, 189, canRecover, -1, -1, -1);
		}

		// Token: 0x06006331 RID: 25393 RVA: 0x0038473C File Offset: 0x0038293C
		public void RecoverBreathValue(DataContext context, CombatCharacter character)
		{
			bool flag = !this.CanRecoverBreath(character);
			if (!flag)
			{
				int addValue = CFormula.CalcBreathRecoverValue((int)character.GetCharacter().GetRecoveryOfStanceAndBreath().Inner);
				addValue = character.CalcBreathRecoverValue(addValue);
				this.ChangeBreathValue(context, character, addValue, false, null);
			}
		}

		// Token: 0x06006332 RID: 25394 RVA: 0x0038478C File Offset: 0x0038298C
		public int ChangeBreathValue(DataContext context, CombatCharacter character, int addValue, bool changedByEffect = false, CombatCharacter changer = null)
		{
			bool flag = changedByEffect && addValue < 0;
			if (flag)
			{
				addValue = DomainManager.SpecialEffect.ModifyValue(character.GetId(), 256, addValue, (changer != null) ? changer.GetId() : -1, -1, -1, 0, 0, 0, 0);
			}
			int prevValue = character.GetBreathValue();
			int currValue = Math.Clamp(prevValue + addValue, 0, character.GetMaxBreathValue());
			addValue = currValue - prevValue;
			bool flag2 = addValue < 0;
			if (flag2)
			{
				bool flag3 = character.PoisonOverflow(2);
				if (flag3)
				{
					character.AddPoisonAffectValue(2, (short)(-addValue * 100 / 30000), false);
				}
			}
			bool lockMaxBreath = character.LockMaxBreath;
			if (lockMaxBreath)
			{
				currValue = 30000;
			}
			character.SetBreathValue(currValue, context);
			return addValue;
		}

		// Token: 0x06006333 RID: 25395 RVA: 0x00384844 File Offset: 0x00382A44
		public bool CanRecoverStance(CombatCharacter character)
		{
			bool canRecover = true;
			return DomainManager.SpecialEffect.ModifyData(character.GetId(), -1, 190, canRecover, -1, -1, -1);
		}

		// Token: 0x06006334 RID: 25396 RVA: 0x00384874 File Offset: 0x00382A74
		public void RecoverStanceValue(DataContext context, CombatCharacter character, int addValue, sbyte attackPreparePointCost, bool isPursue)
		{
			bool flag = !this.CanRecoverStance(character);
			if (!flag)
			{
				addValue = CFormula.CalcStanceRecoverValue((int)character.GetCharacter().GetRecoveryOfStanceAndBreath().Outer, addValue, attackPreparePointCost, isPursue);
				addValue = character.CalcStanceRecoverValue(addValue);
				this.ChangeStanceValue(context, character, addValue, false, null);
				this.UpdateSkillCostBreathStanceCanUse(context, character);
			}
		}

		// Token: 0x06006335 RID: 25397 RVA: 0x003848D4 File Offset: 0x00382AD4
		public int ChangeStanceValue(DataContext context, CombatCharacter character, int addValue, bool changedByEffect = false, CombatCharacter changer = null)
		{
			bool flag = changedByEffect && addValue < 0;
			if (flag)
			{
				addValue = DomainManager.SpecialEffect.ModifyValue(character.GetId(), 255, addValue, (changer != null) ? changer.GetId() : -1, -1, -1, 0, 0, 0, 0);
			}
			int prevValue = character.GetStanceValue();
			int currValue = Math.Clamp(prevValue + addValue, 0, character.GetMaxStanceValue());
			addValue = currValue - prevValue;
			bool flag2 = addValue < 0;
			if (flag2)
			{
				bool flag3 = character.PoisonOverflow(3);
				if (flag3)
				{
					character.AddPoisonAffectValue(3, (short)(-addValue * 100 / 4000), false);
				}
			}
			bool lockMaxStance = character.LockMaxStance;
			if (lockMaxStance)
			{
				currValue = 4000;
			}
			character.SetStanceValue(currValue, context);
			return addValue;
		}

		// Token: 0x06006336 RID: 25398 RVA: 0x0038498C File Offset: 0x00382B8C
		public void CostBreathAndStance(DataContext context, CombatCharacter character, int costBreath, int costStance, short skillId = -1)
		{
			bool flag = costBreath > 0;
			if (flag)
			{
				costBreath = this.ChangeBreathValue(context, character, -costBreath, false, null);
			}
			bool flag2 = costStance > 0;
			if (flag2)
			{
				costStance = this.ChangeStanceValue(context, character, -costStance, false, null);
			}
			Events.RaiseCostBreathAndStance(context, character.GetId(), character.IsAlly, -costBreath, -costStance, skillId);
		}

		// Token: 0x06006337 RID: 25399 RVA: 0x003849E4 File Offset: 0x00382BE4
		public void AddCombatState(DataContext context, CombatCharacter character, sbyte stateType, short stateId)
		{
			this.AddCombatState(context, character, stateType, stateId, 100, false, true, -1);
		}

		// Token: 0x06006338 RID: 25400 RVA: 0x00384A04 File Offset: 0x00382C04
		public void AddCombatState(DataContext context, CombatCharacter character, sbyte stateType, short stateId, int power)
		{
			this.AddCombatState(context, character, stateType, stateId, power, false, true, -1);
		}

		// Token: 0x06006339 RID: 25401 RVA: 0x00384A24 File Offset: 0x00382C24
		public void AddCombatState(DataContext context, CombatCharacter character, sbyte stateType, short stateId, int power, bool reverse)
		{
			this.AddCombatState(context, character, stateType, stateId, power, reverse, true, -1);
		}

		// Token: 0x0600633A RID: 25402 RVA: 0x00384A44 File Offset: 0x00382C44
		public void AddCombatState(DataContext context, CombatCharacter character, sbyte stateType, short stateId, int power, bool reverse, bool applyEffect)
		{
			this.AddCombatState(context, character, stateType, stateId, power, reverse, applyEffect, -1);
		}

		// Token: 0x0600633B RID: 25403 RVA: 0x00384A64 File Offset: 0x00382C64
		public void AddCombatState(DataContext context, CombatCharacter character, sbyte stateType, short stateId, int power, bool reverse, bool applyEffect, int srcCharId)
		{
			CombatStateCollection stateCollection = character.GetCombatStateCollection(stateType);
			short maxPower = character.GetCombatStatePowerLimit(stateType);
			if (applyEffect)
			{
				stateId = (short)DomainManager.SpecialEffect.ModifyData(character.GetId(), -1, 166, (int)stateId, (int)stateType, power, reverse ? 1 : 0);
				bool flag = stateId < 0;
				if (flag)
				{
					return;
				}
				SpecialEffectDomain specialEffect = DomainManager.SpecialEffect;
				int id = character.GetId();
				ushort fieldId = 167;
				int value = power;
				int customParam = (int)stateId;
				BoolArray8 array = default(BoolArray8);
				array[0] = (power <= 0);
				array[1] = !stateCollection.StateDict.ContainsKey(stateId);
				power = specialEffect.ModifyValue(id, fieldId, value, (int)stateType, customParam, (int)array, 0, 0, 0, 0);
				bool flag2 = power == 0;
				if (flag2)
				{
					return;
				}
			}
			bool flag3 = stateCollection.StateDict.ContainsKey(stateId);
			if (flag3)
			{
				short currentPower = stateCollection.StateDict[stateId].Item1;
				bool flag4 = (int)currentPower + power != 0;
				if (flag4)
				{
					ValueTuple<short, bool, int> state = stateCollection.StateDict[stateId];
					state.Item1 = (short)Math.Min((int)currentPower + power, (int)maxPower);
					stateCollection.StateDict[stateId] = state;
					((CombatStateEffect)DomainManager.SpecialEffect.Get(stateCollection.State2EffectId[stateId])).ChangePower(context, stateCollection.StateDict[stateId].Item1);
				}
				else
				{
					this.RemoveCombatState(context, character, stateType, stateId);
				}
			}
			else
			{
				bool flag5 = power > 0;
				if (flag5)
				{
					stateCollection.StateDict.Add(stateId, new ValueTuple<short, bool, int>((short)Math.Min(power, (int)maxPower), reverse, srcCharId));
					ValueTuple<short, bool, int> state2 = stateCollection.StateDict[stateId];
					long effectId = DomainManager.SpecialEffect.AddCombatStateEffect(context, character.GetId(), stateType, stateId, state2.Item1, state2.Item2);
					stateCollection.State2EffectId[stateId] = effectId;
				}
			}
			character.SetCombatStateCollection(stateType, stateCollection, context);
			character.GetDefeatMarkCollection().SyncCombatStateMark(context, character);
		}

		// Token: 0x0600633C RID: 25404 RVA: 0x00384C68 File Offset: 0x00382E68
		public void RemoveCombatState(DataContext context, CombatCharacter character, sbyte stateType, short stateId)
		{
			CombatStateCollection stateCollection = character.GetCombatStateCollection(stateType);
			bool flag = stateCollection.StateDict.ContainsKey(stateId);
			if (flag)
			{
				stateCollection.StateDict.Remove(stateId);
				character.SetCombatStateCollection(stateType, stateCollection, context);
				character.GetDefeatMarkCollection().SyncCombatStateMark(context, character);
				DomainManager.SpecialEffect.Remove(context, stateCollection.State2EffectId[stateId]);
				stateCollection.State2EffectId.Remove(stateId);
			}
		}

		// Token: 0x0600633D RID: 25405 RVA: 0x00384CE0 File Offset: 0x00382EE0
		public void ClearCombatState(DataContext context, CombatCharacter character)
		{
			List<short> stateIdList = ObjectPool<List<short>>.Instance.Get();
			for (sbyte stateType = 0; stateType < 3; stateType += 1)
			{
				stateIdList.Clear();
				stateIdList.AddRange(character.GetCombatStateCollection(stateType).StateDict.Keys);
				for (int i = 0; i < stateIdList.Count; i++)
				{
					this.RemoveCombatState(context, character, stateType, stateIdList[i]);
				}
			}
			ObjectPool<List<short>>.Instance.Return(stateIdList);
		}

		// Token: 0x0600633E RID: 25406 RVA: 0x00384D60 File Offset: 0x00382F60
		[return: TupleElementNames(new string[]
		{
			"stateId",
			"reverse"
		})]
		public static ValueTuple<short, bool> CalcReversedCombatState(short stateId, bool reverse)
		{
			CombatStateItem stateConfig = CombatState.Instance[stateId];
			bool flag = stateConfig.ReverseState < 0;
			if (flag)
			{
				reverse = !reverse;
			}
			else
			{
				stateId = stateConfig.ReverseState;
			}
			return new ValueTuple<short, bool>(stateId, reverse);
		}

		// Token: 0x0600633F RID: 25407 RVA: 0x00384DA4 File Offset: 0x00382FA4
		public void ReverseCombatState(DataContext context, CombatCharacter character, sbyte stateType, short stateId)
		{
			bool flag = stateType - 1 <= 1;
			bool flag2 = !flag;
			if (flag2)
			{
				short predefinedLogId = 8;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(30, 2);
				defaultInterpolatedStringHandler.AppendLiteral("cannot reverse special state ");
				defaultInterpolatedStringHandler.AppendFormatted<sbyte>(stateType);
				defaultInterpolatedStringHandler.AppendLiteral(" ");
				defaultInterpolatedStringHandler.AppendFormatted<short>(stateId);
				PredefinedLog.Show(predefinedLogId, defaultInterpolatedStringHandler.ToStringAndClear());
			}
			else
			{
				CombatStateCollection stateCollection = character.GetCombatStateCollection(stateType);
				bool flag3 = !stateCollection.StateDict.ContainsKey(stateId);
				if (flag3)
				{
					short predefinedLogId2 = 8;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(38, 2);
					defaultInterpolatedStringHandler.AppendLiteral("cannot reverse state of not existing ");
					defaultInterpolatedStringHandler.AppendFormatted<sbyte>(stateType);
					defaultInterpolatedStringHandler.AppendLiteral(" ");
					defaultInterpolatedStringHandler.AppendFormatted<short>(stateId);
					PredefinedLog.Show(predefinedLogId2, defaultInterpolatedStringHandler.ToStringAndClear());
				}
				else
				{
					ValueTuple<short, bool, int> stateInfo = stateCollection.StateDict[stateId];
					sbyte reverseType = (stateType == 1) ? 2 : 1;
					bool reverse = stateInfo.Item2;
					this.RemoveCombatState(context, character, stateType, stateId);
					ValueTuple<short, bool> valueTuple = CombatDomain.CalcReversedCombatState(stateId, reverse);
					stateId = valueTuple.Item1;
					reverse = valueTuple.Item2;
					this.AddCombatState(context, character, reverseType, stateId, (int)stateInfo.Item1, reverse, false);
				}
			}
		}

		// Token: 0x06006340 RID: 25408 RVA: 0x00384ED4 File Offset: 0x003830D4
		public void InvalidateCombatStateCache(DataContext context, CombatCharacter character, sbyte stateType)
		{
			CombatStateCollection stateCollection = character.GetCombatStateCollection(stateType);
			foreach (long effectId in stateCollection.State2EffectId.Values)
			{
				((CombatStateEffect)DomainManager.SpecialEffect.Get(effectId)).InvalidateCache(context);
			}
		}

		// Token: 0x06006341 RID: 25409 RVA: 0x00384F48 File Offset: 0x00383148
		[DomainMethod]
		public void ClearAllReserveAction(DataContext context, bool isAlly = true)
		{
			bool flag = !this.IsInCombat();
			if (!flag)
			{
				CombatCharacter combatChar = isAlly ? this._selfChar : this._enemyChar;
				combatChar.SetCombatReserveData(CombatReserveData.Invalid, context);
			}
		}

		// Token: 0x06006342 RID: 25410 RVA: 0x00384F84 File Offset: 0x00383184
		[DomainMethod]
		public void ClearReserveNormalAttack(DataContext context, bool isAlly = true)
		{
			bool flag = !this.IsInCombat();
			if (!flag)
			{
				CombatCharacter combatChar = isAlly ? this._selfChar : this._enemyChar;
				combatChar.SetReserveNormalAttack(false, context);
			}
		}

		// Token: 0x06006343 RID: 25411 RVA: 0x00384FBC File Offset: 0x003831BC
		[DomainMethod]
		public bool SetPuppetUnyieldingFallen(DataContext context, bool unyieldingFallen)
		{
			bool flag = !this.IsInCombat() || !this.GetIsPlaygroundCombat();
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				DomainManager.Extra.SetEnemyUnyieldingFallen(unyieldingFallen, context);
				result = true;
			}
			return result;
		}

		// Token: 0x06006344 RID: 25412 RVA: 0x00384FF8 File Offset: 0x003831F8
		[DomainMethod]
		public bool SetPuppetDisableAi(DataContext context, bool disableAi)
		{
			bool flag = !this.IsInCombat() || !this.GetIsPlaygroundCombat();
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				DomainManager.Extra.SetDisableEnemyAi(disableAi, context);
				if (disableAi)
				{
					this.SetMoveState(0, false, false);
				}
				result = true;
			}
			return result;
		}

		// Token: 0x06006345 RID: 25413 RVA: 0x00385044 File Offset: 0x00383244
		private bool IsGuardChar(CombatCharacter character)
		{
			bool flag = character.GetCharacter().GetCreatingType() != 2;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				for (int i = 0; i < this._enemyTeam.Length; i++)
				{
					int charId = this._enemyTeam[i];
					bool flag2 = charId < 0;
					if (!flag2)
					{
						bool flag3 = DomainManager.Character.GetElement_Objects(charId).GetCreatingType() == 1;
						if (flag3)
						{
							return true;
						}
					}
				}
				result = false;
			}
			return result;
		}

		// Token: 0x06006346 RID: 25414 RVA: 0x003850C0 File Offset: 0x003832C0
		public bool CanAcceptCommand()
		{
			return this.IsInCombat() && !this.CombatAboutToOver() && this._selfChar.ChangeCharId < 0 && this._enemyChar.ChangeCharId < 0;
		}

		// Token: 0x06006347 RID: 25415 RVA: 0x00385101 File Offset: 0x00383301
		public void CombatEntry(DataContext context, List<int> enemyIds, short combatConfigTemplateId)
		{
			GameDataBridge.AddDisplayEvent<List<int>, short>(DisplayEventType.StartCombat, enemyIds, combatConfigTemplateId);
			Events.RaiseCombatEntry(context, enemyIds, combatConfigTemplateId);
		}

		// Token: 0x06006348 RID: 25416 RVA: 0x00385118 File Offset: 0x00383318
		[return: TupleElementNames(new string[]
		{
			"anim",
			"anyChanged"
		})]
		public ValueTuple<string, bool> SetProperLoopAniAndParticle(DataContext context, CombatCharacter character, bool getMoveAni = false)
		{
			bool anyChanged = false;
			string properLoopAnim = this.GetProperLoopAni(character, getMoveAni);
			bool flag = properLoopAnim != character.GetAnimationToLoop();
			if (flag)
			{
				character.SetAnimationToLoop(properLoopAnim, context);
				anyChanged = true;
			}
			string properLoopParticle = this.GetProperLoopParticle(character, getMoveAni);
			bool flag2 = properLoopParticle != character.GetParticleToLoop();
			if (flag2)
			{
				character.SetParticleToLoop(properLoopParticle, context);
			}
			return new ValueTuple<string, bool>(properLoopAnim, anyChanged);
		}

		// Token: 0x06006349 RID: 25417 RVA: 0x00385180 File Offset: 0x00383380
		public string GetProperLoopAni(CombatCharacter character, bool getMoveAni = false)
		{
			bool flag = this.IsCharacterFallen(character);
			string result;
			if (flag)
			{
				result = null;
			}
			else
			{
				bool flag2 = character.SpecialAnimationLoop != null;
				if (flag2)
				{
					result = character.SpecialAnimationLoop;
				}
				else
				{
					bool flag3 = character.GetAttackingTrickType() >= 0;
					if (flag3)
					{
						result = null;
					}
					else
					{
						bool flag4 = !this.Pause && character.IsMoving && (character.KeepMoving || getMoveAni);
						if (flag4)
						{
							result = (character.MoveForward ? this.GetWalkForwardAni(character) : this.GetWalkBackwardAni(character));
						}
						else
						{
							bool flag5 = character.GetPreparingSkillId() >= 0;
							if (flag5)
							{
								CombatSkillItem configData = Config.CombatSkill.Instance[character.GetPreparingSkillId()];
								bool isBoss = character.BossConfig != null;
								string musicWeaponFix = (configData.Type != 13) ? "" : this.GetMusicWeaponNameFix(character.GetWeaponData(-1));
								string prepareAni = (string.IsNullOrEmpty(configData.PlayerCastBossSkillPrepareAni) || isBoss) ? (configData.PrepareAnimation + musicWeaponFix + "_1") : (configData.PlayerCastBossSkillPrepareAni + musicWeaponFix + "_1");
								result = ((configData.EquipType == 1 && !isBoss) ? prepareAni : "C_007");
							}
							else
							{
								bool flag6 = character.GetPreparingOtherAction() >= 0;
								if (flag6)
								{
									result = this.GetPrepareOtherActionAnim(character);
								}
								else
								{
									bool flag7 = character.GetAffectingDefendSkillId() >= 0;
									if (flag7)
									{
										result = Config.CombatSkill.Instance[character.GetAffectingDefendSkillId()].DefendAnimation;
									}
									else
									{
										result = this.GetIdleAni(character);
									}
								}
							}
						}
					}
				}
			}
			return result;
		}

		// Token: 0x0600634A RID: 25418 RVA: 0x0038530C File Offset: 0x0038350C
		private string GetProperLoopParticle(CombatCharacter character, bool getMoveAni)
		{
			bool flag = this.IsCharacterFallen(character) || character.GetAttackingTrickType() >= 0 || character.GetPreparingOtherAction() < 0;
			string result;
			if (flag)
			{
				result = null;
			}
			else
			{
				OtherActionTypeItem otherActionConfig = OtherActionType.Instance[character.GetPreparingOtherAction()];
				bool flag2 = this.Pause || !character.IsMoving || (!character.KeepMoving && !getMoveAni);
				if (flag2)
				{
					result = otherActionConfig.PrepareParticle;
				}
				else
				{
					bool isNormal = this._currentDistance <= GlobalConfig.Instance.FastWalkDistance;
					bool flag3 = isNormal;
					if (flag3)
					{
						result = (character.MoveForward ? otherActionConfig.ForwardParticle : otherActionConfig.BackwardParticle);
					}
					else
					{
						result = (character.MoveForward ? otherActionConfig.ForwardFastParticle : otherActionConfig.BackwardFastParticle);
					}
				}
			}
			return result;
		}

		// Token: 0x0600634B RID: 25419 RVA: 0x003853D8 File Offset: 0x003835D8
		public string GetPrepareOtherActionAnim(CombatCharacter character)
		{
			bool flag = !character.IsActorSkeleton;
			string result;
			if (flag)
			{
				result = "C_007";
			}
			else
			{
				string prepareAnim = character.PreparingOtherActionTypeConfig.PrepareAnim;
				result = (string.IsNullOrEmpty(prepareAnim) ? "C_007" : prepareAnim);
			}
			return result;
		}

		// Token: 0x0600634C RID: 25420 RVA: 0x0038541C File Offset: 0x0038361C
		public string GetIdleAni(CombatCharacter character)
		{
			int usingWeaponIndex = character.GetUsingWeaponIndex();
			bool flag = usingWeaponIndex < 0;
			string result;
			if (flag)
			{
				result = "C_000";
			}
			else
			{
				short weaponTemplateId = character.GetWeapons()[usingWeaponIndex].TemplateId;
				bool flag2 = weaponTemplateId < 0;
				if (flag2)
				{
					result = "C_000";
				}
				else
				{
					string idleAni = Config.Weapon.Instance[weaponTemplateId].IdleAni;
					result = ((!string.IsNullOrEmpty(idleAni)) ? idleAni : ((character.GetMobilityLevel() == 0) ? "C_000" : "C_000"));
				}
			}
			return result;
		}

		// Token: 0x0600634D RID: 25421 RVA: 0x003854A0 File Offset: 0x003836A0
		private string GetWalkForwardAni(CombatCharacter character)
		{
			bool isNormal = this._currentDistance <= GlobalConfig.Instance.FastWalkDistance;
			OtherActionTypeItem otherActionConfig = character.PreparingOtherActionTypeConfig;
			bool flag = character.IsActorSkeleton && otherActionConfig != null && !string.IsNullOrEmpty(isNormal ? otherActionConfig.ForwardAnim : otherActionConfig.ForwardFastAnim);
			string result;
			if (flag)
			{
				result = (isNormal ? otherActionConfig.ForwardAnim : otherActionConfig.ForwardFastAnim);
			}
			else
			{
				string commonForwardAni = isNormal ? "M_001" : "MR_001";
				int usingWeaponIndex = character.GetUsingWeaponIndex();
				bool flag2 = usingWeaponIndex < 0;
				if (flag2)
				{
					result = commonForwardAni;
				}
				else
				{
					short weaponTemplateId = character.GetWeapons()[usingWeaponIndex].TemplateId;
					bool flag3 = weaponTemplateId < 0;
					if (flag3)
					{
						result = commonForwardAni;
					}
					else
					{
						WeaponItem weaponConfig = Config.Weapon.Instance[weaponTemplateId];
						string forwardAni = (this._currentDistance <= GlobalConfig.Instance.FastWalkDistance) ? weaponConfig.ForwardAni : weaponConfig.FastForwardAni;
						result = ((!string.IsNullOrEmpty(forwardAni)) ? forwardAni : commonForwardAni);
					}
				}
			}
			return result;
		}

		// Token: 0x0600634E RID: 25422 RVA: 0x003855A0 File Offset: 0x003837A0
		private string GetWalkBackwardAni(CombatCharacter character)
		{
			bool isNormal = this._currentDistance <= GlobalConfig.Instance.FastWalkDistance;
			OtherActionTypeItem otherActionConfig = character.PreparingOtherActionTypeConfig;
			bool flag = character.IsActorSkeleton && otherActionConfig != null && !string.IsNullOrEmpty(isNormal ? otherActionConfig.BackwardAnim : otherActionConfig.BackwardFastAnim);
			string result;
			if (flag)
			{
				result = (isNormal ? otherActionConfig.BackwardAnim : otherActionConfig.BackwardFastAnim);
			}
			else
			{
				string commonBackwardAni = isNormal ? "M_002" : "MR_002";
				int usingWeaponIndex = character.GetUsingWeaponIndex();
				bool flag2 = usingWeaponIndex < 0;
				if (flag2)
				{
					result = commonBackwardAni;
				}
				else
				{
					short weaponTemplateId = character.GetWeapons()[usingWeaponIndex].TemplateId;
					bool flag3 = weaponTemplateId < 0;
					if (flag3)
					{
						result = commonBackwardAni;
					}
					else
					{
						WeaponItem weaponConfig = Config.Weapon.Instance[weaponTemplateId];
						string backwardAni = (this._currentDistance <= GlobalConfig.Instance.FastWalkDistance) ? weaponConfig.BackwardAni : weaponConfig.FastBackwardAni;
						result = ((!string.IsNullOrEmpty(backwardAni)) ? backwardAni : commonBackwardAni);
					}
				}
			}
			return result;
		}

		// Token: 0x0600634F RID: 25423 RVA: 0x003856A0 File Offset: 0x003838A0
		public string GetAvoidAni(CombatCharacter character, sbyte hitType)
		{
			bool flag = character.BossConfig != null || character.AnimalConfig != null;
			string result;
			if (flag)
			{
				result = CombatDomain.AvoidAni[(int)hitType];
			}
			else
			{
				int usingWeaponIndex = character.GetUsingWeaponIndex();
				bool flag2 = usingWeaponIndex < 0;
				if (flag2)
				{
					result = CombatDomain.AvoidAni[(int)hitType];
				}
				else
				{
					short weaponTemplateId = character.GetWeapons()[usingWeaponIndex].TemplateId;
					bool flag3 = weaponTemplateId < 0;
					if (flag3)
					{
						result = CombatDomain.AvoidAni[(int)hitType];
					}
					else
					{
						string[] avoidAnis = Config.Weapon.Instance[weaponTemplateId].AvoidAnis;
						result = ((avoidAnis != null) ? avoidAnis[(int)hitType] : CombatDomain.AvoidAni[(int)hitType]);
					}
				}
			}
			return result;
		}

		// Token: 0x06006350 RID: 25424 RVA: 0x0038573C File Offset: 0x0038393C
		public string GetHittedAni(CombatCharacter character, int injuryLevel)
		{
			int usingWeaponIndex = character.GetUsingWeaponIndex();
			bool flag = usingWeaponIndex < 0;
			string result;
			if (flag)
			{
				result = CombatDomain.InjuryAni[injuryLevel];
			}
			else
			{
				short weaponTemplateId = character.GetWeapons()[usingWeaponIndex].TemplateId;
				bool flag2 = weaponTemplateId < 0;
				if (flag2)
				{
					result = CombatDomain.InjuryAni[injuryLevel];
				}
				else
				{
					string[] hittedAnis = Config.Weapon.Instance[weaponTemplateId].HittedAnis;
					result = ((hittedAnis != null) ? hittedAnis[injuryLevel] : CombatDomain.InjuryAni[injuryLevel]);
				}
			}
			return result;
		}

		// Token: 0x06006351 RID: 25425 RVA: 0x003857B4 File Offset: 0x003839B4
		public bool IsPlayingMoveAni(CombatCharacter character)
		{
			string aniName = character.GetAnimationToLoop();
			return aniName == this.GetWalkForwardAni(character) || aniName == this.GetWalkBackwardAni(character) || aniName == "M_003" || aniName == "M_004" || aniName == "M_014" || aniName == "M_015";
		}

		// Token: 0x06006352 RID: 25426 RVA: 0x00385820 File Offset: 0x00383A20
		public void ChangeChangeTrickProgress(DataContext context, CombatCharacter character, int changeValue)
		{
			bool flag = (int)character.GetChangeTrickCount() >= character.MaxChangeTrickCount;
			if (!flag)
			{
				int percent = 100 + DomainManager.SpecialEffect.GetModifyValue(character.GetId(), 198, EDataModifyType.AddPercent, -1, -1, -1, EDataSumType.All);
				changeValue = Math.Max(changeValue * percent / 100, 0);
				int newValue = Math.Clamp((int)character.GetChangeTrickProgress() + changeValue, 0, GlobalConfig.Instance.MaxChangeTrickProgressOnce);
				bool flag2 = newValue >= GlobalConfig.Instance.MaxChangeTrickProgress;
				if (flag2)
				{
					int count = newValue / GlobalConfig.Instance.MaxChangeTrickProgress;
					this.ChangeChangeTrickCount(context, character, count, false);
					newValue -= GlobalConfig.Instance.MaxChangeTrickProgress * count;
				}
				bool flag3 = (int)character.GetChangeTrickCount() == character.MaxChangeTrickCount;
				if (flag3)
				{
					newValue = 0;
				}
				sbyte newProgress = (sbyte)Math.Clamp(newValue, 0, GlobalConfig.Instance.MaxChangeTrickProgress);
				character.SetChangeTrickProgress(newProgress, context);
			}
		}

		// Token: 0x06006353 RID: 25427 RVA: 0x00385904 File Offset: 0x00383B04
		public short GetMaxNeiliAllocation(CombatCharacter character, byte type)
		{
			return character.GetMaxNeiliAllocation(type);
		}

		// Token: 0x06006354 RID: 25428 RVA: 0x00385920 File Offset: 0x00383B20
		public sbyte GetAttackBodyPart(CombatCharacter attacker, CombatCharacter defender, IRandomSource random, short skillId = -1, sbyte trickType = -1, sbyte hitType = -1)
		{
			bool flag = hitType < 0 && trickType >= 0;
			if (flag)
			{
				hitType = this.GetAttackHitType(attacker, trickType);
			}
			sbyte trickHitType = (trickType >= 0 && trickType != 21) ? TrickType.Instance[trickType].AvoidType : hitType;
			bool flag2 = (skillId >= 0 && CombatSkillTemplateHelper.IsMindHitSkill(skillId)) || (trickType >= 0 && trickHitType == 3);
			sbyte result;
			if (flag2)
			{
				result = -1;
			}
			else
			{
				bool flag3 = trickType == 21;
				if (flag3)
				{
					trickType = CombatDomain.GodTrickUseTrickType[hitType];
				}
				DefeatMarkCollection defeatMarks = defender.GetDefeatMarkCollection();
				List<int> attackOdds = ObjectPool<List<int>>.Instance.Get();
				attackOdds.Clear();
				bool flag4 = skillId >= 0 && Config.CombatSkill.Instance[skillId].InjuryPartAtkRateDistribution != null;
				if (flag4)
				{
					attackOdds.AddRange(DomainManager.CombatSkill.GetElement_CombatSkills(new CombatSkillKey(attacker.GetId(), skillId)).GetBodyPartWeights());
				}
				else
				{
					bool flag5 = trickType >= 0 && TrickType.Instance[trickType].InjuryPartAtkRateDistribution != null;
					if (flag5)
					{
						attackOdds.AddRange(from x in TrickType.Instance[trickType].InjuryPartAtkRateDistribution
						select (int)x);
					}
					else
					{
						short predefinedLogId = 8;
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(19, 2);
						defaultInterpolatedStringHandler.AppendLiteral("GetAttackBodyPart ");
						defaultInterpolatedStringHandler.AppendFormatted<short>(skillId);
						defaultInterpolatedStringHandler.AppendLiteral(" ");
						defaultInterpolatedStringHandler.AppendFormatted<sbyte>(trickType);
						PredefinedLog.Show(predefinedLogId, defaultInterpolatedStringHandler.ToStringAndClear());
						for (int i = 0; i < 7; i++)
						{
							attackOdds.Add(0);
						}
					}
				}
				List<sbyte> noExistPartList = ObjectPool<List<sbyte>>.Instance.Get();
				noExistPartList.Clear();
				bool flag6 = !defender.GetCharacter().GetHaveLeftArm();
				if (flag6)
				{
					noExistPartList.Add(3);
				}
				bool flag7 = !defender.GetCharacter().GetHaveRightArm();
				if (flag7)
				{
					noExistPartList.Add(4);
				}
				bool flag8 = !defender.GetCharacter().GetHaveLeftLeg();
				if (flag8)
				{
					noExistPartList.Add(5);
				}
				bool flag9 = !defender.GetCharacter().GetHaveRightLeg();
				if (flag9)
				{
					noExistPartList.Add(6);
				}
				bool flag10 = noExistPartList.Count > 0;
				if (flag10)
				{
					for (int j = 0; j < noExistPartList.Count; j++)
					{
						attackOdds[(int)noExistPartList[j]] = 0;
					}
					bool flag11 = !attackOdds.Exists((int odds) => odds > 0);
					if (flag11)
					{
						for (sbyte part = 0; part < 7; part += 1)
						{
							bool flag12 = !noExistPartList.Contains(part);
							if (flag12)
							{
								attackOdds[(int)part] = 1;
							}
						}
					}
				}
				ObjectPool<List<sbyte>>.Instance.Return(noExistPartList);
				bool flag13 = skillId < 0;
				if (flag13)
				{
					for (sbyte part2 = 0; part2 < 7; part2 += 1)
					{
						byte outerInjury = defeatMarks.OuterInjuryMarkList[(int)part2];
						byte innerInjury = defeatMarks.InnerInjuryMarkList[(int)part2];
						bool flag14 = attackOdds[(int)part2] > 0 && (outerInjury >= 3 || innerInjury >= 3);
						if (flag14)
						{
							attackOdds[(int)part2] = (int)((sbyte)Math.Max(attackOdds[(int)part2] / 10, 1));
						}
					}
				}
				for (sbyte part3 = 0; part3 < 7; part3 += 1)
				{
					bool flag15 = attackOdds[(int)part3] > 0;
					if (flag15)
					{
						attackOdds[(int)part3] = DomainManager.SpecialEffect.ModifyValue(attacker.GetId(), 308, attackOdds[(int)part3], (int)part3, -1, -1, 0, 0, 0, 0);
					}
				}
				sbyte attackBodyPart = (sbyte)RandomUtils.GetRandomIndex(attackOdds, this.Context.Random);
				ObjectPool<List<int>>.Instance.Return(attackOdds);
				attackBodyPart = (sbyte)DomainManager.SpecialEffect.ModifyData(attacker.GetId(), skillId, 140, (int)attackBodyPart, -1, -1, -1);
				result = attackBodyPart;
			}
			return result;
		}

		// Token: 0x06006355 RID: 25429 RVA: 0x00385D2C File Offset: 0x00383F2C
		private int ApplyHitOddsSpecialEffect(CombatCharacter attacker, CombatCharacter defender, int hitOdds, sbyte hitType, short skillId = -1)
		{
			long hitOddsLong = (long)hitOdds;
			int percent = 100;
			percent += DomainManager.SpecialEffect.GetModifyValue(attacker.GetId(), skillId, 74, EDataModifyType.AddPercent, (int)hitType, -1, -1, EDataSumType.All);
			percent += DomainManager.SpecialEffect.GetModifyValue(defender.GetId(), skillId, 107, EDataModifyType.AddPercent, (int)hitType, -1, -1, EDataSumType.All);
			hitOddsLong = Math.Max(hitOddsLong * (long)percent / 100L, 0L);
			ValueTuple<int, int> attackerValue = DomainManager.SpecialEffect.GetTotalPercentModifyValue(attacker.GetId(), skillId, 74, (int)hitType, -1, -1);
			ValueTuple<int, int> defenderValue = DomainManager.SpecialEffect.GetTotalPercentModifyValue(defender.GetId(), skillId, 107, (int)hitType, -1, -1);
			bool isFightBack = attacker.GetIsFightBack();
			if (isFightBack)
			{
				ValueTuple<int, int> attackerFightBackValue = DomainManager.SpecialEffect.GetTotalPercentModifyValue(attacker.GetId(), skillId, 75, (int)hitType, -1, -1);
				attackerValue.Item1 = Math.Max(attackerValue.Item1, attackerFightBackValue.Item1);
				attackerValue.Item2 = Math.Min(attackerValue.Item2, attackerFightBackValue.Item2);
				ValueTuple<int, int> defenderFightBackValue = DomainManager.SpecialEffect.GetTotalPercentModifyValue(defender.GetId(), skillId, 108, (int)hitType, -1, -1);
				defenderValue.Item1 = Math.Max(defenderValue.Item1, defenderFightBackValue.Item1);
				defenderValue.Item2 = Math.Min(defenderValue.Item2, defenderFightBackValue.Item2);
			}
			percent = 100 + Math.Max(attackerValue.Item1, defenderValue.Item1) + Math.Min(attackerValue.Item2, defenderValue.Item2);
			hitOddsLong = Math.Max(hitOddsLong * (long)percent / 100L, 0L);
			hitOdds = (int)Math.Min(hitOddsLong, 2147483647L);
			hitOdds = DomainManager.SpecialEffect.ModifyData(attacker.GetId(), skillId, 74, hitOdds, -1, -1, -1);
			hitOdds = DomainManager.SpecialEffect.ModifyData(defender.GetId(), skillId, 107, hitOdds, -1, -1, -1);
			return hitOdds;
		}

		// Token: 0x06006356 RID: 25430 RVA: 0x00385EE8 File Offset: 0x003840E8
		public void TransferDisorderOfQi(DataContext context, CombatCharacter srcChar, CombatCharacter dstChar, int delta)
		{
			delta = Math.Min(delta, (int)(srcChar.GetCharacter().GetDisorderOfQi() - srcChar.GetOldDisorderOfQi()));
			bool flag = delta <= 0;
			if (!flag)
			{
				srcChar.GetCharacter().TransferDisorderOfQi(context, dstChar.GetCharacter(), delta);
			}
		}

		// Token: 0x06006357 RID: 25431 RVA: 0x00385F34 File Offset: 0x00384134
		public void ChangeDisorderOfQiRandomRecovery(DataContext context, CombatCharacter combatChar, int delta, bool changeToOld = false)
		{
			delta = (int)CFormula.RandomCalcDisorderOfQiDelta(context.Random, delta);
			bool flag = delta < 0 && combatChar.GetOldDisorderOfQi() >= combatChar.GetCharacter().GetDisorderOfQi();
			if (!flag)
			{
				bool flag2 = delta < 0;
				if (flag2)
				{
					delta = Math.Clamp(delta, (int)(combatChar.GetOldDisorderOfQi() - combatChar.GetCharacter().GetDisorderOfQi()), 0);
				}
				bool flag3 = delta > 0 && changeToOld;
				if (flag3)
				{
					combatChar.SetOldDisorderOfQi((short)Math.Clamp((int)combatChar.GetOldDisorderOfQi() + delta, (int)DisorderLevelOfQi.MinValue, (int)DisorderLevelOfQi.MaxValue), context);
				}
				GameData.Domains.Character.Character character = combatChar.GetCharacter();
				character.ChangeDisorderOfQi(context, delta);
			}
		}

		// Token: 0x06006358 RID: 25432 RVA: 0x00385FD4 File Offset: 0x003841D4
		public static int CalcWeaponAttack(CombatCharacter combatChar, GameData.Domains.Item.Weapon weapon, short skillId)
		{
			int equipAttack = (int)weapon.GetEquipmentAttack();
			bool flag = skillId >= 0 && DomainManager.CombatSkill.GetSkillType(combatChar.GetId(), skillId) == 5 && combatChar.LegSkillUseShoes();
			if (flag)
			{
				ItemKey shoesKey = combatChar.Armors[5];
				GameData.Domains.Item.Armor shoes = shoesKey.IsValid() ? DomainManager.Item.GetElement_Armors(shoesKey.Id) : null;
				equipAttack = CombatDomain.CalcArmorAttack(combatChar, shoes);
			}
			return equipAttack * DomainManager.SpecialEffect.GetModify(combatChar.GetId(), skillId, 141, -1, -1, -1, EDataSumType.All);
		}

		// Token: 0x06006359 RID: 25433 RVA: 0x0038606C File Offset: 0x0038426C
		public static int CalcWeaponDefend(CombatCharacter combatChar, GameData.Domains.Item.Weapon weapon, short skillId)
		{
			int equipDefense = (int)weapon.GetEquipmentDefense();
			bool flag = skillId >= 0 && DomainManager.CombatSkill.GetSkillType(combatChar.GetId(), skillId) == 5 && combatChar.LegSkillUseShoes();
			if (flag)
			{
				ItemKey shoesKey = combatChar.Armors[5];
				GameData.Domains.Item.Armor shoes = shoesKey.IsValid() ? DomainManager.Item.GetElement_Armors(shoesKey.Id) : null;
				equipDefense = CombatDomain.CalcArmorDefend(combatChar, shoes);
			}
			return equipDefense * DomainManager.SpecialEffect.GetModify(combatChar.GetId(), skillId, 142, weapon.GetId(), -1, -1, EDataSumType.All);
		}

		// Token: 0x0600635A RID: 25434 RVA: 0x00386108 File Offset: 0x00384308
		public static int CalcArmorAttack(CombatCharacter combatChar, GameData.Domains.Item.Armor armor)
		{
			int equipAttack = (int)((armor != null && armor.GetCurrDurability() > 0) ? armor.GetEquipmentAttack() : 100);
			return DomainManager.SpecialEffect.ModifyValue(combatChar.GetId(), 143, equipAttack, -1, -1, -1, 0, 0, 0, 0);
		}

		// Token: 0x0600635B RID: 25435 RVA: 0x00386150 File Offset: 0x00384350
		public static int CalcArmorDefend(CombatCharacter combatChar, GameData.Domains.Item.Armor armor)
		{
			int equipDefense = (int)((armor != null && armor.GetCurrDurability() > 0) ? armor.GetEquipmentDefense() : 50);
			return DomainManager.SpecialEffect.ModifyValue(combatChar.GetId(), 144, equipDefense, (armor != null) ? armor.GetId() : -1, -1, -1, 0, 0, 0, 0);
		}

		// Token: 0x0600635C RID: 25436 RVA: 0x003861A4 File Offset: 0x003843A4
		public static bool IsWeaponCanBreak(short weaponSubType)
		{
			bool flag = weaponSubType - 16 <= 1;
			return !flag;
		}

		// Token: 0x0600635D RID: 25437 RVA: 0x003861C4 File Offset: 0x003843C4
		private void CalculateWeaponArmorBreak(CombatContext context, sbyte breakOdds)
		{
			bool ignoreArmor = DomainManager.SpecialEffect.ModifyData(context.AttackerId, context.SkillTemplateId, 281, false, -1, -1, -1);
			bool flag = context.BodyPart < 0 || breakOdds == 0 || ignoreArmor;
			if (flag)
			{
				context.Attacker.NeedReduceWeaponDurability = false;
				context.Defender.NeedReduceArmorDurability = false;
			}
			else
			{
				context.CheckReduceWeaponDurability(breakOdds);
				context.CheckReduceArmorDurability(breakOdds);
			}
		}

		// Token: 0x0600635E RID: 25438 RVA: 0x0038623D File Offset: 0x0038443D
		public void ReduceDurabilityByHit(DataContext context, CombatCharacter character, ItemKey key)
		{
			this.ChangeDurability(context, character, key, -1, EChangeDurabilitySourceType.Hit);
		}

		// Token: 0x0600635F RID: 25439 RVA: 0x0038624B File Offset: 0x0038444B
		public void CostDurability(DataContext context, CombatCharacter character, ItemKey key, int cost)
		{
			this.ChangeDurability(context, character, key, -cost, EChangeDurabilitySourceType.Cost);
		}

		// Token: 0x06006360 RID: 25440 RVA: 0x0038625C File Offset: 0x0038445C
		public void ChangeDurability(DataContext context, CombatCharacter character, ItemKey key, int delta, EChangeDurabilitySourceType sourceType)
		{
			bool flag = !key.IsValid();
			if (!flag)
			{
				ItemBase item = DomainManager.Item.GetBaseItem(key);
				bool flag2 = item.GetMaxDurability() <= 0;
				if (!flag2)
				{
					character.ChangingDurabilityItems.Push(key);
					delta = DomainManager.SpecialEffect.ModifyValueCustom(character.GetId(), 309, delta, (int)key.ItemType, (int)sourceType, -1, 0, 0, 0, 0);
					character.ChangingDurabilityItems.Pop();
					short oldDurability = item.GetCurrDurability();
					short newDurability = (short)Math.Clamp((int)oldDurability + delta, 0, (int)item.GetMaxDurability());
					bool flag3 = newDurability == oldDurability;
					if (!flag3)
					{
						item.SetCurrDurability(newDurability, context);
						bool flag4 = delta > 0;
						if (flag4)
						{
							this.EnsureOldDurability(key);
						}
						Events.RaiseCombatChangeDurability(context, character, key, (int)(newDurability - oldDurability));
						bool flag5 = key.ItemType != 0;
						if (!flag5)
						{
							CombatWeaponData weapon = this.GetElement_WeaponDataDict(key.Id);
							weapon.SetDurability(newDurability, context);
							bool flag6 = newDurability != 0;
							if (!flag6)
							{
								weapon.SetCanChangeTo(false, context);
								int index = character.GetWeapons().IndexOf(key);
								bool flag7 = index >= 0 && index < 3;
								if (flag7)
								{
									character.ClearUnlockAttackValue(context, index);
								}
								bool flag8 = key != character.GetWeapons()[character.GetUsingWeaponIndex()] || character.GetRawCreateCollection().Contains(key);
								if (!flag8)
								{
									this.ChangeWeapon(context, character, 3, false, false);
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x06006361 RID: 25441 RVA: 0x003863DC File Offset: 0x003845DC
		private bool CanPlayHitAnimation(CombatCharacter character)
		{
			return !this.IsCharacterFallen(character) && character.GetAttackingTrickType() < 0 && character.GetAffectingDefendSkillId() < 0 && character.GetPreparingSkillId() < 0 && character.GetPreparingOtherAction() < 0 && !character.GetPreparingItem().IsValid() && character.GetAnimationToLoop() != "C_007" && this.IsCurrentCombatCharacter(character);
		}

		// Token: 0x06006362 RID: 25442 RVA: 0x00386448 File Offset: 0x00384648
		public void UpdateAllCommandAvailability(DataContext context, CombatCharacter character)
		{
			this.UpdateSkillCanUse(context, character);
			this.UpdateWeaponCanChange(context, character);
			this.UpdateOtherActionCanUse(context, character, -1);
			this.UpdateAllTeammateCommandUsable(context, character.IsAlly, -1);
			this.UpdateCanUseItem(context, character);
			this.UpdateCanChangeTrick(context, character);
			this.UpdateCanSurrender(context, character);
		}

		// Token: 0x06006363 RID: 25443 RVA: 0x0038649C File Offset: 0x0038469C
		public bool CheckHit(DataContext context, CombatCharacter character, sbyte hitType, int hitValuePercent = 100)
		{
			CombatCharacter enemyChar = this.GetCombatCharacter(!character.IsAlly, false);
			sbyte bodyPart = (sbyte)context.Random.Next(0, 7);
			int hitValue = character.GetHitValue(hitType, bodyPart, 0, -1) * hitValuePercent / 100;
			int avoidValue = enemyChar.GetAvoidValue(hitType, bodyPart, -1, false);
			int hitOdds = hitValue * 100 / avoidValue / ((hitValue < avoidValue) ? 2 : 1);
			hitOdds = this.ApplyHitOddsSpecialEffect(character, enemyChar, hitOdds, hitType, -1);
			return hitOdds < 0 || context.Random.CheckPercentProb(hitOdds);
		}

		// Token: 0x06006364 RID: 25444 RVA: 0x00386520 File Offset: 0x00384720
		public bool CheckHealthImmunity(DataContext context, CombatCharacter character)
		{
			bool flag = character.GetCharacter().GetFeatureIds().All((short x) => !CharacterFeature.Instance[x].IgnoreHealthMark);
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				this.ShowImmunityEffectTips(context, character.GetId(), EMarkType.Health);
				result = true;
			}
			return result;
		}

		// Token: 0x06006365 RID: 25445 RVA: 0x0038657C File Offset: 0x0038477C
		public string GetMusicWeaponNameFix(CombatWeaponData weaponData)
		{
			return (weaponData.Template.ItemSubType == 11) ? "_guqin" : ((weaponData.TemplateId == 884) ? "_sing" : "_flute");
		}

		// Token: 0x06006366 RID: 25446 RVA: 0x003865C0 File Offset: 0x003847C0
		public void ShowImmunityEffectTips(DataContext context, int charId, EMarkType markType)
		{
			if (!true)
			{
			}
			int num;
			switch (markType)
			{
			case EMarkType.Outer:
				num = 1700;
				goto IL_85;
			case EMarkType.Inner:
				num = 1699;
				goto IL_85;
			case EMarkType.Flaw:
				num = 1702;
				goto IL_85;
			case EMarkType.Acupoint:
				num = 1703;
				goto IL_85;
			case EMarkType.Mind:
				num = 1701;
				goto IL_85;
			case EMarkType.Fatal:
				num = 1704;
				goto IL_85;
			case EMarkType.Die:
				num = 1705;
				goto IL_85;
			case EMarkType.Health:
				num = 1708;
				goto IL_85;
			}
			num = -1;
			IL_85:
			if (!true)
			{
			}
			int effectId = num;
			bool flag = effectId < 0;
			if (!flag)
			{
				this.ShowSpecialEffectTips(charId, effectId, 0);
			}
		}

		// Token: 0x06006367 RID: 25447 RVA: 0x0038666C File Offset: 0x0038486C
		public void ShowWugKingEffectTips(DataContext context, int srcCharId, int dstCharId)
		{
			bool flag = srcCharId == dstCharId;
			int effectId;
			if (flag)
			{
				effectId = 1707;
			}
			else
			{
				bool flag2 = this._combatCharacterDict[srcCharId].IsAlly != this._combatCharacterDict[dstCharId].IsAlly;
				if (!flag2)
				{
					short predefinedLogId = 8;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(29, 2);
					defaultInterpolatedStringHandler.AppendLiteral("Unexpected wug king from ");
					defaultInterpolatedStringHandler.AppendFormatted<int>(srcCharId);
					defaultInterpolatedStringHandler.AppendLiteral(" to ");
					defaultInterpolatedStringHandler.AppendFormatted<int>(dstCharId);
					PredefinedLog.Show(predefinedLogId, defaultInterpolatedStringHandler.ToStringAndClear());
					return;
				}
				effectId = 1706;
			}
			this.ShowSpecialEffectTips(srcCharId, effectId, 0);
		}

		// Token: 0x06006368 RID: 25448 RVA: 0x00386710 File Offset: 0x00384910
		public void ShowSpecialEffectTips(int charId, int effectId, byte index = 0)
		{
			CombatCharacter character;
			bool flag = !this.IsInCombat() || !this._combatCharacterDict.TryGetValue(charId, out character) || !this.IsCurrentCombatCharacter(character);
			if (!flag)
			{
				int checkedIndex = ShowSpecialEffectDisplayData.CheckIndex(effectId, index);
				bool flag2 = checkedIndex >= 0;
				if (flag2)
				{
					this.GetCombatCharacter(this._combatCharacterDict[charId].IsAlly, false).NeedShowEffectList.Add(new ShowSpecialEffectDisplayData(charId, effectId, checkedIndex, ItemKey.Invalid));
				}
			}
		}

		// Token: 0x06006369 RID: 25449 RVA: 0x0038678C File Offset: 0x0038498C
		public void ShowSpecialEffectTips(int charId, int effectId, ItemKey itemKey, byte index = 0)
		{
			CombatCharacter character;
			bool flag = !this.IsInCombat() || !this._combatCharacterDict.TryGetValue(charId, out character) || !this.IsCurrentCombatCharacter(character);
			if (!flag)
			{
				int checkedIndex = ShowSpecialEffectDisplayData.CheckIndex(effectId, index);
				bool flag2 = checkedIndex >= 0;
				if (flag2)
				{
					this._combatCharacterDict[charId].NeedShowEffectList.Add(new ShowSpecialEffectDisplayData(charId, effectId, checkedIndex, itemKey));
				}
			}
		}

		// Token: 0x0600636A RID: 25450 RVA: 0x003867FC File Offset: 0x003849FC
		public void ShowSpecialEffectTipsByDisplayEvent(int charId, int effectId, byte index = 0)
		{
			CombatCharacter combatChar;
			bool flag = !this.IsInCombat() || !this._combatCharacterDict.TryGetValue(charId, out combatChar);
			if (!flag)
			{
				int checkedIndex = ShowSpecialEffectDisplayData.CheckIndex(effectId, index);
				ShowSpecialEffectDisplayData data = new ShowSpecialEffectDisplayData(charId, effectId, checkedIndex, ItemKey.Invalid);
				GameDataBridge.AddDisplayEvent<bool, ShowSpecialEffectDisplayData>(DisplayEventType.CombatShowSpecialEffect, combatChar.IsAlly, data);
			}
		}

		// Token: 0x0600636B RID: 25451 RVA: 0x00386854 File Offset: 0x00384A54
		public void ShowTeammateCommand(int teammateId, int index, bool displayEvent = false)
		{
			CombatCharacter teammate;
			bool flag = !this.IsInCombat() || !this._combatCharacterDict.TryGetValue(teammateId, out teammate);
			if (!flag)
			{
				List<sbyte> allCmdTypes = teammate.GetCurrTeammateCommands();
				bool flag2 = !allCmdTypes.CheckIndex(index);
				if (!flag2)
				{
					int[] charIds = this.GetCharacterList(teammate.IsAlly);
					sbyte validIndexCharacter = 0;
					for (int i = 0; i < charIds.Length; i++)
					{
						bool flag3 = charIds[i] == teammateId;
						if (flag3)
						{
							break;
						}
						bool flag4 = charIds[i] >= 0;
						if (flag4)
						{
							validIndexCharacter += 1;
						}
					}
					validIndexCharacter -= 1;
					TeammateCommandDisplayData data = new TeammateCommandDisplayData
					{
						CmdType = allCmdTypes[index],
						IndexCharacter = (sbyte)(charIds.IndexOf(teammateId) - 1),
						ValidIndexCharacter = validIndexCharacter,
						IndexCommand = (sbyte)index,
						IsAlly = teammate.IsAlly
					};
					if (displayEvent)
					{
						GameDataBridge.AddDisplayEvent<TeammateCommandDisplayData>(DisplayEventType.CombatShowCommand, data);
					}
					else
					{
						CombatCharacter mainChar = this.GetMainCharacter(teammate.IsAlly);
						mainChar.NeedShowCommandList.Add(data);
					}
				}
			}
		}

		// Token: 0x0600636C RID: 25452 RVA: 0x00386974 File Offset: 0x00384B74
		public void Reset(DataContext context, CombatCharacter character)
		{
			character.GetCharacter().SetInjuries(default(Injuries), context);
			character.SetInjuries(default(Injuries), context);
			character.SetOldInjuries(default(Injuries), context);
			int[] outerDamageValue = character.GetOuterDamageValue();
			int[] innerDamageValue = character.GetInnerDamageValue();
			Array.Clear(outerDamageValue, 0, outerDamageValue.Length);
			Array.Clear(innerDamageValue, 0, innerDamageValue.Length);
			character.SetOuterDamageValue(outerDamageValue, context);
			character.SetInnerDamageValue(innerDamageValue, context);
			character.SetMindDamageValue(0, context);
			character.SetFatalDamageValue(0, context);
			PoisonInts emptyPoisons = default(PoisonInts);
			character.GetCharacter().SetPoisoned(ref emptyPoisons, context);
			character.SetPoison(ref emptyPoisons, context);
			character.SetOldPoison(ref emptyPoisons, context);
			character.SetFlawCount(new byte[7], context);
			character.SetFlawCollection(new FlawOrAcupointCollection(), context);
			character.SetAcupointCount(new byte[7], context);
			character.SetAcupointCollection(new FlawOrAcupointCollection(), context);
			character.SetMindMarkTime(new MindMarkList(), context);
			this.ClearCombatState(context, character);
			character.UnRegisterMarkHandler();
			character.SetDefeatMarkCollection(new DefeatMarkCollection(), context);
			character.RegisterMarkHandler();
		}

		// Token: 0x0600636D RID: 25453 RVA: 0x00386A94 File Offset: 0x00384C94
		public void AddBossPhase(DataContext context, CombatCharacter character, int effectId)
		{
			int charId = character.GetId();
			bool isAlly = character.IsAlly;
			sbyte newPhase = character.GetBossPhase() + 1;
			this.InterruptSkill(context, character, -1);
			this.InterruptOtherAction(context, character);
			character.NeedChangeBossPhase = true;
			character.ChangeBossPhaseEffectId = effectId;
			character.ClearAllDoingOrReserveCommand(context);
			character.SetMindMarkInfinityCount(0, context);
			character.SetMindMarkInfinityProgress(0, context);
			character.SetHazardValue(0, context);
			character.GetCharacter().SetHealth(character.GetCharacter().GetLeftMaxHealth(false), context);
			character.GetCharacter().SetDisorderOfQi(character.GetOldDisorderOfQi(), context);
			character.SetMixPoisonAffectedCount(character.GetMixPoisonAffectedCount().Clear(), context);
			character.GetCharacter().ClearEatingItems(context);
			this.ClearSkillEffect(context, character);
			List<short> attackSkillList = character.GetAttackSkillList();
			short[] newSkillList = character.BossConfig.PhaseAttackSkills[(int)newPhase];
			for (int i = 0; i < attackSkillList.Count; i++)
			{
				short skillId = attackSkillList[i];
				bool flag = skillId > 0;
				if (flag)
				{
					CombatSkillKey skillKey = new CombatSkillKey(charId, skillId);
					DomainManager.SpecialEffect.Remove(context, charId, skillId, 1);
					this.RemoveElement_SkillDataDict(skillKey);
				}
			}
			sbyte j = 0;
			while ((int)j < attackSkillList.Count)
			{
				short skillId2 = ((int)j < newSkillList.Length) ? newSkillList[(int)j] : -1;
				attackSkillList[(int)j] = skillId2;
				bool flag2 = skillId2 >= 0;
				if (flag2)
				{
					DomainManager.SpecialEffect.Add(context, charId, skillId2, 1, -1);
					this.AddCombatSkillData(context, charId, skillId2);
				}
				j += 1;
			}
			character.SetAttackSkillList(attackSkillList, context);
			bool flag3 = character.BossConfig.PhaseWeapons != null;
			if (flag3)
			{
				short[] weaponList = character.BossConfig.PhaseWeapons[(int)newPhase];
				ItemKey[] combatWeapons = character.GetWeapons();
				GameData.Domains.Character.Character charObj = character.GetCharacter();
				ItemKey[] equipments = charObj.GetEquipment();
				sbyte[] weaponSlots = EquipmentSlot.EquipmentType2Slots[0];
				for (int k = 0; k < weaponSlots.Length; k++)
				{
					ItemKey weaponKey = equipments[(int)weaponSlots[k]];
					bool flag4 = weaponKey.IsValid();
					if (flag4)
					{
						charObj.ChangeEquipment(context, weaponSlots[k], -1, ItemKey.Invalid);
						charObj.RemoveInventoryItem(context, weaponKey, 1, true, false);
						this.RemoveElement_WeaponDataDict(weaponKey.Id);
					}
				}
				for (int l = 0; l < weaponList.Length; l++)
				{
					ItemKey weaponKey2 = DomainManager.Item.CreateWeapon(context, weaponList[l], 0);
					List<sbyte> weaponTricks = DomainManager.Item.GetWeaponTricks(weaponKey2);
					CombatWeaponData weaponData = new CombatWeaponData(weaponKey2, character);
					sbyte[] trickList = weaponData.GetWeaponTricks();
					charObj.AddInventoryItem(context, weaponKey2, 1, false);
					charObj.ChangeEquipment(context, -1, weaponSlots[l], weaponKey2);
					combatWeapons[l] = weaponKey2;
					this.AddElement_WeaponDataDict(weaponKey2.Id, weaponData);
					weaponData.Init(context, l);
					for (int m = 0; m < weaponTricks.Count; m++)
					{
						trickList[m] = weaponTricks[m];
					}
				}
				character.SetWeapons(combatWeapons, context);
				character.SetUsingWeaponIndex(character.GetUsingWeaponIndex(), context);
				this.UpdateCanChangeTrick(context, character);
			}
			else
			{
				ItemKey[] weapons = character.GetWeapons();
				for (int n = 0; n < 3; n++)
				{
					bool flag5 = weapons[n].IsValid();
					if (flag5)
					{
						this.ClearWeaponCd(context, character, n);
					}
				}
			}
			bool flag6 = character.BossConfig.FailPlayerAni != null && character.BossConfig.FailPlayerAni.Count > (int)character.GetBossPhase();
			if (flag6)
			{
				DomainManager.Combat.ForceAllTeammateLeaveCombatField(context, !isAlly);
			}
			Events.RaiseChangeBossPhase(context);
		}

		// Token: 0x0600636E RID: 25454 RVA: 0x00386E54 File Offset: 0x00385054
		[DomainMethod]
		public void SetMoveState(byte state, bool isAlly = true, bool settedByPlayer = false)
		{
			bool flag = !this.CanAcceptCommand();
			if (!flag)
			{
				CombatCharacter combatChar = isAlly ? this._selfChar : this._enemyChar;
				DataContext context = combatChar.GetDataContext();
				byte currState = (!combatChar.KeepMoving) ? 0 : (combatChar.MoveForward ? 1 : 2);
				bool flag2 = currState == state;
				if (!flag2)
				{
					combatChar.KeepMoving = (state > 0);
					if (isAlly)
					{
						combatChar.PlayerControllingMove = (settedByPlayer && combatChar.KeepMoving);
					}
					bool flag3 = state != combatChar.MoveData.JumpPrepareDirection && (combatChar.KeepMoving || (combatChar.MoveData.CanPartlyJump && combatChar.GetJumpPreparedDistance() > 0));
					if (flag3)
					{
						combatChar.MoveData.ResetJumpState(context, true);
					}
					bool keepMoving = combatChar.KeepMoving;
					if (keepMoving)
					{
						combatChar.MoveForward = (state == 1);
						combatChar.MoveData.JumpPrepareDirection = state;
					}
					else
					{
						this.SetProperLoopAniAndParticle(context, combatChar, false);
					}
					Events.RaiseMoveStateChanged(context, combatChar, state);
				}
			}
		}

		// Token: 0x0600636F RID: 25455 RVA: 0x00386F5C File Offset: 0x0038515C
		[DomainMethod]
		public void SetTargetDistance(DataContext context, short targetDistance, bool isAlly = true)
		{
			bool flag = !this.IsInCombat() || this.IsAiMoving;
			if (!flag)
			{
				CombatCharacter combatChar = this.GetCombatCharacter(isAlly, false);
				combatChar.PlayerTargetDistance = targetDistance;
				bool flag2 = this._timeScale <= 0f;
				if (flag2)
				{
					combatChar.SetTargetDistance(targetDistance, context);
				}
			}
		}

		// Token: 0x06006370 RID: 25456 RVA: 0x00386FAE File Offset: 0x003851AE
		[DomainMethod]
		public void ClearTargetDistance(DataContext context)
		{
			this.SetTargetDistance(context, -1, true);
		}

		// Token: 0x06006371 RID: 25457 RVA: 0x00386FBC File Offset: 0x003851BC
		[DomainMethod]
		public bool SetJumpThreshold(DataContext context, short combatSkillId, short jumpThreshold)
		{
			return DomainManager.Extra.SetJumpThreshold(context, combatSkillId, jumpThreshold);
		}

		// Token: 0x06006372 RID: 25458 RVA: 0x00386FDC File Offset: 0x003851DC
		[return: TupleElementNames(new string[]
		{
			"min",
			"max"
		})]
		public ValueTuple<byte, byte> GetDistanceRange()
		{
			return new ValueTuple<byte, byte>(this.CombatConfig.MinDistance, this.CombatConfig.MaxDistance);
		}

		// Token: 0x06006373 RID: 25459 RVA: 0x00387009 File Offset: 0x00385209
		public short GetMoveRangeOffsetCurrentDistance(int offset)
		{
			return this.GetMoveRangeDistance((int)this._currentDistance + offset);
		}

		// Token: 0x06006374 RID: 25460 RVA: 0x0038701C File Offset: 0x0038521C
		public short GetMoveRangeDistance(int distance)
		{
			ValueTuple<byte, byte> distanceRange = this.GetDistanceRange();
			byte min = distanceRange.Item1;
			byte max = distanceRange.Item2;
			return (short)Math.Clamp(distance, (int)min, (int)max);
		}

		// Token: 0x06006375 RID: 25461 RVA: 0x0038704C File Offset: 0x0038524C
		public short GetNearlyOutDistance(OuterAndInnerShorts range)
		{
			ValueTuple<byte, byte> distanceRange = this.GetDistanceRange();
			int forwardDistance = (int)(this._currentDistance - range.Outer);
			int backwardDistance = (int)(range.Inner - this._currentDistance);
			bool canForward = range.Outer - 1 >= (short)distanceRange.Item1;
			bool canBackward = range.Inner + 1 <= (short)distanceRange.Item2;
			bool flag = !canForward && !canBackward;
			short result;
			if (flag)
			{
				result = -1;
			}
			else
			{
				result = (((forwardDistance < backwardDistance || !canBackward) && canForward) ? (range.Outer - 1) : (range.Inner + 1));
			}
			return result;
		}

		// Token: 0x06006376 RID: 25462 RVA: 0x003870EC File Offset: 0x003852EC
		public bool CanMove(CombatCharacter combatChar, bool forward)
		{
			bool flag = !combatChar.GetCharacter().Template.CanMove;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				bool isMoving = combatChar.IsMoving;
				if (isMoving)
				{
					result = false;
				}
				else
				{
					ValueTuple<byte, byte> distanceRange = this.GetDistanceRange();
					bool flag2 = forward ? (this._currentDistance <= (short)distanceRange.Item1) : (this._currentDistance >= (short)distanceRange.Item2);
					if (flag2)
					{
						result = false;
					}
					else
					{
						bool flag3 = combatChar.TeammateBeforeMainChar >= 0 || combatChar.TeammateAfterMainChar >= 0;
						if (flag3)
						{
							result = false;
						}
						else
						{
							bool flag4 = this._isTutorialCombat && !combatChar.CanRecoverMobility;
							if (flag4)
							{
								result = false;
							}
							else
							{
								CombatCharacterStateType currState = combatChar.StateMachine.GetCurrentStateType();
								bool flag5 = currState == CombatCharacterStateType.PrepareSkill;
								bool canMove;
								bool flag6;
								if (flag5)
								{
									canMove = ((forward ? combatChar.MoveData.CanMoveForwardInSkillPrepareDist : combatChar.MoveData.CanMoveBackwardInSkillPrepareDist) > 0);
								}
								else
								{
									flag6 = (currState == CombatCharacterStateType.Idle || currState == CombatCharacterStateType.PrepareOtherAction);
									canMove = flag6;
								}
								short combatConfigId = DomainManager.Combat.CombatConfig.TemplateId;
								flag6 = (combatConfigId - 157 <= 1);
								bool flag7 = flag6;
								if (flag7)
								{
									canMove = (canMove && combatChar.GetAffectingMoveSkillId() >= 0);
								}
								result = canMove;
							}
						}
					}
				}
			}
			return result;
		}

		// Token: 0x06006377 RID: 25463 RVA: 0x00387248 File Offset: 0x00385448
		public void RecoverMobilityValue(DataContext context, CombatCharacter character)
		{
			int currMobility = character.GetMobilityValue();
			int maxMobility = character.GetMaxMobility();
			bool flag = currMobility >= maxMobility || !character.CanRecoverMobility;
			if (!flag)
			{
				this.ChangeMobilityValue(context, character, character.GetMobilityRecoverSpeed(), false, null, false);
			}
		}

		// Token: 0x06006378 RID: 25464 RVA: 0x0038728C File Offset: 0x0038548C
		public void ChangeMobilityValue(DataContext context, CombatCharacter character, int addValue, bool changedByEffect = false, CombatCharacter changer = null, bool costBySkill = false)
		{
			bool flag = changedByEffect && addValue < 0;
			if (flag)
			{
				bool flag2 = !DomainManager.SpecialEffect.ModifyData(character.GetId(), -1, 149, true, (changer != null) ? changer.GetId() : -1, -1, -1);
				if (flag2)
				{
					return;
				}
				addValue = DomainManager.SpecialEffect.ModifyValue(character.GetId(), 150, addValue, (changer != null) ? changer.GetId() : -1, -1, -1, 0, 0, 0, 0);
			}
			int maxMobility = character.GetMaxMobility();
			int mobilityValue = Math.Clamp(character.GetMobilityValue() + addValue, 0, maxMobility);
			bool flag3 = mobilityValue == character.GetMobilityValue();
			if (!flag3)
			{
				character.SetMobilityValue(mobilityValue, context);
				bool flag4 = this.IsCurrentCombatCharacter(character);
				if (flag4)
				{
					this.UpdateSkillNeedMobilityCanUse(context, character);
				}
			}
		}

		// Token: 0x06006379 RID: 25465 RVA: 0x0038734E File Offset: 0x0038554E
		public bool ChangeDistance(DataContext context, CombatCharacter mover, int addDistance)
		{
			return this.ChangeDistance(context, mover, addDistance, false, true);
		}

		// Token: 0x0600637A RID: 25466 RVA: 0x0038735B File Offset: 0x0038555B
		public bool ChangeDistance(DataContext context, CombatCharacter mover, int addDistance, bool isForced)
		{
			return this.ChangeDistance(context, mover, addDistance, isForced, true);
		}

		// Token: 0x0600637B RID: 25467 RVA: 0x0038736C File Offset: 0x0038556C
		public bool ChangeDistance(DataContext context, CombatCharacter mover, int addDistance, bool isForced, bool canStop)
		{
			bool lockDistance = DomainManager.SpecialEffect.ModifyData(-1, -1, 244, false, -1, -1, -1);
			bool flag = lockDistance && canStop;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				bool isMove = DomainManager.SpecialEffect.ModifyData(mover.GetId(), -1, 157, true, -1, -1, -1);
				int modifiedDistance = addDistance;
				bool flag2 = isMove;
				if (flag2)
				{
					modifiedDistance = DomainManager.SpecialEffect.ModifyData(mover.GetId(), -1, 151, addDistance, (addDistance < 0) ? 1 : 0, -1, -1);
					bool moveCanBeStopped = DomainManager.SpecialEffect.ModifyData(mover.GetId(), -1, 147, true, -1, -1, -1);
					bool flag3 = !moveCanBeStopped;
					if (flag3)
					{
						modifiedDistance = ((addDistance > 0) ? Math.Max(addDistance, modifiedDistance) : Math.Min(addDistance, modifiedDistance));
					}
					if (isForced)
					{
						bool canForcedMove = DomainManager.SpecialEffect.ModifyData(mover.GetId(), -1, 148, true, addDistance, -1, -1);
						bool flag4 = !canForcedMove;
						if (flag4)
						{
							Events.RaiseIgnoredForceChangeDistance(context, mover, addDistance);
							return false;
						}
					}
				}
				ValueTuple<byte, byte> distanceRange = this.GetDistanceRange();
				byte newDistance = (byte)Math.Clamp((int)this._currentDistance + modifiedDistance, (int)distanceRange.Item1, (int)distanceRange.Item2);
				int movedDist = (int)((short)newDistance - this._currentDistance);
				bool flag5 = movedDist == 0;
				if (flag5)
				{
					result = true;
				}
				else
				{
					int newPos = mover.GetCurrentPosition() + movedDist * (mover.IsAlly ? -1 : 1);
					bool needUpdateMoveAni = this._currentDistance <= GlobalConfig.Instance.FastWalkDistance != (short)newDistance <= GlobalConfig.Instance.FastWalkDistance;
					mover.SetCurrentPosition((int)((short)newPos), context);
					this.SetCurrentDistance((short)newDistance, context);
					this.UpdateSkillNeedDistanceCanUse(context, this._selfChar);
					this.UpdateSkillNeedDistanceCanUse(context, this._enemyChar);
					this.UpdateOtherActionCanUse(context, this._selfChar, 2);
					this.UpdateOtherActionCanUse(context, this._enemyChar, 2);
					this.UpdateAllTeammateCommandUsable(context, true, -1);
					this.UpdateAllTeammateCommandUsable(context, false, -1);
					this.UpdateShowUseSpecialMisc(context);
					bool flag6 = needUpdateMoveAni;
					if (flag6)
					{
						this.SetProperLoopAniAndParticle(context, this._selfChar, false);
						this.SetProperLoopAniAndParticle(context, this._enemyChar, false);
					}
					bool flag7 = isMove && mover.PoisonOverflow(1);
					if (flag7)
					{
						mover.AddPoisonAffectValue(1, (short)Math.Abs(modifiedDistance), false);
					}
					Events.RaiseDistanceChanged(context, mover, (short)movedDist, isMove, isForced);
					result = true;
				}
			}
			return result;
		}

		// Token: 0x0600637C RID: 25468 RVA: 0x003875BD File Offset: 0x003857BD
		public void EnsureDistance(DataContext context, CombatCharacter checker)
		{
			this.ChangeDistance(context, checker, 0, false, false);
		}

		// Token: 0x0600637D RID: 25469 RVA: 0x003875CC File Offset: 0x003857CC
		public int GetSkillCostMobilityPerFrame(CombatCharacter character, short skillId)
		{
			GameData.Domains.CombatSkill.CombatSkill skill = DomainManager.CombatSkill.GetElement_CombatSkills(new CombatSkillKey(character.GetId(), skillId));
			int breakPercent = skill.GetBreakoutGridCombatSkillPropertyBonus(16);
			foreach (SkillBreakPageEffectImplementItem effect in skill.GetPageEffects())
			{
				breakPercent += effect.CostMobilityByFrame;
			}
			foreach (SkillBreakPlateBonus bonus in skill.GetBreakBonuses())
			{
				breakPercent += bonus.CalcCostMobilityByFrame();
			}
			int costMobility = Config.CombatSkill.Instance[skillId].MobilityReduceSpeed;
			costMobility = DomainManager.SpecialEffect.ModifyValue(character.GetId(), skillId, 179, costMobility, -1, -1, -1, 0, breakPercent, 0, 0);
			return Math.Max(costMobility, 0);
		}

		// Token: 0x0600637E RID: 25470 RVA: 0x003876C4 File Offset: 0x003858C4
		public int GetSkillMoveCostMobility(CombatCharacter character, short skillId)
		{
			int charId = character.GetId();
			GameData.Domains.CombatSkill.CombatSkill skill = DomainManager.CombatSkill.GetElement_CombatSkills(new CombatSkillKey(charId, skillId));
			int gridBonus = skill.GetBreakoutGridCombatSkillPropertyBonus(17);
			foreach (SkillBreakPageEffectImplementItem effect in skill.GetPageEffects())
			{
				gridBonus += effect.CostMobilityByMove;
			}
			foreach (SkillBreakPlateBonus bonus in skill.GetBreakBonuses())
			{
				gridBonus += bonus.CalcCostMobilityByMove();
			}
			int costSkillMobility = Config.CombatSkill.Instance[skillId].MoveCostMobility;
			return DomainManager.SpecialEffect.ModifyValueCustom(charId, skillId, 175, costSkillMobility, -1, -1, -1, 0, gridBonus, 0, 0);
		}

		// Token: 0x0600637F RID: 25471 RVA: 0x003877B8 File Offset: 0x003859B8
		public bool InAttackRange(CombatCharacter character)
		{
			bool canAttackOutRange = character.GetCanAttackOutRange();
			bool result;
			if (canAttackOutRange)
			{
				result = true;
			}
			else
			{
				short distance = this.GetCurrentDistance();
				OuterAndInnerShorts attackRange = character.GetAttackRange();
				result = (distance >= attackRange.Outer && distance <= attackRange.Inner);
			}
			return result;
		}

		// Token: 0x06006380 RID: 25472 RVA: 0x00387800 File Offset: 0x00385A00
		public bool AnyAttackRangeEdge(CombatCharacter character)
		{
			bool canAttackOutRange = character.GetCanAttackOutRange();
			bool result;
			if (canAttackOutRange)
			{
				result = false;
			}
			else
			{
				ValueTuple<byte, byte> moveRange = this.GetDistanceRange();
				OuterAndInnerShorts attackRange = character.GetAttackRange();
				result = ((short)moveRange.Item1 < attackRange.Outer || (short)moveRange.Item2 > attackRange.Inner);
			}
			return result;
		}

		// Token: 0x06006381 RID: 25473 RVA: 0x00387850 File Offset: 0x00385A50
		public bool IsMovedByTeammate(CombatCharacter character)
		{
			bool flag = character.TeammateAfterMainChar < 0;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				CombatCharacter teammateChar = this._combatCharacterDict[character.TeammateAfterMainChar];
				ETeammateCommandImplement executingTeammateCommandImplement = teammateChar.ExecutingTeammateCommandImplement;
				bool flag2 = executingTeammateCommandImplement - ETeammateCommandImplement.Push <= 1;
				result = flag2;
			}
			return result;
		}

		// Token: 0x06006382 RID: 25474 RVA: 0x003878A0 File Offset: 0x00385AA0
		public int GetDisplayPosition(bool isAlly, short distance)
		{
			CombatCharacter enemyChar = DomainManager.Combat.GetCombatCharacter(!isAlly, true);
			int enemyPos = enemyChar.GetDisplayPosition();
			bool flag = enemyPos == int.MinValue;
			if (flag)
			{
				enemyPos = enemyChar.GetCurrentPosition();
			}
			return (int)((short)(isAlly ? (enemyPos - (int)distance) : (enemyPos + (int)distance)));
		}

		// Token: 0x06006383 RID: 25475 RVA: 0x003878E8 File Offset: 0x00385AE8
		public void SetDisplayPosition(DataContext context, bool isAlly, int displayPos)
		{
			CombatCharacter currChar = this.GetCombatCharacter(isAlly, false);
			int mainCharPos = (displayPos != int.MinValue) ? displayPos : currChar.GetCurrentPosition();
			bool flag = currChar.TeammateBeforeMainChar >= 0;
			if (flag)
			{
				CombatCharacter teammateBefore = this._combatCharacterDict[currChar.TeammateBeforeMainChar];
				short posOffset = teammateBefore.ExecutingTeammateCommandConfig.PosOffset;
				teammateBefore.SetDisplayPosition(mainCharPos, context);
				displayPos = (mainCharPos += (int)(posOffset * (isAlly ? -1 : 1)));
			}
			currChar.SetDisplayPosition(displayPos, context);
			bool flag2 = currChar.TeammateAfterMainChar >= 0;
			if (flag2)
			{
				CombatCharacter teammateAfter = this._combatCharacterDict[currChar.TeammateAfterMainChar];
				short posOffset2 = teammateAfter.ExecutingTeammateCommandConfig.PosOffset;
				teammateAfter.SetDisplayPosition(mainCharPos + (int)(posOffset2 * (isAlly ? 1 : -1)), context);
			}
		}

		// Token: 0x06006384 RID: 25476 RVA: 0x003879B0 File Offset: 0x00385BB0
		public void EnableJumpMove(CombatCharacter character, short skillId)
		{
			CombatSkillItem skillConfig = Config.CombatSkill.Instance[skillId];
			this.EnableJumpMove(character, skillId, skillConfig.CanPartlyJump, (short)skillConfig.MaxJumpDistance, (short)skillConfig.MaxJumpDistance, skillConfig.JumpPrepareFrame);
		}

		// Token: 0x06006385 RID: 25477 RVA: 0x003879EC File Offset: 0x00385BEC
		public void EnableJumpMove(CombatCharacter character, short skillId, bool isForward)
		{
			CombatSkillItem skillConfig = Config.CombatSkill.Instance[skillId];
			this.EnableJumpMove(character, skillId, skillConfig.CanPartlyJump, (short)(isForward ? skillConfig.MaxJumpDistance : 0), (short)(isForward ? 0 : skillConfig.MaxJumpDistance), skillConfig.JumpPrepareFrame);
		}

		// Token: 0x06006386 RID: 25478 RVA: 0x00387A34 File Offset: 0x00385C34
		public void EnableJumpMove(CombatCharacter character, short skillId, bool canPartlyJump, short maxForwardDist, short maxBackwardDist, int prepareFrame)
		{
			character.MoveData.JumpMoveSkillId = skillId;
			character.MoveData.CanPartlyJump = canPartlyJump;
			character.MoveData.MaxJumpForwardDist = maxForwardDist;
			character.MoveData.MaxJumpBackwardDist = maxBackwardDist;
			character.MoveData.PrepareProgressUnit = prepareFrame;
			bool flag = skillId >= 0 && Config.CombatSkill.Instance[skillId].EquipType == 2;
			if (flag)
			{
				character.PauseJumpMoveSkillId = skillId;
			}
		}

		// Token: 0x06006387 RID: 25479 RVA: 0x00387AA8 File Offset: 0x00385CA8
		public void DisableJumpMove(DataContext context, CombatCharacter character, short skillId)
		{
			bool flag = character.MoveData.JumpMoveSkillId == skillId;
			if (flag)
			{
				this.DisableJumpMove(context, character);
			}
		}

		// Token: 0x06006388 RID: 25480 RVA: 0x00387AD4 File Offset: 0x00385CD4
		public void DisableJumpMove(DataContext context, CombatCharacter character)
		{
			character.MoveData.JumpMoveSkillId = -1;
			character.MoveData.MaxJumpForwardDist = 0;
			character.MoveData.MaxJumpBackwardDist = 0;
			character.MoveData.PrepareProgressUnit = 0;
			character.MoveData.ResetJumpState(context, true);
		}

		// Token: 0x170003D0 RID: 976
		// (get) Token: 0x06006389 RID: 25481 RVA: 0x00387B20 File Offset: 0x00385D20
		// (set) Token: 0x0600638A RID: 25482 RVA: 0x00387B28 File Offset: 0x00385D28
		public float SelfAvgEquipGrade { get; private set; }

		// Token: 0x170003D1 RID: 977
		// (get) Token: 0x0600638B RID: 25483 RVA: 0x00387B31 File Offset: 0x00385D31
		// (set) Token: 0x0600638C RID: 25484 RVA: 0x00387B39 File Offset: 0x00385D39
		public float EnemyAvgEquipGrade { get; private set; }

		// Token: 0x0600638D RID: 25485 RVA: 0x00387B44 File Offset: 0x00385D44
		[DomainMethod]
		public CombatResultDisplayData GetCombatResultDisplayData()
		{
			return this._combatResultData;
		}

		// Token: 0x0600638E RID: 25486 RVA: 0x00387B5C File Offset: 0x00385D5C
		[DomainMethod]
		public void SelectGetItem(DataContext context, List<ItemKey> acceptItems, List<int> acceptCounts)
		{
			bool flag = acceptItems != null;
			if (flag)
			{
				GameData.Domains.Character.Character charObj = this._selfChar.GetCharacter();
				for (int i = 0; i < acceptItems.Count; i++)
				{
					ItemKey key = acceptItems[i];
					bool flag2 = this._combatResultData.ItemSrcCharDict.ContainsKey(key);
					if (flag2)
					{
						bool flag3 = this._combatResultData.ItemSrcCharDict[key] != this._selfChar.GetId();
						if (flag3)
						{
							GameData.Domains.Character.Character enemyChar = DomainManager.Character.GetElement_Objects(this._combatResultData.ItemSrcCharDict[key]);
							int acceptCount = acceptCounts[i];
							int deleteCount = enemyChar.GetInventory().Items[key] - acceptCount;
							DomainManager.Character.TransferInventoryItem(context, enemyChar, charObj, key, acceptCount);
							bool flag4 = deleteCount > 0;
							if (flag4)
							{
								enemyChar.RemoveInventoryItem(context, key, deleteCount, true, false);
							}
						}
						else
						{
							DomainManager.TaiwuEvent.SetListenerEventActionISerializableArg("CombatOver", "CarrierItemKeyGotInCombat", key);
						}
						this._combatResultData.ItemSrcCharDict.Remove(key);
					}
				}
			}
			foreach (KeyValuePair<ItemKey, int> entry in this._combatResultData.ItemSrcCharDict)
			{
				GameData.Domains.Character.Character character = DomainManager.Character.GetElement_Objects(entry.Value);
				character.RemoveInventoryItem(context, entry.Key, character.GetInventory().Items[entry.Key], true, false);
			}
		}

		// Token: 0x0600638F RID: 25487 RVA: 0x00387D10 File Offset: 0x00385F10
		public void UpdateMaxSkillGrade(bool isAlly, short skillId)
		{
			if (isAlly)
			{
				this.SelfMaxSkillGrade = Math.Max(this.SelfMaxSkillGrade, Config.CombatSkill.Instance[skillId].Grade);
			}
			else
			{
				this.EnemyMaxSkillGrade = Math.Max(this.EnemyMaxSkillGrade, Config.CombatSkill.Instance[skillId].Grade);
			}
		}

		// Token: 0x06006390 RID: 25488 RVA: 0x00387D67 File Offset: 0x00385F67
		public bool IsCharInLoot(int charId)
		{
			return this._lootCharList.Contains(charId);
		}

		// Token: 0x06006391 RID: 25489 RVA: 0x00387D75 File Offset: 0x00385F75
		public void AppendGetChar(int charId)
		{
			this._lootCharList.Add(charId);
		}

		// Token: 0x06006392 RID: 25490 RVA: 0x00387D85 File Offset: 0x00385F85
		public void AppendGetItem(ItemKey itemKey)
		{
			this._combatResultData.ItemList.Add(DomainManager.Item.GetItemDisplayData(itemKey, this._selfCharId));
			this._combatResultData.ItemSrcCharDict.Add(itemKey, this._selfCharId);
		}

		// Token: 0x06006393 RID: 25491 RVA: 0x00387DC4 File Offset: 0x00385FC4
		public void AppendEvaluation(sbyte evaluationId)
		{
			bool flag = this._combatResultData.EvaluationList.Contains(evaluationId);
			if (!flag)
			{
				this._combatResultData.EvaluationList.Add(evaluationId);
			}
		}

		// Token: 0x06006394 RID: 25492 RVA: 0x00387DFC File Offset: 0x00385FFC
		public bool CheckEvaluation(sbyte evaluationTemplateId)
		{
			CombatEvaluationItem evaluation = CombatEvaluation.Instance[evaluationTemplateId];
			bool flag = evaluation.AvailableInPlayground != this._isPlaygroundCombat;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				List<short> requireCombatConfigs = evaluation.RequireCombatConfigs;
				bool flag2 = requireCombatConfigs != null && requireCombatConfigs.Count > 0 && !evaluation.RequireCombatConfigs.Contains(this.CombatConfig.TemplateId);
				result = (!flag2 && evaluation.CombatTypes.Exist(this._combatType));
			}
			return result;
		}

		// Token: 0x06006395 RID: 25493 RVA: 0x00387E80 File Offset: 0x00386080
		private void CalcEvaluationList(DataContext context)
		{
			foreach (CombatEvaluationItem evaluation in ((IEnumerable<CombatEvaluationItem>)CombatEvaluation.Instance))
			{
				ECombatEvaluationExtraCheck extraCheck = evaluation.ExtraCheck;
				bool flag = extraCheck - ECombatEvaluationExtraCheck.Invalid <= 1;
				bool flag2 = flag;
				if (!flag2)
				{
					bool flag3 = evaluation.NeedWin && !this.IsWin(true);
					if (!flag3)
					{
						bool flag4 = !this.CheckEvaluation(evaluation.TemplateId);
						if (!flag4)
						{
							bool flag5 = evaluation.ExtraCheck != ECombatEvaluationExtraCheck.None;
							if (flag5)
							{
								Func<bool> checker;
								bool flag6 = !CombatDomain.EvaluationCheckers.TryGetValue(evaluation.ExtraCheck, out checker);
								if (flag6)
								{
									short predefinedLogId = 8;
									DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(55, 1);
									defaultInterpolatedStringHandler.AppendLiteral("Unexpect checker type ");
									defaultInterpolatedStringHandler.AppendFormatted<ECombatEvaluationExtraCheck>(evaluation.ExtraCheck);
									defaultInterpolatedStringHandler.AppendLiteral(", evaluation will always be true.");
									PredefinedLog.Show(predefinedLogId, defaultInterpolatedStringHandler.ToStringAndClear());
								}
								else
								{
									bool flag7 = !checker();
									if (flag7)
									{
										continue;
									}
								}
							}
							this.AppendEvaluation(evaluation.TemplateId);
						}
					}
				}
			}
			this._combatResultData.EvaluationList.Sort();
		}

		// Token: 0x06006396 RID: 25494 RVA: 0x00387FD8 File Offset: 0x003861D8
		private void CalcReadInCombat(DataContext context)
		{
			this._combatResultData.ShowReadingEvent = false;
			sbyte readInCombatCount = DomainManager.Taiwu.GetReadInCombatCount();
			ItemKey currBook = DomainManager.Taiwu.GetCurReadingBook();
			bool flag = readInCombatCount <= 0 || !currBook.IsValid() || Config.SkillBook.Instance[currBook.TemplateId].CombatSkillTemplateId < 0;
			if (!flag)
			{
				int odds = this._combatResultData.SelectEvaluations<int>((CombatEvaluationItem x) => x.ReadInCombatRate).Sum((int x) => x);
				bool flag2 = !context.Random.CheckPercentProb(odds);
				if (!flag2)
				{
					DomainManager.Taiwu.SetReadInCombatCount(readInCombatCount - 1, context);
					this._combatResultData.EvaluationList.Add(33);
					this._combatResultData.ShowReadingEvent = DomainManager.Taiwu.UpdateReadingProgressInCombat(context);
					bool showReadingEvent = this._combatResultData.ShowReadingEvent;
					if (showReadingEvent)
					{
						DomainManager.Extra.AddReadingEventBookId(context, currBook.Id);
					}
				}
			}
		}

		// Token: 0x06006397 RID: 25495 RVA: 0x003880FC File Offset: 0x003862FC
		private void CalcQiQrtInCombat(DataContext context)
		{
			sbyte loopInCombatCount = DomainManager.Extra.GetLoopInCombatCount();
			this._combatResultData.ShowLoopingEvent = false;
			bool flag = loopInCombatCount <= 0;
			if (!flag)
			{
				GameData.Domains.Character.Character taiwuChar = DomainManager.Taiwu.GetTaiwu();
				short loopingNeigongTemplateId = taiwuChar.GetLoopingNeigong();
				bool flag2 = loopingNeigongTemplateId < 0;
				if (!flag2)
				{
					int percentProb = this._combatResultData.SelectEvaluations<int>((CombatEvaluationItem x) => x.QiArtCombatRate).Sum((int x) => x);
					bool flag3 = !context.Random.CheckPercentProb(percentProb);
					if (!flag3)
					{
						DomainManager.Extra.SetLoopInCombatCount(loopInCombatCount - 1, context);
						DomainManager.Taiwu.ApplyNeigongLoopingImprovementOnce(context, 100);
						this._combatResultData.EvaluationList.Add(43);
						InstantNotificationCollection instantCollection = DomainManager.World.GetInstantNotificationCollection();
						instantCollection.AddQiArtInCombatNoChance(loopingNeigongTemplateId);
						this._combatResultData.ShowLoopingEvent = DomainManager.Taiwu.TryAddLoopingEvent(context, percentProb);
					}
				}
			}
		}

		// Token: 0x06006398 RID: 25496 RVA: 0x00388218 File Offset: 0x00386418
		private void CalcAddLegacyPoint(DataContext context)
		{
			bool win = CombatResultType.IsPlayerWin(this._combatResultData.CombatStatus);
			Dictionary<short, int> legacyPoints = ObjectPool<Dictionary<short, int>>.Instance.Get();
			legacyPoints.Clear();
			foreach (CombatEvaluationItem evaluation in this._combatResultData.Evaluations)
			{
				foreach (LegacyPointReference reference in evaluation.AddLegacyPoint)
				{
					int percent = win ? reference.WinPercent : reference.FailPercent;
					legacyPoints[reference.TemplateId] = legacyPoints.GetOrDefault(reference.TemplateId) + percent;
				}
			}
			foreach (KeyValuePair<short, int> keyValuePair in legacyPoints)
			{
				short num;
				int num2;
				keyValuePair.Deconstruct(out num, out num2);
				short templateId = num;
				int percent2 = num2;
				bool flag = percent2 <= 0;
				if (!flag)
				{
					DomainManager.Taiwu.AddLegacyPoint(context, templateId, percent2);
				}
			}
			ObjectPool<Dictionary<short, int>>.Instance.Return(legacyPoints);
		}

		// Token: 0x06006399 RID: 25497 RVA: 0x0038837C File Offset: 0x0038657C
		private void CalcAndAddFameAction(DataContext context)
		{
			GameData.Domains.Character.Character charObj = this._selfChar.GetCharacter();
			foreach (short fameAction in from x in this._combatResultData.SelectEvaluations<short>((CombatEvaluationItem x) => x.FameAction)
			where x >= 0
			select x)
			{
				charObj.RecordFameAction(context, fameAction, -1, 1, true);
			}
		}

		// Token: 0x0600639A RID: 25498 RVA: 0x00388428 File Offset: 0x00386628
		private void CalcAndAddAreaSpiritualDebt(DataContext context)
		{
			int areaSpiritualDebt = 0;
			areaSpiritualDebt += this._combatResultData.SelectEvaluations<short>((CombatEvaluationItem x) => x.AreaSpiritualDebt).Sum((short x) => (int)x);
			bool flag = this.IsWin(true);
			if (flag)
			{
				areaSpiritualDebt += this.GetAddAreaSpiritualDebt(this._enemyTeam);
			}
			this._combatResultData.AreaSpiritualDebt = areaSpiritualDebt;
			bool flag2 = areaSpiritualDebt == 0;
			if (!flag2)
			{
				Location location = this._selfChar.GetCharacter().GetLocation();
				bool flag3 = !location.IsValid();
				if (flag3)
				{
					location = this._selfChar.GetCharacter().GetValidLocation();
				}
				MapAreaData area = DomainManager.Map.GetElement_Areas((int)location.AreaId);
				DomainManager.Extra.ChangeAreaSpiritualDebt(context, location.AreaId, this._combatResultData.AreaSpiritualDebt, true, true);
			}
		}

		// Token: 0x0600639B RID: 25499 RVA: 0x00388520 File Offset: 0x00386720
		private void CalcAndAddExp(DataContext context)
		{
			bool flag = !this.CombatConfig.DropResource || this._isPlaygroundCombat;
			if (!flag)
			{
				int exp = CombatDomain.CalcAddBase(this._enemyTeam, GlobalConfig.Instance.CombatGetExpBase, false);
				GameData.Domains.Character.Character selfChar = this._selfChar.GetCharacter();
				int consummateLevel = (int)(selfChar.IsLoseConsummateBonusByFeature() ? 0 : selfChar.GetConsummateLevel());
				int extraAddPercent = ConsummateLevel.Instance[consummateLevel].ExpBonus;
				exp = this._combatResultData.ModifyValue(exp, (CombatEvaluationItem x) => (int)x.ExpAddPercent, (CombatEvaluationItem x) => (int)x.ExpTotalPercent, extraAddPercent);
				exp = Math.Max(exp, 0);
				this._combatResultData.Exp = exp;
				DomainManager.Taiwu.GetTaiwu().ChangeExp(context, this._combatResultData.Exp);
				Events.RaiseEvaluationAddExp(context, this._combatResultData.Exp);
				bool flag2 = (int)this._enemyChar.GetCharacter().GetConsummateLevel() < consummateLevel || !this.IsWin(true);
				if (!flag2)
				{
					bool enemyIsIntelligent = this._enemyChar.GetCharacter().GetCreatingType() == 1;
					sbyte enemyOrganization = this._enemyChar.GetCharacter().GetOrganizationInfo().OrgTemplateId;
					foreach (ExpAddProfessionSeniorityData data in ExpAddProfessionSeniorityData.AllData)
					{
						bool flag3 = data.RequireCombatType >= 0 && data.RequireCombatType != this._combatType;
						if (!flag3)
						{
							bool flag4 = data.RequireIntelligent && !enemyIsIntelligent;
							if (!flag4)
							{
								bool flag5 = data.RequireOrganization >= 0 && data.RequireOrganization != enemyOrganization;
								if (!flag5)
								{
									data.DoAddSeniority(context, exp);
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x0600639C RID: 25500 RVA: 0x00388730 File Offset: 0x00386930
		private unsafe void CalcAndAddResource(DataContext context)
		{
			bool flag = !this.CombatConfig.DropResource || this._isPlaygroundCombat;
			if (!flag)
			{
				for (sbyte i = 0; i < 8; i += 1)
				{
					int value = CombatDomain.CalcAddResource(this._enemyTeam, i, false);
					bool flag2 = i == 7;
					if (flag2)
					{
						value += CombatDomain.CalcAddBase(this._enemyTeam, GlobalConfig.Instance.CombatGetAuthorityBase, false);
					}
					value = value * DomainManager.World.GetGainResourcePercent(8) / 100;
					bool flag3 = i == 7;
					if (flag3)
					{
						value = this._combatResultData.ModifyValue(value, (CombatEvaluationItem x) => (int)x.AuthorityAddPercent, (CombatEvaluationItem x) => (int)x.AuthorityTotalPercent, 0);
					}
					else
					{
						bool flag4 = !this._combatResultData.IsWin;
						if (flag4)
						{
							value = 0;
						}
					}
					value = Math.Max(value, 0);
					*this._combatResultData.Resource[(int)i] = value;
					this._selfChar.GetCharacter().ChangeResource(context, i, value);
				}
			}
		}

		// Token: 0x0600639D RID: 25501 RVA: 0x00388858 File Offset: 0x00386A58
		private void CalcAndAddProficiency(DataContext context)
		{
			bool flag = !this.CombatConfig.DropResource || this._isPlaygroundCombat;
			if (!flag)
			{
				bool flag2 = this._combatResultData.Evaluations.Any((CombatEvaluationItem evaluation) => !evaluation.AllowProficiency);
				if (!flag2)
				{
					foreach (short skillId in this._selfChar.GetCombatSkillIds())
					{
						CombatSkillKey key = new CombatSkillKey(this._selfChar.GetId(), skillId);
						int deltaTarget = this.CalcProficiencyDeltaTarget(context.Random, key);
						int delta = DomainManager.Extra.ChangeCombatSkillProficiency(context, key, deltaTarget);
						bool flag3 = delta == 0;
						if (!flag3)
						{
							CombatResultDisplayData combatResultData = this._combatResultData;
							if (combatResultData.ChangedProficiencies == null)
							{
								combatResultData.ChangedProficiencies = new Dictionary<short, int>();
							}
							this._combatResultData.ChangedProficiencies[skillId] = DomainManager.Extra.GetElement_CombatSkillProficiencies(key);
							combatResultData = this._combatResultData;
							if (combatResultData.ChangedProficienciesDelta == null)
							{
								combatResultData.ChangedProficienciesDelta = new Dictionary<short, int>();
							}
							this._combatResultData.ChangedProficienciesDelta[skillId] = delta;
						}
					}
					bool flag4 = this._enemyChar.GetCharacter().GetCreatingType() != 1;
					if (!flag4)
					{
						foreach (short skillId2 in this._enemyChar.GetCombatSkillIds())
						{
							CombatSkillKey key2 = new CombatSkillKey(this._enemyChar.GetId(), skillId2);
							int deltaTarget2 = this.CalcProficiencyDeltaTarget(context.Random, key2);
							DomainManager.Extra.ChangeCombatSkillProficiency(context, key2, deltaTarget2);
						}
					}
				}
			}
		}

		// Token: 0x0600639E RID: 25502 RVA: 0x00388A48 File Offset: 0x00386C48
		private int CalcProficiencyDeltaTarget(IRandomSource random, CombatSkillKey key)
		{
			sbyte equipType = Config.CombatSkill.Instance[key.SkillTemplateId].EquipType;
			bool flag = GameData.Domains.Character.CombatSkillHelper.IsProactiveSkill(equipType);
			int result;
			if (flag)
			{
				result = this._skillCastTimes.GetOrDefault(key) * (int)GlobalConfig.ProactiveProficiencyFactor[(int)this._combatType];
			}
			else
			{
				int rangeMin = 1;
				int rangeMax = 3;
				result = random.Next(rangeMin, rangeMax + 1);
			}
			return result;
		}

		// Token: 0x0600639F RID: 25503 RVA: 0x00388AA8 File Offset: 0x00386CA8
		public static short GetWorldLootRatePercent()
		{
			short lootYield = DomainManager.Extra.GetLootYield();
			WorldCreationItem worldCreationItem = WorldCreation.Instance[14];
			return worldCreationItem.InfluenceFactors[(int)lootYield];
		}

		// Token: 0x060063A0 RID: 25504 RVA: 0x00388ADC File Offset: 0x00386CDC
		private unsafe void CalcLootItem(DataContext context)
		{
			int combatOdds = (int)this.CombatConfig.LootItemRate;
			combatOdds = combatOdds * (int)CombatDomain.GetWorldLootRatePercent() / 100;
			sbyte combatType = this._combatType;
			bool flag = combatType == 0 || combatType == 3;
			bool flag2 = flag || !this.CombatConfig.AllowDropItem || this._combatStatus != CombatStatusType.EnemyFail || (combatOdds <= 0 && !this.CombatConfig.LootAllInventory) || this._isPuppetCombat;
			if (!flag2)
			{
				GameData.Domains.Character.Character charObj = this._selfChar.GetCharacter();
				bool flag3 = !this.CombatConfig.LootAllInventory;
				if (flag3)
				{
					ItemKey[] equips = charObj.GetEquipment();
					Personalities personalities = charObj.GetPersonalities();
					ItemKey carrierKey = equips[11];
					int equipOdds = (int)(100 + *(ref personalities.Items.FixedElementField + 5));
					bool flag4 = carrierKey.IsValid();
					if (flag4)
					{
						equipOdds += (int)DomainManager.Item.GetElement_Carriers(carrierKey.Id).GetDropRateBonus();
					}
					for (sbyte slot = 8; slot <= 10; slot += 1)
					{
						ItemKey accessoryKey = equips[(int)slot];
						bool flag5 = accessoryKey.IsValid();
						if (flag5)
						{
							equipOdds += (int)Config.Accessory.Instance[accessoryKey.TemplateId].DropRateBonus;
						}
					}
					List<ItemKey> dropItemRandomPool = ObjectPool<List<ItemKey>>.Instance.Get();
					List<int> dropItemCount = ObjectPool<List<int>>.Instance.Get();
					for (int i = 0; i < this._enemyTeam.Length; i++)
					{
						int enemyId = this._enemyTeam[i];
						bool flag6 = enemyId < 0 || this._lootCharList.Contains(enemyId);
						if (!flag6)
						{
							GameData.Domains.Character.Character enemyChar = this._combatCharacterDict[enemyId].GetCharacter();
							int charOdds = (i == 0) ? enemyChar.Template.DropRatePercentAsMainChar : enemyChar.Template.DropRatePercentAsTeammate;
							ItemKey[] enemyEquips = enemyChar.GetEquipment();
							Inventory enemyInventory = enemyChar.GetInventory();
							dropItemRandomPool.Clear();
							dropItemCount.Clear();
							for (sbyte slot2 = 0; slot2 < 12; slot2 += 1)
							{
								bool flag7 = slot2 == 4;
								if (!flag7)
								{
									ItemKey equipKey = enemyEquips[(int)slot2];
									bool flag8 = !equipKey.IsValid() || !ItemTemplateHelper.IsTransferable(equipKey.ItemType, equipKey.TemplateId);
									if (!flag8)
									{
										int dropRate = (int)ItemTemplateHelper.GetDropRate(equipKey.ItemType, equipKey.TemplateId) * equipOdds / 100 * combatOdds / 100 * charOdds / 100;
										bool flag9 = !context.Random.CheckPercentProb(dropRate);
										if (!flag9)
										{
											dropItemRandomPool.Add(equipKey);
											dropItemCount.Add(1);
										}
									}
								}
							}
							foreach (KeyValuePair<ItemKey, int> itemEntry in enemyInventory.Items)
							{
								ItemKey key2 = itemEntry.Key;
								bool flag10 = !ItemTemplateHelper.IsTransferable(key2.ItemType, key2.TemplateId);
								if (!flag10)
								{
									int dropRate2 = (int)ItemTemplateHelper.GetDropRate(key2.ItemType, key2.TemplateId) * equipOdds / 100 * combatOdds / 100 * charOdds / 100;
									bool flag11 = context.Random.CheckPercentProb(dropRate2);
									if (flag11)
									{
										dropItemRandomPool.Add(key2);
										dropItemCount.Add(itemEntry.Value);
									}
								}
							}
							int randomCount = Math.Min(dropItemRandomPool.Count, (i == 0) ? 3 : 1);
							for (int j = 0; j < randomCount; j++)
							{
								bool flag12 = !context.Random.CheckPercentProb(100 - 20 * j);
								if (!flag12)
								{
									int index = context.Random.Next(dropItemRandomPool.Count);
									ItemKey key = dropItemRandomPool[index];
									int count = dropItemCount[index];
									sbyte slotIndex = (sbyte)enemyEquips.IndexOf(key);
									dropItemRandomPool.RemoveAt(index);
									dropItemCount.RemoveAt(index);
									bool flag13 = slotIndex >= 0;
									if (flag13)
									{
										enemyChar.ChangeEquipment(context, slotIndex, -1, ItemKey.Invalid);
									}
									int existItemIndex = this._combatResultData.ItemList.FindIndex((ItemDisplayData data) => data.Key.Equals(key));
									bool flag14 = existItemIndex < 0;
									if (flag14)
									{
										ItemDisplayData itemData = DomainManager.Item.GetItemDisplayData(key, this._selfCharId);
										itemData.Amount = count;
										this._combatResultData.ItemList.Add(itemData);
										this._combatResultData.ItemSrcCharDict[key] = enemyChar.GetId();
									}
									else
									{
										this._combatResultData.ItemList[existItemIndex].Amount += count;
										DomainManager.Character.TransferInventoryItem(context, enemyChar, DomainManager.Character.GetElement_Objects(this._combatResultData.ItemSrcCharDict[key]), key, count);
									}
								}
							}
						}
					}
					ObjectPool<List<ItemKey>>.Instance.Return(dropItemRandomPool);
					ObjectPool<List<int>>.Instance.Return(dropItemCount);
				}
				else
				{
					GameData.Domains.Character.Character enemyChar2 = this._combatCharacterDict[this._enemyTeam[0]].GetCharacter();
					Inventory enemyInventory2 = enemyChar2.GetInventory();
					foreach (KeyValuePair<ItemKey, int> itemEntry2 in enemyInventory2.Items)
					{
						ItemKey key = itemEntry2.Key;
						int existItemIndex2 = this._combatResultData.ItemList.FindIndex((ItemDisplayData data) => data.Key.Equals(key));
						bool flag15 = existItemIndex2 < 0;
						if (flag15)
						{
							ItemDisplayData itemData2 = DomainManager.Item.GetItemDisplayData(key, this._selfCharId);
							itemData2.Amount = itemEntry2.Value;
							this._combatResultData.ItemList.Add(itemData2);
							this._combatResultData.ItemSrcCharDict[key] = enemyChar2.GetId();
						}
						else
						{
							this._combatResultData.ItemList[existItemIndex2].Amount += itemEntry2.Value;
							DomainManager.Character.TransferInventoryItem(context, enemyChar2, DomainManager.Character.GetElement_Objects(this._combatResultData.ItemSrcCharDict[key]), key, itemEntry2.Value);
						}
					}
				}
			}
		}

		// Token: 0x060063A1 RID: 25505 RVA: 0x003891B4 File Offset: 0x003873B4
		private void GetLootCharDisplayData()
		{
			this._combatResultData.CharList = DomainManager.Character.GetCharacterDisplayDataList(this._lootCharList);
		}

		// Token: 0x060063A2 RID: 25506 RVA: 0x003891D2 File Offset: 0x003873D2
		private void CalcSnapshotAfterCombat(DataContext context)
		{
			this._combatResultData.SnapshotAfterCombat = CombatResultHelper.CreateSnapshot();
		}

		// Token: 0x060063A3 RID: 25507 RVA: 0x003891E8 File Offset: 0x003873E8
		private static int SumAddValue(IEnumerable<int> addValues)
		{
			int sum = 0;
			bool first = true;
			foreach (int addValue in addValues)
			{
				int value = addValue;
				bool flag = first;
				if (flag)
				{
					first = false;
				}
				else
				{
					value = value * GlobalConfig.Instance.CombatGetNonMainPercent / 100;
				}
				sum += value;
			}
			return sum;
		}

		// Token: 0x060063A4 RID: 25508 RVA: 0x00389260 File Offset: 0x00387460
		public static int CalcAddBase(IEnumerable<int> charIds, IReadOnlyList<short> baseValues, bool useTemplate = false)
		{
			return CombatDomain.SumAddValue(from charId in charIds
			select CombatDomain.CalcAddBase(charId, baseValues, useTemplate));
		}

		// Token: 0x060063A5 RID: 25509 RVA: 0x003892A0 File Offset: 0x003874A0
		private static int CalcAddBase(int charId, IReadOnlyList<short> baseValues, bool useTemplate)
		{
			bool flag = charId < 0;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				int index;
				if (useTemplate)
				{
					CharacterItem characterCfg = Config.Character.Instance[charId];
					bool flag2 = characterCfg.RandomEnemyId < 0;
					if (flag2)
					{
						return 0;
					}
					index = (int)Config.Character.Instance[charId].ConsummateLevel;
				}
				else
				{
					GameData.Domains.Character.Character character = DomainManager.Character.GetElement_Objects(charId);
					index = (int)character.GetConsummateLevel();
				}
				index = Math.Clamp(index, 0, baseValues.Count - 1);
				result = (int)baseValues[index];
			}
			return result;
		}

		// Token: 0x060063A6 RID: 25510 RVA: 0x00389328 File Offset: 0x00387528
		private static int CalcAddResource(IEnumerable<int> charIds, sbyte resourceType, bool useTemplate = false)
		{
			return CombatDomain.SumAddValue(from charId in charIds
			select CombatDomain.CalcAddResource(charId, resourceType, useTemplate));
		}

		// Token: 0x060063A7 RID: 25511 RVA: 0x00389368 File Offset: 0x00387568
		private unsafe static int CalcAddResource(int charId, sbyte resourceType, bool useTemplate)
		{
			bool flag = charId < 0;
			int result;
			if (flag)
			{
				result = 0;
			}
			else if (useTemplate)
			{
				result = *Config.Character.Instance[charId].DropResources[(int)resourceType];
			}
			else
			{
				GameData.Domains.Character.Character character = DomainManager.Character.GetElement_Objects(charId);
				CharacterItem characterCfg = Config.Character.Instance[character.GetTemplateId()];
				bool flag2 = characterCfg.CreatingType != 1;
				if (flag2)
				{
					result = *characterCfg.DropResources[(int)resourceType];
				}
				else
				{
					result = *OrganizationDomain.GetOrgMemberConfig(character.GetOrganizationInfo()).DropResources[(int)resourceType];
				}
			}
			return result;
		}

		// Token: 0x060063A8 RID: 25512 RVA: 0x0038940C File Offset: 0x0038760C
		public void WipeOut(DataContext context, List<short> enemyList)
		{
			ValueTuple<WipeOutType, short> wipeOutType = this.GetWipeOutType(enemyList);
			WipeOutType type = wipeOutType.Item1;
			short enemyId = wipeOutType.Item2;
			InstantNotificationCollection instantNotifications = DomainManager.World.GetInstantNotifications();
			switch (type)
			{
			case WipeOutType.Heretic:
				instantNotifications.AddExpelEnemy(enemyId);
				break;
			case WipeOutType.Righteous:
				instantNotifications.AddExpelRighteous(enemyId);
				DomainManager.Taiwu.GetTaiwu().RecordFameAction(context, 60, -1, 1, true);
				instantNotifications.AddFameDecreased(DomainManager.Taiwu.GetTaiwuCharId());
				break;
			case WipeOutType.Xiangshu:
				instantNotifications.AddExpelXiangshuMinion(enemyId);
				break;
			case WipeOutType.Beast:
				instantNotifications.AddExpelBeast(enemyId);
				break;
			}
			this.GetExpAndAuthorityAndAreaSpiritualDebtOutOfCombat(context, enemyList, 1);
		}

		// Token: 0x060063A9 RID: 25513 RVA: 0x003894B0 File Offset: 0x003876B0
		public bool CanWipeOut(short templateId)
		{
			bool flag = templateId < 0;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
				WipeOutType type;
				bool flag2 = this.TryGetWipeOutType(templateId, out type);
				if (flag2)
				{
					bool flag3 = type == WipeOutType.Righteous && taiwu.GetFameType() > 1;
					if (flag3)
					{
						return false;
					}
					List<int> animalIds;
					bool flag4 = type == WipeOutType.Beast && DomainManager.Extra.TryGetAnimalIdsByLocation(taiwu.GetLocation(), out animalIds);
					if (flag4)
					{
						foreach (int animalId in animalIds)
						{
							GameData.Domains.Character.Animal animal;
							bool flag5 = DomainManager.Extra.TryGetAnimal(animalId, out animal) && animal.CharacterTemplateId == templateId && animal.ItemKey.IsValid();
							if (flag5)
							{
								return false;
							}
						}
					}
				}
				result = ((int)taiwu.GetConsummateLevel() >= (int)Config.Character.Instance[templateId].ConsummateLevel + GlobalConfig.Instance.RandomEnemyEscapeConsummateLevelGap);
			}
			return result;
		}

		// Token: 0x060063AA RID: 25514 RVA: 0x003895C4 File Offset: 0x003877C4
		public bool TryGetWipeOutType(short templateId, out WipeOutType type)
		{
			type = WipeOutType.Invalid;
			bool flag = templateId < 0;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				sbyte orgTemplateId = Config.Character.Instance[templateId].OrganizationInfo.OrgTemplateId;
				sbyte b = orgTemplateId;
				switch (b)
				{
				case 17:
					type = WipeOutType.Heretic;
					result = true;
					break;
				case 18:
					type = WipeOutType.Righteous;
					result = true;
					break;
				case 19:
					type = WipeOutType.Xiangshu;
					result = true;
					break;
				default:
					if (b != 40)
					{
						result = false;
					}
					else
					{
						type = WipeOutType.Beast;
						result = true;
					}
					break;
				}
			}
			return result;
		}

		// Token: 0x060063AB RID: 25515 RVA: 0x00389638 File Offset: 0x00387838
		private ValueTuple<WipeOutType, short> GetWipeOutType(List<short> enemyList)
		{
			foreach (short templateId in enemyList)
			{
				WipeOutType type;
				bool flag = this.TryGetWipeOutType(templateId, out type);
				if (flag)
				{
					return new ValueTuple<WipeOutType, short>(type, templateId);
				}
			}
			return new ValueTuple<WipeOutType, short>(WipeOutType.Invalid, -1);
		}

		// Token: 0x060063AC RID: 25516 RVA: 0x003896A8 File Offset: 0x003878A8
		private int GetAddExp(int[] enemyList)
		{
			int exp = 0;
			foreach (int charId in enemyList)
			{
				bool flag = charId < 0;
				if (!flag)
				{
					int expIndex = Math.Clamp((int)DomainManager.Character.GetElement_Objects(charId).GetConsummateLevel(), 0, GlobalConfig.Instance.CombatGetExpBase.Length - 1);
					exp += (int)GlobalConfig.Instance.CombatGetExpBase[expIndex];
				}
			}
			return exp;
		}

		// Token: 0x060063AD RID: 25517 RVA: 0x0038971C File Offset: 0x0038791C
		private int GetAddExp(List<short> enemyList)
		{
			int exp = 0;
			for (int i = 0; i < enemyList.Count; i++)
			{
				int charTemplateId = (int)enemyList[i];
				bool flag = charTemplateId < 0;
				if (!flag)
				{
					CharacterItem characterCfg = Config.Character.Instance[charTemplateId];
					short randomEnemyId = characterCfg.RandomEnemyId;
					bool flag2 = randomEnemyId < 0;
					if (!flag2)
					{
						int expIndex = Math.Clamp((int)characterCfg.ConsummateLevel, 0, GlobalConfig.Instance.CombatGetExpBase.Length - 1);
						exp += (int)GlobalConfig.Instance.CombatGetExpBase[expIndex];
					}
				}
			}
			return exp;
		}

		// Token: 0x060063AE RID: 25518 RVA: 0x003897B0 File Offset: 0x003879B0
		private int GetAddAuthority(int[] enemyList)
		{
			int authority = 0;
			foreach (int charId in enemyList)
			{
				bool flag = charId < 0;
				if (!flag)
				{
					int authorityIndex = Math.Clamp((int)DomainManager.Character.GetElement_Objects(charId).GetConsummateLevel(), 0, GlobalConfig.Instance.CombatGetAuthorityBase.Length - 1);
					authority += (int)GlobalConfig.Instance.CombatGetAuthorityBase[authorityIndex];
				}
			}
			return authority;
		}

		// Token: 0x060063AF RID: 25519 RVA: 0x00389824 File Offset: 0x00387A24
		private int GetAddAuthority(List<short> enemyList)
		{
			int authority = 0;
			for (int i = 0; i < enemyList.Count; i++)
			{
				int charTemplateId = (int)enemyList[i];
				bool flag = charTemplateId < 0;
				if (!flag)
				{
					CharacterItem characterCfg = Config.Character.Instance[charTemplateId];
					short randomEnemyId = characterCfg.RandomEnemyId;
					bool flag2 = randomEnemyId < 0;
					if (!flag2)
					{
						int authorityIndex = Math.Clamp((int)characterCfg.ConsummateLevel, 0, GlobalConfig.Instance.CombatGetAuthorityBase.Length - 1);
						authority += (int)GlobalConfig.Instance.CombatGetAuthorityBase[authorityIndex];
					}
				}
			}
			return authority;
		}

		// Token: 0x060063B0 RID: 25520 RVA: 0x003898B8 File Offset: 0x00387AB8
		private int GetAddAreaSpiritualDebt(int[] enemyList)
		{
			int areaSpiritualDebt = 0;
			foreach (int charId in enemyList)
			{
				bool flag = charId < 0 || this.IsGuardChar(this._combatCharacterDict[charId]);
				if (!flag)
				{
					short randomEnemyId = Config.Character.Instance[DomainManager.Character.GetElement_Objects(charId).GetTemplateId()].RandomEnemyId;
					bool flag2 = randomEnemyId < 0;
					if (!flag2)
					{
						areaSpiritualDebt += (int)RandomEnemy.Instance[randomEnemyId].SpiritualDebt;
					}
				}
			}
			return areaSpiritualDebt;
		}

		// Token: 0x060063B1 RID: 25521 RVA: 0x0038994C File Offset: 0x00387B4C
		private int GetAddAreaSpiritualDebt(List<short> enemyList)
		{
			int areaSpiritualDebt = 0;
			for (int i = 0; i < enemyList.Count; i++)
			{
				int charTemplateId = (int)enemyList[i];
				bool flag = charTemplateId < 0;
				if (!flag)
				{
					short randomEnemyId = Config.Character.Instance[charTemplateId].RandomEnemyId;
					bool flag2 = randomEnemyId < 0;
					if (!flag2)
					{
						areaSpiritualDebt += (int)RandomEnemy.Instance[randomEnemyId].SpiritualDebt;
					}
				}
			}
			return areaSpiritualDebt;
		}

		// Token: 0x060063B2 RID: 25522 RVA: 0x003899C0 File Offset: 0x00387BC0
		public void GetExpAndAuthorityAndAreaSpiritualDebtOutOfCombat(DataContext context, List<int> enemyIdList)
		{
			GameData.Domains.Character.Character taiwuChar = DomainManager.Taiwu.GetTaiwu();
			Location location = taiwuChar.GetLocation();
			bool flag = !location.IsValid();
			if (flag)
			{
				location = taiwuChar.GetValidLocation();
			}
			short areaId = location.AreaId;
			int[] enemyList = enemyIdList.ToArray();
			taiwuChar.ChangeExp(context, this.GetAddExp(enemyList));
			taiwuChar.ChangeResource(context, 7, this.GetAddAuthority(enemyList));
			DomainManager.Extra.ChangeAreaSpiritualDebt(context, areaId, this.GetAddAreaSpiritualDebt(enemyList), true, true);
		}

		// Token: 0x060063B3 RID: 25523 RVA: 0x00389A3C File Offset: 0x00387C3C
		public void GetExpAndAuthorityAndAreaSpiritualDebtOutOfCombat(DataContext context, List<short> enemyTemplateIdList, int rewardTimes = 1)
		{
			GameData.Domains.Character.Character taiwuChar = DomainManager.Taiwu.GetTaiwu();
			Location location = taiwuChar.GetLocation();
			bool flag = !location.IsValid();
			if (flag)
			{
				location = taiwuChar.GetValidLocation();
			}
			short areaId = location.AreaId;
			int expAdd = this.GetAddExp(enemyTemplateIdList) * rewardTimes;
			int resourceAdd = this.GetAddAuthority(enemyTemplateIdList) * rewardTimes;
			int spiritualDebtDelta = this.GetAddAreaSpiritualDebt(enemyTemplateIdList) * rewardTimes;
			InstantNotificationCollection instantNotifications = DomainManager.World.GetInstantNotifications();
			taiwuChar.ChangeExp(context, expAdd);
			taiwuChar.ChangeResource(context, 7, resourceAdd);
			DomainManager.Extra.ChangeAreaSpiritualDebt(context, areaId, spiritualDebtDelta, true, false);
			bool flag2 = expAdd > 0;
			if (flag2)
			{
				instantNotifications.AddExpIncreased(taiwuChar.GetId(), expAdd);
			}
			bool flag3 = resourceAdd > 0;
			if (flag3)
			{
				instantNotifications.AddResourceIncreased(taiwuChar.GetId(), 7, resourceAdd);
			}
			bool flag4 = spiritualDebtDelta > 0;
			if (flag4)
			{
				instantNotifications.AddGraceUp(new Location(areaId, -1), spiritualDebtDelta);
			}
			else
			{
				bool flag5 = spiritualDebtDelta < 0;
				if (flag5)
				{
					instantNotifications.AddGraceDown(new Location(areaId, -1), -spiritualDebtDelta);
				}
			}
		}

		// Token: 0x060063B4 RID: 25524 RVA: 0x00389B3C File Offset: 0x00387D3C
		private static bool FailChecker()
		{
			return DomainManager.Combat.GetCombatStatus() == CombatStatusType.SelfFail;
		}

		// Token: 0x060063B5 RID: 25525 RVA: 0x00389B60 File Offset: 0x00387D60
		private static bool DrawChecker()
		{
			return false;
		}

		// Token: 0x060063B6 RID: 25526 RVA: 0x00389B74 File Offset: 0x00387D74
		private static bool FleeChecker()
		{
			return DomainManager.Combat.GetCombatStatus() == CombatStatusType.SelfFlee;
		}

		// Token: 0x060063B7 RID: 25527 RVA: 0x00389B98 File Offset: 0x00387D98
		private static bool WinChecker()
		{
			return DomainManager.Combat.IsWin(true);
		}

		// Token: 0x060063B8 RID: 25528 RVA: 0x00389BB8 File Offset: 0x00387DB8
		private static bool FightSameLevelChecker()
		{
			CombatDomain domain = DomainManager.Combat;
			return domain._selfChar.GetCharacter().GetConsummateLevel() <= domain._enemyChar.GetCharacter().GetConsummateLevel();
		}

		// Token: 0x060063B9 RID: 25529 RVA: 0x00389BF8 File Offset: 0x00387DF8
		private static bool FightLessLevelChecker()
		{
			CombatDomain domain = DomainManager.Combat;
			return domain._selfChar.GetCharacter().GetConsummateLevel() > domain._enemyChar.GetCharacter().GetConsummateLevel();
		}

		// Token: 0x060063BA RID: 25530 RVA: 0x00389C34 File Offset: 0x00387E34
		private static bool BeatXiangShuChecker()
		{
			BossItem bossConfig = DomainManager.Combat.GetMainCharacter(false).BossConfig;
			sbyte? b = (bossConfig != null) ? new sbyte?(bossConfig.TemplateId) : null;
			int? num = (b != null) ? new int?((int)b.GetValueOrDefault()) : null;
			int num2 = 9;
			return (num.GetValueOrDefault() < num2 & num != null) && DomainManager.Combat.CombatConfig.Scene >= 0 && !DomainManager.Combat._isPuppetCombat;
		}

		// Token: 0x060063BB RID: 25531 RVA: 0x00389CD0 File Offset: 0x00387ED0
		private static bool WinLessChecker()
		{
			int[] selfTeam = DomainManager.Combat.GetCharacterList(true);
			int[] enemyTeam = DomainManager.Combat.GetCharacterList(false);
			return selfTeam.FindAll((int id) => id >= 0).Count > enemyTeam.FindAll((int id) => id >= 0).Count;
		}

		// Token: 0x060063BC RID: 25532 RVA: 0x00389D50 File Offset: 0x00387F50
		private static bool WinMoreChecker()
		{
			int[] selfTeam = DomainManager.Combat.GetCharacterList(true);
			int[] enemyTeam = DomainManager.Combat.GetCharacterList(false);
			return selfTeam.FindAll((int id) => id >= 0).Count < enemyTeam.FindAll((int id) => id >= 0).Count;
		}

		// Token: 0x060063BD RID: 25533 RVA: 0x00389DD0 File Offset: 0x00387FD0
		private static bool WinChildChecker()
		{
			short selfAge = DomainManager.Combat.GetMainCharacter(true).GetCharacter().GetCurrAge();
			short enemyAge = DomainManager.Combat.GetMainCharacter(false).GetCharacter().GetCurrAge();
			return enemyAge < 16 && selfAge > enemyAge;
		}

		// Token: 0x060063BE RID: 25534 RVA: 0x00389E1C File Offset: 0x0038801C
		private static bool WinOlderChecker()
		{
			short selfAge = DomainManager.Combat.GetMainCharacter(true).GetCharacter().GetCurrAge();
			short enemyAge = DomainManager.Combat.GetMainCharacter(false).GetCharacter().GetCurrAge();
			return selfAge < 16 && selfAge < enemyAge;
		}

		// Token: 0x060063BF RID: 25535 RVA: 0x00389E68 File Offset: 0x00388068
		private static bool WinWorseEquipChecker()
		{
			return DomainManager.Combat.SelfAvgEquipGrade - DomainManager.Combat.EnemyAvgEquipGrade >= 3f;
		}

		// Token: 0x060063C0 RID: 25536 RVA: 0x00389E9C File Offset: 0x0038809C
		private static bool WinBetterEquipChecker()
		{
			return DomainManager.Combat.EnemyAvgEquipGrade - DomainManager.Combat.SelfAvgEquipGrade >= 3f;
		}

		// Token: 0x060063C1 RID: 25537 RVA: 0x00389ED0 File Offset: 0x003880D0
		private static bool WinLessNeiliChecker()
		{
			int selfNeiliAllocation = DomainManager.Combat.GetMaxOriginNeiliAllocationSum(true);
			int enemyNeiliAllocation = DomainManager.Combat.GetMaxOriginNeiliAllocationSum(false);
			return selfNeiliAllocation >= enemyNeiliAllocation * CombatDomain.WinNeiliMinDelta;
		}

		// Token: 0x060063C2 RID: 25538 RVA: 0x00389F0C File Offset: 0x0038810C
		private static bool WinMoreNeiliChecker()
		{
			int selfNeiliAllocation = DomainManager.Combat.GetMaxOriginNeiliAllocationSum(true);
			int enemyNeiliAllocation = DomainManager.Combat.GetMaxOriginNeiliAllocationSum(false);
			return enemyNeiliAllocation >= selfNeiliAllocation * CombatDomain.WinNeiliMinDelta;
		}

		// Token: 0x060063C3 RID: 25539 RVA: 0x00389F48 File Offset: 0x00388148
		private static bool WinWorseSkillChecker()
		{
			return DomainManager.Combat.SelfMaxSkillGrade > DomainManager.Combat.EnemyMaxSkillGrade;
		}

		// Token: 0x060063C4 RID: 25540 RVA: 0x00389F70 File Offset: 0x00388170
		private static bool WinBetterSkillChecker()
		{
			return DomainManager.Combat.SelfMaxSkillGrade < DomainManager.Combat.EnemyMaxSkillGrade;
		}

		// Token: 0x060063C5 RID: 25541 RVA: 0x00389F98 File Offset: 0x00388198
		private static bool WinLessConsummateChecker()
		{
			return DomainManager.Combat.GetMainCharacter(true).GetCharacter().GetConsummateLevel() - DomainManager.Combat.GetMainCharacter(false).GetCharacter().GetConsummateLevel() >= 3;
		}

		// Token: 0x060063C6 RID: 25542 RVA: 0x00389FDC File Offset: 0x003881DC
		private static bool WinMoreConsummateChecker()
		{
			return DomainManager.Combat.GetMainCharacter(false).GetCharacter().GetConsummateLevel() - DomainManager.Combat.GetMainCharacter(true).GetCharacter().GetConsummateLevel() >= 3;
		}

		// Token: 0x060063C7 RID: 25543 RVA: 0x0038A020 File Offset: 0x00388220
		private static bool WinPregnantChecker()
		{
			return DomainManager.Combat.GetMainCharacter(false).GetCharacter().GetFeatureIds().Contains(197);
		}

		// Token: 0x060063C8 RID: 25544 RVA: 0x0038A054 File Offset: 0x00388254
		private static bool WinInPregnantChecker()
		{
			return DomainManager.Combat.GetMainCharacter(true).GetCharacter().GetFeatureIds().Contains(197);
		}

		// Token: 0x060063C9 RID: 25545 RVA: 0x0038A088 File Offset: 0x00388288
		private static bool KillBad0Checker()
		{
			CombatCharacter selfChar = DomainManager.Combat.GetMainCharacter(true);
			CombatCharacter enemyChar = DomainManager.Combat.GetMainCharacter(false);
			return !DomainManager.Combat.IsGuardChar(enemyChar) && enemyChar.GetCharacter().GetOrganizationInfo().OrgTemplateId == 17 && selfChar.GetCharacter().GetConsummateLevel() > enemyChar.GetCharacter().GetConsummateLevel();
		}

		// Token: 0x060063CA RID: 25546 RVA: 0x0038A0F0 File Offset: 0x003882F0
		private static bool KillBad1Checker()
		{
			CombatCharacter selfChar = DomainManager.Combat.GetMainCharacter(true);
			CombatCharacter enemyChar = DomainManager.Combat.GetMainCharacter(false);
			return !DomainManager.Combat.IsGuardChar(enemyChar) && enemyChar.GetCharacter().GetOrganizationInfo().OrgTemplateId == 17 && selfChar.GetCharacter().GetConsummateLevel() <= enemyChar.GetCharacter().GetConsummateLevel();
		}

		// Token: 0x060063CB RID: 25547 RVA: 0x0038A15C File Offset: 0x0038835C
		private static bool KillGood0Checker()
		{
			CombatCharacter selfChar = DomainManager.Combat.GetMainCharacter(true);
			CombatCharacter enemyChar = DomainManager.Combat.GetMainCharacter(false);
			return !DomainManager.Combat.IsGuardChar(enemyChar) && enemyChar.GetCharacter().GetOrganizationInfo().OrgTemplateId == 18 && selfChar.GetCharacter().GetConsummateLevel() > enemyChar.GetCharacter().GetConsummateLevel();
		}

		// Token: 0x060063CC RID: 25548 RVA: 0x0038A1C4 File Offset: 0x003883C4
		private static bool KillGood1Checker()
		{
			CombatCharacter selfChar = DomainManager.Combat.GetMainCharacter(true);
			CombatCharacter enemyChar = DomainManager.Combat.GetMainCharacter(false);
			return !DomainManager.Combat.IsGuardChar(enemyChar) && enemyChar.GetCharacter().GetOrganizationInfo().OrgTemplateId == 18 && selfChar.GetCharacter().GetConsummateLevel() <= enemyChar.GetCharacter().GetConsummateLevel();
		}

		// Token: 0x060063CD RID: 25549 RVA: 0x0038A230 File Offset: 0x00388430
		private static bool KillMinion0Checker()
		{
			CombatCharacter selfChar = DomainManager.Combat.GetMainCharacter(true);
			CombatCharacter enemyChar = DomainManager.Combat.GetMainCharacter(false);
			return !DomainManager.Combat.IsGuardChar(enemyChar) && enemyChar.GetCharacter().GetOrganizationInfo().OrgTemplateId == 19 && selfChar.GetCharacter().GetConsummateLevel() > enemyChar.GetCharacter().GetConsummateLevel();
		}

		// Token: 0x060063CE RID: 25550 RVA: 0x0038A298 File Offset: 0x00388498
		private static bool KillMinion1Checker()
		{
			CombatCharacter selfChar = DomainManager.Combat.GetMainCharacter(true);
			CombatCharacter enemyChar = DomainManager.Combat.GetMainCharacter(false);
			return !DomainManager.Combat.IsGuardChar(enemyChar) && enemyChar.GetCharacter().GetOrganizationInfo().OrgTemplateId == 19 && selfChar.GetCharacter().GetConsummateLevel() <= enemyChar.GetCharacter().GetConsummateLevel();
		}

		// Token: 0x060063CF RID: 25551 RVA: 0x0038A304 File Offset: 0x00388504
		private static bool ShixiangBuff0Checker()
		{
			return DomainManager.Combat.GetMainCharacter(true).GetCharacter().GetFeatureIds().Contains(243);
		}

		// Token: 0x060063D0 RID: 25552 RVA: 0x0038A338 File Offset: 0x00388538
		private static bool ShixiangBuff1Checker()
		{
			return DomainManager.Combat.GetMainCharacter(true).GetCharacter().GetFeatureIds().Contains(244);
		}

		// Token: 0x060063D1 RID: 25553 RVA: 0x0038A36C File Offset: 0x0038856C
		private static bool ShixiangBuff2Checker()
		{
			return DomainManager.Combat.GetMainCharacter(true).GetCharacter().GetFeatureIds().Contains(245);
		}

		// Token: 0x060063D2 RID: 25554 RVA: 0x0038A3A0 File Offset: 0x003885A0
		private static bool PuppetCombatChecker()
		{
			return DomainManager.Combat._isPuppetCombat;
		}

		// Token: 0x060063D3 RID: 25555 RVA: 0x0038A3BC File Offset: 0x003885BC
		private static bool OutBossCombatChecker()
		{
			return DomainManager.Combat.CombatConfig.IsOutBoss;
		}

		// Token: 0x060063D4 RID: 25556 RVA: 0x0038A3E0 File Offset: 0x003885E0
		private static bool WinLoongChecker()
		{
			short templateId = DomainManager.Combat.CombatConfig.TemplateId;
			return templateId - 182 <= 4;
		}

		// Token: 0x060063D5 RID: 25557 RVA: 0x0038A418 File Offset: 0x00388618
		private static bool CombatHardChecker()
		{
			return DomainManager.World.GetCombatDifficulty() == 2;
		}

		// Token: 0x060063D6 RID: 25558 RVA: 0x0038A438 File Offset: 0x00388638
		private static bool CombatVeryHardChecker()
		{
			return DomainManager.World.GetCombatDifficulty() == 3;
		}

		// Token: 0x060063D7 RID: 25559 RVA: 0x0038A458 File Offset: 0x00388658
		public void AddCombatResultLegacy(short legacy)
		{
			CombatResultDisplayData combatResultData = this._combatResultData;
			if (combatResultData.LegacyTemplateIds == null)
			{
				combatResultData.LegacyTemplateIds = new List<short>();
			}
			this._combatResultData.LegacyTemplateIds.Add(legacy);
		}

		// Token: 0x060063D8 RID: 25560 RVA: 0x0038A492 File Offset: 0x00388692
		private void ClearCombatResultLegacies()
		{
			List<short> legacyTemplateIds = this._combatResultData.LegacyTemplateIds;
			if (legacyTemplateIds != null)
			{
				legacyTemplateIds.Clear();
			}
		}

		// Token: 0x060063D9 RID: 25561 RVA: 0x0038A4AC File Offset: 0x003886AC
		[DomainMethod]
		public void ApplyCombatResultDataEffect(DataContext context, CombatResultDisplayData combatResultData, List<ItemDisplayData> selectedLootItem)
		{
			GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
			taiwu.ChangeExp(context, combatResultData.Exp);
			Events.RaiseEvaluationAddExp(context, combatResultData.Exp);
			for (sbyte i = 0; i < 8; i += 1)
			{
				taiwu.ChangeResource(context, i, combatResultData.Resource.Get((int)i));
			}
			int areaSpiritualDebt = combatResultData.AreaSpiritualDebt;
			bool flag = areaSpiritualDebt != 0;
			if (flag)
			{
				Location location = taiwu.GetLocation();
				bool flag2 = !location.IsValid();
				if (flag2)
				{
					location = taiwu.GetValidLocation();
				}
				DomainManager.Extra.ChangeAreaSpiritualDebt(context, location.AreaId, areaSpiritualDebt, true, true);
			}
			ItemDomain itemDomain = DomainManager.Item;
			HashSet<ItemDisplayData> selected = new HashSet<ItemDisplayData>();
			bool flag3 = selectedLootItem != null;
			if (flag3)
			{
				selected.UnionWith(selectedLootItem);
			}
			using (List<ItemDisplayData>.Enumerator enumerator = combatResultData.ItemList.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ItemDisplayData item = enumerator.Current;
					bool flag4 = selected.Any((ItemDisplayData selectedItem) => selectedItem.ContainsItemKey(item.Key));
					if (flag4)
					{
						taiwu.AddInventoryItem(context, item.Key, item.Amount, false);
					}
					else
					{
						itemDomain.RemoveItem(context, item.Key);
					}
				}
			}
		}

		// Token: 0x060063DA RID: 25562 RVA: 0x0038A620 File Offset: 0x00388820
		internal static void ResultCalcExp(CombatConfigItem combatConfig, bool isPlaygroundCombat, GameData.Domains.Character.Character selfChar, ICollection<int> enemyTeam, CombatResultDisplayData combatResultData)
		{
			bool flag = !combatConfig.DropResource || isPlaygroundCombat;
			if (!flag)
			{
				int exp = CombatDomain.CalcAddBase(enemyTeam, GlobalConfig.Instance.CombatGetExpBase, false);
				int consummateLevel = (int)(selfChar.IsLoseConsummateBonusByFeature() ? 0 : selfChar.GetConsummateLevel());
				int extraAddPercent = ConsummateLevel.Instance[consummateLevel].ExpBonus;
				exp = combatResultData.ModifyValue(exp, (CombatEvaluationItem x) => (int)x.ExpAddPercent, (CombatEvaluationItem x) => (int)x.ExpTotalPercent, extraAddPercent);
				exp = Math.Max(exp, 0);
				combatResultData.Exp += exp;
			}
		}

		// Token: 0x060063DB RID: 25563 RVA: 0x0038A6D8 File Offset: 0x003888D8
		internal unsafe static void ResultCalcResource(CombatConfigItem combatConfig, bool isPlaygroundCombat, GameData.Domains.Character.Character selfChar, ICollection<int> enemyTeam, CombatResultDisplayData combatResultData)
		{
			bool flag = !combatConfig.DropResource || isPlaygroundCombat;
			if (!flag)
			{
				for (sbyte i = 0; i < 8; i += 1)
				{
					int value = CombatDomain.CalcAddResource(enemyTeam, i, false);
					bool flag2 = i == 7;
					if (flag2)
					{
						value += CombatDomain.CalcAddBase(enemyTeam, GlobalConfig.Instance.CombatGetAuthorityBase, false);
					}
					value = value * DomainManager.World.GetGainResourcePercent(8) / 100;
					bool flag3 = i == 7;
					if (flag3)
					{
						value = combatResultData.ModifyValue(value, (CombatEvaluationItem x) => (int)x.AuthorityAddPercent, (CombatEvaluationItem x) => (int)x.AuthorityTotalPercent, 0);
					}
					else
					{
						bool flag4 = !combatResultData.IsWin;
						if (flag4)
						{
							value = 0;
						}
					}
					value = Math.Max(value, 0);
					*combatResultData.Resource[(int)i] += value;
				}
			}
		}

		// Token: 0x060063DC RID: 25564 RVA: 0x0038A7CC File Offset: 0x003889CC
		internal static void ResultCalcAreaSpiritualDebt(bool isWin, GameData.Domains.Character.Character selfChar, int[] enemyTeam, CombatResultDisplayData combatResultData)
		{
			CombatDomain.<>c__DisplayClass285_0 CS$<>8__locals1;
			CS$<>8__locals1.enemyTeam = enemyTeam;
			int areaSpiritualDebt = 0;
			areaSpiritualDebt += combatResultData.SelectEvaluations<short>((CombatEvaluationItem x) => x.AreaSpiritualDebt).Sum((short x) => (int)x);
			if (isWin)
			{
				areaSpiritualDebt += CombatDomain.<ResultCalcAreaSpiritualDebt>g__CalcAddAreaSpiritualDebt|285_2(ref CS$<>8__locals1);
			}
			combatResultData.AreaSpiritualDebt += areaSpiritualDebt;
		}

		// Token: 0x060063DD RID: 25565 RVA: 0x0038A850 File Offset: 0x00388A50
		internal unsafe static void ResultCalcLootItem(IRandomSource random, int lootRatePercentFactor, sbyte combatType, sbyte combatStatus, bool isPuppetCombat, CombatConfigItem combatConfig, GameData.Domains.Character.Character charObj, int[] enemyTeam, ICollection<int> lootCharList, CombatResultDisplayData combatResultData)
		{
			int combatOdds = (int)combatConfig.LootItemRate;
			combatOdds = combatOdds * (int)CombatDomain.GetWorldLootRatePercent() / 100;
			combatOdds = combatOdds * lootRatePercentFactor / 100;
			bool flag = combatType == 0 || combatType == 3;
			bool flag2 = flag || !combatConfig.AllowDropItem || combatStatus != CombatStatusType.EnemyFail || (combatOdds <= 0 && !combatConfig.LootAllInventory) || isPuppetCombat;
			if (!flag2)
			{
				bool flag3 = !combatConfig.LootAllInventory;
				if (flag3)
				{
					ItemKey[] equips = charObj.GetEquipment();
					Personalities personalities = charObj.GetPersonalities();
					ItemKey carrierKey = equips[11];
					int equipOdds = (int)(100 + *(ref personalities.Items.FixedElementField + 5));
					bool flag4 = carrierKey.IsValid();
					if (flag4)
					{
						equipOdds += (int)DomainManager.Item.GetElement_Carriers(carrierKey.Id).GetDropRateBonus();
					}
					for (sbyte slot = 8; slot <= 10; slot += 1)
					{
						ItemKey accessoryKey = equips[(int)slot];
						bool flag5 = accessoryKey.IsValid();
						if (flag5)
						{
							equipOdds += (int)Config.Accessory.Instance[accessoryKey.TemplateId].DropRateBonus;
						}
					}
					List<ItemKey> dropItemRandomPool = ObjectPool<List<ItemKey>>.Instance.Get();
					List<int> dropItemCount = ObjectPool<List<int>>.Instance.Get();
					for (int i = 0; i < enemyTeam.Length; i++)
					{
						int enemyId = enemyTeam[i];
						bool flag6 = enemyId < 0 || lootCharList.Contains(enemyId);
						if (!flag6)
						{
							int charOdds = (i == 0) ? 100 : 25;
							GameData.Domains.Character.Character enemyChar = DomainManager.Character.GetElement_Objects(enemyId);
							ItemKey[] enemyEquips = enemyChar.GetEquipment();
							Inventory enemyInventory = enemyChar.GetInventory();
							dropItemRandomPool.Clear();
							dropItemCount.Clear();
							for (sbyte slot2 = 0; slot2 < 12; slot2 += 1)
							{
								bool flag7 = slot2 == 4;
								if (!flag7)
								{
									ItemKey equipKey = enemyEquips[(int)slot2];
									bool flag8 = !equipKey.IsValid() || !ItemTemplateHelper.IsTransferable(equipKey.ItemType, equipKey.TemplateId);
									if (!flag8)
									{
										int dropRate = (int)ItemTemplateHelper.GetDropRate(equipKey.ItemType, equipKey.TemplateId) * equipOdds / 100 * combatOdds / 100 * charOdds / 100;
										bool flag9 = !random.CheckPercentProb(dropRate);
										if (!flag9)
										{
											dropItemRandomPool.Add(equipKey);
											dropItemCount.Add(1);
										}
									}
								}
							}
							foreach (KeyValuePair<ItemKey, int> itemEntry2 in enemyInventory.Items)
							{
								ItemKey key = itemEntry2.Key;
								bool flag10 = !ItemTemplateHelper.IsTransferable(key.ItemType, key.TemplateId);
								if (!flag10)
								{
									int dropRate2 = (int)ItemTemplateHelper.GetDropRate(key.ItemType, key.TemplateId) * equipOdds / 100 * combatOdds / 100 * charOdds / 100;
									bool flag11 = random.CheckPercentProb(dropRate2);
									if (flag11)
									{
										dropItemRandomPool.Add(key);
										dropItemCount.Add(itemEntry2.Value);
									}
								}
							}
							List<ItemKey> collectedKeys = new List<ItemKey>();
							int randomCount = Math.Min(dropItemRandomPool.Count, (i == 0) ? 3 : 1);
							for (int j = 0; j < randomCount; j++)
							{
								bool flag12 = !random.CheckPercentProb(100 - 20 * j);
								if (!flag12)
								{
									int index = random.Next(dropItemRandomPool.Count);
									ItemKey key2 = dropItemRandomPool[index];
									int count = dropItemCount[index];
									dropItemRandomPool.RemoveAt(index);
									dropItemCount.RemoveAt(index);
									collectedKeys.Add(key2);
								}
							}
							List<ItemDisplayData> collection = DomainManager.Item.GetItemDisplayDataListOptional(collectedKeys, charObj.GetId(), 1, true);
							bool flag13 = collection != null;
							if (flag13)
							{
								combatResultData.ItemList.AddRange(collection);
							}
							foreach (ItemKey key3 in collectedKeys)
							{
								combatResultData.ItemSrcCharDict[key3] = enemyChar.GetId();
							}
						}
					}
					ObjectPool<List<ItemKey>>.Instance.Return(dropItemRandomPool);
					ObjectPool<List<int>>.Instance.Return(dropItemCount);
				}
				else
				{
					GameData.Domains.Character.Character enemyChar2 = DomainManager.Character.GetElement_Objects(enemyTeam[0]);
					Inventory enemyInventory2 = enemyChar2.GetInventory();
					List<ItemKey> collectedKeys2 = (from itemEntry in enemyInventory2.Items
					select itemEntry.Key).ToList<ItemKey>();
					List<ItemDisplayData> collection2 = DomainManager.Item.GetItemDisplayDataListOptional(collectedKeys2, charObj.GetId(), 1, true);
					bool flag14 = collection2 != null;
					if (flag14)
					{
						combatResultData.ItemList.AddRange(collection2);
					}
					foreach (ItemKey key4 in collectedKeys2)
					{
						combatResultData.ItemSrcCharDict[key4] = enemyChar2.GetId();
					}
				}
			}
		}

		// Token: 0x060063DE RID: 25566 RVA: 0x0038AD88 File Offset: 0x00388F88
		public static void RegisterHandler_CombatCharAboutToFall(CombatDomain.OnCombatCharAboutToFall handler)
		{
			CombatDomain._handlersCombatCharAboutToFall = (CombatDomain.OnCombatCharAboutToFall)Delegate.Combine(CombatDomain._handlersCombatCharAboutToFall, handler);
		}

		// Token: 0x060063DF RID: 25567 RVA: 0x0038ADA0 File Offset: 0x00388FA0
		public static void UnRegisterHandler_CombatCharAboutToFall(CombatDomain.OnCombatCharAboutToFall handler)
		{
			CombatDomain._handlersCombatCharAboutToFall = (CombatDomain.OnCombatCharAboutToFall)Delegate.Remove(CombatDomain._handlersCombatCharAboutToFall, handler);
		}

		// Token: 0x060063E0 RID: 25568 RVA: 0x0038ADB8 File Offset: 0x00388FB8
		public static void RaiseCombatCharAboutToFall(DataContext context, CombatCharacter combatChar, int eventIndex)
		{
			CombatDomain.OnCombatCharAboutToFall handlersCombatCharAboutToFall = CombatDomain._handlersCombatCharAboutToFall;
			if (handlersCombatCharAboutToFall != null)
			{
				handlersCombatCharAboutToFall(context, combatChar, eventIndex);
			}
		}

		// Token: 0x060063E1 RID: 25569 RVA: 0x0038ADD0 File Offset: 0x00388FD0
		[DomainMethod]
		public void GmCmd_ForceRecoverBreathAndStance(DataContext context)
		{
			this.ChangeBreathValue(context, this._selfChar, this._selfChar.GetMaxBreathValue(), false, null);
			this.ChangeStanceValue(context, this._selfChar, this._selfChar.GetMaxStanceValue(), false, null);
			this.UpdateSkillCostBreathStanceCanUse(context, this._selfChar);
			this.GmCmd_ForceRecoverMobilityValue(context);
			this.GmCmd_ForceRecoverTeammateCommand(context);
		}

		// Token: 0x060063E2 RID: 25570 RVA: 0x0038AE34 File Offset: 0x00389034
		[DomainMethod]
		public void GmCmd_ForceRecoverTeammateCommand(DataContext context)
		{
			CombatCharacter mainChar = this.GetMainCharacter(true);
			foreach (CombatCharacter teammate in this.GetTeammateCharacters(mainChar.GetId()))
			{
				List<sbyte> cmdList = teammate.GetCurrTeammateCommands();
				for (int i = 0; i < cmdList.Count; i++)
				{
					teammate.ClearTeammateCommandCd(context, i);
				}
			}
		}

		// Token: 0x060063E3 RID: 25571 RVA: 0x0038AEBC File Offset: 0x003890BC
		[DomainMethod]
		public void GmCmd_AddTrick(DataContext context, bool isAlly, sbyte trickType)
		{
			this.AddTrick(context, this.GetCombatCharacter(isAlly, false), trickType, true);
		}

		// Token: 0x060063E4 RID: 25572 RVA: 0x0038AED4 File Offset: 0x003890D4
		[DomainMethod]
		public void GmCmd_AddInjury(DataContext context, bool isAlly, sbyte bodyPart, bool isInner, int count = 1, bool changeToOld = false)
		{
			CombatCharacter combatChar = isAlly ? this._selfChar : this._enemyChar;
			foreach (sbyte i in CRandom.IterBodyPart(bodyPart))
			{
				this.AddInjury(context, combatChar, i, isInner, (sbyte)count, true, changeToOld);
			}
			this.AddToCheckFallenSet(combatChar.GetId());
			bool flag = this.IsCharacterFallen(combatChar);
			if (flag)
			{
				this._skipCombatLoop = true;
				this.GetMainCharacter(isAlly).SkipOnFrameBegin = true;
			}
		}

		// Token: 0x060063E5 RID: 25573 RVA: 0x0038AF70 File Offset: 0x00389170
		[DomainMethod]
		public void GmCmd_ForceHealAllInjury(DataContext context, bool isAlly = true)
		{
			CombatCharacter combatChar = isAlly ? this._selfChar : this._enemyChar;
			Injuries injuries = combatChar.GetInjuries();
			injuries.Initialize();
			this.SetInjuries(context, combatChar, injuries, true, true);
		}

		// Token: 0x060063E6 RID: 25574 RVA: 0x0038AFAC File Offset: 0x003891AC
		[DomainMethod]
		public void GmCmd_HealInjury(DataContext context, bool isAlly, bool isInner)
		{
			CombatCharacter combatChar = isAlly ? this._selfChar : this._enemyChar;
			Injuries injuries = combatChar.GetInjuries();
			for (sbyte i = 0; i < 7; i += 1)
			{
				injuries.Change(i, isInner, -6);
			}
			this.SetInjuries(context, combatChar, injuries, true, true);
		}

		// Token: 0x060063E7 RID: 25575 RVA: 0x0038AFFC File Offset: 0x003891FC
		[DomainMethod]
		public void GmCmd_AddPoison(DataContext context, bool isAlly, sbyte poisonType, int count = 1, bool changeToOld = false)
		{
			CombatCharacter combatChar = isAlly ? this._selfChar : this._enemyChar;
			short[] threshold = GlobalConfig.Instance.PoisonLevelThresholds;
			int addValue = (int)threshold[Math.Clamp(count - 1, 0, threshold.Length - 1)] * (1 + (count - 1) / 3);
			foreach (sbyte i in CRandom.IterPoisonType(poisonType))
			{
				this.AddPoison(context, null, combatChar, i, 3, addValue, -1, false, true, default(ItemKey), false, false, changeToOld);
			}
			this.AddToCheckFallenSet(combatChar.GetId());
			bool flag = this.IsCharacterFallen(combatChar);
			if (flag)
			{
				this._skipCombatLoop = true;
				this.GetMainCharacter(isAlly).SkipOnFrameBegin = true;
			}
		}

		// Token: 0x060063E8 RID: 25576 RVA: 0x0038B0D4 File Offset: 0x003892D4
		[DomainMethod]
		public unsafe void GmCmd_ForceHealAllPoison(DataContext context, bool isAlly = true)
		{
			CombatCharacter combatChar = isAlly ? this._selfChar : this._enemyChar;
			PoisonInts poisons = *combatChar.GetPoison();
			for (int i = 0; i < 6; i++)
			{
				*(ref poisons.Items.FixedElementField + (IntPtr)i * 4) = 0;
			}
			this.SetPoisons(context, combatChar, poisons, true);
		}

		// Token: 0x060063E9 RID: 25577 RVA: 0x0038B130 File Offset: 0x00389330
		[DomainMethod]
		public void GmCmd_ForceEnemyUseSkill(DataContext context, short skillId)
		{
			bool flag = !this._skillDataDict.ContainsKey(new CombatSkillKey(this._enemyChar.GetId(), skillId)) || this._enemyChar.NeedUseSkillId >= 0 || this._enemyChar.GetPreparingSkillId() >= 0;
			if (!flag)
			{
				this.CastSkillFree(context, this._enemyChar, skillId, ECombatCastFreePriority.Gm);
				this._enemyChar.MoveData.ResetJumpState(context, true);
				this.UpdateAllCommandAvailability(context, this._enemyChar);
			}
		}

		// Token: 0x060063EA RID: 25578 RVA: 0x0038B1B8 File Offset: 0x003893B8
		[DomainMethod]
		public void GmCmd_ForceEnemyUseOtherAction(DataContext context, sbyte actionType)
		{
			bool flag = actionType == 0;
			if (flag)
			{
				this._enemyChar.SetHealInjuryCount(this._enemyChar.GetHealInjuryCount() + 1, context);
			}
			else
			{
				bool flag2 = actionType == 1;
				if (flag2)
				{
					this._enemyChar.SetHealPoisonCount(this._enemyChar.GetHealPoisonCount() + 1, context);
				}
			}
			this._enemyChar.SetNeedUseOtherAction(context, actionType);
			this._enemyChar.MoveData.ResetJumpState(context, true);
			this.UpdateAllCommandAvailability(context, this._enemyChar);
		}

		// Token: 0x060063EB RID: 25579 RVA: 0x0038B240 File Offset: 0x00389440
		[DomainMethod]
		public void GmCmd_ForceEnemyDefeat(DataContext context)
		{
			bool flag = !this.IsInCombat();
			if (!flag)
			{
				CombatCharacter enemyChar = this.GetCombatCharacter(false, true);
				bool flag2 = enemyChar.StateMachine.GetCurrentStateType() == CombatCharacterStateType.ChangeBossPhase || enemyChar.NeedChangeBossPhase;
				if (!flag2)
				{
					this.AppendMindDefeatMark(context, enemyChar, (int)GlobalConfig.NeedDefeatMarkCount[2], -1, false);
				}
			}
		}

		// Token: 0x060063EC RID: 25580 RVA: 0x0038B298 File Offset: 0x00389498
		[DomainMethod]
		public void GmCmd_ForceSelfDefeat(DataContext context)
		{
			bool flag = !this.IsInCombat();
			if (!flag)
			{
				CombatCharacter selfChar = (this._selfChar.TeammateBeforeMainChar >= 0) ? this.GetElement_CombatCharacterDict(this._selfChar.TeammateBeforeMainChar) : this._selfChar;
				this.AppendMindDefeatMark(context, selfChar, (int)GlobalConfig.NeedDefeatMarkCount[2], -1, false);
				this.AddToCheckFallenSet(selfChar.GetId());
				bool flag2 = DomainManager.Combat.IsCharacterFallen(selfChar);
				if (flag2)
				{
					this._skipCombatLoop = true;
					this.GetMainCharacter(true).SkipOnFrameBegin = true;
					this.SetTimeScale(1f, context);
				}
			}
		}

		// Token: 0x060063ED RID: 25581 RVA: 0x0038B330 File Offset: 0x00389530
		[DomainMethod]
		public unsafe void GmCmd_SetNeiliAllocation(DataContext context, bool isAlly, short[] neiliAllocation)
		{
			CombatCharacter target = isAlly ? this._selfChar : this._enemyChar;
			NeiliAllocation current = target.GetNeiliAllocation();
			for (byte type = 0; type < 4; type += 1)
			{
				target.ChangeNeiliAllocation(context, type, (int)(neiliAllocation[(int)type] - *(ref current.Items.FixedElementField + (IntPtr)type * 2)), false, true);
			}
		}

		// Token: 0x060063EE RID: 25582 RVA: 0x0038B38C File Offset: 0x0038958C
		[DomainMethod]
		public void GmCmd_AddFlaw(DataContext context, bool isAlly, sbyte bodyPart, int count = 1)
		{
			CombatCharacter combatChar = isAlly ? this._selfChar : this._enemyChar;
			foreach (sbyte i in CRandom.IterBodyPart(bodyPart))
			{
				this.AddFlaw(context, combatChar, 3, new CombatSkillKey(-1, -1), i, count, true);
			}
			bool flag = this.IsCharacterFallen(combatChar);
			if (flag)
			{
				this._skipCombatLoop = true;
				this.GetMainCharacter(isAlly).SkipOnFrameBegin = true;
			}
		}

		// Token: 0x060063EF RID: 25583 RVA: 0x0038B420 File Offset: 0x00389620
		[DomainMethod]
		public void GmCmd_HealAllFlaw(DataContext context, bool isAlly)
		{
			this.RemoveAllFlaw(context, isAlly ? this._selfChar : this._enemyChar);
		}

		// Token: 0x060063F0 RID: 25584 RVA: 0x0038B43C File Offset: 0x0038963C
		[DomainMethod]
		public void GmCmd_AddAcupoint(DataContext context, bool isAlly, sbyte bodyPart, int count = 1)
		{
			CombatCharacter combatChar = isAlly ? this._selfChar : this._enemyChar;
			foreach (sbyte i in CRandom.IterBodyPart(bodyPart))
			{
				this.AddAcupoint(context, combatChar, 3, new CombatSkillKey(-1, -1), i, count, true);
			}
			bool flag = this.IsCharacterFallen(combatChar);
			if (flag)
			{
				this._skipCombatLoop = true;
				this.GetMainCharacter(isAlly).SkipOnFrameBegin = true;
			}
		}

		// Token: 0x060063F1 RID: 25585 RVA: 0x0038B4D0 File Offset: 0x003896D0
		[DomainMethod]
		public void GmCmd_HealAllAcupoint(DataContext context, bool isAlly)
		{
			this.RemoveAllAcupoint(context, isAlly ? this._selfChar : this._enemyChar);
		}

		// Token: 0x060063F2 RID: 25586 RVA: 0x0038B4EC File Offset: 0x003896EC
		[DomainMethod]
		public void GmCmd_AddMind(DataContext context, bool isAlly, int count)
		{
			this.AppendMindDefeatMark(context, isAlly ? this._selfChar : this._enemyChar, count, -1, false);
		}

		// Token: 0x060063F3 RID: 25587 RVA: 0x0038B50B File Offset: 0x0038970B
		[DomainMethod]
		public void GmCmd_HealAllMind(DataContext context, bool isAlly)
		{
			this.RemoveMindDefeatMark(context, isAlly ? this._selfChar : this._enemyChar, int.MaxValue, false, 0);
		}

		// Token: 0x060063F4 RID: 25588 RVA: 0x0038B52E File Offset: 0x0038972E
		[DomainMethod]
		public void GmCmd_AddDie(DataContext context, bool isAlly, int count)
		{
			this.AppendDieDefeatMark(context, isAlly ? this._selfChar : this._enemyChar, new CombatSkillKey(-1, -1), count);
		}

		// Token: 0x060063F5 RID: 25589 RVA: 0x0038B554 File Offset: 0x00389754
		[DomainMethod]
		public void GmCmd_HealAllDie(DataContext context, bool isAlly)
		{
			CombatCharacter combatChar = isAlly ? this._selfChar : this._enemyChar;
			combatChar.GetDefeatMarkCollection().DieMarkList.Clear();
			combatChar.SetDefeatMarkCollection(combatChar.GetDefeatMarkCollection(), context);
		}

		// Token: 0x060063F6 RID: 25590 RVA: 0x0038B594 File Offset: 0x00389794
		[DomainMethod]
		public void GmCmd_AddFatal(DataContext context, bool isAlly, int count)
		{
			CombatCharacter combatChar = isAlly ? this._selfChar : this._enemyChar;
			this.AppendFatalDamageMark(context, combatChar, count, -1, -1, false, EDamageType.None);
		}

		// Token: 0x060063F7 RID: 25591 RVA: 0x0038B5C4 File Offset: 0x003897C4
		[DomainMethod]
		public void GmCmd_HealAllFatal(DataContext context, bool isAlly)
		{
			CombatCharacter combatChar = isAlly ? this._selfChar : this._enemyChar;
			combatChar.GetDefeatMarkCollection().FatalDamageMarkCount = 0;
			combatChar.SetDefeatMarkCollection(combatChar.GetDefeatMarkCollection(), context);
		}

		// Token: 0x060063F8 RID: 25592 RVA: 0x0038B600 File Offset: 0x00389800
		[DomainMethod]
		public void GmCmd_AddAllDefeatMark(DataContext context, bool isAlly, int count = 1)
		{
			for (sbyte bodyPart = 0; bodyPart < 7; bodyPart += 1)
			{
				this.GmCmd_AddInjury(context, isAlly, bodyPart, true, count, false);
				this.GmCmd_AddInjury(context, isAlly, bodyPart, false, count, false);
				this.GmCmd_AddFlaw(context, isAlly, bodyPart, count);
				this.GmCmd_AddAcupoint(context, isAlly, bodyPart, count);
			}
			for (sbyte poisonType = 0; poisonType < 6; poisonType += 1)
			{
				this.GmCmd_AddPoison(context, isAlly, poisonType, count, false);
			}
			this.GmCmd_AddDie(context, isAlly, count);
			this.GmCmd_AddMind(context, isAlly, count);
			this.GmCmd_AddFatal(context, isAlly, count);
		}

		// Token: 0x060063F9 RID: 25593 RVA: 0x0038B690 File Offset: 0x00389890
		[DomainMethod]
		public void GmCmd_HealAllDefeatMark(DataContext context, bool isAlly)
		{
			this.GmCmd_ForceHealAllInjury(context, isAlly);
			this.GmCmd_ForceHealAllPoison(context, isAlly);
			this.GmCmd_HealAllFlaw(context, isAlly);
			this.GmCmd_HealAllAcupoint(context, isAlly);
			this.GmCmd_HealAllDie(context, isAlly);
			this.GmCmd_HealAllMind(context, isAlly);
			this.GmCmd_HealAllFatal(context, isAlly);
		}

		// Token: 0x060063FA RID: 25594 RVA: 0x0038B6E0 File Offset: 0x003898E0
		[DomainMethod]
		public void GmCmd_FightBoss(DataContext context, short charTemplateId)
		{
			bool flag = Config.Character.Instance[charTemplateId].XiangshuType == 3;
			if (flag)
			{
				sbyte xiangshuAvatarId = XiangshuAvatarIds.GetXiangshuAvatarIdByCharacterTemplateId(charTemplateId);
				int charId = DomainManager.Character.CreateJuniorXiangshuCombatImage(context, xiangshuAvatarId);
				GameData.Domains.Character.Character character = DomainManager.Character.GetElement_Objects(charId);
				this.CombatEntry(context, new List<int>
				{
					charId
				}, Boss.Instance[CombatDomain.CharId2BossId[character.GetTemplateId()]].CombatConfig + 9);
			}
			else
			{
				GameData.Domains.Character.Character fixedCharacter;
				bool flag2 = DomainManager.Character.TryGetFixedCharacterByTemplateId(charTemplateId, out fixedCharacter);
				if (flag2)
				{
					DomainManager.Character.RemoveNonIntelligentCharacter(context, fixedCharacter);
				}
				GameData.Domains.Character.Character character2 = DomainManager.Character.CreateFixedCharacter(context, charTemplateId);
				DomainManager.Character.CompleteCreatingCharacter(character2.GetId());
				this.CombatEntry(context, new List<int>
				{
					character2.GetId()
				}, Boss.Instance[CombatDomain.CharId2BossId[charTemplateId]].CombatConfig);
			}
		}

		// Token: 0x060063FB RID: 25595 RVA: 0x0038B7DC File Offset: 0x003899DC
		[DomainMethod]
		public void GmCmd_FightAnimal(DataContext context, short charTemplateId)
		{
			byte creatingType = Config.Character.Instance[charTemplateId].CreatingType;
			byte b = creatingType;
			GameData.Domains.Character.Character character;
			if (b != 0)
			{
				if (b != 3)
				{
					return;
				}
				character = DomainManager.Character.CreateFixedEnemy(context, charTemplateId);
				DomainManager.Character.CompleteCreatingCharacter(character.GetId());
			}
			else
			{
				character = DomainManager.Character.GetOrCreateFixedCharacterByTemplateId(context, charTemplateId);
			}
			this.CombatEntry(context, new List<int>
			{
				character.GetId()
			}, 2);
		}

		// Token: 0x060063FC RID: 25596 RVA: 0x0038B854 File Offset: 0x00389A54
		[DomainMethod]
		public void GmCmd_FightTestOrgMember(DataContext context, short charTemplateId, int testCount)
		{
			GameData.Domains.Character.Character character = DomainManager.Character.CreateRandomEnemy(context, charTemplateId, null);
			DomainManager.Character.CompleteCreatingCharacter(character.GetId());
			testCount = Math.Clamp(testCount - 1, 0, 8);
			short testConfig = (short)(136 + testCount);
			this.CombatEntry(context, new List<int>
			{
				character.GetId()
			}, testConfig);
		}

		// Token: 0x060063FD RID: 25597 RVA: 0x0038B8B4 File Offset: 0x00389AB4
		[DomainMethod]
		public void GmCmd_FightRandomEnemy(DataContext context, short charTemplateId, sbyte combatTypeAsSbyte)
		{
			GameData.Domains.Character.Character character = DomainManager.Character.CreateRandomEnemy(context, charTemplateId, null);
			DomainManager.Character.CompleteCreatingCharacter(character.GetId());
			if (!true)
			{
			}
			short num;
			switch (combatTypeAsSbyte)
			{
			case 0:
				num = 0;
				break;
			case 1:
				num = 1;
				break;
			case 2:
				num = 2;
				break;
			default:
				num = 3;
				break;
			}
			if (!true)
			{
			}
			short testConfig = num;
			this.CombatEntry(context, new List<int>
			{
				character.GetId()
			}, testConfig);
		}

		// Token: 0x060063FE RID: 25598 RVA: 0x0038B92C File Offset: 0x00389B2C
		[DomainMethod]
		public void GmCmd_FightCharacter(DataContext context, int charId, short combatConfig)
		{
			bool flag = charId == DomainManager.Taiwu.GetTaiwuCharId();
			if (!flag)
			{
				this.CombatEntry(context, new List<int>
				{
					charId
				}, combatConfig);
			}
		}

		// Token: 0x060063FF RID: 25599 RVA: 0x0038B964 File Offset: 0x00389B64
		[DomainMethod]
		public void GmCmd_EnableEnemyAi(DataContext context, bool on)
		{
			this._enableEnemyAi = on;
			bool flag = !on;
			if (flag)
			{
				this.SetMoveState(0, false, false);
			}
		}

		// Token: 0x06006400 RID: 25600 RVA: 0x0038B98B File Offset: 0x00389B8B
		[DomainMethod]
		public void GmCmd_EnableSkillFreeCast(DataContext context, bool on)
		{
			this._enableSkillFreeCast = on;
			this.UpdateSkillCanUse(context, this._selfChar);
		}

		// Token: 0x06006401 RID: 25601 RVA: 0x0038B9A4 File Offset: 0x00389BA4
		[DomainMethod]
		public void GmCmd_SetImmortal(DataContext context, bool isAlly, bool on)
		{
			CombatCharacter combatChar = isAlly ? this._selfChar : this._enemyChar;
			combatChar.Immortal = on;
			bool flag = !on;
			if (flag)
			{
				this.AddToCheckFallenSet(combatChar.GetId());
			}
		}

		// Token: 0x06006402 RID: 25602 RVA: 0x0038B9E0 File Offset: 0x00389BE0
		[DomainMethod]
		public void GmCmd_UnitTestPrepare(DataContext context, bool testing = true)
		{
		}

		// Token: 0x06006403 RID: 25603 RVA: 0x0038B9E3 File Offset: 0x00389BE3
		[DomainMethod]
		public void GmCmd_UnitTestClearAllEquipSkill(DataContext context, int charId)
		{
		}

		// Token: 0x06006404 RID: 25604 RVA: 0x0038B9E8 File Offset: 0x00389BE8
		[DomainMethod]
		public bool GmCmd_UnitTestEquipSkill(DataContext context, int charId, short skillTemplateId, bool isDirect)
		{
			return false;
		}

		// Token: 0x06006405 RID: 25605 RVA: 0x0038B9FB File Offset: 0x00389BFB
		[DomainMethod]
		public void GmCmd_UnitTestSetDistanceToTarget(DataContext context, bool isAlly)
		{
		}

		// Token: 0x06006406 RID: 25606 RVA: 0x0038B9FE File Offset: 0x00389BFE
		[DomainMethod]
		public void GmCmd_ForceRecoverMobilityValue(DataContext context)
		{
			this.ChangeMobilityValue(context, this._selfChar, this._selfChar.GetMaxMobility(), false, null, false);
		}

		// Token: 0x06006407 RID: 25607 RVA: 0x0038BA20 File Offset: 0x00389C20
		[DomainMethod]
		public void GmCmd_ForceRecoverWugCount(DataContext context, bool isAlly, short wugCount)
		{
			CombatCharacter combatChar = isAlly ? this._selfChar : this._enemyChar;
			combatChar.ChangeWugCount(context, (int)wugCount);
		}

		// Token: 0x06006408 RID: 25608 RVA: 0x0038BA4C File Offset: 0x00389C4C
		[DomainMethod]
		public OuterAndInnerDamageStepDisplayData GetBodyPartDamageStepDisplayData(int charId, sbyte bodyPart)
		{
			return new OuterAndInnerDamageStepDisplayData
			{
				Outer = this.CalcDamageDisplayData(charId, bodyPart, false, false, false),
				Inner = this.CalcDamageDisplayData(charId, bodyPart, true, false, false)
			};
		}

		// Token: 0x06006409 RID: 25609 RVA: 0x0038BA8C File Offset: 0x00389C8C
		[DomainMethod]
		public DamageStepDisplayData GetMindDamageStepDisplayData(int charId)
		{
			return this.CalcDamageDisplayData(charId, -1, false, true, false);
		}

		// Token: 0x0600640A RID: 25610 RVA: 0x0038BAAC File Offset: 0x00389CAC
		[DomainMethod]
		public DamageStepDisplayData GetFatalDamageStepDisplayData(int charId)
		{
			return this.CalcDamageDisplayData(charId, -1, false, false, true);
		}

		// Token: 0x0600640B RID: 25611 RVA: 0x0038BACC File Offset: 0x00389CCC
		[DomainMethod]
		public CompleteDamageStepDisplayData GetCompleteDamageStepDisplayData(int charId)
		{
			GameData.Domains.Character.Character character = DomainManager.Character.GetElement_Objects(charId);
			bool isLoseConsummateBonus = character.IsLoseConsummateBonusByFeature();
			CompleteDamageStepDisplayData ret = new CompleteDamageStepDisplayData
			{
				Fatal = this.GetFatalDamageStepDisplayData(charId),
				Mind = this.GetMindDamageStepDisplayData(charId),
				CharacterTemplateId = character.GetTemplateId(),
				CharacterConsummateLevel = (isLoseConsummateBonus ? 0 : character.GetConsummateLevel())
			};
			for (sbyte bodyPart = 0; bodyPart < 7; bodyPart += 1)
			{
				ret.BodyPart[(int)bodyPart] = this.GetBodyPartDamageStepDisplayData(charId, bodyPart);
			}
			return ret;
		}

		// Token: 0x0600640C RID: 25612 RVA: 0x0038BB5C File Offset: 0x00389D5C
		private unsafe DamageStepDisplayData CalcDamageDisplayData(int charId, sbyte bodyPart = -1, bool inner = false, bool mind = false, bool fatal = false)
		{
			DamageStepDisplayData ret = DamageStepDisplayData.Invalid;
			int maxDamageStep = 0;
			Dictionary<short, GameData.Domains.CombatSkill.CombatSkill> learnedCombatSkills = DomainManager.CombatSkill.GetCharCombatSkills(charId);
			foreach (GameData.Domains.CombatSkill.CombatSkill skill in learnedCombatSkills.Values)
			{
				short skillTemplateId = skill.GetId().SkillTemplateId;
				CombatSkillItem config = Config.CombatSkill.Instance[skillTemplateId];
				int damageStep;
				if (mind)
				{
					damageStep = skill.CalcMindDamageStep();
				}
				else if (fatal)
				{
					damageStep = skill.CalcFatalDamageStep();
				}
				else
				{
					bool flag = bodyPart < 0 || bodyPart >= 7;
					bool flag2 = flag;
					if (flag2)
					{
						damageStep = 0;
					}
					else
					{
						damageStep = skill.CalcInjuryDamageStep(inner, bodyPart);
					}
				}
				bool flag3 = damageStep > maxDamageStep;
				if (flag3)
				{
					maxDamageStep = damageStep;
					ret.ActivateSkillTemplateId = skillTemplateId;
					ret.ActivateSkillBonusData = skill.CalcStepBonusDisplayData();
				}
				else
				{
					bool flag4 = damageStep == maxDamageStep && ret.ActivateSkillTemplateId >= 0;
					if (flag4)
					{
						CombatSkillItem activatedConfig = Config.CombatSkill.Instance[ret.ActivateSkillTemplateId];
						bool flag5 = activatedConfig.Grade >= config.Grade && activatedConfig.TemplateId <= config.TemplateId;
						if (!flag5)
						{
							ret.ActivateSkillTemplateId = skillTemplateId;
							ret.ActivateSkillBonusData = skill.CalcStepBonusDisplayData();
						}
					}
				}
			}
			EMarkType expectType = fatal ? EMarkType.Fatal : (mind ? EMarkType.Mind : (inner ? EMarkType.Inner : EMarkType.Outer));
			GameData.Domains.Character.Character character = DomainManager.Character.GetElement_Objects(charId);
			ret.EatingBonusData = (int)(*character.GetEatingItems()).CalcDamageStepBonus(expectType);
			return ret;
		}

		// Token: 0x0600640D RID: 25613 RVA: 0x0038BD28 File Offset: 0x00389F28
		public unsafe DamageStepCollection GetDamageStepCollection(int charId)
		{
			GameData.Domains.Character.Character character = DomainManager.Character.GetElement_Objects(charId);
			short charTemplateId = character.GetTemplateId();
			DamageStepCollection damageSteps = new DamageStepCollection(Config.Character.Instance[charTemplateId].DamageSteps);
			Span<int> span = new Span<int>(stackalloc byte[(UIntPtr)28], 7);
			Span<int> learnedOuterDamageSteps = span;
			span = new Span<int>(stackalloc byte[(UIntPtr)28], 7);
			Span<int> learnedInnerDamageSteps = span;
			learnedOuterDamageSteps.Fill(0);
			learnedInnerDamageSteps.Fill(0);
			int learnedFatalDamageStep = 0;
			int learnedMindDamageStep = 0;
			Dictionary<short, GameData.Domains.CombatSkill.CombatSkill> learnedCombatSkills = DomainManager.CombatSkill.GetCharCombatSkills(charId);
			foreach (GameData.Domains.CombatSkill.CombatSkill skill in learnedCombatSkills.Values)
			{
				for (sbyte i = 0; i < 7; i += 1)
				{
					*learnedOuterDamageSteps[(int)i] = Math.Max(*learnedOuterDamageSteps[(int)i], skill.CalcInjuryDamageStep(false, i));
					*learnedInnerDamageSteps[(int)i] = Math.Max(*learnedInnerDamageSteps[(int)i], skill.CalcInjuryDamageStep(true, i));
				}
				learnedFatalDamageStep = Math.Max(learnedFatalDamageStep, skill.CalcFatalDamageStep());
				learnedMindDamageStep = Math.Max(learnedMindDamageStep, skill.CalcMindDamageStep());
			}
			for (int j = 0; j < 7; j++)
			{
				damageSteps.OuterDamageSteps[j] += *learnedOuterDamageSteps[j];
				damageSteps.InnerDamageSteps[j] += *learnedInnerDamageSteps[j];
			}
			damageSteps.FatalDamageStep += learnedFatalDamageStep;
			damageSteps.MindDamageStep += learnedMindDamageStep;
			int consummateLevel = (int)(character.IsLoseConsummateBonusByFeature() ? 0 : character.GetConsummateLevel());
			ConsummateLevelItem consummateConfig = ConsummateLevel.Instance[consummateLevel];
			damageSteps.FatalDamageStep *= consummateConfig.FatalDamageStepAddPercent;
			damageSteps.MindDamageStep *= consummateConfig.MindDamageStepAddPercent;
			EatingItems eatingItems = *character.GetEatingItems();
			CValuePercentBonus outerDamageStepBonus = eatingItems.CalcDamageStepBonus(EMarkType.Outer);
			for (int k = 0; k < 7; k++)
			{
				damageSteps.OuterDamageSteps[k] *= outerDamageStepBonus;
			}
			CValuePercentBonus innerDamageStepBonus = eatingItems.CalcDamageStepBonus(EMarkType.Inner);
			for (int l = 0; l < 7; l++)
			{
				damageSteps.InnerDamageSteps[l] *= innerDamageStepBonus;
			}
			damageSteps.FatalDamageStep *= eatingItems.CalcDamageStepBonus(EMarkType.Fatal);
			damageSteps.MindDamageStep *= eatingItems.CalcDamageStepBonus(EMarkType.Mind);
			return damageSteps;
		}

		// Token: 0x0600640E RID: 25614 RVA: 0x0038BFEC File Offset: 0x0038A1EC
		public void UpdateDamageCompareData(CombatContext context)
		{
			CombatCharacter attacker = context.Attacker;
			CombatCharacter defender = context.Defender;
			GameData.Domains.Item.Weapon weapon = context.Weapon;
			sbyte bodyPart = context.BodyPart;
			short skillId = context.SkillTemplateId;
			this._damageCompareData.IsAlly = attacker.IsAlly;
			this._damageCompareData.SkillId = skillId;
			bool flag = skillId >= 0;
			if (flag)
			{
				GameData.Domains.CombatSkill.CombatSkill skill = context.Skill;
				for (int i = 0; i < 3; i++)
				{
					this._damageCompareData.HitType[i] = attacker.SkillHitType[i];
					this._damageCompareData.HitValue[i] = attacker.SkillHitValue[i];
					this._damageCompareData.AvoidValue[i] = attacker.SkillAvoidValue[i];
				}
				this._damageCompareData.OuterAttackValue = attacker.GetPenetrate(false, weapon, bodyPart, skillId, skill.GetPenetrations().Outer);
				this._damageCompareData.InnerAttackValue = attacker.GetPenetrate(true, weapon, bodyPart, skillId, skill.GetPenetrations().Inner);
			}
			else
			{
				sbyte hitType = attacker.NormalAttackHitType;
				this._damageCompareData.HitType[0] = hitType;
				this._damageCompareData.HitType[1] = (this._damageCompareData.HitType[2] = -1);
				this._damageCompareData.HitValue[0] = attacker.GetHitValue(weapon, hitType, bodyPart, 0, -1);
				this._damageCompareData.AvoidValue[0] = defender.GetAvoidValue(hitType, bodyPart, -1, false);
				this._damageCompareData.OuterAttackValue = attacker.GetPenetrate(false, weapon, bodyPart, -1, 0);
				this._damageCompareData.InnerAttackValue = attacker.GetPenetrate(true, weapon, bodyPart, -1, 0);
				bool flag2 = attacker.GetId() == this._carrierAnimalCombatCharId;
				if (flag2)
				{
					sbyte taiwuConsummateLevel = this._selfChar.GetCharacter().GetConsummateLevel();
					this._damageCompareData.HitValue[0] = this._damageCompareData.HitValue[0] * (int)(100 + taiwuConsummateLevel * 50) / 100;
					this._damageCompareData.OuterAttackValue = this._damageCompareData.OuterAttackValue * (200 + (int)(taiwuConsummateLevel * 100)) / 100;
					this._damageCompareData.InnerAttackValue = this._damageCompareData.InnerAttackValue * (200 + (int)(taiwuConsummateLevel * 100)) / 100;
				}
				bool isBreakAttacking = attacker.IsBreakAttacking;
				if (isBreakAttacking)
				{
					sbyte pointCost = weapon.GetAttackPreparePointCost();
					short percent = GlobalConfig.Instance.BreakAttackHitBasePercent[(int)pointCost];
					this._damageCompareData.HitValue[0] = this._damageCompareData.HitValue[0] * (int)percent / 100;
				}
			}
			this._damageCompareData.OuterDefendValue = defender.GetPenetrateResist(false, weapon, bodyPart, skillId, false);
			this._damageCompareData.InnerDefendValue = defender.GetPenetrateResist(true, weapon, bodyPart, skillId, false);
			Events.RaiseCompareDataCalcFinished(context, this._damageCompareData);
			this.SetDamageCompareData(this._damageCompareData, context);
		}

		// Token: 0x0600640F RID: 25615 RVA: 0x0038C2C8 File Offset: 0x0038A4C8
		public int GetFinalCriticalOdds(CombatCharacter combatChar)
		{
			int finalIndex = combatChar.SkillFinalAttackHitIndex;
			int hitValue = this._damageCompareData.HitValue[finalIndex];
			int avoidValue = this._damageCompareData.AvoidValue[finalIndex];
			int hitOdds = CFormula.FormulaCalcHitOdds(hitValue, avoidValue);
			return CFormula.FormulaCalcCriticalOdds(hitOdds);
		}

		// Token: 0x06006410 RID: 25616 RVA: 0x0038C30D File Offset: 0x0038A50D
		public void ClearDamageCompareData(DataContext context)
		{
			this._damageCompareData.Clear();
			this.SetDamageCompareData(this._damageCompareData, context);
		}

		// Token: 0x06006411 RID: 25617 RVA: 0x0038C32C File Offset: 0x0038A52C
		private OuterAndInnerInts CalcAndAddInjury(CombatContext context, sbyte hitType, out int finalDamage, out bool critical, int power = 100, int outerPower = 100, int innerPower = 100)
		{
			OuterAndInnerInts markCounts = new OuterAndInnerInts(0, 0);
			critical = context.CheckCritical(hitType);
			bool flag = context.BodyPart >= 0;
			if (flag)
			{
				CombatDomain.CalcMixedInjuryBegin(context, critical);
				CombatDamageResultMixed result = CombatDomain.CalcMixedInjury(context, hitType, critical, power, outerPower, innerPower);
				markCounts = result.MarkCounts;
				finalDamage = result.TotalDamage;
				this.ApplyMixedInjury(context, result);
				CombatDomain.CalcMixedInjuryEnd(context);
			}
			else
			{
				CombatDamageResult result2 = CombatDomain.CalcMindInjury(context);
				markCounts.Outer = result2.MarkCount;
				finalDamage = result2.TotalDamage;
				this.ApplyMindInjury(context, result2);
			}
			return markCounts;
		}

		// Token: 0x06006412 RID: 25618 RVA: 0x0038C3DE File Offset: 0x0038A5DE
		public void AddBounceDamage(CombatContext context, sbyte hitType)
		{
			this.AddBounceDamage(context, hitType, -1, 100);
		}

		// Token: 0x06006413 RID: 25619 RVA: 0x0038C3F4 File Offset: 0x0038A5F4
		public void AddBounceDamage(CombatContext context, sbyte hitType, short skillId, CValuePercent bouncePercent)
		{
			bool canBounce = DomainManager.SpecialEffect.ModifyData(context.AttackerId, skillId, 85, true, -1, -1, -1);
			bool flag = !canBounce;
			if (!flag)
			{
				OuterAndInnerInts bouncePower = context.Defender.GetBouncePower(context.InnerRatio);
				bouncePower.Outer *= bouncePercent;
				bouncePower.Inner *= bouncePercent;
				bool flag2 = bouncePower.Outer <= 0 && bouncePower.Inner <= 0;
				if (!flag2)
				{
					short defendSkillId = context.Defender.GetAffectingDefendSkillId();
					OuterAndInnerInts bounceRange = (defendSkillId >= 0) ? new OuterAndInnerInts((int)this.CombatConfig.MinDistance, (int)DomainManager.CombatSkill.GetElement_CombatSkills(new ValueTuple<int, short>(context.DefenderId, defendSkillId)).GetBounceDistance()) : new OuterAndInnerInts((int)this.CombatConfig.MaxDistance, (int)this.CombatConfig.MinDistance);
					bounceRange = DomainManager.SpecialEffect.ModifyData(context.DefenderId, -1, 177, bounceRange, -1, -1, -1);
					bool flag3 = bounceRange.Outer <= (int)this._currentDistance && (int)this._currentDistance <= bounceRange.Inner;
					if (flag3)
					{
						int num;
						bool flag4;
						this.CalcAndAddInjury(context.Bounce(), hitType, out num, out flag4, 100, bouncePower.Outer, bouncePower.Inner);
					}
				}
			}
		}

		// Token: 0x06006414 RID: 25620 RVA: 0x0038C548 File Offset: 0x0038A748
		public int AddInjuryDamageValue(CombatCharacter attacker, CombatCharacter defender, sbyte bodyPart, int outerDamage, int innerDamage, short combatSkillId, bool updateDefeatMark = true)
		{
			DamageStepCollection damageStepCollection = defender.GetDamageStepCollection();
			CombatContext context = CombatContext.Create(attacker, defender, bodyPart, -1, -1, null);
			int addMarkCount = 0;
			bool flag = outerDamage > 0;
			if (flag)
			{
				int damageStep = damageStepCollection.OuterDamageSteps[(int)bodyPart];
				int[] damageValue = defender.GetOuterDamageValue();
				ValueTuple<int, int, int> damageResult = CombatDomain.CalcSingleInjury(context, (long)outerDamage, damageStep, false, EDamageType.None, damageValue[(int)bodyPart], combatSkillId, 100, 0);
				addMarkCount += damageResult.Item1;
				bool flag2 = damageResult.Item1 > 0;
				if (flag2)
				{
					this.AddInjury(context, defender, bodyPart, false, (sbyte)damageResult.Item1, false, false);
				}
				bool flag3 = damageResult.Item2 > 0;
				if (flag3)
				{
					bool flag4 = defender.GetInjuries().Get(bodyPart, false) < 6;
					if (flag4)
					{
						damageValue[(int)bodyPart] = damageResult.Item2;
					}
					else
					{
						damageValue[(int)bodyPart] = 0;
						damageResult.Item3 -= damageResult.Item2;
						addMarkCount += this.AddFatalDamageValue(context, defender, damageResult.Item2, 0, bodyPart, combatSkillId, EDamageType.None);
					}
					defender.SetOuterDamageValue(damageValue, context);
				}
				bool flag5 = damageResult.Item3 >= 0;
				if (flag5)
				{
					IntPair[] outerDamageValueToShow = defender.GetOuterDamageValueToShow();
					outerDamageValueToShow[(int)bodyPart].First = Math.Max(outerDamageValueToShow[(int)bodyPart].First, 0);
					IntPair[] array = outerDamageValueToShow;
					array[(int)bodyPart].First = array[(int)bodyPart].First + damageResult.Item3;
					outerDamageValueToShow[(int)bodyPart].Second = -1;
					defender.SetOuterDamageValueToShow(outerDamageValueToShow, context);
				}
			}
			bool flag6 = innerDamage > 0;
			if (flag6)
			{
				int damageStep2 = damageStepCollection.InnerDamageSteps[(int)bodyPart];
				int[] damageValue2 = defender.GetInnerDamageValue();
				int originDamageValue = damageValue2[(int)bodyPart];
				ValueTuple<int, int, int> damageResult2 = CombatDomain.CalcSingleInjury(context, (long)innerDamage, damageStep2, true, EDamageType.None, originDamageValue, combatSkillId, 100, 0);
				addMarkCount += damageResult2.Item1;
				bool flag7 = damageResult2.Item1 > 0;
				if (flag7)
				{
					this.AddInjury(context, defender, bodyPart, true, (sbyte)damageResult2.Item1, false, false);
				}
				bool flag8 = damageResult2.Item2 > 0;
				if (flag8)
				{
					bool flag9 = defender.GetInjuries().Get(bodyPart, true) < 6;
					if (flag9)
					{
						damageValue2[(int)bodyPart] = damageResult2.Item2;
					}
					else
					{
						damageValue2[(int)bodyPart] = 0;
						damageResult2.Item3 -= damageResult2.Item2;
						addMarkCount += this.AddFatalDamageValue(context, defender, damageResult2.Item2, 1, bodyPart, -1, EDamageType.None);
					}
					defender.SetInnerDamageValue(damageValue2, context);
				}
				bool flag10 = damageResult2.Item3 >= 0;
				if (flag10)
				{
					IntPair[] innerDamageValueToShow = defender.GetInnerDamageValueToShow();
					innerDamageValueToShow[(int)bodyPart].First = Math.Max(innerDamageValueToShow[(int)bodyPart].First, 0);
					IntPair[] array2 = innerDamageValueToShow;
					array2[(int)bodyPart].First = array2[(int)bodyPart].First + damageResult2.Item3;
					innerDamageValueToShow[(int)bodyPart].Second = -1;
					defender.SetInnerDamageValueToShow(innerDamageValueToShow, context);
				}
			}
			if (updateDefeatMark)
			{
				this.UpdateBodyDefeatMark(context, defender, bodyPart);
				this.AddToCheckFallenSet(defender.GetId());
			}
			return addMarkCount;
		}

		// Token: 0x06006415 RID: 25621 RVA: 0x0038C864 File Offset: 0x0038AA64
		[return: TupleElementNames(new string[]
		{
			"markCount",
			"leftDamage",
			"finalDamageValue"
		})]
		private static ValueTuple<int, int, int> CalcSingleInjury(CombatContext context, long damage, int injuryStep, bool inner, EDamageType damageType, int originDamageValue, short combatSkillId = -1, int criticalPercent = 100, int armorReducePercent = 0)
		{
			CombatCharacter attacker = context.Attacker;
			CombatCharacter defender = context.Defender;
			sbyte bodyPart = context.BodyPart;
			bool flag = inner ? defender.GetInnerInjuryImmunity() : defender.GetOuterInjuryImmunity();
			ValueTuple<int, int, int> result;
			if (flag)
			{
				result = new ValueTuple<int, int, int>(0, 0, -1);
			}
			else
			{
				bool effectImmunity = inner ? DomainManager.SpecialEffect.ModifyData(defender.GetId(), combatSkillId, 241, false, (int)bodyPart, (int)damageType, -1) : DomainManager.SpecialEffect.ModifyData(defender.GetId(), combatSkillId, 242, false, (int)bodyPart, (int)damageType, -1);
				bool flag2 = effectImmunity;
				if (flag2)
				{
					result = new ValueTuple<int, int, int>(0, 0, -1);
				}
				else
				{
					damage *= criticalPercent;
					int attackerId = (damageType == EDamageType.Bounce) ? context.BounceSourceId : context.AttackerId;
					int defenderId = context.DefenderId;
					if (!true)
					{
					}
					ushort num;
					switch (damageType)
					{
					case EDamageType.Direct:
						num = 69;
						break;
					case EDamageType.Bounce:
						num = 70;
						break;
					case EDamageType.FightBack:
						num = 71;
						break;
					default:
						num = ushort.MaxValue;
						break;
					}
					if (!true)
					{
					}
					ushort attackerFieldId = num;
					if (!true)
					{
					}
					switch (damageType)
					{
					case EDamageType.Direct:
						num = 102;
						break;
					case EDamageType.Bounce:
						num = 103;
						break;
					case EDamageType.FightBack:
						num = 104;
						break;
					default:
						num = ushort.MaxValue;
						break;
					}
					if (!true)
					{
					}
					ushort defenderFieldId = num;
					CValueModify modify = CValueModify.Zero.ChangeB(armorReducePercent);
					GameData.Domains.CombatSkill.CombatSkill skill;
					bool flag3 = DomainManager.CombatSkill.TryGetElement_CombatSkills(context.SkillKey, out skill);
					if (flag3)
					{
						modify = modify.ChangeB(skill.GetMakeDamageBreakBonus());
					}
					CombatSkillKey defendSkillKey = new CombatSkillKey(context.DefenderId, context.Defender.GetAffectingDefendSkillId());
					bool anyFatal = context.Defender.GetDefeatMarkCollection().FatalDamageMarkCount > 0;
					GameData.Domains.CombatSkill.CombatSkill defendSkill;
					bool flag4 = DomainManager.CombatSkill.TryGetElement_CombatSkills(defendSkillKey, out defendSkill);
					if (flag4)
					{
						modify = modify.ChangeB(defendSkill.GetAcceptDirectDamageBreakBonus(anyFatal));
					}
					EDataSumType valueSumType = inner ? context.InnerSumType : context.OuterSumType;
					bool flag5 = attackerFieldId != ushort.MaxValue;
					if (flag5)
					{
						modify += DomainManager.SpecialEffect.GetModify(attackerId, combatSkillId, attackerFieldId, inner ? 1 : 0, (int)bodyPart, (criticalPercent > 100) ? 1 : 0, valueSumType);
					}
					bool flag6 = defenderFieldId != ushort.MaxValue;
					if (flag6)
					{
						modify += DomainManager.SpecialEffect.GetModify(defenderId, combatSkillId, defenderFieldId, inner ? 1 : 0, (int)bodyPart, (criticalPercent > 100) ? 1 : 0, valueSumType);
					}
					modify = modify.MaxB(20);
					damage *= modify;
					bool flag7 = damageType == EDamageType.Direct;
					if (flag7)
					{
						damage = DomainManager.SpecialEffect.ModifyData(attacker.GetId(), combatSkillId, 323, damage, defenderId, -1, -1);
					}
					damage = DomainManager.SpecialEffect.ModifyData(attacker.GetId(), combatSkillId, 89, damage, (int)damageType, inner ? 1 : 0, (int)bodyPart);
					bool flag8 = damageType == EDamageType.Direct;
					if (flag8)
					{
						damage = DomainManager.SpecialEffect.ModifyData(defender.GetId(), combatSkillId, 320, damage, attackerId, (criticalPercent > 100) ? 1 : 0, -1);
					}
					damage = DomainManager.SpecialEffect.ModifyData(defender.GetId(), combatSkillId, 114, damage, (int)damageType, inner ? 1 : 0, (int)bodyPart);
					ValueTuple<int, int> markResult = CombatDomain.CalcInjuryMarkCount((int)Math.Min((long)originDamageValue + damage, 2147483647L), injuryStep, (int)(6 - defender.GetInjuries().Get(bodyPart, inner)));
					ValueTuple<int, int, int> damageResult = new ValueTuple<int, int, int>(markResult.Item1, markResult.Item2, (int)damage);
					bool flag9 = damageType == EDamageType.Direct;
					if (flag9)
					{
						Events.RaiseAddDirectDamageValue(attacker.GetDataContext(), attacker.GetId(), defender.GetId(), bodyPart, inner, (int)damage, combatSkillId);
					}
					bool flag10 = damageResult.Item1 > 0 && defender.GetInjuries().Get(bodyPart, inner) == 0 && !DomainManager.SpecialEffect.ModifyData(attacker.GetId(), combatSkillId, 80, true, inner ? 1 : 0, -1, -1);
					if (flag10)
					{
						damageResult.Item1 = 0;
					}
					result = damageResult;
				}
			}
			return result;
		}

		// Token: 0x06006416 RID: 25622 RVA: 0x0038CC50 File Offset: 0x0038AE50
		[return: TupleElementNames(new string[]
		{
			"markCount",
			"leftDamage"
		})]
		public static ValueTuple<int, int> CalcInjuryMarkCount(int damage, int injuryStep, int maxMarkCount = -1)
		{
			injuryStep = Math.Max(injuryStep, 1);
			int markCount = damage / injuryStep;
			markCount = ((maxMarkCount < 0) ? markCount : Math.Min(markCount, maxMarkCount));
			int leftDamage = damage - markCount * injuryStep;
			return new ValueTuple<int, int>(markCount, leftDamage);
		}

		// Token: 0x06006417 RID: 25623 RVA: 0x0038CC8C File Offset: 0x0038AE8C
		private static CombatDamageResultMixed CalcMixedInjury(CombatContext context, sbyte hitType, bool critical, CValuePercent power, CValuePercent outerPower, CValuePercent innerPower)
		{
			int num;
			int num2;
			context.CalcMixedDamage(hitType, power).Deconstruct(out num, out num2);
			int outerDamage = num;
			int innerDamage = num2;
			outerDamage *= outerPower;
			innerDamage *= innerPower;
			EDamageType outerDamageType = context.OuterDamageType;
			EDamageType innerDamageType = context.InnerDamageType;
			bool flag = outerDamageType == EDamageType.Direct;
			if (flag)
			{
				outerDamage *= context.ConsummateBonus;
			}
			bool flag2 = innerDamageType == EDamageType.Direct;
			if (flag2)
			{
				innerDamage *= context.ConsummateBonus;
			}
			bool ignoreArmor = DomainManager.SpecialEffect.ModifyData(context.AttackerId, context.SkillTemplateId, 281, false, -1, -1, -1);
			ItemKey armorKey = context.Defender.Armors[(int)context.BodyPart];
			int outerArmorReduce = 0;
			int innerArmorReduce = 0;
			bool flag3 = !ignoreArmor && armorKey.IsValid();
			if (flag3)
			{
				GameData.Domains.Item.Armor armor = DomainManager.Item.GetElement_Armors(armorKey.Id);
				int weaponAttack = CombatDomain.CalcWeaponAttack(context.Attacker, context.Weapon, context.SkillTemplateId);
				int armorDefense = CombatDomain.CalcArmorDefend(context.Defender, armor);
				int factor = CFormula.FormulaCalcWeaponArmorFactor(100, weaponAttack, armorDefense);
				bool flag4 = factor > 0;
				if (flag4)
				{
					OuterAndInnerShorts reduceInjury = armor.GetInjuryFactor();
					outerArmorReduce = (int)(-(int)reduceInjury.Outer) * factor / 100;
					innerArmorReduce = (int)(-(int)reduceInjury.Inner) * factor / 100;
				}
			}
			int criticalPercent = critical ? context.CalcProperty(hitType).CriticalPercent : 100;
			ValueTuple<int, int, int> outerResult = CombatDomain.CalcSingleInjury(context, (long)outerDamage, context.OuterStep, false, outerDamageType, context.OuterOrigin, context.SkillTemplateId, criticalPercent, outerArmorReduce);
			ValueTuple<int, int, int> innerResult = CombatDomain.CalcSingleInjury(context, (long)innerDamage, context.InnerStep, true, innerDamageType, context.InnerOrigin, context.SkillTemplateId, criticalPercent, innerArmorReduce);
			ref int ptr = ref outerResult.Item1;
			CombatDomain.CalcMixedInjuryMark(context, new OuterAndInnerInts(outerResult.Item1, innerResult.Item1)).Deconstruct(out num2, out num);
			ptr = num2;
			innerResult.Item1 = num;
			CombatDomain.CalcMixedInjuryRefill(context, false, ref outerResult.Item1, ref outerResult.Item2);
			CombatDomain.CalcMixedInjuryRefill(context, true, ref innerResult.Item1, ref innerResult.Item2);
			return new CombatDamageResultMixed
			{
				Outer = new CombatDamageResult
				{
					TotalDamage = outerResult.Item1 * context.OuterStep + outerResult.Item2 - context.OuterOrigin,
					LeftDamage = outerResult.Item2,
					MarkCount = outerResult.Item1
				},
				Inner = new CombatDamageResult
				{
					TotalDamage = innerResult.Item1 * context.InnerStep + innerResult.Item2 - context.InnerOrigin,
					LeftDamage = innerResult.Item2,
					MarkCount = innerResult.Item1
				},
				CriticalPercent = criticalPercent
			};
		}

		// Token: 0x06006418 RID: 25624 RVA: 0x0038CF7E File Offset: 0x0038B17E
		private static void CalcMixedInjuryBegin(CombatContext context, bool critical)
		{
			context.Defender.BeCriticalDuringCalcAddInjury = critical;
			context.Defender.BeCalcInjuryInnerRatio = (int)context.InnerRatio;
		}

		// Token: 0x06006419 RID: 25625 RVA: 0x0038CFA1 File Offset: 0x0038B1A1
		private static void CalcMixedInjuryEnd(CombatContext context)
		{
			context.Defender.BeCriticalDuringCalcAddInjury = false;
			context.Defender.BeCalcInjuryInnerRatio = -1;
		}

		// Token: 0x0600641A RID: 25626 RVA: 0x0038CFC0 File Offset: 0x0038B1C0
		private static OuterAndInnerInts CalcMixedInjuryMark(CombatContext context, OuterAndInnerInts originMarks)
		{
			CombatDomain.<>c__DisplayClass355_0 CS$<>8__locals1;
			CS$<>8__locals1.context = context;
			CS$<>8__locals1.markCounts = originMarks;
			CS$<>8__locals1.markCounts.Outer = CombatDomain.<CalcMixedInjuryMark>g__ModifyMarkCount|355_0(false, ref CS$<>8__locals1);
			CS$<>8__locals1.markCounts.Inner = CombatDomain.<CalcMixedInjuryMark>g__ModifyMarkCount|355_0(true, ref CS$<>8__locals1);
			bool flag = CS$<>8__locals1.context.OuterDamageType == EDamageType.Direct && CS$<>8__locals1.context.InnerDamageType == EDamageType.Direct;
			if (flag)
			{
				CS$<>8__locals1.markCounts = DomainManager.SpecialEffect.ModifyData(CS$<>8__locals1.context.DefenderId, CS$<>8__locals1.context.SkillTemplateId, 116, CS$<>8__locals1.markCounts, (int)CS$<>8__locals1.context.BodyPart, -1, -1);
			}
			CS$<>8__locals1.markCounts.Outer = Math.Max(CS$<>8__locals1.markCounts.Outer, 0);
			CS$<>8__locals1.markCounts.Inner = Math.Max(CS$<>8__locals1.markCounts.Inner, 0);
			return CS$<>8__locals1.markCounts;
		}

		// Token: 0x0600641B RID: 25627 RVA: 0x0038D0B0 File Offset: 0x0038B2B0
		private static void CalcMixedInjuryRefill(CombatContext context, bool inner, ref int markCount, ref int leftDamage)
		{
			int step = Math.Max(inner ? context.InnerStep : context.OuterStep, 1);
			sbyte injury = context.Defender.GetInjuries().Get(context.BodyPart, inner);
			int finalInjury = (int)injury + markCount;
			bool flag = finalInjury == 6 || (finalInjury < 6 && leftDamage < step);
			if (!flag)
			{
				bool flag2 = finalInjury > 6;
				if (flag2)
				{
					int revertInjury = finalInjury - 6;
					markCount -= revertInjury;
					leftDamage += revertInjury * step;
				}
				else
				{
					ValueTuple<int, int> refill = CombatDomain.CalcInjuryMarkCount(leftDamage, step, 6 - finalInjury);
					markCount += refill.Item1;
					leftDamage = refill.Item2;
				}
			}
		}

		// Token: 0x0600641C RID: 25628 RVA: 0x0038D15C File Offset: 0x0038B35C
		private void ApplyMixedInjury(CombatContext context, CombatDamageResultMixed result)
		{
			CombatDomain.<>c__DisplayClass357_0 CS$<>8__locals1;
			CS$<>8__locals1.context = context;
			CombatDamageResultMixed combatDamageResultMixed = result;
			CombatDamageResult outerResult;
			CombatDamageResult innerResult;
			combatDamageResultMixed.Deconstruct(out outerResult, out innerResult);
			CS$<>8__locals1.outerResult = outerResult;
			CS$<>8__locals1.innerResult = innerResult;
			bool flag = CS$<>8__locals1.context.InnerRatio > 0 && CS$<>8__locals1.context.Defender.GetInnerInjuryImmunity();
			if (flag)
			{
				this.ShowImmunityEffectTips(CS$<>8__locals1.context, CS$<>8__locals1.context.DefenderId, EMarkType.Inner);
			}
			bool flag2 = CS$<>8__locals1.context.OuterRatio > 0 && CS$<>8__locals1.context.Defender.GetOuterInjuryImmunity();
			if (flag2)
			{
				this.ShowImmunityEffectTips(CS$<>8__locals1.context, CS$<>8__locals1.context.DefenderId, EMarkType.Outer);
			}
			OuterAndInnerInts fatalDamage = new OuterAndInnerInts(CombatDomain.<ApplyMixedInjury>g__CalcLeftFatalDamage|357_0(false, ref CS$<>8__locals1), CombatDomain.<ApplyMixedInjury>g__CalcLeftFatalDamage|357_0(true, ref CS$<>8__locals1));
			bool flag3 = CS$<>8__locals1.outerResult.MarkCount > 0;
			if (flag3)
			{
				this.AddInjury(CS$<>8__locals1.context, CS$<>8__locals1.context.Defender, CS$<>8__locals1.context.BodyPart, false, (sbyte)CS$<>8__locals1.outerResult.MarkCount, false, CS$<>8__locals1.context.OuterInjuryChangeToOld);
			}
			bool flag4 = CS$<>8__locals1.innerResult.MarkCount > 0;
			if (flag4)
			{
				this.AddInjury(CS$<>8__locals1.context, CS$<>8__locals1.context.Defender, CS$<>8__locals1.context.BodyPart, true, (sbyte)CS$<>8__locals1.innerResult.MarkCount, false, CS$<>8__locals1.context.InnerInjuryChangeToOld);
			}
			bool flag5 = CS$<>8__locals1.context.OuterRatio > 0 || CS$<>8__locals1.outerResult.TotalDamage > 0;
			if (flag5)
			{
				CS$<>8__locals1.context.Defender.AddDamageToShow(CS$<>8__locals1.context, CS$<>8__locals1.outerResult.TotalDamage - fatalDamage.Outer, result.CriticalPercent, CS$<>8__locals1.context.BodyPart, false);
			}
			bool flag6 = CS$<>8__locals1.context.InnerRatio > 0 || CS$<>8__locals1.innerResult.TotalDamage > 0;
			if (flag6)
			{
				CS$<>8__locals1.context.Defender.AddDamageToShow(CS$<>8__locals1.context, CS$<>8__locals1.innerResult.TotalDamage - fatalDamage.Inner, result.CriticalPercent, CS$<>8__locals1.context.BodyPart, true);
			}
			OuterAndInnerInts fatalMarkCounts = new OuterAndInnerInts(0, 0);
			bool flag7 = fatalDamage.Outer > 0;
			if (flag7)
			{
				fatalMarkCounts.Outer = this.AddFatalDamageValue(CS$<>8__locals1.context, CS$<>8__locals1.context.Defender, fatalDamage.Outer, 0, CS$<>8__locals1.context.BodyPart, CS$<>8__locals1.context.SkillTemplateId, CS$<>8__locals1.context.OuterDamageType);
			}
			bool flag8 = fatalDamage.Inner > 0;
			if (flag8)
			{
				fatalMarkCounts.Inner = this.AddFatalDamageValue(CS$<>8__locals1.context, CS$<>8__locals1.context.Defender, fatalDamage.Inner, 1, CS$<>8__locals1.context.BodyPart, CS$<>8__locals1.context.SkillTemplateId, CS$<>8__locals1.context.InnerDamageType);
			}
			Events.RaiseAddDirectFatalDamage(CS$<>8__locals1.context, fatalDamage.Outer, fatalDamage.Inner);
			Events.RaiseAddDirectFatalDamageMark(CS$<>8__locals1.context, CS$<>8__locals1.context.AttackerId, CS$<>8__locals1.context.DefenderId, CS$<>8__locals1.context.Attacker.IsAlly, CS$<>8__locals1.context.BodyPart, fatalMarkCounts.Outer, fatalMarkCounts.Inner, CS$<>8__locals1.context.SkillTemplateId);
			bool flag9 = CS$<>8__locals1.context.DamageType == EDamageType.Bounce;
			if (flag9)
			{
				Events.RaiseBounceInjury(CS$<>8__locals1.context, CS$<>8__locals1.context.BounceSourceId, CS$<>8__locals1.context.DefenderId, CS$<>8__locals1.context.Attacker.IsAlly, CS$<>8__locals1.context.BodyPart, (sbyte)CS$<>8__locals1.outerResult.MarkCount, (sbyte)CS$<>8__locals1.innerResult.MarkCount);
			}
			else
			{
				bool isNonZero = result.MarkCounts.IsNonZero;
				if (isNonZero)
				{
					Events.RaiseAddDirectInjury(CS$<>8__locals1.context, CS$<>8__locals1.context.AttackerId, CS$<>8__locals1.context.DefenderId, CS$<>8__locals1.context.Attacker.IsAlly, CS$<>8__locals1.context.BodyPart, (sbyte)CS$<>8__locals1.outerResult.MarkCount, (sbyte)CS$<>8__locals1.innerResult.MarkCount, CS$<>8__locals1.context.SkillTemplateId);
				}
			}
			bool isNonZero2 = result.MarkCounts.IsNonZero;
			if (isNonZero2)
			{
				this.UpdateBodyDefeatMark(CS$<>8__locals1.context, CS$<>8__locals1.context.Defender);
			}
		}

		// Token: 0x0600641D RID: 25629 RVA: 0x0038D630 File Offset: 0x0038B830
		private static CombatDamageResult CalcMindInjury(CombatContext context)
		{
			int hitOdds = context.CalcProperty(3).HitOdds;
			int damageValue = CFormula.FormulaCalcDamageValue((long)context.BaseDamage, (long)hitOdds, 100L, (long)context.AttackOdds);
			damageValue *= context.ConsummateBonus;
			int extraAddPercent = 0;
			GameData.Domains.CombatSkill.CombatSkill skill;
			bool flag = DomainManager.CombatSkill.TryGetElement_CombatSkills(context.SkillKey, out skill);
			if (flag)
			{
				extraAddPercent += skill.GetMakeDamageBreakBonus();
			}
			damageValue = DomainManager.SpecialEffect.ModifyValueCustom(context.AttackerId, context.SkillTemplateId, 275, damageValue, -1, -1, -1, 0, extraAddPercent, 0, 0);
			damageValue = DomainManager.SpecialEffect.ModifyValueCustom(context.DefenderId, context.SkillTemplateId, 276, damageValue, -1, -1, -1, 0, 0, 0, 0);
			int originDamageValue = context.Defender.GetMindDamageValue();
			int stepValue = context.DamageStepCollection.MindDamageStep;
			ValueTuple<int, int> damageResult = CombatDomain.CalcInjuryMarkCount(damageValue + originDamageValue, stepValue, -1);
			return new CombatDamageResult
			{
				TotalDamage = damageResult.Item1 * stepValue + damageResult.Item2 - originDamageValue,
				LeftDamage = damageResult.Item2,
				MarkCount = damageResult.Item1
			};
		}

		// Token: 0x0600641E RID: 25630 RVA: 0x0038D760 File Offset: 0x0038B960
		private void ApplyMindInjury(CombatContext context, CombatDamageResult result)
		{
			CombatCharacter defender = context.Defender;
			defender.SetMindDamageValue(result.LeftDamage, context);
			this.AppendMindDefeatMark(context, defender, result.MarkCount, context.SkillTemplateId, false);
			Events.RaiseAddMindDamage(this.Context, context.AttackerId, context.DefenderId, result.TotalDamage, result.MarkCount, context.SkillTemplateId);
			bool flag = result.TotalDamage >= 0;
			if (flag)
			{
				defender.AddMindDamageToShow(context, result.TotalDamage);
			}
		}

		// Token: 0x0600641F RID: 25631 RVA: 0x0038D7FC File Offset: 0x0038B9FC
		public static bool CheckCritical(IRandomSource random, int charId, int hitOdds, bool certainCritical = false)
		{
			int criticalOdds = CFormula.FormulaCalcCriticalOdds(hitOdds);
			criticalOdds = DomainManager.SpecialEffect.ModifyValue(charId, 254, criticalOdds, -1, -1, -1, 0, 0, 0, 0);
			CombatCharacter combatChar;
			bool flag = DomainManager.Combat.TryGetElement_CombatCharacterDict(charId, out combatChar) && combatChar.ExecutingTeammateCommandImplement == ETeammateCommandImplement.Attack;
			if (flag)
			{
				criticalOdds *= combatChar.ExecutingTeammateCommandConfig.IntArg;
			}
			return certainCritical || random.CheckPercentProb(criticalOdds);
		}

		// Token: 0x06006420 RID: 25632 RVA: 0x0038D874 File Offset: 0x0038BA74
		public void AddInjury(DataContext context, CombatCharacter character, sbyte bodyPart, bool isInner, sbyte value, bool updateDefeatMark = false, bool changeToOld = false)
		{
			bool flag = value <= 0 || (isInner ? character.GetInnerInjuryImmunity() : character.GetOuterInjuryImmunity());
			if (flag)
			{
				this.ShowImmunityEffectTips(context, character.GetId(), isInner ? EMarkType.Inner : EMarkType.Outer);
			}
			else
			{
				bool changeToMindMark = character.ChangeToMindMark;
				if (changeToMindMark)
				{
					this.AppendMindDefeatMark(context, character, (int)value, -1, false);
				}
				else
				{
					Injuries injuries = character.GetInjuries();
					sbyte injuryLevel = injuries.Get(bodyPart, isInner);
					bool flag2 = injuryLevel < 6;
					if (flag2)
					{
						injuries.Change(bodyPart, isInner, (int)value);
						this.SetInjuries(context, character, injuries, updateDefeatMark, true);
					}
					if (changeToOld)
					{
						this.ChangeToOldInjury(context, character, bodyPart, isInner, (int)value);
					}
					Events.RaiseAddInjury(context, character, bodyPart, isInner, value, changeToOld);
				}
			}
		}

		// Token: 0x06006421 RID: 25633 RVA: 0x0038D930 File Offset: 0x0038BB30
		public void RemoveHalfInjury(DataContext context, CombatCharacter character, bool isInner)
		{
			Injuries injuries = character.GetInjuries();
			Injuries newInjuries = injuries.Subtract(character.GetOldInjuries());
			List<sbyte> bodyPartRandomPool = ObjectPool<List<sbyte>>.Instance.Get();
			bodyPartRandomPool.Clear();
			CRandom.GenerateRemoveInjuryRandomPool(bodyPartRandomPool, newInjuries, isInner, -1);
			int removeCount = bodyPartRandomPool.Count * CValueHalf.RoundUp;
			foreach (sbyte bodyPart in RandomUtils.GetRandomUnrepeated<sbyte>(context.Random, removeCount, bodyPartRandomPool, null))
			{
				injuries.Change(bodyPart, isInner, -1);
			}
			ObjectPool<List<sbyte>>.Instance.Return(bodyPartRandomPool);
			this.SetInjuries(context, character, injuries, true, true);
		}

		// Token: 0x06006422 RID: 25634 RVA: 0x0038D9F0 File Offset: 0x0038BBF0
		public void RemoveInjury(DataContext context, CombatCharacter character, sbyte bodyPart, bool isInner, sbyte removeCount, bool updateDefeatMark = false, bool removeOldInjury = false)
		{
			Injuries injuries = character.GetInjuries();
			injuries.Change(bodyPart, isInner, (int)(-removeCount));
			if (removeOldInjury)
			{
				Injuries oldInjuries = character.GetOldInjuries();
				oldInjuries.Change(bodyPart, isInner, (int)(-removeCount));
				character.SetOldInjuries(oldInjuries, context);
			}
			this.SetInjuries(context, character, injuries, updateDefeatMark, true);
		}

		// Token: 0x06006423 RID: 25635 RVA: 0x0038DA48 File Offset: 0x0038BC48
		public void AddRandomInjury(DataContext context, CombatCharacter character, bool inner, int count = 1, sbyte value = 1, bool changeToOld = false, sbyte bodyPartType = -1)
		{
			bool flag = inner ? character.GetInnerInjuryImmunity() : character.GetOuterInjuryImmunity();
			if (flag)
			{
				this.ShowImmunityEffectTips(context, character.GetId(), inner ? EMarkType.Inner : EMarkType.Outer);
			}
			else
			{
				bool changeToMindMark = character.ChangeToMindMark;
				if (changeToMindMark)
				{
					this.AppendMindDefeatMark(context, character, count * (int)value, -1, false);
				}
				else
				{
					Injuries injuries = character.GetInjuries();
					List<sbyte> addedInjuryList = ObjectPool<List<sbyte>>.Instance.Get();
					List<sbyte> prefer = ObjectPool<List<sbyte>>.Instance.Get();
					List<sbyte> normal = ObjectPool<List<sbyte>>.Instance.Get();
					addedInjuryList.Clear();
					CRandom.GenerateAddRandomInjuryPool(prefer, normal, injuries, inner, bodyPartType, false);
					foreach (sbyte bodyPart in RandomUtils.GetRandomUnrepeated<sbyte>(context.Random, count, prefer, normal))
					{
						sbyte exist = injuries.Get(bodyPart, inner);
						bool flag2 = exist < 6;
						if (flag2)
						{
							injuries.Change(bodyPart, inner, (int)value);
							addedInjuryList.Add(bodyPart);
						}
					}
					this.SetInjuries(context, character, injuries, true, true);
					for (int i = 0; i < addedInjuryList.Count; i++)
					{
						if (changeToOld)
						{
							this.ChangeToOldInjury(context, character, addedInjuryList[i], inner, (int)value);
						}
						Events.RaiseAddInjury(context, character, addedInjuryList[i], inner, value, false);
					}
					ObjectPool<List<sbyte>>.Instance.Return(addedInjuryList);
					ObjectPool<List<sbyte>>.Instance.Return(prefer);
					ObjectPool<List<sbyte>>.Instance.Return(normal);
				}
			}
		}

		// Token: 0x06006424 RID: 25636 RVA: 0x0038DBE0 File Offset: 0x0038BDE0
		public void SetInjuries(DataContext context, CombatCharacter character, Injuries injuries, bool updateDefeatMark = true, bool syncAutoHealProgress = true)
		{
			Injuries oldInjuries = character.GetOldInjuries();
			Injuries newInjuries = injuries.Subtract(oldInjuries);
			bool oldInjuriesChanged = false;
			bool flag = newInjuries.HasAnyInjury();
			if (flag)
			{
				for (sbyte bodyPart = 0; bodyPart < 7; bodyPart += 1)
				{
					ValueTuple<sbyte, sbyte> injury = injuries.Get(bodyPart);
					ValueTuple<sbyte, sbyte> oldInjury = oldInjuries.Get(bodyPart);
					bool flag2 = injury.Item1 < oldInjury.Item1;
					if (flag2)
					{
						oldInjuries.Change(bodyPart, false, (int)(injury.Item1 - oldInjury.Item1));
						oldInjuriesChanged = true;
					}
					bool flag3 = injury.Item2 < oldInjury.Item2;
					if (flag3)
					{
						oldInjuries.Change(bodyPart, true, (int)(injury.Item2 - oldInjury.Item2));
						oldInjuriesChanged = true;
					}
				}
			}
			bool flag4 = oldInjuriesChanged;
			if (flag4)
			{
				character.SetOldInjuries(oldInjuries, context);
			}
			character.SetInjuries(injuries, context);
			character.GetCharacter().SetInjuries(injuries, context);
			if (syncAutoHealProgress)
			{
				this.SyncInjuryAutoHealCollection(context, character);
			}
			if (updateDefeatMark)
			{
				this.UpdateBodyDefeatMark(context, character);
			}
			this.UpdateOtherActionCanUse(context, character, 0);
			bool flag5 = this.IsMainCharacter(character);
			if (flag5)
			{
				this.UpdateAllTeammateCommandUsable(context, character.IsAlly, ETeammateCommandImplement.HealInjury);
				this.UpdateAllTeammateCommandUsable(context, character.IsAlly, ETeammateCommandImplement.TransferInjury);
			}
		}

		// Token: 0x06006425 RID: 25637 RVA: 0x0038DD30 File Offset: 0x0038BF30
		public bool CheckBodyPartInjury(CombatCharacter character, sbyte bodyPartType, bool checkHeavyInjury = false)
		{
			Injuries injuries = character.GetInjuries();
			int needOuterCount = DomainManager.SpecialEffect.ModifyData(character.GetId(), -1, 168, 6, (int)bodyPartType, 0, -1);
			int needInnerCount = DomainManager.SpecialEffect.ModifyData(character.GetId(), -1, 168, 6, (int)bodyPartType, 1, -1);
			if (checkHeavyInjury)
			{
				needOuterCount = Math.Min(needOuterCount, 5);
				needInnerCount = Math.Min(needInnerCount, 5);
			}
			bool broken = (int)injuries.Get(bodyPartType, false) >= needOuterCount || (int)injuries.Get(bodyPartType, true) >= needInnerCount;
			return DomainManager.SpecialEffect.ModifyData(character.GetId(), -1, 169, broken, (int)bodyPartType, -1, -1);
		}

		// Token: 0x06006426 RID: 25638 RVA: 0x0038DDD8 File Offset: 0x0038BFD8
		public void AddFlaw(DataContext context, CombatCharacter character, sbyte level, CombatSkillKey skillKey, sbyte bodyPart = -1, int count = 1, bool raiseEvent = true)
		{
			bool flawImmunity = character.GetFlawImmunity();
			if (flawImmunity)
			{
				this.ShowImmunityEffectTips(context, character.GetId(), EMarkType.Flaw);
			}
			else
			{
				bool changeToMindMark = character.ChangeToMindMark;
				if (changeToMindMark)
				{
					this.AppendMindDefeatMark(context, character, count, -1, false);
				}
				else
				{
					short skillId = skillKey.SkillTemplateId;
					bool flag = skillId >= 0;
					if (flag)
					{
						GameData.Domains.CombatSkill.CombatSkill skill = DomainManager.CombatSkill.GetElement_CombatSkills(skillKey);
						level = (sbyte)((int)level + skill.GetBreakoutGridCombatSkillPropertyBonus(41));
					}
					bool levelCanReduce = DomainManager.SpecialEffect.ModifyData(character.GetId(), skillId, 128, true, -1, -1, -1);
					EDataSumType levelSumType = DataSumTypeHelper.CalcSumType(true, levelCanReduce);
					int maxLevel = GlobalConfig.Instance.FlawBaseKeepTime.Length - 1;
					level = (sbyte)Math.Clamp((int)level * DomainManager.SpecialEffect.GetModify(character.GetId(), skillId, 127, -1, -1, -1, levelSumType), 0, maxLevel);
					count = (int)((sbyte)(count + DomainManager.SpecialEffect.GetModifyValue(character.GetId(), skillId, 129, EDataModifyType.Add, -1, -1, -1, EDataSumType.All)));
					count = (int)((sbyte)DomainManager.SpecialEffect.ModifyData(character.GetId(), skillId, 129, count, (int)bodyPart, (int)level, -1));
					bool flag2 = count <= 0;
					if (!flag2)
					{
						List<sbyte> partRandomPool = ObjectPool<List<sbyte>>.Instance.Get();
						for (int i = 0; i < count; i++)
						{
							bool flag3 = bodyPart < 0;
							if (flag3)
							{
								partRandomPool.Clear();
								foreach (sbyte part in character.GetAvailableBodyParts())
								{
									bool flag4 = (int)character.GetFlawCount()[(int)part] < character.GetMaxFlawCount();
									if (flag4)
									{
										partRandomPool.Add(part);
									}
								}
								bool flag5 = partRandomPool.Count == 0;
								if (flag5)
								{
									foreach (sbyte part2 in character.GetAvailableBodyParts())
									{
										partRandomPool.Add(part2);
									}
								}
								bodyPart = partRandomPool[context.Random.Next(partRandomPool.Count)];
							}
							character.AddOrUpdateFlawOrAcupoint(context, bodyPart, true, level, raiseEvent, -1, -1);
						}
						ObjectPool<List<sbyte>>.Instance.Return(partRandomPool);
					}
				}
			}
		}

		// Token: 0x06006427 RID: 25639 RVA: 0x0038E03C File Offset: 0x0038C23C
		public void RemoveFlaw(DataContext context, CombatCharacter character, sbyte bodyPart, int index, bool raiseEvent = true, bool updateMark = true)
		{
			FlawOrAcupointCollection flaws = character.GetFlawCollection();
			byte[] flawCount = character.GetFlawCount();
			sbyte level = flaws.BodyPartDict[bodyPart][index].Item1;
			flaws.BodyPartDict[bodyPart].RemoveAt(index);
			byte[] array = flawCount;
			array[(int)bodyPart] = array[(int)bodyPart] - 1;
			character.SetFlawCollection(flaws, context);
			character.SetFlawCount(flawCount, context);
			if (updateMark)
			{
				this.UpdateBodyDefeatMark(context, character, bodyPart);
			}
			if (raiseEvent)
			{
				Events.RaiseFlawRemoved(context, character, bodyPart, level);
			}
		}

		// Token: 0x06006428 RID: 25640 RVA: 0x0038E0C8 File Offset: 0x0038C2C8
		public void ReduceFlawKeepTimePercent(DataContext context, CombatCharacter combatChar, int reducePercent, bool raiseEvent = true)
		{
			FlawOrAcupointCollection flawCollection = combatChar.GetFlawCollection();
			FlawOrAcupointCollection.ReduceKeepTimeResult flawRetValue = flawCollection.ReduceKeepTimePercent(combatChar, reducePercent, combatChar.GetFlawCount(), true);
			this.ApplyReduceFlawResult(context, combatChar, raiseEvent, flawRetValue);
		}

		// Token: 0x06006429 RID: 25641 RVA: 0x0038E0FC File Offset: 0x0038C2FC
		public void RemoveAllFlaw(DataContext context, CombatCharacter combatChar)
		{
			FlawOrAcupointCollection flawCollection = combatChar.GetFlawCollection();
			FlawOrAcupointCollection.ReduceKeepTimeResult flawRetValue = flawCollection.ReduceKeepTime(combatChar, int.MaxValue, combatChar.GetFlawCount(), true);
			this.ApplyReduceFlawResult(context, combatChar, false, flawRetValue);
		}

		// Token: 0x0600642A RID: 25642 RVA: 0x0038E130 File Offset: 0x0038C330
		private void ApplyReduceFlawResult(DataContext context, CombatCharacter combatChar, bool raiseEvent, FlawOrAcupointCollection.ReduceKeepTimeResult reduceResult)
		{
			FlawOrAcupointCollection collection = combatChar.GetFlawCollection();
			bool dataChanged = reduceResult.DataChanged;
			if (dataChanged)
			{
				combatChar.SetFlawCollection(collection, context);
			}
			bool countChanged = reduceResult.CountChanged;
			if (countChanged)
			{
				combatChar.SetFlawCount(combatChar.GetFlawCount(), context);
				this.UpdateBodyDefeatMark(context, combatChar);
				bool flag = this.IsMainCharacter(combatChar);
				if (flag)
				{
					this.UpdateAllTeammateCommandUsable(context, combatChar.IsAlly, ETeammateCommandImplement.HealFlaw);
				}
			}
			if (raiseEvent)
			{
				foreach (ValueTuple<sbyte, sbyte> valueTuple in reduceResult.RemovedList)
				{
					sbyte bodyPart = valueTuple.Item1;
					sbyte level = valueTuple.Item2;
					Events.RaiseFlawRemoved(context, combatChar, bodyPart, level);
				}
			}
		}

		// Token: 0x0600642B RID: 25643 RVA: 0x0038E1FC File Offset: 0x0038C3FC
		public void TransferRandomFlaw(DataContext context, CombatCharacter src, CombatCharacter dst)
		{
			DefeatMarkCollection marks = src.GetDefeatMarkCollection();
			List<sbyte> pool = ObjectPool<List<sbyte>>.Instance.Get();
			for (sbyte i = 0; i < 7; i += 1)
			{
				bool flag = marks.FlawMarkList[(int)i].Count > 0;
				if (flag)
				{
					pool.Add(i);
				}
			}
			bool flag2 = pool.Count > 0;
			if (flag2)
			{
				sbyte bodyPart = pool.GetRandom(context.Random);
				int index = context.Random.Next(marks.FlawMarkList[(int)bodyPart].Count);
				this.TransferFlaw(context, src, dst, bodyPart, index);
			}
			ObjectPool<List<sbyte>>.Instance.Return(pool);
		}

		// Token: 0x0600642C RID: 25644 RVA: 0x0038E2A0 File Offset: 0x0038C4A0
		public void TransferFlaw(DataContext context, CombatCharacter srcChar, CombatCharacter destChar, sbyte bodyPart, int index)
		{
			ValueTuple<sbyte, int, int> flaw = srcChar.GetFlawCollection().BodyPartDict[bodyPart][index];
			this.RemoveFlaw(context, srcChar, bodyPart, index, false, true);
			destChar.AddOrUpdateFlawOrAcupoint(context, bodyPart, true, flaw.Item1, true, flaw.Item3, flaw.Item2);
		}

		// Token: 0x0600642D RID: 25645 RVA: 0x0038E2F8 File Offset: 0x0038C4F8
		public void AddAcupoint(DataContext context, CombatCharacter character, sbyte level, CombatSkillKey skillKey, sbyte bodyPart = -1, int count = 1, bool raiseEvent = true)
		{
			bool acupointImmunity = character.GetAcupointImmunity();
			if (acupointImmunity)
			{
				this.ShowImmunityEffectTips(context, character.GetId(), EMarkType.Acupoint);
			}
			else
			{
				bool changeToMindMark = character.ChangeToMindMark;
				if (changeToMindMark)
				{
					this.AppendMindDefeatMark(context, character, count, -1, false);
				}
				else
				{
					short skillId = skillKey.SkillTemplateId;
					bool flag = skillId >= 0;
					if (flag)
					{
						GameData.Domains.CombatSkill.CombatSkill skill = DomainManager.CombatSkill.GetElement_CombatSkills(skillKey);
						level = (sbyte)((int)level + skill.GetBreakoutGridCombatSkillPropertyBonus(40));
					}
					bool levelCanReduce = DomainManager.SpecialEffect.ModifyData(character.GetId(), skillId, 133, true, -1, -1, -1);
					EDataSumType levelSumType = DataSumTypeHelper.CalcSumType(true, levelCanReduce);
					int maxLevel = GlobalConfig.Instance.AcupointBaseKeepTime.Length - 1;
					level = (sbyte)Math.Clamp((int)level * DomainManager.SpecialEffect.GetModify(character.GetId(), skillId, 132, -1, -1, -1, levelSumType), 0, maxLevel);
					count = (int)((sbyte)(count + DomainManager.SpecialEffect.GetModifyValue(character.GetId(), skillId, 134, EDataModifyType.Add, -1, -1, -1, EDataSumType.All)));
					count = (int)((sbyte)DomainManager.SpecialEffect.ModifyData(character.GetId(), skillId, 134, count, (int)bodyPart, (int)level, -1));
					bool flag2 = count <= 0;
					if (!flag2)
					{
						List<sbyte> partRandomPool = ObjectPool<List<sbyte>>.Instance.Get();
						for (int i = 0; i < count; i++)
						{
							bool flag3 = bodyPart < 0;
							if (flag3)
							{
								partRandomPool.Clear();
								foreach (sbyte part in character.GetAvailableBodyParts())
								{
									bool flag4 = (int)character.GetAcupointCount()[(int)part] < character.GetMaxAcupointCount();
									if (flag4)
									{
										partRandomPool.Add(part);
									}
								}
								bool flag5 = partRandomPool.Count == 0;
								if (flag5)
								{
									foreach (sbyte part2 in character.GetAvailableBodyParts())
									{
										partRandomPool.Add(part2);
									}
								}
								bodyPart = partRandomPool[context.Random.Next(partRandomPool.Count)];
							}
							character.AddOrUpdateFlawOrAcupoint(context, bodyPart, false, level, raiseEvent, -1, -1);
						}
						ObjectPool<List<sbyte>>.Instance.Return(partRandomPool);
					}
				}
			}
		}

		// Token: 0x0600642E RID: 25646 RVA: 0x0038E55C File Offset: 0x0038C75C
		public void RemoveAcupoint(DataContext context, CombatCharacter character, sbyte bodyPart, int index, bool raiseEvent = true, bool updateMark = true)
		{
			FlawOrAcupointCollection acupoints = character.GetAcupointCollection();
			byte[] acupointCount = character.GetAcupointCount();
			sbyte level = acupoints.BodyPartDict[bodyPart][index].Item1;
			acupoints.BodyPartDict[bodyPart].RemoveAt(index);
			byte[] array = acupointCount;
			array[(int)bodyPart] = array[(int)bodyPart] - 1;
			character.SetAcupointCollection(acupoints, context);
			character.SetAcupointCount(acupointCount, context);
			if (updateMark)
			{
				this.UpdateBodyDefeatMark(context, character, bodyPart);
			}
			if (raiseEvent)
			{
				Events.RaiseAcuPointRemoved(context, character, bodyPart, level);
			}
		}

		// Token: 0x0600642F RID: 25647 RVA: 0x0038E5E8 File Offset: 0x0038C7E8
		public void ReduceAcupointKeepTimePercent(DataContext context, CombatCharacter combatChar, int reducePercent, bool raiseEvent = true)
		{
			FlawOrAcupointCollection acupointCollection = combatChar.GetAcupointCollection();
			FlawOrAcupointCollection.ReduceKeepTimeResult acupointRetValue = acupointCollection.ReduceKeepTimePercent(combatChar, reducePercent, combatChar.GetAcupointCount(), false);
			this.ApplyReduceAcupointResult(context, combatChar, raiseEvent, acupointRetValue);
		}

		// Token: 0x06006430 RID: 25648 RVA: 0x0038E61C File Offset: 0x0038C81C
		public void RemoveAllAcupoint(DataContext context, CombatCharacter combatChar)
		{
			FlawOrAcupointCollection acupointCollection = combatChar.GetAcupointCollection();
			FlawOrAcupointCollection.ReduceKeepTimeResult acupointRetValue = acupointCollection.ReduceKeepTime(combatChar, int.MaxValue, combatChar.GetAcupointCount(), false);
			this.ApplyReduceAcupointResult(context, combatChar, false, acupointRetValue);
		}

		// Token: 0x06006431 RID: 25649 RVA: 0x0038E650 File Offset: 0x0038C850
		private void ApplyReduceAcupointResult(DataContext context, CombatCharacter combatChar, bool raiseEvent, FlawOrAcupointCollection.ReduceKeepTimeResult reduceResult)
		{
			FlawOrAcupointCollection collection = combatChar.GetAcupointCollection();
			bool dataChanged = reduceResult.DataChanged;
			if (dataChanged)
			{
				combatChar.SetAcupointCollection(collection, context);
			}
			bool countChanged = reduceResult.CountChanged;
			if (countChanged)
			{
				combatChar.SetAcupointCount(combatChar.GetAcupointCount(), context);
				this.UpdateBodyDefeatMark(context, combatChar);
				bool flag = this.IsMainCharacter(combatChar);
				if (flag)
				{
					this.UpdateAllTeammateCommandUsable(context, combatChar.IsAlly, ETeammateCommandImplement.HealAcupoint);
				}
			}
			if (raiseEvent)
			{
				foreach (ValueTuple<sbyte, sbyte> valueTuple in reduceResult.RemovedList)
				{
					sbyte bodyPart = valueTuple.Item1;
					sbyte level = valueTuple.Item2;
					Events.RaiseAcuPointRemoved(context, combatChar, bodyPart, level);
				}
			}
		}

		// Token: 0x06006432 RID: 25650 RVA: 0x0038E71C File Offset: 0x0038C91C
		public void TransferRandomAcupoint(DataContext context, CombatCharacter src, CombatCharacter dst)
		{
			DefeatMarkCollection marks = src.GetDefeatMarkCollection();
			List<sbyte> pool = ObjectPool<List<sbyte>>.Instance.Get();
			for (sbyte i = 0; i < 7; i += 1)
			{
				bool flag = marks.AcupointMarkList[(int)i].Count > 0;
				if (flag)
				{
					pool.Add(i);
				}
			}
			bool flag2 = pool.Count > 0;
			if (flag2)
			{
				sbyte bodyPart = pool.GetRandom(context.Random);
				int index = context.Random.Next(marks.AcupointMarkList[(int)bodyPart].Count);
				this.TransferAcupoint(context, src, dst, bodyPart, index);
			}
			ObjectPool<List<sbyte>>.Instance.Return(pool);
		}

		// Token: 0x06006433 RID: 25651 RVA: 0x0038E7C0 File Offset: 0x0038C9C0
		public void TransferAcupoint(DataContext context, CombatCharacter srcChar, CombatCharacter destChar, sbyte bodyPart, int index)
		{
			ValueTuple<sbyte, int, int> acupoint = srcChar.GetAcupointCollection().BodyPartDict[bodyPart][index];
			this.RemoveAcupoint(context, srcChar, bodyPart, index, false, true);
			destChar.AddOrUpdateFlawOrAcupoint(context, bodyPart, false, acupoint.Item1, true, acupoint.Item3, acupoint.Item2);
		}

		// Token: 0x06006434 RID: 25652 RVA: 0x0038E818 File Offset: 0x0038CA18
		public void RemoveHalfFlawOrAcupoint(DataContext context, CombatCharacter combatChar, bool isFlaw)
		{
			DefeatMarkCollection collection = combatChar.GetDefeatMarkCollection();
			int totalCount = isFlaw ? collection.GetTotalFlawCount() : collection.GetTotalAcupointCount();
			int removeCount = totalCount * CValueHalf.RoundUp;
			combatChar.RemoveRandomFlawOrAcupoint(context, isFlaw, removeCount);
		}

		// Token: 0x06006435 RID: 25653 RVA: 0x0038E858 File Offset: 0x0038CA58
		public int GetBrokenBodyPartCount(CombatCharacter character)
		{
			int counter = 0;
			for (sbyte type = 0; type < 7; type += 1)
			{
				bool flag = this.CheckBodyPartInjury(character, type, false);
				if (flag)
				{
					counter++;
				}
			}
			return counter;
		}

		// Token: 0x06006436 RID: 25654 RVA: 0x0038E894 File Offset: 0x0038CA94
		public void ChangeToOldInjury(DataContext context, CombatCharacter character, sbyte bodyPart, bool isInner, int count)
		{
			Injuries oldInjuries = character.GetOldInjuries();
			count = Math.Min(count, (int)(character.GetInjuries().Get(bodyPart, isInner) - oldInjuries.Get(bodyPart, isInner)));
			oldInjuries.Change(bodyPart, isInner, (int)((sbyte)count));
			character.SetOldInjuries(oldInjuries, context);
			this.SyncInjuryAutoHealCollection(context, character);
		}

		// Token: 0x06006437 RID: 25655 RVA: 0x0038E8F0 File Offset: 0x0038CAF0
		public int ChangeToOldInjury(DataContext context, CombatCharacter character, int count, Func<sbyte, bool> bodyPartFilter = null)
		{
			Injuries newInjuries = character.GetInjuries().Subtract(character.GetOldInjuries());
			List<ValueTuple<sbyte, bool>> injuryRandomPool = CombatDomain.CachedInjuryRandomPool;
			for (sbyte bodyPart = 0; bodyPart < 7; bodyPart += 1)
			{
				bool flag = bodyPartFilter != null && !bodyPartFilter(bodyPart);
				if (!flag)
				{
					ValueTuple<sbyte, sbyte> injury = newInjuries.Get(bodyPart);
					for (int i = 0; i < (int)injury.Item1; i++)
					{
						injuryRandomPool.Add(new ValueTuple<sbyte, bool>(bodyPart, false));
					}
					for (int j = 0; j < (int)injury.Item2; j++)
					{
						injuryRandomPool.Add(new ValueTuple<sbyte, bool>(bodyPart, true));
					}
				}
			}
			int changeCount = Math.Min(injuryRandomPool.Count, count);
			for (int k = 0; k < changeCount; k++)
			{
				int index = context.Random.Next(0, injuryRandomPool.Count);
				ValueTuple<sbyte, bool> injuryInfo = injuryRandomPool[index];
				this.ChangeToOldInjury(context, character, injuryInfo.Item1, injuryInfo.Item2, 1);
				injuryRandomPool.RemoveAt(index);
			}
			injuryRandomPool.Clear();
			return changeCount;
		}

		// Token: 0x06006438 RID: 25656 RVA: 0x0038EA28 File Offset: 0x0038CC28
		private void SyncInjuryAutoHealCollection(DataContext context, CombatCharacter character)
		{
			InjuryAutoHealCollection oldAutoHealCollection = character.GetOldInjuryAutoHealCollection();
			InjuryAutoHealCollection autoHealCollection = character.GetInjuryAutoHealCollection();
			Injuries oldInjuries = character.GetOldInjuries();
			Injuries newInjuries = character.GetInjuries().Subtract(oldInjuries);
			oldAutoHealCollection.SyncInjuries(ref oldInjuries);
			autoHealCollection.SyncInjuries(ref newInjuries);
			character.SetOldInjuryAutoHealCollection(oldAutoHealCollection, context);
			character.SetInjuryAutoHealCollection(autoHealCollection, context);
		}

		// Token: 0x06006439 RID: 25657 RVA: 0x0038EA80 File Offset: 0x0038CC80
		[DomainMethod]
		public uint GetHealInjuryBanReason(int doctorCharId, int patientCharId)
		{
			GameData.Domains.Character.Character doctor;
			bool flag = !DomainManager.Character.TryGetElement_Objects(doctorCharId, out doctor);
			uint result;
			if (flag)
			{
				result = 0U;
			}
			else
			{
				GameData.Domains.Character.Character patient;
				bool flag2 = !DomainManager.Character.TryGetElement_Objects(patientCharId, out patient);
				if (flag2)
				{
					result = 0U;
				}
				else
				{
					result = this.GetHealInjuryBanReason(doctor, patient);
				}
			}
			return result;
		}

		// Token: 0x0600643A RID: 25658 RVA: 0x0038EAD4 File Offset: 0x0038CCD4
		[DomainMethod]
		public uint GetHealPoisonBanReason(int doctorCharId, int patientCharId)
		{
			GameData.Domains.Character.Character doctor;
			bool flag = !DomainManager.Character.TryGetElement_Objects(doctorCharId, out doctor);
			uint result;
			if (flag)
			{
				result = 0U;
			}
			else
			{
				GameData.Domains.Character.Character patient;
				bool flag2 = !DomainManager.Character.TryGetElement_Objects(patientCharId, out patient);
				if (flag2)
				{
					result = 0U;
				}
				else
				{
					result = this.GetHealPoisonBanReason(doctor, patient);
				}
			}
			return result;
		}

		// Token: 0x0600643B RID: 25659 RVA: 0x0038EB28 File Offset: 0x0038CD28
		public BoolArray32 GetHealInjuryBanReason(CombatCharacter doctor, CombatCharacter patient)
		{
			return this.GetHealInjuryBanReason(doctor.GetCharacter(), patient.GetCharacter());
		}

		// Token: 0x0600643C RID: 25660 RVA: 0x0038EB4C File Offset: 0x0038CD4C
		public BoolArray32 GetHealInjuryBanReason(GameData.Domains.Character.Character doctor, GameData.Domains.Character.Character patient)
		{
			BoolArray32 array = default(BoolArray32);
			bool flag = patient.GetInjuries().HasAnyInjury();
			if (flag)
			{
				int hasHerb = doctor.GetResource(5);
				int needHerb = patient.CalcHealCostHerb(EHealActionType.Healing, false);
				int herbHeal;
				int num;
				int num2;
				this.HealInjury(patient.GetId(), doctor, out herbHeal, out num, out num2, true, true, true, null, null, false);
				array[2] = (hasHerb < needHerb && herbHeal <= 0);
				int attainmentHeal;
				this.HealInjury(patient.GetId(), doctor, out attainmentHeal, out num2, out num, true, true, false, null, null, false);
				array[3] = (attainmentHeal <= 0);
			}
			else
			{
				array[0] = true;
			}
			return array;
		}

		// Token: 0x0600643D RID: 25661 RVA: 0x0038EBFC File Offset: 0x0038CDFC
		public BoolArray32 GetHealPoisonBanReason(CombatCharacter doctor, CombatCharacter patient)
		{
			return this.GetHealPoisonBanReason(doctor.GetCharacter(), patient.GetCharacter());
		}

		// Token: 0x0600643E RID: 25662 RVA: 0x0038EC20 File Offset: 0x0038CE20
		public BoolArray32 GetHealPoisonBanReason(GameData.Domains.Character.Character doctor, GameData.Domains.Character.Character patient)
		{
			BoolArray32 array = default(BoolArray32);
			bool flag = patient.GetPoisoned().IsNonZero();
			if (flag)
			{
				int hasHerb = doctor.GetResource(5);
				int needHerb = patient.CalcHealCostHerb(EHealActionType.Detox, false);
				int num;
				int herbHeal;
				int num2;
				this.HealPoison(patient.GetId(), doctor, out num, out herbHeal, out num2, true, true, true, false);
				array[2] = (hasHerb < needHerb && herbHeal <= 0);
				int attainmentHeal;
				this.HealPoison(patient.GetId(), doctor, out num2, out attainmentHeal, out num, true, true, false, false);
				array[3] = (attainmentHeal <= 0);
			}
			else
			{
				array[0] = true;
			}
			return array;
		}

		// Token: 0x0600643F RID: 25663 RVA: 0x0038ECC4 File Offset: 0x0038CEC4
		public int GetMaxCanHealInjuryCount(int doctorCharId, int patientCharId)
		{
			GameData.Domains.Character.Character doctor;
			bool flag = !DomainManager.Character.TryGetElement_Objects(doctorCharId, out doctor);
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				int num;
				int maxHealMarkCount;
				int num2;
				this.HealInjury(patientCharId, doctor, out num, out maxHealMarkCount, out num2, true, true, true, null, null, false);
				result = maxHealMarkCount;
			}
			return result;
		}

		// Token: 0x06006440 RID: 25664 RVA: 0x0038ED04 File Offset: 0x0038CF04
		public static int GetHealInjuryCostHerb(Injuries injuries)
		{
			int costHerb = (int)GlobalConfig.Instance.HealInjuryBaseHerb;
			for (sbyte bodyPart = 0; bodyPart < 7; bodyPart += 1)
			{
				ValueTuple<sbyte, sbyte> injury = injuries.Get(bodyPart);
				bool flag = injury.Item1 > 0;
				if (flag)
				{
					costHerb += (int)GlobalConfig.Instance.HealInjuryExtraHerb[(int)(injury.Item1 - 1)];
				}
				bool flag2 = injury.Item2 > 0;
				if (flag2)
				{
					costHerb += (int)GlobalConfig.Instance.HealInjuryExtraHerb[(int)(injury.Item2 - 1)];
				}
			}
			return costHerb;
		}

		// Token: 0x06006441 RID: 25665 RVA: 0x0038ED8C File Offset: 0x0038CF8C
		public static int GetHealInjuryCostMoney(Injuries injuries, sbyte doctorBehaviorType)
		{
			int costMoney = (int)GlobalConfig.Instance.HealInjuryBaseMoney;
			for (sbyte bodyPart = 0; bodyPart < 7; bodyPart += 1)
			{
				ValueTuple<sbyte, sbyte> injury = injuries.Get(bodyPart);
				bool flag = injury.Item1 > 0;
				if (flag)
				{
					costMoney += (int)GlobalConfig.Instance.HealInjuryExtraMoney[(int)(injury.Item1 - 1)];
				}
				bool flag2 = injury.Item2 > 0;
				if (flag2)
				{
					costMoney += (int)GlobalConfig.Instance.HealInjuryExtraMoney[(int)(injury.Item2 - 1)];
				}
			}
			return costMoney * (int)GlobalConfig.Instance.HealMoneyPercent[(int)doctorBehaviorType] / 100;
		}

		// Token: 0x06006442 RID: 25666 RVA: 0x0038EE24 File Offset: 0x0038D024
		public static int GetHealInjuryCostSpiritualDebt(Injuries injuries)
		{
			int cost = 0;
			for (sbyte bodyPart = 0; bodyPart < 7; bodyPart += 1)
			{
				ValueTuple<sbyte, sbyte> injury = injuries.Get(bodyPart);
				bool flag = injury.Item1 > 0;
				if (flag)
				{
					cost += (int)GlobalConfig.Instance.HealInjuryCostSpiritualDebt[(int)(injury.Item1 - 1)];
				}
				bool flag2 = injury.Item2 > 0;
				if (flag2)
				{
					cost += (int)GlobalConfig.Instance.HealInjuryCostSpiritualDebt[(int)(injury.Item2 - 1)];
				}
			}
			return cost;
		}

		// Token: 0x06006443 RID: 25667 RVA: 0x0038EEA4 File Offset: 0x0038D0A4
		public unsafe static int GetHealPoisonCostHerb(PoisonInts poisons)
		{
			int costHerb = (int)GlobalConfig.Instance.HealPoisonBaseHerb;
			for (sbyte type = 0; type < 6; type += 1)
			{
				int poison = *(ref poisons.Items.FixedElementField + (IntPtr)type * 4);
				int poisonLevel = (int)PoisonsAndLevels.CalcPoisonedLevel(poison);
				bool flag = poisonLevel > 0;
				if (flag)
				{
					costHerb += (int)GlobalConfig.Instance.HealPoisonExtraHerb[poisonLevel - 1];
				}
			}
			return costHerb;
		}

		// Token: 0x06006444 RID: 25668 RVA: 0x0038EF10 File Offset: 0x0038D110
		public unsafe static int GetHealPoisonCostMoney(PoisonInts poisons, sbyte doctorBehaviorType)
		{
			int costMoney = (int)GlobalConfig.Instance.HealPoisonBaseMoney;
			for (sbyte type = 0; type < 6; type += 1)
			{
				int poison = *(ref poisons.Items.FixedElementField + (IntPtr)type * 4);
				int poisonLevel = (int)PoisonsAndLevels.CalcPoisonedLevel(poison);
				bool flag = poisonLevel > 0;
				if (flag)
				{
					costMoney += (int)GlobalConfig.Instance.HealPoisonExtraMoney[poisonLevel - 1];
				}
			}
			return costMoney * (int)GlobalConfig.Instance.HealMoneyPercent[(int)doctorBehaviorType] / 100;
		}

		// Token: 0x06006445 RID: 25669 RVA: 0x0038EF8C File Offset: 0x0038D18C
		public unsafe static int GetHealPoisonCostSpiritualDebt(PoisonInts poisons)
		{
			int cost = 0;
			for (sbyte type = 0; type < 6; type += 1)
			{
				int poison = *(ref poisons.Items.FixedElementField + (IntPtr)type * 4);
				int poisonLevel = (int)PoisonsAndLevels.CalcPoisonedLevel(poison);
				bool flag = poisonLevel > 0;
				if (flag)
				{
					cost += (int)GlobalConfig.Instance.HealPoisonExtraSpiritualDebt[poisonLevel - 1];
				}
			}
			return cost;
		}

		// Token: 0x06006446 RID: 25670 RVA: 0x0038EFED File Offset: 0x0038D1ED
		public static int GetHealQiDisorderCostHerb(short qiDisorder)
		{
			return (int)GlobalConfig.Instance.HealQiDisorderHerb[(int)DisorderLevelOfQi.GetDisorderLevelOfQi(qiDisorder)];
		}

		// Token: 0x06006447 RID: 25671 RVA: 0x0038F000 File Offset: 0x0038D200
		public static int GetHealQiDisorderCostMoney(short qiDisorder, sbyte doctorBehaviorType)
		{
			return (int)(GlobalConfig.Instance.HealQiDisorderMoney[(int)DisorderLevelOfQi.GetDisorderLevelOfQi(qiDisorder)] * GlobalConfig.Instance.HealMoneyPercent[(int)doctorBehaviorType]);
		}

		// Token: 0x06006448 RID: 25672 RVA: 0x0038F020 File Offset: 0x0038D220
		public static int GetHealQiDisorderCostSpiritualDebt(short qiDisorder)
		{
			return (int)GlobalConfig.Instance.HealQiDisorderCostSpiritualDebt[(int)DisorderLevelOfQi.GetDisorderLevelOfQi(qiDisorder)];
		}

		// Token: 0x06006449 RID: 25673 RVA: 0x0038F034 File Offset: 0x0038D234
		public static int GetHealHealthCostHerb(EHealthType healthType)
		{
			int index = healthType.ToCommonIndex();
			return (int)GlobalConfig.Instance.HealHealthHerb.GetOrDefault(index);
		}

		// Token: 0x0600644A RID: 25674 RVA: 0x0038F060 File Offset: 0x0038D260
		public static int GetHealHealthCostMoney(EHealthType healthType, sbyte doctorBehaviorType)
		{
			int index = healthType.ToCommonIndex();
			CValuePercent behaviorPercent = (int)GlobalConfig.Instance.HealMoneyPercent[(int)doctorBehaviorType];
			return (int)GlobalConfig.Instance.HealHealthMoney.GetOrDefault(index) * behaviorPercent;
		}

		// Token: 0x0600644B RID: 25675 RVA: 0x0038F0A4 File Offset: 0x0038D2A4
		public static int GetHealHealthCostSpiritualDebt(EHealthType healthType)
		{
			int index = healthType.ToCommonIndex();
			return (int)GlobalConfig.Instance.HealHealthCostSpiritualDebt.GetOrDefault(index);
		}

		// Token: 0x0600644C RID: 25676 RVA: 0x0038F0D0 File Offset: 0x0038D2D0
		public sbyte HealInjuryInCombat(DataContext context, CombatCharacter patient, CombatCharacter doctor, bool canHealOld = true)
		{
			int needHerb = CombatDomain.GetHealInjuryCostHerb(patient.GetInjuries());
			int hasHerb = doctor.GetCharacter().GetResource(5);
			bool flag = needHerb > hasHerb;
			if (flag)
			{
				this.ShowSpecialEffectTips(patient.GetId(), 1458, 0);
			}
			Dictionary<int, int> changedInnerDamageValue = ObjectPool<Dictionary<int, int>>.Instance.Get();
			Dictionary<int, int> changedOuterDamageValue = ObjectPool<Dictionary<int, int>>.Instance.Get();
			changedInnerDamageValue.Clear();
			changedOuterDamageValue.Clear();
			int allHealMarkCount;
			int num;
			int num2;
			Injuries newInjuries = this.HealInjury(patient.GetId(), doctor.GetCharacter(), out allHealMarkCount, out num, out num2, canHealOld, false, true, changedInnerDamageValue, changedOuterDamageValue, false);
			doctor.GetCharacter().ChangeResource(context, 5, -Math.Min(needHerb, hasHerb));
			bool flag2 = allHealMarkCount > 0;
			if (flag2)
			{
				this.SetInjuries(context, patient, newInjuries, true, true);
			}
			bool flag3 = changedInnerDamageValue.Count > 0;
			if (flag3)
			{
				int[] innerDamageValue = patient.GetInnerDamageValue();
				foreach (KeyValuePair<int, int> keyValuePair in changedInnerDamageValue)
				{
					keyValuePair.Deconstruct(out num2, out num);
					int bodyPart = num2;
					int value = num;
					innerDamageValue[bodyPart] = value;
				}
				patient.SetInnerDamageValue(innerDamageValue, context);
			}
			bool flag4 = changedOuterDamageValue.Count > 0;
			if (flag4)
			{
				int[] outerDamageValue = patient.GetOuterDamageValue();
				foreach (KeyValuePair<int, int> keyValuePair in changedOuterDamageValue)
				{
					keyValuePair.Deconstruct(out num, out num2);
					int bodyPart2 = num;
					int value2 = num2;
					outerDamageValue[bodyPart2] = value2;
				}
				patient.SetOuterDamageValue(outerDamageValue, context);
			}
			Events.RaiseHealedInjury(context, doctor.GetId(), patient.GetId(), patient.IsAlly, (sbyte)allHealMarkCount);
			return (sbyte)allHealMarkCount;
		}

		// Token: 0x0600644D RID: 25677 RVA: 0x0038F2A0 File Offset: 0x0038D4A0
		public unsafe sbyte HealPoisonInCombat(DataContext context, CombatCharacter patient, CombatCharacter doctor, bool canHealOld = true)
		{
			int needHerb = CombatDomain.GetHealPoisonCostHerb(*patient.GetPoison());
			int hasHerb = doctor.GetCharacter().GetResource(5);
			bool flag = needHerb > hasHerb;
			if (flag)
			{
				this.ShowSpecialEffectTips(patient.GetId(), 1460, 0);
			}
			int healMarkCount;
			int num;
			int num2;
			PoisonInts newPoison = this.HealPoison(patient.GetId(), doctor.GetCharacter(), out healMarkCount, out num, out num2, canHealOld, false, true, false);
			doctor.GetCharacter().ChangeResource(context, 5, -Math.Min(needHerb, hasHerb));
			this.SetPoisons(context, patient, newPoison, true);
			Events.RaiseHealedPoison(context, doctor.GetId(), patient.GetId(), patient.IsAlly, (sbyte)healMarkCount);
			return (sbyte)healMarkCount;
		}

		// Token: 0x0600644E RID: 25678 RVA: 0x0038F34C File Offset: 0x0038D54C
		public Injuries HealInjury(int patientId, GameData.Domains.Character.Character doctor, bool isExpensiveHeal = false)
		{
			int num;
			int num2;
			int num3;
			return this.HealInjury(patientId, doctor, out num, out num2, out num3, true, false, false, null, null, isExpensiveHeal);
		}

		// Token: 0x0600644F RID: 25679 RVA: 0x0038F374 File Offset: 0x0038D574
		public Injuries HealInjury(int patientId, GameData.Domains.Character.Character doctor, out int allHealMarkCount, out int maxHealMarkCount, out int maxRequireAttainment, bool canHealOld = true, bool getCost = false, bool checkHerb = false, Dictionary<int, int> changedInnerDamageValue = null, Dictionary<int, int> changedOuterDamageValue = null, bool isExpensiveHeal = false)
		{
			CombatDomain.<>c__DisplayClass408_0 CS$<>8__locals1;
			CS$<>8__locals1.doctor = doctor;
			CS$<>8__locals1.getCost = getCost;
			CS$<>8__locals1.canHealOld = canHealOld;
			CS$<>8__locals1.changedInnerDamageValue = changedInnerDamageValue;
			CS$<>8__locals1.changedOuterDamageValue = changedOuterDamageValue;
			this.HealInjuryCalcOriginal(patientId, out CS$<>8__locals1.injuries, out CS$<>8__locals1.oldInjuries, out CS$<>8__locals1.damageSteps);
			this.HealInjuryCalcExtraAddPercent(patientId, CS$<>8__locals1.doctor, checkHerb, CS$<>8__locals1.injuries, out CS$<>8__locals1.extraBuffAddPercent, out CS$<>8__locals1.extraInnerDebuffAddPercent, out CS$<>8__locals1.extraOuterDebuffAddPercent);
			CS$<>8__locals1.inCombat = (this._combatCharacterDict.TryGetValue(patientId, out CS$<>8__locals1.patient) && this.IsInCombat());
			CS$<>8__locals1.doctorAttainment = CS$<>8__locals1.doctor.CalcHealAttainment(EHealActionType.Healing);
			if (isExpensiveHeal)
			{
				CS$<>8__locals1.doctorAttainment *= 2;
			}
			allHealMarkCount = 0;
			maxHealMarkCount = 0;
			maxRequireAttainment = 0;
			for (sbyte bodyPart = 0; bodyPart < 7; bodyPart += 1)
			{
				CombatDomain.<HealInjury>g__HealPartInjury|408_0(bodyPart, true, ref allHealMarkCount, ref maxHealMarkCount, ref maxRequireAttainment, ref CS$<>8__locals1);
				CombatDomain.<HealInjury>g__HealPartInjury|408_0(bodyPart, false, ref allHealMarkCount, ref maxHealMarkCount, ref maxRequireAttainment, ref CS$<>8__locals1);
			}
			return CS$<>8__locals1.injuries;
		}

		// Token: 0x06006450 RID: 25680 RVA: 0x0038F48C File Offset: 0x0038D68C
		private void HealInjuryCalcOriginal(int patientId, out Injuries injuries, out Injuries oldInjuries, out DamageStepCollection damageSteps)
		{
			bool flag = this.IsCharInCombat(patientId, true);
			if (flag)
			{
				CombatCharacter combatChar = this._combatCharacterDict[patientId];
				injuries = combatChar.GetInjuries();
				oldInjuries = combatChar.GetOldInjuries();
				damageSteps = combatChar.GetDamageStepCollection();
			}
			else
			{
				GameData.Domains.Character.Character character = DomainManager.Character.GetElement_Objects(patientId);
				injuries = (oldInjuries = character.GetInjuries());
				damageSteps = this.GetDamageStepCollection(patientId);
			}
		}

		// Token: 0x06006451 RID: 25681 RVA: 0x0038F508 File Offset: 0x0038D708
		private void HealInjuryCalcExtraAddPercent(int patientId, GameData.Domains.Character.Character doctor, bool checkHerb, Injuries injuries, out int extraBuffAddPercent, out int extraInnerDebuffAddPercent, out int extraOuterDebuffAddPercent)
		{
			extraBuffAddPercent = 0;
			extraInnerDebuffAddPercent = 0;
			extraOuterDebuffAddPercent = 0;
			bool flag = this.IsCharInCombat(patientId, true);
			if (flag)
			{
				CombatCharacter doctorChar = this._combatCharacterDict[doctor.GetId()];
				bool flag2 = doctorChar.ExecutingTeammateCommandImplement == ETeammateCommandImplement.HealInjury;
				if (flag2)
				{
					extraBuffAddPercent += DomainManager.SpecialEffect.GetModifyValue(patientId, 184, EDataModifyType.Add, 6, -1, -1, EDataSumType.All);
				}
			}
			foreach (SolarTermItem solarTerm in doctor.GetInvokedSolarTerm())
			{
				bool innerHealingBuff = solarTerm.InnerHealingBuff;
				if (innerHealingBuff)
				{
					extraInnerDebuffAddPercent += doctor.GetSolarTermValue((int)GlobalConfig.Instance.SolarTermAddHealInnerInjury);
				}
				bool outerHealingBuff = solarTerm.OuterHealingBuff;
				if (outerHealingBuff)
				{
					extraOuterDebuffAddPercent += doctor.GetSolarTermValue((int)GlobalConfig.Instance.SolarTermAddHealOuterInjury);
				}
			}
			bool flag3 = !checkHerb;
			if (!flag3)
			{
				int needHerb = CombatDomain.GetHealInjuryCostHerb(injuries);
				int hasHerb = doctor.GetResource(5);
				bool flag4 = needHerb <= hasHerb;
				if (!flag4)
				{
					int debuffAddPercent = -(int)CValuePercent.Parse(needHerb - hasHerb, needHerb);
					extraInnerDebuffAddPercent += debuffAddPercent;
					extraOuterDebuffAddPercent += debuffAddPercent;
				}
			}
		}

		// Token: 0x06006452 RID: 25682 RVA: 0x0038F648 File Offset: 0x0038D848
		public PoisonInts HealPoison(int patientId, GameData.Domains.Character.Character doctor, bool isExpensiveHeal = false)
		{
			int num;
			int num2;
			int num3;
			return this.HealPoison(patientId, doctor, out num, out num2, out num3, true, false, false, isExpensiveHeal);
		}

		// Token: 0x06006453 RID: 25683 RVA: 0x0038F66C File Offset: 0x0038D86C
		public unsafe PoisonInts HealPoison(int patientId, GameData.Domains.Character.Character doctor, out int healMarkCount, out int healPoisonValue, out int maxRequireAttainment, bool canHealOld = true, bool getCost = false, bool checkHerb = false, bool isExpensiveHeal = false)
		{
			PoisonInts poisons;
			PoisonInts oldPoisons;
			this.HealPoisonCalcOriginal(patientId, out poisons, out oldPoisons);
			int extraBuffAddPercent;
			int extraDebuffAddPercent;
			this.HealPoisonCalcExtraAddPercent(patientId, doctor, checkHerb, poisons, out extraBuffAddPercent, out extraDebuffAddPercent);
			int doctorAttainment = doctor.CalcHealAttainment(EHealActionType.Detox);
			if (isExpensiveHeal)
			{
				doctorAttainment *= 2;
			}
			healMarkCount = 0;
			healPoisonValue = 0;
			maxRequireAttainment = 0;
			for (sbyte type = 0; type < 6; type += 1)
			{
				int poison = *poisons[(int)type];
				bool flag = poison <= 0;
				if (!flag)
				{
					sbyte poisonLevel = PoisonsAndLevels.CalcPoisonedLevel(poison);
					int requireAttainment;
					int healValue = CFormula.CalcHealPoisonValue(doctorAttainment, (int)poisonLevel, out requireAttainment);
					maxRequireAttainment = Math.Max(maxRequireAttainment, requireAttainment);
					bool flag2 = healValue <= 0;
					if (!flag2)
					{
						int typeExtraDebuffAddPercent = extraDebuffAddPercent;
						foreach (SolarTermItem solarTerm in doctor.GetInvokedSolarTerm())
						{
							bool flag3 = solarTerm.DetoxBuffType == type;
							if (flag3)
							{
								typeExtraDebuffAddPercent += doctor.GetSolarTermValue((int)GlobalConfig.Instance.SolarTermAddHealPoison);
							}
						}
						healValue = DomainManager.SpecialEffect.ModifyValue(doctor.GetId(), 123, healValue, getCost ? 1 : 0, -1, -1, 0, typeExtraDebuffAddPercent, 0, 0);
						int oldPoison = *oldPoisons[(int)type];
						int newPoison = poison - oldPoison;
						bool flag4 = healValue < newPoison;
						if (flag4)
						{
							healValue = Math.Min(DomainManager.SpecialEffect.ModifyValue(doctor.GetId(), 122, healValue, getCost ? 1 : 0, -1, -1, 0, extraBuffAddPercent, 0, 0), newPoison);
						}
						healValue = this.ApplyReducePoisonEffect(patientId, type, healValue, getCost);
						healValue = Math.Min(healValue, canHealOld ? poison : (poison - oldPoison));
						*poisons[(int)type] -= healValue;
						healMarkCount += (int)(poisonLevel - PoisonsAndLevels.CalcPoisonedLevel(poison - healValue));
						healPoisonValue += healValue;
					}
				}
			}
			return poisons;
		}

		// Token: 0x06006454 RID: 25684 RVA: 0x0038F85C File Offset: 0x0038DA5C
		private unsafe void HealPoisonCalcOriginal(int patientId, out PoisonInts poisons, out PoisonInts oldPoisons)
		{
			bool flag = this.IsCharInCombat(patientId, true);
			if (flag)
			{
				CombatCharacter combatChar = this._combatCharacterDict[patientId];
				poisons = *combatChar.GetPoison();
				oldPoisons = *combatChar.GetOldPoison();
			}
			else
			{
				GameData.Domains.Character.Character character = DomainManager.Character.GetElement_Objects(patientId);
				poisons = (oldPoisons = *character.GetPoisoned());
			}
		}

		// Token: 0x06006455 RID: 25685 RVA: 0x0038F8D4 File Offset: 0x0038DAD4
		private void HealPoisonCalcExtraAddPercent(int patientId, GameData.Domains.Character.Character doctor, bool checkHerb, PoisonInts poisons, out int extraBuffAddPercent, out int extraDebuffAddPercent)
		{
			extraBuffAddPercent = 0;
			extraDebuffAddPercent = 0;
			bool flag = this.IsCharInCombat(patientId, true);
			if (flag)
			{
				CombatCharacter doctorChar = this._combatCharacterDict[doctor.GetId()];
				bool flag2 = doctorChar.ExecutingTeammateCommandImplement == ETeammateCommandImplement.HealPoison;
				if (flag2)
				{
					extraBuffAddPercent += DomainManager.SpecialEffect.GetModifyValue(patientId, 184, EDataModifyType.Add, 7, -1, -1, EDataSumType.All);
				}
			}
			bool flag3 = !checkHerb;
			if (!flag3)
			{
				int needHerb = CombatDomain.GetHealPoisonCostHerb(poisons);
				int hasHerb = doctor.GetResource(5);
				bool flag4 = needHerb > hasHerb;
				if (flag4)
				{
					extraDebuffAddPercent = -(int)CValuePercent.Parse(needHerb - hasHerb, needHerb);
				}
			}
		}

		// Token: 0x06006456 RID: 25686 RVA: 0x0038F970 File Offset: 0x0038DB70
		public short HealQiDisorder(int patientId, GameData.Domains.Character.Character doctor, bool isExpensiveHeal = false)
		{
			short qiDisorder = DomainManager.Character.GetElement_Objects(patientId).GetDisorderOfQi();
			int doctorAttainment = doctor.CalcHealAttainment(EHealActionType.Breathing);
			if (isExpensiveHeal)
			{
				doctorAttainment *= 2;
			}
			sbyte qiDisorderLevel = DisorderLevelOfQi.GetDisorderLevelOfQi(qiDisorder);
			int healValue = CFormula.CalcHealQiDisorderValue(doctorAttainment, qiDisorderLevel);
			return (short)Math.Clamp((int)qiDisorder - healValue, (int)DisorderLevelOfQi.MinValue, (int)DisorderLevelOfQi.MaxValue);
		}

		// Token: 0x06006457 RID: 25687 RVA: 0x0038F9CC File Offset: 0x0038DBCC
		public short HealHealth(int patientId, GameData.Domains.Character.Character doctor, bool isExpensiveHeal = false)
		{
			GameData.Domains.Character.Character patient = DomainManager.Character.GetElement_Objects(patientId);
			short health = patient.GetHealth();
			int doctorAttainment = doctor.CalcHealAttainment(EHealActionType.Recover);
			if (isExpensiveHeal)
			{
				doctorAttainment *= 2;
			}
			EHealthType healthType = patient.GetHealthType();
			int healValue = CFormula.CalcHealHealthValue(doctorAttainment, healthType);
			return (short)Math.Clamp((int)health + healValue, 0, Math.Max((int)patient.GetLeftMaxHealth(false), 0));
		}

		// Token: 0x06006458 RID: 25688 RVA: 0x0038FA2E File Offset: 0x0038DC2E
		private static bool IsSwordFragment(ItemKey itemKey)
		{
			return itemKey.ItemType == 12 && GameData.Domains.Combat.SharedConstValue.SwordFragment2BossId.ContainsKey(itemKey.TemplateId);
		}

		// Token: 0x06006459 RID: 25689 RVA: 0x0038FA50 File Offset: 0x0038DC50
		[DomainMethod]
		public List<short> RequestSwordFragmentSkillIds()
		{
			GameData.Domains.Character.Character taiwuChar = DomainManager.Taiwu.GetTaiwu();
			List<short> result = null;
			foreach (ItemKey itemKey in taiwuChar.GetInventory().Items.Keys.Where(new Func<ItemKey, bool>(CombatDomain.IsSwordFragment)))
			{
				short skillId = DomainManager.Item.GetSwordFragmentCurrSkill(itemKey);
				bool flag = skillId < 0;
				if (!flag)
				{
					if (result == null)
					{
						result = new List<short>();
					}
					result.Add(skillId);
				}
			}
			return result;
		}

		// Token: 0x0600645A RID: 25690 RVA: 0x0038FAF8 File Offset: 0x0038DCF8
		[DomainMethod]
		public List<ItemDisplayData> RequestValidItemsInCombat(int charId)
		{
			bool flag = !this.IsCharInCombat(charId, true);
			List<ItemDisplayData> result2;
			if (flag)
			{
				result2 = null;
			}
			else
			{
				CombatCharacter combatChar = this.GetElement_CombatCharacterDict(charId);
				List<ItemDisplayData> result = null;
				foreach (ItemKeyAndCount itemKeyAndCount in combatChar.GetValidItemAndCounts())
				{
					ItemKey itemKey2;
					int num;
					itemKeyAndCount.Deconstruct(out itemKey2, out num);
					ItemKey itemKey = itemKey2;
					int count = num;
					if (result == null)
					{
						result = new List<ItemDisplayData>();
					}
					ItemDisplayData data = itemKey.IsValid() ? DomainManager.Item.GetItemDisplayData(itemKey, charId) : new ItemDisplayData(itemKey.ItemType, itemKey.TemplateId);
					data.Amount = count;
					result.Add(data);
				}
				result2 = result;
			}
			return result2;
		}

		// Token: 0x0600645B RID: 25691 RVA: 0x0038FBCC File Offset: 0x0038DDCC
		[DomainMethod]
		public void UseItem(DataContext context, ItemKey itemKey, sbyte useType = -1, bool isAlly = true, List<sbyte> targetBodyParts = null)
		{
			bool flag = !this.CanAcceptCommand();
			if (!flag)
			{
				CombatCharacter combatChar = isAlly ? this._selfChar : this._enemyChar;
				bool flag2 = !combatChar.GetCanUseItem();
				if (!flag2)
				{
					bool flag3 = !combatChar.GetValidItems().Contains(itemKey);
					if (!flag3)
					{
						int costWisdom = itemKey.GetConsumedFeatureMedals();
						bool flag4 = costWisdom > (int)(isAlly ? this._selfTeamWisdomCount : this._enemyTeamWisdomCount);
						if (!flag4)
						{
							bool needUpdate = !combatChar.HasDoingOrReserveCommand();
							combatChar.SetNeedUseItem(context, itemKey);
							combatChar.ItemUseType = useType;
							combatChar.ItemTargetBodyParts = targetBodyParts;
							combatChar.MoveData.ResetJumpState(context, true);
							bool flag5 = needUpdate;
							if (flag5)
							{
								this.UpdateAllCommandAvailability(context, combatChar);
							}
							else
							{
								this.UpdateCanUseItem(context, combatChar);
							}
						}
					}
				}
			}
		}

		// Token: 0x0600645C RID: 25692 RVA: 0x0038FC9C File Offset: 0x0038DE9C
		[DomainMethod]
		public void RepairItem(DataContext context, ItemKey toolKey, ItemKey targetKey, bool isAlly = true)
		{
			bool flag = !this.CanAcceptCommand();
			if (!flag)
			{
				CombatCharacter combatChar = isAlly ? this._selfChar : this._enemyChar;
				bool flag2 = !combatChar.GetCanUseItem();
				if (!flag2)
				{
					bool needUpdate = !combatChar.HasDoingOrReserveCommand();
					combatChar.SetNeedUseItem(context, toolKey);
					combatChar.NeedRepairItem = targetKey;
					combatChar.MoveData.ResetJumpState(context, true);
					bool flag3 = needUpdate;
					if (flag3)
					{
						this.UpdateAllCommandAvailability(context, combatChar);
					}
					else
					{
						this.UpdateCanUseItem(context, combatChar);
					}
				}
			}
		}

		// Token: 0x0600645D RID: 25693 RVA: 0x0038FD20 File Offset: 0x0038DF20
		[DomainMethod]
		public void UseSpecialItem(DataContext context, sbyte itemType, short templateId, bool isAlly = true)
		{
			bool flag = !this.CanAcceptCommand();
			if (!flag)
			{
				CombatCharacter combatChar = isAlly ? this._selfChar : this._enemyChar;
				ItemKey? specialItemKey = null;
				foreach (ItemKey itemKey in combatChar.GetValidItems())
				{
					bool flag2 = itemKey.TemplateEquals(itemType, templateId);
					if (flag2)
					{
						specialItemKey = new ItemKey?(itemKey);
					}
				}
				bool flag3 = specialItemKey != null;
				if (flag3)
				{
					this.UseItem(context, specialItemKey.Value, -1, true, null);
				}
			}
		}

		// Token: 0x0600645E RID: 25694 RVA: 0x0038FDD4 File Offset: 0x0038DFD4
		public bool IsInfectedCombat()
		{
			GameData.Domains.Character.Character enemyChar = this.GetMainCharacter(false).GetCharacter();
			bool flag = enemyChar.GetCreatingType() != 1;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				bool flag2 = !enemyChar.GetFeatureIds().Contains(218);
				result = (!flag2 && this.CombatConfig.TemplateId == 193);
			}
			return result;
		}

		// Token: 0x0600645F RID: 25695 RVA: 0x0038FE34 File Offset: 0x0038E034
		public void ChangeWisdom(DataContext context, bool isAlly, int delta)
		{
			short value = isAlly ? this._selfTeamWisdomCount : this._enemyTeamWisdomCount;
			short newValue = (short)Math.Clamp((int)value + delta, 0, 32767);
			bool flag = newValue == value;
			if (!flag)
			{
				if (isAlly)
				{
					this.SetSelfTeamWisdomCount(newValue, context);
				}
				else
				{
					this.SetEnemyTeamWisdomCount(newValue, context);
				}
			}
		}

		// Token: 0x06006460 RID: 25696 RVA: 0x0038FE88 File Offset: 0x0038E088
		public bool CostWisdom(DataContext context, bool isAlly, int costValue)
		{
			short value = isAlly ? this._selfTeamWisdomCount : this._enemyTeamWisdomCount;
			bool flag = (int)value < costValue || costValue <= 0;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				short newValue = (short)Math.Clamp((int)value - costValue, 0, 32767);
				if (isAlly)
				{
					this.SetSelfTeamWisdomCount(newValue, context);
				}
				else
				{
					this.SetEnemyTeamWisdomCount(newValue, context);
				}
				Events.RaiseWisdomCosted(context, isAlly, costValue);
				result = true;
			}
			return result;
		}

		// Token: 0x06006461 RID: 25697 RVA: 0x0038FEF8 File Offset: 0x0038E0F8
		public void InitEquipmentDurability()
		{
			this.EquipmentOldDurability.Clear();
			foreach (CombatCharacter combatChar in this._combatCharacterDict.Values)
			{
				foreach (ItemKey key in from x in combatChar.GetCharacter().GetEquipment()
				where x.IsValid()
				select x)
				{
					this.EquipmentOldDurability.Add(key, (int)DomainManager.Item.GetBaseItem(key).GetCurrDurability());
				}
			}
		}

		// Token: 0x06006462 RID: 25698 RVA: 0x0038FFD8 File Offset: 0x0038E1D8
		public void EnsureOldDurability(ItemKey key)
		{
			bool flag = !key.IsValid();
			if (!flag)
			{
				short currDurability = DomainManager.Item.GetBaseItem(key).GetCurrDurability();
				bool flag2 = this.EquipmentOldDurability.ContainsKey(key);
				if (flag2)
				{
					this.EquipmentOldDurability[key] = Math.Max(this.EquipmentOldDurability[key], (int)currDurability);
				}
			}
		}

		// Token: 0x06006463 RID: 25699 RVA: 0x00390038 File Offset: 0x0038E238
		private void UpdateCanUseItem(DataContext context, CombatCharacter character)
		{
			bool flag = !character.GetCanUseItem();
			if (flag)
			{
				character.SetCanUseItem(true, context);
				bool isAlly = character.IsAlly;
				if (isAlly)
				{
					this.UpdateShowUseSpecialMisc(context);
				}
			}
		}

		// Token: 0x06006464 RID: 25700 RVA: 0x00390070 File Offset: 0x0038E270
		public void ClearShowUseSpecialMisc(DataContext context)
		{
			this.SetShowUseGoldenWire(0, context);
		}

		// Token: 0x06006465 RID: 25701 RVA: 0x00390081 File Offset: 0x0038E281
		public void UpdateShowUseSpecialMisc(DataContext context)
		{
			this.UpdateShowUseGoldenWire(context);
		}

		// Token: 0x06006466 RID: 25702 RVA: 0x0039008C File Offset: 0x0038E28C
		private bool CalcShowUseSpecialMiscCommon(short miscTemplateId)
		{
			bool flag = !this.IsMainCharacter(this._selfChar) || !this.IsMainCharacter(this._enemyChar);
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				bool flag2 = !this._selfChar.GetCanUseItem();
				if (flag2)
				{
					result = false;
				}
				else
				{
					bool flag3 = this._selfChar.GetCharacter().GetInventory().GetInventoryItemCount(12, miscTemplateId) == 0;
					if (flag3)
					{
						result = false;
					}
					else
					{
						bool flag4 = this._enemyChar.TeammateBeforeMainChar >= 0;
						if (flag4)
						{
							result = false;
						}
						else
						{
							MiscItem config = Config.Misc.Instance[miscTemplateId];
							List<short> requireCombatConfig = config.RequireCombatConfig;
							bool flag5 = requireCombatConfig != null && requireCombatConfig.Count > 0 && !config.RequireCombatConfig.Contains(this.CombatConfig.TemplateId);
							result = (!flag5 && this._currentDistance <= (short)config.MaxUseDistance);
						}
					}
				}
			}
			return result;
		}

		// Token: 0x06006467 RID: 25703 RVA: 0x0039017C File Offset: 0x0038E37C
		private void UpdateShowUseGoldenWire(DataContext context)
		{
			bool flag = this._selfChar == null || this._enemyChar == null;
			if (!flag)
			{
				short miscTemplateId = 285;
				int chance = this.CalcShowUseSpecialMiscCommon(miscTemplateId) ? this.CalcRopeHitOdds((int)Config.Misc.Instance[miscTemplateId].Grade) : 0;
				bool flag2 = this._showUseGoldenWire != chance;
				if (flag2)
				{
					this.SetShowUseGoldenWire(chance, context);
				}
			}
		}

		// Token: 0x06006468 RID: 25704 RVA: 0x003901F4 File Offset: 0x0038E3F4
		private static int CalcCaptureRateBonus(ItemKey equipKey)
		{
			return (int)((equipKey.ItemType == 2) ? Config.Accessory.Instance[equipKey.TemplateId].BaseCaptureRateBonus : DomainManager.Item.GetElement_Carriers(equipKey.Id).GetCaptureRateBonus());
		}

		// Token: 0x06006469 RID: 25705 RVA: 0x0039023C File Offset: 0x0038E43C
		public bool CheckRopeHit(IRandomSource random, int ropeGrade)
		{
			int hitOdds = this.CalcRopeHitOdds(ropeGrade);
			return random.CheckPercentProb(hitOdds);
		}

		// Token: 0x0600646A RID: 25706 RVA: 0x00390260 File Offset: 0x0038E460
		private int CalcRopeHitOdds(int ropeGrade)
		{
			CombatCharacter enemyChar = this.GetCombatCharacter(false, true);
			int markCount = enemyChar.GetDefeatMarkCollection().GetTotalCount();
			sbyte requireMarkCount = CFormula.CalcRopeRequireMinMarkCount((CombatType)this._combatType);
			bool flag = !this.IsMainCharacter(enemyChar) || enemyChar.TeammateBeforeMainChar >= 0 || enemyChar.BossConfig != null;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				GameData.Domains.Character.Character enemyCharObj = enemyChar.GetCharacter();
				bool flag2 = !Config.Character.Instance[enemyCharObj.GetTemplateId()].CanBeKidnapped || this.CombatConfig.CaptureRate <= 0;
				if (flag2)
				{
					result = 0;
				}
				else
				{
					bool flag3 = this._selfChar.GetCharacter().GetConsummateLevel() >= enemyChar.GetCharacter().GetConsummateLevel() + 6;
					if (flag3)
					{
						result = 100;
					}
					else
					{
						bool flag4 = markCount < (int)requireMarkCount;
						if (flag4)
						{
							result = 0;
						}
						else
						{
							HitOrAvoidInts hitValues = this._selfChar.GetCharacter().GetHitValues();
							HitOrAvoidInts avoidValues = enemyChar.GetCharacter().GetAvoidValues();
							ItemKey[] equipments = this._selfChar.GetCharacter().GetEquipment();
							int totalHit = 0;
							int totalAvoid = 0;
							int bonus = 0;
							for (sbyte hitType = 0; hitType < 4; hitType += 1)
							{
								totalHit += hitValues[(int)hitType];
								totalAvoid += avoidValues[(int)hitType];
							}
							totalAvoid = Math.Max(totalAvoid, 1);
							for (int i = 0; i < CombatDomain.AddCaptureRateEquipSlot.Length; i++)
							{
								ItemKey equipKey = equipments[(int)CombatDomain.AddCaptureRateEquipSlot[i]];
								bool flag5 = !equipKey.IsValid();
								if (!flag5)
								{
									EquipmentBase equipment = DomainManager.Item.GetBaseEquipment(equipKey);
									bool flag6 = equipment.GetCurrDurability() <= 0;
									if (!flag6)
									{
										bonus += CombatDomain.CalcCaptureRateBonus(equipKey);
									}
								}
							}
							bool flag7 = ropeGrade >= 0;
							if (flag7)
							{
								bonus += (int)GlobalConfig.Instance.CaptureRatePerRopeGrade * (ropeGrade + 1);
							}
							sbyte baseHitOdds = CFormula.CalcRopeBaseHitOdds((CombatType)this._combatType);
							int hitOdds = CFormula.FormulaCalcRopeHitOdds(baseHitOdds, requireMarkCount, totalHit, totalAvoid, bonus, markCount);
							bool flag8 = ropeGrade >= 0;
							if (flag8)
							{
								hitOdds = hitOdds * (int)this.CombatConfig.CaptureRate / 100;
							}
							result = hitOdds;
						}
					}
				}
			}
			return result;
		}

		// Token: 0x0600646B RID: 25707 RVA: 0x00390490 File Offset: 0x0038E690
		public unsafe static bool CheckRopeHitOutOfCombat(IRandomSource random, GameData.Domains.Character.Character useChar, GameData.Domains.Character.Character targetChar, sbyte combatType, bool useMaxMarkCount = true, int ropeGrade = -1)
		{
			int markCount = useMaxMarkCount ? ((int)GlobalConfig.NeedDefeatMarkCount[(int)combatType]) : CombatDomain.GetDefeatMarksCountOutOfCombat(targetChar);
			sbyte requireMarkCount = CFormula.CalcRopeRequireMinMarkCount((CombatType)combatType);
			bool flag = markCount < (int)requireMarkCount;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				bool flag2 = !Config.Character.Instance[targetChar.GetTemplateId()].CanBeKidnapped;
				if (flag2)
				{
					result = false;
				}
				else
				{
					HitOrAvoidInts hitValues = useChar.GetHitValues();
					HitOrAvoidInts avoidValues = targetChar.GetAvoidValues();
					ItemKey[] equipments = useChar.GetEquipment();
					sbyte baseHitOdds = CFormula.CalcRopeBaseHitOdds((CombatType)combatType);
					int totalHit = 0;
					int totalAvoid = 0;
					int bonus = 0;
					for (sbyte hitType = 0; hitType < 4; hitType += 1)
					{
						totalHit += *(ref hitValues.Items.FixedElementField + (IntPtr)hitType * 4);
						totalAvoid += *(ref avoidValues.Items.FixedElementField + (IntPtr)hitType * 4);
					}
					totalAvoid = Math.Max(totalAvoid, 1);
					for (int i = 0; i < CombatDomain.AddCaptureRateEquipSlot.Length; i++)
					{
						ItemKey equipKey = equipments[(int)CombatDomain.AddCaptureRateEquipSlot[i]];
						bool flag3 = !equipKey.IsValid();
						if (!flag3)
						{
							EquipmentBase equipment = DomainManager.Item.GetBaseEquipment(equipKey);
							bool flag4 = equipment.GetCurrDurability() <= 0;
							if (!flag4)
							{
								bonus += CombatDomain.CalcCaptureRateBonus(equipKey);
							}
						}
					}
					bool flag5 = ropeGrade >= 0;
					if (flag5)
					{
						bonus += (int)GlobalConfig.Instance.CaptureRatePerRopeGrade * (ropeGrade + 1);
					}
					int hitOdds = CFormula.FormulaCalcRopeHitOdds(baseHitOdds, requireMarkCount, totalHit, totalAvoid, bonus, markCount);
					result = random.CheckPercentProb(hitOdds);
				}
			}
			return result;
		}

		// Token: 0x0600646C RID: 25708 RVA: 0x0039061C File Offset: 0x0038E81C
		public void ChangeEquipmentPowerInCombat(int charId, int delta)
		{
			this.EquipmentPowerChangeInCombat[charId] = this.EquipmentPowerChangeInCombat.GetOrDefault(charId) + delta;
		}

		// Token: 0x0600646D RID: 25709 RVA: 0x0039063C File Offset: 0x0038E83C
		[DomainMethod]
		public void ChangeWeapon(DataContext context, int weaponIndex, bool isAlly = true, bool forceChange = false)
		{
			bool flag = !this.CanAcceptCommand();
			if (!flag)
			{
				CombatCharacter combatChar = isAlly ? this._selfChar : this._enemyChar;
				ItemKey itemKey = combatChar.GetWeapons()[weaponIndex];
				CombatWeaponData data;
				bool flag2 = !this.TryGetElement_WeaponDataDict(itemKey.Id, out data) || (!data.GetCanChangeTo() && !forceChange);
				if (!flag2)
				{
					CombatCharacterStateType stateType = combatChar.StateMachine.GetCurrentStateType();
					bool needUpdate = !combatChar.HasDoingOrReserveCommand();
					combatChar.SetNeedChangeWeaponIndex(context, weaponIndex);
					bool flag3 = stateType == CombatCharacterStateType.Idle;
					if (flag3)
					{
						this.ChangeWeapon(context, combatChar, weaponIndex, false, false);
					}
					else
					{
						bool flag4 = needUpdate;
						if (flag4)
						{
							this.UpdateAllCommandAvailability(context, combatChar);
						}
					}
				}
			}
		}

		// Token: 0x0600646E RID: 25710 RVA: 0x003906F4 File Offset: 0x0038E8F4
		[DomainMethod]
		public void NormalAttack(DataContext context, bool isAlly = true)
		{
			bool flag = !this.CanAcceptCommand();
			if (!flag)
			{
				bool flag2 = !this.CanNormalAttack(isAlly);
				if (!flag2)
				{
					CombatCharacter combatChar = isAlly ? this._selfChar : this._enemyChar;
					combatChar.SetReserveNormalAttack(true, context);
					this.UpdateAllCommandAvailability(context, combatChar);
				}
			}
		}

		// Token: 0x0600646F RID: 25711 RVA: 0x00390748 File Offset: 0x0038E948
		[DomainMethod]
		public void NormalAttackImmediate(DataContext context, bool isAlly = true)
		{
			bool flag = !this.CanAcceptCommand();
			if (!flag)
			{
				bool flag2 = !this.CanNormalAttack(isAlly);
				if (!flag2)
				{
					CombatCharacter combatChar = isAlly ? this._selfChar : this._enemyChar;
					bool flag3 = !combatChar.CanNormalAttackImmediate;
					if (!flag3)
					{
						combatChar.NeedNormalAttackImmediate = true;
						this.UpdateAllCommandAvailability(context, combatChar);
					}
				}
			}
		}

		// Token: 0x06006470 RID: 25712 RVA: 0x003907A8 File Offset: 0x0038E9A8
		[DomainMethod]
		public void UnlockAttack(DataContext context, int index, bool isAlly = true)
		{
			bool flag = !this.CanAcceptCommand();
			if (!flag)
			{
				bool flag2 = !this.CanUnlockAttack(isAlly, index);
				if (!flag2)
				{
					(isAlly ? this._selfChar : this._enemyChar).SetNeedUnlockWeaponIndex(context, index);
				}
			}
		}

		// Token: 0x06006471 RID: 25713 RVA: 0x003907F0 File Offset: 0x0038E9F0
		[DomainMethod]
		public ChangeTrickDisplayData GetChangeTrickDisplayData(bool isAlly = true)
		{
			CombatCharacter combatChar = isAlly ? this._selfChar : this._enemyChar;
			GameData.Domains.Item.Weapon weapon = DomainManager.Item.GetElement_Weapons(this.GetUsingWeaponKey(combatChar).Id);
			sbyte pointCost = weapon.GetAttackPreparePointCost();
			return new ChangeTrickDisplayData
			{
				CanChangeTrick = (combatChar.GetChangeTrickCount() > (short)pointCost),
				CostCount = pointCost + 1,
				AddHitRate = 100 + GlobalConfig.Instance.AttackChangeTrickHitValueAddPercent[(int)pointCost],
				AddBreakBlock = GlobalConfig.Instance.AttackChangeTrickCostBlockBasePercent[(int)pointCost]
			};
		}

		// Token: 0x06006472 RID: 25714 RVA: 0x00390884 File Offset: 0x0038EA84
		[DomainMethod]
		public void StartChangeTrick(DataContext context, bool isAlly = true)
		{
			bool flag = !this.CanAcceptCommand();
			if (!flag)
			{
				CombatCharacter combatChar = isAlly ? this._selfChar : this._enemyChar;
				bool flag2 = !combatChar.GetCanChangeTrick();
				if (!flag2)
				{
					combatChar.SetNeedShowChangeTrick(context, true);
					this.UpdateAllCommandAvailability(context, combatChar);
				}
			}
		}

		// Token: 0x06006473 RID: 25715 RVA: 0x003908D4 File Offset: 0x0038EAD4
		[DomainMethod]
		public void SelectChangeTrick(DataContext context, sbyte trickType, sbyte bodyPart, int flawOrAcupointType)
		{
			this._selfChar.PlayerChangeTrickType = trickType;
			this._selfChar.PlayerChangeTrickBodyPart = bodyPart;
			this.SelectChangeTrick(context, trickType, bodyPart, true, (EFlawOrAcupointType)flawOrAcupointType);
		}

		// Token: 0x06006474 RID: 25716 RVA: 0x003908FC File Offset: 0x0038EAFC
		public void SelectChangeTrick(DataContext context, sbyte trickType, sbyte bodyPart, bool isAlly = true, EFlawOrAcupointType flawOrAcupointType = EFlawOrAcupointType.None)
		{
			CombatCharacter combatChar = isAlly ? this._selfChar : this._enemyChar;
			combatChar.SetNeedShowChangeTrick(context, false);
			int costChangeTrickCount = CFormulaHelper.CalcCostChangeTrickCount(combatChar, flawOrAcupointType);
			bool flag = (int)combatChar.GetChangeTrickCount() < costChangeTrickCount;
			if (!flag)
			{
				this.ChangeChangeTrickCount(context, combatChar, -costChangeTrickCount, true);
				combatChar.NeedChangeTrickAttack = true;
				combatChar.ChangeTrickType = trickType;
				combatChar.ChangeTrickBodyPart = bodyPart;
				combatChar.ChangeTrickFlawOrAcupointType = flawOrAcupointType;
				combatChar.MoveData.ResetJumpState(context, true);
				this.UpdateAllCommandAvailability(context, combatChar);
			}
		}

		// Token: 0x06006475 RID: 25717 RVA: 0x00390980 File Offset: 0x0038EB80
		[DomainMethod]
		public void CancelChangeTrick(DataContext context, bool isAlly = true)
		{
			CombatCharacter combatChar = isAlly ? this._selfChar : this._enemyChar;
			combatChar.SetNeedShowChangeTrick(context, false);
			bool flag = combatChar.StateMachine.GetCurrentStateType() != CombatCharacterStateType.SelectChangeTrick;
			if (flag)
			{
				this.UpdateAllCommandAvailability(context, combatChar);
			}
		}

		// Token: 0x06006476 RID: 25718 RVA: 0x003909C8 File Offset: 0x0038EBC8
		[DomainMethod]
		public void ChangeTaiwuWeaponInnerRatio(DataContext context, int index, sbyte expectInnerRatio)
		{
			bool flag = !this.IsCharInCombat(DomainManager.Taiwu.GetTaiwuCharId(), true);
			if (!flag)
			{
				bool isTaiwu = this._selfChar.IsTaiwu;
				if (isTaiwu)
				{
					DomainManager.Taiwu.SetWeaponInnerRatio(context, index, expectInnerRatio);
				}
				else
				{
					this._expectRatioData.SetValue(this._selfChar.GetId(), index, expectInnerRatio);
					this.SetExpectRatioData(this._expectRatioData, context);
				}
			}
		}

		// Token: 0x06006477 RID: 25719 RVA: 0x00390A38 File Offset: 0x0038EC38
		[DomainMethod]
		public sbyte GetWeaponInnerRatio(DataContext context, ItemKey weaponKey)
		{
			int taiwuWeaponIndex = this.IsCharInCombat(this._selfTeam[0], true) ? this._combatCharacterDict[this._selfTeam[0]].GetWeapons().IndexOf(weaponKey) : -1;
			return (taiwuWeaponIndex >= 0) ? DomainManager.Taiwu.GetWeaponCurrInnerRatios()[taiwuWeaponIndex] : Config.Weapon.Instance[weaponKey.TemplateId].DefaultInnerRatio;
		}

		// Token: 0x06006478 RID: 25720 RVA: 0x00390AA4 File Offset: 0x0038ECA4
		[DomainMethod]
		public WeaponEffectDisplayData[] GetWeaponEffects(ItemKey weaponKey)
		{
			int charId = -1;
			CombatWeaponData weaponData = null;
			bool flag = this.IsInCombat();
			if (flag)
			{
				bool flag2 = this._weaponDataDict.TryGetValue(weaponKey.Id, out weaponData);
				if (flag2)
				{
					charId = weaponData.Character.GetId();
				}
			}
			SkillEffectKey effect0 = (weaponData != null) ? weaponData.GetAutoAttackEffect() : new SkillEffectKey(-1, false);
			SkillEffectKey effect = (weaponData != null) ? weaponData.GetPestleEffect() : new SkillEffectKey(-1, false);
			return new WeaponEffectDisplayData[]
			{
				new WeaponEffectDisplayData(effect0, charId),
				new WeaponEffectDisplayData(effect, charId)
			};
		}

		// Token: 0x06006479 RID: 25721 RVA: 0x00390B3C File Offset: 0x0038ED3C
		private void InitWeaponData(DataContext context)
		{
			this.ClearWeaponDataDict();
			foreach (CombatCharacter character in this._combatCharacterDict.Values)
			{
				for (int i = 0; i < 7; i++)
				{
					this.InitWeaponData(context, character, i);
				}
				this.ChangeToFirstAvailableWeapon(context, character, true);
				character.SetAnimationToLoop(this.GetIdleAni(character), context);
			}
		}

		// Token: 0x0600647A RID: 25722 RVA: 0x00390BCC File Offset: 0x0038EDCC
		public void InitWeaponData(DataContext context, CombatCharacter character, int index)
		{
			ItemKey weaponKey = character.GetWeapons()[index];
			bool flag = !weaponKey.IsValid();
			if (!flag)
			{
				List<sbyte> weaponTricks = DomainManager.Item.GetWeaponTricks(weaponKey);
				CombatWeaponData weaponData = new CombatWeaponData(weaponKey, character);
				sbyte[] trickList = weaponData.GetWeaponTricks();
				this.AddElement_WeaponDataDict(weaponKey.Id, weaponData);
				weaponData.Init(context, index);
				for (int i = 0; i < weaponTricks.Count; i++)
				{
					trickList[i] = weaponTricks[i];
				}
			}
		}

		// Token: 0x0600647B RID: 25723 RVA: 0x00390C52 File Offset: 0x0038EE52
		public void RemoveWeaponData(ItemKey weaponKey)
		{
			this.RemoveElement_WeaponDataDict(weaponKey.Id);
		}

		// Token: 0x0600647C RID: 25724 RVA: 0x00390C64 File Offset: 0x0038EE64
		public void ChangeWeapon(DataContext context, CombatCharacter character, int weaponIndex, bool init = false, bool force = false)
		{
			ItemKey[] weapons = character.GetWeapons();
			int oldWeaponIndex = character.GetUsingWeaponIndex();
			int oldWeaponKeyId = weapons.CheckIndex(oldWeaponIndex) ? weapons[oldWeaponIndex].Id : -1;
			bool flag = force && DomainManager.SpecialEffect.ModifyData(character.GetId(), -1, 181, false, oldWeaponKeyId, -1, -1);
			if (!flag)
			{
				bool flag2 = weaponIndex < 0 || !weapons[weaponIndex].IsValid();
				if (!flag2)
				{
					CombatWeaponData weaponData = character.GetWeaponData(weaponIndex);
					bool flag3 = oldWeaponIndex >= 0 && oldWeaponIndex != 3;
					if (flag3)
					{
						character.GetWeaponData(oldWeaponIndex).SetCdFrame(30000, context);
					}
					character.SetNeedChangeWeaponIndex(context, -1);
					character.SetUsingWeaponIndex(weaponIndex, context);
					character.SetWeaponTricks(weaponData.GetWeaponTricks(), context);
					character.SetChangeTrickAttack(false, context);
					character.SetReserveNormalAttack(false, context);
					bool flag4 = !init;
					if (flag4)
					{
						this.UpdateAllCommandAvailability(context, character);
						this.SetProperLoopAniAndParticle(context, character, false);
						Events.RaiseChangeWeapon(context, character.GetId(), character.IsAlly, weaponData, (oldWeaponIndex >= 0) ? character.GetWeaponData(oldWeaponIndex) : null);
					}
				}
			}
		}

		// Token: 0x0600647D RID: 25725 RVA: 0x00390D8C File Offset: 0x0038EF8C
		private void ChangeToFirstAvailableWeapon(DataContext context, CombatCharacter character, bool init = false)
		{
			ItemKey[] weapons = character.GetWeapons();
			for (int i = 0; i < weapons.Length; i++)
			{
				CombatWeaponData data;
				bool flag = this.TryGetElement_WeaponDataDict(weapons[i].Id, out data) && data.GetCanChangeTo();
				if (flag)
				{
					this.ChangeWeapon(context, character, i, init, false);
					break;
				}
			}
		}

		// Token: 0x0600647E RID: 25726 RVA: 0x00390DEC File Offset: 0x0038EFEC
		public void UpdateWeaponCanChange(DataContext context, CombatCharacter character)
		{
			ItemKey[] weapons = character.GetWeapons();
			for (int i = 0; i < weapons.Length; i++)
			{
				bool flag = weapons[i].IsValid();
				if (flag)
				{
					this.UpdateWeaponCanChange(context, character, i);
				}
			}
		}

		// Token: 0x0600647F RID: 25727 RVA: 0x00390E30 File Offset: 0x0038F030
		private void UpdateWeaponCanChange(DataContext context, CombatCharacter character, int index)
		{
			bool allowUseFreeWeapon = Config.Character.Instance[character.GetCharacter().GetTemplateId()].AllowUseFreeWeapon;
			CombatWeaponData weaponData = character.GetWeaponData(index);
			bool canChange = index != character.GetUsingWeaponIndex() && !character.PreparingTeammateCommand() && ((index >= 3 && allowUseFreeWeapon) || weaponData.GetDurability() > 0) && weaponData.NotInAnyCd;
			bool flag = weaponData.GetCanChangeTo() != canChange;
			if (flag)
			{
				weaponData.SetCanChangeTo(canChange, context);
			}
		}

		// Token: 0x06006480 RID: 25728 RVA: 0x00390EAC File Offset: 0x0038F0AC
		public int GetWeaponCharId(int itemId)
		{
			CombatWeaponData data;
			return this._weaponDataDict.TryGetValue(itemId, out data) ? data.Character.GetId() : -1;
		}

		// Token: 0x06006481 RID: 25729 RVA: 0x00390ED7 File Offset: 0x0038F0D7
		public CombatWeaponData GetUsingWeaponData(CombatCharacter character)
		{
			return this._weaponDataDict[this.GetUsingWeaponKey(character).Id];
		}

		// Token: 0x06006482 RID: 25730 RVA: 0x00390EF0 File Offset: 0x0038F0F0
		public ItemKey GetUsingWeaponKey(CombatCharacter character)
		{
			int weaponIndex = character.GetUsingWeaponIndex();
			weaponIndex = DomainManager.SpecialEffect.ModifyData(character.GetId(), -1, 82, weaponIndex, -1, -1, -1);
			return character.GetWeapons()[weaponIndex];
		}

		// Token: 0x06006483 RID: 25731 RVA: 0x00390F2D File Offset: 0x0038F12D
		public GameData.Domains.Item.Weapon GetUsingWeapon(CombatCharacter combatChar)
		{
			return DomainManager.Item.GetElement_Weapons(this.GetUsingWeaponKey(combatChar).Id);
		}

		// Token: 0x06006484 RID: 25732 RVA: 0x00390F48 File Offset: 0x0038F148
		public bool CanNormalAttack(bool isAlly)
		{
			CombatCharacter combatChar = isAlly ? this._selfChar : this._enemyChar;
			return !combatChar.GetReserveNormalAttack() && this.CanNormalAttackWithoutCommandPrepareValueCheck(isAlly);
		}

		// Token: 0x06006485 RID: 25733 RVA: 0x00390F80 File Offset: 0x0038F180
		private bool CanNormalAttackWithoutCommandPrepareValueCheck(bool isAlly)
		{
			CombatCharacter combatChar = isAlly ? this._selfChar : this._enemyChar;
			bool flag = combatChar.ForbidNormalAttackEffectCount > 0;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				GameData.Domains.Item.Weapon weapon = DomainManager.Item.GetElement_Weapons(this.GetUsingWeaponKey(combatChar).Id);
				result = (weapon.GetMaxDurability() <= 0 || weapon.GetCurrDurability() != 0);
			}
			return result;
		}

		// Token: 0x06006486 RID: 25734 RVA: 0x00390FE4 File Offset: 0x0038F1E4
		private bool CanUnlockAttack(bool isAlly, int index)
		{
			CombatCharacter combatChar = isAlly ? this._selfChar : this._enemyChar;
			List<bool> canUnlockAttack = combatChar.GetCanUnlockAttack();
			return canUnlockAttack.CheckIndex(index) && canUnlockAttack[index];
		}

		// Token: 0x06006487 RID: 25735 RVA: 0x00391024 File Offset: 0x0038F224
		public void UnlockAttack(DataContext context, CombatCharacter combatChar, int index)
		{
			combatChar.ChangeUnlockAttackValue(context, index, -GlobalConfig.Instance.UnlockAttackUnit);
			combatChar.NeedUnlockAttack = true;
			combatChar.UnlockWeaponIndex = index;
			combatChar.MoveData.ResetJumpState(context, true);
			this.UpdateAllCommandAvailability(context, combatChar);
			Events.RaiseUnlockAttack(context, combatChar, index);
			combatChar.DoExtraUnlockEffect(context, index);
		}

		// Token: 0x06006488 RID: 25736 RVA: 0x00391080 File Offset: 0x0038F280
		public sbyte GetAttackHitType(CombatCharacter attacker, sbyte trickType)
		{
			return this.GetAttackHitType(attacker.GetCharacter(), trickType);
		}

		// Token: 0x06006489 RID: 25737 RVA: 0x003910A0 File Offset: 0x0038F2A0
		public unsafe sbyte GetAttackHitType(GameData.Domains.Character.Character attacker, sbyte trickType)
		{
			bool flag = trickType != 21;
			sbyte result;
			if (flag)
			{
				result = TrickType.Instance[trickType].AvoidType;
			}
			else
			{
				HitOrAvoidInts hitValues = attacker.GetHitValues();
				List<sbyte> hitTypeRandomPool = ObjectPool<List<sbyte>>.Instance.Get();
				int maxHitValue = int.MinValue;
				hitTypeRandomPool.Clear();
				for (sbyte type = 0; type < 4; type += 1)
				{
					int hitValue = *(ref hitValues.Items.FixedElementField + (IntPtr)type * 4);
					bool flag2 = hitValue > maxHitValue;
					if (flag2)
					{
						maxHitValue = hitValue;
						hitTypeRandomPool.Clear();
						hitTypeRandomPool.Add(type);
					}
					else
					{
						bool flag3 = hitValue == maxHitValue;
						if (flag3)
						{
							hitTypeRandomPool.Add(type);
						}
					}
				}
				sbyte hitType = hitTypeRandomPool[this.Context.Random.Next(hitTypeRandomPool.Count)];
				ObjectPool<List<sbyte>>.Instance.Return(hitTypeRandomPool);
				result = hitType;
			}
			return result;
		}

		// Token: 0x0600648A RID: 25738 RVA: 0x00391188 File Offset: 0x0038F388
		[return: TupleElementNames(new string[]
		{
			"aniName",
			"fullAniName"
		})]
		public ValueTuple<string, string> GetPrepareAttackAni(CombatCharacter character, sbyte trickType, int aniIndex)
		{
			TrickTypeItem trickData = TrickType.Instance[trickType];
			bool isBreakAttacking = character.IsBreakAttacking;
			ValueTuple<string, string> result;
			if (isBreakAttacking)
			{
				bool flag = character.BossConfig == null && character.AnimalConfig == null;
				string aniName;
				string fullAniName;
				if (flag)
				{
					fullAniName = (aniName = trickData.AttackAnimations[aniIndex] + "_B0");
				}
				else
				{
					bool flag2 = character.BossConfig != null;
					if (flag2)
					{
						string postfix = character.BossConfig.AttackEffectPostfix[character.GetUsingWeaponIndex()];
						aniName = character.BossConfig.AttackAnimation + "_B0" + postfix;
						fullAniName = character.BossConfig.AniPrefix[(int)character.GetBossPhase()] + aniName;
					}
					else
					{
						fullAniName = (aniName = null);
					}
				}
				result = new ValueTuple<string, string>(aniName, fullAniName);
			}
			else
			{
				bool flag3 = character.BossConfig == null && character.AnimalConfig == null;
				string aniName;
				string fullAniName;
				if (flag3)
				{
					fullAniName = (aniName = trickData.AttackAnimations[aniIndex] + "_7");
				}
				else
				{
					bool flag4 = character.BossConfig != null;
					if (flag4)
					{
						string postfix2 = character.BossConfig.AttackEffectPostfix[character.GetUsingWeaponIndex()];
						aniName = character.BossConfig.AttackAnimation + "_7" + postfix2;
						fullAniName = character.BossConfig.AniPrefix[(int)character.GetBossPhase()] + aniName;
					}
					else
					{
						aniName = trickData.AttackAnimations[aniIndex] + "_7";
						fullAniName = character.AnimalConfig.AniPrefix + aniName;
					}
				}
				result = new ValueTuple<string, string>(aniName, fullAniName);
			}
			return result;
		}

		// Token: 0x0600648B RID: 25739 RVA: 0x00391320 File Offset: 0x0038F520
		[return: TupleElementNames(new string[]
		{
			"aniName",
			"fullAniName",
			"particle",
			"sound"
		})]
		public ValueTuple<string, string, string, string> GetAttackEffect(CombatCharacter character, GameData.Domains.Item.Weapon weapon, sbyte trickType)
		{
			ValueTuple<string, string, string, string> result = default(ValueTuple<string, string, string, string>);
			TrickTypeItem trickData = TrickType.Instance[trickType];
			sbyte weaponAction = weapon.GetWeaponAction();
			short defendSkillId = character.GetAffectingDefendSkillId();
			bool flag = character.BossConfig == null && character.AnimalConfig == null;
			if (flag)
			{
				bool flag2 = character.GetIsFightBack() && defendSkillId >= 0 && !string.IsNullOrEmpty(Config.CombatSkill.Instance[defendSkillId].FightBackAnimation);
				if (flag2)
				{
					CombatSkillItem skillConfig = Config.CombatSkill.Instance[defendSkillId];
					result.Item2 = (result.Item1 = skillConfig.FightBackAnimation);
					result.Item3 = skillConfig.FightBackParticle;
					result.Item4 = skillConfig.FightBackSound;
				}
				else
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(1, 2);
					defaultInterpolatedStringHandler.AppendFormatted(trickData.AttackAnimations[(int)weaponAction]);
					defaultInterpolatedStringHandler.AppendLiteral("_");
					defaultInterpolatedStringHandler.AppendFormatted<byte>(character.PursueAttackCount);
					result.Item2 = (result.Item1 = defaultInterpolatedStringHandler.ToStringAndClear());
					defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(1, 2);
					defaultInterpolatedStringHandler.AppendFormatted(trickData.AttackParticles[(int)weaponAction]);
					defaultInterpolatedStringHandler.AppendLiteral("_");
					defaultInterpolatedStringHandler.AppendFormatted<byte>(character.PursueAttackCount);
					result.Item3 = defaultInterpolatedStringHandler.ToStringAndClear();
					defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(1, 2);
					defaultInterpolatedStringHandler.AppendFormatted(trickData.SoundEffects[(int)weaponAction]);
					defaultInterpolatedStringHandler.AppendLiteral("_");
					defaultInterpolatedStringHandler.AppendFormatted<byte>(character.PursueAttackCount);
					result.Item4 = defaultInterpolatedStringHandler.ToStringAndClear();
					result.Item4 += Config.Weapon.Instance[weapon.GetTemplateId()].SwingSoundsSuffix;
				}
			}
			else
			{
				int weaponIndex = character.GetUsingWeaponIndex();
				bool flag3 = character.BossConfig != null;
				if (flag3)
				{
					int bossPhase = (int)character.GetBossPhase();
					bool flag4 = character.GetIsFightBack() && defendSkillId >= 0;
					if (flag4)
					{
						CombatSkillItem skillConfig2 = Config.CombatSkill.Instance[defendSkillId];
						result.Item1 = skillConfig2.FightBackAnimation;
						result.Item2 = character.BossConfig.AniPrefix[bossPhase] + result.Item1;
						result.Item3 = character.BossConfig.DefendSkillParticlePrefix[bossPhase] + skillConfig2.FightBackParticle;
						result.Item4 = character.BossConfig.DefendSkillSoundPrefix[bossPhase] + skillConfig2.FightBackSound;
					}
					else
					{
						string postfix = character.BossConfig.AttackEffectPostfix[weaponIndex];
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(1, 3);
						defaultInterpolatedStringHandler.AppendFormatted(character.BossConfig.AttackAnimation);
						defaultInterpolatedStringHandler.AppendLiteral("_");
						defaultInterpolatedStringHandler.AppendFormatted<byte>(character.PursueAttackCount);
						defaultInterpolatedStringHandler.AppendFormatted(postfix);
						result.Item1 = defaultInterpolatedStringHandler.ToStringAndClear();
						result.Item2 = character.BossConfig.AniPrefix[bossPhase] + result.Item1;
						defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(1, 3);
						defaultInterpolatedStringHandler.AppendFormatted(character.BossConfig.AttackParticles[bossPhase]);
						defaultInterpolatedStringHandler.AppendLiteral("_");
						defaultInterpolatedStringHandler.AppendFormatted<byte>(character.PursueAttackCount);
						defaultInterpolatedStringHandler.AppendFormatted(postfix);
						result.Item3 = defaultInterpolatedStringHandler.ToStringAndClear();
						defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(1, 2);
						defaultInterpolatedStringHandler.AppendFormatted(character.BossConfig.AttackSounds[bossPhase]);
						defaultInterpolatedStringHandler.AppendLiteral("_");
						defaultInterpolatedStringHandler.AppendFormatted<byte>(character.PursueAttackCount);
						result.Item4 = defaultInterpolatedStringHandler.ToStringAndClear();
					}
				}
				else
				{
					result.Item1 = trickData.AttackAnimations.GetClampedIndexValueWithWarning((int)weaponAction, "GetAttackEffect");
					result.Item3 = character.AnimalConfig.AttackParticles[weaponIndex];
					result.Item4 = character.AnimalConfig.AttackSounds[weaponIndex];
					result.Item2 = character.AnimalConfig.AniPrefix + result.Item1;
				}
			}
			return result;
		}

		// Token: 0x0600648C RID: 25740 RVA: 0x00391754 File Offset: 0x0038F954
		[return: TupleElementNames(new string[]
		{
			"aniName",
			"particle"
		})]
		public ValueTuple<string, string> GetBlockEffect(CombatCharacter character, IRandomSource random)
		{
			bool flag = character.BossConfig == null && character.AnimalConfig == null;
			ValueTuple<string, string> result;
			if (flag)
			{
				WeaponItem weaponConfig = character.GetWeaponData(-1).Template;
				int effectIndex = random.Next(weaponConfig.BlockAnis.Count);
				result = new ValueTuple<string, string>(weaponConfig.BlockAnis[effectIndex], weaponConfig.BlockParticles[effectIndex]);
			}
			else
			{
				string aniName = CombatDomain.SpecialCharBlockAni[random.Next(CombatDomain.SpecialCharBlockAni.Count)];
				string aniPrefix = (character.BossConfig != null) ? character.BossConfig.AniPrefix[(int)character.GetBossPhase()] : character.AnimalConfig.AniPrefix;
				result = new ValueTuple<string, string>(aniName, "Particle_" + aniPrefix + aniName);
			}
			return result;
		}

		// Token: 0x0600648D RID: 25741 RVA: 0x00391820 File Offset: 0x0038FA20
		public void UpdateWeaponCd(DataContext context, CombatCharacter character)
		{
			ItemKey[] weapons = character.GetWeapons();
			for (int i = 0; i < weapons.Length; i++)
			{
				bool flag = !weapons[i].IsValid();
				if (!flag)
				{
					CombatWeaponData weaponData = character.GetWeaponData(i);
					bool notInAnyCd = weaponData.NotInAnyCd;
					if (!notInAnyCd)
					{
						bool flag2 = weaponData.GetFixedCdLeftFrame() > 0;
						if (flag2)
						{
							weaponData.SetFixedCdLeftFrame((short)Math.Max((int)(weaponData.GetFixedCdLeftFrame() - 1), 0), context);
						}
						else
						{
							bool flag3 = weaponData.GetCdFrame() > 0;
							if (flag3)
							{
								int weight = weaponData.Item.GetWeight();
								int switchSpeed = (int)character.GetCharacter().GetWeaponSwitchSpeed();
								int cdSpeed = CFormula.CalcWeaponCdFrameSpeed(switchSpeed, weight);
								weaponData.SetCdFrame((short)Math.Max((int)weaponData.GetCdFrame() - cdSpeed, 0), context);
							}
						}
						bool notInAnyCd2 = weaponData.NotInAnyCd;
						if (notInAnyCd2)
						{
							this.UpdateWeaponCanChange(context, character, i);
							Events.RaiseWeaponCdEnd(context, character.GetId(), character.IsAlly, weaponData);
						}
					}
				}
			}
		}

		// Token: 0x0600648E RID: 25742 RVA: 0x00391928 File Offset: 0x0038FB28
		public void ChangeWeaponCd(DataContext context, CombatCharacter character, int index, CValuePercent addPercent)
		{
			CombatWeaponData weaponData = character.GetWeaponData(index);
			int addValue = 30000 * addPercent;
			short newCd = (short)Math.Clamp((int)weaponData.GetCdFrame() + addValue, 0, 30000);
			bool hasCd = weaponData.GetCdFrame() > 0;
			weaponData.SetCdFrame(newCd, context);
			this.UpdateWeaponCanChange(context, character, index);
			bool flag = hasCd && weaponData.NotInAnyCd;
			if (flag)
			{
				Events.RaiseWeaponCdEnd(context, character.GetId(), character.IsAlly, weaponData);
			}
		}

		// Token: 0x0600648F RID: 25743 RVA: 0x003919A4 File Offset: 0x0038FBA4
		public void ClearAllWeaponCd(DataContext context, CombatCharacter character)
		{
			foreach (CombatWeaponData weaponData in this._weaponDataDict.Values)
			{
				bool flag = weaponData.Character != character || weaponData.NotInAnyCd;
				if (!flag)
				{
					weaponData.SetCdFrame(0, context);
					weaponData.SetFixedCdLeftFrame(0, context);
					bool notInAnyCd = weaponData.NotInAnyCd;
					if (notInAnyCd)
					{
						Events.RaiseWeaponCdEnd(context, character.GetId(), character.IsAlly, weaponData);
					}
				}
			}
		}

		// Token: 0x06006490 RID: 25744 RVA: 0x00391A44 File Offset: 0x0038FC44
		public void ClearWeaponCd(DataContext context, CombatCharacter character, int index)
		{
			CombatWeaponData weaponData = character.GetWeaponData(index);
			bool notInAnyCd = weaponData.NotInAnyCd;
			if (!notInAnyCd)
			{
				weaponData.SetCdFrame(0, context);
				weaponData.SetFixedCdLeftFrame(0, context);
				Events.RaiseWeaponCdEnd(context, character.GetId(), character.IsAlly, weaponData);
			}
		}

		// Token: 0x06006491 RID: 25745 RVA: 0x00391A8C File Offset: 0x0038FC8C
		public void SilenceWeapon(DataContext context, CombatCharacter combatChar, int weaponIndex, int cdFrame)
		{
			ItemKey itemKey = combatChar.GetWeapons()[weaponIndex];
			bool flag = !itemKey.IsValid();
			if (!flag)
			{
				bool flag2 = cdFrame > 0;
				if (flag2)
				{
					ValueTuple<int, int> extraTotalPercent = combatChar.GetFeatureSilenceFrameTotalPercent();
					cdFrame = DomainManager.SpecialEffect.ModifyValue(combatChar.GetId(), 264, cdFrame, -1, -1, -1, 0, 0, extraTotalPercent.Item1, extraTotalPercent.Item2);
				}
				short cdFrameShort = (short)Math.Clamp(cdFrame, -1, 32767);
				bool flag3 = cdFrameShort == 0;
				if (!flag3)
				{
					CombatWeaponData weaponData = this._weaponDataDict[itemKey.Id];
					weaponData.SetFixedCdTotalFrame(cdFrameShort, context);
					weaponData.SetFixedCdLeftFrame(cdFrameShort, context);
					this.UpdateWeaponCanChange(context, combatChar, weaponIndex);
				}
			}
		}

		// Token: 0x06006492 RID: 25746 RVA: 0x00391B44 File Offset: 0x0038FD44
		public void CalcNormalAttack(CombatContext context, sbyte trickType)
		{
			CombatCharacter attacker = context.Attacker;
			CombatCharacter defender = context.Defender;
			Events.RaiseNormalAttackBegin(context, attacker, defender, trickType, (int)attacker.PursueAttackCount);
			ItemKey weaponKey = context.WeaponKey;
			GameData.Domains.Item.Weapon weapon = context.Weapon;
			WeaponItem configData = context.WeaponConfig;
			sbyte hitType = attacker.NormalAttackHitType;
			sbyte trickHitType = (trickType != 21) ? TrickType.Instance[trickType].AvoidType : hitType;
			bool isCarrierAnimalAttack = attacker.GetId() == this._carrierAnimalCombatCharId;
			CombatProperty property = this._damageCompareData.GetProperty(0);
			bool critical = context.CheckCritical(hitType);
			context = context.Property(property).Critical(critical);
			int hitOdds = property.HitOdds;
			hitOdds = this.ApplyHitOddsSpecialEffect(attacker, defender, hitOdds, hitType, -1);
			bool flag = hitOdds > 0;
			if (flag)
			{
				hitOdds += GlobalConfig.Instance.NormalAttackExtraHitOdds;
			}
			bool inevitableHit = DomainManager.SpecialEffect.ModifyData(attacker.GetId(), -1, 251, false, -1, -1, -1);
			bool hit = !DomainManager.SpecialEffect.ModifyData(defender.GetId(), -1, 291, false, critical ? 1 : 0, (int)context.BodyPart, attacker.GetId()) && (inevitableHit || hitOdds < 0 || context.Random.CheckPercentProb(hitOdds));
			bool isFightBack = context.IsFightBack;
			bool flag2 = attacker.AttackForceHitCount > 0;
			if (flag2)
			{
				hit = true;
			}
			else
			{
				bool flag3 = attacker.AttackForceMissCount > 0;
				if (flag3)
				{
					hit = false;
				}
			}
			Events.RaiseNormalAttackCalcHitEnd(context, attacker, defender, (int)attacker.PursueAttackCount, hit, isFightBack, trickHitType == 3);
			Events.RaiseNormalAttackCalcCriticalEnd(context, attacker, defender, critical);
			Events.RaiseCalcLeveragingValue(context, trickHitType, hit, (int)attacker.PursueAttackCount);
			bool flag4 = hit;
			if (flag4)
			{
				sbyte breakOddsTrickType = (trickType != 21) ? trickType : CombatDomain.GodTrickUseTrickType[hitType];
				sbyte breakOdds = (attacker.NormalAttackBodyPart >= 0) ? TrickType.Instance[breakOddsTrickType].EquipmentBreakOdds : 0;
				this.CalculateWeaponArmorBreak(context, breakOdds);
				int finalDamage = 0;
				bool addFlawOrAcupoint = false;
				bool flag5 = !TrickType.NoBodyDamageTrickType.Exist(trickType);
				if (flag5)
				{
					OuterAndInnerInts markCounts = this.CalcAndAddInjury(context, hitType, out finalDamage, out critical, isFightBack ? attacker.GetFightBackPower(attacker.FightBackHitType) : 100, 100, 100);
					addFlawOrAcupoint = attacker.ApplyChangeTrickFlawOrAcupoint(context, defender, attacker.ChangeTrickBodyPart);
					bool flag6 = trickHitType != 3;
					if (flag6)
					{
						context.ApplyWeaponAndArmorPoison(1);
					}
					bool flag7 = !attacker.IsAutoNormalAttackingSpecial && trickHitType != 3 && !isCarrierAnimalAttack;
					if (flag7)
					{
						this.AddBounceDamage(context, hitType);
					}
					bool flag8 = this.CanPlayHitAnimation(defender);
					if (flag8)
					{
						int totalMarkCount = markCounts.Outer + markCounts.Inner;
						bool flag9 = !attacker.NoBlockAttack && !critical;
						if (flag9)
						{
							ValueTuple<string, string> blockEffect = this.GetBlockEffect(defender, context.Random);
							defender.SetAnimationToPlayOnce(blockEffect.Item1, context);
							defender.SetParticleToPlay(blockEffect.Item2, context);
						}
						else
						{
							bool flag10 = totalMarkCount > 0 || addFlawOrAcupoint;
							if (flag10)
							{
								defender.SetAnimationToPlayOnce(this.GetHittedAni(defender, Math.Clamp(totalMarkCount - 1, 0, 2)), context);
							}
						}
					}
					defender.PlayBeHitSound(context, configData, attacker, critical);
				}
				bool needReduceWeaponDurability = attacker.NeedReduceWeaponDurability;
				if (needReduceWeaponDurability)
				{
					this.ReduceDurabilityByHit(context, attacker, this.GetUsingWeaponKey(attacker));
					attacker.NeedReduceWeaponDurability = false;
				}
				bool needReduceArmorDurability = defender.NeedReduceArmorDurability;
				if (needReduceArmorDurability)
				{
					this.ReduceDurabilityByHit(context, defender, defender.Armors[(int)attacker.NormalAttackBodyPart]);
					defender.NeedReduceArmorDurability = false;
				}
				bool flag11 = !defender.GetNewPoisonsToShow().IsNonZero() && !addFlawOrAcupoint && finalDamage <= 0;
				if (flag11)
				{
					defender.SetParticleToPlay("Particle_D_qidun", context);
				}
				bool flag12 = defender.GetPreparingOtherAction() == 2 && this._currentDistance <= (short)this.InterruptFleeNeedDistance;
				if (flag12)
				{
					this.InterruptOtherAction(context, defender);
				}
				this.AddToCheckFallenSet(attacker.GetId());
				this.AddToCheckFallenSet(defender.GetId());
			}
			else
			{
				bool flag13 = this.CanPlayHitAnimation(defender);
				if (flag13)
				{
					defender.SetAnimationToPlayOnce(this.GetAvoidAni(defender, hitType), context);
				}
				defender.SetParticleToPlay((defender.IsAlly ? this._selfAvoidParticle : this._enemyAvoidParticle)[(int)hitType], context);
				string[] avoidSounds = this._avoidSound[(int)hitType];
				defender.SetHitSoundToPlay(avoidSounds[context.Random.Next(avoidSounds.Length)], context);
			}
			int avoidTrickOdds = GlobalConfig.Instance.AvoidAddTrickBaseOdds + hitOdds / GlobalConfig.Instance.AvoidAddTrickHitOddsDivisor;
			bool addTrick = hit || (context.Random.CheckPercentProb(avoidTrickOdds) && attacker.AttackForceMissCount <= 0);
			bool flag14 = !attacker.IsAutoNormalAttackingSpecial && attacker.GetId() != this._carrierAnimalCombatCharId;
			bool flag15 = flag14;
			if (flag15)
			{
				byte pursueAttackCount = attacker.PursueAttackCount;
				bool flag16 = pursueAttackCount == 0 || pursueAttackCount == 2 || pursueAttackCount == 5;
				flag15 = flag16;
			}
			bool flag17 = flag15 && addTrick;
			if (flag17)
			{
				int trickCount = 1 * DomainManager.SpecialEffect.GetModify(attacker.GetId(), 328, -1, -1, -1, EDataSumType.All);
				this.AddTrick(context, this.GetCombatCharacter(attacker.IsAlly, false), trickType, trickCount, true, !hit);
			}
			bool canFightBack = DomainManager.SpecialEffect.ModifyData(attacker.GetId(), -1, 86, true, -1, -1, -1);
			bool canFightBackWithHit = DomainManager.SpecialEffect.ModifyData(defender.GetId(), -1, 250, false, critical ? 1 : 0, -1, -1);
			bool flag18 = !isFightBack && !isCarrierAnimalAttack && canFightBack && this.CanFightBack(defender, hitType) && (canFightBackWithHit || !hit);
			if (flag18)
			{
				defender.FightBackWithHit = hit;
				defender.FightBackHitType = hitType;
				defender.SetIsFightBack(true, context);
			}
			bool flag19 = this.InAttackRange(attacker) && !attacker.IsAutoNormalAttackingSpecial;
			if (flag19)
			{
				bool isPursue = attacker.PursueAttackCount > 0;
				int addStance = (int)((!isPursue) ? configData.StanceIncrement : (configData.StanceIncrement * 25 / 100));
				sbyte attackPreparePointCost = weapon.GetAttackPreparePointCost();
				bool flag20 = attacker.GetStanceValue() < attacker.GetMaxStanceValue();
				if (flag20)
				{
					this.RecoverStanceValue(context, attacker, addStance, attackPreparePointCost, isPursue);
				}
				bool flag21 = defender.GetStanceValue() < defender.GetMaxStanceValue();
				if (flag21)
				{
					this.RecoverStanceValue(context, defender, addStance / 3, attackPreparePointCost, isPursue);
				}
			}
			bool flag22 = weapon.GetCanChangeTrick() && attacker.AttackForceHitCount <= 0 && attacker.AttackForceMissCount <= 0 && !attacker.GetChangeTrickAttack();
			if (flag22)
			{
				int addValue = DomainManager.Character.CalcWeaponChangeTrickValue(attacker.GetId(), weaponKey, attacker.PursueAttackCount == 0, hit);
				this.ChangeChangeTrickProgress(context, attacker, addValue);
			}
			bool flag23 = !attacker.IsAutoNormalAttackingSpecial && attacker.PursueAttackCount == 0 && attacker.PoisonOverflow(0);
			if (flag23)
			{
				attacker.AddPoisonAffectValue(0, 1, false);
			}
			Events.RaiseNormalAttackEnd(context, attacker, defender, trickType, (int)attacker.PursueAttackCount, hit, isFightBack);
		}

		// Token: 0x06006493 RID: 25747 RVA: 0x0039229C File Offset: 0x0039049C
		public void CalcUnlockAttack(CombatCharacter attacker, int index)
		{
			IRandomSource random = this.Context.Random;
			CombatCharacter defender = this.GetCombatCharacter(!attacker.IsAlly, true);
			sbyte trickType = attacker.UnlockWeapon.GetTricks().GetRandom(random);
			sbyte hitType = this.GetAttackHitType(attacker, trickType);
			hitType = (sbyte)DomainManager.SpecialEffect.ModifyData(attacker.GetId(), -1, 68, (int)hitType, -1, -1, -1);
			sbyte bodyPart = this.GetAttackBodyPart(attacker, defender, random, -1, trickType, hitType);
			CombatContext context = CombatContext.Create(attacker, defender, bodyPart, -1, attacker.UnlockWeaponIndex, null).Critical(true);
			attacker.NormalAttackHitType = hitType;
			attacker.NormalAttackBodyPart = bodyPart;
			Events.RaiseNormalAttackBegin(context, attacker, defender, trickType, 0);
			Events.RaiseNormalAttackCalcHitEnd(context, attacker, defender, 0, true, false, hitType == 3);
			Events.RaiseNormalAttackCalcCriticalEnd(context, attacker, defender, true);
			Events.RaiseCalcLeveragingValue(context, hitType, true, index);
			bool flag = bodyPart >= 0;
			if (flag)
			{
				context.ApplyWeaponAndArmorPoison(attacker.UnlockEffect.PoisonRatio);
				bool flag2 = attacker.UnlockEffect.FlawLevels != null && attacker.UnlockEffect.FlawLevels.Length > index;
				if (flag2)
				{
					this.AddFlaw(this.Context, defender, attacker.UnlockEffect.FlawLevels[index], new ValueTuple<int, short>(-1, -1), bodyPart, 1, true);
				}
				bool flag3 = attacker.UnlockEffect.AcupointLevels != null && attacker.UnlockEffect.AcupointLevels.Length > index;
				if (flag3)
				{
					this.AddAcupoint(this.Context, defender, attacker.UnlockEffect.AcupointLevels[index], new ValueTuple<int, short>(-1, -1), bodyPart, 1, true);
				}
			}
			sbyte breakOdds = TrickType.Instance[trickType].EquipmentBreakOdds;
			context.CheckReduceArmorDurability(breakOdds);
			int power = (int)(100 + DomainManager.Character.GetItemPower(context.AttackerId, context.WeaponKey) / 5);
			this.UpdateDamageCompareData(context);
			context = context.Property(this._damageCompareData.GetProperty(0));
			int num;
			bool flag4;
			this.CalcAndAddInjury(context, hitType, out num, out flag4, power, 100, 100);
			bool flag5 = hitType != 3;
			if (flag5)
			{
				this.AddBounceDamage(context, hitType);
			}
			bool needReduceArmorDurability = defender.NeedReduceArmorDurability;
			if (needReduceArmorDurability)
			{
				defender.NeedReduceArmorDurability = false;
				this.ReduceDurabilityByHit(context, defender, context.ArmorKey);
			}
			bool flag6 = attacker.UnlockEffect.StealNeiliAllocationPercent > 0;
			if (flag6)
			{
				attacker.StealNeiliAllocationRandom(this.Context, defender, attacker.UnlockEffect.StealNeiliAllocationPercent);
			}
			short banableSkillId = defender.GetRandomBanableSkillId(this.Context.Random, null, -1);
			bool flag7 = banableSkillId >= 0 && attacker.UnlockEffect.SilenceSkillFrame > 0;
			if (flag7)
			{
				this.SilenceSkill(this.Context, defender, banableSkillId, attacker.UnlockEffect.SilenceSkillFrame, 100);
			}
			bool flag8 = attacker.UnlockEffect.AddQiDisorder > 0;
			if (flag8)
			{
				this.ChangeDisorderOfQiRandomRecovery(this.Context, defender, attacker.UnlockEffect.AddQiDisorder, false);
			}
			this.AddToCheckFallenSet(defender.GetId());
			bool flag9 = defender.GetPreparingOtherAction() == 2 && this._currentDistance <= (short)this.InterruptFleeNeedDistance;
			if (flag9)
			{
				this.InterruptOtherAction(context, defender);
			}
			Events.RaiseNormalAttackEnd(context, attacker, defender, trickType, 0, true, false);
			attacker.NormalAttackHitType = -1;
			attacker.NormalAttackBodyPart = -1;
		}

		// Token: 0x06006494 RID: 25748 RVA: 0x00392608 File Offset: 0x00390808
		public bool CalcSpiritAttack(CombatCharacter attacker, int index)
		{
			IRandomSource random = this.Context.Random;
			CombatCharacter defender = this.GetCombatCharacter(!attacker.IsAlly, true);
			CombatWeaponData weaponData = attacker.GetWeaponData(index);
			sbyte trickType = weaponData.GetWeaponTricks().GetRandom(random);
			sbyte hitType = DomainManager.Combat.GetAttackHitType(attacker, trickType);
			sbyte bodyPart = DomainManager.Combat.GetAttackBodyPart(attacker, defender, random, -1, trickType, hitType);
			CombatContext context = CombatContext.Create(attacker, defender, bodyPart, -1, index, null);
			bool prevIsChangeTrick = attacker.GetChangeTrickAttack();
			attacker.SetChangeTrickAttack(true, context);
			attacker.IsAutoNormalAttackingSpecial = true;
			attacker.NormalAttackHitType = hitType;
			sbyte level = weaponData.Item.GetAttackPreparePointCost();
			bool hasFlawOrAcupoint = random.CheckPercentProb(50);
			bool flag = hasFlawOrAcupoint;
			if (flag)
			{
				bool flag2 = random.CheckPercentProb(75);
				if (flag2)
				{
					this.AddFlaw(context, defender, level, new ValueTuple<int, short>(-1, -1), bodyPart, 1, true);
				}
				else
				{
					this.AddAcupoint(context, defender, level, new ValueTuple<int, short>(-1, -1), bodyPart, 1, true);
				}
			}
			context.ApplyWeaponAndArmorPoison(1);
			context = context.Critical(true);
			this.UpdateDamageCompareData(context);
			context = context.Property(this._damageCompareData.GetProperty(0));
			int num;
			bool critical;
			this.CalcAndAddInjury(context, hitType, out num, out critical, 100, 100, 100);
			defender.PlayBeHitSound(context, weaponData.Template, attacker, critical);
			attacker.IsAutoNormalAttackingSpecial = false;
			attacker.NormalAttackHitType = -1;
			attacker.SetChangeTrickAttack(prevIsChangeTrick, context);
			return hasFlawOrAcupoint;
		}

		// Token: 0x06006495 RID: 25749 RVA: 0x0039279C File Offset: 0x0039099C
		public void AddWeaponAttackSelfInjury(DataContext context, CombatCharacter character, int weaponIndex)
		{
			CombatWeaponData weaponData = character.GetWeaponData(weaponIndex);
			GameData.Domains.Item.Weapon weapon = weaponData.Item;
			List<sbyte> trickList = weapon.GetTricks();
			sbyte trickType = trickList[context.Random.Next(0, trickList.Count)];
			sbyte hitType = this.GetAttackHitType(character, trickType);
			sbyte bodyPart = this.GetAttackBodyPart(character, character, context.Random, -1, trickType, hitType);
			CombatContext combatContext = CombatContext.Create(character, character, bodyPart, -1, weaponIndex, null);
			int num;
			bool flag;
			this.CalcAndAddInjury(combatContext, hitType, out num, out flag, 100, 100, 100);
		}

		// Token: 0x06006496 RID: 25750 RVA: 0x00392824 File Offset: 0x00390A24
		public void ChangeChangeTrickCount(DataContext context, CombatCharacter character, int addValue, bool bySelectChangeTrick = false)
		{
			character.SetChangeTrickCount((short)Math.Clamp((int)character.GetChangeTrickCount() + addValue, 0, character.MaxChangeTrickCount), context);
			bool flag = (int)character.GetChangeTrickCount() == character.MaxChangeTrickCount && character.GetChangeTrickProgress() > 0;
			if (flag)
			{
				character.SetChangeTrickProgress(0, context);
			}
			this.UpdateCanChangeTrick(context, character);
			Events.RaiseChangeTrickCountChanged(context, character, addValue, bySelectChangeTrick);
		}

		// Token: 0x06006497 RID: 25751 RVA: 0x0039288C File Offset: 0x00390A8C
		public void UpdateCanChangeTrick(DataContext context, CombatCharacter character)
		{
			GameData.Domains.Item.Weapon weapon = DomainManager.Item.GetElement_Weapons(this.GetUsingWeaponKey(character).Id);
			bool canChangeTrick = character.GetChangeTrickCount() > (short)weapon.GetAttackPreparePointCost() && this.IsCurrentCombatCharacter(character) && weapon.GetCanChangeTrick() && this.CanNormalAttackWithoutCommandPrepareValueCheck(character.IsAlly);
			bool flag = character.GetCanChangeTrick() != canChangeTrick;
			if (flag)
			{
				character.SetCanChangeTrick(canChangeTrick, context);
			}
		}

		// Token: 0x06006498 RID: 25752 RVA: 0x003928FC File Offset: 0x00390AFC
		public bool CanPursue(CombatCharacter character, bool critical)
		{
			CombatCharacter enemyChar = this.GetCombatCharacter(!character.IsAlly, true);
			bool flag = !this.IsInCombat() || !DomainManager.SpecialEffect.ModifyData(character.GetId(), -1, 252, true, -1, -1, -1) || character.PursueAttackCount >= 5 || character.GetIsFightBack() || enemyChar.GetIsFightBack() || enemyChar.ChangeCharId >= 0 || character.ChangeCharId >= 0 || enemyChar.NeedChangeBossPhase || this.IsCharacterFallen(enemyChar) || this.IsCharacterFallen(character) || this._saveDyingEffectTriggerd || character.AttackForceHitCount > 0 || character.AttackForceMissCount > 0;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				bool flag2 = character.IsAutoNormalAttackingSpecial || character.IsBreakAttacking;
				if (flag2)
				{
					result = true;
				}
				else
				{
					GameData.Domains.Item.Weapon weapon = DomainManager.Item.GetElement_Weapons(this.GetUsingWeaponKey(character).Id);
					short usePower = DomainManager.Character.GetItemPower(character.GetId(), weapon.GetItemKey());
					int pursueOdds = CFormula.FormulaCalcPursueOdds((int)weapon.GetPursueAttackFactor(), (int)usePower, (int)character.PursueAttackCount);
					int attackerAdd = DomainManager.SpecialEffect.GetModifyValue(character.GetId(), 76, EDataModifyType.Add, (int)character.PursueAttackCount, -1, -1, EDataSumType.All);
					int defenderAdd = DomainManager.SpecialEffect.GetModifyValue(enemyChar.GetId(), 109, EDataModifyType.Add, (int)character.PursueAttackCount, -1, -1, EDataSumType.All);
					pursueOdds = Math.Max(pursueOdds + attackerAdd + defenderAdd, 0);
					int attackerAddPercent = DomainManager.SpecialEffect.GetModifyValue(character.GetId(), 76, EDataModifyType.AddPercent, (int)character.PursueAttackCount, critical ? 1 : 0, -1, EDataSumType.All);
					int defenderAddPercent = DomainManager.SpecialEffect.GetModifyValue(enemyChar.GetId(), 109, EDataModifyType.AddPercent, (int)character.PursueAttackCount, critical ? 1 : 0, -1, EDataSumType.All);
					int percent = 100 + attackerAddPercent + defenderAddPercent;
					pursueOdds = pursueOdds * percent / 100;
					ValueTuple<int, int> attackerTotalPercent = DomainManager.SpecialEffect.GetTotalPercentModifyValue(character.GetId(), -1, 76, -1, -1, -1);
					ValueTuple<int, int> defenderTotalPercent = DomainManager.SpecialEffect.GetTotalPercentModifyValue(enemyChar.GetId(), -1, 109, -1, -1, -1);
					percent = 100 + Math.Max(attackerTotalPercent.Item1, defenderTotalPercent.Item1) + Math.Min(attackerTotalPercent.Item2, defenderTotalPercent.Item2);
					pursueOdds = pursueOdds * percent / 100;
					pursueOdds = DomainManager.SpecialEffect.ModifyData(character.GetId(), -1, 76, pursueOdds, -1, -1, -1);
					result = this.Context.Random.CheckPercentProb(pursueOdds);
				}
			}
			return result;
		}

		// Token: 0x06006499 RID: 25753 RVA: 0x00392B4C File Offset: 0x00390D4C
		private bool CanFightBack(CombatCharacter defender, sbyte hitType)
		{
			bool canFightBackDuringPrepareSkill = DomainManager.SpecialEffect.ModifyData(defender.GetId(), -1, 193, false, -1, -1, -1);
			bool flag = !this.IsMainCharacter(defender) || this.IsCharacterFallen(defender) || !this.InAttackRange(defender) || hitType == 3 || (defender.GetPreparingSkillId() >= 0 && !canFightBackDuringPrepareSkill) || defender.GetPreparingOtherAction() >= 0 || defender.GetPreparingItem().IsValid() || defender.GetFightBackPower(hitType) <= 0;
			return !flag;
		}

		// Token: 0x0600649A RID: 25754 RVA: 0x00392BD8 File Offset: 0x00390DD8
		public void AddRandomTrick(DataContext context, CombatCharacter combatChar, int count)
		{
			List<NeedTrick> tricks = ObjectPool<List<NeedTrick>>.Instance.Get();
			sbyte[] weaponTricks = combatChar.GetWeaponTricks();
			for (int i = 0; i < count; i++)
			{
				tricks.Add(new NeedTrick(weaponTricks.GetRandom(context.Random), 1));
			}
			this.AddTrick(context, combatChar, tricks, true, false);
			ObjectPool<List<NeedTrick>>.Instance.Return(tricks);
		}

		// Token: 0x0600649B RID: 25755 RVA: 0x00392C39 File Offset: 0x00390E39
		public void AddTrick(DataContext context, CombatCharacter combatChar, sbyte trickType, bool addedByAlly = true)
		{
			this.AddTrick(context, combatChar, trickType, 1, addedByAlly, false);
		}

		// Token: 0x0600649C RID: 25756 RVA: 0x00392C4C File Offset: 0x00390E4C
		public void AddTrick(DataContext context, CombatCharacter combatChar, sbyte trickType, int count, bool addedByAlly = true, bool addByAvoid = false)
		{
			List<NeedTrick> tricks = ObjectPool<List<NeedTrick>>.Instance.Get();
			tricks.Add(new NeedTrick(trickType, (byte)Math.Clamp(count, 0, 255)));
			this.AddTrick(context, combatChar, tricks, addedByAlly, addByAvoid);
			ObjectPool<List<NeedTrick>>.Instance.Return(tricks);
		}

		// Token: 0x0600649D RID: 25757 RVA: 0x00392C9C File Offset: 0x00390E9C
		public void AddTrick(DataContext context, CombatCharacter combatChar, IEnumerable<sbyte> trickTypes)
		{
			List<NeedTrick> tricks = ObjectPool<List<NeedTrick>>.Instance.Get();
			CombatDomain.ConvertTricks(tricks, trickTypes);
			this.AddTrick(context, combatChar, tricks, true, false);
			ObjectPool<List<NeedTrick>>.Instance.Return(tricks);
		}

		// Token: 0x0600649E RID: 25758 RVA: 0x00392CD8 File Offset: 0x00390ED8
		public void AddTrick(DataContext context, CombatCharacter character, List<NeedTrick> tricks, bool addedByAlly = true, bool addByAvoid = false)
		{
			TrickCollection trickCollection = character.GetTricks();
			int addedShaCount = 0;
			foreach (NeedTrick needTrick in tricks)
			{
				for (int i = 0; i < (int)needTrick.NeedCount; i++)
				{
					sbyte trickType = needTrick.TrickType;
					bool canGetTrick = DomainManager.SpecialEffect.ModifyData(character.GetId(), -1, 138, true, (int)trickType, addedByAlly ? 1 : 0, 1);
					bool flag = !canGetTrick;
					if (!flag)
					{
						trickType = (sbyte)DomainManager.SpecialEffect.ModifyData(character.GetId(), -1, 139, (int)trickType, -1, -1, -1);
						trickCollection.AppendTrick(trickType, addByAvoid);
						bool flag2 = trickCollection.Tricks.Count > character.GetMaxTrickCount();
						if (flag2)
						{
							this.RemoveOverflowTrick(context, character, false);
						}
						character.SetTricks(trickCollection, context);
						this.UpdateSkillCostTrickCanUse(context, character);
						Events.RaiseGetTrick(context, character.GetId(), character.IsAlly, trickType, character.IsTrickUsable(trickType));
						bool flag3 = trickType == 19;
						if (flag3)
						{
							addedShaCount++;
						}
					}
				}
			}
			for (int j = 0; j < addedShaCount; j++)
			{
				Events.RaiseGetShaTrick(context, character.GetId(), character.IsAlly, true);
			}
		}

		// Token: 0x0600649F RID: 25759 RVA: 0x00392E54 File Offset: 0x00391054
		public void RemoveOverflowTrick(DataContext context, CombatCharacter character, bool updateFieldAndSkill = false)
		{
			TrickCollection trickCollection = character.GetTricks();
			List<int> indexList = ObjectPool<List<int>>.Instance.Get();
			indexList.Clear();
			indexList.AddRange(trickCollection.Tricks.Keys);
			int removedCount = 0;
			while (trickCollection.Tricks.Count > character.GetMaxTrickCount())
			{
				trickCollection.RemoveTrick(indexList[0]);
				indexList.RemoveAt(0);
				removedCount++;
			}
			ObjectPool<List<int>>.Instance.Return(indexList);
			Events.RaiseOverflowTrickRemoved(context, character.GetId(), character.IsAlly, removedCount);
			if (updateFieldAndSkill)
			{
				character.SetTricks(trickCollection, context);
				this.UpdateSkillCostTrickCanUse(context, character);
			}
		}

		// Token: 0x060064A0 RID: 25760 RVA: 0x00392F00 File Offset: 0x00391100
		public void RemoveUsableTrickInsteadCostTrick([DisallowNull] CombatCharacter character, short skillId, [DisallowNull] List<NeedTrick> costTricks, [AllowNull] List<NeedTrick> costEnemyTricks = null)
		{
			if (costEnemyTricks != null)
			{
				costEnemyTricks.Clear();
			}
			bool canInstead = DomainManager.SpecialEffect.ModifyData(character.GetId(), skillId, 280, false, (costEnemyTricks == null) ? 0 : 1, -1, -1);
			CombatCharacter enemyChar = this.GetCombatCharacter(!character.IsAlly, false);
			bool flag = !canInstead || enemyChar == null;
			if (!flag)
			{
				Dictionary<sbyte, byte> costTrickDict = ObjectPool<Dictionary<sbyte, byte>>.Instance.Get();
				Dictionary<sbyte, byte> lackTrickDict = ObjectPool<Dictionary<sbyte, byte>>.Instance.Get();
				character.CalcCostTrickStatus(costTricks, costTrickDict, lackTrickDict);
				costTricks.Clear();
				Dictionary<sbyte, byte> extraDict = ObjectPool<Dictionary<sbyte, byte>>.Instance.Get();
				enemyChar.CalcInsteadTricks(extraDict, new Func<sbyte, bool>(enemyChar.IsTrickUsable), costTrickDict, lackTrickDict, int.MaxValue, false);
				costTricks.AddRange(from tup in costTrickDict
				select new NeedTrick(tup.Key, tup.Value));
				if (costEnemyTricks != null)
				{
					costEnemyTricks.AddRange(from tup in extraDict
					select new NeedTrick(tup.Key, tup.Value));
				}
				ObjectPool<Dictionary<sbyte, byte>>.Instance.Return(costTrickDict);
				ObjectPool<Dictionary<sbyte, byte>>.Instance.Return(lackTrickDict);
				ObjectPool<Dictionary<sbyte, byte>>.Instance.Return(extraDict);
			}
		}

		// Token: 0x060064A1 RID: 25761 RVA: 0x0039303C File Offset: 0x0039123C
		public void RemoveCostTrickInsteadUselessTrick(CombatCharacter character, short skillId, List<NeedTrick> costTricks, bool trulyCost)
		{
			bool canInstead = DomainManager.SpecialEffect.ModifyData(character.GetId(), skillId, 284, false, trulyCost ? 1 : 0, -1, -1);
			bool flag = !canInstead;
			if (!flag)
			{
				Dictionary<sbyte, byte> costTrickDict = ObjectPool<Dictionary<sbyte, byte>>.Instance.Get();
				Dictionary<sbyte, byte> lackTrickDict = ObjectPool<Dictionary<sbyte, byte>>.Instance.Get();
				character.CalcCostTrickStatus(costTricks, costTrickDict, lackTrickDict);
				costTricks.Clear();
				Dictionary<sbyte, byte> extraDict = ObjectPool<Dictionary<sbyte, byte>>.Instance.Get();
				character.CalcInsteadTricks(extraDict, new Func<sbyte, bool>(character.IsTrickUseless), costTrickDict, lackTrickDict, int.MaxValue, false);
				costTricks.AddRange(from tup in costTrickDict
				select new NeedTrick(tup.Key, tup.Value));
				costTricks.AddRange(from tup in extraDict
				select new NeedTrick(tup.Key, tup.Value));
				ObjectPool<Dictionary<sbyte, byte>>.Instance.Return(costTrickDict);
				ObjectPool<Dictionary<sbyte, byte>>.Instance.Return(lackTrickDict);
				ObjectPool<Dictionary<sbyte, byte>>.Instance.Return(extraDict);
			}
		}

		// Token: 0x060064A2 RID: 25762 RVA: 0x00393148 File Offset: 0x00391348
		public void RemoveCostTrickBySelfShaTrick(DataContext context, CombatCharacter character, short skillId, List<NeedTrick> costTricks, bool trulyCost)
		{
			bool canInstead = DomainManager.SpecialEffect.ModifyData(character.GetId(), skillId, 319, false, 1, -1, -1);
			bool flag = !canInstead;
			if (!flag)
			{
				Dictionary<sbyte, byte> costTrickDict = ObjectPool<Dictionary<sbyte, byte>>.Instance.Get();
				Dictionary<sbyte, byte> lackTrickDict = ObjectPool<Dictionary<sbyte, byte>>.Instance.Get();
				character.CalcCostTrickStatus(costTricks, costTrickDict, lackTrickDict);
				costTricks.Clear();
				Dictionary<sbyte, byte> extraDict = ObjectPool<Dictionary<sbyte, byte>>.Instance.Get();
				bool flag2 = costTrickDict.Keys.All((sbyte x) => x != 19);
				if (flag2)
				{
					character.CalcInsteadTricks(extraDict, (sbyte x) => x == 19, costTrickDict, lackTrickDict, int.MaxValue, true);
				}
				bool flag3;
				if (trulyCost)
				{
					flag3 = (extraDict.Values.Sum((byte x) => (int)x) > 0);
				}
				else
				{
					flag3 = false;
				}
				bool flag4 = flag3;
				if (flag4)
				{
					Events.RaiseShaTrickInsteadCostTricks(context, character, skillId);
				}
				costTricks.AddRange(from tup in costTrickDict
				select new NeedTrick(tup.Key, tup.Value));
				costTricks.AddRange(from tup in extraDict
				select new NeedTrick(tup.Key, tup.Value));
				ObjectPool<Dictionary<sbyte, byte>>.Instance.Return(costTrickDict);
				ObjectPool<Dictionary<sbyte, byte>>.Instance.Return(lackTrickDict);
				ObjectPool<Dictionary<sbyte, byte>>.Instance.Return(extraDict);
			}
		}

		// Token: 0x060064A3 RID: 25763 RVA: 0x003932D4 File Offset: 0x003914D4
		public void RemoveCostTrickByEnemyShaTrick(DataContext context, CombatCharacter character, short skillId, List<NeedTrick> costTricks, List<NeedTrick> costEnemyTricks = null)
		{
			bool canInstead = DomainManager.SpecialEffect.ModifyData(character.GetId(), skillId, 319, false, 0, -1, -1);
			CombatCharacter enemyChar = this.GetCombatCharacter(!character.IsAlly, false);
			bool flag = !canInstead || enemyChar == null;
			if (!flag)
			{
				Dictionary<sbyte, byte> costTrickDict = ObjectPool<Dictionary<sbyte, byte>>.Instance.Get();
				Dictionary<sbyte, byte> lackTrickDict = ObjectPool<Dictionary<sbyte, byte>>.Instance.Get();
				character.CalcCostTrickStatus(costTricks, costTrickDict, lackTrickDict);
				costTricks.Clear();
				Dictionary<sbyte, byte> extraDict = ObjectPool<Dictionary<sbyte, byte>>.Instance.Get();
				bool flag2 = costTrickDict.Keys.All((sbyte x) => x != 19);
				if (flag2)
				{
					enemyChar.CalcInsteadTricks(extraDict, (sbyte x) => x == 19, costTrickDict, lackTrickDict, int.MaxValue, true);
				}
				bool flag3;
				if (costEnemyTricks != null)
				{
					flag3 = (extraDict.Values.Sum((byte x) => (int)x) > 0);
				}
				else
				{
					flag3 = false;
				}
				bool flag4 = flag3;
				if (flag4)
				{
					Events.RaiseShaTrickInsteadCostTricks(context, character, skillId);
				}
				costTricks.AddRange(from tup in costTrickDict
				select new NeedTrick(tup.Key, tup.Value));
				if (costEnemyTricks != null)
				{
					costEnemyTricks.AddRange(from tup in extraDict
					select new NeedTrick(tup.Key, tup.Value));
				}
				ObjectPool<Dictionary<sbyte, byte>>.Instance.Return(costTrickDict);
				ObjectPool<Dictionary<sbyte, byte>>.Instance.Return(lackTrickDict);
				ObjectPool<Dictionary<sbyte, byte>>.Instance.Return(extraDict);
			}
		}

		// Token: 0x060064A4 RID: 25764 RVA: 0x00393484 File Offset: 0x00391684
		public void RemoveCostTrickByJiTrick(DataContext context, CombatCharacter character, short skillId, List<NeedTrick> costTricks, bool trulyCost)
		{
			int maxInsteadCount = DomainManager.SpecialEffect.ModifyData(character.GetId(), skillId, 313, 0, -1, -1, -1);
			bool flag = maxInsteadCount <= 0;
			if (!flag)
			{
				Dictionary<sbyte, byte> costTrickDict = ObjectPool<Dictionary<sbyte, byte>>.Instance.Get();
				Dictionary<sbyte, byte> lackTrickDict = ObjectPool<Dictionary<sbyte, byte>>.Instance.Get();
				character.CalcCostTrickStatus(costTricks, costTrickDict, lackTrickDict);
				costTricks.Clear();
				Dictionary<sbyte, byte> extraDict = ObjectPool<Dictionary<sbyte, byte>>.Instance.Get();
				bool flag2 = costTrickDict.Keys.All((sbyte x) => x != 12);
				if (flag2)
				{
					character.CalcInsteadTricks(extraDict, (sbyte x) => x == 12, costTrickDict, lackTrickDict, maxInsteadCount, false);
				}
				if (trulyCost)
				{
					Events.RaiseJiTrickInsteadCostTricks(context, character, extraDict.Values.Sum((byte x) => (int)x));
				}
				costTricks.AddRange(from tup in costTrickDict
				select new NeedTrick(tup.Key, tup.Value));
				costTricks.AddRange(from tup in extraDict
				select new NeedTrick(tup.Key, tup.Value));
				ObjectPool<Dictionary<sbyte, byte>>.Instance.Return(costTrickDict);
				ObjectPool<Dictionary<sbyte, byte>>.Instance.Return(lackTrickDict);
				ObjectPool<Dictionary<sbyte, byte>>.Instance.Return(extraDict);
			}
		}

		// Token: 0x060064A5 RID: 25765 RVA: 0x00393608 File Offset: 0x00391808
		public void RemoveJiTrickByUselessTrick(DataContext context, CombatCharacter character, short skillId, List<NeedTrick> costTricks, bool trulyCost)
		{
			int maxInsteadCount = DomainManager.SpecialEffect.ModifyData(character.GetId(), skillId, 314, 0, -1, -1, -1);
			maxInsteadCount = Math.Min(maxInsteadCount, costTricks.Sum((NeedTrick x) => (int)((x.TrickType == 12) ? x.NeedCount : 0)));
			bool flag = maxInsteadCount <= 0;
			if (!flag)
			{
				Dictionary<sbyte, byte> costTrickDict = ObjectPool<Dictionary<sbyte, byte>>.Instance.Get();
				foreach (NeedTrick needTrick in costTricks)
				{
					costTrickDict[needTrick.TrickType] = needTrick.NeedCount;
				}
				costTricks.Clear();
				Dictionary<sbyte, byte> extraDict = ObjectPool<Dictionary<sbyte, byte>>.Instance.Get();
				foreach (sbyte trickType in character.GetTricks().Tricks.Values)
				{
					bool flag2 = character.IsTrickUsable(trickType);
					if (!flag2)
					{
						extraDict[trickType] = (byte)Math.Min((int)(extraDict.GetOrDefault(trickType) + 1), 255);
						Dictionary<sbyte, byte> dictionary = costTrickDict;
						dictionary[12] = dictionary[12] - 1;
						bool flag3 = costTrickDict[12] > 0;
						if (!flag3)
						{
							costTrickDict.Remove(12);
							break;
						}
					}
				}
				if (trulyCost)
				{
					Events.RaiseUselessTrickInsteadJiTricks(context, character, extraDict.Values.Sum((byte x) => (int)x));
				}
				costTricks.AddRange(from tup in costTrickDict
				select new NeedTrick(tup.Key, tup.Value));
				costTricks.AddRange(from tup in extraDict
				select new NeedTrick(tup.Key, tup.Value));
				ObjectPool<Dictionary<sbyte, byte>>.Instance.Return(costTrickDict);
				ObjectPool<Dictionary<sbyte, byte>>.Instance.Return(extraDict);
			}
		}

		// Token: 0x060064A6 RID: 25766 RVA: 0x0039383C File Offset: 0x00391A3C
		public bool RemoveTrick(DataContext context, CombatCharacter character, sbyte trickType, byte count = 1, bool removedByAlly = true, int preferIndex = -1)
		{
			List<NeedTrick> removeTricks = ObjectPool<List<NeedTrick>>.Instance.Get();
			removeTricks.Clear();
			removeTricks.Add(new NeedTrick(trickType, count));
			bool anyRemoved = this.RemoveTrick(context, character, removeTricks, removedByAlly, false, preferIndex);
			ObjectPool<List<NeedTrick>>.Instance.Return(removeTricks);
			return anyRemoved;
		}

		// Token: 0x060064A7 RID: 25767 RVA: 0x0039388C File Offset: 0x00391A8C
		public void RemoveTrick(DataContext context, CombatCharacter combatChar, IEnumerable<sbyte> trickTypes, bool removedByAlly = true)
		{
			List<NeedTrick> tricks = ObjectPool<List<NeedTrick>>.Instance.Get();
			CombatDomain.ConvertTricks(tricks, trickTypes);
			this.RemoveTrick(context, combatChar, tricks, removedByAlly, false, -1);
			ObjectPool<List<NeedTrick>>.Instance.Return(tricks);
		}

		// Token: 0x060064A8 RID: 25768 RVA: 0x003938C8 File Offset: 0x00391AC8
		public bool RemoveTrick(DataContext context, CombatCharacter character, List<NeedTrick> tricks, bool removedByAlly = true, bool skillCost = false, int preferIndex = -1)
		{
			TrickCollection trickCollection = character.GetTricks();
			List<int> indexList = ObjectPool<List<int>>.Instance.Get();
			bool anyRemoved = false;
			tricks = DomainManager.SpecialEffect.ModifyData(character.GetId(), -1, 164, tricks, skillCost ? 1 : 0, removedByAlly ? 1 : 0, -1);
			int removeShaTrickCount = 0;
			foreach (NeedTrick needTrick in tricks)
			{
				bool flag = needTrick.NeedCount <= 0;
				if (!flag)
				{
					int removeCounter = 0;
					indexList.Clear();
					indexList.AddRange(trickCollection.Tricks.Keys);
					bool flag2 = indexList.Contains(preferIndex);
					if (flag2)
					{
						indexList.MoveIndexToFirst(indexList.IndexOf(preferIndex));
					}
					for (int i = 0; i < indexList.Count; i++)
					{
						bool flag3 = character.TrickEquals(trickCollection.Tricks[indexList[i]], needTrick.TrickType);
						if (flag3)
						{
							trickCollection.RemoveTrick(indexList[i]);
							removeCounter++;
							bool flag4 = needTrick.TrickType == 19;
							if (flag4)
							{
								removeShaTrickCount++;
							}
							bool flag5 = removeCounter == (int)needTrick.NeedCount;
							if (flag5)
							{
								break;
							}
						}
					}
					bool flag6 = removeCounter > 0;
					if (flag6)
					{
						anyRemoved = true;
					}
				}
			}
			ObjectPool<List<int>>.Instance.Return(indexList);
			character.SetTricks(trickCollection, context);
			for (int j = 0; j < removeShaTrickCount; j++)
			{
				Events.RaiseRemoveShaTrick(context, character.GetId());
			}
			return anyRemoved;
		}

		// Token: 0x060064A9 RID: 25769 RVA: 0x00393A7C File Offset: 0x00391C7C
		public bool StealTrick(DataContext context, CombatCharacter thief, CombatCharacter victim, sbyte trickType, byte count = 1)
		{
			List<NeedTrick> tricks = ObjectPool<List<NeedTrick>>.Instance.Get();
			tricks.Add(new NeedTrick(trickType, count));
			bool anyChanged = this.StealTrick(context, thief, victim, tricks);
			ObjectPool<List<NeedTrick>>.Instance.Return(tricks);
			return anyChanged;
		}

		// Token: 0x060064AA RID: 25770 RVA: 0x00393AC4 File Offset: 0x00391CC4
		public bool StealTrick(DataContext context, CombatCharacter thief, CombatCharacter victim, IEnumerable<sbyte> trickTypes)
		{
			List<NeedTrick> tricks = ObjectPool<List<NeedTrick>>.Instance.Get();
			CombatDomain.ConvertTricks(tricks, trickTypes);
			bool anyChanged = this.StealTrick(context, thief, victim, tricks);
			ObjectPool<List<NeedTrick>>.Instance.Return(tricks);
			return anyChanged;
		}

		// Token: 0x060064AB RID: 25771 RVA: 0x00393B04 File Offset: 0x00391D04
		public bool StealTrick(DataContext context, CombatCharacter thief, CombatCharacter victim, List<NeedTrick> tricks)
		{
			tricks = DomainManager.SpecialEffect.ModifyData(victim.GetId(), -1, 164, tricks, -1, -1, -1);
			List<NeedTrick> actualTricks = ObjectPool<List<NeedTrick>>.Instance.Get();
			TrickCollection trickCollection = victim.GetTricks();
			List<int> indexList = ObjectPool<List<int>>.Instance.Get();
			foreach (NeedTrick needTrick in tricks)
			{
				bool flag = needTrick.NeedCount <= 0;
				if (!flag)
				{
					byte removeCounter = 0;
					indexList.Clear();
					indexList.AddRange(trickCollection.Tricks.Keys);
					for (int i = 0; i < indexList.Count; i++)
					{
						bool flag2 = trickCollection.Tricks[indexList[i]] != needTrick.TrickType;
						if (!flag2)
						{
							trickCollection.RemoveTrick(indexList[i]);
							removeCounter += 1;
							bool flag3 = removeCounter == needTrick.NeedCount;
							if (flag3)
							{
								break;
							}
						}
					}
					bool flag4 = removeCounter > 0;
					if (flag4)
					{
						actualTricks.Add(new NeedTrick(needTrick.TrickType, removeCounter));
					}
				}
			}
			victim.SetTricks(trickCollection, context);
			bool anyChanged = actualTricks.Count > 0;
			bool flag5 = anyChanged;
			if (flag5)
			{
				this.AddTrick(context, thief, actualTricks, true, false);
			}
			ObjectPool<List<int>>.Instance.Return(indexList);
			ObjectPool<List<NeedTrick>>.Instance.Return(actualTricks);
			return anyChanged;
		}

		// Token: 0x060064AC RID: 25772 RVA: 0x00393C9C File Offset: 0x00391E9C
		private static void ConvertTricks(ICollection<NeedTrick> needTricks, IEnumerable<sbyte> trickTypes)
		{
			Dictionary<sbyte, byte> dict = ObjectPool<Dictionary<sbyte, byte>>.Instance.Get();
			dict.Clear();
			foreach (sbyte trickType in trickTypes)
			{
				dict[trickType] = (byte)Math.Clamp((int)(dict.GetOrDefault(trickType) + 1), 0, 255);
			}
			needTricks.Clear();
			foreach (KeyValuePair<sbyte, byte> keyValuePair in dict)
			{
				sbyte b;
				byte b2;
				keyValuePair.Deconstruct(out b, out b2);
				sbyte trickType2 = b;
				byte count = b2;
				needTricks.Add(new NeedTrick(trickType2, count));
			}
			ObjectPool<Dictionary<sbyte, byte>>.Instance.Return(dict);
		}

		// Token: 0x060064AD RID: 25773 RVA: 0x00393D80 File Offset: 0x00391F80
		public bool WeaponHasNeedTrick(CombatCharacter character, short skillTemplateId, CombatWeaponData weaponData)
		{
			sbyte[] weaponTricks = weaponData.GetWeaponTricks();
			List<NeedTrick> costTricks = ObjectPool<List<NeedTrick>>.Instance.Get();
			costTricks.Clear();
			GameData.Domains.CombatSkill.CombatSkill combatSkill;
			bool flag = DomainManager.CombatSkill.TryGetElement_CombatSkills(new CombatSkillKey(character.GetId(), skillTemplateId), out combatSkill);
			if (flag)
			{
				DomainManager.CombatSkill.GetCombatSkillCostTrick(combatSkill, costTricks, false);
			}
			else
			{
				costTricks.AddRange(Config.CombatSkill.Instance[skillTemplateId].TrickCost);
			}
			bool result = costTricks.All((NeedTrick x) => weaponTricks.Exist(x.TrickType));
			ObjectPool<List<NeedTrick>>.Instance.Return(costTricks);
			return result;
		}

		// Token: 0x060064AE RID: 25774 RVA: 0x00393E20 File Offset: 0x00392020
		public bool HasNeedTrick(CombatCharacter character, GameData.Domains.CombatSkill.CombatSkill skill, bool useConfigValue = false)
		{
			TrickCollection trickCollection = character.GetTricks();
			List<NeedTrick> costTricks = ObjectPool<List<NeedTrick>>.Instance.Get();
			List<int> indexList = ObjectPool<List<int>>.Instance.Get();
			bool result = true;
			costTricks.Clear();
			if (useConfigValue)
			{
				costTricks.AddRange(Config.CombatSkill.Instance[skill.GetId().SkillTemplateId].TrickCost);
			}
			else
			{
				DomainManager.CombatSkill.GetCombatSkillCostTrick(skill, costTricks, true);
			}
			this.RemoveUsableTrickInsteadCostTrick(character, skill.GetId().SkillTemplateId, costTricks, null);
			this.RemoveCostTrickInsteadUselessTrick(character, skill.GetId().SkillTemplateId, costTricks, false);
			this.RemoveCostTrickBySelfShaTrick(this.Context, character, skill.GetId().SkillTemplateId, costTricks, false);
			this.RemoveCostTrickByEnemyShaTrick(this.Context, character, skill.GetId().SkillTemplateId, costTricks, null);
			this.RemoveCostTrickByJiTrick(this.Context, character, skill.GetId().SkillTemplateId, costTricks, false);
			this.RemoveJiTrickByUselessTrick(this.Context, character, skill.GetId().SkillTemplateId, costTricks, false);
			indexList.Clear();
			indexList.AddRange(trickCollection.Tricks.Keys);
			for (int i = 0; i < costTricks.Count; i++)
			{
				NeedTrick needTrick = costTricks[i];
				bool flag = needTrick.NeedCount > 0;
				if (flag)
				{
					int removeCounter = 0;
					for (int j = 0; j < indexList.Count; j++)
					{
						bool flag2 = character.TrickEquals(trickCollection.Tricks[indexList[j]], needTrick.TrickType);
						if (flag2)
						{
							removeCounter++;
							bool flag3 = removeCounter == (int)needTrick.NeedCount;
							if (flag3)
							{
								break;
							}
						}
					}
					bool flag4 = removeCounter < (int)needTrick.NeedCount;
					if (flag4)
					{
						result = false;
						break;
					}
				}
			}
			ObjectPool<List<int>>.Instance.Return(indexList);
			ObjectPool<List<NeedTrick>>.Instance.Return(costTricks);
			return result;
		}

		// Token: 0x060064AF RID: 25775 RVA: 0x00394010 File Offset: 0x00392210
		public void NpcCombatAttack(DataContext context, int attackerId, int defenderId, ItemKey weaponKey, short attackSkillId, short defendSkillId)
		{
			CombatCharacter attacker = this._combatCharacterDict[attackerId];
			CombatCharacter defender = this._combatCharacterDict[defenderId];
			this.SetCombatCharacter(context, attacker.IsAlly, attackerId);
			this.SetCombatCharacter(context, defender.IsAlly, defenderId);
			ItemKey[] weapons = attacker.GetWeapons();
			bool flag = !weaponKey.IsValid();
			if (flag)
			{
				weaponKey = weapons[3];
			}
			this.ChangeWeapon(context, attacker, weapons.IndexOf(weaponKey), false, false);
			bool flag2 = defendSkillId >= 0;
			if (flag2)
			{
				this.ApplyAgileOrDefenseSkill(defender, Config.CombatSkill.Instance[defendSkillId]);
			}
			bool flag3 = attackSkillId >= 0;
			if (flag3)
			{
				CombatSkillItem skillConfig = Config.CombatSkill.Instance[attackSkillId];
				this.CalcSkillQiDisorderAndInjury(attacker, skillConfig);
				attacker.SkillAttackBodyPart = this.GetAttackBodyPart(attacker, defender, context.Random, attackSkillId, -1, -1);
				Events.RaisePrepareSkillEnd(context, attackerId, attacker.IsAlly, attackSkillId);
				this.CalcAttackSkillDataCompare(CombatContext.Create(attacker, null, -1, attackSkillId, -1, null));
				Events.RaiseCastAttackSkillBegin(context, attacker, defender, attackSkillId);
				for (int i = 0; i < 4; i++)
				{
					bool flag4 = i == 3 || attacker.SkillHitType[i] >= 0;
					if (flag4)
					{
						this.CalcSkillAttack(CombatContext.Create(attacker, null, -1, -1, -1, null), i);
					}
					bool flag5 = i < 3;
					if (flag5)
					{
						attacker.SetAttackSkillAttackIndex((byte)(i + 1), context);
					}
				}
				sbyte power = (sbyte)attacker.GetAttackSkillPower();
				attacker.SetPerformingSkillId(-1, context);
				attacker.SetAttackSkillPower(0, context);
				this.ClearDamageCompareData(context);
				DomainManager.Combat.RaiseCastSkillEnd(context, attackerId, attacker.IsAlly, attackSkillId, power, false, 0);
			}
			else
			{
				sbyte[] weaponTricks = attacker.GetWeaponTricks();
				sbyte trickType = weaponTricks[context.Random.Next(0, weaponTricks.Length)];
				attacker.NormalAttackHitType = this.GetAttackHitType(attacker, trickType);
				attacker.NormalAttackBodyPart = this.GetAttackBodyPart(attacker, this._enemyChar, context.Random, -1, trickType, attacker.NormalAttackHitType);
				this.UpdateDamageCompareData(CombatContext.Create(attacker, null, -1, -1, -1, null));
				this.CalcNormalAttack(CombatContext.Create(attacker, null, -1, -1, -1, null), trickType);
				defender.SetIsFightBack(false, context);
			}
			this.ClearAffectingDefenseSkill(context, defender);
		}

		// Token: 0x060064B0 RID: 25776 RVA: 0x00394268 File Offset: 0x00392468
		[Obsolete]
		public void EndNpcCombat(DataContext context)
		{
		}

		// Token: 0x060064B1 RID: 25777 RVA: 0x0039426C File Offset: 0x0039246C
		public unsafe void NpcSimplifiedAttack(DataContext context, GameData.Domains.Character.Character attacker, GameData.Domains.Character.Character defender, GameData.Domains.CombatSkill.CombatSkill combatSkill, ItemKey weaponKey, CombatType combatType)
		{
			sbyte damageCount = AiHelper.NpcCombat.CombatTypeWeaponAttackInjuryCount[(int)combatType];
			sbyte poisonScale = AiHelper.NpcCombat.CombatTypePoisonScale[(int)combatType];
			sbyte innerRatio = Config.Weapon.Instance[weaponKey.TemplateId].DefaultInnerRatio;
			bool flag = combatSkill != null;
			PoisonsAndLevels poisonsAndLevels;
			if (flag)
			{
				poisonsAndLevels = combatSkill.GetPoisons();
				damageCount = AiHelper.NpcCombat.CombatTypeSkillAttackInjuryCount[(int)combatType];
				innerRatio = combatSkill.GetCurrInnerRatio();
			}
			bool flag2 = ModificationStateHelper.IsActive(weaponKey.ModificationState, 1);
			if (flag2)
			{
				poisonsAndLevels.Add(DomainManager.Item.GetAttachedPoisons(weaponKey));
			}
			int innerDamage = (int)(damageCount * innerRatio / 100);
			int outerDamage = (int)damageCount - innerDamage;
			defender.TakeRandomDamage(context, innerDamage, true);
			defender.TakeRandomDamage(context, outerDamage, false);
			bool flag3 = poisonScale != 100;
			if (flag3)
			{
				for (sbyte poisonType = 0; poisonType < 6; poisonType += 1)
				{
					bool flag4 = *(ref poisonsAndLevels.Values.FixedElementField + (IntPtr)poisonType * 2) > 0;
					if (flag4)
					{
						*(ref poisonsAndLevels.Values.FixedElementField + (IntPtr)poisonType * 2) = *(ref poisonsAndLevels.Values.FixedElementField + (IntPtr)poisonType * 2) * (short)poisonScale / 100;
					}
				}
			}
			defender.ChangePoisoned(context, ref poisonsAndLevels);
			bool flag5 = combatType != CombatType.Play && combatSkill != null;
			if (flag5)
			{
				sbyte direction = combatSkill.GetDirection();
				bool flag6 = direction != -1;
				if (flag6)
				{
					CombatSkillItem skillConfig = Config.CombatSkill.Instance[combatSkill.GetId().SkillTemplateId];
					bool isDirect = direction == 0;
					bool flag7 = skillConfig.AddWugType >= 0;
					if (flag7)
					{
						GameData.Domains.Character.Character wugChar = isDirect ? defender : attacker;
						sbyte growthType = WugEffectBase.GetNpcCombatAddGrowthType(wugChar, skillConfig.AddWugType, isDirect);
						short wugTemplateId = ItemDomain.GetWugTemplateId(skillConfig.AddWugType, growthType);
						bool flag8 = wugChar.GetCreatingType() == 1 && (*wugChar.GetEatingItems()).IndexOfWug(Config.Medicine.Instance[wugTemplateId]) < 0;
						if (flag8)
						{
							wugChar.AddWug(context, wugTemplateId);
						}
					}
					bool flag9 = skillConfig.AddBreakBodyFeature != null;
					if (flag9)
					{
						short featureId = skillConfig.AddBreakBodyFeature[isDirect ? 0 : 1];
						Injuries injuries = defender.GetInjuries();
						sbyte[] bodyParts = BreakFeatureHelper.Feature2BodyPart[featureId];
						bool hasAnyInjury = false;
						for (int i = 0; i < bodyParts.Length; i++)
						{
							bool flag10 = injuries.Get(bodyParts[i], !isDirect) > 0;
							if (flag10)
							{
								hasAnyInjury = true;
								break;
							}
						}
						bool flag11 = hasAnyInjury && !defender.GetFeatureIds().Contains(featureId);
						if (flag11)
						{
							defender.AddFeature(context, featureId, false);
							DomainManager.SpecialEffect.Add(context, defender.GetId(), SpecialEffectDomain.BreakBodyFeatureEffectClassName[featureId]);
						}
					}
				}
			}
		}

		// Token: 0x060064B2 RID: 25778 RVA: 0x00394524 File Offset: 0x00392724
		[Obsolete]
		public unsafe void NpcAttackQuick(DataContext context, int attackerId, int defenderId, ItemKey weaponKey, ItemKey emptyHandWeaponKey, short attackSkillId, short defendSkillId, sbyte skillPower = 100)
		{
			bool flag = !emptyHandWeaponKey.IsValid();
			if (flag)
			{
				throw new Exception("emptyHandWeaponKey cannot be invalid");
			}
			bool flag2 = !weaponKey.IsValid();
			if (flag2)
			{
				weaponKey = emptyHandWeaponKey;
			}
			GameData.Domains.Character.Character attacker = DomainManager.Character.GetElement_Objects(attackerId);
			GameData.Domains.Character.Character defender = DomainManager.Character.GetElement_Objects(defenderId);
			GameData.Domains.Item.Weapon weapon = DomainManager.Item.GetElement_Weapons(weaponKey.Id);
			List<sbyte> weaponTricks = weapon.GetTricks();
			sbyte trickType = weaponTricks[context.Random.Next(0, weaponTricks.Count)];
			sbyte hitType = this.GetAttackHitType(attacker, trickType);
			sbyte bodyPart = this.GetNpcAttackBodyPart(attacker, defender, context.Random, weapon, attackSkillId, (attackSkillId >= 0) ? -1 : trickType, hitType);
			bool flag3 = attackSkillId >= 0;
			if (flag3)
			{
				int power = this.CalcNpcSkillAttack(context, attacker, defender, bodyPart, weapon, emptyHandWeaponKey, attackSkillId, defendSkillId, skillPower);
				bool flag4 = power >= 100;
				if (flag4)
				{
					sbyte direction = DomainManager.CombatSkill.GetSkillDirection(attackerId, attackSkillId);
					bool flag5 = direction != -1;
					if (flag5)
					{
						CombatSkillItem skillConfig = Config.CombatSkill.Instance[attackSkillId];
						bool isDirect = direction == 0;
						bool flag6 = skillConfig.AddWugType >= 0;
						if (flag6)
						{
							GameData.Domains.Character.Character wugChar = isDirect ? defender : attacker;
							sbyte growthType = WugEffectBase.GetNpcCombatAddGrowthType(wugChar, skillConfig.AddWugType, isDirect);
							short wugTemplateId = ItemDomain.GetWugTemplateId(skillConfig.AddWugType, growthType);
							bool flag7 = (*wugChar.GetEatingItems()).IndexOfWug(Config.Medicine.Instance[wugTemplateId]) < 0;
							if (flag7)
							{
								wugChar.AddWug(context, wugTemplateId);
							}
						}
						bool flag8 = skillConfig.AddBreakBodyFeature != null;
						if (flag8)
						{
							short featureId = skillConfig.AddBreakBodyFeature[isDirect ? 0 : 1];
							Injuries injuries = defender.GetInjuries();
							sbyte[] bodyParts = BreakFeatureHelper.Feature2BodyPart[featureId];
							bool hasAnyInjury = false;
							for (int i = 0; i < bodyParts.Length; i++)
							{
								bool flag9 = injuries.Get(bodyParts[i], !isDirect) > 0;
								if (flag9)
								{
									hasAnyInjury = true;
									break;
								}
							}
							bool flag10 = hasAnyInjury && !defender.GetFeatureIds().Contains(featureId);
							if (flag10)
							{
								defender.AddFeature(context, featureId, false);
								DomainManager.SpecialEffect.Add(context, defenderId, SpecialEffectDomain.BreakBodyFeatureEffectClassName[featureId]);
							}
						}
					}
				}
			}
			else
			{
				this.CalcNpcNormalAttack(context, attacker, defender, trickType, hitType, bodyPart, weapon, emptyHandWeaponKey, defendSkillId, skillPower);
			}
		}

		// Token: 0x060064B3 RID: 25779 RVA: 0x00394798 File Offset: 0x00392998
		private sbyte GetNpcAttackBodyPart(GameData.Domains.Character.Character attacker, GameData.Domains.Character.Character defender, IRandomSource random, GameData.Domains.Item.Weapon weapon, short skillId = -1, sbyte trickType = -1, sbyte hitType = -1)
		{
			bool flag = (skillId >= 0 && CombatSkillTemplateHelper.IsMindHitSkill(skillId)) || (trickType >= 0 && hitType == 3);
			sbyte result2;
			if (flag)
			{
				result2 = -1;
			}
			else
			{
				bool flag2 = trickType == 21;
				if (flag2)
				{
					trickType = CombatDomain.GodTrickUseTrickType[hitType];
				}
				Injuries injuries = defender.GetInjuries();
				sbyte innerRatio = (skillId >= 0) ? DomainManager.CombatSkill.GetElement_CombatSkills(new CombatSkillKey(attacker.GetId(), skillId)).GetCurrInnerRatio() : Config.Weapon.Instance[weapon.GetTemplateId()].DefaultInnerRatio;
				sbyte attackBodyPart = -1;
				List<sbyte> attackOdds = ObjectPool<List<sbyte>>.Instance.Get();
				sbyte[] attackPartDistribution = null;
				bool flag3 = skillId >= 0 && Config.CombatSkill.Instance[skillId].InjuryPartAtkRateDistribution != null;
				if (flag3)
				{
					attackPartDistribution = Config.CombatSkill.Instance[skillId].InjuryPartAtkRateDistribution;
				}
				else
				{
					bool flag4 = trickType >= 0 && TrickType.Instance[trickType].InjuryPartAtkRateDistribution != null;
					if (flag4)
					{
						attackPartDistribution = TrickType.Instance[trickType].InjuryPartAtkRateDistribution;
					}
				}
				attackOdds.Clear();
				attackOdds.AddRange(attackPartDistribution);
				bool flag5 = attackOdds.Count > 0;
				if (flag5)
				{
					for (sbyte part = 0; part < 7; part += 1)
					{
						bool flag6 = (innerRatio == 100 || injuries.Get(part, false) >= 6) && (innerRatio == 0 || injuries.Get(part, true) >= 6);
						if (flag6)
						{
							attackOdds[(int)part] = 0;
						}
					}
					int sum = attackOdds.ToArray().Sum();
					bool flag7 = sum == 0;
					if (flag7)
					{
						attackOdds.Clear();
						attackOdds.AddRange(attackPartDistribution);
						sum = attackOdds.ToArray().Sum();
					}
					int result = random.Next(sum);
					int accumulator = 0;
					for (sbyte part2 = 0; part2 < 7; part2 += 1)
					{
						accumulator += (int)attackOdds[(int)part2];
						bool flag8 = accumulator > result;
						if (flag8)
						{
							attackBodyPart = part2;
							break;
						}
					}
				}
				ObjectPool<List<sbyte>>.Instance.Return(attackOdds);
				result2 = attackBodyPart;
			}
			return result2;
		}

		// Token: 0x060064B4 RID: 25780 RVA: 0x003949B0 File Offset: 0x00392BB0
		private unsafe int GetNpcAttackHitValue(GameData.Domains.Character.Character attacker, GameData.Domains.Item.Weapon weapon, ItemKey emptyHandWeaponKey, sbyte hitType, short attackSkillId = -1, int skillAddPercent = 0)
		{
			HitOrAvoidInts hitValues = attacker.GetHitValues();
			int charId = attacker.GetId();
			int hitValue = *(ref hitValues.Items.FixedElementField + (IntPtr)hitType * 4);
			int percent = 100;
			bool flag = attackSkillId < 0 || Config.CombatSkill.Instance[attackSkillId].Type != 5;
			if (flag)
			{
				percent += (int)(*(ref weapon.GetHitFactors(charId).Items.FixedElementField + (IntPtr)hitType * 2));
			}
			else
			{
				ItemKey shoesKey = attacker.GetEquipment()[7];
				GameData.Domains.Item.Armor shoes = shoesKey.IsValid() ? DomainManager.Item.GetElement_Armors(shoesKey.Id) : null;
				bool flag2 = shoes != null && shoes.GetCurrDurability() > 0;
				if (flag2)
				{
					short shoesWeaponTemplateId = Config.Armor.Instance[shoesKey.TemplateId].RelatedWeapon;
					ItemKey shoesWeaponKey = new ItemKey(0, 0, shoesWeaponTemplateId, -1);
					percent += (int)(*(ref Config.Weapon.Instance[shoesWeaponTemplateId].BaseHitFactors.Items.FixedElementField + (IntPtr)hitType * 2) * DomainManager.Character.GetItemPower(charId, shoesWeaponKey) / 100);
				}
				else
				{
					GameData.Domains.Item.Weapon emptyHandWeapon = DomainManager.Item.GetElement_Weapons(emptyHandWeaponKey.Id);
					percent += (int)(*(ref emptyHandWeapon.GetHitFactors(charId).Items.FixedElementField + (IntPtr)hitType * 2));
				}
			}
			percent = Math.Max(percent, 33);
			hitValue = hitValue * percent / 100;
			hitValue = hitValue * (100 + skillAddPercent) / 100;
			return Math.Max(hitValue, 0);
		}

		// Token: 0x060064B5 RID: 25781 RVA: 0x00394B34 File Offset: 0x00392D34
		private unsafe int GetNpcAttackAvoidValue(GameData.Domains.Character.Character defender, sbyte hitType, sbyte bodyPart, short defendSkillId, sbyte skillPower)
		{
			HitOrAvoidInts avoidValues = defender.GetAvoidValues();
			int charId = defender.GetId();
			int avoidValue = *(ref avoidValues.Items.FixedElementField + (IntPtr)hitType * 4);
			bool flag = defendSkillId >= 0;
			if (flag)
			{
				GameData.Domains.CombatSkill.CombatSkill defendSkill = DomainManager.CombatSkill.GetElement_CombatSkills(new CombatSkillKey(charId, defendSkillId));
				HitOrAvoidInts addAvoids = defendSkill.GetAddAvoidValueOnCast();
				avoidValue += (int)GlobalConfig.Instance.DefendSkillBaseAddAvoid * *(ref addAvoids.Items.FixedElementField + (IntPtr)hitType * 4) / 100;
			}
			int percent = 100;
			bool flag2 = bodyPart >= 0;
			if (flag2)
			{
				ItemKey armorKey = defender.GetEquipment()[(int)EquipmentSlotHelper.GetSlotByBodyPartType(bodyPart)];
				bool flag3 = armorKey.IsValid();
				if (flag3)
				{
					GameData.Domains.Item.Armor armor = DomainManager.Item.GetElement_Armors(armorKey.Id);
					bool flag4 = armor.GetCurrDurability() > 0;
					if (flag4)
					{
						percent += (int)(*(ref armor.GetAvoidFactors(charId).Items.FixedElementField + (IntPtr)hitType * 2));
					}
				}
			}
			percent = Math.Max(percent, 33);
			avoidValue = avoidValue * percent / 100;
			return Math.Max(avoidValue, 1);
		}

		// Token: 0x060064B6 RID: 25782 RVA: 0x00394C48 File Offset: 0x00392E48
		private int GetNpcAttackPenetrate(GameData.Domains.Character.Character attacker, GameData.Domains.Character.Character defender, bool inner, GameData.Domains.Item.Weapon weapon, ItemKey emptyHandWeaponKey, sbyte bodyPart, short attackSkillId = -1, int skillAddPercent = 0)
		{
			int charId = attacker.GetId();
			int attackValue = inner ? attacker.GetPenetrations().Inner : attacker.GetPenetrations().Outer;
			bool isLegSkill = attackSkillId >= 0 && Config.CombatSkill.Instance[attackSkillId].Type == 5;
			ItemKey shoesKey = attacker.GetEquipment()[7];
			GameData.Domains.Item.Armor shoes = shoesKey.IsValid() ? DomainManager.Item.GetElement_Armors(shoesKey.Id) : null;
			bool flag = !isLegSkill;
			int totalFactor;
			int innerFactor;
			if (flag)
			{
				totalFactor = (int)(weapon.GetPenetrationFactor() * DomainManager.Character.GetItemPower(charId, weapon.GetItemKey()) / 100);
				innerFactor = totalFactor * (int)Config.Weapon.Instance[weapon.GetTemplateId()].DefaultInnerRatio / 100;
			}
			else
			{
				bool flag2 = shoes != null && shoes.GetCurrDurability() > 0;
				if (flag2)
				{
					short shoesWeaponTemplateId = Config.Armor.Instance[shoesKey.TemplateId].RelatedWeapon;
					ItemKey shoesWeaponKey = new ItemKey(0, 0, shoesWeaponTemplateId, -1);
					WeaponItem shoesWeaponConfig = Config.Weapon.Instance[shoesWeaponTemplateId];
					totalFactor = (int)(shoesWeaponConfig.BasePenetrationFactor * DomainManager.Character.GetItemPower(charId, shoesWeaponKey) / 100);
					innerFactor = totalFactor * (int)shoesWeaponConfig.DefaultInnerRatio / 100;
				}
				else
				{
					GameData.Domains.Item.Weapon emptyHandWeapon = DomainManager.Item.GetElement_Weapons(emptyHandWeaponKey.Id);
					totalFactor = (int)(emptyHandWeapon.GetPenetrationFactor() * DomainManager.Character.GetItemPower(charId, emptyHandWeaponKey) / 100);
					innerFactor = totalFactor * (int)Config.Weapon.Instance[emptyHandWeaponKey.TemplateId].DefaultInnerRatio / 100;
				}
			}
			int weaponFactor = inner ? innerFactor : (totalFactor - innerFactor);
			bool flag3 = bodyPart >= 0;
			if (flag3)
			{
				ItemKey armorKey = defender.GetEquipment()[(int)EquipmentSlotHelper.GetSlotByBodyPartType(bodyPart)];
				GameData.Domains.Item.Armor armor = armorKey.IsValid() ? DomainManager.Item.GetElement_Armors(armorKey.Id) : null;
				int weaponEquipDefense = (int)((!isLegSkill) ? weapon.GetEquipmentDefense() : ((shoes != null && shoes.GetCurrDurability() > 0) ? shoes.GetEquipmentDefense() : 50));
				int armorEquipAttack = (int)((armor != null && armor.GetCurrDurability() > 0) ? armor.GetEquipmentAttack() : 100);
				bool flag4 = armorEquipAttack > weaponEquipDefense;
				if (flag4)
				{
					weaponFactor -= weaponFactor * Math.Min(20 + 10 * armorEquipAttack / Math.Max(weaponEquipDefense, 1), 100) / 100;
				}
			}
			attackValue += weaponFactor;
			attackValue = attackValue * (100 + skillAddPercent) / 100;
			return Math.Max(attackValue, 0);
		}

		// Token: 0x060064B7 RID: 25783 RVA: 0x00394EB8 File Offset: 0x003930B8
		private unsafe int GetNpcAttackPenetrateResist(GameData.Domains.Character.Character attacker, GameData.Domains.Character.Character defender, bool inner, GameData.Domains.Item.Weapon weapon, sbyte bodyPart, short defendSkillId, sbyte skillPower, short attackSkillId = -1)
		{
			int charId = defender.GetId();
			int defendValue = inner ? defender.GetPenetrationResists().Inner : defender.GetPenetrationResists().Outer;
			PoisonInts poison = *defender.GetPoisoned();
			int armorFactor = 0;
			bool flag = bodyPart >= 0;
			if (flag)
			{
				ItemKey armorKey = defender.GetEquipment()[(int)EquipmentSlotHelper.GetSlotByBodyPartType(bodyPart)];
				GameData.Domains.Item.Armor armor = armorKey.IsValid() ? DomainManager.Item.GetElement_Armors(armorKey.Id) : null;
				bool flag2 = armor != null;
				if (flag2)
				{
					OuterAndInnerShorts penetrationResistFactors = armor.GetPenetrationResistFactors();
					armorFactor = (int)((inner ? penetrationResistFactors.Inner : penetrationResistFactors.Outer) * DomainManager.Character.GetItemPower(charId, armorKey) / 100);
				}
				else
				{
					armorFactor = 0;
				}
				int weaponEquipAttack = (int)weapon.GetEquipmentAttack();
				int armorEquipDefense = (int)((armor != null && armor.GetCurrDurability() > 0) ? armor.GetEquipmentDefense() : 50);
				bool flag3 = attackSkillId >= 0 && Config.CombatSkill.Instance[attackSkillId].Type == 5;
				if (flag3)
				{
					ItemKey shoesKey = attacker.GetEquipment()[7];
					GameData.Domains.Item.Armor shoes = shoesKey.IsValid() ? DomainManager.Item.GetElement_Armors(shoesKey.Id) : null;
					weaponEquipAttack = (int)((shoes != null && shoes.GetCurrDurability() > 0) ? shoes.GetEquipmentAttack() : 100);
				}
				bool flag4 = weaponEquipAttack > armorEquipDefense;
				if (flag4)
				{
					armorFactor -= armorFactor * Math.Min(20 + 10 * weaponEquipAttack / Math.Max(armorEquipDefense, 1), 100) / 100;
				}
			}
			defendValue += armorFactor;
			bool flag5 = defendSkillId >= 0;
			if (flag5)
			{
				GameData.Domains.CombatSkill.CombatSkill defendSkill = DomainManager.CombatSkill.GetElement_CombatSkills(new CombatSkillKey(charId, defendSkillId));
				OuterAndInnerInts addPenetrateResists = defendSkill.GetAddPenetrateResist();
				defendValue += (inner ? addPenetrateResists.Inner : addPenetrateResists.Outer);
			}
			int percent = 100;
			sbyte poisonType = inner ? 5 : 4;
			sbyte poisonLevel = PoisonsAndLevels.CalcPoisonedLevel(*(ref poison.Items.FixedElementField + (IntPtr)poisonType * 4));
			bool flag6 = poisonLevel > 0;
			if (flag6)
			{
				if (inner)
				{
					percent -= (int)((short)poisonLevel * Poison.Instance[poisonType].ReduceInnerResist);
				}
				else
				{
					percent -= (int)((short)poisonLevel * Poison.Instance[poisonType].ReduceOuterResist);
				}
			}
			percent = Math.Max(percent, 33);
			defendValue = defendValue * percent / 100;
			return Math.Max(defendValue, 1);
		}

		// Token: 0x060064B8 RID: 25784 RVA: 0x00395110 File Offset: 0x00393310
		private unsafe void CalcNpcNormalAttack(DataContext context, GameData.Domains.Character.Character attacker, GameData.Domains.Character.Character defender, sbyte trickType, sbyte hitType, sbyte bodyPart, GameData.Domains.Item.Weapon weapon, ItemKey emptyHandWeaponKey, short defendSkillId, sbyte skillPower)
		{
			int hitValue = this.GetNpcAttackHitValue(attacker, weapon, emptyHandWeaponKey, hitType, -1, 0);
			int avoidValue = this.GetNpcAttackAvoidValue(defender, hitType, bodyPart, defendSkillId, skillPower);
			int hitOdds = hitValue * 100 / avoidValue / ((hitValue < avoidValue) ? 2 : 1);
			bool hit = context.Random.CheckPercentProb(hitOdds);
			bool flag = !hit || TrickType.NoBodyDamageTrickType.Exist(trickType);
			if (!flag)
			{
				bool flag2 = hitType != 3;
				if (flag2)
				{
					this.CalcNpcAttackInjury(context, attacker, defender, weapon, bodyPart, hitValue, avoidValue, Config.Weapon.Instance[weapon.GetTemplateId()].DefaultInnerRatio, this.GetNpcAttackPenetrate(attacker, defender, false, weapon, emptyHandWeaponKey, bodyPart, -1, 0), this.GetNpcAttackPenetrateResist(attacker, defender, false, weapon, bodyPart, defendSkillId, skillPower, -1), this.GetNpcAttackPenetrate(attacker, defender, true, weapon, emptyHandWeaponKey, bodyPart, -1, 0), this.GetNpcAttackPenetrateResist(attacker, defender, true, weapon, bodyPart, defendSkillId, skillPower, -1), -1, 100);
				}
				PoisonsAndLevels poisons = *weapon.GetInnatePoisons();
				for (sbyte type = 0; type < 6; type += 1)
				{
					bool flag3 = *(ref poisons.Levels.FixedElementField + type) > 0;
					if (flag3)
					{
						defender.ChangePoisoned(context, type, *(ref poisons.Levels.FixedElementField + type), (int)(*(ref poisons.Values.FixedElementField + (IntPtr)type * 2)));
					}
				}
			}
		}

		// Token: 0x060064B9 RID: 25785 RVA: 0x00395264 File Offset: 0x00393464
		private unsafe int CalcNpcSkillAttack(DataContext context, GameData.Domains.Character.Character attacker, GameData.Domains.Character.Character defender, sbyte bodyPart, GameData.Domains.Item.Weapon weapon, ItemKey emptyHandWeaponKey, short attackSkillId, short defendSkillId, sbyte skillPower)
		{
			GameData.Domains.CombatSkill.CombatSkill skill = DomainManager.CombatSkill.GetElement_CombatSkills(new CombatSkillKey(attacker.GetId(), attackSkillId));
			HitOrAvoidInts skillHitValue = skill.GetHitValue();
			int hittedPower = 0;
			bool flag = !CombatSkillTemplateHelper.IsMindHitSkill(attackSkillId);
			if (flag)
			{
				HitOrAvoidInts hitDistribution = skill.GetHitDistribution();
				List<int> hitValueList = ObjectPool<List<int>>.Instance.Get();
				List<int> avoidValueList = ObjectPool<List<int>>.Instance.Get();
				int maxHitValue = int.MinValue;
				int maxAvoidValue = int.MinValue;
				hitValueList.Clear();
				avoidValueList.Clear();
				for (sbyte hitType = 2; hitType >= 0; hitType -= 1)
				{
					int hitValue = this.GetNpcAttackHitValue(attacker, weapon, emptyHandWeaponKey, hitType, attackSkillId, *(ref skillHitValue.Items.FixedElementField + (IntPtr)hitType * 4) * (int)skillPower / 100);
					int avoidValue = this.GetNpcAttackAvoidValue(defender, hitType, bodyPart, defendSkillId, skillPower);
					bool flag2 = hitValue > maxHitValue || (hitValue == maxHitValue && avoidValue > maxAvoidValue);
					if (flag2)
					{
						maxHitValue = hitValue;
						maxAvoidValue = avoidValue;
					}
					hitValueList.Add(hitValue);
					avoidValueList.Add(avoidValue);
				}
				for (int i = 0; i < 3; i++)
				{
					int hitValue2 = hitValueList[i];
					int avoidValue2 = avoidValueList[i];
					int hitOdds = hitValue2 * 100 / avoidValue2 / ((hitValue2 < avoidValue2) ? 2 : 1);
					bool flag3 = context.Random.CheckPercentProb(hitOdds);
					if (flag3)
					{
						hittedPower += *(ref hitDistribution.Items.FixedElementField + (IntPtr)i * 4);
					}
				}
				ObjectPool<List<int>>.Instance.Return(hitValueList);
				ObjectPool<List<int>>.Instance.Return(avoidValueList);
				bool flag4 = hittedPower > 0;
				if (flag4)
				{
					OuterAndInnerInts skillPenetrate = skill.GetPenetrations();
					this.CalcNpcAttackInjury(context, attacker, defender, weapon, bodyPart, maxHitValue, maxAvoidValue, skill.GetCurrInnerRatio(), this.GetNpcAttackPenetrate(attacker, defender, false, weapon, emptyHandWeaponKey, bodyPart, attackSkillId, skillPenetrate.Outer * (int)skillPower / 100), this.GetNpcAttackPenetrateResist(attacker, defender, false, weapon, bodyPart, defendSkillId, skillPower, attackSkillId), this.GetNpcAttackPenetrate(attacker, defender, true, weapon, emptyHandWeaponKey, bodyPart, attackSkillId, skillPenetrate.Inner * (int)skillPower / 100), this.GetNpcAttackPenetrateResist(attacker, defender, true, weapon, bodyPart, defendSkillId, skillPower, attackSkillId), attackSkillId, hittedPower);
				}
			}
			else
			{
				int hitValue3 = this.GetNpcAttackHitValue(attacker, weapon, emptyHandWeaponKey, 3, attackSkillId, *(ref skillHitValue.Items.FixedElementField + (IntPtr)3 * 4) * (int)skillPower / 100);
				int avoidValue3 = this.GetNpcAttackAvoidValue(defender, 3, bodyPart, defendSkillId, skillPower);
				int hitOdds2 = hitValue3 * 100 / avoidValue3 / ((hitValue3 < avoidValue3) ? 2 : 1);
				hittedPower = (context.Random.CheckPercentProb(hitOdds2) ? 100 : 0);
			}
			bool flag5 = hittedPower > 0;
			if (flag5)
			{
				PoisonsAndLevels poisons = skill.GetPoisons();
				for (sbyte type = 0; type < 6; type += 1)
				{
					bool flag6 = *(ref poisons.Levels.FixedElementField + type) > 0;
					if (flag6)
					{
						defender.ChangePoisoned(context, type, *(ref poisons.Levels.FixedElementField + type), (int)(*(ref poisons.Values.FixedElementField + (IntPtr)type * 2)) * hittedPower / 100);
					}
				}
			}
			return hittedPower;
		}

		// Token: 0x060064BA RID: 25786 RVA: 0x0039557C File Offset: 0x0039377C
		private void CalcNpcAttackInjury(DataContext context, GameData.Domains.Character.Character attacker, GameData.Domains.Character.Character defender, GameData.Domains.Item.Weapon weapon, sbyte bodyPart, int hitValue, int avoidValue, sbyte innerRatio, int outerPenetrate, int outerPenetrateResist, int innerPenetrate, int innerPenetrateResist, short attackSkillId = -1, int attackSkillPower = 100)
		{
			bool flag = bodyPart < 0;
			if (!flag)
			{
				Injuries injuries = defender.GetInjuries();
				DamageStepCollection damageStepCollection = this.GetDamageStepCollection(defender.GetId());
				int outerAttack = outerPenetrate * attackSkillPower / outerPenetrateResist / ((outerPenetrate < outerPenetrateResist) ? 2 : 1);
				int innerAttack = innerPenetrate * attackSkillPower / innerPenetrateResist / ((innerPenetrate < innerPenetrateResist) ? 2 : 1);
				int hitOdds = CFormula.FormulaCalcHitOdds(hitValue, avoidValue);
				int criticalPercent = CombatDomain.CheckCritical(context.Random, attacker.GetId(), hitOdds, false) ? CFormula.FormulaCalcCriticalPercent(hitOdds) : 100;
				int baseDamage = CFormula.CalcBaseDamageValue((attackSkillId >= 0) ? CFormula.EAttackType.Skill : CFormula.EAttackType.Normal, weapon.GetAttackPreparePointCost());
				int damageValue = baseDamage * (60 + 80 * Math.Abs((int)(innerRatio - 50)) / 100) / 100;
				int outerDamage = (innerRatio < 100) ? (damageValue * (baseDamage / 2 + outerAttack / 4) / 100 * criticalPercent / 100) : 0;
				int innerDamage = (innerRatio > 0) ? (damageValue * (baseDamage / 2 + innerAttack / 4) / 100 * criticalPercent / 100) : 0;
				int outerInjury = (innerRatio < 100) ? CombatDomain.CalcInjuryMarkCount(outerDamage, damageStepCollection.OuterDamageSteps[(int)bodyPart], (int)(6 - injuries.Get(bodyPart, false))).Item1 : 0;
				int innerInjury = (innerRatio > 0) ? CombatDomain.CalcInjuryMarkCount(innerDamage, damageStepCollection.InnerDamageSteps[(int)bodyPart], (int)(6 - injuries.Get(bodyPart, true))).Item1 : 0;
				ItemKey armorKey = defender.GetEquipment()[(int)EquipmentSlotHelper.GetSlotByBodyPartType(bodyPart)];
				GameData.Domains.Item.Armor armor = armorKey.IsValid() ? DomainManager.Item.GetElement_Armors(armorKey.Id) : null;
				bool flag2 = armor != null && armor.GetCurrDurability() > 0;
				if (flag2)
				{
					int weaponEquipAttack = (int)weapon.GetEquipmentAttack();
					bool flag3 = attackSkillId >= 0 && Config.CombatSkill.Instance[attackSkillId].Type == 5;
					if (flag3)
					{
						ItemKey shoesKey = attacker.GetEquipment()[7];
						GameData.Domains.Item.Armor shoes = shoesKey.IsValid() ? DomainManager.Item.GetElement_Armors(shoesKey.Id) : null;
						weaponEquipAttack = (int)((shoes != null && shoes.GetCurrDurability() > 0) ? shoes.GetEquipmentAttack() : 100);
					}
					bool flag4 = weaponEquipAttack <= (int)armor.GetEquipmentDefense();
					if (flag4)
					{
						OuterAndInnerShorts reduceInjury = armor.GetInjuryFactor();
						outerInjury -= (int)reduceInjury.Outer;
						innerInjury -= (int)reduceInjury.Inner;
					}
				}
				bool flag5 = outerInjury > 0 || innerInjury > 0;
				if (flag5)
				{
					bool flag6 = outerInjury > 0;
					if (flag6)
					{
						injuries.Change(bodyPart, false, (int)((sbyte)outerInjury));
					}
					bool flag7 = innerInjury > 0;
					if (flag7)
					{
						injuries.Change(bodyPart, true, (int)((sbyte)innerInjury));
					}
					defender.SetInjuries(injuries, context);
				}
			}
		}

		// Token: 0x170003D2 RID: 978
		// (get) Token: 0x060064BB RID: 25787 RVA: 0x00395815 File Offset: 0x00393A15
		public byte FleeNeedDistance
		{
			get
			{
				return this.CombatConfig.FleeDistance;
			}
		}

		// Token: 0x170003D3 RID: 979
		// (get) Token: 0x060064BC RID: 25788 RVA: 0x00395822 File Offset: 0x00393A22
		public byte InterruptFleeNeedDistance
		{
			get
			{
				return this.CombatConfig.FleeInterruptDistance;
			}
		}

		// Token: 0x060064BD RID: 25789 RVA: 0x00395830 File Offset: 0x00393A30
		[DomainMethod]
		public void StartPrepareOtherAction(DataContext context, sbyte actionType, bool isAlly = true)
		{
			bool flag = !this.CanAcceptCommand();
			if (!flag)
			{
				CombatCharacter combatChar = isAlly ? this._selfChar : this._enemyChar;
				bool flag2 = !combatChar.GetOtherActionCanUse()[(int)actionType];
				if (!flag2)
				{
					bool needUpdate = !combatChar.HasDoingOrReserveCommand();
					combatChar.SetNeedUseOtherAction(context, actionType);
					combatChar.MoveData.ResetJumpState(context, true);
					bool flag3 = needUpdate;
					if (flag3)
					{
						this.UpdateAllCommandAvailability(context, combatChar);
					}
					else
					{
						this.UpdateOtherActionCanUse(context, combatChar, -1);
					}
				}
			}
		}

		// Token: 0x060064BE RID: 25790 RVA: 0x003958B0 File Offset: 0x00393AB0
		[DomainMethod]
		public void InterruptSurrender()
		{
			bool flag = !this.IsInCombat();
			if (!flag)
			{
				bool flag2 = this._selfChar.GetPreparingOtherAction() == 4;
				if (flag2)
				{
					this._selfChar.NeedInterruptSurrender = true;
				}
			}
		}

		// Token: 0x060064BF RID: 25791 RVA: 0x003958EC File Offset: 0x00393AEC
		private void UpdateOtherActionCanUse(DataContext context, CombatCharacter character, sbyte actionType = -1)
		{
			bool[] canUseList = character.GetOtherActionCanUse();
			bool changed = false;
			bool flag = actionType < 0 || actionType == 0;
			if (flag)
			{
				bool canUse = character.GetHealInjuryCount() > 0 && !character.PreparingTeammateCommand() && (!character.GetInjuries().HasAnyInjury() || !this.GetHealInjuryBanReason(character, character).Any());
				bool flag2 = canUseList[0] != canUse;
				if (flag2)
				{
					canUseList[0] = canUse;
					changed = true;
				}
			}
			bool flag3 = actionType < 0 || actionType == 1;
			if (flag3)
			{
				bool canUse = character.GetHealPoisonCount() > 0 && !character.PreparingTeammateCommand() && (!character.GetPoison().IsNonZero() || !this.GetHealPoisonBanReason(character, character).Any());
				bool flag4 = canUseList[1] != canUse;
				if (flag4)
				{
					canUseList[1] = canUse;
					changed = true;
				}
			}
			bool flag5 = actionType < 0 || actionType == 2;
			if (flag5)
			{
				bool canUse = this.CanFlee(character.IsAlly) && !character.PreparingTeammateCommand() && this.GetCurrentDistance() >= (short)this.FleeNeedDistance;
				bool flag6 = canUseList[2] != canUse;
				if (flag6)
				{
					canUseList[2] = canUse;
					changed = true;
				}
			}
			bool flag7 = actionType < 0 || actionType == 3;
			if (flag7)
			{
				bool canUse = this._carrierAnimalCombatCharId >= 0 && !character.PreparingTeammateCommand() && character.GetAnimalAttackCount() > 0;
				bool flag8 = canUseList[3] != canUse;
				if (flag8)
				{
					canUseList[3] = canUse;
					changed = true;
				}
			}
			bool flag9 = actionType < 0 || actionType == 4;
			if (flag9)
			{
				bool canUse = character.GetCanSurrender();
				bool flag10 = canUseList[4] != canUse;
				if (flag10)
				{
					canUseList[4] = canUse;
					changed = true;
				}
			}
			bool flag11 = changed;
			if (flag11)
			{
				character.SetOtherActionCanUse(canUseList, context);
			}
		}

		// Token: 0x060064C0 RID: 25792 RVA: 0x00395AAC File Offset: 0x00393CAC
		public void InterruptOtherAction(DataContext context, CombatCharacter character)
		{
			bool flag = character.GetPreparingOtherAction() == 2;
			if (flag)
			{
				DomainManager.Combat.ShowSpecialEffectTips(character.GetId(), 1456, 0);
			}
			Events.RaiseInterruptOtherAction(context, character, character.GetPreparingOtherAction());
			character.SetPreparingOtherAction(-1, context);
			this.SetProperLoopAniAndParticle(context, character, false);
		}

		// Token: 0x060064C1 RID: 25793 RVA: 0x00395B00 File Offset: 0x00393D00
		public bool CanFlee(bool isAlly)
		{
			return isAlly ? this.CombatConfig.SelfCanFlee : this.CombatConfig.EnemyCanFlee;
		}

		// Token: 0x060064C2 RID: 25794 RVA: 0x00395B30 File Offset: 0x00393D30
		public unsafe void SetPoisons(DataContext context, CombatCharacter character, PoisonInts poisons, bool updateDefeatMark = true)
		{
			PoisonInts oldPoison = *character.GetOldPoison();
			bool oldPoisonChanged = false;
			for (sbyte type = 0; type < 6; type += 1)
			{
				bool flag = *(ref poisons.Items.FixedElementField + (IntPtr)type * 4) < *(ref oldPoison.Items.FixedElementField + (IntPtr)type * 4);
				if (flag)
				{
					*(ref oldPoison.Items.FixedElementField + (IntPtr)type * 4) = *(ref poisons.Items.FixedElementField + (IntPtr)type * 4);
					oldPoisonChanged = true;
				}
			}
			bool flag2 = oldPoisonChanged;
			if (flag2)
			{
				character.SetOldPoison(ref oldPoison, context);
			}
			character.SetPoison(ref poisons, context);
			character.SyncPoisonData(context);
			if (updateDefeatMark)
			{
				this.UpdatePoisonDefeatMark(context, character);
			}
			this.UpdateOtherActionCanUse(context, character, 1);
			bool flag3 = this.IsMainCharacter(character);
			if (flag3)
			{
				this.UpdateAllTeammateCommandUsable(context, character.IsAlly, ETeammateCommandImplement.HealPoison);
			}
		}

		// Token: 0x060064C3 RID: 25795 RVA: 0x00395C10 File Offset: 0x00393E10
		public unsafe void AddPoison(DataContext context, CombatCharacter attacker, CombatCharacter defender, sbyte poisonType, sbyte level, int addValue, short skillId = -1, bool applySpecialEffect = true, bool canBounce = true, ItemKey equipKey = default(ItemKey), bool isDirectPoison = false, bool ignorePositiveResist = false, bool forceChangeToOld = false)
		{
			int attackerId = (attacker != null) ? attacker.GetId() : -1;
			defender = DomainManager.SpecialEffect.ModifyData(attackerId, skillId, 246, defender, -1, -1, -1);
			defender = DomainManager.SpecialEffect.ModifyData(defender.GetId(), skillId, 247, defender, -1, -1, -1);
			int defenderId = defender.GetId();
			bool flag = defender.GetCharacter().GetPoisonImmunities()[(int)poisonType] || DomainManager.Extra.HasPoisonImmunity(defenderId, poisonType);
			if (!flag)
			{
				PoisonInts poisons = *defender.GetPoison();
				int poisonResist = *defender.GetPoisonResist()[(int)poisonType];
				bool flag2 = ignorePositiveResist && poisonResist > 0 && poisonResist < 1000;
				if (flag2)
				{
					poisonResist = 0;
				}
				if (applySpecialEffect)
				{
					poisonType = (sbyte)DomainManager.SpecialEffect.ModifyData(attackerId, skillId, 81, (int)poisonType, -1, -1, -1);
					level = (sbyte)((int)level + DomainManager.SpecialEffect.GetModifyValue(attackerId, skillId, 72, EDataModifyType.Add, (int)poisonType, -1, -1, EDataSumType.All));
					level = (sbyte)((int)level + DomainManager.SpecialEffect.GetModifyValue(defender.GetId(), skillId, 105, EDataModifyType.Add, (int)poisonType, -1, -1, EDataSumType.All));
					level = (sbyte)Math.Clamp((int)level, 0, 3);
					addValue *= CFormulaHelper.CalcConsummateChangeDamagePercent(attacker, defender);
					CValuePercentBonus percent = 0;
					percent += DomainManager.SpecialEffect.GetModifyValue(attackerId, skillId, 73, EDataModifyType.AddPercent, (int)poisonType, (int)equipKey.ItemType, equipKey.Id, EDataSumType.All);
					percent += DomainManager.SpecialEffect.GetModifyValue(defender.GetId(), skillId, 106, EDataModifyType.AddPercent, (int)poisonType, (int)equipKey.ItemType, equipKey.Id, EDataSumType.All);
					GameData.Domains.Character.Character attackerChar = attacker.GetCharacter();
					foreach (SolarTermItem solarTerm in attackerChar.GetInvokedSolarTerm())
					{
						bool flag3 = solarTerm.PoisonBuffType == poisonType;
						if (flag3)
						{
							percent += attackerChar.GetSolarTermBonus((int)GlobalConfig.Instance.SolarTermAddPoisonEffect);
						}
					}
					addValue *= percent;
					ValueTuple<int, int> attackerTotalPercent = DomainManager.SpecialEffect.GetTotalPercentModifyValue(attackerId, -1, 73, (int)poisonType, -1, -1);
					ValueTuple<int, int> defenderTotalPercent = DomainManager.SpecialEffect.GetTotalPercentModifyValue(defender.GetId(), -1, 106, (int)poisonType, -1, -1);
					addValue = addValue * (100 + attackerTotalPercent.Item1 + attackerTotalPercent.Item2 + defenderTotalPercent.Item1 + defenderTotalPercent.Item2) / 100;
					addValue = DomainManager.SpecialEffect.ModifyData(defender.GetId(), skillId, 106, addValue, (int)poisonType, -1, -1);
					bool canAddPoison = DomainManager.SpecialEffect.ModifyData(defender.GetId(), skillId, 159, true, (int)poisonType, -1, -1);
					bool flag4 = !canAddPoison;
					if (flag4)
					{
						return;
					}
					sbyte poisonedLevel = PoisonsAndLevels.CalcPoisonedLevel(*defender.GetPoison()[(int)poisonType]);
					poisonResist += attacker.CalcAccessoryReducePoisonResist(poisonType, poisonedLevel);
					poisonResist += DomainManager.SpecialEffect.GetModifyValue(attacker.GetId(), skillId, 233, EDataModifyType.Add, (int)poisonType, poisonResist, -1, EDataSumType.All);
					poisonResist += DomainManager.SpecialEffect.GetModifyValue(defender.GetId(), skillId, 232, EDataModifyType.Add, (int)poisonType, poisonResist, -1, EDataSumType.All);
					percent = 0;
					percent += DomainManager.SpecialEffect.GetModifyValue(attacker.GetId(), skillId, 233, EDataModifyType.AddPercent, (int)poisonType, poisonResist, -1, EDataSumType.All);
					percent += DomainManager.SpecialEffect.GetModifyValue(defender.GetId(), skillId, 232, EDataModifyType.AddPercent, (int)poisonType, poisonResist, -1, EDataSumType.All);
					poisonResist *= percent;
				}
				addValue = PoisonsAndLevels.CalcPoisonDelta(addValue, level, *(ref poisons.Items.FixedElementField + (IntPtr)poisonType * 4), poisonResist);
				bool flag5 = addValue <= 0 || *(ref poisons.Items.FixedElementField + (IntPtr)poisonType * 4) >= 25000 || poisonResist >= 1000;
				if (!flag5)
				{
					PoisonsAndLevels poisonToShow = *defender.GetNewPoisonsToShow();
					*(ref poisons.Items.FixedElementField + (IntPtr)poisonType * 4) = Math.Clamp(*(ref poisons.Items.FixedElementField + (IntPtr)poisonType * 4) + addValue, 0, 25000);
					*(ref poisonToShow.Levels.FixedElementField + poisonType) = Math.Max(*(ref poisonToShow.Levels.FixedElementField + poisonType), level);
					*(ref poisonToShow.Values.FixedElementField + (IntPtr)poisonType * 2) = (short)((int)(*(ref poisonToShow.Values.FixedElementField + (IntPtr)poisonType * 2)) + addValue);
					this.SetPoisons(context, defender, poisons, false);
					bool changeToOld = attackerId >= 0 && DomainManager.SpecialEffect.ModifyData(attackerId, skillId, 78, false, (int)poisonType, -1, -1);
					bool flag6 = changeToOld || forceChangeToOld;
					if (flag6)
					{
						this.ChangeToOldPoison(context, defender, poisonType, addValue);
					}
					Events.RaiseAddPoison(context, attackerId, defender.GetId(), poisonType, level, addValue, skillId, canBounce);
					int markCount = this.UpdatePoisonDefeatMark(context, defender, poisonType);
					if (isDirectPoison)
					{
						Events.RaiseAddDirectPoisonMark(context, attacker, defender, poisonType, skillId, markCount);
					}
					defender.SetNewPoisonsToShow(ref poisonToShow, context);
				}
			}
		}

		// Token: 0x060064C4 RID: 25796 RVA: 0x0039610C File Offset: 0x0039430C
		public unsafe int ReducePoison(DataContext context, CombatCharacter character, sbyte poisonType, int reduceValue, bool applySpecialEffect = true, bool canReduceOld = false)
		{
			PoisonInts poisons = *character.GetPoison();
			PoisonInts oldPoisons = *character.GetOldPoison();
			if (applySpecialEffect)
			{
				reduceValue = this.ApplyReducePoisonEffect(character.GetId(), poisonType, reduceValue, false);
			}
			bool flag = reduceValue > 0;
			if (flag)
			{
				int prevValue = *poisons[(int)poisonType];
				int minPoison = canReduceOld ? 0 : (*oldPoisons[(int)poisonType]);
				int currValue = *poisons[(int)poisonType] = Math.Max(*poisons[(int)poisonType] - reduceValue, minPoison);
				this.SetPoisons(context, character, poisons, true);
				reduceValue = prevValue - currValue;
			}
			return reduceValue;
		}

		// Token: 0x060064C5 RID: 25797 RVA: 0x003961B0 File Offset: 0x003943B0
		private void AddDirectPoison(DataContext context, CombatCharacter attacker, CombatCharacter defender, PoisonsAndLevels poisons, CValuePercent ratio, short skillId = -1, ItemKey weaponKey = default(ItemKey))
		{
			for (sbyte type = 0; type < 6; type += 1)
			{
				ValueTuple<short, sbyte> valueAndLevel = poisons.GetValueAndLevel(type);
				short value = valueAndLevel.Item1;
				sbyte level = valueAndLevel.Item2;
				bool flag = value > 0;
				if (flag)
				{
					this.AddPoison(context, attacker, defender, type, level, (int)value * ratio, skillId, true, true, weaponKey, true, false, false);
				}
			}
		}

		// Token: 0x060064C6 RID: 25798 RVA: 0x00396210 File Offset: 0x00394410
		private int ApplyReducePoisonEffect(int charId, sbyte poisonType, int reduceValue, bool getCost = false)
		{
			reduceValue = DomainManager.SpecialEffect.ModifyData(charId, -1, 161, reduceValue, (int)poisonType, getCost ? 1 : 0, -1);
			bool canReducePoison = DomainManager.SpecialEffect.ModifyData(charId, -1, 160, true, (int)poisonType, -1, -1);
			bool flag = !canReducePoison;
			if (flag)
			{
				reduceValue = 0;
			}
			return reduceValue;
		}

		// Token: 0x060064C7 RID: 25799 RVA: 0x00396264 File Offset: 0x00394464
		public void PoisonAffect(DataContext context, CombatCharacter character, sbyte poisonType)
		{
			int affectCount = 1 + DomainManager.SpecialEffect.GetModifyValue(character.GetId(), 163, EDataModifyType.Add, -1, -1, -1, EDataSumType.All);
			for (int i = 0; i < affectCount; i++)
			{
				this.PoisonAffectInternal(context, character, poisonType);
			}
		}

		// Token: 0x060064C8 RID: 25800 RVA: 0x003962AC File Offset: 0x003944AC
		private unsafe void PoisonAffectInternal(DataContext context, CombatCharacter character, sbyte poisonType)
		{
			bool flag = !DomainManager.SpecialEffect.ModifyData(character.GetId(), -1, 162, true, (int)poisonType, -1, -1);
			if (!flag)
			{
				int poisonValue = *(ref character.GetPoison().Items.FixedElementField + (IntPtr)poisonType * 4);
				sbyte currLevel = PoisonsAndLevels.CalcPoisonedLevel(poisonValue);
				bool flag2 = currLevel == 0;
				if (!flag2)
				{
					if (poisonType > 1)
					{
						if (poisonType - 2 <= 1)
						{
							character.WorsenRandomInjury(context, poisonType == 2, WorsenConstants.CalcPoisonPercent((int)currLevel));
						}
					}
					else
					{
						List<sbyte> bodyPartRandomPool = ObjectPool<List<sbyte>>.Instance.Get();
						Injuries injuries = character.GetInjuries();
						bool isInner = poisonType == 1;
						bodyPartRandomPool.Clear();
						for (sbyte type = 0; type < 7; type += 1)
						{
							bool flag3 = injuries.Get(type, isInner) < 6;
							if (flag3)
							{
								bodyPartRandomPool.Add(type);
							}
						}
						bool flag4 = bodyPartRandomPool.Count > 0 && currLevel > 0;
						if (flag4)
						{
							sbyte bodyPart = bodyPartRandomPool.GetRandom(context.Random);
							this.AddInjury(context, character, bodyPart, isInner, (sbyte)Math.Min((int)currLevel, (int)(6 - injuries.Get(bodyPart, isInner))), false, false);
							this.UpdateBodyDefeatMark(context, character, bodyPart);
						}
						ObjectPool<List<sbyte>>.Instance.Return(bodyPartRandomPool);
					}
					this.CalcMixPoisonEffects(context, character, poisonType);
					Events.RaisePoisonAffected(context, character.GetId(), poisonType);
					this.PoisonProduce(context, character, poisonType, 1);
					this.PoisonProduceWeaken(context, character, poisonType);
					this.ShowSpecialEffectTips(character.GetId(), 1466 + (int)poisonType, 0);
				}
			}
		}

		// Token: 0x060064C9 RID: 25801 RVA: 0x00396444 File Offset: 0x00394644
		public unsafe void PoisonProduce(DataContext context, CombatCharacter combatChar, sbyte poisonType, int multiplier = 1)
		{
			PoisonItem poisonData = Poison.Instance[poisonType];
			CValuePercent producePercent = (int)poisonData.ProducePercent;
			int poisonValue = *combatChar.GetPoison()[(int)poisonType];
			sbyte poisonLevel = PoisonsAndLevels.CalcPoisonedLevel(poisonValue);
			int produceValue = DomainManager.SpecialEffect.ModifyValue(combatChar.GetId(), 259, poisonValue * multiplier * producePercent, -1, -1, -1, 0, 0, 0, 0);
			this.AddPoison(context, null, combatChar, poisonData.ProduceType, poisonLevel, produceValue, -1, false, true, default(ItemKey), false, false, false);
		}

		// Token: 0x060064CA RID: 25802 RVA: 0x003964CC File Offset: 0x003946CC
		public unsafe void PoisonProduceWeaken(DataContext context, CombatCharacter combatChar, sbyte poisonType)
		{
			PoisonItem poisonData = Poison.Instance[poisonType];
			int poisonValue = *combatChar.GetPoison()[(int)poisonType];
			sbyte poisonLevel = PoisonsAndLevels.CalcPoisonedLevel(poisonValue);
			int oldPoisonValue = *combatChar.GetOldPoison()[(int)poisonType];
			CValuePercent weakenPercent = (int)poisonData.AffectCostPercent[Math.Clamp((int)(poisonLevel - 1), 0, poisonData.AffectCostPercent.Length)];
			int weakenValue = (poisonValue - oldPoisonValue) * weakenPercent;
			this.ReducePoison(context, combatChar, poisonType, weakenValue, false, false);
		}

		// Token: 0x060064CB RID: 25803 RVA: 0x00396544 File Offset: 0x00394744
		public unsafe void ChangeToOldPoison(DataContext context, CombatCharacter character, sbyte poisonType, int poisonValue)
		{
			PoisonInts oldPoison = *character.GetOldPoison();
			PoisonInts poison = *character.GetPoison();
			*(ref oldPoison.Items.FixedElementField + (IntPtr)poisonType * 4) = Math.Min(*(ref oldPoison.Items.FixedElementField + (IntPtr)poisonType * 4) + poisonValue, *(ref poison.Items.FixedElementField + (IntPtr)poisonType * 4));
			character.SetOldPoison(ref oldPoison, context);
		}

		// Token: 0x060064CC RID: 25804 RVA: 0x003965B4 File Offset: 0x003947B4
		public bool CheckSkillPoison(short skillId, sbyte poisonType)
		{
			return Config.CombatSkill.Instance[skillId].Poisons.GetValueAndLevel(poisonType).Item1 > 0;
		}

		// Token: 0x060064CD RID: 25805 RVA: 0x003965E4 File Offset: 0x003947E4
		public bool CheckEquipmentPoison(ItemKey itemKey, out PoisonsAndLevels attachedPoisons)
		{
			PoisonsAndLevels innatePoisons;
			bool anyPoison = this.CheckEquipmentPoison(itemKey, out attachedPoisons, out innatePoisons);
			bool flag = !attachedPoisons.IsNonZero();
			if (flag)
			{
				attachedPoisons = innatePoisons;
			}
			return anyPoison;
		}

		// Token: 0x060064CE RID: 25806 RVA: 0x00396618 File Offset: 0x00394818
		public unsafe bool CheckEquipmentPoison(ItemKey itemKey, out PoisonsAndLevels attachedPoisons, out PoisonsAndLevels innatePoisons)
		{
			ItemBase item = itemKey.IsValid() ? DomainManager.Item.GetBaseItem(itemKey) : null;
			bool flag = item == null || item.GetCurrDurability() <= 0;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				bool flag2 = ModificationStateHelper.IsActive(itemKey.ModificationState, 1);
				if (flag2)
				{
					attachedPoisons = DomainManager.Item.GetAttachedPoisons(itemKey);
				}
				GameData.Domains.Item.Weapon weapon = item as GameData.Domains.Item.Weapon;
				bool flag3 = weapon != null;
				if (flag3)
				{
					innatePoisons = *weapon.GetInnatePoisons();
				}
				result = (attachedPoisons.IsNonZero() || innatePoisons.IsNonZero());
			}
			return result;
		}

		// Token: 0x060064CF RID: 25807 RVA: 0x003966B4 File Offset: 0x003948B4
		public void ApplyEquipmentPoison(DataContext context, CombatCharacter poisoner, CombatCharacter victim, ItemKey itemKey, int valueMultiplier = 1)
		{
			CombatDomain.<>c__DisplayClass561_0 CS$<>8__locals1;
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.context = context;
			CS$<>8__locals1.poisoner = poisoner;
			CS$<>8__locals1.victim = victim;
			CS$<>8__locals1.valueMultiplier = valueMultiplier;
			CS$<>8__locals1.itemKey = itemKey;
			bool flag = CS$<>8__locals1.victim.GetId() == this._carrierAnimalCombatCharId;
			if (!flag)
			{
				PoisonsAndLevels attachedPoisons;
				PoisonsAndLevels innatePoisons;
				bool flag2 = !this.CheckEquipmentPoison(CS$<>8__locals1.itemKey, out attachedPoisons, out innatePoisons);
				if (!flag2)
				{
					List<sbyte> validTypes = ObjectPool<List<sbyte>>.Instance.Get();
					List<int> typeWeights = ObjectPool<List<int>>.Instance.Get();
					validTypes.Clear();
					typeWeights.Clear();
					for (sbyte type = 0; type < 6; type += 1)
					{
						int weight = (int)(attachedPoisons.GetValueAndLevel(type).Item1 + innatePoisons.GetValueAndLevel(type).Item1);
						bool flag3 = weight <= 0;
						if (!flag3)
						{
							validTypes.Add(type);
							typeWeights.Add(weight);
						}
					}
					CS$<>8__locals1.poisonType = validTypes[RandomUtils.GetRandomIndex(typeWeights, CS$<>8__locals1.context.Random)];
					ObjectPool<List<sbyte>>.Instance.Return(validTypes);
					ObjectPool<List<int>>.Instance.Return(typeWeights);
					this.<ApplyEquipmentPoison>g__AddPoisonByTuple|561_0(attachedPoisons.GetValueAndLevel(CS$<>8__locals1.poisonType), ref CS$<>8__locals1);
					this.<ApplyEquipmentPoison>g__AddPoisonByTuple|561_0(innatePoisons.GetValueAndLevel(CS$<>8__locals1.poisonType), ref CS$<>8__locals1);
				}
			}
		}

		// Token: 0x060064D0 RID: 25808 RVA: 0x00396818 File Offset: 0x00394A18
		private static void BindMixPoisonEffectImplements()
		{
			bool flag = CombatDomain.MixPoisonEffectImplements.Count > 0;
			if (!flag)
			{
				Type type = typeof(MixPoisonEffectImplements);
				Type attributeType = typeof(MixPoisonEffectAttribute);
				foreach (MethodInfo method in type.GetMethods())
				{
					object[] attributes = method.GetCustomAttributes(attributeType, false);
					bool flag2 = attributes.Length == 0;
					if (!flag2)
					{
						MixPoisonEffectAttribute attribute = (MixPoisonEffectAttribute)attributes[0];
						MixPoisonEffectDelegate func = method.CreateDelegate<MixPoisonEffectDelegate>();
						CombatDomain.MixPoisonEffectImplements.Add(attribute.TemplateId, func);
					}
				}
			}
		}

		// Token: 0x060064D1 RID: 25809 RVA: 0x003968B4 File Offset: 0x00394AB4
		private void CalcMixPoisonEffects(DataContext context, CombatCharacter combatChar, sbyte affectPoisonType)
		{
			byte[] poisonMarkList = combatChar.GetDefeatMarkCollection().PoisonMarkList;
			bool flag = poisonMarkList.CountAll((byte count) => count > 0) < 3;
			if (!flag)
			{
				List<sbyte> typeList = ObjectPool<List<sbyte>>.Instance.Get();
				typeList.Clear();
				for (sbyte type2 = 0; type2 < 6; type2 += 1)
				{
					bool flag2 = poisonMarkList[(int)type2] > 0;
					if (flag2)
					{
						typeList.Add(type2);
					}
				}
				Predicate<sbyte> <>9__2;
				Func<sbyte, int> <>9__1;
				foreach (MixPoisonEffectItem effect in ((IEnumerable<MixPoisonEffectItem>)MixPoisonEffect.Instance))
				{
					bool flag3;
					if (effect.AffectPoisonTypes.Exist(affectPoisonType))
					{
						IReadOnlyList<sbyte> hasPoisonTypes = effect.HasPoisonTypes;
						Predicate<sbyte> predicate;
						if ((predicate = <>9__2) == null)
						{
							predicate = (<>9__2 = ((sbyte type) => !typeList.Contains(type)));
						}
						flag3 = hasPoisonTypes.Exist(predicate);
					}
					else
					{
						flag3 = true;
					}
					bool flag4 = flag3;
					if (!flag4)
					{
						MixPoisonEffectDelegate implement;
						bool flag5 = !CombatDomain.MixPoisonEffectImplements.TryGetValue(effect.TemplateId, out implement);
						if (flag5)
						{
							short predefinedLogId = 8;
							DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(32, 1);
							defaultInterpolatedStringHandler.AppendLiteral("Not implement mixed poison type ");
							defaultInterpolatedStringHandler.AppendFormatted<sbyte>(effect.TemplateId);
							PredefinedLog.Show(predefinedLogId, defaultInterpolatedStringHandler.ToStringAndClear());
						}
						else
						{
							bool infinityAffect = DomainManager.SpecialEffect.ModifyData(combatChar.GetId(), -1, 272, false, -1, -1, -1);
							IEnumerable<sbyte> hasPoisonTypes2 = effect.HasPoisonTypes;
							Func<sbyte, int> selector;
							if ((selector = <>9__1) == null)
							{
								selector = (<>9__1 = ((sbyte x) => (int)poisonMarkList[(int)x]));
							}
							int totalMarkCount = hasPoisonTypes2.Sum(selector);
							int canAffectCount = CFormula.CalcMixPoisonAffectCount(totalMarkCount);
							int affectedCount = combatChar.GetMixPoisonAffectedCount().GetAffectedCount(effect.TemplateId);
							bool canAffect = infinityAffect || affectedCount < canAffectCount;
							bool flag6 = canAffect && implement(context, combatChar, poisonMarkList);
							if (flag6)
							{
								combatChar.SetMixPoisonAffectedCount(combatChar.GetMixPoisonAffectedCount().AddAffectedCount(effect.TemplateId), context);
							}
						}
					}
				}
				ObjectPool<List<sbyte>>.Instance.Return(typeList);
			}
		}

		// Token: 0x060064D2 RID: 25810 RVA: 0x00396B0C File Offset: 0x00394D0C
		[DomainMethod]
		public bool DoRawCreate(DataContext context, int effectId, sbyte equipmentSlot, short newTemplateId)
		{
			bool flag = !this._selfChar.IsAlly;
			return !flag && this._selfChar.DoRawCreate(context, effectId, equipmentSlot, newTemplateId);
		}

		// Token: 0x060064D3 RID: 25811 RVA: 0x00396B44 File Offset: 0x00394D44
		[DomainMethod]
		public bool IgnoreRawCreate(DataContext context, int effectId)
		{
			bool flag = !this._selfChar.IsAlly;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				this._selfChar.IgnoreRawCreate(context, effectId);
				result = true;
			}
			return result;
		}

		// Token: 0x060064D4 RID: 25812 RVA: 0x00396B7C File Offset: 0x00394D7C
		[DomainMethod]
		public bool IgnoreAllRawCreate(DataContext context)
		{
			bool flag = !this._selfChar.IsAlly;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				this._selfChar.IgnoreAllRawCreate(context);
				result = true;
			}
			return result;
		}

		// Token: 0x060064D5 RID: 25813 RVA: 0x00396BB4 File Offset: 0x00394DB4
		[DomainMethod]
		public List<sbyte> GetAllCanRawCreateEquipmentSlots(int effectId)
		{
			SpecialEffectItem config = SpecialEffect.Instance[effectId];
			return new List<sbyte>(this._selfChar.GetAllCanRawCreateEquipmentSlots(config.RawCreateType));
		}

		// Token: 0x060064D6 RID: 25814 RVA: 0x00396BE8 File Offset: 0x00394DE8
		[DomainMethod]
		public UnlockSimulateResult GetUnlockSimulateResult(int index, bool isAlly = true)
		{
			bool flag = !this.IsInCombat();
			UnlockSimulateResult result2;
			if (flag)
			{
				result2 = null;
			}
			else
			{
				CombatCharacter combatChar = isAlly ? this._selfChar : this._enemyChar;
				List<bool> canUnlockAttack = combatChar.GetCanUnlockAttack();
				bool flag2 = !canUnlockAttack.CheckIndex(index) || !canUnlockAttack[index];
				if (flag2)
				{
					result2 = null;
				}
				else
				{
					List<int> effects = ObjectPool<List<int>>.Instance.Get();
					effects.Clear();
					effects = DomainManager.SpecialEffect.ModifyData(combatChar.GetId(), -1, 312, effects, index, -1, -1);
					UnlockSimulateResult result = new UnlockSimulateResult(effects, new Func<int, bool>(combatChar.AllRawCreateSlotsBlocked));
					ObjectPool<List<int>>.Instance.Return(effects);
					result2 = result;
				}
			}
			return result2;
		}

		// Token: 0x060064D7 RID: 25815 RVA: 0x00396C9C File Offset: 0x00394E9C
		[DomainMethod]
		public List<CombatSkillDisplayData> GetProactiveSkillList(int charId)
		{
			bool flag = !this.IsCharInCombat(charId, true);
			List<CombatSkillDisplayData> result;
			if (flag)
			{
				result = null;
			}
			else
			{
				CombatCharacter combatChar = this._combatCharacterDict[charId];
				List<short> skillIdList = ObjectPool<List<short>>.Instance.Get();
				skillIdList.Clear();
				skillIdList.AddRange(combatChar.GetAttackSkillList());
				skillIdList.AddRange(combatChar.GetAgileSkillList());
				skillIdList.AddRange(combatChar.GetDefenceSkillList());
				skillIdList.RemoveAll((short id) => id < 0);
				List<CombatSkillDisplayData> dataList = DomainManager.CombatSkill.GetCombatSkillDisplayData(charId, skillIdList);
				ObjectPool<List<short>>.Instance.Return(skillIdList);
				result = dataList;
			}
			return result;
		}

		// Token: 0x060064D8 RID: 25816 RVA: 0x00396D50 File Offset: 0x00394F50
		[DomainMethod]
		public OuterAndInnerShorts GetPreviewAttackRange(short skillId, int weaponIndex = -1)
		{
			bool flag = !this.IsInCombat();
			OuterAndInnerShorts result;
			if (flag)
			{
				result = default(OuterAndInnerShorts);
			}
			else
			{
				result = this._selfChar.CalcAttackRangeImmediate(skillId, weaponIndex);
			}
			return result;
		}

		// Token: 0x060064D9 RID: 25817 RVA: 0x00396D88 File Offset: 0x00394F88
		[DomainMethod]
		public void StartPrepareSkill(DataContext context, short skillId, bool isAlly = true)
		{
			bool flag = !this.CanAcceptCommand();
			if (!flag)
			{
				CombatCharacter combatChar = isAlly ? this._selfChar : this._enemyChar;
				CombatSkillData skillData;
				bool flag2 = !this._skillDataDict.TryGetValue(new ValueTuple<int, short>(combatChar.GetId(), skillId), out skillData) || !skillData.GetCanUse();
				if (!flag2)
				{
					combatChar.SetNeedUseSkillId(context, skillId);
					bool needPrepare = Config.CombatSkill.Instance[combatChar.NeedUseSkillId].EquipType == 1 || combatChar.GetPreparingSkillId() < 0 || !combatChar.CanCastDuringPrepareSkills.Contains(skillId);
					bool flag3 = !needPrepare;
					if (flag3)
					{
						this.CastAgileOrDefenseWithoutPrepare(combatChar, combatChar.NeedUseSkillId);
						combatChar.SetNeedUseSkillId(context, -1);
					}
					bool flag4 = needPrepare;
					if (flag4)
					{
						combatChar.MoveData.ResetJumpState(context, true);
					}
					this.UpdateMaxSkillGrade(isAlly, skillId);
					this.UpdateAllCommandAvailability(context, combatChar);
				}
			}
		}

		// Token: 0x060064DA RID: 25818 RVA: 0x00396E78 File Offset: 0x00395078
		[DomainMethod]
		public bool ClearDefendInBlockAttackSkill(DataContext context, bool isAlly = true)
		{
			CombatCharacter combatChar = isAlly ? this._selfChar : this._enemyChar;
			bool flag = combatChar.NeedUseSkillId < 0 || combatChar.GetAffectingDefendSkillId() < 0;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				bool flag2 = DomainManager.SpecialEffect.ModifyData(combatChar.GetId(), combatChar.NeedUseSkillId, 223, false, -1, -1, -1);
				if (flag2)
				{
					result = false;
				}
				else
				{
					this.ClearAffectingDefenseSkillManual(context, isAlly);
					result = true;
				}
			}
			return result;
		}

		// Token: 0x060064DB RID: 25819 RVA: 0x00396EEC File Offset: 0x003950EC
		private void InitSkillData(DataContext context)
		{
			this.ClearSkillDataDict();
			foreach (CombatCharacter combatChar in this._combatCharacterDict.Values)
			{
				GameData.Domains.Character.Character character = combatChar.GetCharacter();
				int charId = character.GetId();
				foreach (short skillId in combatChar.GetCharacter().GetCombatSkillEquipment())
				{
					bool flag = combatChar.BossConfig != null && CombatSkillTemplateHelper.IsAttack(skillId);
					if (!flag)
					{
						bool combatSkillCanAffect = character.GetCombatSkillCanAffect(skillId);
						if (combatSkillCanAffect)
						{
							this.AddCombatSkillData(context, charId, skillId);
						}
					}
				}
				bool flag2 = combatChar.BossConfig != null;
				if (flag2)
				{
					foreach (short skillId2 in combatChar.BossConfig.PhaseAttackSkills[0])
					{
						this.AddCombatSkillData(context, charId, skillId2);
					}
				}
			}
		}

		// Token: 0x060064DC RID: 25820 RVA: 0x0039701C File Offset: 0x0039521C
		private void AddCombatSkillData(DataContext context, int charId, short skillId)
		{
			CombatSkillKey skillKey = new CombatSkillKey(charId, skillId);
			bool flag = this._skillDataDict.ContainsKey(skillKey);
			if (flag)
			{
				PredefinedLog.Show(8, "AddCombatSkillData already exist key " + skillKey.ToString());
			}
			else
			{
				CombatSkillData skillData = new CombatSkillData(skillKey);
				this.AddElement_SkillDataDict(skillKey, skillData);
				skillData.SetLeftCdFrame(0, context);
			}
		}

		// Token: 0x060064DD RID: 25821 RVA: 0x00397080 File Offset: 0x00395280
		public CombatSkillData GetCombatSkillData(int charId, short skillId)
		{
			return this._skillDataDict[new CombatSkillKey(charId, skillId)];
		}

		// Token: 0x060064DE RID: 25822 RVA: 0x003970A4 File Offset: 0x003952A4
		public bool TryGetCombatSkillData(int charId, short skillId, out CombatSkillData combatSkillData)
		{
			return this._skillDataDict.TryGetValue(new ValueTuple<int, short>(charId, skillId), out combatSkillData);
		}

		// Token: 0x060064DF RID: 25823 RVA: 0x003970D0 File Offset: 0x003952D0
		public bool IsCombatSkillSilenceInfinity(CombatSkillKey skillKey)
		{
			CombatSkillData combatSkillData;
			return this.TryGetCombatSkillData(skillKey.CharId, skillKey.SkillTemplateId, out combatSkillData) && combatSkillData.GetTotalCdFrame() < 0;
		}

		// Token: 0x060064E0 RID: 25824 RVA: 0x00397104 File Offset: 0x00395304
		public bool CombatSkillDataExist(CombatSkillKey skillKey)
		{
			return this._skillDataDict.ContainsKey(skillKey);
		}

		// Token: 0x060064E1 RID: 25825 RVA: 0x00397124 File Offset: 0x00395324
		public void UpdateSkillCanUse(DataContext context, CombatCharacter character)
		{
			List<short> skillIdList = ObjectPool<List<short>>.Instance.Get();
			skillIdList.Clear();
			skillIdList.AddRange(character.GetAttackSkillList());
			skillIdList.AddRange(character.GetAgileSkillList());
			skillIdList.AddRange(character.GetDefenceSkillList());
			skillIdList.AddRange(character.GetAssistSkillList());
			skillIdList.RemoveAll((short id) => id <= 0);
			for (int i = 0; i < skillIdList.Count; i++)
			{
				this.UpdateSkillCanUse(context, character, skillIdList[i]);
			}
			ObjectPool<List<short>>.Instance.Return(skillIdList);
		}

		// Token: 0x060064E2 RID: 25826 RVA: 0x003971D0 File Offset: 0x003953D0
		public void UpdateSkillCanUse(DataContext context, CombatCharacter character, short skillId)
		{
			CombatSkillKey skillKey = new CombatSkillKey(character.GetId(), skillId);
			CombatSkillData skillData = this._skillDataDict[skillKey];
			CombatSkillItem configData = Config.CombatSkill.Instance[skillId];
			bool flag = configData.EquipType != 4;
			bool canUse;
			if (flag)
			{
				canUse = this.CanCastSkill(character, skillId, false, false);
			}
			else
			{
				canUse = (skillData.GetLeftCdFrame() == 0);
			}
			bool flag2 = skillData.GetCanUse() != canUse;
			if (flag2)
			{
				skillData.SetCanUse(canUse, context);
			}
			else
			{
				bool flag3 = !canUse;
				if (flag3)
				{
					skillData.InvalidateSelfAndInfluencedCache(7, context);
				}
			}
			bool flag4 = !canUse && character.GetCombatReserveData().NeedUseSkillId == skillId;
			if (flag4)
			{
				character.SetCombatReserveData(CombatReserveData.Invalid, context);
			}
		}

		// Token: 0x060064E3 RID: 25827 RVA: 0x0039728C File Offset: 0x0039548C
		public void UpdateSkillCostBreathStanceCanUse(DataContext context, CombatCharacter character)
		{
			foreach (CombatSkillKey skillKey in this._skillDataDict.Keys)
			{
				bool flag = skillKey.CharId == character.GetId() && DomainManager.CombatSkill.GetElement_CombatSkills(skillKey).GetCostBreathAndStancePercent() > 0;
				if (flag)
				{
					this.UpdateSkillCanUse(context, character, skillKey.SkillTemplateId);
				}
			}
		}

		// Token: 0x060064E4 RID: 25828 RVA: 0x0039731C File Offset: 0x0039551C
		public void UpdateSkillCostTrickCanUse(DataContext context, CombatCharacter character)
		{
			foreach (CombatSkillKey skillKey in this._skillDataDict.Keys)
			{
				bool flag = skillKey.CharId == character.GetId() && Config.CombatSkill.Instance[skillKey.SkillTemplateId].TrickCost.Count > 0;
				if (flag)
				{
					this.UpdateSkillCanUse(context, character, skillKey.SkillTemplateId);
				}
			}
			CombatCharacter enemyChar = this.GetCombatCharacter(!character.IsAlly, false);
			foreach (CombatSkillKey skillKey2 in this._skillDataDict.Keys)
			{
				bool flag2 = skillKey2.CharId != enemyChar.GetId();
				if (!flag2)
				{
					bool flag3 = DomainManager.SpecialEffect.ModifyData(skillKey2.CharId, skillKey2.SkillTemplateId, 280, false, -1, -1, -1);
					if (flag3)
					{
						this.UpdateSkillCanUse(context, enemyChar, skillKey2.SkillTemplateId);
					}
				}
			}
		}

		// Token: 0x060064E5 RID: 25829 RVA: 0x0039745C File Offset: 0x0039565C
		private void UpdateSkillNeedMobilityCanUse(DataContext context, CombatCharacter character)
		{
			foreach (CombatSkillKey skillKey in this._skillDataDict.Keys)
			{
				GameData.Domains.CombatSkill.CombatSkill skill = DomainManager.CombatSkill.GetElement_CombatSkills(skillKey);
				CombatSkillItem configData = Config.CombatSkill.Instance[skillKey.SkillTemplateId];
				bool canUseMobilityAsBreath = DomainManager.SpecialEffect.ModifyData(character.GetId(), skillKey.SkillTemplateId, 229, false, -1, -1, -1);
				bool canUseMobilityAsStance = DomainManager.SpecialEffect.ModifyData(character.GetId(), skillKey.SkillTemplateId, 230, false, -1, -1, -1);
				bool flag = skillKey.CharId == character.GetId() && (configData.EquipType == 2 || skill.GetCostMobilityPercent() > 0 || canUseMobilityAsBreath || canUseMobilityAsStance);
				if (flag)
				{
					this.UpdateSkillCanUse(context, character, skillKey.SkillTemplateId);
				}
			}
		}

		// Token: 0x060064E6 RID: 25830 RVA: 0x00397558 File Offset: 0x00395758
		private void UpdateSkillNeedDistanceCanUse(DataContext context, CombatCharacter character)
		{
			foreach (CombatSkillKey skillKey in this._skillDataDict.Keys)
			{
				bool flag = skillKey.CharId == character.GetId() && Config.CombatSkill.Instance[skillKey.SkillTemplateId].EquipType == 1;
				if (flag)
				{
					this.UpdateSkillCanUse(context, character, skillKey.SkillTemplateId);
				}
			}
		}

		// Token: 0x060064E7 RID: 25831 RVA: 0x003975EC File Offset: 0x003957EC
		public void UpdateSkillNeedBodyPartCanUse(DataContext context, CombatCharacter character)
		{
			foreach (CombatSkillKey skillKey in this._skillDataDict.Keys)
			{
				bool flag = skillKey.CharId == character.GetId() && Config.CombatSkill.Instance[skillKey.SkillTemplateId].NeedBodyPartTypes.Count > 0;
				if (flag)
				{
					this.UpdateSkillCanUse(context, character, skillKey.SkillTemplateId);
				}
			}
		}

		// Token: 0x060064E8 RID: 25832 RVA: 0x00397684 File Offset: 0x00395884
		public OuterAndInnerInts GetSkillCostBreathStance(int charId, GameData.Domains.CombatSkill.CombatSkill skill)
		{
			int costBreathPercent = (int)skill.GetCostBreathPercent();
			int costStancePercent = (int)skill.GetCostStancePercent();
			int convertStatus = DomainManager.SpecialEffect.ModifyValue(charId, skill.GetId().SkillTemplateId, 302, 0, -1, -1, -1, 0, 0, 0, 0);
			if (!true)
			{
			}
			ValueTuple<int, int> valueTuple;
			if (convertStatus <= 0)
			{
				if (convertStatus >= 0)
				{
					valueTuple = new ValueTuple<int, int>(costBreathPercent, costStancePercent);
				}
				else
				{
					valueTuple = new ValueTuple<int, int>(0, costBreathPercent + costStancePercent);
				}
			}
			else
			{
				valueTuple = new ValueTuple<int, int>(costBreathPercent + costStancePercent, 0);
			}
			if (!true)
			{
			}
			ValueTuple<int, int> valueTuple2 = valueTuple;
			costBreathPercent = valueTuple2.Item1;
			costStancePercent = valueTuple2.Item2;
			costBreathPercent = DomainManager.SpecialEffect.ModifyData(charId, skill.GetId().SkillTemplateId, 227, costBreathPercent, -1, -1, -1);
			costStancePercent = DomainManager.SpecialEffect.ModifyData(charId, skill.GetId().SkillTemplateId, 228, costStancePercent, -1, -1, -1);
			return new OuterAndInnerInts(costStancePercent, costBreathPercent);
		}

		// Token: 0x060064E9 RID: 25833 RVA: 0x00397758 File Offset: 0x00395958
		public bool SkillCostEnough(CombatCharacter character, short skillId)
		{
			bool enableSkillFreeCast = this._enableSkillFreeCast;
			bool result;
			if (enableSkillFreeCast)
			{
				result = true;
			}
			else
			{
				bool hasCost = DomainManager.SpecialEffect.ModifyData(character.GetId(), skillId, 154, true, -1, -1, -1);
				bool flag = !hasCost;
				if (flag)
				{
					result = true;
				}
				else
				{
					foreach (ECombatSkillBanReasonType banReason in this.CalcSkillCostEnoughBanReasons(character, skillId))
					{
						bool flag2 = banReason != ECombatSkillBanReasonType.None;
						if (flag2)
						{
							return false;
						}
					}
					result = true;
				}
			}
			return result;
		}

		// Token: 0x060064EA RID: 25834 RVA: 0x003977F8 File Offset: 0x003959F8
		public IEnumerable<ECombatSkillBanReasonType> CalcSkillCostEnoughBanReasons(CombatCharacter character, short skillId)
		{
			CombatSkillItem configData = Config.CombatSkill.Instance[skillId];
			CombatSkillKey skillKey = new CombatSkillKey(character.GetId(), skillId);
			GameData.Domains.CombatSkill.CombatSkill skill = DomainManager.CombatSkill.GetElement_CombatSkills(skillKey);
			bool flag = !DomainManager.SpecialEffect.ModifyData(character.GetId(), skillId, 290, true, -1, -1, -1);
			if (flag)
			{
				yield return ECombatSkillBanReasonType.SpecialEffectBan;
			}
			bool flag2 = (!character.CanCastSkillCostBreath && skill.GetCostBreathPercent() > 0) || (!character.CanCastSkillCostStance && skill.GetCostStancePercent() > 0);
			if (flag2)
			{
				yield return ECombatSkillBanReasonType.SpecialEffectBan;
			}
			else
			{
				int mobilityPercent = CValuePercent.ParseInt(character.GetMobilityValue(), MoveSpecialConstants.MaxMobility);
				OuterAndInnerInts costBreathStance = this.GetSkillCostBreathStance(character.GetId(), skill);
				CValuePercent innerRatio = (int)skill.GetCurrInnerRatio();
				int breathExtra = DomainManager.SpecialEffect.GetModifyValue(character.GetId(), skillId, 173, EDataModifyType.Add, -1, -1, -1, EDataSumType.All);
				int breathCostPercent = Math.Max(costBreathStance.Inner - breathExtra, 0);
				bool breathCanUseMobility = DomainManager.SpecialEffect.ModifyData(character.GetId(), skillKey.SkillTemplateId, 229, false, -1, -1, -1);
				int breathUseMobility = 0;
				bool flag3 = breathCanUseMobility && breathCostPercent > 0;
				if (flag3)
				{
					breathUseMobility = Math.Min(breathCostPercent, mobilityPercent * 2 * innerRatio);
				}
				breathCostPercent -= breathUseMobility;
				int breathCost = 30000 * breathCostPercent / 100;
				bool breathEnough = character.GetBreathValue() >= breathCost;
				bool breathCanCastOnLack = DomainManager.SpecialEffect.ModifyData(character.GetId(), skillId, 225, false, breathCost, -1, -1);
				bool flag4 = !breathEnough && !breathCanCastOnLack;
				if (flag4)
				{
					yield return ECombatSkillBanReasonType.BreathNotEnough;
				}
				int stanceExtra = DomainManager.SpecialEffect.GetModifyValue(character.GetId(), skillId, 174, EDataModifyType.Add, -1, -1, -1, EDataSumType.All);
				int stanceCostPercent = Math.Max(costBreathStance.Outer - stanceExtra, 0);
				bool stanceCanUseMobility = DomainManager.SpecialEffect.ModifyData(character.GetId(), skillKey.SkillTemplateId, 230, false, -1, -1, -1);
				int stanceUseMobility = 0;
				bool flag5 = stanceCanUseMobility && stanceCostPercent > 0;
				if (flag5)
				{
					stanceUseMobility = Math.Min(stanceCostPercent, mobilityPercent * 2 - breathUseMobility);
				}
				stanceCostPercent -= stanceUseMobility;
				int stanceCost = 4000 * stanceCostPercent / 100;
				bool stanceEnough = character.GetStanceValue() >= stanceCost;
				bool stanceCanCastOnLack = DomainManager.SpecialEffect.ModifyData(character.GetId(), skillId, 226, false, stanceCost, -1, -1);
				bool flag6 = !stanceEnough && !stanceCanCastOnLack;
				if (flag6)
				{
					yield return ECombatSkillBanReasonType.StanceNotEnough;
				}
				costBreathStance = default(OuterAndInnerInts);
				innerRatio = default(CValuePercent);
			}
			bool flag7 = !this.HasNeedTrick(character, skill, false);
			if (flag7)
			{
				yield return ECombatSkillBanReasonType.TrickNotEnough;
			}
			bool flag8 = configData.WeaponDurableCost > 0 && character.GetUsingWeaponIndex() < 3 && DomainManager.Item.GetElement_Weapons(this.GetUsingWeaponKey(character).Id).GetCurrDurability() < (short)configData.WeaponDurableCost;
			if (flag8)
			{
				yield return ECombatSkillBanReasonType.WeaponDestroyed;
			}
			CValuePercent costMobilityPercent = (int)skill.GetCostMobilityPercent();
			bool flag9 = costMobilityPercent > 0;
			if (flag9)
			{
				int costMobility = MoveSpecialConstants.MaxMobility * costMobilityPercent;
				bool flag10 = character.GetMobilityValue() < costMobility;
				if (flag10)
				{
					yield return ECombatSkillBanReasonType.MobilityNotEnough;
				}
			}
			bool flag11 = character.GetWugCount() < (short)configData.WugCost;
			if (flag11)
			{
				yield return ECombatSkillBanReasonType.WugNotEnough;
			}
			bool flag12 = !this.HasNeedNeiliAllocation(character, skill);
			if (flag12)
			{
				yield return ECombatSkillBanReasonType.NeiliAllocationNotEnough;
			}
			yield break;
		}

		// Token: 0x060064EB RID: 25835 RVA: 0x00397818 File Offset: 0x00395A18
		public bool SkillInCastRange(CombatCharacter character, short skillId)
		{
			OuterAndInnerInts skillRange = this.GetSkillAttackRange(character, skillId);
			return (int)this._currentDistance >= skillRange.Outer && (int)this._currentDistance <= skillRange.Inner;
		}

		// Token: 0x060064EC RID: 25836 RVA: 0x00397858 File Offset: 0x00395A58
		public OuterAndInnerInts GetSkillAttackRange(CombatCharacter character, short skillId)
		{
			OuterAndInnerShorts attackRange = character.CalcAttackRangeImmediate(skillId, -1);
			return new OuterAndInnerInts((int)attackRange.Outer, (int)attackRange.Inner);
		}

		// Token: 0x060064ED RID: 25837 RVA: 0x00397884 File Offset: 0x00395A84
		public bool HasSkillNeedBodyPart(CombatCharacter character, short skillId, bool applyEffect = true)
		{
			CombatSkillItem skillConfigData = Config.CombatSkill.Instance[skillId];
			bool canCastWithBrokenBody = applyEffect && DomainManager.SpecialEffect.ModifyData(character.GetId(), skillId, 219, false, -1, -1, -1);
			byte[] acupointCount = character.GetAcupointCount();
			for (int i = 0; i < skillConfigData.NeedBodyPartTypes.Count; i++)
			{
				switch (skillConfigData.NeedBodyPartTypes[i])
				{
				case 0:
				{
					bool flag = (!canCastWithBrokenBody && this.CheckBodyPartInjury(character, 2, false)) || acupointCount[2] >= 3;
					if (flag)
					{
						return false;
					}
					break;
				}
				case 1:
				{
					bool flag2 = (!canCastWithBrokenBody && this.CheckBodyPartInjury(character, 0, false)) || acupointCount[0] >= 3;
					if (flag2)
					{
						return false;
					}
					break;
				}
				case 2:
				{
					bool flag3 = (!canCastWithBrokenBody && this.CheckBodyPartInjury(character, 1, false)) || acupointCount[1] >= 3;
					if (flag3)
					{
						return false;
					}
					break;
				}
				case 3:
				{
					bool flag4 = (!canCastWithBrokenBody && this.CheckBodyPartInjury(character, 3, false)) || acupointCount[3] >= 3 || (!canCastWithBrokenBody && this.CheckBodyPartInjury(character, 4, false)) || acupointCount[4] >= 3;
					if (flag4)
					{
						return false;
					}
					break;
				}
				case 4:
				{
					bool flag5 = ((!canCastWithBrokenBody && this.CheckBodyPartInjury(character, 3, false)) || acupointCount[3] >= 3) && ((!canCastWithBrokenBody && this.CheckBodyPartInjury(character, 4, false)) || acupointCount[4] >= 3);
					if (flag5)
					{
						return false;
					}
					break;
				}
				case 5:
				{
					bool flag6 = (!canCastWithBrokenBody && this.CheckBodyPartInjury(character, 5, false)) || acupointCount[5] >= 3 || (!canCastWithBrokenBody && this.CheckBodyPartInjury(character, 6, false)) || acupointCount[6] >= 3;
					if (flag6)
					{
						return false;
					}
					break;
				}
				case 6:
				{
					bool flag7 = ((!canCastWithBrokenBody && this.CheckBodyPartInjury(character, 5, false)) || acupointCount[5] >= 3) && ((!canCastWithBrokenBody && this.CheckBodyPartInjury(character, 6, false)) || acupointCount[6] >= 3);
					if (flag7)
					{
						return false;
					}
					break;
				}
				}
			}
			return true;
		}

		// Token: 0x060064EE RID: 25838 RVA: 0x00397AB4 File Offset: 0x00395CB4
		public bool SkillBodyPartHasHeavyInjury(CombatCharacter character, short skillId)
		{
			CombatSkillItem skillConfigData = Config.CombatSkill.Instance[skillId];
			for (int i = 0; i < skillConfigData.NeedBodyPartTypes.Count; i++)
			{
				switch (skillConfigData.NeedBodyPartTypes[i])
				{
				case 0:
				{
					bool flag = this.CheckBodyPartInjury(character, 2, true);
					if (flag)
					{
						return true;
					}
					break;
				}
				case 1:
				{
					bool flag2 = this.CheckBodyPartInjury(character, 0, true);
					if (flag2)
					{
						return true;
					}
					break;
				}
				case 2:
				{
					bool flag3 = this.CheckBodyPartInjury(character, 1, true);
					if (flag3)
					{
						return true;
					}
					break;
				}
				case 3:
				{
					bool flag4 = this.CheckBodyPartInjury(character, 3, true) || this.CheckBodyPartInjury(character, 4, true);
					if (flag4)
					{
						return true;
					}
					break;
				}
				case 4:
				{
					bool flag5 = this.CheckBodyPartInjury(character, 3, true) && this.CheckBodyPartInjury(character, 4, true);
					if (flag5)
					{
						return true;
					}
					break;
				}
				case 5:
				{
					bool flag6 = this.CheckBodyPartInjury(character, 5, true) || this.CheckBodyPartInjury(character, 6, true);
					if (flag6)
					{
						return true;
					}
					break;
				}
				case 6:
				{
					bool flag7 = this.CheckBodyPartInjury(character, 5, true) && this.CheckBodyPartInjury(character, 6, true);
					if (flag7)
					{
						return true;
					}
					break;
				}
				}
			}
			return false;
		}

		// Token: 0x060064EF RID: 25839 RVA: 0x00397C1C File Offset: 0x00395E1C
		public bool SkillCanUseInCurrCombat(int charId, CombatSkillItem configData)
		{
			List<sbyte> typeList = this.CombatConfig.CombatSkillType;
			return CombatSkillDomain.FiveElementMatch(charId, configData, this.CombatConfig.FiveElementsOfSkill) && (typeList == null || typeList.Count == 0 || typeList.Contains(DomainManager.CombatSkill.GetSkillType(charId, configData.TemplateId))) && (this.CombatConfig.Sect < 0 || this.CombatConfig.Sect == configData.SectId);
		}

		// Token: 0x060064F0 RID: 25840 RVA: 0x00397C9C File Offset: 0x00395E9C
		public bool SkillDirectionCanCast(CombatCharacter character, short skillId)
		{
			sbyte direction = DomainManager.CombatSkill.GetSkillDirection(character.GetId(), skillId);
			return (direction != 0 || character.CanCastDirectSkill) && (direction != 1 || character.CanCastReverseSkill);
		}

		// Token: 0x060064F1 RID: 25841 RVA: 0x00397CDC File Offset: 0x00395EDC
		public unsafe bool HasNeedNeiliAllocation(CombatCharacter character, GameData.Domains.CombatSkill.CombatSkill skill)
		{
			ValueTuple<sbyte, sbyte> costNeiliAllocation = skill.GetCostNeiliAllocation();
			NeiliAllocation neiliAllocation = character.GetNeiliAllocation();
			return costNeiliAllocation.Item1 < 0 || *(ref neiliAllocation.Items.FixedElementField + (IntPtr)costNeiliAllocation.Item1 * 2) >= (short)costNeiliAllocation.Item2;
		}

		// Token: 0x060064F2 RID: 25842 RVA: 0x00397D2C File Offset: 0x00395F2C
		public void DoCombatSkillCost(DataContext context, CombatCharacter character, short skillId)
		{
			bool enableSkillFreeCast = this._enableSkillFreeCast;
			if (!enableSkillFreeCast)
			{
				bool hasCost = DomainManager.SpecialEffect.ModifyData(character.GetId(), skillId, 154, true, -1, -1, -1);
				bool flag = !hasCost;
				if (!flag)
				{
					CombatSkillItem configData = Config.CombatSkill.Instance[skillId];
					CombatSkillKey skillKey = new CombatSkillKey(character.GetId(), skillId);
					GameData.Domains.CombatSkill.CombatSkill skill = DomainManager.CombatSkill.GetElement_CombatSkills(skillKey);
					int mobilityPercent = CValuePercent.ParseInt(character.GetMobilityValue(), MoveSpecialConstants.MaxMobility);
					OuterAndInnerInts costBreathStance = this.GetSkillCostBreathStance(character.GetId(), skill);
					CValuePercent innerRatio = (int)skill.GetCurrInnerRatio();
					int breathExtra = DomainManager.SpecialEffect.GetModifyValue(character.GetId(), skillId, 173, EDataModifyType.Add, -1, -1, -1, EDataSumType.All);
					int breathCostPercent = Math.Max(costBreathStance.Inner - breathExtra, 0);
					int breathExtraCost = costBreathStance.Inner - breathCostPercent;
					bool breathCanUseMobility = DomainManager.SpecialEffect.ModifyData(character.GetId(), skillKey.SkillTemplateId, 229, false, -1, -1, -1);
					int breathUseMobility = 0;
					bool flag2 = breathCanUseMobility && breathCostPercent > 0;
					if (flag2)
					{
						breathUseMobility = Math.Min(breathCostPercent, mobilityPercent * 2 * innerRatio);
					}
					breathCostPercent -= breathUseMobility;
					int breathCost = 30000 * breathCostPercent / 100;
					int stanceExtra = DomainManager.SpecialEffect.GetModifyValue(character.GetId(), skillId, 174, EDataModifyType.Add, -1, -1, -1, EDataSumType.All);
					int stanceCostPercent = Math.Max(costBreathStance.Outer - stanceExtra, 0);
					int stanceExtraCost = costBreathStance.Outer - stanceCostPercent;
					bool stanceCanUseMobility = DomainManager.SpecialEffect.ModifyData(character.GetId(), skillKey.SkillTemplateId, 230, false, -1, -1, -1);
					int stanceUseMobility = 0;
					bool flag3 = stanceCanUseMobility && stanceCostPercent > 0;
					if (flag3)
					{
						stanceUseMobility = Math.Min(stanceCostPercent, mobilityPercent * 2 - breathUseMobility);
					}
					stanceCostPercent -= stanceUseMobility;
					int stanceCost = 4000 * stanceCostPercent / 100;
					Events.RaiseCastSkillUseExtraBreathOrStance(context, character.GetId(), skillId, breathExtraCost, stanceExtraCost);
					bool flag4 = breathUseMobility > 0;
					if (flag4)
					{
						Events.RaiseCastSkillUseMobilityAsBreathOrStance(context, character.GetId(), skillId, true);
					}
					bool flag5 = stanceUseMobility > 0;
					if (flag5)
					{
						Events.RaiseCastSkillUseMobilityAsBreathOrStance(context, character.GetId(), skillId, false);
					}
					CValuePercent breathAndStanceCostMobilityPercent = (breathUseMobility + stanceUseMobility > 0) ? Math.Max((breathUseMobility + stanceUseMobility) / 2, 1) : 0;
					bool flag6 = breathAndStanceCostMobilityPercent > 0;
					if (flag6)
					{
						this.ChangeMobilityValue(context, character, -MoveSpecialConstants.MaxMobility * breathAndStanceCostMobilityPercent, false, null, true);
					}
					bool flag7 = breathCost > 0 || stanceCost > 0;
					if (flag7)
					{
						bool canCastOnLackBreath = DomainManager.SpecialEffect.ModifyData(character.GetId(), skillId, 225, false, breathCost, -1, -1);
						bool canCastOnLackStance = DomainManager.SpecialEffect.ModifyData(character.GetId(), skillId, 226, false, stanceCost, -1, -1);
						bool flag8 = canCastOnLackBreath || canCastOnLackStance;
						if (flag8)
						{
							int breathValue = character.GetBreathValue();
							int stanceValue = character.GetStanceValue();
							bool flag9 = breathValue < breathCost || stanceValue < stanceCost;
							if (flag9)
							{
								Events.RaiseCastSkillOnLackBreathStance(context, character, skillId, breathValue - breathCost, stanceValue - stanceCost, breathCost, stanceCost);
							}
						}
					}
					this.CostBreathAndStance(context, character, breathCost, stanceCost, skillId);
					List<NeedTrick> costTricks = ObjectPool<List<NeedTrick>>.Instance.Get();
					List<NeedTrick> costEnemyTricks = ObjectPool<List<NeedTrick>>.Instance.Get();
					DomainManager.CombatSkill.GetCombatSkillCostTrick(skill, costTricks, true);
					this.RemoveUsableTrickInsteadCostTrick(character, skillId, costTricks, costEnemyTricks);
					this.RemoveCostTrickInsteadUselessTrick(character, skillId, costTricks, true);
					this.RemoveCostTrickBySelfShaTrick(this.Context, character, skill.GetId().SkillTemplateId, costTricks, true);
					this.RemoveCostTrickByEnemyShaTrick(this.Context, character, skill.GetId().SkillTemplateId, costTricks, costEnemyTricks);
					this.RemoveCostTrickByJiTrick(this.Context, character, skill.GetId().SkillTemplateId, costTricks, true);
					this.RemoveJiTrickByUselessTrick(this.Context, character, skill.GetId().SkillTemplateId, costTricks, true);
					this.RemoveTrick(context, character, costTricks, true, true, -1);
					bool flag10 = costEnemyTricks.Count > 0;
					if (flag10)
					{
						this.RemoveTrick(context, this.GetCombatCharacter(!character.IsAlly, false), costEnemyTricks, false, false, -1);
					}
					Events.RaiseCastSkillTrickCosted(context, character, skillId, costTricks);
					ObjectPool<List<NeedTrick>>.Instance.Return(costTricks);
					ObjectPool<List<NeedTrick>>.Instance.Return(costEnemyTricks);
					int costMobilityPercent = (int)skill.GetCostMobilityPercent();
					bool flag11 = costMobilityPercent > 0;
					if (flag11)
					{
						int costMobility = MoveSpecialConstants.MaxMobility * costMobilityPercent / 100;
						this.ChangeMobilityValue(context, character, -costMobility, false, null, true);
					}
					bool flag12 = configData.WugCost > 0;
					if (flag12)
					{
						character.ChangeWugCount(context, (int)(-(int)configData.WugCost));
					}
					ValueTuple<sbyte, sbyte> costNeiliAllocation = skill.GetCostNeiliAllocation();
					bool flag13 = costNeiliAllocation.Item1 >= 0;
					if (flag13)
					{
						character.ChangeNeiliAllocation(context, (byte)costNeiliAllocation.Item1, (int)(-(int)costNeiliAllocation.Item2), true, true);
					}
					Events.RaiseCastSkillCosted(context, character, skillId);
					this.UpdateAllCommandAvailability(context, character);
				}
			}
		}

		// Token: 0x060064F3 RID: 25843 RVA: 0x003981D4 File Offset: 0x003963D4
		public int GetSkillPrepareSpeed(CombatCharacter character)
		{
			int charSpeed = DomainManager.SpecialEffect.ModifyValue(character.GetId(), character.GetPreparingSkillId(), 194, (int)character.GetSkillPrepareSpeed(), -1, -1, -1, 0, 0, 0, 0);
			return CFormula.CalcSkillPrepareSpeed(charSpeed);
		}

		// Token: 0x060064F4 RID: 25844 RVA: 0x00398218 File Offset: 0x00396418
		public void CalcSkillQiDisorderAndInjury(CombatCharacter character, CombatSkillItem skillConfig)
		{
			NeiliTypeItem neiliTypeConfig = NeiliType.Instance[character.GetNeiliType()];
			bool flag = CombatSkillDomain.FiveElementEquals(character.GetId(), skillConfig, neiliTypeConfig.InjuryOnUseType);
			if (flag)
			{
				this.AddGoneMadInjury(this.Context, character, skillConfig.TemplateId, 0);
				this.ShowSpecialEffectTips(character.GetId(), 1462, 0);
			}
			bool flag2 = this.Context.Random.CheckPercentProb(character.GetInjuredRate(skillConfig));
			if (flag2)
			{
				this.AddGoneMadInjury(this.Context, character, skillConfig.TemplateId, 0);
				this.ShowSpecialEffectTips(character.GetId(), 1487, 0);
			}
		}

		// Token: 0x060064F5 RID: 25845 RVA: 0x003982BC File Offset: 0x003964BC
		public void ApplyAgileOrDefenseSkill(CombatCharacter character, CombatSkillItem skillConfig)
		{
			GameData.Domains.CombatSkill.CombatSkill skill = DomainManager.CombatSkill.GetElement_CombatSkills(new CombatSkillKey(character.GetId(), skillConfig.TemplateId));
			bool flag = skillConfig.EquipType == 2;
			if (flag)
			{
				character.SetAffectingMoveSkillId(skillConfig.TemplateId, this.Context);
				character.MoveData.ResetJumpState(this.Context, true);
				character.NeedAddEffectAgileSkillId = skillConfig.TemplateId;
				this.UpdateTeammateCommandUsable(this.Context, character, ETeammateCommandImplement.ClearAgileAndDefense);
			}
			else
			{
				bool flag2 = skillConfig.EquipType == 3;
				if (flag2)
				{
					short keepFrame = CombatSkillDomain.CalcContinuousFrames(skill);
					character.SetAffectingDefendSkillId(skillConfig.TemplateId, this.Context);
					character.DefendSkillLeftFrame = (character.DefendSkillTotalFrame = keepFrame);
					DomainManager.SpecialEffect.Add(this.Context, character.GetId(), skillConfig.TemplateId, 0, -1);
					this.UpdateTeammateCommandUsable(this.Context, character, ETeammateCommandImplement.ClearAgileAndDefense);
				}
			}
			bool flag3 = character.GetCharacter().IsCombatSkillEquipped(skillConfig.TemplateId);
			if (flag3)
			{
				CombatSkillKey key = new CombatSkillKey(character.GetId(), skillConfig.TemplateId);
				this._skillCastTimes[key] = this._skillCastTimes.GetOrDefault(key) + 1;
			}
		}

		// Token: 0x060064F6 RID: 25846 RVA: 0x003983EC File Offset: 0x003965EC
		public void CastAgileOrDefenseWithoutPrepare(CombatCharacter character, short skillId)
		{
			Events.RaiseCastAgileOrDefenseWithoutPrepareBegin(this.Context, character.GetId(), skillId);
			CombatSkillItem skillConfig = Config.CombatSkill.Instance[skillId];
			this.CalcSkillQiDisorderAndInjury(character, skillConfig);
			this.ApplyAgileOrDefenseSkill(character, skillConfig);
			this.AddToCheckFallenSet(character.GetId());
			Events.RaiseCastAgileOrDefenseWithoutPrepareEnd(this.Context, character.GetId(), skillId);
		}

		// Token: 0x060064F7 RID: 25847 RVA: 0x0039844C File Offset: 0x0039664C
		public unsafe void CalcAttackSkillDataCompare(CombatContext context)
		{
			CombatDomain.<>c__DisplayClass610_0 CS$<>8__locals1;
			CS$<>8__locals1.attacker = context.Attacker;
			CombatCharacter defender = context.Defender;
			short skillId = context.SkillTemplateId;
			GameData.Domains.CombatSkill.CombatSkill skill = DomainManager.CombatSkill.GetElement_CombatSkills(new CombatSkillKey(CS$<>8__locals1.attacker.GetId(), skillId));
			HitOrAvoidInts hitValue = skill.GetHitValue();
			bool flag = !CombatSkillTemplateHelper.IsMindHitSkill(skillId);
			if (flag)
			{
				HitOrAvoidInts hitDistribution = skill.GetHitDistribution();
				CS$<>8__locals1.attacker.SkillHitType[0] = 2;
				CS$<>8__locals1.attacker.SkillHitType[1] = 1;
				CS$<>8__locals1.attacker.SkillHitType[2] = 0;
				CS$<>8__locals1.attacker.SkillHitValue[0] = ((*(ref hitDistribution.Items.FixedElementField + (IntPtr)2 * 4) > 0) ? CS$<>8__locals1.attacker.GetHitValue(2, CS$<>8__locals1.attacker.SkillAttackBodyPart, *(ref hitValue.Items.FixedElementField + (IntPtr)2 * 4), skillId) : -1);
				CS$<>8__locals1.attacker.SkillAvoidValue[0] = ((*(ref hitDistribution.Items.FixedElementField + (IntPtr)2 * 4) > 0) ? defender.GetAvoidValue(2, CS$<>8__locals1.attacker.SkillAttackBodyPart, skillId, false) : -1);
				CS$<>8__locals1.attacker.SkillHitValue[1] = ((*(ref hitDistribution.Items.FixedElementField + 4) > 0) ? CS$<>8__locals1.attacker.GetHitValue(1, CS$<>8__locals1.attacker.SkillAttackBodyPart, *(ref hitValue.Items.FixedElementField + 4), skillId) : -1);
				CS$<>8__locals1.attacker.SkillAvoidValue[1] = ((*(ref hitDistribution.Items.FixedElementField + 4) > 0) ? defender.GetAvoidValue(1, CS$<>8__locals1.attacker.SkillAttackBodyPart, skillId, false) : -1);
				CS$<>8__locals1.attacker.SkillHitValue[2] = ((hitDistribution.Items.FixedElementField > 0) ? CS$<>8__locals1.attacker.GetHitValue(0, CS$<>8__locals1.attacker.SkillAttackBodyPart, hitValue.Items.FixedElementField, skillId) : -1);
				CS$<>8__locals1.attacker.SkillAvoidValue[2] = ((hitDistribution.Items.FixedElementField > 0) ? defender.GetAvoidValue(0, CS$<>8__locals1.attacker.SkillAttackBodyPart, skillId, false) : -1);
				CS$<>8__locals1.attacker.SkillFinalAttackHitIndex = 0;
				bool maxCanHit = CombatDomain.<CalcAttackSkillDataCompare>g__CanHit|610_0(0, ref CS$<>8__locals1);
				int maxDistribution = *(ref hitDistribution.Items.FixedElementField + (IntPtr)2 * 4);
				int maxHitOdds = CombatDomain.<CalcAttackSkillDataCompare>g__CalcHitOdds|610_1(0, ref CS$<>8__locals1);
				for (int i = 1; i < 3; i++)
				{
					bool canHit = CombatDomain.<CalcAttackSkillDataCompare>g__CanHit|610_0(i, ref CS$<>8__locals1);
					int distribution = *(ref hitDistribution.Items.FixedElementField + (IntPtr)(2 - i) * 4);
					int hitOdds = CombatDomain.<CalcAttackSkillDataCompare>g__CalcHitOdds|610_1(i, ref CS$<>8__locals1);
					bool flag2 = (!maxCanHit || canHit) && (distribution > maxDistribution || (distribution == maxDistribution && hitOdds > maxHitOdds));
					if (flag2)
					{
						CS$<>8__locals1.attacker.SkillFinalAttackHitIndex = i;
						maxCanHit = canHit;
						maxDistribution = distribution;
						maxHitOdds = hitOdds;
					}
				}
			}
			else
			{
				CS$<>8__locals1.attacker.SkillHitType[0] = 3;
				CS$<>8__locals1.attacker.SkillHitType[1] = (CS$<>8__locals1.attacker.SkillHitType[2] = -1);
				CS$<>8__locals1.attacker.SkillHitValue[0] = CS$<>8__locals1.attacker.GetHitValue(3, CS$<>8__locals1.attacker.SkillAttackBodyPart, *(ref hitValue.Items.FixedElementField + (IntPtr)3 * 4), skillId);
				CS$<>8__locals1.attacker.SkillAvoidValue[0] = defender.GetAvoidValue(3, CS$<>8__locals1.attacker.SkillAttackBodyPart, skillId, false);
				CS$<>8__locals1.attacker.SkillFinalAttackHitIndex = 0;
			}
			CS$<>8__locals1.attacker.SetAttackSkillAttackIndex(0, context);
			CS$<>8__locals1.attacker.SetPerformingSkillId(skillId, context);
			this.UpdateDamageCompareData(context);
		}

		// Token: 0x060064F8 RID: 25848 RVA: 0x003987F0 File Offset: 0x003969F0
		public unsafe void CalcSkillAttack(CombatContext context, int attackIndex)
		{
			CombatCharacter character = context.Attacker;
			CombatCharacter enemyChar = context.Defender;
			short skillId = character.GetPerformingSkillId();
			GameData.Domains.Item.Weapon weapon = context.Weapon;
			GameData.Domains.CombatSkill.CombatSkill skill = DomainManager.CombatSkill.GetElement_CombatSkills(new CombatSkillKey(character.GetId(), skillId));
			HitOrAvoidInts hitDistribution = skill.GetHitDistribution();
			bool isMindHit = CombatSkillTemplateHelper.IsMindHitSkill(skillId);
			int skillPower = (!isMindHit) ? ((attackIndex < 3) ? (*(ref hitDistribution.Items.FixedElementField + (IntPtr)(2 - attackIndex) * 4)) : -1) : ((attackIndex == 0) ? 100 : 0);
			int compareDataIndex = (attackIndex < 3) ? attackIndex : character.SkillFinalAttackHitIndex;
			sbyte hitType = this._damageCompareData.HitType[compareDataIndex];
			CombatProperty property = this._damageCompareData.GetProperty(compareDataIndex);
			bool critical = context.CheckCritical(hitType);
			context = context.Property(property).Critical(critical);
			int hitOdds = property.HitOdds;
			bool flag = attackIndex < 3;
			if (flag)
			{
				hitOdds = this.ApplyHitOddsSpecialEffect(character, enemyChar, hitOdds, character.SkillHitType[attackIndex], skillId);
			}
			bool inevitableHit = DomainManager.SpecialEffect.ModifyData(character.GetId(), skillId, 251, false, -1, -1, -1);
			bool inevitableAvoid = DomainManager.SpecialEffect.ModifyData(enemyChar.GetId(), skillId, 291, false, critical ? 1 : 0, (int)context.BodyPart, character.GetId());
			bool flag2 = !inevitableAvoid;
			bool flag3 = flag2;
			if (flag3)
			{
				bool flag6;
				if (attackIndex < 3)
				{
					bool isValid = property.IsValid;
					bool flag4 = isValid;
					if (flag4)
					{
						bool flag5 = hitOdds < 0 || hitOdds >= 100;
						flag4 = (flag5 || character.SkillForceHit || inevitableHit);
					}
					flag6 = flag4;
				}
				else
				{
					flag6 = (character.GetAttackSkillPower() > 0);
				}
				flag3 = flag6;
			}
			bool hit = flag3;
			this.SetSkillAttackedIndexAndHit(new IntPair(attackIndex, hit ? 1 : 0), context);
			bool isValid2 = property.IsValid;
			if (isValid2)
			{
				Events.RaiseAttackSkillAttackBegin(context, character, enemyChar, skillId, attackIndex, hit);
			}
			bool flag7 = hit;
			if (flag7)
			{
				bool isLeg = DomainManager.CombatSkill.GetSkillType(character.GetId(), skillId) == 5;
				WeaponItem weaponConfig = Config.Weapon.Instance[weapon.GetTemplateId()];
				bool flag8 = isLeg && character.LegSkillUseShoes();
				if (flag8)
				{
					ItemKey shoesKey = character.Armors[5];
					GameData.Domains.Item.Armor shoes = shoesKey.IsValid() ? DomainManager.Item.GetElement_Armors(shoesKey.Id) : null;
					bool flag9 = shoes != null && shoes.GetCurrDurability() > 0;
					if (flag9)
					{
						weaponConfig = Config.Weapon.Instance[Config.Armor.Instance[shoesKey.TemplateId].RelatedWeapon];
					}
					else
					{
						weaponConfig = Config.Weapon.Instance[0];
					}
				}
				this.PlayHitSound(context, enemyChar, weaponConfig);
				bool flag10 = attackIndex < 3;
				if (flag10)
				{
					bool flag11 = skillPower > 0;
					if (flag11)
					{
						character.SetAttackSkillPower((byte)((int)character.GetAttackSkillPower() + skillPower), context);
						bool flag12 = this.CanPlayHitAnimation(enemyChar);
						if (flag12)
						{
							enemyChar.SetAnimationToPlayOnce(this.GetHittedAni(enemyChar, (skillPower <= 30) ? 0 : ((skillPower <= 60) ? 1 : 2)), context);
						}
						bool flag13 = !isMindHit;
						if (flag13)
						{
							int power = skillPower / 2;
							bool flag14 = power > 0;
							if (flag14)
							{
								int num;
								this.CalcSkillDamage(context, hitType, power, out num, out critical, power);
							}
						}
						else
						{
							int statePower = hitOdds / 5;
							bool flag15 = statePower > 0;
							if (flag15)
							{
								this.AddCombatState(context, enemyChar, 2, 116, statePower);
							}
						}
					}
				}
				else
				{
					int finalDamage;
					OuterAndInnerInts markCounts = this.CalcSkillHit(context, hitType, out finalDamage, out critical);
					bool flag16 = this.CanPlayHitAnimation(enemyChar);
					if (flag16)
					{
						enemyChar.SetAnimationToPlayOnce(this.GetHittedAni(enemyChar, Math.Clamp(markCounts.Outer + markCounts.Inner - 1, 0, 2)), context);
					}
					bool flag17 = !enemyChar.GetNewPoisonsToShow().IsNonZero() && finalDamage <= 0;
					if (flag17)
					{
						enemyChar.SetParticleToPlay("Particle_D_qidun", context);
					}
					bool flag18 = enemyChar.GetPreparingOtherAction() == 2 && this._currentDistance <= (short)this.InterruptFleeNeedDistance;
					if (flag18)
					{
						this.InterruptOtherAction(context, enemyChar);
					}
				}
			}
			else
			{
				bool flag19 = attackIndex < 3 && property.IsValid;
				if (flag19)
				{
					bool flag20 = this.CanPlayHitAnimation(enemyChar);
					if (flag20)
					{
						enemyChar.SetAnimationToPlayOnce(this.GetAvoidAni(enemyChar, hitType), context);
					}
					enemyChar.SetParticleToPlay((enemyChar.IsAlly ? this._selfAvoidParticle : this._enemyAvoidParticle)[(int)hitType], context);
					string[] avoidSounds = this._avoidSound[(int)hitType];
					enemyChar.SetHitSoundToPlay(avoidSounds[context.Random.Next(avoidSounds.Length)], context);
				}
				else
				{
					bool flag21 = attackIndex == 3;
					if (flag21)
					{
						bool flag22 = this.CanPlayHitAnimation(enemyChar);
						if (flag22)
						{
							enemyChar.SetAnimationToPlayOnce((enemyChar.AnimalConfig == null) ? "H_008" : CombatDomain.AvoidAni[2], context);
						}
					}
				}
			}
			bool flag23 = hit;
			if (flag23)
			{
				Events.RaiseAttackSkillAttackHit(context, character, enemyChar, skillId, attackIndex, critical);
			}
			bool flag24 = attackIndex == 3 && (!this.IsInCombat() || this._selectedMercyOption >= 0);
			if (flag24)
			{
				character.SetPerformingSkillId(-1, context);
				character.SetAttackSkillPower(0, context);
				this.ClearDamageCompareData(context);
				this.SetSkillAttackedIndexAndHit(new IntPair(-1, 0), context);
			}
			bool flag25 = attackIndex == 3 && character.GetCharacter().IsCombatSkillEquipped(skillId);
			if (flag25)
			{
				CombatSkillKey key = new CombatSkillKey(character.GetId(), skillId);
				this._skillCastTimes[key] = this._skillCastTimes.GetOrDefault(key) + 1;
			}
			bool flag26 = attackIndex == 3 && character.GetAttackSkillPower() >= 100 && character.GetId() == DomainManager.Taiwu.GetTaiwuCharId() && character.GetCharacter().GetConsummateLevel() <= enemyChar.GetCharacter().GetConsummateLevel();
			if (flag26)
			{
				DomainManager.Taiwu.AddFullPowerCastTimes(context, skillId);
			}
			bool isValid3 = property.IsValid;
			if (isValid3)
			{
				Events.RaiseAttackSkillAttackEnd(context, hitType, hit, attackIndex);
			}
		}

		// Token: 0x060064F9 RID: 25849 RVA: 0x00398E10 File Offset: 0x00397010
		private OuterAndInnerInts CalcSkillDamage(CombatContext context, sbyte hitType, int skillPower, out int finalDamage, out bool critical, int bouncePercent = 100)
		{
			CombatCharacter character = context.Attacker;
			CombatCharacter enemyChar = context.Defender;
			short skillId = context.SkillTemplateId;
			bool isMindHit = CombatSkillTemplateHelper.IsMindHitSkill(skillId);
			GameData.Domains.CombatSkill.CombatSkill skill = context.Skill;
			OuterAndInnerInts markCounts = this.CalcAndAddInjury(context, hitType, out finalDamage, out critical, skillPower, 100, 100);
			CValuePercent poisonRatio = (int)skill.GetPower() * skillPower;
			PoisonsAndLevels poisons = skill.GetPoisons();
			bool flag = poisons.IsNonZero();
			if (flag)
			{
				this.AddDirectPoison(context, character, enemyChar, poisons, poisonRatio, skillId, context.WeaponKey);
			}
			bool flag2 = isMindHit;
			OuterAndInnerInts result;
			if (flag2)
			{
				result = markCounts;
			}
			else
			{
				context.ApplyWeaponAndArmorPoison(3 * skillPower / 100);
				this.AddBounceDamage(context, hitType, skillId, bouncePercent);
				result = markCounts;
			}
			return result;
		}

		// Token: 0x060064FA RID: 25850 RVA: 0x00398ED8 File Offset: 0x003970D8
		private OuterAndInnerInts CalcSkillHit(CombatContext context, sbyte hitType, out int finalDamage, out bool critical)
		{
			CombatCharacter character = context.Attacker;
			CombatCharacter enemyChar = context.Defender;
			sbyte bodyPart = context.BodyPart;
			short skillId = context.SkillTemplateId;
			byte skillPower = character.GetAttackSkillPower();
			CombatSkillItem skillConfig = Config.CombatSkill.Instance[skillId];
			GameData.Domains.CombatSkill.CombatSkill skill = DomainManager.CombatSkill.GetElement_CombatSkills(new CombatSkillKey(character.GetId(), skillId));
			this.CalculateWeaponArmorBreak(context, skillConfig.EquipmentBreakOdds);
			sbyte level = skillConfig.GridCost - 1;
			bool flag = skillConfig.HasAtkAcupointEffect && (int)skillPower >= skill.GetSumMax2HitDistribution();
			if (flag)
			{
				this.AddAcupoint(context, enemyChar, level, skill.GetId(), bodyPart, 1, true);
			}
			bool flag2 = skillConfig.HasAtkFlawEffect && (int)skillPower >= skill.GetSumMax2HitDistribution();
			if (flag2)
			{
				this.AddFlaw(context, enemyChar, level, skill.GetId(), bodyPart, 1, true);
			}
			OuterAndInnerInts markCounts = this.CalcSkillDamage(context, hitType, (int)skillPower, out finalDamage, out critical, 100);
			bool needReduceWeaponDurability = character.NeedReduceWeaponDurability;
			if (needReduceWeaponDurability)
			{
				bool flag3 = DomainManager.CombatSkill.GetSkillType(character.GetId(), skillId) != 5 || !character.LegSkillUseShoes();
				if (flag3)
				{
					this.ReduceDurabilityByHit(context, character, context.WeaponKey);
				}
				else
				{
					ItemKey shoesKey = character.Armors[5];
					bool flag4 = shoesKey.IsValid();
					if (flag4)
					{
						this.ReduceDurabilityByHit(context, character, shoesKey);
					}
				}
				character.NeedReduceWeaponDurability = false;
			}
			bool needReduceArmorDurability = enemyChar.NeedReduceArmorDurability;
			if (needReduceArmorDurability)
			{
				this.ReduceDurabilityByHit(context, enemyChar, enemyChar.Armors[(int)bodyPart]);
				enemyChar.NeedReduceArmorDurability = false;
			}
			return markCounts;
		}

		// Token: 0x060064FB RID: 25851 RVA: 0x0039908C File Offset: 0x0039728C
		public void DoSkillHit(CombatCharacter attacker, CombatCharacter defender, short skillId, sbyte bodyPart, sbyte hitType)
		{
			CombatContext context = CombatContext.Create(attacker, defender, bodyPart, skillId, -1, null);
			int num;
			bool flag;
			this.CalcSkillHit(context, hitType, out num, out flag);
		}

		// Token: 0x060064FC RID: 25852 RVA: 0x003990C0 File Offset: 0x003972C0
		public void UpdateSkillCd(DataContext context, CombatCharacter character)
		{
			foreach (CombatSkillData skillData in this._skillDataDict.Values)
			{
				bool flag = skillData.GetId().CharId == character.GetId() && skillData.GetLeftCdFrame() > 0;
				if (flag)
				{
					skillData.SetLeftCdFrame(skillData.GetLeftCdFrame() - 1, context);
					bool flag2 = skillData.GetLeftCdFrame() == 0;
					if (flag2)
					{
						skillData.RaiseSkillSilenceEnd(context);
						this.UpdateSkillCanUse(context, character, skillData.GetId().SkillTemplateId);
					}
				}
			}
		}

		// Token: 0x060064FD RID: 25853 RVA: 0x00399178 File Offset: 0x00397378
		public void AddGoneMadInjury(DataContext context, CombatCharacter character, short skillId, int factor = 0)
		{
			CombatDomain.<>c__DisplayClass616_0 CS$<>8__locals1;
			CS$<>8__locals1.character = character;
			CS$<>8__locals1.skillId = skillId;
			CS$<>8__locals1.factor = factor;
			CombatSkillItem configData = Config.CombatSkill.Instance[CS$<>8__locals1.skillId];
			CS$<>8__locals1.extraTotalPercent = CS$<>8__locals1.character.GetGoneMadInjuryTotalPercent(configData);
			sbyte injuryCount = configData.GoneMadInjuryValue;
			CS$<>8__locals1.inner = configData.GoneMadInnerInjury;
			DamageStepCollection steps = CS$<>8__locals1.character.GetDamageStepCollection();
			CS$<>8__locals1.addingDisorderOfQi = false;
			CS$<>8__locals1.part = CS$<>8__locals1.character.RandomInjuryBodyPart(context.Random, CS$<>8__locals1.inner, configData.GoneMadInjuredPart);
			int fatalDamage = 0;
			bool flag = CS$<>8__locals1.part < 0;
			if (flag)
			{
				fatalDamage = CombatDomain.<AddGoneMadInjury>g__ModifyValue|616_0((int)injuryCount * steps.FatalDamageStep, ref CS$<>8__locals1);
			}
			else
			{
				int[] injuryValues = CS$<>8__locals1.inner ? CS$<>8__locals1.character.GetInnerDamageValue() : CS$<>8__locals1.character.GetOuterDamageValue();
				int injuryStep = (CS$<>8__locals1.inner ? steps.InnerDamageSteps : steps.OuterDamageSteps)[(int)CS$<>8__locals1.part];
				int remainMark = (int)(6 - CS$<>8__locals1.character.GetInjuries().Get(CS$<>8__locals1.part, CS$<>8__locals1.inner));
				int totalDamage = CombatDomain.<AddGoneMadInjury>g__ModifyValue|616_0((int)injuryCount * injuryStep, ref CS$<>8__locals1) + injuryValues[(int)CS$<>8__locals1.part];
				ValueTuple<int, int> valueTuple = CombatDomain.CalcInjuryMarkCount(totalDamage, injuryStep, remainMark);
				int markCount = valueTuple.Item1;
				int leftDamage = valueTuple.Item2;
				bool flag2 = markCount > 0;
				if (flag2)
				{
					this.AddInjury(context, CS$<>8__locals1.character, CS$<>8__locals1.part, CS$<>8__locals1.inner, (sbyte)markCount, true, false);
				}
				bool flag3 = markCount == remainMark;
				if (flag3)
				{
					fatalDamage = leftDamage * steps.FatalDamageStep / injuryStep;
				}
				else
				{
					injuryValues[(int)CS$<>8__locals1.part] = leftDamage;
					bool inner = CS$<>8__locals1.inner;
					if (inner)
					{
						CS$<>8__locals1.character.SetInnerDamageValue(injuryValues, context);
					}
					else
					{
						CS$<>8__locals1.character.SetOuterDamageValue(injuryValues, context);
					}
				}
			}
			bool flag4 = fatalDamage > 0;
			if (flag4)
			{
				this.AddFatalDamageValue(context, CS$<>8__locals1.character, fatalDamage, configData.GoneMadInnerInjury ? 1 : 0, -1, -1, EDamageType.None);
			}
			CS$<>8__locals1.addingDisorderOfQi = true;
			CS$<>8__locals1.character.GetCharacter().ChangeDisorderOfQiRandomRecovery(this.Context, CombatDomain.<AddGoneMadInjury>g__ModifyValue|616_0((int)configData.GoneMadQiDisorder, ref CS$<>8__locals1));
		}

		// Token: 0x060064FE RID: 25854 RVA: 0x003993A8 File Offset: 0x003975A8
		public void AddGoneMadInjuryOutOfCombat(DataContext context, GameData.Domains.Character.Character character, short skillId)
		{
			CombatSkillItem configData = Config.CombatSkill.Instance[skillId];
			Injuries injuries = character.GetInjuries();
			List<sbyte> partRandomPool = ObjectPool<List<sbyte>>.Instance.Get();
			partRandomPool.Clear();
			foreach (sbyte part in configData.GoneMadInjuredPart)
			{
				bool flag = injuries.Get(part, configData.GoneMadInnerInjury) < 6;
				if (flag)
				{
					partRandomPool.Add(part);
				}
			}
			bool flag2 = partRandomPool.Count > 0;
			if (flag2)
			{
				sbyte part2 = partRandomPool[context.Random.Next(0, partRandomPool.Count)];
				injuries.Change(part2, configData.GoneMadInnerInjury, (int)configData.GoneMadInjuryValue);
				character.SetInjuries(injuries, context);
			}
			ObjectPool<List<sbyte>>.Instance.Return(partRandomPool);
		}

		// Token: 0x060064FF RID: 25855 RVA: 0x00399498 File Offset: 0x00397698
		[DomainMethod]
		public bool InterruptSkillManual(DataContext context, bool isAlly = true)
		{
			return this.InterruptSkill(context, isAlly ? this._selfChar : this._enemyChar, -1);
		}

		// Token: 0x06006500 RID: 25856 RVA: 0x003994C4 File Offset: 0x003976C4
		public bool InterruptSkill(DataContext context, CombatCharacter character, int odds = 100)
		{
			short preparingSkillId = character.GetPreparingSkillId();
			bool flag = preparingSkillId < 0;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				bool flag2 = odds > 0;
				if (flag2)
				{
					bool canInterrupt = DomainManager.SpecialEffect.ModifyData(character.GetId(), preparingSkillId, 215, true, -1, -1, -1);
					bool flag3 = canInterrupt;
					if (flag3)
					{
						odds += DomainManager.SpecialEffect.GetModifyValue(character.GetId(), preparingSkillId, 216, EDataModifyType.Add, -1, -1, -1, EDataSumType.All);
					}
					else
					{
						odds = 0;
					}
					odds = Math.Max(odds, 0);
				}
				bool flag4 = odds < 0 || context.Random.CheckPercentProb(odds);
				if (flag4)
				{
					character.SetPreparingSkillId(-1, context);
					DomainManager.Combat.RaiseCastSkillEndByInterrupt(context, character.GetId(), character.IsAlly, preparingSkillId);
					result = true;
				}
				else
				{
					result = false;
				}
			}
			return result;
		}

		// Token: 0x06006501 RID: 25857 RVA: 0x00399588 File Offset: 0x00397788
		public int GetInterruptSkillOdds(CombatSkillKey skillKey, CombatCharacter castingChar)
		{
			short preparingSkillId = castingChar.GetPreparingSkillId();
			bool flag = preparingSkillId < 0;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				sbyte direction = DomainManager.CombatSkill.GetSkillDirection(skillKey.CharId, skillKey.SkillTemplateId);
				bool flag2 = direction == -1;
				if (flag2)
				{
					result = 0;
				}
				else
				{
					bool flag3 = !DomainManager.SpecialEffect.ModifyData(castingChar.GetId(), preparingSkillId, 215, true, -1, -1, -1);
					if (flag3)
					{
						result = 0;
					}
					else
					{
						Dictionary<short, Func<CombatSkillKey, bool, CombatSkillKey, int>> funcDict = CombatSkillEffectBase.CalcInterruptOddsFuncDict;
						int interruptOdds = funcDict.ContainsKey(skillKey.SkillTemplateId) ? funcDict[skillKey.SkillTemplateId](skillKey, direction == 0, new CombatSkillKey(castingChar.GetId(), preparingSkillId)) : 0;
						interruptOdds += DomainManager.SpecialEffect.GetModifyValue(castingChar.GetId(), preparingSkillId, 216, EDataModifyType.Add, -1, -1, -1, EDataSumType.All);
						result = Math.Clamp(interruptOdds, 0, 100);
					}
				}
			}
			return result;
		}

		// Token: 0x06006502 RID: 25858 RVA: 0x00399668 File Offset: 0x00397868
		public bool SilenceSkill(DataContext context, CombatCharacter character, short skillId, int silenceFrame, int odds = 100)
		{
			CombatSkillKey skillKey = new CombatSkillKey(character.GetId(), skillId);
			GameData.Domains.CombatSkill.CombatSkill skill = DomainManager.CombatSkill.GetElement_CombatSkills(skillKey);
			bool flag = odds > 0;
			if (flag)
			{
				bool canSilence = DomainManager.SpecialEffect.ModifyData(character.GetId(), skillId, 217, true, -1, -1, -1);
				bool flag2 = !canSilence;
				if (flag2)
				{
					return false;
				}
				int pageEffect = skill.GetPageEffects().Sum((SkillBreakPageEffectImplementItem x) => x.SilenceRate);
				odds = DomainManager.SpecialEffect.ModifyValue(character.GetId(), skillId, 218, odds, -1, -1, -1, 0, pageEffect, 0, 0);
				odds = Math.Max(odds, 0);
			}
			bool flag3 = odds >= 0 && !context.Random.CheckPercentProb(odds);
			bool result;
			if (flag3)
			{
				result = false;
			}
			else
			{
				bool flag4 = silenceFrame > 0;
				if (flag4)
				{
					int pageEffect2 = skill.GetPageEffects().Sum((SkillBreakPageEffectImplementItem x) => x.SilenceFrame);
					ValueTuple<int, int> extraTotalPercent = character.GetFeatureSilenceFrameTotalPercent();
					silenceFrame = DomainManager.SpecialEffect.ModifyValue(character.GetId(), skillId, 265, silenceFrame, -1, -1, -1, 0, pageEffect2, extraTotalPercent.Item1, extraTotalPercent.Item2);
				}
				short silenceFrameShort = (short)Math.Clamp(silenceFrame, -1, 32767);
				bool flag5 = silenceFrameShort == 0;
				if (flag5)
				{
					result = false;
				}
				else
				{
					CombatSkillData skillData = this._skillDataDict[skillKey];
					bool flag6 = skillData.GetLeftCdFrame() < 0 || skillData.GetTotalCdFrame() < 0;
					if (flag6)
					{
						result = false;
					}
					else
					{
						bool flag7 = silenceFrameShort < 0 || silenceFrameShort > skillData.GetTotalCdFrame() || skillData.GetLeftCdFrame() == 0;
						if (flag7)
						{
							skillData.SetTotalCdFrame(silenceFrameShort, context);
						}
						bool flag8 = silenceFrameShort < 0 || silenceFrameShort > skillData.GetLeftCdFrame();
						if (flag8)
						{
							skillData.SetLeftCdFrame(silenceFrameShort, context);
						}
						skillData.RaiseSkillSilence(context);
						this.UpdateSkillCanUse(context, character, skillId);
						bool flag9 = character.GetAffectingMoveSkillId() == skillId;
						if (flag9)
						{
							this.ClearAffectingAgileSkill(context, character);
						}
						bool flag10 = character.GetAffectingDefendSkillId() == skillId;
						if (flag10)
						{
							this.ClearAffectingDefenseSkill(context, character);
						}
						bool flag11 = character.GetPreparingSkillId() == skillId;
						if (flag11)
						{
							this.InterruptSkill(context, character, 100);
						}
						result = true;
					}
				}
			}
			return result;
		}

		// Token: 0x06006503 RID: 25859 RVA: 0x003998B8 File Offset: 0x00397AB8
		public void DoubleSkillCd(DataContext context, CombatCharacter character, short skillId)
		{
			CombatSkillKey skillKey = new CombatSkillKey(character.GetId(), skillId);
			CombatSkillData skillData = this._skillDataDict[skillKey];
			bool flag = skillData.GetLeftCdFrame() <= 0;
			if (!flag)
			{
				short newTotalCd = (short)Math.Min((int)(skillData.GetTotalCdFrame() * 2), 32767);
				short newCd = (short)Math.Min((int)(skillData.GetLeftCdFrame() * 2), (int)newTotalCd);
				skillData.SetTotalCdFrame(newTotalCd, context);
				skillData.SetLeftCdFrame(newCd, context);
			}
		}

		// Token: 0x06006504 RID: 25860 RVA: 0x0039992C File Offset: 0x00397B2C
		public void ResetSkillCd(DataContext context, CombatCharacter character, short skillId)
		{
			CombatSkillKey skillKey = new CombatSkillKey(character.GetId(), skillId);
			CombatSkillData skillData = this._skillDataDict[skillKey];
			skillData.SetLeftCdFrame(skillData.GetTotalCdFrame(), context);
		}

		// Token: 0x06006505 RID: 25861 RVA: 0x00399964 File Offset: 0x00397B64
		public void ClearSkillCd(DataContext context, CombatCharacter character, short skillId)
		{
			CombatSkillKey skillKey = new CombatSkillKey(character.GetId(), skillId);
			CombatSkillData skillData = this._skillDataDict[skillKey];
			skillData.SetTotalCdFrame(0, context);
			skillData.SetLeftCdFrame(0, context);
			skillData.RaiseSkillSilenceEnd(context);
			this.UpdateSkillCanUse(context, character, skillId);
		}

		// Token: 0x06006506 RID: 25862 RVA: 0x003999B1 File Offset: 0x00397BB1
		public void RaiseCastSkillEndByInterrupt(DataContext context, int charId, bool isAlly, short skillId)
		{
			this.RaiseCastSkillEnd(context, charId, isAlly, skillId, 0, true, 0);
		}

		// Token: 0x06006507 RID: 25863 RVA: 0x003999C4 File Offset: 0x00397BC4
		public void RaiseCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power = 0, bool interrupt = false, int finalCriticalOdds = 0)
		{
			Events.RaiseCastSkillEnd(context, charId, isAlly, skillId, power, interrupt);
			CombatCharacter combatChar;
			bool flag = this.IsInCombat() && this._combatCharacterDict.TryGetValue(charId, out combatChar);
			if (flag)
			{
				DomainManager.Combat.OnCastSkillEndEffect(context, combatChar, skillId, (int)power, finalCriticalOdds);
			}
			Events.RaiseCastSkillAllEnd(context, charId, skillId);
		}

		// Token: 0x06006508 RID: 25864 RVA: 0x00399A1B File Offset: 0x00397C1B
		public void OnCastSkillEndEffect(DataContext context, CombatCharacter combatChar, short skillId, int power = 0, int finalCriticalOdds = 0)
		{
			this.OnCastSkillEndBreakBonus(context, combatChar, skillId);
			this.OnCastSkillEndFeature(context, combatChar, skillId, power, finalCriticalOdds);
		}

		// Token: 0x06006509 RID: 25865 RVA: 0x00399A38 File Offset: 0x00397C38
		private void OnCastSkillEndBreakBonus(DataContext context, CombatCharacter combatChar, short skillId)
		{
			GameData.Domains.CombatSkill.CombatSkill combatSkill;
			bool flag = !DomainManager.CombatSkill.TryGetElement_CombatSkills(new ValueTuple<int, short>(combatChar.GetId(), skillId), out combatSkill);
			if (!flag)
			{
				int silenceRate = combatSkill.GetBreakoutGridCombatSkillPropertyBonus(70);
				bool flag2 = silenceRate <= 0;
				if (!flag2)
				{
					int silenceFrame = combatSkill.GetBreakoutGridCombatSkillPropertyBonus(71);
					this.SilenceSkill(context, combatChar, skillId, (int)((short)Math.Clamp(silenceFrame, 0, 32767)), silenceRate);
				}
			}
		}

		// Token: 0x0600650A RID: 25866 RVA: 0x00399AA8 File Offset: 0x00397CA8
		private void OnCastSkillEndFeature(DataContext context, CombatCharacter attacker, short skillId, int power, int finalCriticalOdds)
		{
			CombatDomain.<>c__DisplayClass629_0 CS$<>8__locals1;
			CS$<>8__locals1.context = context;
			CS$<>8__locals1.finalCriticalOdds = finalCriticalOdds;
			CS$<>8__locals1.attacker = attacker;
			CS$<>8__locals1.skillId = skillId;
			bool flag = power < 100;
			if (!flag)
			{
				CombatCharacter defender = this.GetCombatCharacter(!CS$<>8__locals1.attacker.IsAlly, true);
				List<short> attackerFeatures = CS$<>8__locals1.attacker.GetCharacter().GetFeatureIds();
				List<short> defenderFeatures = defender.GetCharacter().GetFeatureIds();
				byte neiliTypeFiveElements = NeiliType.Instance[defender.GetNeiliType()].FiveElements;
				foreach (LifeLinkFeatureEffectItem config in ((IEnumerable<LifeLinkFeatureEffectItem>)LifeLinkFeatureEffect.Instance))
				{
					CombatDomain.<>c__DisplayClass629_1 CS$<>8__locals2;
					CS$<>8__locals2.config = config;
					bool featureInAttacker = attackerFeatures.Contains(CS$<>8__locals2.config.FeatureId);
					bool featureInDefender = defenderFeatures.Contains(CS$<>8__locals2.config.FeatureId);
					bool flag2 = !featureInAttacker && !featureInDefender;
					if (!flag2)
					{
						bool flag3 = CS$<>8__locals2.config.CriticalProbPercent > 0;
						if (flag3)
						{
							bool flag4 = CombatDomain.<OnCastSkillEndFeature>g__CheckProb|629_0(ref CS$<>8__locals1, ref CS$<>8__locals2) && featureInAttacker && CombatDomain.<OnCastSkillEndFeature>g__FiveElementEquals|629_1((int)CS$<>8__locals2.config.FiveElements, ref CS$<>8__locals1) && neiliTypeFiveElements == (byte)FiveElementsType.Countering[(int)CS$<>8__locals2.config.FiveElements];
							if (flag4)
							{
								this.AddGoneMadInjury(CS$<>8__locals1.context, defender, CS$<>8__locals1.skillId, 0);
								this.ShowSpecialEffectTips(CS$<>8__locals1.attacker.GetId(), 1713, 0);
							}
							bool flag5 = CombatDomain.<OnCastSkillEndFeature>g__CheckProb|629_0(ref CS$<>8__locals1, ref CS$<>8__locals2) && featureInDefender && CombatDomain.<OnCastSkillEndFeature>g__FiveElementEquals|629_1((int)FiveElementsType.Countered[(int)CS$<>8__locals2.config.FiveElements], ref CS$<>8__locals1);
							if (flag5)
							{
								this.AddGoneMadInjury(CS$<>8__locals1.context, defender, CS$<>8__locals1.skillId, 0);
								this.ShowSpecialEffectTips(defender.GetId(), 1714, 0);
							}
						}
						bool flag6 = CS$<>8__locals2.config.CriticalProbPercent < 0 && featureInDefender;
						if (flag6)
						{
							bool flag7 = CombatDomain.<OnCastSkillEndFeature>g__CheckProb|629_0(ref CS$<>8__locals1, ref CS$<>8__locals2) && (CombatDomain.<OnCastSkillEndFeature>g__FiveElementEquals|629_1((int)CS$<>8__locals2.config.FiveElements, ref CS$<>8__locals1) || CombatDomain.<OnCastSkillEndFeature>g__FiveElementEquals|629_1((int)FiveElementsType.Countered[(int)CS$<>8__locals2.config.FiveElements], ref CS$<>8__locals1));
							if (flag7)
							{
								this.AddGoneMadInjury(CS$<>8__locals1.context, defender, CS$<>8__locals1.skillId, 0);
								this.ShowSpecialEffectTips(defender.GetId(), 1715, 0);
							}
						}
					}
				}
			}
		}

		// Token: 0x0600650B RID: 25867 RVA: 0x00399D3C File Offset: 0x00397F3C
		public short GetRandomAttackSkill(CombatCharacter combatChar, sbyte skillType, sbyte targetGrade, IRandomSource random, bool descSearch = true, short expectSkillId = -1)
		{
			List<short> attackSkillList = ObjectPool<List<short>>.Instance.Get();
			List<short> skillRandomPool = ObjectPool<List<short>>.Instance.Get();
			attackSkillList.Clear();
			attackSkillList.AddRange(combatChar.GetAttackSkillList());
			attackSkillList.RemoveAll((short id) => id < 0 || id == expectSkillId);
			skillRandomPool.Clear();
			targetGrade = Math.Clamp(targetGrade, 0, 8);
			for (int grade = (int)targetGrade; grade != (descSearch ? -1 : 9); grade += (descSearch ? -1 : 1))
			{
				for (int i = 0; i < attackSkillList.Count; i++)
				{
					short attackSkillId = attackSkillList[i];
					bool flag = (int)Config.CombatSkill.Instance[attackSkillId].Grade == grade && DomainManager.CombatSkill.GetSkillType(combatChar.GetId(), attackSkillId) == skillType && DomainManager.Combat.CanCastSkill(combatChar, attackSkillId, true, false);
					if (flag)
					{
						skillRandomPool.Add(attackSkillId);
					}
				}
				bool flag2 = skillRandomPool.Count > 0;
				if (flag2)
				{
					break;
				}
			}
			short skillId = (skillRandomPool.Count > 0) ? skillRandomPool[random.Next(0, skillRandomPool.Count)] : -1;
			ObjectPool<List<short>>.Instance.Return(attackSkillList);
			ObjectPool<List<short>>.Instance.Return(skillRandomPool);
			return skillId;
		}

		// Token: 0x0600650C RID: 25868 RVA: 0x00399E9C File Offset: 0x0039809C
		[DomainMethod]
		public void ClearAffectingMoveSkillManual(DataContext context, bool isAlly)
		{
			CombatCharacter combatChar = isAlly ? this._selfChar : this._enemyChar;
			this.ClearAffectingAgileSkill(context, combatChar);
		}

		// Token: 0x0600650D RID: 25869 RVA: 0x00399EC8 File Offset: 0x003980C8
		public bool ClearAffectingAgileSkillByEffect(DataContext context, CombatCharacter character, CombatCharacter changer = null)
		{
			bool flag = !DomainManager.SpecialEffect.ModifyData(character.GetId(), -1, 149, true, (changer != null) ? changer.GetId() : -1, -1, -1);
			return !flag && this.ClearAffectingAgileSkill(context, character);
		}

		// Token: 0x0600650E RID: 25870 RVA: 0x00399F14 File Offset: 0x00398114
		public bool ClearAffectingAgileSkill(DataContext context, CombatCharacter character)
		{
			bool flag = character.GetAffectingMoveSkillId() < 0;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				bool flag2 = character.NeedAddEffectAgileSkillId == character.GetAffectingMoveSkillId();
				if (flag2)
				{
					character.NeedAddEffectAgileSkillId = -1;
				}
				character.SetAffectingMoveSkillId(-1, context);
				result = true;
			}
			return result;
		}

		// Token: 0x0600650F RID: 25871 RVA: 0x00399F5C File Offset: 0x0039815C
		[DomainMethod]
		public void ClearAffectingDefenseSkillManual(DataContext context, bool isAlly)
		{
			CombatCharacter combatChar = isAlly ? this._selfChar : this._enemyChar;
			this.ClearAffectingDefenseSkill(context, combatChar);
		}

		// Token: 0x06006510 RID: 25872 RVA: 0x00399F88 File Offset: 0x00398188
		public void ClearAffectingDefenseSkill(DataContext context, CombatCharacter character)
		{
			bool flag = character.GetAffectingDefendSkillId() < 0;
			if (!flag)
			{
				character.DefendSkillLeftFrame = 1;
				character.SetAffectingDefendSkillId(-1, context);
				this.SetProperLoopAniAndParticle(context, character, false);
			}
		}

		// Token: 0x06006511 RID: 25873 RVA: 0x00399FC0 File Offset: 0x003981C0
		public bool CanCastSkill(CombatCharacter character, short skillId, bool costFree = false, bool checkRange = false)
		{
			CombatSkillData skillData;
			this._skillDataDict.TryGetValue(new CombatSkillKey(character.GetId(), skillId), out skillData);
			CombatSkillItem configData = Config.CombatSkill.Instance[skillId];
			bool isAttackSkill = configData.EquipType == 1;
			return character.PreventCastSkillEffectCount == 0 && !character.PreparingTeammateCommand() && (skillData == null || skillData.GetLeftCdFrame() == 0) && (costFree || this.SkillCostEnough(character, skillId)) && this.HasSkillNeedBodyPart(character, skillId, true) && (!isAttackSkill || this.WeaponHasNeedTrick(character, skillId, this.GetUsingWeaponData(character))) && (!character.IsAlly || costFree || this.SkillCanUseInCurrCombat(character.GetId(), configData)) && this.SkillDirectionCanCast(character, skillId) && (!isAttackSkill || !checkRange || this.SkillInCastRange(character, skillId));
		}

		// Token: 0x06006512 RID: 25874 RVA: 0x0039A084 File Offset: 0x00398284
		public void CastSkillFree(DataContext context, CombatCharacter character, short skillId, ECombatCastFreePriority priority = ECombatCastFreePriority.Normal)
		{
			bool flag = character.CastFreeDataList.Contains(new ValueTuple<short, ECombatCastFreePriority>(skillId, priority));
			if (!flag)
			{
				character.CastFreeDataList.Add(new ValueTuple<short, ECombatCastFreePriority>(skillId, priority));
				character.CastFreeDataList.Sort();
				this.UpdateAllCommandAvailability(context, character);
			}
		}

		// Token: 0x06006513 RID: 25875 RVA: 0x0039A0E0 File Offset: 0x003982E0
		public void ChangeSkillPrepareProgress(CombatCharacter character, int progress)
		{
			bool flag = character.GetPreparingSkillId() < 0;
			if (!flag)
			{
				character.SkillPrepareCurrProgress = Math.Max(character.SkillPrepareCurrProgress, progress);
			}
		}

		// Token: 0x06006514 RID: 25876 RVA: 0x0039A110 File Offset: 0x00398310
		public void AddSkillPowerInCombat(DataContext context, CombatSkillKey skillKey, SkillEffectKey effectKey, int power)
		{
			bool flag = power <= 0;
			if (!flag)
			{
				SkillPowerChangeCollection powerCollection;
				bool exist = this._skillPowerAddInCombat.TryGetValue(skillKey, out powerCollection);
				bool flag2 = !exist;
				if (flag2)
				{
					powerCollection = new SkillPowerChangeCollection();
				}
				powerCollection.Add(effectKey, power);
				bool flag3 = !exist;
				if (flag3)
				{
					this.AddElement_SkillPowerAddInCombat(skillKey, powerCollection, context);
				}
				else
				{
					this.SetElement_SkillPowerAddInCombat(skillKey, powerCollection, context);
				}
			}
		}

		// Token: 0x06006515 RID: 25877 RVA: 0x0039A174 File Offset: 0x00398374
		public SkillPowerChangeCollection RemoveSkillPowerAddInCombat(DataContext context, CombatSkillKey skillKey)
		{
			SkillPowerChangeCollection powerCollection;
			bool flag = !this._skillPowerAddInCombat.TryGetValue(skillKey, out powerCollection);
			SkillPowerChangeCollection result;
			if (flag)
			{
				result = null;
			}
			else
			{
				bool addPowerCanBeRemoved = DomainManager.SpecialEffect.ModifyData(skillKey.CharId, skillKey.SkillTemplateId, 220, true, -1, -1, -1);
				bool flag2 = !addPowerCanBeRemoved;
				if (flag2)
				{
					result = null;
				}
				else
				{
					this.RemoveElement_SkillPowerAddInCombat(skillKey, context);
					result = powerCollection;
				}
			}
			return result;
		}

		// Token: 0x06006516 RID: 25878 RVA: 0x0039A1D8 File Offset: 0x003983D8
		public int RemoveSkillPowerAddInCombat(DataContext context, CombatSkillKey skillKey, SkillEffectKey source)
		{
			SkillPowerChangeCollection powerCollection;
			bool flag = !this._skillPowerAddInCombat.TryGetValue(skillKey, out powerCollection);
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				bool addPowerCanBeRemoved = DomainManager.SpecialEffect.ModifyData(skillKey.CharId, skillKey.SkillTemplateId, 220, true, -1, -1, -1);
				bool flag2 = !addPowerCanBeRemoved;
				if (flag2)
				{
					result = 0;
				}
				else
				{
					bool flag3 = powerCollection.EffectDict.ContainsKey(source);
					if (flag3)
					{
						int power = powerCollection.EffectDict[source];
						powerCollection.EffectDict.Remove(source);
						this.SetElement_SkillPowerAddInCombat(skillKey, powerCollection, context);
						result = power;
					}
					else
					{
						result = 0;
					}
				}
			}
			return result;
		}

		// Token: 0x06006517 RID: 25879 RVA: 0x0039A270 File Offset: 0x00398470
		public Dictionary<CombatSkillKey, SkillPowerChangeCollection> GetAllSkillPowerAddInCombat()
		{
			return this._skillPowerAddInCombat;
		}

		// Token: 0x06006518 RID: 25880 RVA: 0x0039A288 File Offset: 0x00398488
		public int GetReduceSkillPowerInCombat(CombatSkillKey skillKey, SkillEffectKey effectKey)
		{
			SkillPowerChangeCollection powerCollection;
			bool flag = !this._skillPowerReduceInCombat.TryGetValue(skillKey, out powerCollection);
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				int value;
				result = (powerCollection.EffectDict.TryGetValue(effectKey, out value) ? value : 0);
			}
			return result;
		}

		// Token: 0x06006519 RID: 25881 RVA: 0x0039A2C8 File Offset: 0x003984C8
		public void ReduceSkillPowerInCombat(DataContext context, CombatSkillKey skillKey, SkillEffectKey effectKey, int power)
		{
			bool flag = power >= 0;
			if (!flag)
			{
				SkillPowerChangeCollection powerCollection;
				bool exist = this._skillPowerReduceInCombat.TryGetValue(skillKey, out powerCollection);
				bool flag2 = !exist;
				if (flag2)
				{
					powerCollection = new SkillPowerChangeCollection();
				}
				powerCollection.Add(effectKey, power);
				bool flag3 = !exist;
				if (flag3)
				{
					this.AddElement_SkillPowerReduceInCombat(skillKey, powerCollection, context);
				}
				else
				{
					this.SetElement_SkillPowerReduceInCombat(skillKey, powerCollection, context);
				}
			}
		}

		// Token: 0x0600651A RID: 25882 RVA: 0x0039A32C File Offset: 0x0039852C
		public SkillPowerChangeCollection RemoveSkillPowerReduceInCombat(DataContext context, CombatSkillKey skillKey)
		{
			SkillPowerChangeCollection powerCollection;
			bool flag = !this._skillPowerReduceInCombat.TryGetValue(skillKey, out powerCollection);
			SkillPowerChangeCollection result;
			if (flag)
			{
				result = null;
			}
			else
			{
				this.RemoveElement_SkillPowerReduceInCombat(skillKey, context);
				result = powerCollection;
			}
			return result;
		}

		// Token: 0x0600651B RID: 25883 RVA: 0x0039A364 File Offset: 0x00398564
		public int RemoveSkillPowerReduceInCombat(DataContext context, CombatSkillKey skillKey, SkillEffectKey source)
		{
			SkillPowerChangeCollection powerCollection;
			bool flag = !this._skillPowerReduceInCombat.TryGetValue(skillKey, out powerCollection);
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				bool flag2 = powerCollection.EffectDict.ContainsKey(source);
				if (flag2)
				{
					int power = powerCollection.EffectDict[source];
					powerCollection.EffectDict.Remove(source);
					this.SetElement_SkillPowerReduceInCombat(skillKey, powerCollection, context);
					result = power;
				}
				else
				{
					result = 0;
				}
			}
			return result;
		}

		// Token: 0x0600651C RID: 25884 RVA: 0x0039A3CC File Offset: 0x003985CC
		public Dictionary<CombatSkillKey, SkillPowerChangeCollection> GetAllSkillPowerReduceInCombat()
		{
			return this._skillPowerReduceInCombat;
		}

		// Token: 0x0600651D RID: 25885 RVA: 0x0039A3E4 File Offset: 0x003985E4
		public void SetSkillPowerReplaceInCombat(DataContext context, CombatSkillKey targetSkillKey, CombatSkillKey powerSkillKey)
		{
			bool flag = !this._skillPowerReplaceInCombat.ContainsKey(targetSkillKey);
			if (flag)
			{
				this.AddElement_SkillPowerReplaceInCombat(targetSkillKey, powerSkillKey, context);
			}
			else
			{
				this.SetElement_SkillPowerReplaceInCombat(targetSkillKey, powerSkillKey, context);
			}
		}

		// Token: 0x0600651E RID: 25886 RVA: 0x0039A41C File Offset: 0x0039861C
		public void RemoveSkillPowerReplaceInCombat(DataContext context, CombatSkillKey targetSkillKey)
		{
			bool flag = this._skillPowerReplaceInCombat.ContainsKey(targetSkillKey);
			if (flag)
			{
				this.RemoveElement_SkillPowerReplaceInCombat(targetSkillKey, context);
			}
		}

		// Token: 0x0600651F RID: 25887 RVA: 0x0039A444 File Offset: 0x00398644
		public Dictionary<CombatSkillKey, CombatSkillKey> GetAllSkillPowerReplaceInCombat()
		{
			return this._skillPowerReplaceInCombat;
		}

		// Token: 0x06006520 RID: 25888 RVA: 0x0039A45C File Offset: 0x0039865C
		public void AddMoveDistInSkillPrepare(CombatCharacter character, short dist, bool forward)
		{
			if (forward)
			{
				character.MoveData.CanMoveForwardInSkillPrepareDist = character.MoveData.CanMoveForwardInSkillPrepareDist + dist;
			}
			else
			{
				character.MoveData.CanMoveBackwardInSkillPrepareDist = character.MoveData.CanMoveBackwardInSkillPrepareDist + dist;
			}
		}

		// Token: 0x06006521 RID: 25889 RVA: 0x0039A494 File Offset: 0x00398694
		public void AddSkillEffect(DataContext context, CombatCharacter combatChar, SkillEffectKey key, short count, short maxCount, bool autoRemoveOnNoCount)
		{
			SkillEffectCollection effectCollection = combatChar.GetSkillEffectCollection();
			SkillEffectCollection skillEffectCollection = effectCollection;
			if (skillEffectCollection.EffectDict == null)
			{
				skillEffectCollection.EffectDict = new Dictionary<SkillEffectKey, short>();
			}
			skillEffectCollection = effectCollection;
			if (skillEffectCollection.EffectDescriptionDict == null)
			{
				skillEffectCollection.EffectDescriptionDict = new Dictionary<SkillEffectKey, CombatSkillEffectDescriptionDisplayData>();
			}
			effectCollection.EffectDict[key] = count;
			effectCollection.EffectDescriptionDict[key] = DomainManager.CombatSkill.GetEffectDisplayData(combatChar.GetId(), key.SkillId);
			effectCollection.MaxEffectCountDict[key] = maxCount;
			effectCollection.AutoRemoveOnNoCountDict[key] = autoRemoveOnNoCount;
			combatChar.SetSkillEffectCollection(effectCollection, context);
			Events.RaiseSkillEffectChange(context, combatChar.GetId(), key, 0, count, false);
		}

		// Token: 0x06006522 RID: 25890 RVA: 0x0039A540 File Offset: 0x00398740
		public void ChangeSkillEffectCount(DataContext context, CombatCharacter combatChar, SkillEffectKey key, short addValue, bool raiseEvent = true, bool forceChange = false)
		{
			bool flag = !DomainManager.SpecialEffect.ModifyData(combatChar.GetId(), key.SkillId, 222, true, -1, -1, -1) && !forceChange;
			if (!flag)
			{
				SkillEffectCollection effectCollection = combatChar.GetSkillEffectCollection();
				bool flag2 = effectCollection.EffectDict != null && effectCollection.EffectDict.ContainsKey(key);
				if (flag2)
				{
					short oldCount = effectCollection.EffectDict[key];
					short maxCount = effectCollection.MaxEffectCountDict[key];
					short count = (short)Math.Clamp((int)(oldCount + addValue), 0, (int)maxCount);
					bool flag3 = count == 0 && effectCollection.AutoRemoveOnNoCountDict[key];
					if (flag3)
					{
						this.RemoveSkillEffect(context, combatChar, key);
					}
					else
					{
						effectCollection.EffectDict[key] = count;
						combatChar.SetSkillEffectCollection(effectCollection, context);
						if (raiseEvent)
						{
							Events.RaiseSkillEffectChange(context, combatChar.GetId(), key, oldCount, count, false);
						}
					}
				}
			}
		}

		// Token: 0x06006523 RID: 25891 RVA: 0x0039A634 File Offset: 0x00398834
		public bool IsSkillEffectExist(CombatCharacter combatChar, SkillEffectKey key)
		{
			SkillEffectCollection effectCollection = combatChar.GetSkillEffectCollection();
			return effectCollection.EffectDict != null && effectCollection.EffectDict.ContainsKey(key);
		}

		// Token: 0x06006524 RID: 25892 RVA: 0x0039A664 File Offset: 0x00398864
		public short GetSkillEffectCount(CombatCharacter combatChar, SkillEffectKey key)
		{
			SkillEffectCollection effectCollection = combatChar.GetSkillEffectCollection();
			bool flag = effectCollection.EffectDict != null && effectCollection.EffectDict.ContainsKey(key);
			short result;
			if (flag)
			{
				result = effectCollection.EffectDict[key];
			}
			else
			{
				result = 0;
			}
			return result;
		}

		// Token: 0x06006525 RID: 25893 RVA: 0x0039A6A8 File Offset: 0x003988A8
		public void ClearSkillEffect(DataContext context, CombatCharacter combatChar)
		{
			SkillEffectCollection effectCollection = combatChar.GetSkillEffectCollection();
			bool flag = effectCollection.EffectDict == null;
			if (!flag)
			{
				List<SkillEffectKey> keys = ObjectPool<List<SkillEffectKey>>.Instance.Get();
				List<short> values = ObjectPool<List<short>>.Instance.Get();
				keys.Clear();
				values.Clear();
				keys.AddRange(effectCollection.EffectDict.Keys);
				values.AddRange(effectCollection.EffectDict.Values);
				effectCollection.EffectDict.Clear();
				effectCollection.EffectDescriptionDict.Clear();
				effectCollection.MaxEffectCountDict.Clear();
				effectCollection.AutoRemoveOnNoCountDict.Clear();
				combatChar.SetSkillEffectCollection(effectCollection, context);
				for (int i = 0; i < keys.Count; i++)
				{
					Events.RaiseSkillEffectChange(context, combatChar.GetId(), keys[i], values[i], 0, true);
				}
				ObjectPool<List<SkillEffectKey>>.Instance.Return(keys);
				ObjectPool<List<short>>.Instance.Return(values);
			}
		}

		// Token: 0x06006526 RID: 25894 RVA: 0x0039A7A4 File Offset: 0x003989A4
		public void RemoveSkillEffect(DataContext context, CombatCharacter combatChar, SkillEffectKey key)
		{
			SkillEffectCollection effectCollection = combatChar.GetSkillEffectCollection();
			bool flag = effectCollection.EffectDict != null && effectCollection.EffectDict.ContainsKey(key);
			if (flag)
			{
				short oldCount = effectCollection.EffectDict[key];
				effectCollection.EffectDict.Remove(key);
				effectCollection.EffectDescriptionDict.Remove(key);
				effectCollection.MaxEffectCountDict.Remove(key);
				effectCollection.AutoRemoveOnNoCountDict.Remove(key);
				combatChar.SetSkillEffectCollection(effectCollection, context);
				Events.RaiseSkillEffectChange(context, combatChar.GetId(), key, oldCount, 0, true);
			}
		}

		// Token: 0x06006527 RID: 25895 RVA: 0x0039A834 File Offset: 0x00398A34
		public bool ChangeSkillEffectRandom(DataContext context, CombatCharacter target, CValuePercent percent, int maxChangeCount = 1, sbyte requireEquipType = -1)
		{
			bool flag = percent == 0;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				SkillEffectCollection effectCollection = target.GetSkillEffectCollection();
				bool flag2 = ((effectCollection != null) ? effectCollection.EffectDict : null) == null;
				if (flag2)
				{
					result = false;
				}
				else
				{
					bool anyChanged = false;
					bool isAdd = percent > 0;
					int maxDeltaRange = 0;
					List<SkillEffectKey> prefer = ObjectPool<List<SkillEffectKey>>.Instance.Get();
					List<SkillEffectKey> normal = ObjectPool<List<SkillEffectKey>>.Instance.Get();
					foreach (KeyValuePair<SkillEffectKey, short> keyValuePair in effectCollection.EffectDict)
					{
						SkillEffectKey skillEffectKey;
						short num;
						keyValuePair.Deconstruct(out skillEffectKey, out num);
						SkillEffectKey key = skillEffectKey;
						short count = num;
						short maxCount = effectCollection.MaxEffectCountDict[key];
						int deltaRange = (int)(isAdd ? (maxCount - count) : count);
						bool flag3 = deltaRange == 0;
						if (!flag3)
						{
							bool flag4 = requireEquipType >= 0 && requireEquipType != Config.CombatSkill.Instance[key.SkillId].EquipType;
							if (!flag4)
							{
								bool flag5 = deltaRange > maxDeltaRange;
								if (flag5)
								{
									normal.AddRange(prefer);
									prefer.Clear();
									maxDeltaRange = deltaRange;
								}
								bool flag6 = deltaRange == maxDeltaRange;
								if (flag6)
								{
									prefer.Add(key);
								}
								else
								{
									normal.Add(key);
								}
							}
						}
					}
					foreach (SkillEffectKey key2 in RandomUtils.GetRandomUnrepeated<SkillEffectKey>(context.Random, maxChangeCount, prefer, normal))
					{
						anyChanged = true;
						int delta = (int)effectCollection.MaxEffectCountDict[key2] * percent;
						bool flag7 = delta == 0;
						if (flag7)
						{
							delta = (isAdd ? 1 : -1);
						}
						this.ChangeSkillEffectCount(context, target, key2, (short)delta, true, false);
					}
					ObjectPool<List<SkillEffectKey>>.Instance.Return(prefer);
					ObjectPool<List<SkillEffectKey>>.Instance.Return(normal);
					result = anyChanged;
				}
			}
			return result;
		}

		// Token: 0x06006528 RID: 25896 RVA: 0x0039AA40 File Offset: 0x00398C40
		public void ChangeSkillEffectToMinCount(DataContext context, CombatCharacter combatChar, SkillEffectKey key)
		{
			SkillEffectCollection effectCollection = combatChar.GetSkillEffectCollection();
			bool flag = effectCollection.EffectDict != null && effectCollection.EffectDict.ContainsKey(key);
			if (flag)
			{
				CombatSkillItem skillConfig = Config.CombatSkill.Instance[key.SkillId];
				int effectId = key.IsDirect ? skillConfig.DirectEffectID : skillConfig.ReverseEffectID;
				short minCount = SpecialEffect.Instance[effectId].MinEffectCount;
				bool flag2 = effectCollection.EffectDict[key] > minCount;
				if (flag2)
				{
					this.ChangeSkillEffectCount(context, combatChar, key, minCount - effectCollection.EffectDict[key], true, false);
				}
			}
		}

		// Token: 0x06006529 RID: 25897 RVA: 0x0039AAE0 File Offset: 0x00398CE0
		public void ChangeSkillEffectToMaxCount(DataContext context, CombatCharacter combatChar, SkillEffectKey key)
		{
			SkillEffectCollection effectCollection = combatChar.GetSkillEffectCollection();
			bool flag = effectCollection.EffectDict != null && effectCollection.EffectDict.ContainsKey(key);
			if (flag)
			{
				short maxCount = effectCollection.MaxEffectCountDict[key];
				bool flag2 = effectCollection.EffectDict[key] < maxCount;
				if (flag2)
				{
					this.ChangeSkillEffectCount(context, combatChar, key, maxCount - effectCollection.EffectDict[key], true, false);
				}
			}
		}

		// Token: 0x0600652A RID: 25898 RVA: 0x0039AB50 File Offset: 0x00398D50
		public void ChangeSkillEffectDirection(DataContext context, CombatCharacter combatChar, SkillEffectKey key, bool isDirect)
		{
			SkillEffectCollection effectCollection = combatChar.GetSkillEffectCollection();
			bool flag = effectCollection.EffectDict != null && effectCollection.EffectDict.ContainsKey(key);
			if (flag)
			{
				SkillEffectKey newKey = new SkillEffectKey(key.SkillId, isDirect);
				effectCollection.EffectDict.Add(newKey, effectCollection.EffectDict[key]);
				effectCollection.EffectDescriptionDict.Add(newKey, effectCollection.EffectDescriptionDict[key]);
				effectCollection.MaxEffectCountDict.Add(newKey, effectCollection.MaxEffectCountDict[key]);
				effectCollection.AutoRemoveOnNoCountDict.Add(newKey, effectCollection.AutoRemoveOnNoCountDict[key]);
				effectCollection.EffectDict.Remove(key);
				effectCollection.EffectDescriptionDict.Remove(key);
				effectCollection.MaxEffectCountDict.Remove(key);
				effectCollection.AutoRemoveOnNoCountDict.Remove(key);
				combatChar.SetSkillEffectCollection(effectCollection, context);
			}
		}

		// Token: 0x0600652B RID: 25899 RVA: 0x0039AC38 File Offset: 0x00398E38
		[DomainMethod]
		public void PlayMoveStepSound(DataContext context, bool isAlly)
		{
			bool flag = !this.IsInCombat();
			if (!flag)
			{
				CombatCharacter combatChar = isAlly ? this._selfChar : this._enemyChar;
				this.PlayStepSound(context, combatChar);
				this.PlayShockSound(context, combatChar);
			}
		}

		// Token: 0x0600652C RID: 25900 RVA: 0x0039AC7C File Offset: 0x00398E7C
		public void PlayHitSound(DataContext context, CombatCharacter character, WeaponItem weaponData)
		{
			List<string> hitSoundList = weaponData.HitSounds;
			ItemKey armorKey = character.Armors[0];
			bool flag = hitSoundList != null && hitSoundList.Count > 0;
			if (flag)
			{
				character.SetHitSoundToPlay(hitSoundList[context.Random.Next(hitSoundList.Count)], context);
			}
			bool flag2 = armorKey.IsValid();
			if (flag2)
			{
				sbyte resourceType = DomainManager.Item.GetElement_Armors(armorKey.Id).GetResourceType();
				bool flag3 = resourceType >= 0;
				if (flag3)
				{
					string[] armorHitSoundList = Config.ResourceType.Instance[resourceType].HitSound;
					string[] armorShockSoundList = Config.ResourceType.Instance[resourceType].ShockSound;
					bool flag4 = weaponData.PlayArmorHitSound && armorHitSoundList != null && armorHitSoundList.Length != 0;
					if (flag4)
					{
						character.SetArmorHitSoundToPlay(armorHitSoundList[context.Random.Next(armorHitSoundList.Length)], context);
					}
					bool flag5 = armorShockSoundList != null && armorShockSoundList.Length != 0;
					if (flag5)
					{
						character.SetShockSoundToPlay(armorShockSoundList[context.Random.Next(armorShockSoundList.Length)], context);
					}
				}
			}
		}

		// Token: 0x0600652D RID: 25901 RVA: 0x0039AD94 File Offset: 0x00398F94
		public void PlayBlockSound(DataContext context, CombatCharacter character)
		{
			string blockSound = null;
			bool flag = character.AnimalConfig == null;
			if (flag)
			{
				List<string> blockSoundList = Config.Weapon.Instance[this.GetUsingWeaponKey(character).TemplateId].BlockSounds;
				bool flag2 = blockSoundList != null && blockSoundList.Count > 0;
				if (flag2)
				{
					blockSound = blockSoundList[context.Random.Next(blockSoundList.Count)];
				}
			}
			else
			{
				blockSound = character.AnimalConfig.BlockSound;
			}
			bool flag3 = blockSound != null;
			if (flag3)
			{
				character.SetHitSoundToPlay(blockSound, context);
			}
		}

		// Token: 0x0600652E RID: 25902 RVA: 0x0039AE20 File Offset: 0x00399020
		public void PlayWhooshSound(DataContext context, CombatCharacter character)
		{
			ItemKey armorKey = character.Armors[0];
			bool flag = armorKey.IsValid();
			if (flag)
			{
				sbyte resourceType = DomainManager.Item.GetElement_Armors(armorKey.Id).GetResourceType();
				bool flag2 = resourceType >= 0;
				if (flag2)
				{
					string[] soundList = Config.ResourceType.Instance[resourceType].WhooshSound;
					bool flag3 = soundList != null && soundList.Length != 0;
					if (flag3)
					{
						character.SetWhooshSoundToPlay(soundList[context.Random.Next(soundList.Length)], context);
					}
				}
			}
			else
			{
				character.SetWhooshSoundToPlay("se_combat_whoosh_empty", context);
			}
		}

		// Token: 0x0600652F RID: 25903 RVA: 0x0039AEBC File Offset: 0x003990BC
		public void PlayShockSound(DataContext context, CombatCharacter character)
		{
			ItemKey armorKey = character.Armors[0];
			bool flag = armorKey.IsValid();
			if (flag)
			{
				sbyte resourceType = DomainManager.Item.GetElement_Armors(armorKey.Id).GetResourceType();
				bool flag2 = resourceType >= 0;
				if (flag2)
				{
					string[] soundList = Config.ResourceType.Instance[resourceType].ShockSound;
					bool flag3 = soundList != null && soundList.Length != 0;
					if (flag3)
					{
						character.SetShockSoundToPlay(soundList[context.Random.Next(soundList.Length)], context);
					}
				}
			}
		}

		// Token: 0x06006530 RID: 25904 RVA: 0x0039AF48 File Offset: 0x00399148
		public void PlayStepSound(DataContext context, CombatCharacter character)
		{
			ItemKey shoesKey = character.Armors[5];
			bool isAnimal = character.IsAnimal;
			IList<string> soundList;
			if (isAnimal)
			{
				soundList = character.AnimalConfig.StepSound;
			}
			else
			{
				bool flag = shoesKey.IsValid();
				if (flag)
				{
					sbyte resourceType = DomainManager.Item.GetElement_Armors(shoesKey.Id).GetResourceType();
					soundList = ((resourceType >= 0) ? Config.ResourceType.Instance[resourceType].StepSound : CombatDomain.NoResourceTypeStepSound);
				}
				else
				{
					soundList = CombatDomain.NoResourceTypeStepSound;
				}
			}
			bool flag2 = soundList != null && soundList.Count > 0;
			if (flag2)
			{
				character.SetStepSoundToPlay(soundList.GetRandom(context.Random), context);
			}
		}

		// Token: 0x06006531 RID: 25905 RVA: 0x0039AFF0 File Offset: 0x003991F0
		[DomainMethod]
		public bool ExecuteTeammateCommand(DataContext context, bool isAlly, int index, int charId)
		{
			bool flag = !this.IsCharInCombat(charId, true);
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				CombatCharacter teammateChar = this.GetElement_CombatCharacterDict(charId);
				bool flag2 = teammateChar.IsAlly != isAlly;
				if (flag2)
				{
					result = false;
				}
				else
				{
					CombatCharacter currChar = this.GetCombatCharacter(isAlly, false);
					List<sbyte> currCmds = teammateChar.GetCurrTeammateCommands();
					bool flag3 = currCmds.Count <= index || !teammateChar.GetTeammateCommandCanUse()[index] || teammateChar.GetExecutingTeammateCommand() >= 0 || currChar == teammateChar;
					if (flag3)
					{
						result = false;
					}
					else
					{
						sbyte cmdType = this.GetMainCharacter(isAlly).GetShowTransferInjuryCommand() ? 13 : teammateChar.GetCurrTeammateCommands()[index];
						TeammateCommandItem commandConfig = TeammateCommand.Instance[cmdType];
						ETeammateCommandImplement commandImplement = commandConfig.Implement;
						bool flag4 = commandImplement == ETeammateCommandImplement.TransferInjury && index != 0;
						if (flag4)
						{
							result = false;
						}
						else
						{
							currChar.SetNeedTeammateCommand(context, charId, index);
							result = true;
						}
					}
				}
			}
			return result;
		}

		// Token: 0x06006532 RID: 25906 RVA: 0x0039B0DC File Offset: 0x003992DC
		[DomainMethod]
		public unsafe CombatCharacterDisplayData GetCombatCharDisplayData(DataContext context, int charId)
		{
			CombatCharacter combatChar;
			bool flag = !this.TryGetElement_CombatCharacterDict(charId, out combatChar);
			CombatCharacterDisplayData result;
			if (flag)
			{
				result = null;
			}
			else
			{
				result = new CombatCharacterDisplayData
				{
					DefeatMarks = combatChar.GetDefeatMarkCollection(),
					OldInjuries = combatChar.GetOldInjuries(),
					OldPoisons = *combatChar.GetOldPoison(),
					OldDisorderOfQi = combatChar.GetOldDisorderOfQi(),
					Happiness = combatChar.GetHappiness()
				};
			}
			return result;
		}

		// Token: 0x06006533 RID: 25907 RVA: 0x0039B14C File Offset: 0x0039934C
		private int GetTeamWisdomCount(bool isAlly)
		{
			return CFormulaHelper.CalcTeamWisdomCount(isAlly ? this._selfTeam : this._enemyTeam);
		}

		// Token: 0x06006534 RID: 25908 RVA: 0x0039B174 File Offset: 0x00399374
		public void GetAllCharInCombat(List<int> charIdList)
		{
			charIdList.Clear();
			charIdList.AddRange(this._combatCharacterDict.Keys);
		}

		// Token: 0x06006535 RID: 25909 RVA: 0x0039B190 File Offset: 0x00399390
		public bool IsTeamCharacter(int charId)
		{
			return this._selfTeam.Exist(charId) || this._enemyTeam.Exist(charId);
		}

		// Token: 0x06006536 RID: 25910 RVA: 0x0039B1C0 File Offset: 0x003993C0
		public bool IsMainCharacter(CombatCharacter character)
		{
			return character.GetId() == (character.IsAlly ? this._selfTeam : this._enemyTeam)[0];
		}

		// Token: 0x06006537 RID: 25911 RVA: 0x0039B1F4 File Offset: 0x003993F4
		public bool AnyTeammateChar(bool isAlly)
		{
			int[] team = isAlly ? this._selfTeam : this._enemyTeam;
			for (int i = 1; i < team.Length; i++)
			{
				bool flag = team[i] >= 0;
				if (flag)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06006538 RID: 25912 RVA: 0x0039B240 File Offset: 0x00399440
		public CombatCharacter GetMainCharacter(bool isAlly)
		{
			return this.GetElement_CombatCharacterDict((isAlly ? this._selfTeam : this._enemyTeam)[0]);
		}

		// Token: 0x06006539 RID: 25913 RVA: 0x0039B26B File Offset: 0x0039946B
		public IEnumerable<int> GetTeamCharacterIds()
		{
			foreach (int teamCharId in this._selfTeam)
			{
				bool flag = teamCharId >= 0;
				if (flag)
				{
					yield return teamCharId;
				}
			}
			int[] array = null;
			foreach (int teamCharId2 in this._enemyTeam)
			{
				bool flag2 = teamCharId2 >= 0;
				if (flag2)
				{
					yield return teamCharId2;
				}
			}
			int[] array2 = null;
			yield break;
		}

		// Token: 0x0600653A RID: 25914 RVA: 0x0039B27C File Offset: 0x0039947C
		public int[] GetCharacterList(bool isAlly)
		{
			return isAlly ? this._selfTeam : this._enemyTeam;
		}

		// Token: 0x0600653B RID: 25915 RVA: 0x0039B29F File Offset: 0x0039949F
		public IEnumerable<CombatCharacter> GetCharacters(bool isAlly)
		{
			int[] team = this.GetCharacterList(isAlly);
			int num;
			for (int i = 0; i < team.Length; i = num + 1)
			{
				bool flag = team[i] < 0;
				if (!flag)
				{
					yield return this._combatCharacterDict[team[i]];
				}
				num = i;
			}
			yield break;
		}

		// Token: 0x0600653C RID: 25916 RVA: 0x0039B2B6 File Offset: 0x003994B6
		public IEnumerable<CombatCharacter> GetTeammateCharacters(int charId)
		{
			bool flag = !this.IsCharInCombat(charId, true);
			if (flag)
			{
				yield break;
			}
			CombatCharacter combatChar = this._combatCharacterDict[charId];
			bool flag2 = !this.IsMainCharacter(combatChar);
			if (flag2)
			{
				yield break;
			}
			int[] team = this.GetCharacterList(combatChar.IsAlly);
			int num;
			for (int i = 1; i < team.Length; i = num + 1)
			{
				bool flag3 = team[i] < 0;
				if (!flag3)
				{
					yield return this._combatCharacterDict[team[i]];
				}
				num = i;
			}
			yield break;
		}

		// Token: 0x0600653D RID: 25917 RVA: 0x0039B2D0 File Offset: 0x003994D0
		public unsafe int GetMaxOriginNeiliAllocationSum(bool isAlly)
		{
			NeiliAllocation maxNeiliAllocation = default(NeiliAllocation);
			maxNeiliAllocation.Initialize();
			foreach (CombatCharacter character in this.GetCharacters(isAlly))
			{
				NeiliAllocation originNeiliAllocation = character.GetOriginNeiliAllocation();
				for (int i = 0; i < 4; i++)
				{
					*maxNeiliAllocation[i] = Math.Max(*maxNeiliAllocation[i], *originNeiliAllocation[i]);
				}
			}
			return maxNeiliAllocation.Sum();
		}

		// Token: 0x0600653E RID: 25918 RVA: 0x0039B37C File Offset: 0x0039957C
		public bool IsCurrentCombatCharacter(CombatCharacter character)
		{
			return this.GetCombatCharacter(character.IsAlly, false) == character;
		}

		// Token: 0x0600653F RID: 25919 RVA: 0x0039B3A0 File Offset: 0x003995A0
		public CombatCharacter GetCombatCharacter(bool isAlly, bool tryGetCoverCharacter = false)
		{
			CombatCharacter combatChar = isAlly ? this._selfChar : this._enemyChar;
			bool flag = tryGetCoverCharacter && combatChar.TeammateBeforeMainChar >= 0;
			if (flag)
			{
				CombatCharacter teammateChar = this._combatCharacterDict[combatChar.TeammateBeforeMainChar];
				bool visible = teammateChar.GetVisible();
				if (visible)
				{
					return teammateChar;
				}
			}
			return combatChar;
		}

		// Token: 0x06006540 RID: 25920 RVA: 0x0039B400 File Offset: 0x00399600
		public void SetCombatCharacter(DataContext context, bool isAlly, int charId)
		{
			CombatCharacter character = this._combatCharacterDict[charId];
			character.SetVisible(true, context);
			bool flag = !this.IsMainCharacter(character);
			if (flag)
			{
				TrickCollection tricks = character.GetTricks();
				tricks.ClearTricks();
				character.SetTricks(tricks, context);
			}
			if (isAlly)
			{
				bool flag2 = !this._selfTeam.Exist(charId);
				if (flag2)
				{
					throw new Exception("Character " + charId.ToString() + " is not in self team");
				}
				this.SetSelfCharId(charId, context);
				this._selfChar = character;
			}
			else
			{
				bool flag3 = !this._enemyTeam.Exist(charId);
				if (flag3)
				{
					throw new Exception("Character " + charId.ToString() + " is not in enemy team");
				}
				this.SetEnemyCharId(charId, context);
				this._enemyChar = character;
			}
			this.UpdateAllCommandAvailability(context, character);
		}

		// Token: 0x06006541 RID: 25921 RVA: 0x0039B4E4 File Offset: 0x003996E4
		public bool IsCharInCombat(int charId, bool checkCombatStatus = true)
		{
			return (!checkCombatStatus || this.IsInCombat()) && this._combatCharacterDict.ContainsKey(charId);
		}

		// Token: 0x06006542 RID: 25922 RVA: 0x0039B510 File Offset: 0x00399710
		public bool IsAlly(int charId1, int charId2)
		{
			return this._combatCharacterDict[charId1].IsAlly == this._combatCharacterDict[charId2].IsAlly;
		}

		// Token: 0x06006543 RID: 25923 RVA: 0x0039B548 File Offset: 0x00399748
		public void UpdateAllTeammateCommandUsable(DataContext context, bool isAlly, sbyte type = -1)
		{
			ETeammateCommandImplement implement = (type < 0) ? ETeammateCommandImplement.Invalid : TeammateCommand.Instance[type].Implement;
			this.UpdateAllTeammateCommandUsable(context, isAlly, implement);
		}

		// Token: 0x06006544 RID: 25924 RVA: 0x0039B578 File Offset: 0x00399778
		public void UpdateAllTeammateCommandUsable(DataContext context, bool isAlly, ETeammateCommandImplement implement)
		{
			int[] team = isAlly ? this._selfTeam : this._enemyTeam;
			for (int i = 1; i < team.Length; i++)
			{
				int charId = team[i];
				bool flag = charId < 0;
				if (!flag)
				{
					this.UpdateTeammateCommandUsable(context, this.GetElement_CombatCharacterDict(charId), implement);
				}
			}
		}

		// Token: 0x06006545 RID: 25925 RVA: 0x0039B5CC File Offset: 0x003997CC
		public void UpdateTeammateCommandUsable(DataContext context, CombatCharacter teammateChar, sbyte type = -1)
		{
			ETeammateCommandImplement implement = (type < 0) ? ETeammateCommandImplement.Invalid : TeammateCommand.Instance[type].Implement;
			this.UpdateTeammateCommandUsable(context, teammateChar, implement);
		}

		// Token: 0x06006546 RID: 25926 RVA: 0x0039B5FC File Offset: 0x003997FC
		public void UpdateTeammateCommandUsable(DataContext context, CombatCharacter teammateChar, ETeammateCommandImplement implement)
		{
			CombatCharacter currChar = this.GetCombatCharacter(teammateChar.IsAlly, false);
			List<sbyte> cmdTypeList = teammateChar.GetCurrTeammateCommands();
			CombatDomain.<>c__DisplayClass715_0 CS$<>8__locals1;
			CS$<>8__locals1.cmdBanReasonList = teammateChar.GetTeammateCommandBanReasons();
			CS$<>8__locals1.checkerContext = new TeammateCommandCheckerContext
			{
				CurrChar = currChar,
				TeammateChar = teammateChar
			};
			CS$<>8__locals1.checkerContext.InitExtraFields();
			CS$<>8__locals1.tempBanReasons = ObjectPool<List<sbyte>>.Instance.Get();
			CS$<>8__locals1.changed = false;
			bool showTransferInjuryCommand = this.GetMainCharacter(teammateChar.IsAlly).GetShowTransferInjuryCommand();
			if (showTransferInjuryCommand)
			{
				for (int index = 0; index < cmdTypeList.Count; index++)
				{
					CombatDomain.<UpdateTeammateCommandUsable>g__UpdateBanReasons|715_0(ETeammateCommandImplement.TransferInjury, index, ref CS$<>8__locals1);
				}
			}
			else
			{
				List<int> indexes = ObjectPool<List<int>>.Instance.Get();
				indexes.Clear();
				for (int index2 = 0; index2 < cmdTypeList.Count; index2++)
				{
					sbyte cmdType = cmdTypeList[index2];
					ETeammateCommandImplement cmdImplement = (cmdType < 0) ? ETeammateCommandImplement.Invalid : TeammateCommand.Instance[cmdType].Implement;
					bool flag = implement == ETeammateCommandImplement.Invalid || cmdImplement == implement;
					bool flag2 = flag;
					if (!flag2)
					{
						bool flag3 = cmdImplement - ETeammateCommandImplement.Push <= 1;
						bool flag4 = flag3;
						bool flag5 = flag4;
						if (flag5)
						{
							bool flag6 = implement - ETeammateCommandImplement.Push <= 1;
							flag5 = flag6;
						}
						flag2 = flag5;
					}
					bool flag7 = flag2;
					if (flag7)
					{
						indexes.Add(index2);
					}
				}
				foreach (int index3 in indexes)
				{
					sbyte cmdType2 = cmdTypeList[index3];
					ETeammateCommandImplement cmdImplement2 = (cmdType2 < 0) ? ETeammateCommandImplement.Invalid : TeammateCommand.Instance[cmdType2].Implement;
					CombatDomain.<UpdateTeammateCommandUsable>g__UpdateBanReasons|715_0(cmdImplement2, index3, ref CS$<>8__locals1);
				}
				ObjectPool<List<int>>.Instance.Return(indexes);
			}
			ObjectPool<List<sbyte>>.Instance.Return(CS$<>8__locals1.tempBanReasons);
			bool changed = CS$<>8__locals1.changed;
			if (changed)
			{
				teammateChar.SetTeammateCommandBanReasons(CS$<>8__locals1.cmdBanReasonList, context);
			}
		}

		// Token: 0x06006547 RID: 25927 RVA: 0x0039B820 File Offset: 0x00399A20
		public void ForceAllTeammateLeaveCombatField(DataContext context, bool isAlly)
		{
			CombatCharacter currChar = this.GetCombatCharacter(isAlly, false);
			bool flag = !this.IsMainCharacter(currChar);
			if (flag)
			{
				currChar.ChangeCharId = this.GetCharacterList(isAlly)[0];
			}
			bool flag2 = currChar.TeammateBeforeMainChar >= 0;
			if (flag2)
			{
				this.GetElement_CombatCharacterDict(currChar.TeammateBeforeMainChar).ClearTeammateCommand(context, true);
			}
			bool flag3 = currChar.TeammateAfterMainChar >= 0;
			if (flag3)
			{
				this.GetElement_CombatCharacterDict(currChar.TeammateAfterMainChar).ClearTeammateCommand(context, true);
			}
		}

		// Token: 0x06006548 RID: 25928 RVA: 0x0039B89C File Offset: 0x00399A9C
		public void TryUpdatePreRandomizedTeammateCommands(DataContext context, int teammateId)
		{
			TeammateCommandChangeData preRandomizedTeammateCommandReplaceData = this.PreRandomizedTeammateCommandReplaceData;
			IReadOnlyList<sbyte> teammateCmdTypes = (preRandomizedTeammateCommandReplaceData != null) ? preRandomizedTeammateCommandReplaceData.GetCharTeammateCommands(teammateId) : null;
			bool flag = teammateCmdTypes == null;
			if (!flag)
			{
				List<sbyte> newCmdTypes = ObjectPool<List<sbyte>>.Instance.Get();
				newCmdTypes.Clear();
				newCmdTypes.AddRange(DomainManager.Extra.GetCharUsableTeammateCommands(context, teammateId));
				bool flag2 = !newCmdTypes.SequenceEqual(teammateCmdTypes);
				if (flag2)
				{
					this.PreRandomizedTeammateCommandReplaceData.SetCharTeammateCommands(teammateId, newCmdTypes);
					GameDataBridge.AddDisplayEvent<int, List<sbyte>>(DisplayEventType.ChangeTeammateCommandOnCombatBegin, teammateId, newCmdTypes);
				}
				ObjectPool<List<sbyte>>.Instance.Return(newCmdTypes);
			}
		}

		// Token: 0x06006549 RID: 25929 RVA: 0x0039B920 File Offset: 0x00399B20
		private static TeammateCommandChangeDataPart CalcTeammateBetrayData(DataContext context, short combatConfigId, IReadOnlyList<int> charIds, bool isAlly)
		{
			CombatConfigItem config = Config.CombatConfig.Instance[combatConfigId];
			TeammateCommandChangeDataPart data = new TeammateCommandChangeDataPart();
			int mainCharId = charIds[0];
			for (int i = 1; i < charIds.Count; i++)
			{
				int charId = charIds[i];
				bool flag = charId < 0;
				if (!flag)
				{
					data.TeammateCharIds.Add(charId);
					IEnumerable<sbyte> originCmds = DomainManager.Extra.GetCharUsableTeammateCommands(context, charId);
					SByteList oldCmdsSbyteList = new SByteList(originCmds);
					List<sbyte> oldCmds = oldCmdsSbyteList.Items;
					data.OriginTeammateCommands.Add(oldCmdsSbyteList);
					SByteList newCmdsSbyteList = SByteList.Create();
					List<sbyte> newCmds = newCmdsSbyteList.Items;
					bool flag2;
					if (config.SpecialTeammateCommands.Count > i - 1 && !isAlly)
					{
						List<sbyte> list = config.SpecialTeammateCommands[i - 1];
						flag2 = (list != null && list.Count > 0);
					}
					else
					{
						flag2 = false;
					}
					bool flag3 = flag2;
					if (flag3)
					{
						oldCmds.Clear();
						newCmds.AddRange(config.SpecialTeammateCommands[i - 1]);
					}
					else
					{
						newCmds.AddRange(oldCmds);
						short favor = DomainManager.Character.GetFavorability(charId, mainCharId);
						int count = CombatDomain.CalcNegativeTeammateCommandCount(favor, oldCmds);
						DomainManager.Extra.FillNegativeTeammateCommands(context.Random, charId, count, newCmds);
					}
					data.ReplaceTeammateCommands.Add(newCmdsSbyteList);
				}
			}
			return data;
		}

		// Token: 0x0600654A RID: 25930 RVA: 0x0039BA80 File Offset: 0x00399C80
		private static int CalcNegativeTeammateCommandCount(short favor, IReadOnlyList<sbyte> cmdTypes)
		{
			sbyte favorType = FavorabilityType.GetFavorabilityType(favor);
			if (!true)
			{
			}
			int num;
			if (favorType <= -3)
			{
				if (favorType > -5)
				{
					num = 2;
				}
				else
				{
					num = 3;
				}
			}
			else if (favorType > -1)
			{
				num = 0;
			}
			else
			{
				num = 1;
			}
			if (!true)
			{
			}
			int count = num;
			int advanceCount = cmdTypes.Count((sbyte x) => TeammateCommand.Instance[x].Type == ETeammateCommandType.Advance);
			return Math.Min(count, 3 - advanceCount);
		}

		// Token: 0x0600654B RID: 25931 RVA: 0x0039BAF8 File Offset: 0x00399CF8
		public IReadOnlyList<sbyte> GetPreRandomizedTeammateCommands(DataContext context, int teammateId)
		{
			TeammateCommandChangeData preRandomizedTeammateCommandReplaceData = this.PreRandomizedTeammateCommandReplaceData;
			IReadOnlyList<sbyte> cmdList = (preRandomizedTeammateCommandReplaceData != null) ? preRandomizedTeammateCommandReplaceData.GetCharTeammateCommands(teammateId) : null;
			return cmdList ?? DomainManager.Extra.GetCharOriginalTeammateCommands(context, teammateId);
		}

		// Token: 0x0600654C RID: 25932 RVA: 0x0039BB30 File Offset: 0x00399D30
		private void ProcessVitalDemonBetray(DataContext context, TeammateCommandChangeDataPart enemyTeam)
		{
			bool vitalIsDemon = DomainManager.Extra.AreVitalsDemon();
			List<SectStoryThreeVitalsCharacterType> betrayVitalDemons = null;
			foreach (SectStoryThreeVitalsCharacterType type in Enum.GetValues<SectStoryThreeVitalsCharacterType>())
			{
				SectStoryThreeVitalsCharacter vital = DomainManager.Extra.GetVitalByType(type);
				bool flag = vital == null;
				if (!flag)
				{
					int odds = vital.CalcBetrayOdds(vitalIsDemon);
					bool flag2 = !context.Random.CheckPercentProb(odds);
					if (!flag2)
					{
						if (betrayVitalDemons == null)
						{
							betrayVitalDemons = new List<SectStoryThreeVitalsCharacterType>();
						}
						betrayVitalDemons.Add(type);
					}
				}
			}
			bool flag3 = betrayVitalDemons == null;
			if (!flag3)
			{
				int emptyTeammateCount = 3 - enemyTeam.TeammateCharIds.Count;
				emptyTeammateCount = Math.Min(emptyTeammateCount, betrayVitalDemons.Count);
				betrayVitalDemons.Sort();
				bool flag4 = emptyTeammateCount < betrayVitalDemons.Count;
				if (flag4)
				{
					betrayVitalDemons.MoveLastToFirst(emptyTeammateCount);
				}
				int fillingIndex = 0;
				bool betrayVitalIsDemon = !vitalIsDemon;
				for (int i = 0; i < emptyTeammateCount; i++)
				{
					SectStoryThreeVitalsCharacterType vitalType = betrayVitalDemons[fillingIndex++];
					short vitalTemplateId = vitalType.GetVitalTemplateId(betrayVitalIsDemon);
					GameData.Domains.Character.Character vital2 = DomainManager.Character.GetOrCreateFixedCharacterByTemplateId(context, vitalTemplateId);
					int vitalCharId = vital2.GetId();
					SByteList vitalTeammateCommands = DomainManager.Extra.GetCharTeammateCommandsSByteList(context, vitalCharId);
					enemyTeam.TeammateCharIds.Add(vitalCharId);
					enemyTeam.OriginTeammateCommands.Add(vitalTeammateCommands);
					enemyTeam.ReplaceTeammateCommands.Add(vitalTeammateCommands);
				}
				int replaceTeammateCount = Math.Min(3, betrayVitalDemons.Count) - emptyTeammateCount;
				for (int j = 0; j < replaceTeammateCount; j++)
				{
					SectStoryThreeVitalsCharacterType vitalType2 = betrayVitalDemons[fillingIndex++];
					short vitalTemplateId2 = vitalType2.GetVitalTemplateId(betrayVitalIsDemon);
					GameData.Domains.Character.Character vital3 = DomainManager.Character.GetOrCreateFixedCharacterByTemplateId(context, vitalTemplateId2);
					int vitalCharId2 = vital3.GetId();
					SByteList vitalTeammateCommands2 = DomainManager.Extra.GetCharTeammateCommandsSByteList(context, vitalCharId2);
					enemyTeam.BetrayedCharIds[j] = enemyTeam.TeammateCharIds[j];
					enemyTeam.TeammateCharIds[j] = vitalCharId2;
					enemyTeam.OriginTeammateCommands[j] = (enemyTeam.ReplaceTeammateCommands[j] = vitalTeammateCommands2);
				}
			}
		}

		// Token: 0x0600654D RID: 25933 RVA: 0x0039BD4C File Offset: 0x00399F4C
		public void JoinSpecialGroup(DataContext context, int charId)
		{
			bool flag = this._taiwuSpecialGroupCharIds.Contains(charId);
			if (!flag)
			{
				this._taiwuSpecialGroupCharIds.Add(charId);
				this.SetTaiwuSpecialGroupCharIds(this._taiwuSpecialGroupCharIds, context);
				this.SpecialGroupInvalidateAllCaches(context, charId);
			}
		}

		// Token: 0x0600654E RID: 25934 RVA: 0x0039BD90 File Offset: 0x00399F90
		public void ExitSpecialGroup(DataContext context, int charId)
		{
			bool flag = !this._taiwuSpecialGroupCharIds.Remove(charId);
			if (!flag)
			{
				this.SetTaiwuSpecialGroupCharIds(this._taiwuSpecialGroupCharIds, context);
				this.SpecialGroupInvalidateAllCaches(context, charId);
			}
		}

		// Token: 0x0600654F RID: 25935 RVA: 0x0039BDCC File Offset: 0x00399FCC
		public void ClearSpecialGroup(DataContext context)
		{
			List<int> taiwuSpecialGroupCharIds = this._taiwuSpecialGroupCharIds;
			bool flag = taiwuSpecialGroupCharIds == null || taiwuSpecialGroupCharIds.Count <= 0;
			if (!flag)
			{
				List<int> specialCharIds = ObjectPool<List<int>>.Instance.Get();
				specialCharIds.AddRange(this._taiwuSpecialGroupCharIds);
				this._taiwuSpecialGroupCharIds.Clear();
				this.SetTaiwuSpecialGroupCharIds(this._taiwuSpecialGroupCharIds, context);
				foreach (int charId in specialCharIds)
				{
					this.SpecialGroupInvalidateAllCaches(context, charId);
				}
				ObjectPool<List<int>>.Instance.Return(specialCharIds);
			}
		}

		// Token: 0x06006550 RID: 25936 RVA: 0x0039BE80 File Offset: 0x0039A080
		private void SpecialGroupInvalidateAllCaches(DataContext context, int charId)
		{
			GameData.Domains.Character.Character character;
			bool flag = !DomainManager.Character.TryGetElement_Objects(charId, out character);
			if (!flag)
			{
				character.InvalidateSelfAndInfluencedCache(112, context);
			}
		}

		// Token: 0x06006551 RID: 25937 RVA: 0x0039BEB0 File Offset: 0x0039A0B0
		public void ForbidNormalAttackInTutorial(int charId)
		{
			bool flag = !this._isTutorialCombat;
			if (!flag)
			{
				CombatCharacter combatChar = this._combatCharacterDict[charId];
				combatChar.ForbidNormalAttackEffectCount = int.MaxValue;
			}
		}

		// Token: 0x06006552 RID: 25938 RVA: 0x0039BEE8 File Offset: 0x0039A0E8
		public void PermitNormalAttackInTutorial(int charId)
		{
			bool flag = !this._isTutorialCombat;
			if (!flag)
			{
				CombatCharacter combatChar = this._combatCharacterDict[charId];
				combatChar.ForbidNormalAttackEffectCount = 0;
			}
		}

		// Token: 0x06006553 RID: 25939 RVA: 0x0039BF1C File Offset: 0x0039A11C
		public void ClearMobilityAndForbidRecover(DataContext context, int charId)
		{
			CombatCharacter combatChar = this._combatCharacterDict[charId];
			combatChar.CanRecoverMobility = false;
			this.ChangeMobilityValue(context, combatChar, -MoveSpecialConstants.MaxMobility, false, null, false);
		}

		// Token: 0x06006554 RID: 25940 RVA: 0x0039BF50 File Offset: 0x0039A150
		public void SetAttackForceMiss(int charId, sbyte count)
		{
			this._combatCharacterDict[charId].AttackForceMissCount = count;
		}

		// Token: 0x06006555 RID: 25941 RVA: 0x0039BF65 File Offset: 0x0039A165
		public void SetAttackForceHit(int charId, sbyte count)
		{
			this._combatCharacterDict[charId].AttackForceHitCount = count;
		}

		// Token: 0x06006556 RID: 25942 RVA: 0x0039BF7A File Offset: 0x0039A17A
		public void SetSkillAttackForceHit(int charId, bool forceHit)
		{
			this._combatCharacterDict[charId].SkillForceHit = forceHit;
		}

		// Token: 0x06006557 RID: 25943 RVA: 0x0039BF90 File Offset: 0x0039A190
		public void SetSkillToMaxRange(DataContext context, int charId, short skillId)
		{
			CombatSkillKey skillKey = new CombatSkillKey(charId, skillId);
			SpecialEffectBase effect = new MaxAttackRange(skillKey);
			DomainManager.SpecialEffect.Add(context, effect);
		}

		// Token: 0x06006558 RID: 25944 RVA: 0x0039BFBC File Offset: 0x0039A1BC
		public void AddSkillPower(DataContext context, int charId, short skillId, int power)
		{
			SkillEffectKey effectKey = new SkillEffectKey(-1, false);
			this.AddSkillPowerInCombat(context, new CombatSkillKey(charId, skillId), effectKey, power);
		}

		// Token: 0x06006559 RID: 25945 RVA: 0x0039BFE8 File Offset: 0x0039A1E8
		public void AddInjury(DataContext context, int charId, sbyte bodyPart, bool isInner, sbyte count)
		{
			CombatCharacter combatChar = this._combatCharacterDict[charId];
			this.AddInjury(context, combatChar, bodyPart, isInner, count, true, false);
			this.AddToCheckFallenSet(combatChar.GetId());
		}

		// Token: 0x0600655A RID: 25946 RVA: 0x0039C020 File Offset: 0x0039A220
		public void SetDefeatMarkImmunity(int charId, bool outerInjuryImmunity, bool innerInjuryImmunity, bool mindImmunity, bool flawImmunity, bool acupointImmunity)
		{
			CombatCharacter combatChar = this._combatCharacterDict[charId];
			combatChar.OuterInjuryImmunity = outerInjuryImmunity;
			combatChar.InnerInjuryImmunity = innerInjuryImmunity;
			combatChar.MindImmunity = mindImmunity;
			combatChar.FlawImmunity = flawImmunity;
			combatChar.AcupointImmunity = acupointImmunity;
		}

		// Token: 0x0600655B RID: 25947 RVA: 0x0039C064 File Offset: 0x0039A264
		public void AppendEquipAttackSkill(DataContext context, int charId, short skillId)
		{
			GameData.Domains.CombatSkill.CombatSkill combatSkill;
			bool flag = !DomainManager.CombatSkill.TryGetElement_CombatSkills(new CombatSkillKey(charId, skillId), out combatSkill);
			if (flag)
			{
				DomainManager.Character.LearnCombatSkill(context, charId, skillId, 0);
			}
			CombatCharacter combatChar = this._combatCharacterDict[charId];
			List<short> attackSkillList = combatChar.GetAttackSkillList();
			attackSkillList.Add(skillId);
			combatChar.SetAttackSkillList(attackSkillList, context);
			this.AddCombatSkillData(context, charId, skillId);
			this.UpdateSkillCanUse(context, combatChar, skillId);
		}

		// Token: 0x0600655C RID: 25948 RVA: 0x0039C0D4 File Offset: 0x0039A2D4
		public void ForceDefeat(int charId)
		{
			this._combatCharacterDict[charId].ForceDefeat = true;
			DomainManager.Combat.AddToCheckFallenSet(charId);
		}

		// Token: 0x0600655D RID: 25949 RVA: 0x0039C0F8 File Offset: 0x0039A2F8
		[DomainMethod]
		public void SelectMercyOption(DataContext context, bool isAlly, bool mercy)
		{
			bool flag = this._combatStatus != CombatStatusType.InCombat;
			if (!flag)
			{
				EShowMercySelect selected = mercy ? EShowMercySelect.Cancel : EShowMercySelect.Confirm;
				this.SetSelectedMercyOption(context, selected);
			}
		}

		// Token: 0x0600655E RID: 25950 RVA: 0x0039C130 File Offset: 0x0039A330
		[DomainMethod]
		public void Surrender(DataContext context, bool isAlly = true)
		{
			bool flag = !this.CanAcceptCommand();
			if (!flag)
			{
				CombatCharacter combatChar = this.GetMainCharacter(isAlly);
				combatChar.ForceDefeat = true;
				this.AddToCheckFallenSet(combatChar.GetId());
				this.UpdateCanSurrender(context, combatChar);
				bool flag2 = !this.CheckEvaluation(45);
				if (!flag2)
				{
					this.AppendEvaluation(45);
					byte addInjuryCount = GlobalConfig.SurrenderInjuryCount[(int)this._combatType];
					Injuries injuries = combatChar.GetInjuries();
					Injuries oldInjuries = combatChar.GetOldInjuries();
					DefeatMarkCollection marks = combatChar.GetDefeatMarkCollection();
					List<int> showData = ObjectPool<List<int>>.Instance.Get();
					showData.Clear();
					for (int i = 0; i < (int)addInjuryCount; i++)
					{
						bool inner = context.Random.NextBool();
						sbyte bodyPart = (sbyte)context.Random.Next(7);
						bool flag3 = injuries.Get(bodyPart, inner) < 6;
						if (flag3)
						{
							injuries.Change(bodyPart, inner, 1);
							oldInjuries.Change(bodyPart, inner, 1);
							showData.Add(new DefeatMarkKey(inner ? EMarkType.Inner : EMarkType.Outer, (int)bodyPart, 1));
						}
						else
						{
							marks.FatalDamageMarkCount = CMath.ClampFatalMarkCount(marks.FatalDamageMarkCount + 1);
							showData.Add(new DefeatMarkKey(EMarkType.Fatal, 0, 0));
						}
					}
					combatChar.SetInjuries(injuries, context);
					combatChar.SetOldInjuries(injuries, context);
					this.UpdateBodyDefeatMark(context, combatChar);
					combatChar.SetDefeatMarkCollection(marks, context);
					GameDataBridge.AddDisplayEvent<List<int>>(DisplayEventType.CombatShowSurrenderMark, showData);
					ObjectPool<List<int>>.Instance.Return(showData);
				}
			}
		}

		// Token: 0x0600655F RID: 25951 RVA: 0x0039C2C0 File Offset: 0x0039A4C0
		[DomainMethod]
		public bool IsInCombat()
		{
			return this._combatStatus == CombatStatusType.InCombat;
		}

		// Token: 0x06006560 RID: 25952 RVA: 0x0039C2E0 File Offset: 0x0039A4E0
		public bool CombatAboutToOver()
		{
			return this._showMercyOption >= 0 || this._selfChar.NeedDelaySettlement || this._enemyChar.NeedDelaySettlement || this._selfChar.StateMachine.GetCurrentStateType() == CombatCharacterStateType.DelaySettlement || this._enemyChar.StateMachine.GetCurrentStateType() == CombatCharacterStateType.DelaySettlement;
		}

		// Token: 0x06006561 RID: 25953 RVA: 0x0039C340 File Offset: 0x0039A540
		public bool IsWin(bool isAlly)
		{
			return isAlly ? (this._combatStatus == CombatStatusType.EnemyFail || this._combatStatus == CombatStatusType.EnemyFlee) : (this._combatStatus == CombatStatusType.SelfFail || this._combatStatus == CombatStatusType.SelfFlee);
		}

		// Token: 0x06006562 RID: 25954 RVA: 0x0039C394 File Offset: 0x0039A594
		private void UpdateCanSurrender(DataContext context, CombatCharacter character)
		{
			bool canSurrender = this.IsMainCharacter(character) && !character.ForceDefeat && !this._isTutorialCombat;
			bool flag = character.GetCanSurrender() != canSurrender;
			if (flag)
			{
				character.SetCanSurrender(canSurrender, context);
			}
			this.UpdateOtherActionCanUse(context, character, 4);
		}

		// Token: 0x06006563 RID: 25955 RVA: 0x0039C3E4 File Offset: 0x0039A5E4
		public void ShowMercyOption(DataContext context, CombatCharacter winChar)
		{
			CombatCharacter failChar = this.GetCombatCharacter(!winChar.IsAlly, false);
			failChar.SetAnimationToPlayOnce("C_011_stun", context);
			winChar.NeedSelectMercyOption = true;
		}

		// Token: 0x06006564 RID: 25956 RVA: 0x0039C417 File Offset: 0x0039A617
		public void SetShowMercyOption(DataContext context, EShowMercyOption option)
		{
			this.SetShowMercyOption((sbyte)option, context);
		}

		// Token: 0x06006565 RID: 25957 RVA: 0x0039C423 File Offset: 0x0039A623
		public void SetSelectedMercyOption(DataContext context, EShowMercySelect selected)
		{
			this.SetSelectedMercyOption((sbyte)selected, context);
		}

		// Token: 0x06006566 RID: 25958 RVA: 0x0039C430 File Offset: 0x0039A630
		public void UpdateBodyDefeatMark(DataContext context, CombatCharacter character)
		{
			DefeatMarkCollection markCollection = character.GetDefeatMarkCollection();
			Injuries injuries = character.GetInjuries();
			SortedDictionary<sbyte, List<ValueTuple<sbyte, int, int>>> flawDict = character.GetFlawCollection().BodyPartDict;
			SortedDictionary<sbyte, List<ValueTuple<sbyte, int, int>>> acupointDict = character.GetAcupointCollection().BodyPartDict;
			List<byte> flawOrAcupointList = ObjectPool<List<byte>>.Instance.Get();
			bool markChanged = false;
			for (sbyte bodyPart = 0; bodyPart < 7; bodyPart += 1)
			{
				ValueTuple<sbyte, sbyte> injury = injuries.Get(bodyPart);
				bool flag = markCollection.OuterInjuryMarkList[(int)bodyPart] != (byte)injury.Item1;
				if (flag)
				{
					markCollection.OuterInjuryMarkList[(int)bodyPart] = (byte)injury.Item1;
					markChanged = true;
				}
				bool flag2 = markCollection.InnerInjuryMarkList[(int)bodyPart] != (byte)injury.Item2;
				if (flag2)
				{
					markCollection.InnerInjuryMarkList[(int)bodyPart] = (byte)injury.Item2;
					markChanged = true;
				}
				flawOrAcupointList.Clear();
				for (int i = 0; i < flawDict[bodyPart].Count; i++)
				{
					flawOrAcupointList.Add((byte)flawDict[bodyPart][i].Item1);
				}
				bool flag3 = !markCollection.FlawMarkList[(int)bodyPart].SequenceEqual(flawOrAcupointList);
				if (flag3)
				{
					markCollection.FlawMarkList[(int)bodyPart].Clear();
					markCollection.FlawMarkList[(int)bodyPart].AddRange(flawOrAcupointList);
					markChanged = true;
				}
				flawOrAcupointList.Clear();
				for (int j = 0; j < acupointDict[bodyPart].Count; j++)
				{
					flawOrAcupointList.Add((byte)acupointDict[bodyPart][j].Item1);
				}
				bool flag4 = !markCollection.AcupointMarkList[(int)bodyPart].SequenceEqual(flawOrAcupointList);
				if (flag4)
				{
					markCollection.AcupointMarkList[(int)bodyPart].Clear();
					markCollection.AcupointMarkList[(int)bodyPart].AddRange(flawOrAcupointList);
					markChanged = true;
				}
			}
			ObjectPool<List<byte>>.Instance.Return(flawOrAcupointList);
			bool flag5 = markChanged;
			if (flag5)
			{
				character.SetDefeatMarkCollection(markCollection, context);
				this.AddToCheckFallenSet(character.GetId());
				bool flag6 = this.IsMainCharacter(character);
				if (flag6)
				{
					this.UpdateSkillNeedBodyPartCanUse(context, character);
				}
			}
		}

		// Token: 0x06006567 RID: 25959 RVA: 0x0039C64C File Offset: 0x0039A84C
		public void UpdateBodyDefeatMark(DataContext context, CombatCharacter character, sbyte bodyPart)
		{
			DefeatMarkCollection markCollection = character.GetDefeatMarkCollection();
			Injuries injuries = character.GetInjuries();
			bool markChanged = false;
			bool flag = bodyPart != -1;
			if (flag)
			{
				ValueTuple<sbyte, sbyte> injury = injuries.Get(bodyPart);
				List<ValueTuple<sbyte, int, int>> flawList = character.GetFlawCollection().BodyPartDict[bodyPart];
				List<ValueTuple<sbyte, int, int>> acupointList = character.GetAcupointCollection().BodyPartDict[bodyPart];
				List<byte> flawOrAcupointList = ObjectPool<List<byte>>.Instance.Get();
				bool flag2 = markCollection.OuterInjuryMarkList[(int)bodyPart] != (byte)injury.Item1;
				if (flag2)
				{
					markCollection.OuterInjuryMarkList[(int)bodyPart] = (byte)injury.Item1;
					markChanged = true;
				}
				bool flag3 = markCollection.InnerInjuryMarkList[(int)bodyPart] != (byte)injury.Item2;
				if (flag3)
				{
					markCollection.InnerInjuryMarkList[(int)bodyPart] = (byte)injury.Item2;
					markChanged = true;
				}
				flawOrAcupointList.Clear();
				for (int i = 0; i < flawList.Count; i++)
				{
					flawOrAcupointList.Add((byte)flawList[i].Item1);
				}
				bool flag4 = !markCollection.FlawMarkList[(int)bodyPart].SequenceEqual(flawOrAcupointList);
				if (flag4)
				{
					markCollection.FlawMarkList[(int)bodyPart].Clear();
					markCollection.FlawMarkList[(int)bodyPart].AddRange(flawOrAcupointList);
					markChanged = true;
				}
				flawOrAcupointList.Clear();
				for (int j = 0; j < acupointList.Count; j++)
				{
					flawOrAcupointList.Add((byte)acupointList[j].Item1);
				}
				bool flag5 = !markCollection.AcupointMarkList[(int)bodyPart].SequenceEqual(flawOrAcupointList);
				if (flag5)
				{
					markCollection.AcupointMarkList[(int)bodyPart].Clear();
					markCollection.AcupointMarkList[(int)bodyPart].AddRange(flawOrAcupointList);
					markChanged = true;
				}
				ObjectPool<List<byte>>.Instance.Return(flawOrAcupointList);
			}
			bool flag6 = markChanged;
			if (flag6)
			{
				character.SetDefeatMarkCollection(markCollection, context);
				bool flag7 = this.IsMainCharacter(character);
				if (flag7)
				{
					this.UpdateSkillNeedBodyPartCanUse(context, character);
				}
				bool flag8 = bodyPart == 5 || bodyPart == 6;
				if (flag8)
				{
					this.ChangeMobilityValue(context, character, 0, false, null, false);
				}
				this.AddToCheckFallenSet(character.GetId());
			}
		}

		// Token: 0x06006568 RID: 25960 RVA: 0x0039C85C File Offset: 0x0039AA5C
		public unsafe void UpdatePoisonDefeatMark(DataContext context, CombatCharacter character)
		{
			DefeatMarkCollection markCollection = character.GetDefeatMarkCollection();
			PoisonInts poison = *character.GetPoison();
			bool markChanged = false;
			for (sbyte poisonType = 0; poisonType < 6; poisonType += 1)
			{
				sbyte markCount = character.GetCharacter().HasPoisonImmunity(poisonType) ? 0 : PoisonsAndLevels.CalcPoisonedLevel(*(ref poison.Items.FixedElementField + (IntPtr)poisonType * 4));
				bool flag = markCount != (sbyte)markCollection.PoisonMarkList[(int)poisonType];
				if (flag)
				{
					markCollection.PoisonMarkList[(int)poisonType] = (byte)markCount;
					markChanged = true;
				}
			}
			bool flag2 = markChanged;
			if (flag2)
			{
				this.AddToCheckFallenSet(character.GetId());
				character.SetDefeatMarkCollection(markCollection, context);
			}
		}

		// Token: 0x06006569 RID: 25961 RVA: 0x0039C904 File Offset: 0x0039AB04
		public unsafe int UpdatePoisonDefeatMark(DataContext context, CombatCharacter character, sbyte poisonType)
		{
			DefeatMarkCollection markCollection = character.GetDefeatMarkCollection();
			int poison = *(ref character.GetPoison().Items.FixedElementField + (IntPtr)poisonType * 4);
			sbyte markCount = character.GetCharacter().HasPoisonImmunity(poisonType) ? 0 : PoisonsAndLevels.CalcPoisonedLevel(poison);
			byte oldMarkCount = markCollection.PoisonMarkList[(int)poisonType];
			bool flag = markCount != (sbyte)oldMarkCount;
			if (flag)
			{
				markCollection.PoisonMarkList[(int)poisonType] = (byte)markCount;
				character.SetDefeatMarkCollection(markCollection, context);
				this.AddToCheckFallenSet(character.GetId());
			}
			return (int)(markCount - (sbyte)oldMarkCount);
		}

		// Token: 0x0600656A RID: 25962 RVA: 0x0039C989 File Offset: 0x0039AB89
		public void UpdateOtherDefeatMark(DataContext context, CombatCharacter character)
		{
			character.GetDefeatMarkCollection().SyncOtherMark(context, character);
		}

		// Token: 0x0600656B RID: 25963 RVA: 0x0039C99C File Offset: 0x0039AB9C
		public void AddMindDamage(CombatContext context, int damageValue)
		{
			bool flag = damageValue <= 0;
			if (!flag)
			{
				int step = context.Defender.GetDamageStepCollection().MindDamageStep;
				ValueTuple<int, int> valueTuple = CombatDomain.CalcInjuryMarkCount(damageValue + context.Defender.GetMindDamageValue(), step, -1);
				int markCount = valueTuple.Item1;
				int leftDamage = valueTuple.Item2;
				context.Defender.SetMindDamageValue(leftDamage, context);
				context.Defender.AddMindDamageToShow(context, damageValue);
				this.AppendMindDefeatMark(context, context.Defender, markCount, context.SkillTemplateId, false);
			}
		}

		// Token: 0x0600656C RID: 25964 RVA: 0x0039CA30 File Offset: 0x0039AC30
		public void AppendMindDefeatMark(DataContext context, CombatCharacter character, int count, short skillId = -1, bool infinity = false)
		{
			bool mindImmunity = character.GetMindImmunity();
			if (mindImmunity)
			{
				this.ShowImmunityEffectTips(context, character.GetId(), EMarkType.Mind);
			}
			else
			{
				count = DomainManager.SpecialEffect.ModifyData(character.GetId(), -1, 249, count, -1, -1, -1);
				bool flag = count <= 0;
				if (!flag)
				{
					bool changeToFatal = DomainManager.SpecialEffect.ModifyData(character.GetId(), skillId, 289, false, -1, -1, -1);
					bool flag2 = !character.ChangeToMindMark && changeToFatal;
					if (flag2)
					{
						this.AppendFatalDamageMark(context, character, count, -1, -1, false, EDamageType.None);
					}
					else
					{
						MindMarkList mindMarkList = character.GetMindMarkTime();
						int keepTime = infinity ? -1 : DomainManager.SpecialEffect.ModifyValue(character.GetId(), 178, (int)GlobalConfig.Instance.MindMarkBaseKeepTime, -1, -1, -1, 0, 0, 0, 0);
						for (int i = 0; i < count; i++)
						{
							MindMarkList mindMarkList2 = mindMarkList;
							if (mindMarkList2.MarkList == null)
							{
								mindMarkList2.MarkList = new List<SilenceFrameData>();
							}
							mindMarkList.MarkList.Add(SilenceFrameData.Create(keepTime));
						}
						character.GetDefeatMarkCollection().SyncMindMark(context, character);
						character.SetMindMarkTime(mindMarkList, context);
						Events.RaiseAddMindMark(context, character, count);
					}
				}
			}
		}

		// Token: 0x0600656D RID: 25965 RVA: 0x0039CB64 File Offset: 0x0039AD64
		public void RemoveHalfMindDefeatMark(DataContext context, CombatCharacter character)
		{
			int removeCount = character.GetDefeatMarkCollection().MindMarkList.Count * CValueHalf.RoundUp;
			DomainManager.Combat.RemoveMindDefeatMark(context, character, removeCount, true, 0);
		}

		// Token: 0x0600656E RID: 25966 RVA: 0x0039CBA0 File Offset: 0x0039ADA0
		public void RemoveMindDefeatMark(DataContext context, CombatCharacter character, int count, bool random, int index = 0)
		{
			MindMarkList mindMarkList = character.GetMindMarkTime();
			int removeCount = Math.Min(count, mindMarkList.MarkList.Count);
			for (int i = 0; i < removeCount; i++)
			{
				mindMarkList.MarkList.RemoveAt(random ? context.Random.Next(0, mindMarkList.MarkList.Count) : index);
			}
			character.GetDefeatMarkCollection().SyncMindMark(context, character);
			character.SetMindMarkTime(mindMarkList, context);
		}

		// Token: 0x0600656F RID: 25967 RVA: 0x0039CC1C File Offset: 0x0039AE1C
		public void TransferRandomMindDefeatMark(DataContext context, CombatCharacter srcChar, CombatCharacter dstChar)
		{
			List<bool> markList = srcChar.GetDefeatMarkCollection().MindMarkList;
			this.TransferMindDefeatMark(context, srcChar, dstChar, context.Random.Next(markList.Count));
		}

		// Token: 0x06006570 RID: 25968 RVA: 0x0039CC54 File Offset: 0x0039AE54
		public void TransferMindDefeatMark(DataContext context, CombatCharacter srcChar, CombatCharacter dstChar, int index)
		{
			MindMarkList srcList = srcChar.GetMindMarkTime();
			MindMarkList dstList = dstChar.GetMindMarkTime();
			SilenceFrameData mindMark = srcList.MarkList[index];
			short keepTime = GlobalConfig.Instance.MindMarkBaseKeepTime;
			srcList.MarkList.RemoveAt(index);
			MindMarkList mindMarkList = dstList;
			if (mindMarkList.MarkList == null)
			{
				mindMarkList.MarkList = new List<SilenceFrameData>();
			}
			dstList.MarkList.Add(mindMark.Infinity ? SilenceFrameData.Create((int)keepTime) : mindMark);
			srcChar.SetMindMarkTime(srcList, context);
			dstChar.SetMindMarkTime(dstList, context);
			srcChar.GetDefeatMarkCollection().SyncMindMark(context, srcChar);
			dstChar.GetDefeatMarkCollection().SyncMindMark(context, dstChar);
		}

		// Token: 0x06006571 RID: 25969 RVA: 0x0039CCFC File Offset: 0x0039AEFC
		public void AppendDieDefeatMark(DataContext context, CombatCharacter character, CombatSkillKey skillKey, int count)
		{
			bool dieImmunity = character.GetCharacter().GetDieImmunity();
			if (dieImmunity)
			{
				this.ShowImmunityEffectTips(context, character.GetId(), EMarkType.Die);
			}
			else
			{
				bool changeToMindMark = character.ChangeToMindMark;
				if (changeToMindMark)
				{
					this.AppendMindDefeatMark(context, character, count, -1, false);
				}
				else
				{
					DefeatMarkCollection markCollection = character.GetDefeatMarkCollection();
					for (int i = 0; i < count; i++)
					{
						markCollection.DieMarkList.Add(skillKey);
					}
					character.SetDefeatMarkCollection(markCollection, context);
					this.AddToCheckFallenSet(character.GetId());
				}
			}
		}

		// Token: 0x06006572 RID: 25970 RVA: 0x0039CD84 File Offset: 0x0039AF84
		public int AddFatalDamageValue(DataContext context, CombatCharacter combatChar, int damageValue, int type = -1, sbyte bodyPart = -1, short skillId = -1, EDamageType damageType = EDamageType.None)
		{
			damageValue = DomainManager.SpecialEffect.ModifyValueCustom(combatChar.GetId(), skillId, 191, damageValue, type, (int)damageType, -1, 0, 0, 0, 0);
			damageValue = DomainManager.SpecialEffect.ModifyData(combatChar.GetId(), skillId, 294, damageValue, -1, -1, -1);
			bool flag = damageValue <= 0;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				int originDamageValue = combatChar.GetFatalDamageValue();
				int damageStep = combatChar.GetDamageStepCollection().FatalDamageStep;
				ValueTuple<int, int> damageResult = CombatDomain.CalcInjuryMarkCount(damageValue + originDamageValue, damageStep, -1);
				combatChar.SetFatalDamageValue(damageResult.Item2, context);
				int damageValueToShow = damageResult.Item1 * damageStep + damageResult.Item2 - originDamageValue;
				bool flag2 = damageValueToShow > 0;
				if (flag2)
				{
					combatChar.AddFatalDamageToShow(context, damageValueToShow);
				}
				bool flag3 = damageResult.Item1 > 0;
				if (flag3)
				{
					result = this.AppendFatalDamageMark(context, combatChar, damageResult.Item1, type, bodyPart, true, damageType);
				}
				else
				{
					result = 0;
				}
			}
			return result;
		}

		// Token: 0x06006573 RID: 25971 RVA: 0x0039CE64 File Offset: 0x0039B064
		public int AppendFatalDamageMark(DataContext context, CombatCharacter character, int count, int type = -1, sbyte bodyPart = -1, bool addByValue = false, EDamageType damageType = EDamageType.None)
		{
			bool fatalImmunity = character.GetCharacter().GetFatalImmunity();
			int result;
			if (fatalImmunity)
			{
				this.ShowImmunityEffectTips(context, character.GetId(), EMarkType.Fatal);
				result = 0;
			}
			else
			{
				count = DomainManager.SpecialEffect.ModifyData(character.GetId(), -1, 192, count, type, (int)bodyPart, addByValue ? 1 : 0);
				count = DomainManager.SpecialEffect.ModifyData(character.GetId(), -1, 304, count, (int)damageType, -1, -1);
				bool flag = count <= 0;
				if (flag)
				{
					result = 0;
				}
				else
				{
					bool changeToMindMark = character.ChangeToMindMark;
					if (changeToMindMark)
					{
						this.AppendMindDefeatMark(context, character, count, -1, false);
						result = 0;
					}
					else
					{
						this.AppendFatalDamageMarkImmediate(context, character, count);
						result = count;
					}
				}
			}
			return result;
		}

		// Token: 0x06006574 RID: 25972 RVA: 0x0039CF10 File Offset: 0x0039B110
		public void AppendFatalDamageMarkImmediate(DataContext context, CombatCharacter character, int count)
		{
			DefeatMarkCollection markCollection = character.GetDefeatMarkCollection();
			markCollection.FatalDamageMarkCount = CMath.ClampFatalMarkCount(markCollection.FatalDamageMarkCount + count);
			character.SetDefeatMarkCollection(markCollection, context);
			Events.RaiseAddFatalDamageMark(context, character, count);
			this.AddToCheckFallenSet(character.GetId());
		}

		// Token: 0x06006575 RID: 25973 RVA: 0x0039CF58 File Offset: 0x0039B158
		public void RemoveHalfFatalDamageMark(DataContext context, CombatCharacter character)
		{
			int removeCount = character.GetDefeatMarkCollection().FatalDamageMarkCount * CValueHalf.RoundUp;
			DomainManager.Combat.RemoveFatalDamageMark(context, character, removeCount);
		}

		// Token: 0x06006576 RID: 25974 RVA: 0x0039CF8C File Offset: 0x0039B18C
		public void RemoveFatalDamageMark(DataContext context, CombatCharacter character, int count)
		{
			DefeatMarkCollection markCollection = character.GetDefeatMarkCollection();
			markCollection.FatalDamageMarkCount = CMath.ClampFatalMarkCount(markCollection.FatalDamageMarkCount - count);
			character.SetDefeatMarkCollection(markCollection, context);
		}

		// Token: 0x06006577 RID: 25975 RVA: 0x0039CFC0 File Offset: 0x0039B1C0
		public void TransferFatalDamageMark(DataContext context, CombatCharacter srcChar, CombatCharacter destChar, int count)
		{
			DefeatMarkCollection srcMarkCollection = srcChar.GetDefeatMarkCollection();
			DefeatMarkCollection destMarkCollection = destChar.GetDefeatMarkCollection();
			count = Math.Min(count, srcMarkCollection.FatalDamageMarkCount);
			srcMarkCollection.FatalDamageMarkCount = CMath.ClampFatalMarkCount(srcMarkCollection.FatalDamageMarkCount - count);
			destMarkCollection.FatalDamageMarkCount = CMath.ClampFatalMarkCount(destMarkCollection.FatalDamageMarkCount + count);
			srcChar.SetDefeatMarkCollection(srcMarkCollection, context);
			destChar.SetDefeatMarkCollection(destMarkCollection, context);
		}

		// Token: 0x06006578 RID: 25976 RVA: 0x0039D025 File Offset: 0x0039B225
		public void AddToCheckFallenSet(int charId)
		{
			this._needCheckFallenCharSet.Add(charId);
		}

		// Token: 0x06006579 RID: 25977 RVA: 0x0039D038 File Offset: 0x0039B238
		private bool CheckCurrCharDangerOrFallen(DataContext context, CombatCharacter character)
		{
			bool flag = !this.IsInCombat() || this.CombatAboutToOver() || (!this.IsCharacterFallen(character) && !this.DefeatMarkReachFailCount(character));
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				bool flag2 = this.EnemyUnyieldingFallen && !character.IsAlly;
				if (flag2)
				{
					this.GmCmd_HealAllDefeatMark(context, character.IsAlly);
				}
				for (int i = 0; i < 4; i++)
				{
					CombatDomain.RaiseCombatCharAboutToFall(context, character, i);
					bool flag3 = !this.IsCharacterFallen(character) && !this.DefeatMarkReachFailCount(character);
					if (flag3)
					{
						this._saveDyingEffectTriggerd = true;
						return false;
					}
				}
				bool flag4 = !this.IsCharacterFallen(character);
				if (flag4)
				{
					result = false;
				}
				else
				{
					Events.RaiseCombatCharFallen(context, character);
					CombatCharacter currChar = this.GetCombatCharacter(character.IsAlly, false);
					bool isCurrChar = currChar == character;
					bool flag5 = this.IsMainCharacter(character);
					if (flag5)
					{
						CombatCharacter enemyChar = this.GetCombatCharacter(!character.IsAlly, false);
						currChar.ClearAllDoingOrReserveCommand(context);
						enemyChar.ClearAllDoingOrReserveCommand(context);
						bool flag6 = currChar.NeedShowChangeTrick && currChar.IsAlly;
						if (flag6)
						{
							this.CancelChangeTrick(context, currChar.IsAlly);
						}
						bool flag7 = enemyChar.NeedShowChangeTrick && enemyChar.IsAlly;
						if (flag7)
						{
							this.CancelChangeTrick(context, enemyChar.IsAlly);
						}
						DomainManager.Combat.ForceAllTeammateLeaveCombatField(context, true);
						DomainManager.Combat.ForceAllTeammateLeaveCombatField(context, false);
						currChar.ChangeCharId = (isCurrChar ? -1 : (character.IsAlly ? this._selfTeam : this._enemyTeam)[0]);
						enemyChar.ChangeCharId = (this.IsMainCharacter(enemyChar) ? -1 : ((!character.IsAlly) ? this._selfTeam : this._enemyTeam)[0]);
						bool flag8 = character.ChangeCharId == -1 && enemyChar.ChangeCharId == -1;
						if (flag8)
						{
							this.EndCombat(context, character, false, true);
						}
						else
						{
							bool flag9 = character.BossConfig == null && character.AnimalConfig == null;
							if (flag9)
							{
								character.SetAnimationToPlayOnce(this.NeedShowMercy(character) ? "C_011_stun" : CombatDomain.FailAni[(int)this._combatType], context);
							}
						}
					}
					else
					{
						bool flag10 = isCurrChar;
						if (flag10)
						{
							character.ChangeCharId = (character.IsAlly ? this._selfTeam : this._enemyTeam)[0];
							character.ClearAllDoingOrReserveCommand(context);
							bool flag11 = character.NeedShowChangeTrick && character.IsAlly;
							if (flag11)
							{
								this.CancelChangeTrick(context, character.IsAlly);
							}
							bool flag12 = this.NeedShowMercy(character);
							if (flag12)
							{
								this.ShowMercyOption(context, this.GetCombatCharacter(!character.IsAlly, false));
							}
							else
							{
								character.ChangeCharFailAni = "C_005";
							}
						}
						else
						{
							character.ClearTeammateCommand(context, true);
						}
					}
					result = true;
				}
			}
			return result;
		}

		// Token: 0x0600657A RID: 25978 RVA: 0x0039D314 File Offset: 0x0039B514
		public bool IsCharacterFallen(CombatCharacter character)
		{
			bool immortal = character.Immortal;
			bool result;
			if (immortal)
			{
				result = false;
			}
			else
			{
				bool forceDefeat = character.ForceDefeat;
				if (forceDefeat)
				{
					result = true;
				}
				else
				{
					DefeatMarkCollection markCollection = character.GetDefeatMarkCollection();
					result = ((this.DefeatMarkReachFailCount(character) && !character.UnyieldingFallen) || markCollection.DieMarkList.Count >= (int)GameData.Domains.Combat.SharedConstValue.DefeatNeedDieMarkCount);
				}
			}
			return result;
		}

		// Token: 0x0600657B RID: 25979 RVA: 0x0039D374 File Offset: 0x0039B574
		public bool DefeatMarkReachFailCount(CombatCharacter character)
		{
			return character.GetDefeatMarkCollection().GetTotalCount() >= (int)GlobalConfig.NeedDefeatMarkCount[(int)this._combatType];
		}

		// Token: 0x0600657C RID: 25980 RVA: 0x0039D3A4 File Offset: 0x0039B5A4
		public bool IsCharacterHalfFallen(CombatCharacter character)
		{
			return character.GetDefeatMarkCollection().GetTotalCount() > (int)(GlobalConfig.NeedDefeatMarkCount[(int)this._combatType] / 2);
		}

		// Token: 0x0600657D RID: 25981 RVA: 0x0039D3D4 File Offset: 0x0039B5D4
		public ValueTuple<string, string, string> GetFailAnimationAndSound(DataContext context, CombatCharacter attacker, bool flee = false, bool kill = false)
		{
			ValueTuple<string, string, string> result;
			if (flee)
			{
				result = new ValueTuple<string, string, string>("M_004", "", "");
			}
			else
			{
				bool flag = !kill;
				if (flag)
				{
					CombatCharacter defender = this.GetCombatCharacter(!attacker.IsAlly, false);
					result = ((defender.BossConfig != null) ? new ValueTuple<string, string, string>(defender.BossConfig.FailAnimation, defender.BossConfig.FailParticles[(int)((this.CombatConfig.Scene >= 0) ? defender.GetBossPhase() : 1)], defender.BossConfig.FailSounds[(int)((this.CombatConfig.Scene >= 0) ? defender.GetBossPhase() : 1)]) : ((defender.AnimalConfig != null) ? new ValueTuple<string, string, string>(CombatDomain.FailAni[2], defender.AnimalConfig.FailParticle, defender.AnimalConfig.FailSound) : new ValueTuple<string, string, string>(CombatDomain.FailAni[(int)this._combatType], "", "")));
				}
				else
				{
					GameData.Domains.Item.Weapon weapon = DomainManager.Item.GetElement_Weapons(this.GetUsingWeaponKey(attacker).Id);
					sbyte trickType = attacker.GetWeaponTricks()[(int)attacker.GetWeaponTrickIndex()];
					TrickTypeItem trickConfig = TrickType.Instance[trickType];
					int weaponAction = (int)weapon.GetWeaponAction();
					List<string> aniRandomPool = trickConfig.ExecuteAni[weaponAction];
					List<string> particleRandomPool = trickConfig.ExecuteParticle[weaponAction];
					List<string> soundRandomPool = trickConfig.ExecuteSound[weaponAction];
					int index = context.Random.Next(aniRandomPool.Count);
					result = new ValueTuple<string, string, string>(aniRandomPool[index], particleRandomPool[index], soundRandomPool[index]);
				}
			}
			return result;
		}

		// Token: 0x0600657E RID: 25982 RVA: 0x0039D580 File Offset: 0x0039B780
		public void ClearBurstBodyPartFlawAndAcupoint(DataContext context, CombatCharacter combatChar, string aniName)
		{
			bool flag = aniName.Contains("burst");
			if (flag)
			{
				sbyte bodyPart = aniName.Contains("head") ? 2 : (aniName.Contains("arm") ? 4 : (aniName.Contains("leg") ? 5 : -1));
				bool flag2 = bodyPart >= 0;
				if (flag2)
				{
					byte[] flawCount = combatChar.GetFlawCount();
					FlawOrAcupointCollection flawCollection = combatChar.GetFlawCollection();
					byte[] acupointCount = combatChar.GetAcupointCount();
					FlawOrAcupointCollection acupointCollection = combatChar.GetAcupointCollection();
					flawCount[(int)bodyPart] = 0;
					flawCollection.BodyPartDict[bodyPart].Clear();
					acupointCount[(int)bodyPart] = 0;
					acupointCollection.BodyPartDict[bodyPart].Clear();
					combatChar.SetFlawCount(flawCount, context);
					combatChar.SetFlawCollection(flawCollection, context);
					combatChar.SetAcupointCount(acupointCount, context);
					combatChar.SetAcupointCollection(acupointCollection, context);
				}
			}
		}

		// Token: 0x0600657F RID: 25983 RVA: 0x0039D658 File Offset: 0x0039B858
		public bool NeedShowMercy(CombatCharacter failChar)
		{
			bool flag = !this.IsMainCharacter(failChar);
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				CombatCharacter winChar = this.GetCombatCharacter(!failChar.IsAlly, false);
				CombatCharacterStateType winnerState = winChar.StateMachine.GetCurrentStateType();
				bool flag2 = winnerState == CombatCharacterStateType.SelectMercy;
				if (flag2)
				{
					result = false;
				}
				else
				{
					bool flag3 = this.IsInfectedCombat() && !failChar.IsAlly;
					if (flag3)
					{
						result = true;
					}
					else
					{
						bool flag4 = DomainManager.World.GetAllowExecute() && this._combatType == 2 && this.CombatConfig.AllowShowMercy && failChar.GetDefeatMarkCollection().GetTotalCount() > (int)GlobalConfig.NeedDefeatMarkCount[2];
						bool flag5 = flag4;
						if (flag5)
						{
							bool flag6 = winnerState == CombatCharacterStateType.Idle || winnerState == CombatCharacterStateType.Attack || winnerState == CombatCharacterStateType.CastSkill;
							flag5 = flag6;
						}
						result = (flag5 && winChar.IsActorSkeleton && failChar.IsActorSkeleton && failChar.GetCharacter().GetAgeGroup() == 2 && failChar.GetId() != DomainManager.Character.GetAvoidDeathCharId() && !this._isTutorialCombat && this.Context.Random.CheckPercentProb(50));
					}
				}
			}
			return result;
		}

		// Token: 0x06006580 RID: 25984 RVA: 0x0039D784 File Offset: 0x0039B984
		public void EndCombat(DataContext context, CombatCharacter failChar, bool flee = false, bool playAni = true)
		{
			this.ClearCombatResultLegacies();
			CombatCharacter winChar = this.GetCombatCharacter(!failChar.IsAlly, false);
			failChar.ClearAllDoingOrReserveCommand(context);
			bool flag = !flee && this.NeedShowMercy(failChar);
			if (flag)
			{
				this.ShowMercyOption(context, winChar);
			}
			else
			{
				if (playAni)
				{
					bool flag2 = !this.GetIsPuppetCombat();
					if (flag2)
					{
						ValueTuple<string, string, string> dieEffect = this.GetFailAnimationAndSound(context, winChar, flee, false);
						bool flag3 = (failChar.GetAnimationToPlayOnce() != dieEffect.Item1 || failChar.BossConfig != null) && (!flee || failChar.AnimalConfig == null);
						if (flag3)
						{
							failChar.SetAnimationToPlayOnce(dieEffect.Item1, context);
						}
						bool flag4 = dieEffect.Item2 != "" && failChar.GetParticleToPlay() != dieEffect.Item2;
						if (flag4)
						{
							failChar.SetParticleToPlay(dieEffect.Item2, context);
						}
						bool flag5 = dieEffect.Item3 != "" && failChar.GetDieSoundToPlay() != dieEffect.Item3;
						if (flag5)
						{
							failChar.SetDieSoundToPlay(dieEffect.Item3, context);
						}
						BossItem bossConfig = failChar.BossConfig;
						bool flag6;
						if (bossConfig != null)
						{
							string[] failPetParticles = bossConfig.FailPetParticles;
							if (failPetParticles != null && failPetParticles.Length > 0)
							{
								flag6 = !string.IsNullOrEmpty(failChar.BossConfig.FailPetParticles[0]);
								goto IL_15F;
							}
						}
						flag6 = false;
						IL_15F:
						bool flag7 = flag6;
						if (flag7)
						{
							failChar.SetPetParticle(failChar.BossConfig.FailPetParticles[1], context);
						}
					}
					else
					{
						failChar.SetAnimationToLoop("C_000", context);
						failChar.SetAnimationToPlayOnce(null, context);
						failChar.SetParticleToPlay(null, context);
						failChar.SetDieSoundToPlay(null, context);
						BossItem bossConfig = failChar.BossConfig;
						bool flag8;
						if (bossConfig != null)
						{
							string[] failPetParticles = bossConfig.FailPetParticles;
							if (failPetParticles != null)
							{
								flag8 = (failPetParticles.Length > 0);
								goto IL_1C9;
							}
						}
						flag8 = false;
						IL_1C9:
						bool flag9 = flag8;
						if (flag9)
						{
							failChar.SetPetParticle(null, context);
						}
					}
					winChar.PlayWinAnimation(context);
				}
				bool flag10 = !flee && playAni;
				if (flag10)
				{
					this.SetWaitingDelaySettlement(true, context);
					winChar.NeedDelaySettlement = true;
					winChar.StateMachine.TranslateState();
				}
				else
				{
					this.CombatSettlement(context, flee ? (failChar.IsAlly ? CombatStatusType.SelfFlee : CombatStatusType.EnemyFlee) : (failChar.IsAlly ? CombatStatusType.SelfFail : CombatStatusType.EnemyFail));
				}
			}
			bool flag11 = (this._combatType == 1 || this._combatType == 2) && winChar.IsAlly;
			if (flag11)
			{
				for (int i = 0; i < this._enemyTeam.Length; i++)
				{
					int charId = this._enemyTeam[i];
					bool flag12 = charId < 0;
					if (!flag12)
					{
						GameData.Domains.Character.Character enemyChar = DomainManager.Character.GetElement_Objects(charId);
						bool flag13 = enemyChar.GetCreatingType() == 1;
						if (flag13)
						{
							DomainManager.Character.LoseGuard(context, charId, enemyChar);
						}
					}
				}
			}
			bool flag14 = (36 <= this.CombatConfig.TemplateId && this.CombatConfig.TemplateId <= 44) || (54 <= this.CombatConfig.TemplateId && this.CombatConfig.TemplateId <= 62);
			if (flag14)
			{
				bool flag15 = failChar.BossConfig != null;
				if (flag15)
				{
					sbyte xiangshuAvatarId = XiangshuAvatarIds.GetXiangshuAvatarIdByCharacterTemplateId(failChar.GetCharacter().GetTemplateId());
					bool flag16 = xiangshuAvatarId >= 0;
					if (flag16)
					{
						DomainManager.World.SetSwordTombStatus(context, xiangshuAvatarId, 2);
					}
				}
				else
				{
					bool flag17 = winChar.BossConfig != null && winChar.GetBossPhase() > 0;
					if (flag17)
					{
						sbyte xiangshuAvatarId2 = XiangshuAvatarIds.GetXiangshuAvatarIdByCharacterTemplateId(winChar.GetCharacter().GetTemplateId());
						bool flag18 = xiangshuAvatarId2 >= 0;
						if (flag18)
						{
							DomainManager.World.SetSwordTombStatus(context, xiangshuAvatarId2, 1);
						}
					}
				}
			}
		}

		// Token: 0x06006581 RID: 25985 RVA: 0x0039DB3D File Offset: 0x0039BD3D
		public void Flee(DataContext context, CombatCharacter character)
		{
			this.EndCombat(context, character, true, true);
		}

		// Token: 0x06006582 RID: 25986 RVA: 0x0039DB4C File Offset: 0x0039BD4C
		public void CombatSettlement(DataContext context, sbyte statusType)
		{
			this.Started = false;
			this._combatResultData.CombatStatus = statusType;
			Events.RaiseCombatSettlement(context, statusType);
			this.SetCombatStatus(statusType, context);
			this.CalcEvaluationList(context);
			this.CalcReadInCombat(context);
			this.CalcQiQrtInCombat(context);
			this.CalcAddLegacyPoint(context);
			this.CalcAndAddFameAction(context);
			this.CalcAndAddAreaSpiritualDebt(context);
			foreach (CombatCharacter combatChar in this._combatCharacterDict.Values)
			{
				combatChar.RevertAllRawCreates(context);
			}
			this.CalcAndAddExp(context);
			this.CalcAndAddResource(context);
			this.CalcAndAddProficiency(context);
			this.CalcLootItem(context);
			this.GetLootCharDisplayData();
			this._selfChar.OnFrameBegin();
			this._selfChar.OnFrameEnd();
			this._enemyChar.OnFrameBegin();
			this._enemyChar.OnFrameEnd();
			DomainManager.SpecialEffect.RemoveAllEffectsInCombat(context);
			this.ClearSkillPowerAddInCombat(context);
			this.ClearSkillPowerReduceInCombat(context);
			this.ClearSkillPowerReplaceInCombat(context);
			this.EquipmentPowerChangeInCombat.Clear();
			foreach (CombatCharacter combatChar2 in this._combatCharacterDict.Values)
			{
				combatChar2.OnCombatEnd(context);
			}
			CombatCharacter failChar = this.IsWin(true) ? this._enemyChar : this._selfChar;
			bool flag = failChar.GetId() == DomainManager.Character.GetAvoidDeathCharId();
			if (flag)
			{
				DomainManager.Character.TransferAvoidDeathCharInjuries(context);
			}
			bool isPuppetCombat = this._isPuppetCombat;
			if (isPuppetCombat)
			{
				DomainManager.Character.RemoveNonIntelligentCharacter(context, this._enemyChar.GetCharacter());
				this.SetIsPuppetCombat(false, context);
				this.SetIsPlaygroundCombat(false, context);
			}
			bool flag2 = this._carrierAnimalCombatCharId >= 0;
			if (flag2)
			{
				DomainManager.Character.RemoveNonIntelligentCharacter(context, this._combatCharacterDict[this._carrierAnimalCombatCharId].GetCharacter());
			}
			bool flag3 = this._specialShowCombatCharId >= 0;
			if (flag3)
			{
				DomainManager.Character.RemoveNonIntelligentCharacter(context, this._combatCharacterDict[this._specialShowCombatCharId].GetCharacter());
			}
			this.ClearSpecialGroup(context);
			foreach (CombatCharacter combatChar3 in this._combatCharacterDict.Values)
			{
				bool allowUseFreeWeapon = combatChar3.GetCharacter().GetAllowUseFreeWeapon();
				if (allowUseFreeWeapon)
				{
					combatChar3.RemoveTempWeapons(context);
				}
			}
			List<int> combatCharIds = ObjectPool<List<int>>.Instance.Get();
			combatCharIds.Clear();
			combatCharIds.AddRange(this._combatCharacterDict.Keys);
			foreach (int combatCharId in combatCharIds)
			{
				this.RemoveElement_CombatCharacterDict(combatCharId);
			}
			ObjectPool<List<int>>.Instance.Return(combatCharIds);
			this.CalcSnapshotAfterCombat(context);
			Events.RaiseCombatEnd(context);
		}

		// Token: 0x06006583 RID: 25987 RVA: 0x0039DE98 File Offset: 0x0039C098
		public static int GetDefeatMarksCountOutOfCombat(GameData.Domains.Character.Character character)
		{
			int count = 0;
			count += character.GetInjuries().GetSum();
			count += character.GetPoisonMarkCount();
			count += (int)character.GetEatingItems().CountOfWugMark();
			count += (int)DefeatMarkCollection.CalcQiDisorderMarkCount((int)character.GetDisorderOfQi());
			return count + (int)DefeatMarkCollection.GetHealthMarkCount(character);
		}

		// Token: 0x06006584 RID: 25988 RVA: 0x0039DEEC File Offset: 0x0039C0EC
		[DomainMethod]
		public static DefeatMarksCountOutOfCombatData GetDefeatMarksCountOutOfCombat(DataContext context, int charId)
		{
			DefeatMarksCountOutOfCombatData data = new DefeatMarksCountOutOfCombatData();
			GameData.Domains.Character.Character character = DomainManager.Character.GetElement_Objects(charId);
			int injuryCount = character.GetInjuries().GetSum();
			int poisonCount = character.GetPoisonMarkCount();
			sbyte wugCount = character.GetEatingItems().CountOfWugMark();
			sbyte qiDisorderCount = DefeatMarkCollection.CalcQiDisorderMarkCount((int)character.GetDisorderOfQi());
			sbyte healthCount = DefeatMarkCollection.GetHealthMarkCount(character);
			bool flag = injuryCount > 0;
			if (flag)
			{
				data.DefeatMarksDict.Add(0, injuryCount);
			}
			bool flag2 = poisonCount > 0;
			if (flag2)
			{
				data.DefeatMarksDict.Add(2, poisonCount);
			}
			bool flag3 = wugCount > 0;
			if (flag3)
			{
				data.DefeatMarksDict.Add(5, (int)wugCount);
			}
			bool flag4 = qiDisorderCount > 0;
			if (flag4)
			{
				data.DefeatMarksDict.Add(6, (int)qiDisorderCount);
			}
			bool flag5 = healthCount > 0;
			if (flag5)
			{
				data.DefeatMarksDict.Add(9, (int)healthCount);
			}
			return data;
		}

		// Token: 0x06006585 RID: 25989 RVA: 0x0039DFCC File Offset: 0x0039C1CC
		public bool AddWug(DataContext context, CombatCharacter affectChar, sbyte wugType, bool isBad, int srcCharId)
		{
			short wugTemplateId = ItemDomain.GetWugTemplateId(wugType, isBad ? 2 : 0);
			return this.AddWug(context, affectChar, wugTemplateId, srcCharId, EWugReplaceType.CombatOnly);
		}

		// Token: 0x06006586 RID: 25990 RVA: 0x0039DFFC File Offset: 0x0039C1FC
		public unsafe bool AddWug(DataContext context, CombatCharacter affectChar, short wugTemplateId, int srcCharId, EWugReplaceType replaceType = EWugReplaceType.None)
		{
			bool flag = !DomainManager.SpecialEffect.ModifyData(affectChar.GetId(), -1, 180, true, srcCharId, -1, -1);
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				MedicineItem wugConfig = Config.Medicine.Instance[wugTemplateId];
				EatingItems eatingItems = *affectChar.GetCharacter().GetEatingItems();
				int index = eatingItems.IndexOfWug(wugConfig);
				bool flag2 = index >= 0 && !replaceType.IsMatchWug(eatingItems.Get(index).TemplateId, wugTemplateId);
				if (flag2)
				{
					result = false;
				}
				else
				{
					affectChar.GetCharacter().AddWug(context, wugTemplateId);
					result = true;
				}
			}
			return result;
		}

		// Token: 0x06006587 RID: 25991 RVA: 0x0039E095 File Offset: 0x0039C295
		public void AddWugIrresistibly(DataContext context, CombatCharacter affectChar, ItemKey wugItemKey)
		{
			affectChar.GetCharacter().AddEatingItem(context, wugItemKey, null);
		}

		// Token: 0x06006588 RID: 25992 RVA: 0x0039E0A8 File Offset: 0x0039C2A8
		public unsafe short RemoveRandomWug(DataContext context, CombatCharacter combatChar, EWugReplaceType replaceType = EWugReplaceType.CombatOnly)
		{
			List<short> allExistWug = ObjectPool<List<short>>.Instance.Get();
			allExistWug.Clear();
			EatingItems eatingItems = *combatChar.GetCharacter().GetEatingItems();
			for (sbyte wugType = 0; wugType < 8; wugType += 1)
			{
				int index = eatingItems.IndexOfWug(wugType, false);
				bool flag = index < 0;
				if (!flag)
				{
					short wugTemplateId = eatingItems.Get(index).TemplateId;
					bool flag2 = replaceType.IsMatchWug(wugTemplateId, -1);
					if (flag2)
					{
						allExistWug.Add(wugTemplateId);
					}
				}
			}
			short removeWugTemplateId = (allExistWug.Count > 0) ? allExistWug.GetRandom(context.Random) : -1;
			ObjectPool<List<short>>.Instance.Return(allExistWug);
			bool flag3 = removeWugTemplateId >= 0;
			if (flag3)
			{
				combatChar.GetCharacter().RemoveWug(context, removeWugTemplateId);
			}
			return removeWugTemplateId;
		}

		// Token: 0x06006589 RID: 25993 RVA: 0x0039E174 File Offset: 0x0039C374
		public CombatDomain() : base(33)
		{
			this._timeScale = 0f;
			this._autoCombat = false;
			this._combatFrame = 0UL;
			this._combatType = 0;
			this._currentDistance = 0;
			this._damageCompareData = new DamageCompareData();
			this._skillPowerAddInCombat = new Dictionary<CombatSkillKey, SkillPowerChangeCollection>(0);
			this._skillPowerReduceInCombat = new Dictionary<CombatSkillKey, SkillPowerChangeCollection>(0);
			this._skillPowerReplaceInCombat = new Dictionary<CombatSkillKey, CombatSkillKey>(0);
			this._bgmIndex = 0;
			this._combatCharacterDict = new Dictionary<int, CombatCharacter>(0);
			this._selfTeam = new int[4];
			this._selfCharId = 0;
			this._selfTeamWisdomType = 0;
			this._selfTeamWisdomCount = 0;
			this._enemyTeam = new int[4];
			this._enemyCharId = 0;
			this._enemyTeamWisdomType = 0;
			this._enemyTeamWisdomCount = 0;
			this._combatStatus = 0;
			this._showMercyOption = 0;
			this._selectedMercyOption = 0;
			this._carrierAnimalCombatCharId = 0;
			this._specialShowCombatCharId = 0;
			this._skillAttackedIndexAndHit = default(IntPair);
			this._waitingDelaySettlement = false;
			this._showUseGoldenWire = default(SpecialMiscData);
			this._isPuppetCombat = false;
			this._isPlaygroundCombat = false;
			this._skillDataDict = new Dictionary<CombatSkillKey, CombatSkillData>(0);
			this._weaponDataDict = new Dictionary<int, CombatWeaponData>(0);
			this._expectRatioData = new WeaponExpectInnerRatioData();
			this._taiwuSpecialGroupCharIds = new List<int>();
			this.HelperDataCombatCharacterDict = new ObjectCollectionHelperData(8, 10, CombatDomain.CacheInfluencesCombatCharacterDict, this._dataStatesCombatCharacterDict, false);
			this.HelperDataSkillDataDict = new ObjectCollectionHelperData(8, 29, CombatDomain.CacheInfluencesSkillDataDict, this._dataStatesSkillDataDict, false);
			this.HelperDataWeaponDataDict = new ObjectCollectionHelperData(8, 30, CombatDomain.CacheInfluencesWeaponDataDict, this._dataStatesWeaponDataDict, false);
			this.OnInitializedDomainData();
		}

		// Token: 0x0600658A RID: 25994 RVA: 0x0039E49C File Offset: 0x0039C69C
		public float GetTimeScale()
		{
			return this._timeScale;
		}

		// Token: 0x0600658B RID: 25995 RVA: 0x0039E4B4 File Offset: 0x0039C6B4
		private void SetTimeScale(float value, DataContext context)
		{
			this._timeScale = value;
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(0, this.DataStates, CombatDomain.CacheInfluences, context);
		}

		// Token: 0x0600658C RID: 25996 RVA: 0x0039E4D4 File Offset: 0x0039C6D4
		public bool GetAutoCombat()
		{
			return this._autoCombat;
		}

		// Token: 0x0600658D RID: 25997 RVA: 0x0039E4EC File Offset: 0x0039C6EC
		private void SetAutoCombat(bool value, DataContext context)
		{
			this._autoCombat = value;
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(1, this.DataStates, CombatDomain.CacheInfluences, context);
		}

		// Token: 0x0600658E RID: 25998 RVA: 0x0039E50C File Offset: 0x0039C70C
		public ulong GetCombatFrame()
		{
			return this._combatFrame;
		}

		// Token: 0x0600658F RID: 25999 RVA: 0x0039E524 File Offset: 0x0039C724
		public void SetCombatFrame(ulong value, DataContext context)
		{
			this._combatFrame = value;
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(2, this.DataStates, CombatDomain.CacheInfluences, context);
		}

		// Token: 0x06006590 RID: 26000 RVA: 0x0039E544 File Offset: 0x0039C744
		public sbyte GetCombatType()
		{
			return this._combatType;
		}

		// Token: 0x06006591 RID: 26001 RVA: 0x0039E55C File Offset: 0x0039C75C
		public void SetCombatType(sbyte value, DataContext context)
		{
			this._combatType = value;
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(3, this.DataStates, CombatDomain.CacheInfluences, context);
		}

		// Token: 0x06006592 RID: 26002 RVA: 0x0039E57C File Offset: 0x0039C77C
		public short GetCurrentDistance()
		{
			return this._currentDistance;
		}

		// Token: 0x06006593 RID: 26003 RVA: 0x0039E594 File Offset: 0x0039C794
		public void SetCurrentDistance(short value, DataContext context)
		{
			this._currentDistance = value;
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(4, this.DataStates, CombatDomain.CacheInfluences, context);
		}

		// Token: 0x06006594 RID: 26004 RVA: 0x0039E5B4 File Offset: 0x0039C7B4
		public DamageCompareData GetDamageCompareData()
		{
			return this._damageCompareData;
		}

		// Token: 0x06006595 RID: 26005 RVA: 0x0039E5CC File Offset: 0x0039C7CC
		public void SetDamageCompareData(DamageCompareData value, DataContext context)
		{
			this._damageCompareData = value;
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(5, this.DataStates, CombatDomain.CacheInfluences, context);
		}

		// Token: 0x06006596 RID: 26006 RVA: 0x0039E5EC File Offset: 0x0039C7EC
		public SkillPowerChangeCollection GetElement_SkillPowerAddInCombat(CombatSkillKey elementId)
		{
			return this._skillPowerAddInCombat[elementId];
		}

		// Token: 0x06006597 RID: 26007 RVA: 0x0039E60C File Offset: 0x0039C80C
		public bool TryGetElement_SkillPowerAddInCombat(CombatSkillKey elementId, out SkillPowerChangeCollection value)
		{
			return this._skillPowerAddInCombat.TryGetValue(elementId, out value);
		}

		// Token: 0x06006598 RID: 26008 RVA: 0x0039E62B File Offset: 0x0039C82B
		private void AddElement_SkillPowerAddInCombat(CombatSkillKey elementId, SkillPowerChangeCollection value, DataContext context)
		{
			this._skillPowerAddInCombat.Add(elementId, value);
			this._modificationsSkillPowerAddInCombat.RecordAdding(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(6, this.DataStates, CombatDomain.CacheInfluences, context);
		}

		// Token: 0x06006599 RID: 26009 RVA: 0x0039E65C File Offset: 0x0039C85C
		private void SetElement_SkillPowerAddInCombat(CombatSkillKey elementId, SkillPowerChangeCollection value, DataContext context)
		{
			this._skillPowerAddInCombat[elementId] = value;
			this._modificationsSkillPowerAddInCombat.RecordSetting(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(6, this.DataStates, CombatDomain.CacheInfluences, context);
		}

		// Token: 0x0600659A RID: 26010 RVA: 0x0039E68D File Offset: 0x0039C88D
		private void RemoveElement_SkillPowerAddInCombat(CombatSkillKey elementId, DataContext context)
		{
			this._skillPowerAddInCombat.Remove(elementId);
			this._modificationsSkillPowerAddInCombat.RecordRemoving(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(6, this.DataStates, CombatDomain.CacheInfluences, context);
		}

		// Token: 0x0600659B RID: 26011 RVA: 0x0039E6BD File Offset: 0x0039C8BD
		private void ClearSkillPowerAddInCombat(DataContext context)
		{
			this._skillPowerAddInCombat.Clear();
			this._modificationsSkillPowerAddInCombat.RecordClearing();
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(6, this.DataStates, CombatDomain.CacheInfluences, context);
		}

		// Token: 0x0600659C RID: 26012 RVA: 0x0039E6EC File Offset: 0x0039C8EC
		public SkillPowerChangeCollection GetElement_SkillPowerReduceInCombat(CombatSkillKey elementId)
		{
			return this._skillPowerReduceInCombat[elementId];
		}

		// Token: 0x0600659D RID: 26013 RVA: 0x0039E70C File Offset: 0x0039C90C
		public bool TryGetElement_SkillPowerReduceInCombat(CombatSkillKey elementId, out SkillPowerChangeCollection value)
		{
			return this._skillPowerReduceInCombat.TryGetValue(elementId, out value);
		}

		// Token: 0x0600659E RID: 26014 RVA: 0x0039E72B File Offset: 0x0039C92B
		private void AddElement_SkillPowerReduceInCombat(CombatSkillKey elementId, SkillPowerChangeCollection value, DataContext context)
		{
			this._skillPowerReduceInCombat.Add(elementId, value);
			this._modificationsSkillPowerReduceInCombat.RecordAdding(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(7, this.DataStates, CombatDomain.CacheInfluences, context);
		}

		// Token: 0x0600659F RID: 26015 RVA: 0x0039E75C File Offset: 0x0039C95C
		private void SetElement_SkillPowerReduceInCombat(CombatSkillKey elementId, SkillPowerChangeCollection value, DataContext context)
		{
			this._skillPowerReduceInCombat[elementId] = value;
			this._modificationsSkillPowerReduceInCombat.RecordSetting(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(7, this.DataStates, CombatDomain.CacheInfluences, context);
		}

		// Token: 0x060065A0 RID: 26016 RVA: 0x0039E78D File Offset: 0x0039C98D
		private void RemoveElement_SkillPowerReduceInCombat(CombatSkillKey elementId, DataContext context)
		{
			this._skillPowerReduceInCombat.Remove(elementId);
			this._modificationsSkillPowerReduceInCombat.RecordRemoving(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(7, this.DataStates, CombatDomain.CacheInfluences, context);
		}

		// Token: 0x060065A1 RID: 26017 RVA: 0x0039E7BD File Offset: 0x0039C9BD
		private void ClearSkillPowerReduceInCombat(DataContext context)
		{
			this._skillPowerReduceInCombat.Clear();
			this._modificationsSkillPowerReduceInCombat.RecordClearing();
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(7, this.DataStates, CombatDomain.CacheInfluences, context);
		}

		// Token: 0x060065A2 RID: 26018 RVA: 0x0039E7EC File Offset: 0x0039C9EC
		public CombatSkillKey GetElement_SkillPowerReplaceInCombat(CombatSkillKey elementId)
		{
			return this._skillPowerReplaceInCombat[elementId];
		}

		// Token: 0x060065A3 RID: 26019 RVA: 0x0039E80C File Offset: 0x0039CA0C
		public bool TryGetElement_SkillPowerReplaceInCombat(CombatSkillKey elementId, out CombatSkillKey value)
		{
			return this._skillPowerReplaceInCombat.TryGetValue(elementId, out value);
		}

		// Token: 0x060065A4 RID: 26020 RVA: 0x0039E82B File Offset: 0x0039CA2B
		private void AddElement_SkillPowerReplaceInCombat(CombatSkillKey elementId, CombatSkillKey value, DataContext context)
		{
			this._skillPowerReplaceInCombat.Add(elementId, value);
			this._modificationsSkillPowerReplaceInCombat.RecordAdding(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(8, this.DataStates, CombatDomain.CacheInfluences, context);
		}

		// Token: 0x060065A5 RID: 26021 RVA: 0x0039E85C File Offset: 0x0039CA5C
		private void SetElement_SkillPowerReplaceInCombat(CombatSkillKey elementId, CombatSkillKey value, DataContext context)
		{
			this._skillPowerReplaceInCombat[elementId] = value;
			this._modificationsSkillPowerReplaceInCombat.RecordSetting(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(8, this.DataStates, CombatDomain.CacheInfluences, context);
		}

		// Token: 0x060065A6 RID: 26022 RVA: 0x0039E88D File Offset: 0x0039CA8D
		private void RemoveElement_SkillPowerReplaceInCombat(CombatSkillKey elementId, DataContext context)
		{
			this._skillPowerReplaceInCombat.Remove(elementId);
			this._modificationsSkillPowerReplaceInCombat.RecordRemoving(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(8, this.DataStates, CombatDomain.CacheInfluences, context);
		}

		// Token: 0x060065A7 RID: 26023 RVA: 0x0039E8BD File Offset: 0x0039CABD
		private void ClearSkillPowerReplaceInCombat(DataContext context)
		{
			this._skillPowerReplaceInCombat.Clear();
			this._modificationsSkillPowerReplaceInCombat.RecordClearing();
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(8, this.DataStates, CombatDomain.CacheInfluences, context);
		}

		// Token: 0x060065A8 RID: 26024 RVA: 0x0039E8EC File Offset: 0x0039CAEC
		public sbyte GetBgmIndex()
		{
			return this._bgmIndex;
		}

		// Token: 0x060065A9 RID: 26025 RVA: 0x0039E904 File Offset: 0x0039CB04
		public void SetBgmIndex(sbyte value, DataContext context)
		{
			this._bgmIndex = value;
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(9, this.DataStates, CombatDomain.CacheInfluences, context);
		}

		// Token: 0x060065AA RID: 26026 RVA: 0x0039E924 File Offset: 0x0039CB24
		public CombatCharacter GetElement_CombatCharacterDict(int objectId)
		{
			return this._combatCharacterDict[objectId];
		}

		// Token: 0x060065AB RID: 26027 RVA: 0x0039E944 File Offset: 0x0039CB44
		public bool TryGetElement_CombatCharacterDict(int objectId, out CombatCharacter element)
		{
			return this._combatCharacterDict.TryGetValue(objectId, out element);
		}

		// Token: 0x060065AC RID: 26028 RVA: 0x0039E963 File Offset: 0x0039CB63
		private void AddElement_CombatCharacterDict(int objectId, CombatCharacter instance)
		{
			instance.CollectionHelperData = this.HelperDataCombatCharacterDict;
			instance.DataStatesOffset = this._dataStatesCombatCharacterDict.Create();
			this._combatCharacterDict.Add(objectId, instance);
		}

		// Token: 0x060065AD RID: 26029 RVA: 0x0039E994 File Offset: 0x0039CB94
		private void RemoveElement_CombatCharacterDict(int objectId)
		{
			CombatCharacter instance;
			bool flag = !this._combatCharacterDict.TryGetValue(objectId, out instance);
			if (!flag)
			{
				this._dataStatesCombatCharacterDict.Remove(instance.DataStatesOffset);
				this._combatCharacterDict.Remove(objectId);
			}
		}

		// Token: 0x060065AE RID: 26030 RVA: 0x0039E9D8 File Offset: 0x0039CBD8
		private void ClearCombatCharacterDict()
		{
			this._dataStatesCombatCharacterDict.Clear();
			this._combatCharacterDict.Clear();
		}

		// Token: 0x060065AF RID: 26031 RVA: 0x0039E9F4 File Offset: 0x0039CBF4
		public unsafe int GetElementField_CombatCharacterDict(int objectId, ushort fieldId, RawDataPool dataPool, bool resetModified)
		{
			CombatCharacter instance;
			bool flag = !this._combatCharacterDict.TryGetValue(objectId, out instance);
			int result;
			if (flag)
			{
				string tag = "GetElementField_CombatCharacterDict";
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Failed to find element ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(objectId);
				defaultInterpolatedStringHandler.AppendLiteral(" with field ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(fieldId);
				AdaptableLog.TagWarning(tag, defaultInterpolatedStringHandler.ToStringAndClear(), false);
				result = -1;
			}
			else
			{
				if (resetModified)
				{
					this._dataStatesCombatCharacterDict.ResetModified(instance.DataStatesOffset, (int)fieldId);
				}
				switch (fieldId)
				{
				case 0:
					result = Serializer.Serialize(instance.GetId(), dataPool);
					break;
				case 1:
					result = Serializer.Serialize(instance.GetBreathValue(), dataPool);
					break;
				case 2:
					result = Serializer.Serialize(instance.GetStanceValue(), dataPool);
					break;
				case 3:
					result = Serializer.Serialize(instance.GetNeiliAllocation(), dataPool);
					break;
				case 4:
					result = Serializer.Serialize(instance.GetOriginNeiliAllocation(), dataPool);
					break;
				case 5:
					result = Serializer.Serialize(instance.GetNeiliAllocationRecoverProgress(), dataPool);
					break;
				case 6:
					result = Serializer.Serialize(instance.GetOldDisorderOfQi(), dataPool);
					break;
				case 7:
					result = Serializer.Serialize(instance.GetNeiliType(), dataPool);
					break;
				case 8:
					result = Serializer.Serialize(instance.GetAvoidToShow(), dataPool);
					break;
				case 9:
					result = Serializer.Serialize(instance.GetCurrentPosition(), dataPool);
					break;
				case 10:
					result = Serializer.Serialize(instance.GetDisplayPosition(), dataPool);
					break;
				case 11:
					result = Serializer.Serialize(instance.GetMobilityValue(), dataPool);
					break;
				case 12:
					result = Serializer.Serialize(instance.GetJumpPrepareProgress(), dataPool);
					break;
				case 13:
					result = Serializer.Serialize(instance.GetJumpPreparedDistance(), dataPool);
					break;
				case 14:
					result = Serializer.Serialize(instance.GetMobilityLockEffectCount(), dataPool);
					break;
				case 15:
					result = Serializer.Serialize(instance.GetJumpChangeDistanceDuration(), dataPool);
					break;
				case 16:
					result = Serializer.Serialize(instance.GetUsingWeaponIndex(), dataPool);
					break;
				case 17:
					result = Serializer.Serialize(instance.GetWeaponTricks(), dataPool);
					break;
				case 18:
					result = Serializer.Serialize(instance.GetWeaponTrickIndex(), dataPool);
					break;
				case 19:
					result = Serializer.Serialize(instance.GetWeapons(), dataPool);
					break;
				case 20:
					result = Serializer.Serialize(instance.GetAttackingTrickType(), dataPool);
					break;
				case 21:
					result = Serializer.Serialize(instance.GetCanAttackOutRange(), dataPool);
					break;
				case 22:
					result = Serializer.Serialize(instance.GetChangeTrickProgress(), dataPool);
					break;
				case 23:
					result = Serializer.Serialize(instance.GetChangeTrickCount(), dataPool);
					break;
				case 24:
					result = Serializer.Serialize(instance.GetCanChangeTrick(), dataPool);
					break;
				case 25:
					result = Serializer.Serialize(instance.GetChangingTrick(), dataPool);
					break;
				case 26:
					result = Serializer.Serialize(instance.GetChangeTrickAttack(), dataPool);
					break;
				case 27:
					result = Serializer.Serialize(instance.GetIsFightBack(), dataPool);
					break;
				case 28:
					result = Serializer.Serialize(instance.GetTricks(), dataPool);
					break;
				case 29:
					result = Serializer.Serialize(instance.GetInjuries(), dataPool);
					break;
				case 30:
					result = Serializer.Serialize(instance.GetOldInjuries(), dataPool);
					break;
				case 31:
					result = Serializer.Serialize(instance.GetInjuryAutoHealCollection(), dataPool);
					break;
				case 32:
					result = Serializer.Serialize(instance.GetDamageStepCollection(), dataPool);
					break;
				case 33:
					result = Serializer.Serialize(instance.GetOuterDamageValue(), dataPool);
					break;
				case 34:
					result = Serializer.Serialize(instance.GetInnerDamageValue(), dataPool);
					break;
				case 35:
					result = Serializer.Serialize(instance.GetMindDamageValue(), dataPool);
					break;
				case 36:
					result = Serializer.Serialize(instance.GetFatalDamageValue(), dataPool);
					break;
				case 37:
					result = Serializer.Serialize(instance.GetOuterDamageValueToShow(), dataPool);
					break;
				case 38:
					result = Serializer.Serialize(instance.GetInnerDamageValueToShow(), dataPool);
					break;
				case 39:
					result = Serializer.Serialize(instance.GetMindDamageValueToShow(), dataPool);
					break;
				case 40:
					result = Serializer.Serialize(instance.GetFatalDamageValueToShow(), dataPool);
					break;
				case 41:
					result = Serializer.Serialize(instance.GetFlawCount(), dataPool);
					break;
				case 42:
					result = Serializer.Serialize(instance.GetFlawCollection(), dataPool);
					break;
				case 43:
					result = Serializer.Serialize(instance.GetAcupointCount(), dataPool);
					break;
				case 44:
					result = Serializer.Serialize(instance.GetAcupointCollection(), dataPool);
					break;
				case 45:
					result = Serializer.Serialize(instance.GetMindMarkTime(), dataPool);
					break;
				case 46:
					result = Serializer.Serialize(*instance.GetPoison(), dataPool);
					break;
				case 47:
					result = Serializer.Serialize(*instance.GetOldPoison(), dataPool);
					break;
				case 48:
					result = Serializer.Serialize(*instance.GetPoisonResist(), dataPool);
					break;
				case 49:
					result = Serializer.Serialize(*instance.GetNewPoisonsToShow(), dataPool);
					break;
				case 50:
					result = Serializer.Serialize(instance.GetDefeatMarkCollection(), dataPool);
					break;
				case 51:
					result = Serializer.Serialize(instance.GetNeigongList(), dataPool);
					break;
				case 52:
					result = Serializer.Serialize(instance.GetAttackSkillList(), dataPool);
					break;
				case 53:
					result = Serializer.Serialize(instance.GetAgileSkillList(), dataPool);
					break;
				case 54:
					result = Serializer.Serialize(instance.GetDefenceSkillList(), dataPool);
					break;
				case 55:
					result = Serializer.Serialize(instance.GetAssistSkillList(), dataPool);
					break;
				case 56:
					result = Serializer.Serialize(instance.GetPreparingSkillId(), dataPool);
					break;
				case 57:
					result = Serializer.Serialize(instance.GetSkillPreparePercent(), dataPool);
					break;
				case 58:
					result = Serializer.Serialize(instance.GetPerformingSkillId(), dataPool);
					break;
				case 59:
					result = Serializer.Serialize(instance.GetAutoCastingSkill(), dataPool);
					break;
				case 60:
					result = Serializer.Serialize(instance.GetAttackSkillAttackIndex(), dataPool);
					break;
				case 61:
					result = Serializer.Serialize(instance.GetAttackSkillPower(), dataPool);
					break;
				case 62:
					result = Serializer.Serialize(instance.GetAffectingMoveSkillId(), dataPool);
					break;
				case 63:
					result = Serializer.Serialize(instance.GetAffectingDefendSkillId(), dataPool);
					break;
				case 64:
					result = Serializer.Serialize(instance.GetDefendSkillTimePercent(), dataPool);
					break;
				case 65:
					result = Serializer.Serialize(instance.GetWugCount(), dataPool);
					break;
				case 66:
					result = Serializer.Serialize(instance.GetHealInjuryCount(), dataPool);
					break;
				case 67:
					result = Serializer.Serialize(instance.GetHealPoisonCount(), dataPool);
					break;
				case 68:
					result = Serializer.Serialize(instance.GetOtherActionCanUse(), dataPool);
					break;
				case 69:
					result = Serializer.Serialize(instance.GetPreparingOtherAction(), dataPool);
					break;
				case 70:
					result = Serializer.Serialize(instance.GetOtherActionPreparePercent(), dataPool);
					break;
				case 71:
					result = Serializer.Serialize(instance.GetCanSurrender(), dataPool);
					break;
				case 72:
					result = Serializer.Serialize(instance.GetCanUseItem(), dataPool);
					break;
				case 73:
					result = Serializer.Serialize(instance.GetPreparingItem(), dataPool);
					break;
				case 74:
					result = Serializer.Serialize(instance.GetUseItemPreparePercent(), dataPool);
					break;
				case 75:
					result = Serializer.Serialize(instance.GetCombatReserveData(), dataPool);
					break;
				case 76:
					result = Serializer.Serialize(instance.GetBuffCombatStateCollection(), dataPool);
					break;
				case 77:
					result = Serializer.Serialize(instance.GetDebuffCombatStateCollection(), dataPool);
					break;
				case 78:
					result = Serializer.Serialize(instance.GetSpecialCombatStateCollection(), dataPool);
					break;
				case 79:
					result = Serializer.Serialize(instance.GetSkillEffectCollection(), dataPool);
					break;
				case 80:
					result = Serializer.Serialize(instance.GetXiangshuEffectId(), dataPool);
					break;
				case 81:
					result = Serializer.Serialize(instance.GetHazardValue(), dataPool);
					break;
				case 82:
					result = Serializer.Serialize(instance.GetShowEffectList(), dataPool);
					break;
				case 83:
					result = Serializer.Serialize(instance.GetAnimationToLoop(), dataPool);
					break;
				case 84:
					result = Serializer.Serialize(instance.GetAnimationToPlayOnce(), dataPool);
					break;
				case 85:
					result = Serializer.Serialize(instance.GetParticleToPlay(), dataPool);
					break;
				case 86:
					result = Serializer.Serialize(instance.GetParticleToLoop(), dataPool);
					break;
				case 87:
					result = Serializer.Serialize(instance.GetSkillPetAnimation(), dataPool);
					break;
				case 88:
					result = Serializer.Serialize(instance.GetPetParticle(), dataPool);
					break;
				case 89:
					result = Serializer.Serialize(instance.GetAnimationTimeScale(), dataPool);
					break;
				case 90:
					result = Serializer.Serialize(instance.GetAttackOutOfRange(), dataPool);
					break;
				case 91:
					result = Serializer.Serialize(instance.GetAttackSoundToPlay(), dataPool);
					break;
				case 92:
					result = Serializer.Serialize(instance.GetSkillSoundToPlay(), dataPool);
					break;
				case 93:
					result = Serializer.Serialize(instance.GetHitSoundToPlay(), dataPool);
					break;
				case 94:
					result = Serializer.Serialize(instance.GetArmorHitSoundToPlay(), dataPool);
					break;
				case 95:
					result = Serializer.Serialize(instance.GetWhooshSoundToPlay(), dataPool);
					break;
				case 96:
					result = Serializer.Serialize(instance.GetShockSoundToPlay(), dataPool);
					break;
				case 97:
					result = Serializer.Serialize(instance.GetStepSoundToPlay(), dataPool);
					break;
				case 98:
					result = Serializer.Serialize(instance.GetDieSoundToPlay(), dataPool);
					break;
				case 99:
					result = Serializer.Serialize(instance.GetSoundToLoop(), dataPool);
					break;
				case 100:
					result = Serializer.Serialize(instance.GetBossPhase(), dataPool);
					break;
				case 101:
					result = Serializer.Serialize(instance.GetAnimalAttackCount(), dataPool);
					break;
				case 102:
					result = Serializer.Serialize(instance.GetShowTransferInjuryCommand(), dataPool);
					break;
				case 103:
					result = Serializer.Serialize(instance.GetCurrTeammateCommands(), dataPool);
					break;
				case 104:
					result = Serializer.Serialize(instance.GetTeammateCommandCdPercent(), dataPool);
					break;
				case 105:
					result = Serializer.Serialize(instance.GetExecutingTeammateCommand(), dataPool);
					break;
				case 106:
					result = Serializer.Serialize(instance.GetVisible(), dataPool);
					break;
				case 107:
					result = Serializer.Serialize(instance.GetTeammateCommandPreparePercent(), dataPool);
					break;
				case 108:
					result = Serializer.Serialize(instance.GetTeammateCommandTimePercent(), dataPool);
					break;
				case 109:
					result = Serializer.Serialize(instance.GetAttackCommandWeaponKey(), dataPool);
					break;
				case 110:
					result = Serializer.Serialize(instance.GetAttackCommandTrickType(), dataPool);
					break;
				case 111:
					result = Serializer.Serialize(instance.GetDefendCommandSkillId(), dataPool);
					break;
				case 112:
					result = Serializer.Serialize(instance.GetShowEffectCommandIndex(), dataPool);
					break;
				case 113:
					result = Serializer.Serialize(instance.GetAttackCommandSkillId(), dataPool);
					break;
				case 114:
					result = Serializer.Serialize(instance.GetTeammateCommandBanReasons(), dataPool);
					break;
				case 115:
					result = Serializer.Serialize(instance.GetTargetDistance(), dataPool);
					break;
				case 116:
					result = Serializer.Serialize(instance.GetOldInjuryAutoHealCollection(), dataPool);
					break;
				case 117:
					result = Serializer.Serialize(instance.GetMixPoisonAffectedCount(), dataPool);
					break;
				case 118:
					result = Serializer.Serialize(instance.GetParticleToLoopByCombatSkill(), dataPool);
					break;
				case 119:
					result = Serializer.Serialize(instance.GetNeiliAllocationCd(), dataPool);
					break;
				case 120:
					result = Serializer.Serialize(instance.GetProportionDelta(), dataPool);
					break;
				case 121:
					result = Serializer.Serialize(instance.GetMindMarkInfinityCount(), dataPool);
					break;
				case 122:
					result = Serializer.Serialize(instance.GetMindMarkInfinityProgress(), dataPool);
					break;
				case 123:
					result = Serializer.Serialize(instance.GetShowCommandList(), dataPool);
					break;
				case 124:
					result = Serializer.Serialize(instance.GetUnlockPrepareValue(), dataPool);
					break;
				case 125:
					result = Serializer.Serialize(instance.GetRawCreateEffects(), dataPool);
					break;
				case 126:
					result = Serializer.Serialize(instance.GetRawCreateCollection(), dataPool);
					break;
				case 127:
					result = Serializer.Serialize(instance.GetNormalAttackRecovery(), dataPool);
					break;
				case 128:
					result = Serializer.Serialize(instance.GetReserveNormalAttack(), dataPool);
					break;
				case 129:
					result = Serializer.Serialize(instance.GetGangqi(), dataPool);
					break;
				case 130:
					result = Serializer.Serialize(instance.GetGangqiMax(), dataPool);
					break;
				case 131:
					result = Serializer.Serialize(instance.GetMaxTrickCount(), dataPool);
					break;
				case 132:
					result = Serializer.Serialize(instance.GetMobilityLevel(), dataPool);
					break;
				case 133:
					result = Serializer.Serialize(instance.GetTeammateCommandCanUse(), dataPool);
					break;
				case 134:
					result = Serializer.Serialize(instance.GetChangeDistanceDuration(), dataPool);
					break;
				case 135:
					result = Serializer.Serialize(instance.GetAttackRange(), dataPool);
					break;
				case 136:
					result = Serializer.Serialize(instance.GetHappiness(), dataPool);
					break;
				case 137:
					result = Serializer.Serialize(instance.GetSilenceData(), dataPool);
					break;
				case 138:
					result = Serializer.Serialize(instance.GetCombatStateTotalBuffPower(), dataPool);
					break;
				case 139:
					result = Serializer.Serialize(instance.GetHeavyOrBreakInjuryData(), dataPool);
					break;
				case 140:
					result = Serializer.Serialize(instance.GetMoveCd(), dataPool);
					break;
				case 141:
					result = Serializer.Serialize(instance.GetMobilityRecoverSpeed(), dataPool);
					break;
				case 142:
					result = Serializer.Serialize(instance.GetCanUnlockAttack(), dataPool);
					break;
				case 143:
					result = Serializer.Serialize(instance.GetValidItems(), dataPool);
					break;
				case 144:
					result = Serializer.Serialize(instance.GetValidItemAndCounts(), dataPool);
					break;
				default:
				{
					bool flag2 = fieldId >= 145;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
					if (flag2)
					{
						defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(20, 1);
						defaultInterpolatedStringHandler.AppendLiteral("Unsupported fieldId ");
						defaultInterpolatedStringHandler.AppendFormatted<ushort>(fieldId);
						throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
					}
					defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(38, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Not allow to get readonly field data: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(fieldId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				}
			}
			return result;
		}

		// Token: 0x060065B0 RID: 26032 RVA: 0x0039F788 File Offset: 0x0039D988
		public void SetElementField_CombatCharacterDict(int objectId, ushort fieldId, int valueOffset, RawDataPool dataPool, DataContext context)
		{
			CombatCharacter instance;
			bool flag = !this._combatCharacterDict.TryGetValue(objectId, out instance);
			if (flag)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Failed to find element ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(objectId);
				defaultInterpolatedStringHandler.AppendLiteral(" with field ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(fieldId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			switch (fieldId)
			{
			case 0:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(38, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to set readonly field data: ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(fieldId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 1:
			{
				int value = 0;
				Serializer.Deserialize(dataPool, valueOffset, ref value);
				instance.SetBreathValue(value, context);
				break;
			}
			case 2:
			{
				int value2 = 0;
				Serializer.Deserialize(dataPool, valueOffset, ref value2);
				instance.SetStanceValue(value2, context);
				break;
			}
			case 3:
			{
				NeiliAllocation value3 = default(NeiliAllocation);
				Serializer.Deserialize(dataPool, valueOffset, ref value3);
				instance.SetNeiliAllocation(value3, context);
				break;
			}
			case 4:
			{
				NeiliAllocation value4 = default(NeiliAllocation);
				Serializer.Deserialize(dataPool, valueOffset, ref value4);
				instance.SetOriginNeiliAllocation(value4, context);
				break;
			}
			case 5:
			{
				NeiliAllocation value5 = default(NeiliAllocation);
				Serializer.Deserialize(dataPool, valueOffset, ref value5);
				instance.SetNeiliAllocationRecoverProgress(value5, context);
				break;
			}
			case 6:
			{
				short value6 = 0;
				Serializer.Deserialize(dataPool, valueOffset, ref value6);
				instance.SetOldDisorderOfQi(value6, context);
				break;
			}
			case 7:
			{
				sbyte value7 = 0;
				Serializer.Deserialize(dataPool, valueOffset, ref value7);
				instance.SetNeiliType(value7, context);
				break;
			}
			case 8:
			{
				ShowAvoidData value8 = default(ShowAvoidData);
				Serializer.Deserialize(dataPool, valueOffset, ref value8);
				instance.SetAvoidToShow(value8, context);
				break;
			}
			case 9:
			{
				int value9 = 0;
				Serializer.Deserialize(dataPool, valueOffset, ref value9);
				instance.SetCurrentPosition(value9, context);
				break;
			}
			case 10:
			{
				int value10 = 0;
				Serializer.Deserialize(dataPool, valueOffset, ref value10);
				instance.SetDisplayPosition(value10, context);
				break;
			}
			case 11:
			{
				int value11 = 0;
				Serializer.Deserialize(dataPool, valueOffset, ref value11);
				instance.SetMobilityValue(value11, context);
				break;
			}
			case 12:
			{
				sbyte value12 = 0;
				Serializer.Deserialize(dataPool, valueOffset, ref value12);
				instance.SetJumpPrepareProgress(value12, context);
				break;
			}
			case 13:
			{
				short value13 = 0;
				Serializer.Deserialize(dataPool, valueOffset, ref value13);
				instance.SetJumpPreparedDistance(value13, context);
				break;
			}
			case 14:
			{
				short value14 = 0;
				Serializer.Deserialize(dataPool, valueOffset, ref value14);
				instance.SetMobilityLockEffectCount(value14, context);
				break;
			}
			case 15:
			{
				float value15 = 0f;
				Serializer.Deserialize(dataPool, valueOffset, ref value15);
				instance.SetJumpChangeDistanceDuration(value15, context);
				break;
			}
			case 16:
			{
				int value16 = 0;
				Serializer.Deserialize(dataPool, valueOffset, ref value16);
				instance.SetUsingWeaponIndex(value16, context);
				break;
			}
			case 17:
			{
				sbyte[] value17 = instance.GetWeaponTricks();
				Serializer.Deserialize(dataPool, valueOffset, ref value17);
				instance.SetWeaponTricks(value17, context);
				break;
			}
			case 18:
			{
				byte value18 = 0;
				Serializer.Deserialize(dataPool, valueOffset, ref value18);
				instance.SetWeaponTrickIndex(value18, context);
				break;
			}
			case 19:
			{
				ItemKey[] value19 = instance.GetWeapons();
				Serializer.Deserialize(dataPool, valueOffset, ref value19);
				instance.SetWeapons(value19, context);
				break;
			}
			case 20:
			{
				sbyte value20 = 0;
				Serializer.Deserialize(dataPool, valueOffset, ref value20);
				instance.SetAttackingTrickType(value20, context);
				break;
			}
			case 21:
			{
				bool value21 = false;
				Serializer.Deserialize(dataPool, valueOffset, ref value21);
				instance.SetCanAttackOutRange(value21, context);
				break;
			}
			case 22:
			{
				sbyte value22 = 0;
				Serializer.Deserialize(dataPool, valueOffset, ref value22);
				instance.SetChangeTrickProgress(value22, context);
				break;
			}
			case 23:
			{
				short value23 = 0;
				Serializer.Deserialize(dataPool, valueOffset, ref value23);
				instance.SetChangeTrickCount(value23, context);
				break;
			}
			case 24:
			{
				bool value24 = false;
				Serializer.Deserialize(dataPool, valueOffset, ref value24);
				instance.SetCanChangeTrick(value24, context);
				break;
			}
			case 25:
			{
				bool value25 = false;
				Serializer.Deserialize(dataPool, valueOffset, ref value25);
				instance.SetChangingTrick(value25, context);
				break;
			}
			case 26:
			{
				bool value26 = false;
				Serializer.Deserialize(dataPool, valueOffset, ref value26);
				instance.SetChangeTrickAttack(value26, context);
				break;
			}
			case 27:
			{
				bool value27 = false;
				Serializer.Deserialize(dataPool, valueOffset, ref value27);
				instance.SetIsFightBack(value27, context);
				break;
			}
			case 28:
			{
				TrickCollection value28 = instance.GetTricks();
				Serializer.Deserialize(dataPool, valueOffset, ref value28);
				instance.SetTricks(value28, context);
				break;
			}
			case 29:
			{
				Injuries value29 = default(Injuries);
				Serializer.Deserialize(dataPool, valueOffset, ref value29);
				instance.SetInjuries(value29, context);
				break;
			}
			case 30:
			{
				Injuries value30 = default(Injuries);
				Serializer.Deserialize(dataPool, valueOffset, ref value30);
				instance.SetOldInjuries(value30, context);
				break;
			}
			case 31:
			{
				InjuryAutoHealCollection value31 = instance.GetInjuryAutoHealCollection();
				Serializer.Deserialize(dataPool, valueOffset, ref value31);
				instance.SetInjuryAutoHealCollection(value31, context);
				break;
			}
			case 32:
			{
				DamageStepCollection value32 = instance.GetDamageStepCollection();
				Serializer.Deserialize(dataPool, valueOffset, ref value32);
				instance.SetDamageStepCollection(value32, context);
				break;
			}
			case 33:
			{
				int[] value33 = instance.GetOuterDamageValue();
				Serializer.Deserialize(dataPool, valueOffset, ref value33);
				instance.SetOuterDamageValue(value33, context);
				break;
			}
			case 34:
			{
				int[] value34 = instance.GetInnerDamageValue();
				Serializer.Deserialize(dataPool, valueOffset, ref value34);
				instance.SetInnerDamageValue(value34, context);
				break;
			}
			case 35:
			{
				int value35 = 0;
				Serializer.Deserialize(dataPool, valueOffset, ref value35);
				instance.SetMindDamageValue(value35, context);
				break;
			}
			case 36:
			{
				int value36 = 0;
				Serializer.Deserialize(dataPool, valueOffset, ref value36);
				instance.SetFatalDamageValue(value36, context);
				break;
			}
			case 37:
			{
				IntPair[] value37 = instance.GetOuterDamageValueToShow();
				Serializer.Deserialize(dataPool, valueOffset, ref value37);
				instance.SetOuterDamageValueToShow(value37, context);
				break;
			}
			case 38:
			{
				IntPair[] value38 = instance.GetInnerDamageValueToShow();
				Serializer.Deserialize(dataPool, valueOffset, ref value38);
				instance.SetInnerDamageValueToShow(value38, context);
				break;
			}
			case 39:
			{
				int value39 = 0;
				Serializer.Deserialize(dataPool, valueOffset, ref value39);
				instance.SetMindDamageValueToShow(value39, context);
				break;
			}
			case 40:
			{
				int value40 = 0;
				Serializer.Deserialize(dataPool, valueOffset, ref value40);
				instance.SetFatalDamageValueToShow(value40, context);
				break;
			}
			case 41:
			{
				byte[] value41 = instance.GetFlawCount();
				Serializer.Deserialize(dataPool, valueOffset, ref value41);
				instance.SetFlawCount(value41, context);
				break;
			}
			case 42:
			{
				FlawOrAcupointCollection value42 = instance.GetFlawCollection();
				Serializer.Deserialize(dataPool, valueOffset, ref value42);
				instance.SetFlawCollection(value42, context);
				break;
			}
			case 43:
			{
				byte[] value43 = instance.GetAcupointCount();
				Serializer.Deserialize(dataPool, valueOffset, ref value43);
				instance.SetAcupointCount(value43, context);
				break;
			}
			case 44:
			{
				FlawOrAcupointCollection value44 = instance.GetAcupointCollection();
				Serializer.Deserialize(dataPool, valueOffset, ref value44);
				instance.SetAcupointCollection(value44, context);
				break;
			}
			case 45:
			{
				MindMarkList value45 = instance.GetMindMarkTime();
				Serializer.Deserialize(dataPool, valueOffset, ref value45);
				instance.SetMindMarkTime(value45, context);
				break;
			}
			case 46:
			{
				PoisonInts value46 = default(PoisonInts);
				Serializer.Deserialize(dataPool, valueOffset, ref value46);
				instance.SetPoison(ref value46, context);
				break;
			}
			case 47:
			{
				PoisonInts value47 = default(PoisonInts);
				Serializer.Deserialize(dataPool, valueOffset, ref value47);
				instance.SetOldPoison(ref value47, context);
				break;
			}
			case 48:
			{
				PoisonInts value48 = default(PoisonInts);
				Serializer.Deserialize(dataPool, valueOffset, ref value48);
				instance.SetPoisonResist(ref value48, context);
				break;
			}
			case 49:
			{
				PoisonsAndLevels value49 = default(PoisonsAndLevels);
				Serializer.Deserialize(dataPool, valueOffset, ref value49);
				instance.SetNewPoisonsToShow(ref value49, context);
				break;
			}
			case 50:
			{
				DefeatMarkCollection value50 = instance.GetDefeatMarkCollection();
				Serializer.Deserialize(dataPool, valueOffset, ref value50);
				instance.SetDefeatMarkCollection(value50, context);
				break;
			}
			case 51:
			{
				List<short> value51 = instance.GetNeigongList();
				Serializer.Deserialize(dataPool, valueOffset, ref value51);
				instance.SetNeigongList(value51, context);
				break;
			}
			case 52:
			{
				List<short> value52 = instance.GetAttackSkillList();
				Serializer.Deserialize(dataPool, valueOffset, ref value52);
				instance.SetAttackSkillList(value52, context);
				break;
			}
			case 53:
			{
				List<short> value53 = instance.GetAgileSkillList();
				Serializer.Deserialize(dataPool, valueOffset, ref value53);
				instance.SetAgileSkillList(value53, context);
				break;
			}
			case 54:
			{
				List<short> value54 = instance.GetDefenceSkillList();
				Serializer.Deserialize(dataPool, valueOffset, ref value54);
				instance.SetDefenceSkillList(value54, context);
				break;
			}
			case 55:
			{
				List<short> value55 = instance.GetAssistSkillList();
				Serializer.Deserialize(dataPool, valueOffset, ref value55);
				instance.SetAssistSkillList(value55, context);
				break;
			}
			case 56:
			{
				short value56 = 0;
				Serializer.Deserialize(dataPool, valueOffset, ref value56);
				instance.SetPreparingSkillId(value56, context);
				break;
			}
			case 57:
			{
				byte value57 = 0;
				Serializer.Deserialize(dataPool, valueOffset, ref value57);
				instance.SetSkillPreparePercent(value57, context);
				break;
			}
			case 58:
			{
				short value58 = 0;
				Serializer.Deserialize(dataPool, valueOffset, ref value58);
				instance.SetPerformingSkillId(value58, context);
				break;
			}
			case 59:
			{
				bool value59 = false;
				Serializer.Deserialize(dataPool, valueOffset, ref value59);
				instance.SetAutoCastingSkill(value59, context);
				break;
			}
			case 60:
			{
				byte value60 = 0;
				Serializer.Deserialize(dataPool, valueOffset, ref value60);
				instance.SetAttackSkillAttackIndex(value60, context);
				break;
			}
			case 61:
			{
				byte value61 = 0;
				Serializer.Deserialize(dataPool, valueOffset, ref value61);
				instance.SetAttackSkillPower(value61, context);
				break;
			}
			case 62:
			{
				short value62 = 0;
				Serializer.Deserialize(dataPool, valueOffset, ref value62);
				instance.SetAffectingMoveSkillId(value62, context);
				break;
			}
			case 63:
			{
				short value63 = 0;
				Serializer.Deserialize(dataPool, valueOffset, ref value63);
				instance.SetAffectingDefendSkillId(value63, context);
				break;
			}
			case 64:
			{
				byte value64 = 0;
				Serializer.Deserialize(dataPool, valueOffset, ref value64);
				instance.SetDefendSkillTimePercent(value64, context);
				break;
			}
			case 65:
			{
				short value65 = 0;
				Serializer.Deserialize(dataPool, valueOffset, ref value65);
				instance.SetWugCount(value65, context);
				break;
			}
			case 66:
			{
				byte value66 = 0;
				Serializer.Deserialize(dataPool, valueOffset, ref value66);
				instance.SetHealInjuryCount(value66, context);
				break;
			}
			case 67:
			{
				byte value67 = 0;
				Serializer.Deserialize(dataPool, valueOffset, ref value67);
				instance.SetHealPoisonCount(value67, context);
				break;
			}
			case 68:
			{
				bool[] value68 = instance.GetOtherActionCanUse();
				Serializer.Deserialize(dataPool, valueOffset, ref value68);
				instance.SetOtherActionCanUse(value68, context);
				break;
			}
			case 69:
			{
				sbyte value69 = 0;
				Serializer.Deserialize(dataPool, valueOffset, ref value69);
				instance.SetPreparingOtherAction(value69, context);
				break;
			}
			case 70:
			{
				byte value70 = 0;
				Serializer.Deserialize(dataPool, valueOffset, ref value70);
				instance.SetOtherActionPreparePercent(value70, context);
				break;
			}
			case 71:
			{
				bool value71 = false;
				Serializer.Deserialize(dataPool, valueOffset, ref value71);
				instance.SetCanSurrender(value71, context);
				break;
			}
			case 72:
			{
				bool value72 = false;
				Serializer.Deserialize(dataPool, valueOffset, ref value72);
				instance.SetCanUseItem(value72, context);
				break;
			}
			case 73:
			{
				ItemKey value73 = default(ItemKey);
				Serializer.Deserialize(dataPool, valueOffset, ref value73);
				instance.SetPreparingItem(value73, context);
				break;
			}
			case 74:
			{
				byte value74 = 0;
				Serializer.Deserialize(dataPool, valueOffset, ref value74);
				instance.SetUseItemPreparePercent(value74, context);
				break;
			}
			case 75:
			{
				CombatReserveData value75 = default(CombatReserveData);
				Serializer.Deserialize(dataPool, valueOffset, ref value75);
				instance.SetCombatReserveData(value75, context);
				break;
			}
			case 76:
			{
				CombatStateCollection value76 = instance.GetBuffCombatStateCollection();
				Serializer.Deserialize(dataPool, valueOffset, ref value76);
				instance.SetBuffCombatStateCollection(value76, context);
				break;
			}
			case 77:
			{
				CombatStateCollection value77 = instance.GetDebuffCombatStateCollection();
				Serializer.Deserialize(dataPool, valueOffset, ref value77);
				instance.SetDebuffCombatStateCollection(value77, context);
				break;
			}
			case 78:
			{
				CombatStateCollection value78 = instance.GetSpecialCombatStateCollection();
				Serializer.Deserialize(dataPool, valueOffset, ref value78);
				instance.SetSpecialCombatStateCollection(value78, context);
				break;
			}
			case 79:
			{
				SkillEffectCollection value79 = instance.GetSkillEffectCollection();
				Serializer.Deserialize(dataPool, valueOffset, ref value79);
				instance.SetSkillEffectCollection(value79, context);
				break;
			}
			case 80:
			{
				short value80 = 0;
				Serializer.Deserialize(dataPool, valueOffset, ref value80);
				instance.SetXiangshuEffectId(value80, context);
				break;
			}
			case 81:
			{
				int value81 = 0;
				Serializer.Deserialize(dataPool, valueOffset, ref value81);
				instance.SetHazardValue(value81, context);
				break;
			}
			case 82:
			{
				ShowSpecialEffectCollection value82 = instance.GetShowEffectList();
				Serializer.Deserialize(dataPool, valueOffset, ref value82);
				instance.SetShowEffectList(value82, context);
				break;
			}
			case 83:
			{
				string value83 = null;
				Serializer.Deserialize(dataPool, valueOffset, ref value83);
				instance.SetAnimationToLoop(value83, context);
				break;
			}
			case 84:
			{
				string value84 = null;
				Serializer.Deserialize(dataPool, valueOffset, ref value84);
				instance.SetAnimationToPlayOnce(value84, context);
				break;
			}
			case 85:
			{
				string value85 = null;
				Serializer.Deserialize(dataPool, valueOffset, ref value85);
				instance.SetParticleToPlay(value85, context);
				break;
			}
			case 86:
			{
				string value86 = null;
				Serializer.Deserialize(dataPool, valueOffset, ref value86);
				instance.SetParticleToLoop(value86, context);
				break;
			}
			case 87:
			{
				string value87 = null;
				Serializer.Deserialize(dataPool, valueOffset, ref value87);
				instance.SetSkillPetAnimation(value87, context);
				break;
			}
			case 88:
			{
				string value88 = null;
				Serializer.Deserialize(dataPool, valueOffset, ref value88);
				instance.SetPetParticle(value88, context);
				break;
			}
			case 89:
			{
				float value89 = 0f;
				Serializer.Deserialize(dataPool, valueOffset, ref value89);
				instance.SetAnimationTimeScale(value89, context);
				break;
			}
			case 90:
			{
				bool value90 = false;
				Serializer.Deserialize(dataPool, valueOffset, ref value90);
				instance.SetAttackOutOfRange(value90, context);
				break;
			}
			case 91:
			{
				string value91 = null;
				Serializer.Deserialize(dataPool, valueOffset, ref value91);
				instance.SetAttackSoundToPlay(value91, context);
				break;
			}
			case 92:
			{
				string value92 = null;
				Serializer.Deserialize(dataPool, valueOffset, ref value92);
				instance.SetSkillSoundToPlay(value92, context);
				break;
			}
			case 93:
			{
				string value93 = null;
				Serializer.Deserialize(dataPool, valueOffset, ref value93);
				instance.SetHitSoundToPlay(value93, context);
				break;
			}
			case 94:
			{
				string value94 = null;
				Serializer.Deserialize(dataPool, valueOffset, ref value94);
				instance.SetArmorHitSoundToPlay(value94, context);
				break;
			}
			case 95:
			{
				string value95 = null;
				Serializer.Deserialize(dataPool, valueOffset, ref value95);
				instance.SetWhooshSoundToPlay(value95, context);
				break;
			}
			case 96:
			{
				string value96 = null;
				Serializer.Deserialize(dataPool, valueOffset, ref value96);
				instance.SetShockSoundToPlay(value96, context);
				break;
			}
			case 97:
			{
				string value97 = null;
				Serializer.Deserialize(dataPool, valueOffset, ref value97);
				instance.SetStepSoundToPlay(value97, context);
				break;
			}
			case 98:
			{
				string value98 = null;
				Serializer.Deserialize(dataPool, valueOffset, ref value98);
				instance.SetDieSoundToPlay(value98, context);
				break;
			}
			case 99:
			{
				string value99 = null;
				Serializer.Deserialize(dataPool, valueOffset, ref value99);
				instance.SetSoundToLoop(value99, context);
				break;
			}
			case 100:
			{
				sbyte value100 = 0;
				Serializer.Deserialize(dataPool, valueOffset, ref value100);
				instance.SetBossPhase(value100, context);
				break;
			}
			case 101:
			{
				sbyte value101 = 0;
				Serializer.Deserialize(dataPool, valueOffset, ref value101);
				instance.SetAnimalAttackCount(value101, context);
				break;
			}
			case 102:
			{
				bool value102 = false;
				Serializer.Deserialize(dataPool, valueOffset, ref value102);
				instance.SetShowTransferInjuryCommand(value102, context);
				break;
			}
			case 103:
			{
				List<sbyte> value103 = instance.GetCurrTeammateCommands();
				Serializer.Deserialize(dataPool, valueOffset, ref value103);
				instance.SetCurrTeammateCommands(value103, context);
				break;
			}
			case 104:
			{
				List<byte> value104 = instance.GetTeammateCommandCdPercent();
				Serializer.Deserialize(dataPool, valueOffset, ref value104);
				instance.SetTeammateCommandCdPercent(value104, context);
				break;
			}
			case 105:
			{
				sbyte value105 = 0;
				Serializer.Deserialize(dataPool, valueOffset, ref value105);
				instance.SetExecutingTeammateCommand(value105, context);
				break;
			}
			case 106:
			{
				bool value106 = false;
				Serializer.Deserialize(dataPool, valueOffset, ref value106);
				instance.SetVisible(value106, context);
				break;
			}
			case 107:
			{
				byte value107 = 0;
				Serializer.Deserialize(dataPool, valueOffset, ref value107);
				instance.SetTeammateCommandPreparePercent(value107, context);
				break;
			}
			case 108:
			{
				byte value108 = 0;
				Serializer.Deserialize(dataPool, valueOffset, ref value108);
				instance.SetTeammateCommandTimePercent(value108, context);
				break;
			}
			case 109:
			{
				ItemKey value109 = default(ItemKey);
				Serializer.Deserialize(dataPool, valueOffset, ref value109);
				instance.SetAttackCommandWeaponKey(value109, context);
				break;
			}
			case 110:
			{
				sbyte value110 = 0;
				Serializer.Deserialize(dataPool, valueOffset, ref value110);
				instance.SetAttackCommandTrickType(value110, context);
				break;
			}
			case 111:
			{
				short value111 = 0;
				Serializer.Deserialize(dataPool, valueOffset, ref value111);
				instance.SetDefendCommandSkillId(value111, context);
				break;
			}
			case 112:
			{
				sbyte value112 = 0;
				Serializer.Deserialize(dataPool, valueOffset, ref value112);
				instance.SetShowEffectCommandIndex(value112, context);
				break;
			}
			case 113:
			{
				short value113 = 0;
				Serializer.Deserialize(dataPool, valueOffset, ref value113);
				instance.SetAttackCommandSkillId(value113, context);
				break;
			}
			case 114:
			{
				List<SByteList> value114 = instance.GetTeammateCommandBanReasons();
				Serializer.Deserialize(dataPool, valueOffset, ref value114);
				instance.SetTeammateCommandBanReasons(value114, context);
				break;
			}
			case 115:
			{
				short value115 = 0;
				Serializer.Deserialize(dataPool, valueOffset, ref value115);
				instance.SetTargetDistance(value115, context);
				break;
			}
			case 116:
			{
				InjuryAutoHealCollection value116 = instance.GetOldInjuryAutoHealCollection();
				Serializer.Deserialize(dataPool, valueOffset, ref value116);
				instance.SetOldInjuryAutoHealCollection(value116, context);
				break;
			}
			case 117:
			{
				MixPoisonAffectedCountCollection value117 = instance.GetMixPoisonAffectedCount();
				Serializer.Deserialize(dataPool, valueOffset, ref value117);
				instance.SetMixPoisonAffectedCount(value117, context);
				break;
			}
			case 118:
			{
				string value118 = null;
				Serializer.Deserialize(dataPool, valueOffset, ref value118);
				instance.SetParticleToLoopByCombatSkill(value118, context);
				break;
			}
			case 119:
			{
				SilenceFrameData value119 = default(SilenceFrameData);
				Serializer.Deserialize(dataPool, valueOffset, ref value119);
				instance.SetNeiliAllocationCd(value119, context);
				break;
			}
			case 120:
			{
				NeiliProportionOfFiveElements value120 = default(NeiliProportionOfFiveElements);
				Serializer.Deserialize(dataPool, valueOffset, ref value120);
				instance.SetProportionDelta(value120, context);
				break;
			}
			case 121:
			{
				int value121 = 0;
				Serializer.Deserialize(dataPool, valueOffset, ref value121);
				instance.SetMindMarkInfinityCount(value121, context);
				break;
			}
			case 122:
			{
				int value122 = 0;
				Serializer.Deserialize(dataPool, valueOffset, ref value122);
				instance.SetMindMarkInfinityProgress(value122, context);
				break;
			}
			case 123:
			{
				List<TeammateCommandDisplayData> value123 = instance.GetShowCommandList();
				Serializer.Deserialize(dataPool, valueOffset, ref value123);
				instance.SetShowCommandList(value123, context);
				break;
			}
			case 124:
			{
				List<int> value124 = instance.GetUnlockPrepareValue();
				Serializer.Deserialize(dataPool, valueOffset, ref value124);
				instance.SetUnlockPrepareValue(value124, context);
				break;
			}
			case 125:
			{
				List<int> value125 = instance.GetRawCreateEffects();
				Serializer.Deserialize(dataPool, valueOffset, ref value125);
				instance.SetRawCreateEffects(value125, context);
				break;
			}
			case 126:
			{
				RawCreateCollection value126 = instance.GetRawCreateCollection();
				Serializer.Deserialize(dataPool, valueOffset, ref value126);
				instance.SetRawCreateCollection(value126, context);
				break;
			}
			case 127:
			{
				SilenceFrameData value127 = default(SilenceFrameData);
				Serializer.Deserialize(dataPool, valueOffset, ref value127);
				instance.SetNormalAttackRecovery(value127, context);
				break;
			}
			case 128:
			{
				bool value128 = false;
				Serializer.Deserialize(dataPool, valueOffset, ref value128);
				instance.SetReserveNormalAttack(value128, context);
				break;
			}
			case 129:
			{
				int value129 = 0;
				Serializer.Deserialize(dataPool, valueOffset, ref value129);
				instance.SetGangqi(value129, context);
				break;
			}
			case 130:
			{
				int value130 = 0;
				Serializer.Deserialize(dataPool, valueOffset, ref value130);
				instance.SetGangqiMax(value130, context);
				break;
			}
			default:
			{
				bool flag2 = fieldId >= 145;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				if (flag2)
				{
					defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(20, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported fieldId ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(fieldId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				bool flag3 = fieldId >= 145;
				if (flag3)
				{
					defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(38, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Not allow to set readonly field data: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(fieldId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to set cache field data: ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(fieldId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			}
		}

		// Token: 0x060065B1 RID: 26033 RVA: 0x003A0BA8 File Offset: 0x0039EDA8
		private unsafe int CheckModified_CombatCharacterDict(int objectId, ushort fieldId, RawDataPool dataPool)
		{
			CombatCharacter instance;
			bool flag = !this._combatCharacterDict.TryGetValue(objectId, out instance);
			int result;
			if (flag)
			{
				result = -1;
			}
			else
			{
				bool flag2 = fieldId >= 145;
				if (flag2)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(40, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Not allow to check readonly field data: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(fieldId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				bool flag3 = !this._dataStatesCombatCharacterDict.IsModified(instance.DataStatesOffset, (int)fieldId);
				if (flag3)
				{
					result = -1;
				}
				else
				{
					this._dataStatesCombatCharacterDict.ResetModified(instance.DataStatesOffset, (int)fieldId);
					switch (fieldId)
					{
					case 0:
						result = Serializer.Serialize(instance.GetId(), dataPool);
						break;
					case 1:
						result = Serializer.Serialize(instance.GetBreathValue(), dataPool);
						break;
					case 2:
						result = Serializer.Serialize(instance.GetStanceValue(), dataPool);
						break;
					case 3:
						result = Serializer.Serialize(instance.GetNeiliAllocation(), dataPool);
						break;
					case 4:
						result = Serializer.Serialize(instance.GetOriginNeiliAllocation(), dataPool);
						break;
					case 5:
						result = Serializer.Serialize(instance.GetNeiliAllocationRecoverProgress(), dataPool);
						break;
					case 6:
						result = Serializer.Serialize(instance.GetOldDisorderOfQi(), dataPool);
						break;
					case 7:
						result = Serializer.Serialize(instance.GetNeiliType(), dataPool);
						break;
					case 8:
						result = Serializer.Serialize(instance.GetAvoidToShow(), dataPool);
						break;
					case 9:
						result = Serializer.Serialize(instance.GetCurrentPosition(), dataPool);
						break;
					case 10:
						result = Serializer.Serialize(instance.GetDisplayPosition(), dataPool);
						break;
					case 11:
						result = Serializer.Serialize(instance.GetMobilityValue(), dataPool);
						break;
					case 12:
						result = Serializer.Serialize(instance.GetJumpPrepareProgress(), dataPool);
						break;
					case 13:
						result = Serializer.Serialize(instance.GetJumpPreparedDistance(), dataPool);
						break;
					case 14:
						result = Serializer.Serialize(instance.GetMobilityLockEffectCount(), dataPool);
						break;
					case 15:
						result = Serializer.Serialize(instance.GetJumpChangeDistanceDuration(), dataPool);
						break;
					case 16:
						result = Serializer.Serialize(instance.GetUsingWeaponIndex(), dataPool);
						break;
					case 17:
						result = Serializer.Serialize(instance.GetWeaponTricks(), dataPool);
						break;
					case 18:
						result = Serializer.Serialize(instance.GetWeaponTrickIndex(), dataPool);
						break;
					case 19:
						result = Serializer.Serialize(instance.GetWeapons(), dataPool);
						break;
					case 20:
						result = Serializer.Serialize(instance.GetAttackingTrickType(), dataPool);
						break;
					case 21:
						result = Serializer.Serialize(instance.GetCanAttackOutRange(), dataPool);
						break;
					case 22:
						result = Serializer.Serialize(instance.GetChangeTrickProgress(), dataPool);
						break;
					case 23:
						result = Serializer.Serialize(instance.GetChangeTrickCount(), dataPool);
						break;
					case 24:
						result = Serializer.Serialize(instance.GetCanChangeTrick(), dataPool);
						break;
					case 25:
						result = Serializer.Serialize(instance.GetChangingTrick(), dataPool);
						break;
					case 26:
						result = Serializer.Serialize(instance.GetChangeTrickAttack(), dataPool);
						break;
					case 27:
						result = Serializer.Serialize(instance.GetIsFightBack(), dataPool);
						break;
					case 28:
						result = Serializer.Serialize(instance.GetTricks(), dataPool);
						break;
					case 29:
						result = Serializer.Serialize(instance.GetInjuries(), dataPool);
						break;
					case 30:
						result = Serializer.Serialize(instance.GetOldInjuries(), dataPool);
						break;
					case 31:
						result = Serializer.Serialize(instance.GetInjuryAutoHealCollection(), dataPool);
						break;
					case 32:
						result = Serializer.Serialize(instance.GetDamageStepCollection(), dataPool);
						break;
					case 33:
						result = Serializer.Serialize(instance.GetOuterDamageValue(), dataPool);
						break;
					case 34:
						result = Serializer.Serialize(instance.GetInnerDamageValue(), dataPool);
						break;
					case 35:
						result = Serializer.Serialize(instance.GetMindDamageValue(), dataPool);
						break;
					case 36:
						result = Serializer.Serialize(instance.GetFatalDamageValue(), dataPool);
						break;
					case 37:
						result = Serializer.Serialize(instance.GetOuterDamageValueToShow(), dataPool);
						break;
					case 38:
						result = Serializer.Serialize(instance.GetInnerDamageValueToShow(), dataPool);
						break;
					case 39:
						result = Serializer.Serialize(instance.GetMindDamageValueToShow(), dataPool);
						break;
					case 40:
						result = Serializer.Serialize(instance.GetFatalDamageValueToShow(), dataPool);
						break;
					case 41:
						result = Serializer.Serialize(instance.GetFlawCount(), dataPool);
						break;
					case 42:
						result = Serializer.Serialize(instance.GetFlawCollection(), dataPool);
						break;
					case 43:
						result = Serializer.Serialize(instance.GetAcupointCount(), dataPool);
						break;
					case 44:
						result = Serializer.Serialize(instance.GetAcupointCollection(), dataPool);
						break;
					case 45:
						result = Serializer.Serialize(instance.GetMindMarkTime(), dataPool);
						break;
					case 46:
						result = Serializer.Serialize(*instance.GetPoison(), dataPool);
						break;
					case 47:
						result = Serializer.Serialize(*instance.GetOldPoison(), dataPool);
						break;
					case 48:
						result = Serializer.Serialize(*instance.GetPoisonResist(), dataPool);
						break;
					case 49:
						result = Serializer.Serialize(*instance.GetNewPoisonsToShow(), dataPool);
						break;
					case 50:
						result = Serializer.Serialize(instance.GetDefeatMarkCollection(), dataPool);
						break;
					case 51:
						result = Serializer.Serialize(instance.GetNeigongList(), dataPool);
						break;
					case 52:
						result = Serializer.Serialize(instance.GetAttackSkillList(), dataPool);
						break;
					case 53:
						result = Serializer.Serialize(instance.GetAgileSkillList(), dataPool);
						break;
					case 54:
						result = Serializer.Serialize(instance.GetDefenceSkillList(), dataPool);
						break;
					case 55:
						result = Serializer.Serialize(instance.GetAssistSkillList(), dataPool);
						break;
					case 56:
						result = Serializer.Serialize(instance.GetPreparingSkillId(), dataPool);
						break;
					case 57:
						result = Serializer.Serialize(instance.GetSkillPreparePercent(), dataPool);
						break;
					case 58:
						result = Serializer.Serialize(instance.GetPerformingSkillId(), dataPool);
						break;
					case 59:
						result = Serializer.Serialize(instance.GetAutoCastingSkill(), dataPool);
						break;
					case 60:
						result = Serializer.Serialize(instance.GetAttackSkillAttackIndex(), dataPool);
						break;
					case 61:
						result = Serializer.Serialize(instance.GetAttackSkillPower(), dataPool);
						break;
					case 62:
						result = Serializer.Serialize(instance.GetAffectingMoveSkillId(), dataPool);
						break;
					case 63:
						result = Serializer.Serialize(instance.GetAffectingDefendSkillId(), dataPool);
						break;
					case 64:
						result = Serializer.Serialize(instance.GetDefendSkillTimePercent(), dataPool);
						break;
					case 65:
						result = Serializer.Serialize(instance.GetWugCount(), dataPool);
						break;
					case 66:
						result = Serializer.Serialize(instance.GetHealInjuryCount(), dataPool);
						break;
					case 67:
						result = Serializer.Serialize(instance.GetHealPoisonCount(), dataPool);
						break;
					case 68:
						result = Serializer.Serialize(instance.GetOtherActionCanUse(), dataPool);
						break;
					case 69:
						result = Serializer.Serialize(instance.GetPreparingOtherAction(), dataPool);
						break;
					case 70:
						result = Serializer.Serialize(instance.GetOtherActionPreparePercent(), dataPool);
						break;
					case 71:
						result = Serializer.Serialize(instance.GetCanSurrender(), dataPool);
						break;
					case 72:
						result = Serializer.Serialize(instance.GetCanUseItem(), dataPool);
						break;
					case 73:
						result = Serializer.Serialize(instance.GetPreparingItem(), dataPool);
						break;
					case 74:
						result = Serializer.Serialize(instance.GetUseItemPreparePercent(), dataPool);
						break;
					case 75:
						result = Serializer.Serialize(instance.GetCombatReserveData(), dataPool);
						break;
					case 76:
						result = Serializer.Serialize(instance.GetBuffCombatStateCollection(), dataPool);
						break;
					case 77:
						result = Serializer.Serialize(instance.GetDebuffCombatStateCollection(), dataPool);
						break;
					case 78:
						result = Serializer.Serialize(instance.GetSpecialCombatStateCollection(), dataPool);
						break;
					case 79:
						result = Serializer.Serialize(instance.GetSkillEffectCollection(), dataPool);
						break;
					case 80:
						result = Serializer.Serialize(instance.GetXiangshuEffectId(), dataPool);
						break;
					case 81:
						result = Serializer.Serialize(instance.GetHazardValue(), dataPool);
						break;
					case 82:
						result = Serializer.Serialize(instance.GetShowEffectList(), dataPool);
						break;
					case 83:
						result = Serializer.Serialize(instance.GetAnimationToLoop(), dataPool);
						break;
					case 84:
						result = Serializer.Serialize(instance.GetAnimationToPlayOnce(), dataPool);
						break;
					case 85:
						result = Serializer.Serialize(instance.GetParticleToPlay(), dataPool);
						break;
					case 86:
						result = Serializer.Serialize(instance.GetParticleToLoop(), dataPool);
						break;
					case 87:
						result = Serializer.Serialize(instance.GetSkillPetAnimation(), dataPool);
						break;
					case 88:
						result = Serializer.Serialize(instance.GetPetParticle(), dataPool);
						break;
					case 89:
						result = Serializer.Serialize(instance.GetAnimationTimeScale(), dataPool);
						break;
					case 90:
						result = Serializer.Serialize(instance.GetAttackOutOfRange(), dataPool);
						break;
					case 91:
						result = Serializer.Serialize(instance.GetAttackSoundToPlay(), dataPool);
						break;
					case 92:
						result = Serializer.Serialize(instance.GetSkillSoundToPlay(), dataPool);
						break;
					case 93:
						result = Serializer.Serialize(instance.GetHitSoundToPlay(), dataPool);
						break;
					case 94:
						result = Serializer.Serialize(instance.GetArmorHitSoundToPlay(), dataPool);
						break;
					case 95:
						result = Serializer.Serialize(instance.GetWhooshSoundToPlay(), dataPool);
						break;
					case 96:
						result = Serializer.Serialize(instance.GetShockSoundToPlay(), dataPool);
						break;
					case 97:
						result = Serializer.Serialize(instance.GetStepSoundToPlay(), dataPool);
						break;
					case 98:
						result = Serializer.Serialize(instance.GetDieSoundToPlay(), dataPool);
						break;
					case 99:
						result = Serializer.Serialize(instance.GetSoundToLoop(), dataPool);
						break;
					case 100:
						result = Serializer.Serialize(instance.GetBossPhase(), dataPool);
						break;
					case 101:
						result = Serializer.Serialize(instance.GetAnimalAttackCount(), dataPool);
						break;
					case 102:
						result = Serializer.Serialize(instance.GetShowTransferInjuryCommand(), dataPool);
						break;
					case 103:
						result = Serializer.Serialize(instance.GetCurrTeammateCommands(), dataPool);
						break;
					case 104:
						result = Serializer.Serialize(instance.GetTeammateCommandCdPercent(), dataPool);
						break;
					case 105:
						result = Serializer.Serialize(instance.GetExecutingTeammateCommand(), dataPool);
						break;
					case 106:
						result = Serializer.Serialize(instance.GetVisible(), dataPool);
						break;
					case 107:
						result = Serializer.Serialize(instance.GetTeammateCommandPreparePercent(), dataPool);
						break;
					case 108:
						result = Serializer.Serialize(instance.GetTeammateCommandTimePercent(), dataPool);
						break;
					case 109:
						result = Serializer.Serialize(instance.GetAttackCommandWeaponKey(), dataPool);
						break;
					case 110:
						result = Serializer.Serialize(instance.GetAttackCommandTrickType(), dataPool);
						break;
					case 111:
						result = Serializer.Serialize(instance.GetDefendCommandSkillId(), dataPool);
						break;
					case 112:
						result = Serializer.Serialize(instance.GetShowEffectCommandIndex(), dataPool);
						break;
					case 113:
						result = Serializer.Serialize(instance.GetAttackCommandSkillId(), dataPool);
						break;
					case 114:
						result = Serializer.Serialize(instance.GetTeammateCommandBanReasons(), dataPool);
						break;
					case 115:
						result = Serializer.Serialize(instance.GetTargetDistance(), dataPool);
						break;
					case 116:
						result = Serializer.Serialize(instance.GetOldInjuryAutoHealCollection(), dataPool);
						break;
					case 117:
						result = Serializer.Serialize(instance.GetMixPoisonAffectedCount(), dataPool);
						break;
					case 118:
						result = Serializer.Serialize(instance.GetParticleToLoopByCombatSkill(), dataPool);
						break;
					case 119:
						result = Serializer.Serialize(instance.GetNeiliAllocationCd(), dataPool);
						break;
					case 120:
						result = Serializer.Serialize(instance.GetProportionDelta(), dataPool);
						break;
					case 121:
						result = Serializer.Serialize(instance.GetMindMarkInfinityCount(), dataPool);
						break;
					case 122:
						result = Serializer.Serialize(instance.GetMindMarkInfinityProgress(), dataPool);
						break;
					case 123:
						result = Serializer.Serialize(instance.GetShowCommandList(), dataPool);
						break;
					case 124:
						result = Serializer.Serialize(instance.GetUnlockPrepareValue(), dataPool);
						break;
					case 125:
						result = Serializer.Serialize(instance.GetRawCreateEffects(), dataPool);
						break;
					case 126:
						result = Serializer.Serialize(instance.GetRawCreateCollection(), dataPool);
						break;
					case 127:
						result = Serializer.Serialize(instance.GetNormalAttackRecovery(), dataPool);
						break;
					case 128:
						result = Serializer.Serialize(instance.GetReserveNormalAttack(), dataPool);
						break;
					case 129:
						result = Serializer.Serialize(instance.GetGangqi(), dataPool);
						break;
					case 130:
						result = Serializer.Serialize(instance.GetGangqiMax(), dataPool);
						break;
					case 131:
						result = Serializer.Serialize(instance.GetMaxTrickCount(), dataPool);
						break;
					case 132:
						result = Serializer.Serialize(instance.GetMobilityLevel(), dataPool);
						break;
					case 133:
						result = Serializer.Serialize(instance.GetTeammateCommandCanUse(), dataPool);
						break;
					case 134:
						result = Serializer.Serialize(instance.GetChangeDistanceDuration(), dataPool);
						break;
					case 135:
						result = Serializer.Serialize(instance.GetAttackRange(), dataPool);
						break;
					case 136:
						result = Serializer.Serialize(instance.GetHappiness(), dataPool);
						break;
					case 137:
						result = Serializer.Serialize(instance.GetSilenceData(), dataPool);
						break;
					case 138:
						result = Serializer.Serialize(instance.GetCombatStateTotalBuffPower(), dataPool);
						break;
					case 139:
						result = Serializer.Serialize(instance.GetHeavyOrBreakInjuryData(), dataPool);
						break;
					case 140:
						result = Serializer.Serialize(instance.GetMoveCd(), dataPool);
						break;
					case 141:
						result = Serializer.Serialize(instance.GetMobilityRecoverSpeed(), dataPool);
						break;
					case 142:
						result = Serializer.Serialize(instance.GetCanUnlockAttack(), dataPool);
						break;
					case 143:
						result = Serializer.Serialize(instance.GetValidItems(), dataPool);
						break;
					case 144:
						result = Serializer.Serialize(instance.GetValidItemAndCounts(), dataPool);
						break;
					default:
					{
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(20, 1);
						defaultInterpolatedStringHandler.AppendLiteral("Unsupported fieldId ");
						defaultInterpolatedStringHandler.AppendFormatted<ushort>(fieldId);
						throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
					}
					}
				}
			}
			return result;
		}

		// Token: 0x060065B2 RID: 26034 RVA: 0x003A18FC File Offset: 0x0039FAFC
		private void ResetModifiedWrapper_CombatCharacterDict(int objectId, ushort fieldId)
		{
			CombatCharacter instance;
			bool flag = !this._combatCharacterDict.TryGetValue(objectId, out instance);
			if (!flag)
			{
				bool flag2 = fieldId >= 145;
				if (flag2)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(62, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Not allow to reset modification state of readonly field data: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(fieldId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				bool flag3 = !this._dataStatesCombatCharacterDict.IsModified(instance.DataStatesOffset, (int)fieldId);
				if (!flag3)
				{
					this._dataStatesCombatCharacterDict.ResetModified(instance.DataStatesOffset, (int)fieldId);
				}
			}
		}

		// Token: 0x060065B3 RID: 26035 RVA: 0x003A1990 File Offset: 0x0039FB90
		private bool IsModifiedWrapper_CombatCharacterDict(int objectId, ushort fieldId)
		{
			CombatCharacter instance;
			bool flag = !this._combatCharacterDict.TryGetValue(objectId, out instance);
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				bool flag2 = fieldId >= 145;
				if (flag2)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(62, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Not allow to check modification state of readonly field data: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(fieldId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				result = this._dataStatesCombatCharacterDict.IsModified(instance.DataStatesOffset, (int)fieldId);
			}
			return result;
		}

		// Token: 0x060065B4 RID: 26036 RVA: 0x003A1A0C File Offset: 0x0039FC0C
		public int[] GetSelfTeam()
		{
			return this._selfTeam;
		}

		// Token: 0x060065B5 RID: 26037 RVA: 0x003A1A24 File Offset: 0x0039FC24
		public void SetSelfTeam(int[] value, DataContext context)
		{
			this._selfTeam = value;
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(11, this.DataStates, CombatDomain.CacheInfluences, context);
		}

		// Token: 0x060065B6 RID: 26038 RVA: 0x003A1A44 File Offset: 0x0039FC44
		public int GetSelfCharId()
		{
			return this._selfCharId;
		}

		// Token: 0x060065B7 RID: 26039 RVA: 0x003A1A5C File Offset: 0x0039FC5C
		public void SetSelfCharId(int value, DataContext context)
		{
			this._selfCharId = value;
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(12, this.DataStates, CombatDomain.CacheInfluences, context);
		}

		// Token: 0x060065B8 RID: 26040 RVA: 0x003A1A7C File Offset: 0x0039FC7C
		public sbyte GetSelfTeamWisdomType()
		{
			return this._selfTeamWisdomType;
		}

		// Token: 0x060065B9 RID: 26041 RVA: 0x003A1A94 File Offset: 0x0039FC94
		public void SetSelfTeamWisdomType(sbyte value, DataContext context)
		{
			this._selfTeamWisdomType = value;
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(13, this.DataStates, CombatDomain.CacheInfluences, context);
		}

		// Token: 0x060065BA RID: 26042 RVA: 0x003A1AB4 File Offset: 0x0039FCB4
		public short GetSelfTeamWisdomCount()
		{
			return this._selfTeamWisdomCount;
		}

		// Token: 0x060065BB RID: 26043 RVA: 0x003A1ACC File Offset: 0x0039FCCC
		public void SetSelfTeamWisdomCount(short value, DataContext context)
		{
			this._selfTeamWisdomCount = value;
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(14, this.DataStates, CombatDomain.CacheInfluences, context);
		}

		// Token: 0x060065BC RID: 26044 RVA: 0x003A1AEC File Offset: 0x0039FCEC
		public int[] GetEnemyTeam()
		{
			return this._enemyTeam;
		}

		// Token: 0x060065BD RID: 26045 RVA: 0x003A1B04 File Offset: 0x0039FD04
		public void SetEnemyTeam(int[] value, DataContext context)
		{
			this._enemyTeam = value;
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(15, this.DataStates, CombatDomain.CacheInfluences, context);
		}

		// Token: 0x060065BE RID: 26046 RVA: 0x003A1B24 File Offset: 0x0039FD24
		public int GetEnemyCharId()
		{
			return this._enemyCharId;
		}

		// Token: 0x060065BF RID: 26047 RVA: 0x003A1B3C File Offset: 0x0039FD3C
		public void SetEnemyCharId(int value, DataContext context)
		{
			this._enemyCharId = value;
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(16, this.DataStates, CombatDomain.CacheInfluences, context);
		}

		// Token: 0x060065C0 RID: 26048 RVA: 0x003A1B5C File Offset: 0x0039FD5C
		public sbyte GetEnemyTeamWisdomType()
		{
			return this._enemyTeamWisdomType;
		}

		// Token: 0x060065C1 RID: 26049 RVA: 0x003A1B74 File Offset: 0x0039FD74
		public void SetEnemyTeamWisdomType(sbyte value, DataContext context)
		{
			this._enemyTeamWisdomType = value;
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(17, this.DataStates, CombatDomain.CacheInfluences, context);
		}

		// Token: 0x060065C2 RID: 26050 RVA: 0x003A1B94 File Offset: 0x0039FD94
		public short GetEnemyTeamWisdomCount()
		{
			return this._enemyTeamWisdomCount;
		}

		// Token: 0x060065C3 RID: 26051 RVA: 0x003A1BAC File Offset: 0x0039FDAC
		public void SetEnemyTeamWisdomCount(short value, DataContext context)
		{
			this._enemyTeamWisdomCount = value;
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(18, this.DataStates, CombatDomain.CacheInfluences, context);
		}

		// Token: 0x060065C4 RID: 26052 RVA: 0x003A1BCC File Offset: 0x0039FDCC
		public sbyte GetCombatStatus()
		{
			return this._combatStatus;
		}

		// Token: 0x060065C5 RID: 26053 RVA: 0x003A1BE4 File Offset: 0x0039FDE4
		public void SetCombatStatus(sbyte value, DataContext context)
		{
			this._combatStatus = value;
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(19, this.DataStates, CombatDomain.CacheInfluences, context);
		}

		// Token: 0x060065C6 RID: 26054 RVA: 0x003A1C04 File Offset: 0x0039FE04
		public sbyte GetShowMercyOption()
		{
			return this._showMercyOption;
		}

		// Token: 0x060065C7 RID: 26055 RVA: 0x003A1C1C File Offset: 0x0039FE1C
		public void SetShowMercyOption(sbyte value, DataContext context)
		{
			this._showMercyOption = value;
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(20, this.DataStates, CombatDomain.CacheInfluences, context);
		}

		// Token: 0x060065C8 RID: 26056 RVA: 0x003A1C3C File Offset: 0x0039FE3C
		public sbyte GetSelectedMercyOption()
		{
			return this._selectedMercyOption;
		}

		// Token: 0x060065C9 RID: 26057 RVA: 0x003A1C54 File Offset: 0x0039FE54
		public void SetSelectedMercyOption(sbyte value, DataContext context)
		{
			this._selectedMercyOption = value;
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(21, this.DataStates, CombatDomain.CacheInfluences, context);
		}

		// Token: 0x060065CA RID: 26058 RVA: 0x003A1C74 File Offset: 0x0039FE74
		public int GetCarrierAnimalCombatCharId()
		{
			return this._carrierAnimalCombatCharId;
		}

		// Token: 0x060065CB RID: 26059 RVA: 0x003A1C8C File Offset: 0x0039FE8C
		public void SetCarrierAnimalCombatCharId(int value, DataContext context)
		{
			this._carrierAnimalCombatCharId = value;
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(22, this.DataStates, CombatDomain.CacheInfluences, context);
		}

		// Token: 0x060065CC RID: 26060 RVA: 0x003A1CAC File Offset: 0x0039FEAC
		public int GetSpecialShowCombatCharId()
		{
			return this._specialShowCombatCharId;
		}

		// Token: 0x060065CD RID: 26061 RVA: 0x003A1CC4 File Offset: 0x0039FEC4
		public void SetSpecialShowCombatCharId(int value, DataContext context)
		{
			this._specialShowCombatCharId = value;
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(23, this.DataStates, CombatDomain.CacheInfluences, context);
		}

		// Token: 0x060065CE RID: 26062 RVA: 0x003A1CE4 File Offset: 0x0039FEE4
		public IntPair GetSkillAttackedIndexAndHit()
		{
			return this._skillAttackedIndexAndHit;
		}

		// Token: 0x060065CF RID: 26063 RVA: 0x003A1CFC File Offset: 0x0039FEFC
		public void SetSkillAttackedIndexAndHit(IntPair value, DataContext context)
		{
			this._skillAttackedIndexAndHit = value;
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(24, this.DataStates, CombatDomain.CacheInfluences, context);
		}

		// Token: 0x060065D0 RID: 26064 RVA: 0x003A1D1C File Offset: 0x0039FF1C
		public bool GetWaitingDelaySettlement()
		{
			return this._waitingDelaySettlement;
		}

		// Token: 0x060065D1 RID: 26065 RVA: 0x003A1D34 File Offset: 0x0039FF34
		public void SetWaitingDelaySettlement(bool value, DataContext context)
		{
			this._waitingDelaySettlement = value;
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(25, this.DataStates, CombatDomain.CacheInfluences, context);
		}

		// Token: 0x060065D2 RID: 26066 RVA: 0x003A1D54 File Offset: 0x0039FF54
		public SpecialMiscData GetShowUseGoldenWire()
		{
			return this._showUseGoldenWire;
		}

		// Token: 0x060065D3 RID: 26067 RVA: 0x003A1D6C File Offset: 0x0039FF6C
		public void SetShowUseGoldenWire(SpecialMiscData value, DataContext context)
		{
			this._showUseGoldenWire = value;
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(26, this.DataStates, CombatDomain.CacheInfluences, context);
		}

		// Token: 0x060065D4 RID: 26068 RVA: 0x003A1D8C File Offset: 0x0039FF8C
		public bool GetIsPuppetCombat()
		{
			return this._isPuppetCombat;
		}

		// Token: 0x060065D5 RID: 26069 RVA: 0x003A1DA4 File Offset: 0x0039FFA4
		public void SetIsPuppetCombat(bool value, DataContext context)
		{
			this._isPuppetCombat = value;
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(27, this.DataStates, CombatDomain.CacheInfluences, context);
		}

		// Token: 0x060065D6 RID: 26070 RVA: 0x003A1DC4 File Offset: 0x0039FFC4
		public bool GetIsPlaygroundCombat()
		{
			return this._isPlaygroundCombat;
		}

		// Token: 0x060065D7 RID: 26071 RVA: 0x003A1DDC File Offset: 0x0039FFDC
		public void SetIsPlaygroundCombat(bool value, DataContext context)
		{
			this._isPlaygroundCombat = value;
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(28, this.DataStates, CombatDomain.CacheInfluences, context);
		}

		// Token: 0x060065D8 RID: 26072 RVA: 0x003A1DFC File Offset: 0x0039FFFC
		public CombatSkillData GetElement_SkillDataDict(CombatSkillKey objectId)
		{
			return this._skillDataDict[objectId];
		}

		// Token: 0x060065D9 RID: 26073 RVA: 0x003A1E1C File Offset: 0x003A001C
		public bool TryGetElement_SkillDataDict(CombatSkillKey objectId, out CombatSkillData element)
		{
			return this._skillDataDict.TryGetValue(objectId, out element);
		}

		// Token: 0x060065DA RID: 26074 RVA: 0x003A1E3B File Offset: 0x003A003B
		private void AddElement_SkillDataDict(CombatSkillKey objectId, CombatSkillData instance)
		{
			instance.CollectionHelperData = this.HelperDataSkillDataDict;
			instance.DataStatesOffset = this._dataStatesSkillDataDict.Create();
			this._skillDataDict.Add(objectId, instance);
		}

		// Token: 0x060065DB RID: 26075 RVA: 0x003A1E6C File Offset: 0x003A006C
		private void RemoveElement_SkillDataDict(CombatSkillKey objectId)
		{
			CombatSkillData instance;
			bool flag = !this._skillDataDict.TryGetValue(objectId, out instance);
			if (!flag)
			{
				this._dataStatesSkillDataDict.Remove(instance.DataStatesOffset);
				this._skillDataDict.Remove(objectId);
			}
		}

		// Token: 0x060065DC RID: 26076 RVA: 0x003A1EB0 File Offset: 0x003A00B0
		private void ClearSkillDataDict()
		{
			this._dataStatesSkillDataDict.Clear();
			this._skillDataDict.Clear();
		}

		// Token: 0x060065DD RID: 26077 RVA: 0x003A1ECC File Offset: 0x003A00CC
		public int GetElementField_SkillDataDict(CombatSkillKey objectId, ushort fieldId, RawDataPool dataPool, bool resetModified)
		{
			CombatSkillData instance;
			bool flag = !this._skillDataDict.TryGetValue(objectId, out instance);
			int result;
			if (flag)
			{
				string tag = "GetElementField_SkillDataDict";
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Failed to find element ");
				defaultInterpolatedStringHandler.AppendFormatted<CombatSkillKey>(objectId);
				defaultInterpolatedStringHandler.AppendLiteral(" with field ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(fieldId);
				AdaptableLog.TagWarning(tag, defaultInterpolatedStringHandler.ToStringAndClear(), false);
				result = -1;
			}
			else
			{
				if (resetModified)
				{
					this._dataStatesSkillDataDict.ResetModified(instance.DataStatesOffset, (int)fieldId);
				}
				switch (fieldId)
				{
				case 0:
					result = Serializer.Serialize(instance.GetId(), dataPool);
					break;
				case 1:
					result = Serializer.Serialize(instance.GetCanUse(), dataPool);
					break;
				case 2:
					result = Serializer.Serialize(instance.GetLeftCdFrame(), dataPool);
					break;
				case 3:
					result = Serializer.Serialize(instance.GetTotalCdFrame(), dataPool);
					break;
				case 4:
					result = Serializer.Serialize(instance.GetConstAffecting(), dataPool);
					break;
				case 5:
					result = Serializer.Serialize(instance.GetShowAffectTips(), dataPool);
					break;
				case 6:
					result = Serializer.Serialize(instance.GetSilencing(), dataPool);
					break;
				case 7:
					result = Serializer.Serialize(instance.GetBanReason(), dataPool);
					break;
				case 8:
					result = Serializer.Serialize(instance.GetEffectData(), dataPool);
					break;
				case 9:
					result = Serializer.Serialize(instance.GetCanAffect(), dataPool);
					break;
				default:
				{
					bool flag2 = fieldId >= 10;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
					if (flag2)
					{
						defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(20, 1);
						defaultInterpolatedStringHandler.AppendLiteral("Unsupported fieldId ");
						defaultInterpolatedStringHandler.AppendFormatted<ushort>(fieldId);
						throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
					}
					defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(38, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Not allow to get readonly field data: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(fieldId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				}
			}
			return result;
		}

		// Token: 0x060065DE RID: 26078 RVA: 0x003A20B0 File Offset: 0x003A02B0
		public void SetElementField_SkillDataDict(CombatSkillKey objectId, ushort fieldId, int valueOffset, RawDataPool dataPool, DataContext context)
		{
			CombatSkillData instance;
			bool flag = !this._skillDataDict.TryGetValue(objectId, out instance);
			if (flag)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Failed to find element ");
				defaultInterpolatedStringHandler.AppendFormatted<CombatSkillKey>(objectId);
				defaultInterpolatedStringHandler.AppendLiteral(" with field ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(fieldId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			switch (fieldId)
			{
			case 0:
			{
				CombatSkillKey value = default(CombatSkillKey);
				Serializer.Deserialize(dataPool, valueOffset, ref value);
				instance.SetId(value, context);
				break;
			}
			case 1:
			{
				bool value2 = false;
				Serializer.Deserialize(dataPool, valueOffset, ref value2);
				instance.SetCanUse(value2, context);
				break;
			}
			case 2:
			{
				short value3 = 0;
				Serializer.Deserialize(dataPool, valueOffset, ref value3);
				instance.SetLeftCdFrame(value3, context);
				break;
			}
			case 3:
			{
				short value4 = 0;
				Serializer.Deserialize(dataPool, valueOffset, ref value4);
				instance.SetTotalCdFrame(value4, context);
				break;
			}
			case 4:
			{
				bool value5 = false;
				Serializer.Deserialize(dataPool, valueOffset, ref value5);
				instance.SetConstAffecting(value5, context);
				break;
			}
			case 5:
			{
				bool value6 = false;
				Serializer.Deserialize(dataPool, valueOffset, ref value6);
				instance.SetShowAffectTips(value6, context);
				break;
			}
			case 6:
			{
				bool value7 = false;
				Serializer.Deserialize(dataPool, valueOffset, ref value7);
				instance.SetSilencing(value7, context);
				break;
			}
			default:
			{
				bool flag2 = fieldId >= 10;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				if (flag2)
				{
					defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(20, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported fieldId ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(fieldId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				bool flag3 = fieldId >= 10;
				if (flag3)
				{
					defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(38, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Not allow to set readonly field data: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(fieldId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to set cache field data: ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(fieldId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			}
		}

		// Token: 0x060065DF RID: 26079 RVA: 0x003A22C4 File Offset: 0x003A04C4
		private int CheckModified_SkillDataDict(CombatSkillKey objectId, ushort fieldId, RawDataPool dataPool)
		{
			CombatSkillData instance;
			bool flag = !this._skillDataDict.TryGetValue(objectId, out instance);
			int result;
			if (flag)
			{
				result = -1;
			}
			else
			{
				bool flag2 = fieldId >= 10;
				if (flag2)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(40, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Not allow to check readonly field data: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(fieldId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				bool flag3 = !this._dataStatesSkillDataDict.IsModified(instance.DataStatesOffset, (int)fieldId);
				if (flag3)
				{
					result = -1;
				}
				else
				{
					this._dataStatesSkillDataDict.ResetModified(instance.DataStatesOffset, (int)fieldId);
					switch (fieldId)
					{
					case 0:
						result = Serializer.Serialize(instance.GetId(), dataPool);
						break;
					case 1:
						result = Serializer.Serialize(instance.GetCanUse(), dataPool);
						break;
					case 2:
						result = Serializer.Serialize(instance.GetLeftCdFrame(), dataPool);
						break;
					case 3:
						result = Serializer.Serialize(instance.GetTotalCdFrame(), dataPool);
						break;
					case 4:
						result = Serializer.Serialize(instance.GetConstAffecting(), dataPool);
						break;
					case 5:
						result = Serializer.Serialize(instance.GetShowAffectTips(), dataPool);
						break;
					case 6:
						result = Serializer.Serialize(instance.GetSilencing(), dataPool);
						break;
					case 7:
						result = Serializer.Serialize(instance.GetBanReason(), dataPool);
						break;
					case 8:
						result = Serializer.Serialize(instance.GetEffectData(), dataPool);
						break;
					case 9:
						result = Serializer.Serialize(instance.GetCanAffect(), dataPool);
						break;
					default:
					{
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(20, 1);
						defaultInterpolatedStringHandler.AppendLiteral("Unsupported fieldId ");
						defaultInterpolatedStringHandler.AppendFormatted<ushort>(fieldId);
						throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
					}
					}
				}
			}
			return result;
		}

		// Token: 0x060065E0 RID: 26080 RVA: 0x003A2468 File Offset: 0x003A0668
		private void ResetModifiedWrapper_SkillDataDict(CombatSkillKey objectId, ushort fieldId)
		{
			CombatSkillData instance;
			bool flag = !this._skillDataDict.TryGetValue(objectId, out instance);
			if (!flag)
			{
				bool flag2 = fieldId >= 10;
				if (flag2)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(62, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Not allow to reset modification state of readonly field data: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(fieldId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				bool flag3 = !this._dataStatesSkillDataDict.IsModified(instance.DataStatesOffset, (int)fieldId);
				if (!flag3)
				{
					this._dataStatesSkillDataDict.ResetModified(instance.DataStatesOffset, (int)fieldId);
				}
			}
		}

		// Token: 0x060065E1 RID: 26081 RVA: 0x003A24F8 File Offset: 0x003A06F8
		private bool IsModifiedWrapper_SkillDataDict(CombatSkillKey objectId, ushort fieldId)
		{
			CombatSkillData instance;
			bool flag = !this._skillDataDict.TryGetValue(objectId, out instance);
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				bool flag2 = fieldId >= 10;
				if (flag2)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(62, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Not allow to check modification state of readonly field data: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(fieldId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				result = this._dataStatesSkillDataDict.IsModified(instance.DataStatesOffset, (int)fieldId);
			}
			return result;
		}

		// Token: 0x060065E2 RID: 26082 RVA: 0x003A2570 File Offset: 0x003A0770
		public CombatWeaponData GetElement_WeaponDataDict(int objectId)
		{
			return this._weaponDataDict[objectId];
		}

		// Token: 0x060065E3 RID: 26083 RVA: 0x003A2590 File Offset: 0x003A0790
		public bool TryGetElement_WeaponDataDict(int objectId, out CombatWeaponData element)
		{
			return this._weaponDataDict.TryGetValue(objectId, out element);
		}

		// Token: 0x060065E4 RID: 26084 RVA: 0x003A25AF File Offset: 0x003A07AF
		private void AddElement_WeaponDataDict(int objectId, CombatWeaponData instance)
		{
			instance.CollectionHelperData = this.HelperDataWeaponDataDict;
			instance.DataStatesOffset = this._dataStatesWeaponDataDict.Create();
			this._weaponDataDict.Add(objectId, instance);
		}

		// Token: 0x060065E5 RID: 26085 RVA: 0x003A25E0 File Offset: 0x003A07E0
		private void RemoveElement_WeaponDataDict(int objectId)
		{
			CombatWeaponData instance;
			bool flag = !this._weaponDataDict.TryGetValue(objectId, out instance);
			if (!flag)
			{
				this._dataStatesWeaponDataDict.Remove(instance.DataStatesOffset);
				this._weaponDataDict.Remove(objectId);
			}
		}

		// Token: 0x060065E6 RID: 26086 RVA: 0x003A2624 File Offset: 0x003A0824
		private void ClearWeaponDataDict()
		{
			this._dataStatesWeaponDataDict.Clear();
			this._weaponDataDict.Clear();
		}

		// Token: 0x060065E7 RID: 26087 RVA: 0x003A2640 File Offset: 0x003A0840
		public int GetElementField_WeaponDataDict(int objectId, ushort fieldId, RawDataPool dataPool, bool resetModified)
		{
			CombatWeaponData instance;
			bool flag = !this._weaponDataDict.TryGetValue(objectId, out instance);
			int result;
			if (flag)
			{
				string tag = "GetElementField_WeaponDataDict";
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Failed to find element ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(objectId);
				defaultInterpolatedStringHandler.AppendLiteral(" with field ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(fieldId);
				AdaptableLog.TagWarning(tag, defaultInterpolatedStringHandler.ToStringAndClear(), false);
				result = -1;
			}
			else
			{
				if (resetModified)
				{
					this._dataStatesWeaponDataDict.ResetModified(instance.DataStatesOffset, (int)fieldId);
				}
				switch (fieldId)
				{
				case 0:
					result = Serializer.Serialize(instance.GetId(), dataPool);
					break;
				case 1:
					result = Serializer.Serialize(instance.GetWeaponTricks(), dataPool);
					break;
				case 2:
					result = Serializer.Serialize(instance.GetCanChangeTo(), dataPool);
					break;
				case 3:
					result = Serializer.Serialize(instance.GetDurability(), dataPool);
					break;
				case 4:
					result = Serializer.Serialize(instance.GetCdFrame(), dataPool);
					break;
				case 5:
					result = Serializer.Serialize(instance.GetAutoAttackEffect(), dataPool);
					break;
				case 6:
					result = Serializer.Serialize(instance.GetPestleEffect(), dataPool);
					break;
				case 7:
					result = Serializer.Serialize(instance.GetFixedCdLeftFrame(), dataPool);
					break;
				case 8:
					result = Serializer.Serialize(instance.GetFixedCdTotalFrame(), dataPool);
					break;
				case 9:
					result = Serializer.Serialize(instance.GetInnerRatio(), dataPool);
					break;
				default:
				{
					bool flag2 = fieldId >= 10;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
					if (flag2)
					{
						defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(20, 1);
						defaultInterpolatedStringHandler.AppendLiteral("Unsupported fieldId ");
						defaultInterpolatedStringHandler.AppendFormatted<ushort>(fieldId);
						throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
					}
					defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(38, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Not allow to get readonly field data: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(fieldId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				}
			}
			return result;
		}

		// Token: 0x060065E8 RID: 26088 RVA: 0x003A2824 File Offset: 0x003A0A24
		public void SetElementField_WeaponDataDict(int objectId, ushort fieldId, int valueOffset, RawDataPool dataPool, DataContext context)
		{
			CombatWeaponData instance;
			bool flag = !this._weaponDataDict.TryGetValue(objectId, out instance);
			if (flag)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Failed to find element ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(objectId);
				defaultInterpolatedStringHandler.AppendLiteral(" with field ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(fieldId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			switch (fieldId)
			{
			case 0:
			{
				int value = 0;
				Serializer.Deserialize(dataPool, valueOffset, ref value);
				instance.SetId(value, context);
				break;
			}
			case 1:
			{
				sbyte[] value2 = instance.GetWeaponTricks();
				Serializer.Deserialize(dataPool, valueOffset, ref value2);
				instance.SetWeaponTricks(value2, context);
				break;
			}
			case 2:
			{
				bool value3 = false;
				Serializer.Deserialize(dataPool, valueOffset, ref value3);
				instance.SetCanChangeTo(value3, context);
				break;
			}
			case 3:
			{
				short value4 = 0;
				Serializer.Deserialize(dataPool, valueOffset, ref value4);
				instance.SetDurability(value4, context);
				break;
			}
			case 4:
			{
				short value5 = 0;
				Serializer.Deserialize(dataPool, valueOffset, ref value5);
				instance.SetCdFrame(value5, context);
				break;
			}
			case 5:
			{
				SkillEffectKey value6 = default(SkillEffectKey);
				Serializer.Deserialize(dataPool, valueOffset, ref value6);
				instance.SetAutoAttackEffect(value6, context);
				break;
			}
			case 6:
			{
				SkillEffectKey value7 = default(SkillEffectKey);
				Serializer.Deserialize(dataPool, valueOffset, ref value7);
				instance.SetPestleEffect(value7, context);
				break;
			}
			case 7:
			{
				short value8 = 0;
				Serializer.Deserialize(dataPool, valueOffset, ref value8);
				instance.SetFixedCdLeftFrame(value8, context);
				break;
			}
			case 8:
			{
				short value9 = 0;
				Serializer.Deserialize(dataPool, valueOffset, ref value9);
				instance.SetFixedCdTotalFrame(value9, context);
				break;
			}
			default:
			{
				bool flag2 = fieldId >= 10;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				if (flag2)
				{
					defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(20, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported fieldId ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(fieldId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				bool flag3 = fieldId >= 10;
				if (flag3)
				{
					defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(38, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Not allow to set readonly field data: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(fieldId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to set cache field data: ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(fieldId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			}
		}

		// Token: 0x060065E9 RID: 26089 RVA: 0x003A2A88 File Offset: 0x003A0C88
		private int CheckModified_WeaponDataDict(int objectId, ushort fieldId, RawDataPool dataPool)
		{
			CombatWeaponData instance;
			bool flag = !this._weaponDataDict.TryGetValue(objectId, out instance);
			int result;
			if (flag)
			{
				result = -1;
			}
			else
			{
				bool flag2 = fieldId >= 10;
				if (flag2)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(40, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Not allow to check readonly field data: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(fieldId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				bool flag3 = !this._dataStatesWeaponDataDict.IsModified(instance.DataStatesOffset, (int)fieldId);
				if (flag3)
				{
					result = -1;
				}
				else
				{
					this._dataStatesWeaponDataDict.ResetModified(instance.DataStatesOffset, (int)fieldId);
					switch (fieldId)
					{
					case 0:
						result = Serializer.Serialize(instance.GetId(), dataPool);
						break;
					case 1:
						result = Serializer.Serialize(instance.GetWeaponTricks(), dataPool);
						break;
					case 2:
						result = Serializer.Serialize(instance.GetCanChangeTo(), dataPool);
						break;
					case 3:
						result = Serializer.Serialize(instance.GetDurability(), dataPool);
						break;
					case 4:
						result = Serializer.Serialize(instance.GetCdFrame(), dataPool);
						break;
					case 5:
						result = Serializer.Serialize(instance.GetAutoAttackEffect(), dataPool);
						break;
					case 6:
						result = Serializer.Serialize(instance.GetPestleEffect(), dataPool);
						break;
					case 7:
						result = Serializer.Serialize(instance.GetFixedCdLeftFrame(), dataPool);
						break;
					case 8:
						result = Serializer.Serialize(instance.GetFixedCdTotalFrame(), dataPool);
						break;
					case 9:
						result = Serializer.Serialize(instance.GetInnerRatio(), dataPool);
						break;
					default:
					{
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(20, 1);
						defaultInterpolatedStringHandler.AppendLiteral("Unsupported fieldId ");
						defaultInterpolatedStringHandler.AppendFormatted<ushort>(fieldId);
						throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
					}
					}
				}
			}
			return result;
		}

		// Token: 0x060065EA RID: 26090 RVA: 0x003A2C2C File Offset: 0x003A0E2C
		private void ResetModifiedWrapper_WeaponDataDict(int objectId, ushort fieldId)
		{
			CombatWeaponData instance;
			bool flag = !this._weaponDataDict.TryGetValue(objectId, out instance);
			if (!flag)
			{
				bool flag2 = fieldId >= 10;
				if (flag2)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(62, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Not allow to reset modification state of readonly field data: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(fieldId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				bool flag3 = !this._dataStatesWeaponDataDict.IsModified(instance.DataStatesOffset, (int)fieldId);
				if (!flag3)
				{
					this._dataStatesWeaponDataDict.ResetModified(instance.DataStatesOffset, (int)fieldId);
				}
			}
		}

		// Token: 0x060065EB RID: 26091 RVA: 0x003A2CBC File Offset: 0x003A0EBC
		private bool IsModifiedWrapper_WeaponDataDict(int objectId, ushort fieldId)
		{
			CombatWeaponData instance;
			bool flag = !this._weaponDataDict.TryGetValue(objectId, out instance);
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				bool flag2 = fieldId >= 10;
				if (flag2)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(62, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Not allow to check modification state of readonly field data: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(fieldId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				result = this._dataStatesWeaponDataDict.IsModified(instance.DataStatesOffset, (int)fieldId);
			}
			return result;
		}

		// Token: 0x060065EC RID: 26092 RVA: 0x003A2D34 File Offset: 0x003A0F34
		public WeaponExpectInnerRatioData GetExpectRatioData()
		{
			return this._expectRatioData;
		}

		// Token: 0x060065ED RID: 26093 RVA: 0x003A2D4C File Offset: 0x003A0F4C
		public void SetExpectRatioData(WeaponExpectInnerRatioData value, DataContext context)
		{
			this._expectRatioData = value;
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(31, this.DataStates, CombatDomain.CacheInfluences, context);
		}

		// Token: 0x060065EE RID: 26094 RVA: 0x003A2D6C File Offset: 0x003A0F6C
		public List<int> GetTaiwuSpecialGroupCharIds()
		{
			return this._taiwuSpecialGroupCharIds;
		}

		// Token: 0x060065EF RID: 26095 RVA: 0x003A2D84 File Offset: 0x003A0F84
		public void SetTaiwuSpecialGroupCharIds(List<int> value, DataContext context)
		{
			this._taiwuSpecialGroupCharIds = value;
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(32, this.DataStates, CombatDomain.CacheInfluences, context);
		}

		// Token: 0x060065F0 RID: 26096 RVA: 0x003A2DA2 File Offset: 0x003A0FA2
		public override void OnInitializeGameDataModule()
		{
			this.InitializeOnInitializeGameDataModule();
		}

		// Token: 0x060065F1 RID: 26097 RVA: 0x003A2DAC File Offset: 0x003A0FAC
		public override void OnEnterNewWorld()
		{
			this.InitializeOnEnterNewWorld();
			this.InitializeInternalDataOfCollections();
		}

		// Token: 0x060065F2 RID: 26098 RVA: 0x003A2DBD File Offset: 0x003A0FBD
		public override void OnLoadWorld()
		{
			this.InitializeInternalDataOfCollections();
			this.OnLoadedArchiveData();
			DomainManager.Global.CompleteLoading(8);
		}

		// Token: 0x060065F3 RID: 26099 RVA: 0x003A2DDC File Offset: 0x003A0FDC
		public override int GetData(ushort dataId, ulong subId0, uint subId1, RawDataPool dataPool, bool resetModified)
		{
			int result;
			switch (dataId)
			{
			case 0:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 0);
				}
				result = Serializer.Serialize(this._timeScale, dataPool);
				break;
			case 1:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 1);
				}
				result = Serializer.Serialize(this._autoCombat, dataPool);
				break;
			case 2:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 2);
				}
				result = Serializer.Serialize(this._combatFrame, dataPool);
				break;
			case 3:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 3);
				}
				result = Serializer.Serialize(this._combatType, dataPool);
				break;
			case 4:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 4);
				}
				result = Serializer.Serialize(this._currentDistance, dataPool);
				break;
			case 5:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 5);
				}
				result = Serializer.Serialize(this._damageCompareData, dataPool);
				break;
			case 6:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 6);
					this._modificationsSkillPowerAddInCombat.Reset();
				}
				result = Serializer.SerializeModifications<CombatSkillKey>(this._skillPowerAddInCombat, dataPool);
				break;
			case 7:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 7);
					this._modificationsSkillPowerReduceInCombat.Reset();
				}
				result = Serializer.SerializeModifications<CombatSkillKey>(this._skillPowerReduceInCombat, dataPool);
				break;
			case 8:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 8);
					this._modificationsSkillPowerReplaceInCombat.Reset();
				}
				result = Serializer.SerializeModifications<CombatSkillKey>(this._skillPowerReplaceInCombat, dataPool);
				break;
			case 9:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 9);
				}
				result = Serializer.Serialize(this._bgmIndex, dataPool);
				break;
			case 10:
				result = this.GetElementField_CombatCharacterDict((int)subId0, (ushort)subId1, dataPool, resetModified);
				break;
			case 11:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 11);
				}
				result = Serializer.Serialize(this._selfTeam, dataPool);
				break;
			case 12:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 12);
				}
				result = Serializer.Serialize(this._selfCharId, dataPool);
				break;
			case 13:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 13);
				}
				result = Serializer.Serialize(this._selfTeamWisdomType, dataPool);
				break;
			case 14:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 14);
				}
				result = Serializer.Serialize(this._selfTeamWisdomCount, dataPool);
				break;
			case 15:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 15);
				}
				result = Serializer.Serialize(this._enemyTeam, dataPool);
				break;
			case 16:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 16);
				}
				result = Serializer.Serialize(this._enemyCharId, dataPool);
				break;
			case 17:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 17);
				}
				result = Serializer.Serialize(this._enemyTeamWisdomType, dataPool);
				break;
			case 18:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 18);
				}
				result = Serializer.Serialize(this._enemyTeamWisdomCount, dataPool);
				break;
			case 19:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 19);
				}
				result = Serializer.Serialize(this._combatStatus, dataPool);
				break;
			case 20:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 20);
				}
				result = Serializer.Serialize(this._showMercyOption, dataPool);
				break;
			case 21:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 21);
				}
				result = Serializer.Serialize(this._selectedMercyOption, dataPool);
				break;
			case 22:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 22);
				}
				result = Serializer.Serialize(this._carrierAnimalCombatCharId, dataPool);
				break;
			case 23:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 23);
				}
				result = Serializer.Serialize(this._specialShowCombatCharId, dataPool);
				break;
			case 24:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 24);
				}
				result = Serializer.Serialize(this._skillAttackedIndexAndHit, dataPool);
				break;
			case 25:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 25);
				}
				result = Serializer.Serialize(this._waitingDelaySettlement, dataPool);
				break;
			case 26:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 26);
				}
				result = Serializer.Serialize(this._showUseGoldenWire, dataPool);
				break;
			case 27:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 27);
				}
				result = Serializer.Serialize(this._isPuppetCombat, dataPool);
				break;
			case 28:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 28);
				}
				result = Serializer.Serialize(this._isPlaygroundCombat, dataPool);
				break;
			case 29:
				result = this.GetElementField_SkillDataDict((CombatSkillKey)subId0, (ushort)subId1, dataPool, resetModified);
				break;
			case 30:
				result = this.GetElementField_WeaponDataDict((int)subId0, (ushort)subId1, dataPool, resetModified);
				break;
			case 31:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 31);
				}
				result = Serializer.Serialize(this._expectRatioData, dataPool);
				break;
			case 32:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 32);
				}
				result = Serializer.Serialize(this._taiwuSpecialGroupCharIds, dataPool);
				break;
			default:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(19, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Unsupported dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			}
			return result;
		}

		// Token: 0x060065F4 RID: 26100 RVA: 0x003A33D0 File Offset: 0x003A15D0
		public override void SetData(ushort dataId, ulong subId0, uint subId1, int valueOffset, RawDataPool dataPool, DataContext context)
		{
			switch (dataId)
			{
			case 0:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(33, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to set value of dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 1:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(33, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to set value of dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 2:
				Serializer.Deserialize(dataPool, valueOffset, ref this._combatFrame);
				this.SetCombatFrame(this._combatFrame, context);
				break;
			case 3:
				Serializer.Deserialize(dataPool, valueOffset, ref this._combatType);
				this.SetCombatType(this._combatType, context);
				break;
			case 4:
				Serializer.Deserialize(dataPool, valueOffset, ref this._currentDistance);
				this.SetCurrentDistance(this._currentDistance, context);
				break;
			case 5:
				Serializer.Deserialize(dataPool, valueOffset, ref this._damageCompareData);
				this.SetDamageCompareData(this._damageCompareData, context);
				break;
			case 6:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(33, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to set value of dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 7:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(33, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to set value of dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 8:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(33, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to set value of dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 9:
				Serializer.Deserialize(dataPool, valueOffset, ref this._bgmIndex);
				this.SetBgmIndex(this._bgmIndex, context);
				break;
			case 10:
				this.SetElementField_CombatCharacterDict((int)subId0, (ushort)subId1, valueOffset, dataPool, context);
				break;
			case 11:
				Serializer.Deserialize(dataPool, valueOffset, ref this._selfTeam);
				this.SetSelfTeam(this._selfTeam, context);
				break;
			case 12:
				Serializer.Deserialize(dataPool, valueOffset, ref this._selfCharId);
				this.SetSelfCharId(this._selfCharId, context);
				break;
			case 13:
				Serializer.Deserialize(dataPool, valueOffset, ref this._selfTeamWisdomType);
				this.SetSelfTeamWisdomType(this._selfTeamWisdomType, context);
				break;
			case 14:
				Serializer.Deserialize(dataPool, valueOffset, ref this._selfTeamWisdomCount);
				this.SetSelfTeamWisdomCount(this._selfTeamWisdomCount, context);
				break;
			case 15:
				Serializer.Deserialize(dataPool, valueOffset, ref this._enemyTeam);
				this.SetEnemyTeam(this._enemyTeam, context);
				break;
			case 16:
				Serializer.Deserialize(dataPool, valueOffset, ref this._enemyCharId);
				this.SetEnemyCharId(this._enemyCharId, context);
				break;
			case 17:
				Serializer.Deserialize(dataPool, valueOffset, ref this._enemyTeamWisdomType);
				this.SetEnemyTeamWisdomType(this._enemyTeamWisdomType, context);
				break;
			case 18:
				Serializer.Deserialize(dataPool, valueOffset, ref this._enemyTeamWisdomCount);
				this.SetEnemyTeamWisdomCount(this._enemyTeamWisdomCount, context);
				break;
			case 19:
				Serializer.Deserialize(dataPool, valueOffset, ref this._combatStatus);
				this.SetCombatStatus(this._combatStatus, context);
				break;
			case 20:
				Serializer.Deserialize(dataPool, valueOffset, ref this._showMercyOption);
				this.SetShowMercyOption(this._showMercyOption, context);
				break;
			case 21:
				Serializer.Deserialize(dataPool, valueOffset, ref this._selectedMercyOption);
				this.SetSelectedMercyOption(this._selectedMercyOption, context);
				break;
			case 22:
				Serializer.Deserialize(dataPool, valueOffset, ref this._carrierAnimalCombatCharId);
				this.SetCarrierAnimalCombatCharId(this._carrierAnimalCombatCharId, context);
				break;
			case 23:
				Serializer.Deserialize(dataPool, valueOffset, ref this._specialShowCombatCharId);
				this.SetSpecialShowCombatCharId(this._specialShowCombatCharId, context);
				break;
			case 24:
				Serializer.Deserialize(dataPool, valueOffset, ref this._skillAttackedIndexAndHit);
				this.SetSkillAttackedIndexAndHit(this._skillAttackedIndexAndHit, context);
				break;
			case 25:
				Serializer.Deserialize(dataPool, valueOffset, ref this._waitingDelaySettlement);
				this.SetWaitingDelaySettlement(this._waitingDelaySettlement, context);
				break;
			case 26:
				Serializer.Deserialize(dataPool, valueOffset, ref this._showUseGoldenWire);
				this.SetShowUseGoldenWire(this._showUseGoldenWire, context);
				break;
			case 27:
				Serializer.Deserialize(dataPool, valueOffset, ref this._isPuppetCombat);
				this.SetIsPuppetCombat(this._isPuppetCombat, context);
				break;
			case 28:
				Serializer.Deserialize(dataPool, valueOffset, ref this._isPlaygroundCombat);
				this.SetIsPlaygroundCombat(this._isPlaygroundCombat, context);
				break;
			case 29:
				this.SetElementField_SkillDataDict((CombatSkillKey)subId0, (ushort)subId1, valueOffset, dataPool, context);
				break;
			case 30:
				this.SetElementField_WeaponDataDict((int)subId0, (ushort)subId1, valueOffset, dataPool, context);
				break;
			case 31:
				Serializer.Deserialize(dataPool, valueOffset, ref this._expectRatioData);
				this.SetExpectRatioData(this._expectRatioData, context);
				break;
			case 32:
				Serializer.Deserialize(dataPool, valueOffset, ref this._taiwuSpecialGroupCharIds);
				this.SetTaiwuSpecialGroupCharIds(this._taiwuSpecialGroupCharIds, context);
				break;
			default:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(19, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Unsupported dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			}
		}

		// Token: 0x060065F5 RID: 26101 RVA: 0x003A395C File Offset: 0x003A1B5C
		public override int CallMethod(Operation operation, RawDataPool argDataPool, RawDataPool returnDataPool, DataContext context)
		{
			int argsOffset = operation.ArgsOffset;
			int result;
			switch (operation.MethodId)
			{
			case 0:
			{
				int argsCount = operation.ArgsCount;
				int num = argsCount;
				if (num != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				bool isAlly = false;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref isAlly);
				this.PlayMoveStepSound(context, isAlly);
				result = -1;
				break;
			}
			case 1:
			{
				int argsCount2 = operation.ArgsCount;
				int num2 = argsCount2;
				if (num2 != 3)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				bool isAlly2 = false;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref isAlly2);
				int index = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref index);
				int charId = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref charId);
				bool returnValue = this.ExecuteTeammateCommand(context, isAlly2, index, charId);
				result = Serializer.Serialize(returnValue, returnDataPool);
				break;
			}
			case 2:
			{
				int argsCount3 = operation.ArgsCount;
				int num3 = argsCount3;
				if (num3 != 2)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				int charId2 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref charId2);
				int index2 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref index2);
				this.RemoveTeammateCommand(context, charId2, index2);
				result = -1;
				break;
			}
			case 3:
			{
				int argsCount4 = operation.ArgsCount;
				int num4 = argsCount4;
				if (num4 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				int charId3 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref charId3);
				CombatCharacterDisplayData returnValue2 = this.GetCombatCharDisplayData(context, charId3);
				result = Serializer.Serialize(returnValue2, returnDataPool);
				break;
			}
			case 4:
			{
				int argsCount5 = operation.ArgsCount;
				int num5 = argsCount5;
				if (num5 != 2)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				bool isAlly3 = false;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref isAlly3);
				bool mercy = false;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref mercy);
				this.SelectMercyOption(context, isAlly3, mercy);
				result = -1;
				break;
			}
			case 5:
				switch (operation.ArgsCount)
				{
				case 1:
				{
					int weaponIndex = 0;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref weaponIndex);
					this.ChangeWeapon(context, weaponIndex, true, false);
					result = -1;
					break;
				}
				case 2:
				{
					int weaponIndex2 = 0;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref weaponIndex2);
					bool isAlly4 = false;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref isAlly4);
					this.ChangeWeapon(context, weaponIndex2, isAlly4, false);
					result = -1;
					break;
				}
				case 3:
				{
					int weaponIndex3 = 0;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref weaponIndex3);
					bool isAlly5 = false;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref isAlly5);
					bool forceChange = false;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref forceChange);
					this.ChangeWeapon(context, weaponIndex3, isAlly5, forceChange);
					result = -1;
					break;
				}
				default:
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				}
				break;
			case 6:
			{
				int argsCount6 = operation.ArgsCount;
				int num6 = argsCount6;
				if (num6 != 0)
				{
					if (num6 != 1)
					{
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
						defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
						defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
						throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
					}
					bool isAlly6 = false;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref isAlly6);
					this.NormalAttack(context, isAlly6);
					result = -1;
				}
				else
				{
					this.NormalAttack(context, true);
					result = -1;
				}
				break;
			}
			case 7:
			{
				int argsCount7 = operation.ArgsCount;
				int num7 = argsCount7;
				if (num7 != 0)
				{
					if (num7 != 1)
					{
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
						defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
						defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
						throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
					}
					bool isAlly7 = false;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref isAlly7);
					this.StartChangeTrick(context, isAlly7);
					result = -1;
				}
				else
				{
					this.StartChangeTrick(context, true);
					result = -1;
				}
				break;
			}
			case 8:
			{
				int argsCount8 = operation.ArgsCount;
				int num8 = argsCount8;
				if (num8 != 3)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				sbyte trickType = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref trickType);
				sbyte bodyPart = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref bodyPart);
				int flawOrAcupointType = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref flawOrAcupointType);
				this.SelectChangeTrick(context, trickType, bodyPart, flawOrAcupointType);
				result = -1;
				break;
			}
			case 9:
			{
				int argsCount9 = operation.ArgsCount;
				int num9 = argsCount9;
				if (num9 != 2)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				int index3 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref index3);
				sbyte expectInnerRatio = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref expectInnerRatio);
				this.ChangeTaiwuWeaponInnerRatio(context, index3, expectInnerRatio);
				result = -1;
				break;
			}
			case 10:
			{
				int argsCount10 = operation.ArgsCount;
				int num10 = argsCount10;
				if (num10 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				ItemKey weaponKey = default(ItemKey);
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref weaponKey);
				sbyte returnValue3 = this.GetWeaponInnerRatio(context, weaponKey);
				result = Serializer.Serialize(returnValue3, returnDataPool);
				break;
			}
			case 11:
			{
				int argsCount11 = operation.ArgsCount;
				int num11 = argsCount11;
				if (num11 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				ItemKey weaponKey2 = default(ItemKey);
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref weaponKey2);
				WeaponEffectDisplayData[] returnValue4 = this.GetWeaponEffects(weaponKey2);
				result = Serializer.Serialize(returnValue4, returnDataPool);
				break;
			}
			case 12:
			{
				int argsCount12 = operation.ArgsCount;
				int num12 = argsCount12;
				if (num12 != 1)
				{
					if (num12 != 2)
					{
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
						defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
						defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
						throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
					}
					sbyte actionType = 0;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref actionType);
					bool isAlly8 = false;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref isAlly8);
					this.StartPrepareOtherAction(context, actionType, isAlly8);
					result = -1;
				}
				else
				{
					sbyte actionType2 = 0;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref actionType2);
					this.StartPrepareOtherAction(context, actionType2, true);
					result = -1;
				}
				break;
			}
			case 13:
			{
				int argsCount13 = operation.ArgsCount;
				int num13 = argsCount13;
				if (num13 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				int charId4 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref charId4);
				List<CombatSkillDisplayData> returnValue5 = this.GetProactiveSkillList(charId4);
				result = Serializer.Serialize(returnValue5, returnDataPool);
				break;
			}
			case 14:
			{
				int argsCount14 = operation.ArgsCount;
				int num14 = argsCount14;
				if (num14 != 1)
				{
					if (num14 != 2)
					{
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
						defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
						defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
						throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
					}
					short skillId = 0;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref skillId);
					bool isAlly9 = false;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref isAlly9);
					this.StartPrepareSkill(context, skillId, isAlly9);
					result = -1;
				}
				else
				{
					short skillId2 = 0;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref skillId2);
					this.StartPrepareSkill(context, skillId2, true);
					result = -1;
				}
				break;
			}
			case 15:
			{
				int argsCount15 = operation.ArgsCount;
				int num15 = argsCount15;
				if (num15 != 0)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				this.GmCmd_ForceRecoverBreathAndStance(context);
				result = -1;
				break;
			}
			case 16:
			{
				int argsCount16 = operation.ArgsCount;
				int num16 = argsCount16;
				if (num16 != 2)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				bool isAlly10 = false;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref isAlly10);
				sbyte trickType2 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref trickType2);
				this.GmCmd_AddTrick(context, isAlly10, trickType2);
				result = -1;
				break;
			}
			case 17:
				switch (operation.ArgsCount)
				{
				case 3:
				{
					bool isAlly11 = false;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref isAlly11);
					sbyte bodyPart2 = 0;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref bodyPart2);
					bool isInner = false;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref isInner);
					this.GmCmd_AddInjury(context, isAlly11, bodyPart2, isInner, 1, false);
					result = -1;
					break;
				}
				case 4:
				{
					bool isAlly12 = false;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref isAlly12);
					sbyte bodyPart3 = 0;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref bodyPart3);
					bool isInner2 = false;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref isInner2);
					int count = 0;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref count);
					this.GmCmd_AddInjury(context, isAlly12, bodyPart3, isInner2, count, false);
					result = -1;
					break;
				}
				case 5:
				{
					bool isAlly13 = false;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref isAlly13);
					sbyte bodyPart4 = 0;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref bodyPart4);
					bool isInner3 = false;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref isInner3);
					int count2 = 0;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref count2);
					bool changeToOld = false;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref changeToOld);
					this.GmCmd_AddInjury(context, isAlly13, bodyPart4, isInner3, count2, changeToOld);
					result = -1;
					break;
				}
				default:
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				}
				break;
			case 18:
			{
				int argsCount17 = operation.ArgsCount;
				int num17 = argsCount17;
				if (num17 != 0)
				{
					if (num17 != 1)
					{
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
						defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
						defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
						throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
					}
					bool isAlly14 = false;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref isAlly14);
					this.GmCmd_ForceHealAllInjury(context, isAlly14);
					result = -1;
				}
				else
				{
					this.GmCmd_ForceHealAllInjury(context, true);
					result = -1;
				}
				break;
			}
			case 19:
				switch (operation.ArgsCount)
				{
				case 2:
				{
					bool isAlly15 = false;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref isAlly15);
					sbyte poisonType = 0;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref poisonType);
					this.GmCmd_AddPoison(context, isAlly15, poisonType, 1, false);
					result = -1;
					break;
				}
				case 3:
				{
					bool isAlly16 = false;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref isAlly16);
					sbyte poisonType2 = 0;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref poisonType2);
					int count3 = 0;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref count3);
					this.GmCmd_AddPoison(context, isAlly16, poisonType2, count3, false);
					result = -1;
					break;
				}
				case 4:
				{
					bool isAlly17 = false;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref isAlly17);
					sbyte poisonType3 = 0;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref poisonType3);
					int count4 = 0;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref count4);
					bool changeToOld2 = false;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref changeToOld2);
					this.GmCmd_AddPoison(context, isAlly17, poisonType3, count4, changeToOld2);
					result = -1;
					break;
				}
				default:
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				}
				break;
			case 20:
			{
				int argsCount18 = operation.ArgsCount;
				int num18 = argsCount18;
				if (num18 != 0)
				{
					if (num18 != 1)
					{
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
						defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
						defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
						throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
					}
					bool isAlly18 = false;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref isAlly18);
					this.GmCmd_ForceHealAllPoison(context, isAlly18);
					result = -1;
				}
				else
				{
					this.GmCmd_ForceHealAllPoison(context, true);
					result = -1;
				}
				break;
			}
			case 21:
			{
				int argsCount19 = operation.ArgsCount;
				int num19 = argsCount19;
				if (num19 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				short skillId3 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref skillId3);
				this.GmCmd_ForceEnemyUseSkill(context, skillId3);
				result = -1;
				break;
			}
			case 22:
			{
				int argsCount20 = operation.ArgsCount;
				int num20 = argsCount20;
				if (num20 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				sbyte actionType3 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref actionType3);
				this.GmCmd_ForceEnemyUseOtherAction(context, actionType3);
				result = -1;
				break;
			}
			case 23:
			{
				int argsCount21 = operation.ArgsCount;
				int num21 = argsCount21;
				if (num21 != 0)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				this.GmCmd_ForceEnemyDefeat(context);
				result = -1;
				break;
			}
			case 24:
			{
				int argsCount22 = operation.ArgsCount;
				int num22 = argsCount22;
				if (num22 != 0)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				this.GmCmd_ForceSelfDefeat(context);
				result = -1;
				break;
			}
			case 25:
			{
				int argsCount23 = operation.ArgsCount;
				int num23 = argsCount23;
				if (num23 != 2)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				bool isAlly19 = false;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref isAlly19);
				short[] neiliAllocation = null;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref neiliAllocation);
				this.GmCmd_SetNeiliAllocation(context, isAlly19, neiliAllocation);
				result = -1;
				break;
			}
			case 26:
			{
				int argsCount24 = operation.ArgsCount;
				int num24 = argsCount24;
				if (num24 != 2)
				{
					if (num24 != 3)
					{
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
						defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
						defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
						throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
					}
					bool isAlly20 = false;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref isAlly20);
					sbyte bodyPart5 = 0;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref bodyPart5);
					int count5 = 0;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref count5);
					this.GmCmd_AddFlaw(context, isAlly20, bodyPart5, count5);
					result = -1;
				}
				else
				{
					bool isAlly21 = false;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref isAlly21);
					sbyte bodyPart6 = 0;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref bodyPart6);
					this.GmCmd_AddFlaw(context, isAlly21, bodyPart6, 1);
					result = -1;
				}
				break;
			}
			case 27:
			{
				int argsCount25 = operation.ArgsCount;
				int num25 = argsCount25;
				if (num25 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				bool isAlly22 = false;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref isAlly22);
				this.GmCmd_HealAllFlaw(context, isAlly22);
				result = -1;
				break;
			}
			case 28:
			{
				int argsCount26 = operation.ArgsCount;
				int num26 = argsCount26;
				if (num26 != 2)
				{
					if (num26 != 3)
					{
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
						defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
						defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
						throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
					}
					bool isAlly23 = false;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref isAlly23);
					sbyte bodyPart7 = 0;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref bodyPart7);
					int count6 = 0;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref count6);
					this.GmCmd_AddAcupoint(context, isAlly23, bodyPart7, count6);
					result = -1;
				}
				else
				{
					bool isAlly24 = false;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref isAlly24);
					sbyte bodyPart8 = 0;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref bodyPart8);
					this.GmCmd_AddAcupoint(context, isAlly24, bodyPart8, 1);
					result = -1;
				}
				break;
			}
			case 29:
			{
				int argsCount27 = operation.ArgsCount;
				int num27 = argsCount27;
				if (num27 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				bool isAlly25 = false;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref isAlly25);
				this.GmCmd_HealAllAcupoint(context, isAlly25);
				result = -1;
				break;
			}
			case 30:
			{
				int argsCount28 = operation.ArgsCount;
				int num28 = argsCount28;
				if (num28 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				short charTemplateId = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref charTemplateId);
				this.GmCmd_FightBoss(context, charTemplateId);
				result = -1;
				break;
			}
			case 31:
			{
				int argsCount29 = operation.ArgsCount;
				int num29 = argsCount29;
				if (num29 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				short charTemplateId2 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref charTemplateId2);
				this.GmCmd_FightAnimal(context, charTemplateId2);
				result = -1;
				break;
			}
			case 32:
			{
				int argsCount30 = operation.ArgsCount;
				int num30 = argsCount30;
				if (num30 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				bool on = false;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref on);
				this.GmCmd_EnableEnemyAi(context, on);
				result = -1;
				break;
			}
			case 33:
			{
				int argsCount31 = operation.ArgsCount;
				int num31 = argsCount31;
				if (num31 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				bool on2 = false;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref on2);
				this.GmCmd_EnableSkillFreeCast(context, on2);
				result = -1;
				break;
			}
			case 34:
			{
				int argsCount32 = operation.ArgsCount;
				int num32 = argsCount32;
				if (num32 != 2)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				int doctorCharId = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref doctorCharId);
				int patientCharId = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref patientCharId);
				uint returnValue6 = this.GetHealInjuryBanReason(doctorCharId, patientCharId);
				result = Serializer.Serialize(returnValue6, returnDataPool);
				break;
			}
			case 35:
			{
				int argsCount33 = operation.ArgsCount;
				int num33 = argsCount33;
				if (num33 != 2)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				int doctorCharId2 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref doctorCharId2);
				int patientCharId2 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref patientCharId2);
				uint returnValue7 = this.GetHealPoisonBanReason(doctorCharId2, patientCharId2);
				result = Serializer.Serialize(returnValue7, returnDataPool);
				break;
			}
			case 36:
				switch (operation.ArgsCount)
				{
				case 1:
				{
					ItemKey itemKey = default(ItemKey);
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref itemKey);
					this.UseItem(context, itemKey, -1, true, null);
					result = -1;
					break;
				}
				case 2:
				{
					ItemKey itemKey2 = default(ItemKey);
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref itemKey2);
					sbyte useType = 0;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref useType);
					this.UseItem(context, itemKey2, useType, true, null);
					result = -1;
					break;
				}
				case 3:
				{
					ItemKey itemKey3 = default(ItemKey);
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref itemKey3);
					sbyte useType2 = 0;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref useType2);
					bool isAlly26 = false;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref isAlly26);
					this.UseItem(context, itemKey3, useType2, isAlly26, null);
					result = -1;
					break;
				}
				case 4:
				{
					ItemKey itemKey4 = default(ItemKey);
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref itemKey4);
					sbyte useType3 = 0;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref useType3);
					bool isAlly27 = false;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref isAlly27);
					List<sbyte> targetBodyParts = null;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref targetBodyParts);
					this.UseItem(context, itemKey4, useType3, isAlly27, targetBodyParts);
					result = -1;
					break;
				}
				default:
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				}
				break;
			case 37:
			{
				int argsCount34 = operation.ArgsCount;
				int num34 = argsCount34;
				if (num34 != 2)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				short combatConfigId = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref combatConfigId);
				int[] enemyTeam = null;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref enemyTeam);
				sbyte returnValue8 = this.PrepareCombat(context, combatConfigId, enemyTeam);
				result = Serializer.Serialize(returnValue8, returnDataPool);
				break;
			}
			case 38:
			{
				int argsCount35 = operation.ArgsCount;
				int num35 = argsCount35;
				if (num35 != 0)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				bool returnValue9 = this.StartCombat(context);
				result = Serializer.Serialize(returnValue9, returnDataPool);
				break;
			}
			case 39:
			{
				int argsCount36 = operation.ArgsCount;
				int num36 = argsCount36;
				if (num36 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				float timeScale = 0f;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref timeScale);
				this.SetTimeScale(context, timeScale);
				result = -1;
				break;
			}
			case 40:
			{
				int argsCount37 = operation.ArgsCount;
				int num37 = argsCount37;
				if (num37 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				bool autoCombat = false;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref autoCombat);
				this.SetPlayerAutoCombat(context, autoCombat);
				result = -1;
				break;
			}
			case 41:
			{
				int argsCount38 = operation.ArgsCount;
				int num38 = argsCount38;
				if (num38 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				bool isAlly28 = false;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref isAlly28);
				TestAiData returnValue10 = this.GetAiTestData(isAlly28);
				result = Serializer.Serialize(returnValue10, returnDataPool);
				break;
			}
			case 42:
			{
				int argsCount39 = operation.ArgsCount;
				int num39 = argsCount39;
				if (num39 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				AiOptions aiOptions = null;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref aiOptions);
				this.SetAiOptions(aiOptions);
				result = -1;
				break;
			}
			case 43:
				switch (operation.ArgsCount)
				{
				case 1:
				{
					byte state = 0;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref state);
					this.SetMoveState(state, true, false);
					result = -1;
					break;
				}
				case 2:
				{
					byte state2 = 0;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref state2);
					bool isAlly29 = false;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref isAlly29);
					this.SetMoveState(state2, isAlly29, false);
					result = -1;
					break;
				}
				case 3:
				{
					byte state3 = 0;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref state3);
					bool isAlly30 = false;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref isAlly30);
					bool settedByPlayer = false;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref settedByPlayer);
					this.SetMoveState(state3, isAlly30, settedByPlayer);
					result = -1;
					break;
				}
				default:
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				}
				break;
			case 44:
			{
				int argsCount40 = operation.ArgsCount;
				int num40 = argsCount40;
				if (num40 != 0)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				CombatResultDisplayData returnValue11 = this.GetCombatResultDisplayData();
				result = Serializer.Serialize(returnValue11, returnDataPool);
				break;
			}
			case 45:
			{
				int argsCount41 = operation.ArgsCount;
				int num41 = argsCount41;
				if (num41 != 2)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				List<ItemKey> acceptItems = null;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref acceptItems);
				List<int> acceptCounts = null;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref acceptCounts);
				this.SelectGetItem(context, acceptItems, acceptCounts);
				result = -1;
				break;
			}
			case 46:
			{
				int argsCount42 = operation.ArgsCount;
				int num42 = argsCount42;
				if (num42 != 0)
				{
					if (num42 != 1)
					{
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
						defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
						defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
						throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
					}
					bool isAlly31 = false;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref isAlly31);
					this.Surrender(context, isAlly31);
					result = -1;
				}
				else
				{
					this.Surrender(context, true);
					result = -1;
				}
				break;
			}
			case 47:
			{
				int argsCount43 = operation.ArgsCount;
				int num43 = argsCount43;
				if (num43 != 2)
				{
					if (num43 != 3)
					{
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
						defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
						defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
						throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
					}
					short puppetCharTemplateId = 0;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref puppetCharTemplateId);
					sbyte consummateLevel = 0;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref consummateLevel);
					bool playground = false;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref playground);
					this.EnterBossPuppetCombat(context, puppetCharTemplateId, consummateLevel, playground);
					result = -1;
				}
				else
				{
					short puppetCharTemplateId2 = 0;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref puppetCharTemplateId2);
					sbyte consummateLevel2 = 0;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref consummateLevel2);
					this.EnterBossPuppetCombat(context, puppetCharTemplateId2, consummateLevel2, false);
					result = -1;
				}
				break;
			}
			case 48:
			{
				int argsCount44 = operation.ArgsCount;
				int num44 = argsCount44;
				if (num44 != 2)
				{
					if (num44 != 3)
					{
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
						defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
						defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
						throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
					}
					ItemKey toolKey = default(ItemKey);
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref toolKey);
					ItemKey targetKey = default(ItemKey);
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref targetKey);
					bool isAlly32 = false;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref isAlly32);
					this.RepairItem(context, toolKey, targetKey, isAlly32);
					result = -1;
				}
				else
				{
					ItemKey toolKey2 = default(ItemKey);
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref toolKey2);
					ItemKey targetKey2 = default(ItemKey);
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref targetKey2);
					this.RepairItem(context, toolKey2, targetKey2, true);
					result = -1;
				}
				break;
			}
			case 49:
			{
				int argsCount45 = operation.ArgsCount;
				int num45 = argsCount45;
				if (num45 != 2)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				short combatConfigId2 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref combatConfigId2);
				List<int> enemyList = null;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref enemyList);
				this.PrepareEnemyEquipments(context, combatConfigId2, enemyList);
				result = -1;
				break;
			}
			case 50:
			{
				int argsCount46 = operation.ArgsCount;
				int num46 = argsCount46;
				if (num46 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				bool enable = false;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref enable);
				this.EnableBulletTime(context, enable);
				result = -1;
				break;
			}
			case 51:
			{
				int argsCount47 = operation.ArgsCount;
				int num47 = argsCount47;
				if (num47 != 2)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				bool isAlly33 = false;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref isAlly33);
				bool on3 = false;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref on3);
				this.GmCmd_SetImmortal(context, isAlly33, on3);
				result = -1;
				break;
			}
			case 52:
			{
				int argsCount48 = operation.ArgsCount;
				int num48 = argsCount48;
				if (num48 != 0)
				{
					if (num48 != 1)
					{
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
						defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
						defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
						throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
					}
					bool isAlly34 = false;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref isAlly34);
					this.CancelChangeTrick(context, isAlly34);
					result = -1;
				}
				else
				{
					this.CancelChangeTrick(context, true);
					result = -1;
				}
				break;
			}
			case 53:
			{
				int argsCount49 = operation.ArgsCount;
				int num49 = argsCount49;
				if (num49 != 0)
				{
					if (num49 != 1)
					{
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
						defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
						defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
						throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
					}
					bool isAlly35 = false;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref isAlly35);
					this.ClearAllReserveAction(context, isAlly35);
					result = -1;
				}
				else
				{
					this.ClearAllReserveAction(context, true);
					result = -1;
				}
				break;
			}
			case 54:
			{
				int argsCount50 = operation.ArgsCount;
				int num50 = argsCount50;
				if (num50 != 0)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				this.InterruptSurrender();
				result = -1;
				break;
			}
			case 55:
			{
				int argsCount51 = operation.ArgsCount;
				int num51 = argsCount51;
				if (num51 != 0)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				bool returnValue12 = this.IsInCombat();
				result = Serializer.Serialize(returnValue12, returnDataPool);
				break;
			}
			case 56:
			{
				int argsCount52 = operation.ArgsCount;
				int num52 = argsCount52;
				if (num52 != 2)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				short charTemplateId3 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref charTemplateId3);
				int testCount = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref testCount);
				this.GmCmd_FightTestOrgMember(context, charTemplateId3, testCount);
				result = -1;
				break;
			}
			case 57:
			{
				int argsCount53 = operation.ArgsCount;
				int num53 = argsCount53;
				if (num53 != 2)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				short charTemplateId4 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref charTemplateId4);
				sbyte combatTypeAsSbyte = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref combatTypeAsSbyte);
				this.GmCmd_FightRandomEnemy(context, charTemplateId4, combatTypeAsSbyte);
				result = -1;
				break;
			}
			case 58:
			{
				int argsCount54 = operation.ArgsCount;
				int num54 = argsCount54;
				if (num54 != 0)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				this.GmCmd_ForceRecoverMobilityValue(context);
				result = -1;
				break;
			}
			case 59:
			{
				int argsCount55 = operation.ArgsCount;
				int num55 = argsCount55;
				if (num55 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				bool isAlly36 = false;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref isAlly36);
				this.GmCmd_UnitTestSetDistanceToTarget(context, isAlly36);
				result = -1;
				break;
			}
			case 60:
			{
				int argsCount56 = operation.ArgsCount;
				int num56 = argsCount56;
				if (num56 != 3)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				int charId5 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref charId5);
				short skillTemplateId = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref skillTemplateId);
				bool isDirect = false;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref isDirect);
				bool returnValue13 = this.GmCmd_UnitTestEquipSkill(context, charId5, skillTemplateId, isDirect);
				result = Serializer.Serialize(returnValue13, returnDataPool);
				break;
			}
			case 61:
			{
				int argsCount57 = operation.ArgsCount;
				int num57 = argsCount57;
				if (num57 != 0)
				{
					if (num57 != 1)
					{
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
						defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
						defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
						throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
					}
					bool testing = false;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref testing);
					this.GmCmd_UnitTestPrepare(context, testing);
					result = -1;
				}
				else
				{
					this.GmCmd_UnitTestPrepare(context, true);
					result = -1;
				}
				break;
			}
			case 62:
			{
				int argsCount58 = operation.ArgsCount;
				int num58 = argsCount58;
				if (num58 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				int charId6 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref charId6);
				this.GmCmd_UnitTestClearAllEquipSkill(context, charId6);
				result = -1;
				break;
			}
			case 63:
			{
				int argsCount59 = operation.ArgsCount;
				int num59 = argsCount59;
				if (num59 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				int charId7 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref charId7);
				DamageStepDisplayData returnValue14 = this.GetFatalDamageStepDisplayData(charId7);
				result = Serializer.Serialize(returnValue14, returnDataPool);
				break;
			}
			case 64:
			{
				int argsCount60 = operation.ArgsCount;
				int num60 = argsCount60;
				if (num60 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				int charId8 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref charId8);
				DamageStepDisplayData returnValue15 = this.GetMindDamageStepDisplayData(charId8);
				result = Serializer.Serialize(returnValue15, returnDataPool);
				break;
			}
			case 65:
			{
				int argsCount61 = operation.ArgsCount;
				int num61 = argsCount61;
				if (num61 != 2)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				int charId9 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref charId9);
				sbyte bodyPart9 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref bodyPart9);
				OuterAndInnerDamageStepDisplayData returnValue16 = this.GetBodyPartDamageStepDisplayData(charId9, bodyPart9);
				result = Serializer.Serialize(returnValue16, returnDataPool);
				break;
			}
			case 66:
			{
				int argsCount62 = operation.ArgsCount;
				int num62 = argsCount62;
				if (num62 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				int charId10 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref charId10);
				CompleteDamageStepDisplayData returnValue17 = this.GetCompleteDamageStepDisplayData(charId10);
				result = Serializer.Serialize(returnValue17, returnDataPool);
				break;
			}
			case 67:
			{
				int argsCount63 = operation.ArgsCount;
				int num63 = argsCount63;
				if (num63 != 2)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				bool isAlly37 = false;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref isAlly37);
				short wugCount = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref wugCount);
				this.GmCmd_ForceRecoverWugCount(context, isAlly37, wugCount);
				result = -1;
				break;
			}
			case 68:
			{
				int argsCount64 = operation.ArgsCount;
				int num64 = argsCount64;
				if (num64 != 2)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				int charId11 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref charId11);
				short combatConfig = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref combatConfig);
				this.GmCmd_FightCharacter(context, charId11, combatConfig);
				result = -1;
				break;
			}
			case 69:
			{
				int argsCount65 = operation.ArgsCount;
				int num65 = argsCount65;
				if (num65 != 0)
				{
					if (num65 != 1)
					{
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
						defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
						defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
						throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
					}
					bool isAlly38 = false;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref isAlly38);
					ChangeTrickDisplayData returnValue18 = this.GetChangeTrickDisplayData(isAlly38);
					result = Serializer.Serialize(returnValue18, returnDataPool);
				}
				else
				{
					ChangeTrickDisplayData returnValue19 = this.GetChangeTrickDisplayData(true);
					result = Serializer.Serialize(returnValue19, returnDataPool);
				}
				break;
			}
			case 70:
			{
				int argsCount66 = operation.ArgsCount;
				int num66 = argsCount66;
				if (num66 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				bool isAlly39 = false;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref isAlly39);
				this.ClearAffectingDefenseSkillManual(context, isAlly39);
				result = -1;
				break;
			}
			case 71:
			{
				int argsCount67 = operation.ArgsCount;
				int num67 = argsCount67;
				if (num67 != 0)
				{
					if (num67 != 1)
					{
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
						defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
						defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
						throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
					}
					bool isAlly40 = false;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref isAlly40);
					bool returnValue20 = this.ClearDefendInBlockAttackSkill(context, isAlly40);
					result = Serializer.Serialize(returnValue20, returnDataPool);
				}
				else
				{
					bool returnValue21 = this.ClearDefendInBlockAttackSkill(context, true);
					result = Serializer.Serialize(returnValue21, returnDataPool);
				}
				break;
			}
			case 72:
			{
				int argsCount68 = operation.ArgsCount;
				int num68 = argsCount68;
				if (num68 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				bool isAlly41 = false;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref isAlly41);
				this.GmCmd_HealAllFatal(context, isAlly41);
				result = -1;
				break;
			}
			case 73:
			{
				int argsCount69 = operation.ArgsCount;
				int num69 = argsCount69;
				if (num69 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				bool isAlly42 = false;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref isAlly42);
				this.GmCmd_HealAllDefeatMark(context, isAlly42);
				result = -1;
				break;
			}
			case 74:
			{
				int argsCount70 = operation.ArgsCount;
				int num70 = argsCount70;
				if (num70 != 1)
				{
					if (num70 != 2)
					{
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
						defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
						defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
						throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
					}
					bool isAlly43 = false;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref isAlly43);
					int count7 = 0;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref count7);
					this.GmCmd_AddAllDefeatMark(context, isAlly43, count7);
					result = -1;
				}
				else
				{
					bool isAlly44 = false;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref isAlly44);
					this.GmCmd_AddAllDefeatMark(context, isAlly44, 1);
					result = -1;
				}
				break;
			}
			case 75:
			{
				int argsCount71 = operation.ArgsCount;
				int num71 = argsCount71;
				if (num71 != 2)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				bool isAlly45 = false;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref isAlly45);
				int count8 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref count8);
				this.GmCmd_AddFatal(context, isAlly45, count8);
				result = -1;
				break;
			}
			case 76:
			{
				int argsCount72 = operation.ArgsCount;
				int num72 = argsCount72;
				if (num72 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				bool isAlly46 = false;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref isAlly46);
				this.GmCmd_HealAllDie(context, isAlly46);
				result = -1;
				break;
			}
			case 77:
			{
				int argsCount73 = operation.ArgsCount;
				int num73 = argsCount73;
				if (num73 != 2)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				bool isAlly47 = false;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref isAlly47);
				int count9 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref count9);
				this.GmCmd_AddDie(context, isAlly47, count9);
				result = -1;
				break;
			}
			case 78:
			{
				int argsCount74 = operation.ArgsCount;
				int num74 = argsCount74;
				if (num74 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				bool isAlly48 = false;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref isAlly48);
				this.GmCmd_HealAllMind(context, isAlly48);
				result = -1;
				break;
			}
			case 79:
			{
				int argsCount75 = operation.ArgsCount;
				int num75 = argsCount75;
				if (num75 != 2)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				bool isAlly49 = false;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref isAlly49);
				bool isInner4 = false;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref isInner4);
				this.GmCmd_HealInjury(context, isAlly49, isInner4);
				result = -1;
				break;
			}
			case 80:
			{
				int argsCount76 = operation.ArgsCount;
				int num76 = argsCount76;
				if (num76 != 2)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				bool isAlly50 = false;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref isAlly50);
				int count10 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref count10);
				this.GmCmd_AddMind(context, isAlly50, count10);
				result = -1;
				break;
			}
			case 81:
			{
				int argsCount77 = operation.ArgsCount;
				int num77 = argsCount77;
				if (num77 != 2)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				short combatConfigId3 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref combatConfigId3);
				int[] enemyTeam2 = null;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref enemyTeam2);
				int returnValue22 = this.SimulatePrepareCombat(context, combatConfigId3, enemyTeam2);
				result = Serializer.Serialize(returnValue22, returnDataPool);
				break;
			}
			case 82:
			{
				int argsCount78 = operation.ArgsCount;
				int num78 = argsCount78;
				if (num78 != 1)
				{
					if (num78 != 2)
					{
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
						defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
						defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
						throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
					}
					short targetDistance = 0;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref targetDistance);
					bool isAlly51 = false;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref isAlly51);
					this.SetTargetDistance(context, targetDistance, isAlly51);
					result = -1;
				}
				else
				{
					short targetDistance2 = 0;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref targetDistance2);
					this.SetTargetDistance(context, targetDistance2, true);
					result = -1;
				}
				break;
			}
			case 83:
			{
				int argsCount79 = operation.ArgsCount;
				int num79 = argsCount79;
				if (num79 != 0)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				this.ClearTargetDistance(context);
				result = -1;
				break;
			}
			case 84:
			{
				int argsCount80 = operation.ArgsCount;
				int num80 = argsCount80;
				if (num80 != 2)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				short combatSkillId = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref combatSkillId);
				short jumpThreshold = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref jumpThreshold);
				bool returnValue23 = this.SetJumpThreshold(context, combatSkillId, jumpThreshold);
				result = Serializer.Serialize(returnValue23, returnDataPool);
				break;
			}
			case 85:
			{
				int argsCount81 = operation.ArgsCount;
				int num81 = argsCount81;
				if (num81 != 1)
				{
					if (num81 != 2)
					{
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
						defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
						defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
						throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
					}
					short skillId4 = 0;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref skillId4);
					int weaponIndex4 = 0;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref weaponIndex4);
					OuterAndInnerShorts returnValue24 = this.GetPreviewAttackRange(skillId4, weaponIndex4);
					result = Serializer.Serialize(returnValue24, returnDataPool);
				}
				else
				{
					short skillId5 = 0;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref skillId5);
					OuterAndInnerShorts returnValue25 = this.GetPreviewAttackRange(skillId5, -1);
					result = Serializer.Serialize(returnValue25, returnDataPool);
				}
				break;
			}
			case 86:
			{
				int argsCount82 = operation.ArgsCount;
				int num82 = argsCount82;
				if (num82 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				bool unyieldingFallen = false;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref unyieldingFallen);
				bool returnValue26 = this.SetPuppetUnyieldingFallen(context, unyieldingFallen);
				result = Serializer.Serialize(returnValue26, returnDataPool);
				break;
			}
			case 87:
			{
				int argsCount83 = operation.ArgsCount;
				int num83 = argsCount83;
				if (num83 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				bool disableAi = false;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref disableAi);
				bool returnValue27 = this.SetPuppetDisableAi(context, disableAi);
				result = Serializer.Serialize(returnValue27, returnDataPool);
				break;
			}
			case 88:
			{
				int argsCount84 = operation.ArgsCount;
				int num84 = argsCount84;
				if (num84 != 0)
				{
					if (num84 != 1)
					{
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
						defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
						defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
						throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
					}
					bool isAlly52 = false;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref isAlly52);
					bool returnValue28 = this.InterruptSkillManual(context, isAlly52);
					result = Serializer.Serialize(returnValue28, returnDataPool);
				}
				else
				{
					bool returnValue29 = this.InterruptSkillManual(context, true);
					result = Serializer.Serialize(returnValue29, returnDataPool);
				}
				break;
			}
			case 89:
			{
				int argsCount85 = operation.ArgsCount;
				int num85 = argsCount85;
				if (num85 != 2)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				short combatConfigId4 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref combatConfigId4);
				int[] enemyTeam3 = null;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref enemyTeam3);
				TeammateCommandChangeData returnValue30 = this.ProcessCombatTeammateCommands(context, combatConfigId4, enemyTeam3);
				result = Serializer.Serialize(returnValue30, returnDataPool);
				break;
			}
			case 90:
			{
				int argsCount86 = operation.ArgsCount;
				int num86 = argsCount86;
				if (num86 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				bool isAlly53 = false;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref isAlly53);
				this.ClearAffectingMoveSkillManual(context, isAlly53);
				result = -1;
				break;
			}
			case 91:
			{
				int argsCount87 = operation.ArgsCount;
				int num87 = argsCount87;
				if (num87 != 1)
				{
					if (num87 != 2)
					{
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
						defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
						defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
						throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
					}
					int index4 = 0;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref index4);
					bool isAlly54 = false;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref isAlly54);
					this.UnlockAttack(context, index4, isAlly54);
					result = -1;
				}
				else
				{
					int index5 = 0;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref index5);
					this.UnlockAttack(context, index5, true);
					result = -1;
				}
				break;
			}
			case 92:
			{
				int argsCount88 = operation.ArgsCount;
				int num88 = argsCount88;
				if (num88 != 0)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				bool returnValue31 = this.IgnoreAllRawCreate(context);
				result = Serializer.Serialize(returnValue31, returnDataPool);
				break;
			}
			case 93:
			{
				int argsCount89 = operation.ArgsCount;
				int num89 = argsCount89;
				if (num89 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				int effectId = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref effectId);
				bool returnValue32 = this.IgnoreRawCreate(context, effectId);
				result = Serializer.Serialize(returnValue32, returnDataPool);
				break;
			}
			case 94:
			{
				int argsCount90 = operation.ArgsCount;
				int num90 = argsCount90;
				if (num90 != 3)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				int effectId2 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref effectId2);
				sbyte equipmentSlot = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref equipmentSlot);
				short newTemplateId = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref newTemplateId);
				bool returnValue33 = this.DoRawCreate(context, effectId2, equipmentSlot, newTemplateId);
				result = Serializer.Serialize(returnValue33, returnDataPool);
				break;
			}
			case 95:
			{
				int argsCount91 = operation.ArgsCount;
				int num91 = argsCount91;
				if (num91 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				int effectId3 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref effectId3);
				List<sbyte> returnValue34 = this.GetAllCanRawCreateEquipmentSlots(effectId3);
				result = Serializer.Serialize(returnValue34, returnDataPool);
				break;
			}
			case 96:
			{
				int argsCount92 = operation.ArgsCount;
				int num92 = argsCount92;
				if (num92 != 1)
				{
					if (num92 != 2)
					{
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
						defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
						defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
						throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
					}
					int index6 = 0;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref index6);
					bool isAlly55 = false;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref isAlly55);
					UnlockSimulateResult returnValue35 = this.GetUnlockSimulateResult(index6, isAlly55);
					result = Serializer.Serialize(returnValue35, returnDataPool);
				}
				else
				{
					int index7 = 0;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref index7);
					UnlockSimulateResult returnValue36 = this.GetUnlockSimulateResult(index7, true);
					result = Serializer.Serialize(returnValue36, returnDataPool);
				}
				break;
			}
			case 97:
			{
				int argsCount93 = operation.ArgsCount;
				int num93 = argsCount93;
				if (num93 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				int charId12 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref charId12);
				DefeatMarksCountOutOfCombatData returnValue37 = CombatDomain.GetDefeatMarksCountOutOfCombat(context, charId12);
				result = Serializer.Serialize(returnValue37, returnDataPool);
				break;
			}
			case 98:
			{
				int argsCount94 = operation.ArgsCount;
				int num94 = argsCount94;
				if (num94 != 2)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				CombatResultDisplayData combatResultData = null;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref combatResultData);
				List<ItemDisplayData> selectedLootItem = null;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref selectedLootItem);
				this.ApplyCombatResultDataEffect(context, combatResultData, selectedLootItem);
				result = -1;
				break;
			}
			case 99:
			{
				int argsCount95 = operation.ArgsCount;
				int num95 = argsCount95;
				if (num95 != 0)
				{
					if (num95 != 1)
					{
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
						defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
						defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
						throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
					}
					bool isAlly56 = false;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref isAlly56);
					this.ClearReserveNormalAttack(context, isAlly56);
					result = -1;
				}
				else
				{
					this.ClearReserveNormalAttack(context, true);
					result = -1;
				}
				break;
			}
			case 100:
			{
				int argsCount96 = operation.ArgsCount;
				int num96 = argsCount96;
				if (num96 != 2)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				int typeInt = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref typeInt);
				int index8 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref index8);
				CharacterDisplayData returnValue38 = this.ApplyVitalOnTeammate(context, typeInt, index8);
				result = Serializer.Serialize(returnValue38, returnDataPool);
				break;
			}
			case 101:
			{
				int argsCount97 = operation.ArgsCount;
				int num97 = argsCount97;
				if (num97 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				int typeInt2 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref typeInt2);
				int returnValue39 = this.RevertVitalOnTeammate(context, typeInt2);
				result = Serializer.Serialize(returnValue39, returnDataPool);
				break;
			}
			case 102:
			{
				int argsCount98 = operation.ArgsCount;
				int num98 = argsCount98;
				if (num98 != 0)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				this.GmCmd_ForceRecoverTeammateCommand(context);
				result = -1;
				break;
			}
			case 103:
			{
				int argsCount99 = operation.ArgsCount;
				int num99 = argsCount99;
				if (num99 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				int charId13 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref charId13);
				List<ItemDisplayData> returnValue40 = this.RequestValidItemsInCombat(charId13);
				result = Serializer.Serialize(returnValue40, returnDataPool);
				break;
			}
			case 104:
			{
				int argsCount100 = operation.ArgsCount;
				int num100 = argsCount100;
				if (num100 != 0)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				List<short> returnValue41 = this.RequestSwordFragmentSkillIds();
				result = Serializer.Serialize(returnValue41, returnDataPool);
				break;
			}
			case 105:
			{
				int argsCount101 = operation.ArgsCount;
				int num101 = argsCount101;
				if (num101 != 2)
				{
					if (num101 != 3)
					{
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
						defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
						defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
						throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
					}
					sbyte itemType = 0;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref itemType);
					short templateId = 0;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref templateId);
					bool isAlly57 = false;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref isAlly57);
					this.UseSpecialItem(context, itemType, templateId, isAlly57);
					result = -1;
				}
				else
				{
					sbyte itemType2 = 0;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref itemType2);
					short templateId2 = 0;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref templateId2);
					this.UseSpecialItem(context, itemType2, templateId2, true);
					result = -1;
				}
				break;
			}
			case 106:
			{
				int argsCount102 = operation.ArgsCount;
				int num102 = argsCount102;
				if (num102 != 0)
				{
					if (num102 != 1)
					{
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
						defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
						defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
						throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
					}
					bool isAlly58 = false;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref isAlly58);
					this.NormalAttackImmediate(context, isAlly58);
					result = -1;
				}
				else
				{
					this.NormalAttackImmediate(context, true);
					result = -1;
				}
				break;
			}
			default:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(21, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Unsupported methodId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			}
			return result;
		}

		// Token: 0x060065F6 RID: 26102 RVA: 0x003A7A3C File Offset: 0x003A5C3C
		public override void OnMonitorData(ushort dataId, ulong subId0, uint subId1, bool monitoring)
		{
			switch (dataId)
			{
			case 0:
				break;
			case 1:
				break;
			case 2:
				break;
			case 3:
				break;
			case 4:
				break;
			case 5:
				break;
			case 6:
				this._modificationsSkillPowerAddInCombat.ChangeRecording(monitoring);
				break;
			case 7:
				this._modificationsSkillPowerReduceInCombat.ChangeRecording(monitoring);
				break;
			case 8:
				this._modificationsSkillPowerReplaceInCombat.ChangeRecording(monitoring);
				break;
			case 9:
				break;
			case 10:
				break;
			case 11:
				break;
			case 12:
				break;
			case 13:
				break;
			case 14:
				break;
			case 15:
				break;
			case 16:
				break;
			case 17:
				break;
			case 18:
				break;
			case 19:
				break;
			case 20:
				break;
			case 21:
				break;
			case 22:
				break;
			case 23:
				break;
			case 24:
				break;
			case 25:
				break;
			case 26:
				break;
			case 27:
				break;
			case 28:
				break;
			case 29:
				break;
			case 30:
				break;
			case 31:
				break;
			case 32:
				break;
			default:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(19, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Unsupported dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			}
		}

		// Token: 0x060065F7 RID: 26103 RVA: 0x003A7B88 File Offset: 0x003A5D88
		public override int CheckModified(ushort dataId, ulong subId0, uint subId1, RawDataPool dataPool)
		{
			int result;
			switch (dataId)
			{
			case 0:
			{
				bool flag = !BaseGameDataDomain.IsModified(this.DataStates, 0);
				if (flag)
				{
					result = -1;
				}
				else
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 0);
					result = Serializer.Serialize(this._timeScale, dataPool);
				}
				break;
			}
			case 1:
			{
				bool flag2 = !BaseGameDataDomain.IsModified(this.DataStates, 1);
				if (flag2)
				{
					result = -1;
				}
				else
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 1);
					result = Serializer.Serialize(this._autoCombat, dataPool);
				}
				break;
			}
			case 2:
			{
				bool flag3 = !BaseGameDataDomain.IsModified(this.DataStates, 2);
				if (flag3)
				{
					result = -1;
				}
				else
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 2);
					result = Serializer.Serialize(this._combatFrame, dataPool);
				}
				break;
			}
			case 3:
			{
				bool flag4 = !BaseGameDataDomain.IsModified(this.DataStates, 3);
				if (flag4)
				{
					result = -1;
				}
				else
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 3);
					result = Serializer.Serialize(this._combatType, dataPool);
				}
				break;
			}
			case 4:
			{
				bool flag5 = !BaseGameDataDomain.IsModified(this.DataStates, 4);
				if (flag5)
				{
					result = -1;
				}
				else
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 4);
					result = Serializer.Serialize(this._currentDistance, dataPool);
				}
				break;
			}
			case 5:
			{
				bool flag6 = !BaseGameDataDomain.IsModified(this.DataStates, 5);
				if (flag6)
				{
					result = -1;
				}
				else
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 5);
					result = Serializer.Serialize(this._damageCompareData, dataPool);
				}
				break;
			}
			case 6:
			{
				bool flag7 = !BaseGameDataDomain.IsModified(this.DataStates, 6);
				if (flag7)
				{
					result = -1;
				}
				else
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 6);
					int offset = Serializer.SerializeModifications<CombatSkillKey>(this._skillPowerAddInCombat, dataPool, this._modificationsSkillPowerAddInCombat);
					this._modificationsSkillPowerAddInCombat.Reset();
					result = offset;
				}
				break;
			}
			case 7:
			{
				bool flag8 = !BaseGameDataDomain.IsModified(this.DataStates, 7);
				if (flag8)
				{
					result = -1;
				}
				else
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 7);
					int offset2 = Serializer.SerializeModifications<CombatSkillKey>(this._skillPowerReduceInCombat, dataPool, this._modificationsSkillPowerReduceInCombat);
					this._modificationsSkillPowerReduceInCombat.Reset();
					result = offset2;
				}
				break;
			}
			case 8:
			{
				bool flag9 = !BaseGameDataDomain.IsModified(this.DataStates, 8);
				if (flag9)
				{
					result = -1;
				}
				else
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 8);
					int offset3 = Serializer.SerializeModifications<CombatSkillKey>(this._skillPowerReplaceInCombat, dataPool, this._modificationsSkillPowerReplaceInCombat);
					this._modificationsSkillPowerReplaceInCombat.Reset();
					result = offset3;
				}
				break;
			}
			case 9:
			{
				bool flag10 = !BaseGameDataDomain.IsModified(this.DataStates, 9);
				if (flag10)
				{
					result = -1;
				}
				else
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 9);
					result = Serializer.Serialize(this._bgmIndex, dataPool);
				}
				break;
			}
			case 10:
				result = this.CheckModified_CombatCharacterDict((int)subId0, (ushort)subId1, dataPool);
				break;
			case 11:
			{
				bool flag11 = !BaseGameDataDomain.IsModified(this.DataStates, 11);
				if (flag11)
				{
					result = -1;
				}
				else
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 11);
					result = Serializer.Serialize(this._selfTeam, dataPool);
				}
				break;
			}
			case 12:
			{
				bool flag12 = !BaseGameDataDomain.IsModified(this.DataStates, 12);
				if (flag12)
				{
					result = -1;
				}
				else
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 12);
					result = Serializer.Serialize(this._selfCharId, dataPool);
				}
				break;
			}
			case 13:
			{
				bool flag13 = !BaseGameDataDomain.IsModified(this.DataStates, 13);
				if (flag13)
				{
					result = -1;
				}
				else
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 13);
					result = Serializer.Serialize(this._selfTeamWisdomType, dataPool);
				}
				break;
			}
			case 14:
			{
				bool flag14 = !BaseGameDataDomain.IsModified(this.DataStates, 14);
				if (flag14)
				{
					result = -1;
				}
				else
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 14);
					result = Serializer.Serialize(this._selfTeamWisdomCount, dataPool);
				}
				break;
			}
			case 15:
			{
				bool flag15 = !BaseGameDataDomain.IsModified(this.DataStates, 15);
				if (flag15)
				{
					result = -1;
				}
				else
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 15);
					result = Serializer.Serialize(this._enemyTeam, dataPool);
				}
				break;
			}
			case 16:
			{
				bool flag16 = !BaseGameDataDomain.IsModified(this.DataStates, 16);
				if (flag16)
				{
					result = -1;
				}
				else
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 16);
					result = Serializer.Serialize(this._enemyCharId, dataPool);
				}
				break;
			}
			case 17:
			{
				bool flag17 = !BaseGameDataDomain.IsModified(this.DataStates, 17);
				if (flag17)
				{
					result = -1;
				}
				else
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 17);
					result = Serializer.Serialize(this._enemyTeamWisdomType, dataPool);
				}
				break;
			}
			case 18:
			{
				bool flag18 = !BaseGameDataDomain.IsModified(this.DataStates, 18);
				if (flag18)
				{
					result = -1;
				}
				else
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 18);
					result = Serializer.Serialize(this._enemyTeamWisdomCount, dataPool);
				}
				break;
			}
			case 19:
			{
				bool flag19 = !BaseGameDataDomain.IsModified(this.DataStates, 19);
				if (flag19)
				{
					result = -1;
				}
				else
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 19);
					result = Serializer.Serialize(this._combatStatus, dataPool);
				}
				break;
			}
			case 20:
			{
				bool flag20 = !BaseGameDataDomain.IsModified(this.DataStates, 20);
				if (flag20)
				{
					result = -1;
				}
				else
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 20);
					result = Serializer.Serialize(this._showMercyOption, dataPool);
				}
				break;
			}
			case 21:
			{
				bool flag21 = !BaseGameDataDomain.IsModified(this.DataStates, 21);
				if (flag21)
				{
					result = -1;
				}
				else
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 21);
					result = Serializer.Serialize(this._selectedMercyOption, dataPool);
				}
				break;
			}
			case 22:
			{
				bool flag22 = !BaseGameDataDomain.IsModified(this.DataStates, 22);
				if (flag22)
				{
					result = -1;
				}
				else
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 22);
					result = Serializer.Serialize(this._carrierAnimalCombatCharId, dataPool);
				}
				break;
			}
			case 23:
			{
				bool flag23 = !BaseGameDataDomain.IsModified(this.DataStates, 23);
				if (flag23)
				{
					result = -1;
				}
				else
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 23);
					result = Serializer.Serialize(this._specialShowCombatCharId, dataPool);
				}
				break;
			}
			case 24:
			{
				bool flag24 = !BaseGameDataDomain.IsModified(this.DataStates, 24);
				if (flag24)
				{
					result = -1;
				}
				else
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 24);
					result = Serializer.Serialize(this._skillAttackedIndexAndHit, dataPool);
				}
				break;
			}
			case 25:
			{
				bool flag25 = !BaseGameDataDomain.IsModified(this.DataStates, 25);
				if (flag25)
				{
					result = -1;
				}
				else
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 25);
					result = Serializer.Serialize(this._waitingDelaySettlement, dataPool);
				}
				break;
			}
			case 26:
			{
				bool flag26 = !BaseGameDataDomain.IsModified(this.DataStates, 26);
				if (flag26)
				{
					result = -1;
				}
				else
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 26);
					result = Serializer.Serialize(this._showUseGoldenWire, dataPool);
				}
				break;
			}
			case 27:
			{
				bool flag27 = !BaseGameDataDomain.IsModified(this.DataStates, 27);
				if (flag27)
				{
					result = -1;
				}
				else
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 27);
					result = Serializer.Serialize(this._isPuppetCombat, dataPool);
				}
				break;
			}
			case 28:
			{
				bool flag28 = !BaseGameDataDomain.IsModified(this.DataStates, 28);
				if (flag28)
				{
					result = -1;
				}
				else
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 28);
					result = Serializer.Serialize(this._isPlaygroundCombat, dataPool);
				}
				break;
			}
			case 29:
				result = this.CheckModified_SkillDataDict((CombatSkillKey)subId0, (ushort)subId1, dataPool);
				break;
			case 30:
				result = this.CheckModified_WeaponDataDict((int)subId0, (ushort)subId1, dataPool);
				break;
			case 31:
			{
				bool flag29 = !BaseGameDataDomain.IsModified(this.DataStates, 31);
				if (flag29)
				{
					result = -1;
				}
				else
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 31);
					result = Serializer.Serialize(this._expectRatioData, dataPool);
				}
				break;
			}
			case 32:
			{
				bool flag30 = !BaseGameDataDomain.IsModified(this.DataStates, 32);
				if (flag30)
				{
					result = -1;
				}
				else
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 32);
					result = Serializer.Serialize(this._taiwuSpecialGroupCharIds, dataPool);
				}
				break;
			}
			default:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(19, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Unsupported dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			}
			return result;
		}

		// Token: 0x060065F8 RID: 26104 RVA: 0x003A8418 File Offset: 0x003A6618
		public override void ResetModifiedWrapper(ushort dataId, ulong subId0, uint subId1)
		{
			switch (dataId)
			{
			case 0:
			{
				bool flag = !BaseGameDataDomain.IsModified(this.DataStates, 0);
				if (!flag)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 0);
				}
				break;
			}
			case 1:
			{
				bool flag2 = !BaseGameDataDomain.IsModified(this.DataStates, 1);
				if (!flag2)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 1);
				}
				break;
			}
			case 2:
			{
				bool flag3 = !BaseGameDataDomain.IsModified(this.DataStates, 2);
				if (!flag3)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 2);
				}
				break;
			}
			case 3:
			{
				bool flag4 = !BaseGameDataDomain.IsModified(this.DataStates, 3);
				if (!flag4)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 3);
				}
				break;
			}
			case 4:
			{
				bool flag5 = !BaseGameDataDomain.IsModified(this.DataStates, 4);
				if (!flag5)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 4);
				}
				break;
			}
			case 5:
			{
				bool flag6 = !BaseGameDataDomain.IsModified(this.DataStates, 5);
				if (!flag6)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 5);
				}
				break;
			}
			case 6:
			{
				bool flag7 = !BaseGameDataDomain.IsModified(this.DataStates, 6);
				if (!flag7)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 6);
					this._modificationsSkillPowerAddInCombat.Reset();
				}
				break;
			}
			case 7:
			{
				bool flag8 = !BaseGameDataDomain.IsModified(this.DataStates, 7);
				if (!flag8)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 7);
					this._modificationsSkillPowerReduceInCombat.Reset();
				}
				break;
			}
			case 8:
			{
				bool flag9 = !BaseGameDataDomain.IsModified(this.DataStates, 8);
				if (!flag9)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 8);
					this._modificationsSkillPowerReplaceInCombat.Reset();
				}
				break;
			}
			case 9:
			{
				bool flag10 = !BaseGameDataDomain.IsModified(this.DataStates, 9);
				if (!flag10)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 9);
				}
				break;
			}
			case 10:
				this.ResetModifiedWrapper_CombatCharacterDict((int)subId0, (ushort)subId1);
				break;
			case 11:
			{
				bool flag11 = !BaseGameDataDomain.IsModified(this.DataStates, 11);
				if (!flag11)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 11);
				}
				break;
			}
			case 12:
			{
				bool flag12 = !BaseGameDataDomain.IsModified(this.DataStates, 12);
				if (!flag12)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 12);
				}
				break;
			}
			case 13:
			{
				bool flag13 = !BaseGameDataDomain.IsModified(this.DataStates, 13);
				if (!flag13)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 13);
				}
				break;
			}
			case 14:
			{
				bool flag14 = !BaseGameDataDomain.IsModified(this.DataStates, 14);
				if (!flag14)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 14);
				}
				break;
			}
			case 15:
			{
				bool flag15 = !BaseGameDataDomain.IsModified(this.DataStates, 15);
				if (!flag15)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 15);
				}
				break;
			}
			case 16:
			{
				bool flag16 = !BaseGameDataDomain.IsModified(this.DataStates, 16);
				if (!flag16)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 16);
				}
				break;
			}
			case 17:
			{
				bool flag17 = !BaseGameDataDomain.IsModified(this.DataStates, 17);
				if (!flag17)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 17);
				}
				break;
			}
			case 18:
			{
				bool flag18 = !BaseGameDataDomain.IsModified(this.DataStates, 18);
				if (!flag18)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 18);
				}
				break;
			}
			case 19:
			{
				bool flag19 = !BaseGameDataDomain.IsModified(this.DataStates, 19);
				if (!flag19)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 19);
				}
				break;
			}
			case 20:
			{
				bool flag20 = !BaseGameDataDomain.IsModified(this.DataStates, 20);
				if (!flag20)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 20);
				}
				break;
			}
			case 21:
			{
				bool flag21 = !BaseGameDataDomain.IsModified(this.DataStates, 21);
				if (!flag21)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 21);
				}
				break;
			}
			case 22:
			{
				bool flag22 = !BaseGameDataDomain.IsModified(this.DataStates, 22);
				if (!flag22)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 22);
				}
				break;
			}
			case 23:
			{
				bool flag23 = !BaseGameDataDomain.IsModified(this.DataStates, 23);
				if (!flag23)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 23);
				}
				break;
			}
			case 24:
			{
				bool flag24 = !BaseGameDataDomain.IsModified(this.DataStates, 24);
				if (!flag24)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 24);
				}
				break;
			}
			case 25:
			{
				bool flag25 = !BaseGameDataDomain.IsModified(this.DataStates, 25);
				if (!flag25)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 25);
				}
				break;
			}
			case 26:
			{
				bool flag26 = !BaseGameDataDomain.IsModified(this.DataStates, 26);
				if (!flag26)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 26);
				}
				break;
			}
			case 27:
			{
				bool flag27 = !BaseGameDataDomain.IsModified(this.DataStates, 27);
				if (!flag27)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 27);
				}
				break;
			}
			case 28:
			{
				bool flag28 = !BaseGameDataDomain.IsModified(this.DataStates, 28);
				if (!flag28)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 28);
				}
				break;
			}
			case 29:
				this.ResetModifiedWrapper_SkillDataDict((CombatSkillKey)subId0, (ushort)subId1);
				break;
			case 30:
				this.ResetModifiedWrapper_WeaponDataDict((int)subId0, (ushort)subId1);
				break;
			case 31:
			{
				bool flag29 = !BaseGameDataDomain.IsModified(this.DataStates, 31);
				if (!flag29)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 31);
				}
				break;
			}
			case 32:
			{
				bool flag30 = !BaseGameDataDomain.IsModified(this.DataStates, 32);
				if (!flag30)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 32);
				}
				break;
			}
			default:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(19, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Unsupported dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			}
		}

		// Token: 0x060065F9 RID: 26105 RVA: 0x003A8A9C File Offset: 0x003A6C9C
		public override bool IsModifiedWrapper(ushort dataId, ulong subId0, uint subId1)
		{
			bool result;
			switch (dataId)
			{
			case 0:
				result = BaseGameDataDomain.IsModified(this.DataStates, 0);
				break;
			case 1:
				result = BaseGameDataDomain.IsModified(this.DataStates, 1);
				break;
			case 2:
				result = BaseGameDataDomain.IsModified(this.DataStates, 2);
				break;
			case 3:
				result = BaseGameDataDomain.IsModified(this.DataStates, 3);
				break;
			case 4:
				result = BaseGameDataDomain.IsModified(this.DataStates, 4);
				break;
			case 5:
				result = BaseGameDataDomain.IsModified(this.DataStates, 5);
				break;
			case 6:
				result = BaseGameDataDomain.IsModified(this.DataStates, 6);
				break;
			case 7:
				result = BaseGameDataDomain.IsModified(this.DataStates, 7);
				break;
			case 8:
				result = BaseGameDataDomain.IsModified(this.DataStates, 8);
				break;
			case 9:
				result = BaseGameDataDomain.IsModified(this.DataStates, 9);
				break;
			case 10:
				result = this.IsModifiedWrapper_CombatCharacterDict((int)subId0, (ushort)subId1);
				break;
			case 11:
				result = BaseGameDataDomain.IsModified(this.DataStates, 11);
				break;
			case 12:
				result = BaseGameDataDomain.IsModified(this.DataStates, 12);
				break;
			case 13:
				result = BaseGameDataDomain.IsModified(this.DataStates, 13);
				break;
			case 14:
				result = BaseGameDataDomain.IsModified(this.DataStates, 14);
				break;
			case 15:
				result = BaseGameDataDomain.IsModified(this.DataStates, 15);
				break;
			case 16:
				result = BaseGameDataDomain.IsModified(this.DataStates, 16);
				break;
			case 17:
				result = BaseGameDataDomain.IsModified(this.DataStates, 17);
				break;
			case 18:
				result = BaseGameDataDomain.IsModified(this.DataStates, 18);
				break;
			case 19:
				result = BaseGameDataDomain.IsModified(this.DataStates, 19);
				break;
			case 20:
				result = BaseGameDataDomain.IsModified(this.DataStates, 20);
				break;
			case 21:
				result = BaseGameDataDomain.IsModified(this.DataStates, 21);
				break;
			case 22:
				result = BaseGameDataDomain.IsModified(this.DataStates, 22);
				break;
			case 23:
				result = BaseGameDataDomain.IsModified(this.DataStates, 23);
				break;
			case 24:
				result = BaseGameDataDomain.IsModified(this.DataStates, 24);
				break;
			case 25:
				result = BaseGameDataDomain.IsModified(this.DataStates, 25);
				break;
			case 26:
				result = BaseGameDataDomain.IsModified(this.DataStates, 26);
				break;
			case 27:
				result = BaseGameDataDomain.IsModified(this.DataStates, 27);
				break;
			case 28:
				result = BaseGameDataDomain.IsModified(this.DataStates, 28);
				break;
			case 29:
				result = this.IsModifiedWrapper_SkillDataDict((CombatSkillKey)subId0, (ushort)subId1);
				break;
			case 30:
				result = this.IsModifiedWrapper_WeaponDataDict((int)subId0, (ushort)subId1);
				break;
			case 31:
				result = BaseGameDataDomain.IsModified(this.DataStates, 31);
				break;
			case 32:
				result = BaseGameDataDomain.IsModified(this.DataStates, 32);
				break;
			default:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(19, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Unsupported dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			}
			return result;
		}

		// Token: 0x060065FA RID: 26106 RVA: 0x003A8DC0 File Offset: 0x003A6FC0
		public override void InvalidateCache(BaseGameDataObject sourceObject, DataInfluence influence, DataContext context, bool unconditionallyInfluenceAll)
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			switch (influence.TargetIndicator.DataId)
			{
			case 0:
				break;
			case 1:
				break;
			case 2:
				break;
			case 3:
				break;
			case 4:
				break;
			case 5:
				break;
			case 6:
				break;
			case 7:
				break;
			case 8:
				break;
			case 9:
				break;
			case 10:
			{
				bool flag = !unconditionallyInfluenceAll;
				if (flag)
				{
					List<BaseGameDataObject> influencedObjects = InfluenceChecker.InfluencedObjectsPool.Get();
					bool influenceAll = InfluenceChecker.GetScope<int, CombatCharacter>(context, sourceObject, influence.Scope, this._combatCharacterDict, influencedObjects);
					bool flag2 = !influenceAll;
					if (flag2)
					{
						int influencedObjectsCount = influencedObjects.Count;
						for (int i = 0; i < influencedObjectsCount; i++)
						{
							BaseGameDataObject targetObject = influencedObjects[i];
							List<DataUid> targetUids = influence.TargetUids;
							int targetUidsCount = targetUids.Count;
							for (int j = 0; j < targetUidsCount; j++)
							{
								DataUid targetUid = targetUids[j];
								targetObject.InvalidateSelfAndInfluencedCache((ushort)targetUid.SubId1, context);
							}
						}
					}
					else
					{
						BaseGameDataDomain.InvalidateAllAndInfluencedCaches(CombatDomain.CacheInfluencesCombatCharacterDict, this._dataStatesCombatCharacterDict, influence, context);
					}
					influencedObjects.Clear();
					InfluenceChecker.InfluencedObjectsPool.Return(influencedObjects);
				}
				else
				{
					BaseGameDataDomain.InvalidateAllAndInfluencedCaches(CombatDomain.CacheInfluencesCombatCharacterDict, this._dataStatesCombatCharacterDict, influence, context);
				}
				return;
			}
			case 11:
				break;
			case 12:
				break;
			case 13:
				break;
			case 14:
				break;
			case 15:
				break;
			case 16:
				break;
			case 17:
				break;
			case 18:
				break;
			case 19:
				break;
			case 20:
				break;
			case 21:
				break;
			case 22:
				break;
			case 23:
				break;
			case 24:
				break;
			case 25:
				break;
			case 26:
				break;
			case 27:
				break;
			case 28:
				break;
			case 29:
			{
				bool flag3 = !unconditionallyInfluenceAll;
				if (flag3)
				{
					List<BaseGameDataObject> influencedObjects2 = InfluenceChecker.InfluencedObjectsPool.Get();
					bool influenceAll2 = InfluenceChecker.GetScope<CombatSkillKey, CombatSkillData>(context, sourceObject, influence.Scope, this._skillDataDict, influencedObjects2);
					bool flag4 = !influenceAll2;
					if (flag4)
					{
						int influencedObjectsCount2 = influencedObjects2.Count;
						for (int k = 0; k < influencedObjectsCount2; k++)
						{
							BaseGameDataObject targetObject2 = influencedObjects2[k];
							List<DataUid> targetUids2 = influence.TargetUids;
							int targetUidsCount2 = targetUids2.Count;
							for (int l = 0; l < targetUidsCount2; l++)
							{
								DataUid targetUid2 = targetUids2[l];
								targetObject2.InvalidateSelfAndInfluencedCache((ushort)targetUid2.SubId1, context);
							}
						}
					}
					else
					{
						BaseGameDataDomain.InvalidateAllAndInfluencedCaches(CombatDomain.CacheInfluencesSkillDataDict, this._dataStatesSkillDataDict, influence, context);
					}
					influencedObjects2.Clear();
					InfluenceChecker.InfluencedObjectsPool.Return(influencedObjects2);
				}
				else
				{
					BaseGameDataDomain.InvalidateAllAndInfluencedCaches(CombatDomain.CacheInfluencesSkillDataDict, this._dataStatesSkillDataDict, influence, context);
				}
				return;
			}
			case 30:
			{
				bool flag5 = !unconditionallyInfluenceAll;
				if (flag5)
				{
					List<BaseGameDataObject> influencedObjects3 = InfluenceChecker.InfluencedObjectsPool.Get();
					bool influenceAll3 = InfluenceChecker.GetScope<int, CombatWeaponData>(context, sourceObject, influence.Scope, this._weaponDataDict, influencedObjects3);
					bool flag6 = !influenceAll3;
					if (flag6)
					{
						int influencedObjectsCount3 = influencedObjects3.Count;
						for (int m = 0; m < influencedObjectsCount3; m++)
						{
							BaseGameDataObject targetObject3 = influencedObjects3[m];
							List<DataUid> targetUids3 = influence.TargetUids;
							int targetUidsCount3 = targetUids3.Count;
							for (int n = 0; n < targetUidsCount3; n++)
							{
								DataUid targetUid3 = targetUids3[n];
								targetObject3.InvalidateSelfAndInfluencedCache((ushort)targetUid3.SubId1, context);
							}
						}
					}
					else
					{
						BaseGameDataDomain.InvalidateAllAndInfluencedCaches(CombatDomain.CacheInfluencesWeaponDataDict, this._dataStatesWeaponDataDict, influence, context);
					}
					influencedObjects3.Clear();
					InfluenceChecker.InfluencedObjectsPool.Return(influencedObjects3);
				}
				else
				{
					BaseGameDataDomain.InvalidateAllAndInfluencedCaches(CombatDomain.CacheInfluencesWeaponDataDict, this._dataStatesWeaponDataDict, influence, context);
				}
				return;
			}
			case 31:
				break;
			case 32:
				break;
			default:
				defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(19, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Unsupported dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(influence.TargetIndicator.DataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(48, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Cannot invalidate cache state of non-cache data ");
			defaultInterpolatedStringHandler.AppendFormatted<ushort>(influence.TargetIndicator.DataId);
			throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
		}

		// Token: 0x060065FB RID: 26107 RVA: 0x003A9248 File Offset: 0x003A7448
		public unsafe override void ProcessArchiveResponse(OperationWrapper operation, byte* pResult)
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(52, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Cannot process archive response of non-archive data ");
			defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.DataId);
			throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
		}

		// Token: 0x060065FC RID: 26108 RVA: 0x003A9288 File Offset: 0x003A7488
		private void InitializeInternalDataOfCollections()
		{
			foreach (KeyValuePair<int, CombatCharacter> entry in this._combatCharacterDict)
			{
				CombatCharacter instance = entry.Value;
				instance.CollectionHelperData = this.HelperDataCombatCharacterDict;
				instance.DataStatesOffset = this._dataStatesCombatCharacterDict.Create();
			}
			foreach (KeyValuePair<CombatSkillKey, CombatSkillData> entry2 in this._skillDataDict)
			{
				CombatSkillData instance2 = entry2.Value;
				instance2.CollectionHelperData = this.HelperDataSkillDataDict;
				instance2.DataStatesOffset = this._dataStatesSkillDataDict.Create();
			}
			foreach (KeyValuePair<int, CombatWeaponData> entry3 in this._weaponDataDict)
			{
				CombatWeaponData instance3 = entry3.Value;
				instance3.CollectionHelperData = this.HelperDataWeaponDataDict;
				instance3.DataStatesOffset = this._dataStatesWeaponDataDict.Create();
			}
		}

		// Token: 0x060065FD RID: 26109 RVA: 0x003A93D0 File Offset: 0x003A75D0
		// Note: this type is marked as 'beforefieldinit'.
		static CombatDomain()
		{
			Dictionary<short, short> dictionary = new Dictionary<short, short>();
			dictionary[490] = 48;
			dictionary[491] = 57;
			dictionary[492] = 66;
			dictionary[493] = 75;
			dictionary[494] = 84;
			dictionary[495] = 93;
			dictionary[496] = 102;
			dictionary[497] = 111;
			dictionary[498] = 120;
			CombatDomain.BossPuppet2BossId = dictionary;
			CombatDomain.CharId2BossId = new Dictionary<short, sbyte>();
			Dictionary<ECombatEvaluationExtraCheck, Func<bool>> dictionary2 = new Dictionary<ECombatEvaluationExtraCheck, Func<bool>>();
			dictionary2[ECombatEvaluationExtraCheck.Fail] = new Func<bool>(CombatDomain.FailChecker);
			dictionary2[ECombatEvaluationExtraCheck.Draw] = new Func<bool>(CombatDomain.DrawChecker);
			dictionary2[ECombatEvaluationExtraCheck.Flee] = new Func<bool>(CombatDomain.FleeChecker);
			dictionary2[ECombatEvaluationExtraCheck.Win] = new Func<bool>(CombatDomain.WinChecker);
			dictionary2[ECombatEvaluationExtraCheck.FightSameLevel] = new Func<bool>(CombatDomain.FightSameLevelChecker);
			dictionary2[ECombatEvaluationExtraCheck.FightLessLevel] = new Func<bool>(CombatDomain.FightLessLevelChecker);
			dictionary2[ECombatEvaluationExtraCheck.BeatXiangShu] = new Func<bool>(CombatDomain.BeatXiangShuChecker);
			dictionary2[ECombatEvaluationExtraCheck.WinLess] = new Func<bool>(CombatDomain.WinLessChecker);
			dictionary2[ECombatEvaluationExtraCheck.WinChild] = new Func<bool>(CombatDomain.WinChildChecker);
			dictionary2[ECombatEvaluationExtraCheck.WinWorseEquip] = new Func<bool>(CombatDomain.WinWorseEquipChecker);
			dictionary2[ECombatEvaluationExtraCheck.WinLessNeili] = new Func<bool>(CombatDomain.WinLessNeiliChecker);
			dictionary2[ECombatEvaluationExtraCheck.WinWorseSkill] = new Func<bool>(CombatDomain.WinWorseSkillChecker);
			dictionary2[ECombatEvaluationExtraCheck.WinLessConsummate] = new Func<bool>(CombatDomain.WinLessConsummateChecker);
			dictionary2[ECombatEvaluationExtraCheck.WinPregnant] = new Func<bool>(CombatDomain.WinPregnantChecker);
			dictionary2[ECombatEvaluationExtraCheck.WinMore] = new Func<bool>(CombatDomain.WinMoreChecker);
			dictionary2[ECombatEvaluationExtraCheck.WinOlder] = new Func<bool>(CombatDomain.WinOlderChecker);
			dictionary2[ECombatEvaluationExtraCheck.WinBetterEquip] = new Func<bool>(CombatDomain.WinBetterEquipChecker);
			dictionary2[ECombatEvaluationExtraCheck.WinMoreNeili] = new Func<bool>(CombatDomain.WinMoreNeiliChecker);
			dictionary2[ECombatEvaluationExtraCheck.WinBetterSkill] = new Func<bool>(CombatDomain.WinBetterSkillChecker);
			dictionary2[ECombatEvaluationExtraCheck.WinMoreConsummate] = new Func<bool>(CombatDomain.WinMoreConsummateChecker);
			dictionary2[ECombatEvaluationExtraCheck.WinInPregnant] = new Func<bool>(CombatDomain.WinInPregnantChecker);
			dictionary2[ECombatEvaluationExtraCheck.KillBad0] = new Func<bool>(CombatDomain.KillBad0Checker);
			dictionary2[ECombatEvaluationExtraCheck.KillBad1] = new Func<bool>(CombatDomain.KillBad1Checker);
			dictionary2[ECombatEvaluationExtraCheck.KillGood0] = new Func<bool>(CombatDomain.KillGood0Checker);
			dictionary2[ECombatEvaluationExtraCheck.KillGood1] = new Func<bool>(CombatDomain.KillGood1Checker);
			dictionary2[ECombatEvaluationExtraCheck.KillMinion0] = new Func<bool>(CombatDomain.KillMinion0Checker);
			dictionary2[ECombatEvaluationExtraCheck.KillMinion1] = new Func<bool>(CombatDomain.KillMinion1Checker);
			dictionary2[ECombatEvaluationExtraCheck.ShixiangBuff0] = new Func<bool>(CombatDomain.ShixiangBuff0Checker);
			dictionary2[ECombatEvaluationExtraCheck.ShixiangBuff1] = new Func<bool>(CombatDomain.ShixiangBuff1Checker);
			dictionary2[ECombatEvaluationExtraCheck.ShixiangBuff2] = new Func<bool>(CombatDomain.ShixiangBuff2Checker);
			dictionary2[ECombatEvaluationExtraCheck.PuppetCombat] = new Func<bool>(CombatDomain.PuppetCombatChecker);
			dictionary2[ECombatEvaluationExtraCheck.OutBossCombat] = new Func<bool>(CombatDomain.OutBossCombatChecker);
			dictionary2[ECombatEvaluationExtraCheck.WinLoong] = new Func<bool>(CombatDomain.WinLoongChecker);
			dictionary2[ECombatEvaluationExtraCheck.CombatHard] = new Func<bool>(CombatDomain.CombatHardChecker);
			dictionary2[ECombatEvaluationExtraCheck.CombatVeryHard] = new Func<bool>(CombatDomain.CombatVeryHardChecker);
			CombatDomain.EvaluationCheckers = dictionary2;
			CombatDomain.WinNeiliMinDelta = 25;
			CombatDomain.InjuryAni = new string[]
			{
				"H_003",
				"H_004",
				"H_005"
			};
			CombatDomain.AvoidAni = new string[]
			{
				"H_002",
				"H_001",
				"H_000",
				"H_002"
			};
			CombatDomain.CachedInjuryRandomPool = new List<ValueTuple<sbyte, bool>>();
			CombatDomain.AddCaptureRateEquipSlot = new sbyte[]
			{
				8,
				9,
				10,
				11
			};
			CombatDomain.SpecialCharBlockAni = new List<string>
			{
				"T_001_0_1",
				"T_001_0_2"
			};
			Dictionary<sbyte, sbyte> dictionary3 = new Dictionary<sbyte, sbyte>();
			dictionary3[0] = 3;
			dictionary3[1] = 5;
			dictionary3[2] = 4;
			dictionary3[3] = 9;
			CombatDomain.GodTrickUseTrickType = dictionary3;
			CombatDomain.OtherActionSpecialEffectId = new int[]
			{
				1457,
				1459
			};
			CombatDomain.OtherActionPrepareFrame = new short[]
			{
				320,
				320,
				600,
				120,
				-1
			};
			CombatDomain.MixPoisonEffectImplements = new Dictionary<sbyte, MixPoisonEffectDelegate>();
			CombatDomain.NoResourceTypeStepSound = new string[]
			{
				"se_combat_foot_empty_1",
				"se_combat_foot_empty_2"
			};
			CombatDomain.TeammateCommandOptions = new Dictionary<ETeammateCommandImplement, ETeammateCommandOption>();
			CombatDomain.TeammateCommandCheckers = new Dictionary<ETeammateCommandImplement, ITeammateCommandChecker>
			{
				{
					ETeammateCommandImplement.Fight,
					new TeammateCommandCheckerFight()
				},
				{
					ETeammateCommandImplement.AccelerateCast,
					new TeammateCommandCheckerAccelerateCast()
				},
				{
					ETeammateCommandImplement.Push,
					new TeammateCommandCheckerPush()
				},
				{
					ETeammateCommandImplement.Pull,
					new TeammateCommandCheckerPull()
				},
				{
					ETeammateCommandImplement.Attack,
					new TeammateCommandCheckerAttack()
				},
				{
					ETeammateCommandImplement.AttackSkill,
					new TeammateCommandCheckerAttackSkill()
				},
				{
					ETeammateCommandImplement.Defend,
					new TeammateCommandCheckerDefendSkill()
				},
				{
					ETeammateCommandImplement.HealInjury,
					new TeammateCommandCheckerHealInjury()
				},
				{
					ETeammateCommandImplement.HealPoison,
					new TeammateCommandCheckerHealPoison()
				},
				{
					ETeammateCommandImplement.HealFlaw,
					new TeammateCommandCheckerHealFlaw()
				},
				{
					ETeammateCommandImplement.HealAcupoint,
					new TeammateCommandCheckerHealAcupoint()
				},
				{
					ETeammateCommandImplement.AddHit,
					new TeammateCommandCheckerAddHit()
				},
				{
					ETeammateCommandImplement.AddAvoid,
					new TeammateCommandCheckerAddAvoid()
				},
				{
					ETeammateCommandImplement.StopEnemy,
					new TeammateCommandCheckerStopEnemy()
				},
				{
					ETeammateCommandImplement.TransferNeiliAllocation,
					new TeammateCommandCheckerTransferNeiliAllocation()
				},
				{
					ETeammateCommandImplement.TransferInjury,
					new TeammateCommandCheckerTransferInjury()
				},
				{
					ETeammateCommandImplement.InterruptSkill,
					new TeammateCommandCheckerInterruptSkill()
				},
				{
					ETeammateCommandImplement.PushOrPullIntoDanger,
					new TeammateCommandCheckerPushOrPullIntoDanger()
				},
				{
					ETeammateCommandImplement.AttackFlawAndAcupoint,
					new TeammateCommandCheckerAttackFlawAndAcupoint()
				},
				{
					ETeammateCommandImplement.ClearAgileAndDefense,
					new TeammateCommandCheckerClearAgileAndDefense()
				},
				{
					ETeammateCommandImplement.AddInjuryAndPoison,
					new TeammateCommandCheckerAddInjuryAndPoison()
				},
				{
					ETeammateCommandImplement.ReduceHitAndAvoid,
					new TeammateCommandCheckerReduceHitAndAvoid()
				},
				{
					ETeammateCommandImplement.InterruptOtherAction,
					new TeammateCommandCheckerInterruptOtherAction()
				},
				{
					ETeammateCommandImplement.ReduceNeiliAllocation,
					new TeammateCommandCheckerReduceNeiliAllocation()
				},
				{
					ETeammateCommandImplement.AnimalEffect,
					new TeammateCommandCheckerAnimalEffect()
				},
				{
					ETeammateCommandImplement.GearMateA,
					new TeammateCommandCheckerGearMateA()
				},
				{
					ETeammateCommandImplement.GearMateB,
					new TeammateCommandCheckerGearMateB()
				},
				{
					ETeammateCommandImplement.GearMateC,
					new TeammateCommandCheckerGearMateC()
				},
				{
					ETeammateCommandImplement.AddUnlockAttackValue,
					new TeammateCommandCheckerAddUnlockAttackValue()
				},
				{
					ETeammateCommandImplement.TransferManyMark,
					new TeammateCommandCheckerTransferManyMark()
				},
				{
					ETeammateCommandImplement.RepairItem,
					new TeammateCommandCheckerRepairItem()
				},
				{
					ETeammateCommandImplement.VitalDemonA,
					new TeammateCommandCheckerVitalDemonA()
				},
				{
					ETeammateCommandImplement.VitalDemonB,
					new TeammateCommandCheckerVitalDemonB()
				},
				{
					ETeammateCommandImplement.VitalDemonC,
					new TeammateCommandCheckerVitalDemonC()
				}
			};
			CombatDomain.FailAni = new string[]
			{
				"C_005",
				"C_012",
				"C_011",
				"C_005"
			};
			CombatDomain.CacheInfluences = new DataInfluence[33][];
			CombatDomain.CacheInfluencesCombatCharacterDict = new DataInfluence[145][];
			CombatDomain.CacheInfluencesSkillDataDict = new DataInfluence[10][];
			CombatDomain.CacheInfluencesWeaponDataDict = new DataInfluence[10][];
		}

		// Token: 0x060065FE RID: 26110 RVA: 0x003A9ABC File Offset: 0x003A7CBC
		[CompilerGenerated]
		internal static int <ResultCalcAreaSpiritualDebt>g__CalcAddAreaSpiritualDebt|285_2(ref CombatDomain.<>c__DisplayClass285_0 A_0)
		{
			int debt = 0;
			for (int i = 0; i < A_0.enemyTeam.Length; i++)
			{
				int charId = A_0.enemyTeam[i];
				bool flag = charId < 0 || CombatDomain.<ResultCalcAreaSpiritualDebt>g__IsGuardChar|285_3(charId, ref A_0);
				if (!flag)
				{
					short randomEnemyId = Config.Character.Instance[DomainManager.Character.GetElement_Objects(charId).GetTemplateId()].RandomEnemyId;
					bool flag2 = randomEnemyId < 0;
					if (!flag2)
					{
						debt += (int)RandomEnemy.Instance[randomEnemyId].SpiritualDebt;
					}
				}
			}
			return debt;
		}

		// Token: 0x060065FF RID: 26111 RVA: 0x003A9B4C File Offset: 0x003A7D4C
		[CompilerGenerated]
		internal static bool <ResultCalcAreaSpiritualDebt>g__IsGuardChar|285_3(int chId, ref CombatDomain.<>c__DisplayClass285_0 A_1)
		{
			GameData.Domains.Character.Character ch = DomainManager.Character.GetElement_Objects(chId);
			bool flag = ch.GetCreatingType() != 2;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				for (int i = 0; i < A_1.enemyTeam.Length; i++)
				{
					int charId = A_1.enemyTeam[i];
					bool flag2 = charId < 0;
					if (!flag2)
					{
						bool flag3 = DomainManager.Character.GetElement_Objects(charId).GetCreatingType() == 1;
						if (flag3)
						{
							return true;
						}
					}
				}
				result = false;
			}
			return result;
		}

		// Token: 0x06006600 RID: 26112 RVA: 0x003A9BD0 File Offset: 0x003A7DD0
		[CompilerGenerated]
		internal static int <CalcMixedInjuryMark>g__ModifyMarkCount|355_0(bool inner, ref CombatDomain.<>c__DisplayClass355_0 A_1)
		{
			EDamageType damageType = inner ? A_1.context.InnerDamageType : A_1.context.OuterDamageType;
			int markCount = inner ? A_1.markCounts.Inner : A_1.markCounts.Outer;
			bool flag = markCount <= 0;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				sbyte bodyPart = A_1.context.BodyPart;
				short skillId = A_1.context.SkillTemplateId;
				if (!true)
				{
				}
				int num;
				if (damageType != EDamageType.Direct)
				{
					if (damageType != EDamageType.FightBack)
					{
						num = markCount;
					}
					else
					{
						num = DomainManager.SpecialEffect.ModifyValue(A_1.context.AttackerId, skillId, 87, markCount, (int)bodyPart, inner ? 1 : 0, markCount, 0, 0, 0, 0);
					}
				}
				else
				{
					num = DomainManager.SpecialEffect.ModifyValue(A_1.context.DefenderId, skillId, 116, markCount, (int)bodyPart, inner ? 1 : 0, markCount, 0, 0, 0, 0);
				}
				if (!true)
				{
				}
				int modifiedMarkCount = num;
				result = Math.Max(modifiedMarkCount, 0);
			}
			return result;
		}

		// Token: 0x06006601 RID: 26113 RVA: 0x003A9CC4 File Offset: 0x003A7EC4
		[CompilerGenerated]
		internal static int <ApplyMixedInjury>g__CalcLeftFatalDamage|357_0(bool inner, ref CombatDomain.<>c__DisplayClass357_0 A_1)
		{
			int leftDamage = inner ? A_1.innerResult.LeftDamage : A_1.outerResult.LeftDamage;
			sbyte markCount = A_1.context.Defender.GetInjuries().Get(A_1.context.BodyPart, inner);
			bool flag = (int)markCount + (inner ? A_1.innerResult.MarkCount : A_1.outerResult.MarkCount) < 6;
			int result;
			if (flag)
			{
				A_1.context.Defender.SetDamageValue(A_1.context, leftDamage, A_1.context.BodyPart, inner);
				result = 0;
			}
			else
			{
				bool flag2 = A_1.context.Defender.GetDamageValue(A_1.context.BodyPart, inner) != 0;
				if (flag2)
				{
					A_1.context.Defender.SetDamageValue(A_1.context, 0, A_1.context.BodyPart, inner);
				}
				result = leftDamage;
			}
			return result;
		}

		// Token: 0x06006602 RID: 26114 RVA: 0x003A9DBC File Offset: 0x003A7FBC
		[CompilerGenerated]
		internal static void <HealInjury>g__HealPartInjury|408_0(sbyte bodyPart, bool isInner, ref int allHealMarkCount, ref int maxHealMarkCount, ref int maxRequireAttainment, ref CombatDomain.<>c__DisplayClass408_0 A_5)
		{
			int damageValue = (A_5.patient == null) ? 0 : (isInner ? A_5.patient.GetInnerDamageValue() : A_5.patient.GetOuterDamageValue())[(int)bodyPart];
			int injuryStep = (isInner ? A_5.damageSteps.InnerDamageSteps : A_5.damageSteps.OuterDamageSteps)[(int)bodyPart];
			sbyte injuryCount = A_5.injuries.Get(bodyPart, isInner);
			int injury = (int)injuryCount * injuryStep + damageValue;
			bool flag = injury <= 0;
			if (!flag)
			{
				int requireAttainment;
				int healValue = CFormula.CalcHealInjuryValue(A_5.doctorAttainment, injuryStep, (int)injuryCount, out requireAttainment);
				maxRequireAttainment = Math.Max(maxRequireAttainment, requireAttainment);
				bool flag2 = healValue <= 0;
				if (!flag2)
				{
					int extraDebuffAddPercent = isInner ? A_5.extraInnerDebuffAddPercent : A_5.extraOuterDebuffAddPercent;
					healValue = DomainManager.SpecialEffect.ModifyValue(A_5.doctor.GetId(), 120, healValue, A_5.getCost ? 1 : 0, -1, -1, 0, extraDebuffAddPercent, 0, 0);
					int oldInjury = (int)A_5.oldInjuries.Get(bodyPart, isInner) * injuryStep;
					int newInjury = injury - oldInjury;
					bool flag3 = healValue < newInjury;
					if (flag3)
					{
						healValue = Math.Min(DomainManager.SpecialEffect.ModifyValue(A_5.doctor.GetId(), 119, healValue, A_5.getCost ? 1 : 0, -1, -1, 0, A_5.extraBuffAddPercent, 0, 0), newInjury);
					}
					int maxHealValue = healValue;
					healValue = Math.Min(healValue, A_5.canHealOld ? injury : (injury - oldInjury));
					int healMarkCount = healValue / injuryStep;
					Dictionary<int, int> changedDamageValue = isInner ? A_5.changedInnerDamageValue : A_5.changedOuterDamageValue;
					bool flag4 = A_5.inCombat && changedDamageValue != null;
					if (flag4)
					{
						int newDamageValue = damageValue;
						int healInjuryValue = healValue % injuryStep;
						newDamageValue -= healInjuryValue;
						bool flag5 = newDamageValue < 0;
						if (flag5)
						{
							healMarkCount++;
							newDamageValue += injuryStep;
						}
						bool flag6 = newDamageValue != damageValue;
						if (flag6)
						{
							changedDamageValue[(int)bodyPart] = newDamageValue;
						}
					}
					A_5.injuries.Change(bodyPart, isInner, (int)((sbyte)(-(sbyte)healMarkCount)));
					allHealMarkCount += healMarkCount;
					maxHealMarkCount += maxHealValue / injuryStep;
				}
			}
		}

		// Token: 0x06006603 RID: 26115 RVA: 0x003A9FCC File Offset: 0x003A81CC
		[CompilerGenerated]
		private void <ApplyEquipmentPoison>g__AddPoisonByTuple|561_0([TupleElementNames(new string[]
		{
			"value",
			"level"
		})] ValueTuple<short, sbyte> tuple, ref CombatDomain.<>c__DisplayClass561_0 A_2)
		{
			bool flag = tuple.Item1 <= 0;
			if (!flag)
			{
				this.AddPoison(A_2.context, A_2.poisoner, A_2.victim, A_2.poisonType, tuple.Item2, (int)tuple.Item1 * A_2.valueMultiplier, -1, true, true, A_2.itemKey, true, false, false);
			}
		}

		// Token: 0x06006604 RID: 26116 RVA: 0x003AA02A File Offset: 0x003A822A
		[CompilerGenerated]
		internal static bool <CalcAttackSkillDataCompare>g__CanHit|610_0(int index, ref CombatDomain.<>c__DisplayClass610_0 A_1)
		{
			return A_1.attacker.SkillHitValue[index] >= 0 && A_1.attacker.SkillHitValue[index] >= A_1.attacker.SkillAvoidValue[index];
		}

		// Token: 0x06006605 RID: 26117 RVA: 0x003AA05E File Offset: 0x003A825E
		[CompilerGenerated]
		internal static int <CalcAttackSkillDataCompare>g__CalcHitOdds|610_1(int index, ref CombatDomain.<>c__DisplayClass610_0 A_1)
		{
			return A_1.attacker.SkillHitValue[index] * 100 / Math.Max(A_1.attacker.SkillAvoidValue[index], 1);
		}

		// Token: 0x06006606 RID: 26118 RVA: 0x003AA084 File Offset: 0x003A8284
		[CompilerGenerated]
		internal static int <AddGoneMadInjury>g__ModifyValue|616_0(int value, ref CombatDomain.<>c__DisplayClass616_0 A_1)
		{
			value = DomainManager.SpecialEffect.ModifyValueCustom(A_1.character.GetId(), A_1.skillId, 117, value, -1, -1, -1, 0, 0, A_1.extraTotalPercent, A_1.extraTotalPercent) * A_1.factor;
			return DomainManager.SpecialEffect.ModifyData(A_1.character.GetId(), A_1.skillId, 322, value, A_1.inner ? 1 : 0, (int)A_1.part, A_1.addingDisorderOfQi ? 1 : 0);
		}

		// Token: 0x06006607 RID: 26119 RVA: 0x003AA116 File Offset: 0x003A8316
		[CompilerGenerated]
		internal static bool <OnCastSkillEndFeature>g__CheckProb|629_0(ref CombatDomain.<>c__DisplayClass629_0 A_0, ref CombatDomain.<>c__DisplayClass629_1 A_1)
		{
			return A_0.context.Random.CheckPercentProb(A_0.finalCriticalOdds * Math.Abs(A_1.config.CriticalProbPercent));
		}

		// Token: 0x06006608 RID: 26120 RVA: 0x003AA148 File Offset: 0x003A8348
		[CompilerGenerated]
		internal static bool <OnCastSkillEndFeature>g__FiveElementEquals|629_1(int fiveElementType, ref CombatDomain.<>c__DisplayClass629_0 A_1)
		{
			return CombatSkillDomain.FiveElementEquals(A_1.attacker.GetId(), A_1.skillId, (sbyte)fiveElementType);
		}

		// Token: 0x06006609 RID: 26121 RVA: 0x003AA164 File Offset: 0x003A8364
		[CompilerGenerated]
		internal static void <UpdateTeammateCommandUsable>g__UpdateBanReasons|715_0(ETeammateCommandImplement cmdImplement, int index, ref CombatDomain.<>c__DisplayClass715_0 A_2)
		{
			A_2.tempBanReasons.Clear();
			ITeammateCommandChecker checker;
			bool flag = CombatDomain.TeammateCommandCheckers.TryGetValue(cmdImplement, out checker);
			if (flag)
			{
				A_2.tempBanReasons.AddRange(from x in checker.Check(index, A_2.checkerContext)
				select (sbyte)x);
			}
			else
			{
				A_2.tempBanReasons.Add(-1);
			}
			SByteList cmdBanReasons = A_2.cmdBanReasonList[index];
			bool flag2 = cmdBanReasons.Items.SequenceEqual(A_2.tempBanReasons);
			if (!flag2)
			{
				cmdBanReasons.Items.Clear();
				cmdBanReasons.Items.AddRange(A_2.tempBanReasons);
				A_2.changed = true;
			}
		}

		// Token: 0x04001B08 RID: 6920
		[DomainData(DomainDataType.SingleValue, false, false, true, false)]
		private float _timeScale;

		// Token: 0x04001B09 RID: 6921
		[DomainData(DomainDataType.SingleValue, false, false, true, false)]
		private bool _autoCombat;

		// Token: 0x04001B0A RID: 6922
		private float _frameTimeAccumulator;

		// Token: 0x04001B0B RID: 6923
		[DomainData(DomainDataType.SingleValue, false, false, true, true)]
		private ulong _combatFrame;

		// Token: 0x04001B0C RID: 6924
		[DomainData(DomainDataType.SingleValue, false, false, true, true)]
		private sbyte _combatType;

		// Token: 0x04001B0D RID: 6925
		[DomainData(DomainDataType.SingleValue, false, false, true, true)]
		private bool _isPuppetCombat;

		// Token: 0x04001B0E RID: 6926
		[DomainData(DomainDataType.SingleValue, false, false, true, true)]
		private bool _isPlaygroundCombat;

		// Token: 0x04001B0F RID: 6927
		public CombatConfigItem CombatConfig;

		// Token: 0x04001B12 RID: 6930
		private bool _inBulletTime;

		// Token: 0x04001B13 RID: 6931
		private float _timeScaleSaveInBulletTime;

		// Token: 0x04001B14 RID: 6932
		public sbyte TestSkillCounter;

		// Token: 0x04001B15 RID: 6933
		private bool _saveDyingEffectTriggerd;

		// Token: 0x04001B16 RID: 6934
		public DataContext Context;

		// Token: 0x04001B17 RID: 6935
		private bool _isTutorialCombat;

		// Token: 0x04001B18 RID: 6936
		private bool _enableEnemyAi = true;

		// Token: 0x04001B19 RID: 6937
		private bool _enableSkillFreeCast = false;

		// Token: 0x04001B1A RID: 6938
		public AiOptions AiOptions = new AiOptions();

		// Token: 0x04001B1B RID: 6939
		public const short CombatStatePowerLimit = 500;

		// Token: 0x04001B1C RID: 6940
		public const sbyte MaxFlawCount = 3;

		// Token: 0x04001B1D RID: 6941
		public const sbyte MaxAcupointCount = 3;

		// Token: 0x04001B1E RID: 6942
		private const short NoArmorEquipAttack = 100;

		// Token: 0x04001B1F RID: 6943
		private const short NoArmorEquipDefense = 50;

		// Token: 0x04001B20 RID: 6944
		public const sbyte MinEffectPercent = 33;

		// Token: 0x04001B21 RID: 6945
		private readonly string[][] _avoidSound = new string[][]
		{
			new string[]
			{
				"se_combat_block_combat_1",
				"se_combat_block_combat_2",
				"se_combat_block_combat_3"
			},
			new string[]
			{
				"se_combat_block_combat_1",
				"se_combat_block_combat_2",
				"se_combat_block_combat_3"
			},
			new string[]
			{
				"se_combat_block_dodge_1",
				"se_combat_block_dodge_2",
				"se_combat_block_dodge_3"
			},
			new string[]
			{
				"se_combat_block_dodge_1",
				"se_combat_block_dodge_2",
				"se_combat_block_dodge_3"
			}
		};

		// Token: 0x04001B22 RID: 6946
		private readonly string[] _selfAvoidParticle = new string[]
		{
			"Particle_H_xieli",
			"Particle_H_chaizhao",
			"Particle_H_shanbi",
			"Particle_H_shouxin"
		};

		// Token: 0x04001B23 RID: 6947
		private readonly string[] _enemyAvoidParticle = new string[]
		{
			"Particle_H_xieli_1",
			"Particle_H_chaizhao_1",
			"Particle_H_shanbi_1",
			"Particle_H_shouxin_1"
		};

		// Token: 0x04001B24 RID: 6948
		private const string NoDamageParticle = "Particle_D_qidun";

		// Token: 0x04001B25 RID: 6949
		private const sbyte AnimalAttackPosOffset = 8;

		// Token: 0x04001B26 RID: 6950
		public static readonly Dictionary<short, short> BossPuppet2BossId;

		// Token: 0x04001B27 RID: 6951
		public static readonly Dictionary<short, sbyte> CharId2BossId;

		// Token: 0x04001B28 RID: 6952
		public const string IdleAni = "C_000";

		// Token: 0x04001B29 RID: 6953
		public const string WeakIdleAni = "C_000";

		// Token: 0x04001B2A RID: 6954
		public const string WalkForwardAni = "M_001";

		// Token: 0x04001B2B RID: 6955
		public const string WalkBackwardAni = "M_002";

		// Token: 0x04001B2C RID: 6956
		public const string WalkForwardFastAni = "MR_001";

		// Token: 0x04001B2D RID: 6957
		public const string WalkBackwardFastAni = "MR_002";

		// Token: 0x04001B2E RID: 6958
		public const string SkillMoveForwardAni = "M_003";

		// Token: 0x04001B2F RID: 6959
		public const string SkillMoveBackwardAni = "M_004";

		// Token: 0x04001B30 RID: 6960
		public const string JumpMovePrepareAni = "C_007";

		// Token: 0x04001B31 RID: 6961
		public const string JumpMoveForwardAni = "M_003_fly";

		// Token: 0x04001B32 RID: 6962
		public const string JumpMoveBackwardAni = "M_004_fly";

		// Token: 0x04001B33 RID: 6963
		public const string SlowForwardAni = "M_014";

		// Token: 0x04001B34 RID: 6964
		public const string SlowBackwardAni = "M_015";

		// Token: 0x04001B35 RID: 6965
		[DomainData(DomainDataType.SingleValue, false, false, true, true)]
		private short _currentDistance;

		// Token: 0x04001B36 RID: 6966
		private readonly CombatResultDisplayData _combatResultData = new CombatResultDisplayData();

		// Token: 0x04001B37 RID: 6967
		private static readonly Dictionary<ECombatEvaluationExtraCheck, Func<bool>> EvaluationCheckers;

		// Token: 0x04001B38 RID: 6968
		private static readonly CValuePercentBonus WinNeiliMinDelta;

		// Token: 0x04001B39 RID: 6969
		public sbyte SelfMaxSkillGrade;

		// Token: 0x04001B3A RID: 6970
		public sbyte EnemyMaxSkillGrade;

		// Token: 0x04001B3D RID: 6973
		private readonly List<int> _lootCharList = new List<int>();

		// Token: 0x04001B3E RID: 6974
		private const sbyte CombatCharAboutToFallEventSendTimes = 4;

		// Token: 0x04001B3F RID: 6975
		private static CombatDomain.OnCombatCharAboutToFall _handlersCombatCharAboutToFall;

		// Token: 0x04001B40 RID: 6976
		[DomainData(DomainDataType.SingleValue, false, false, true, true)]
		private DamageCompareData _damageCompareData;

		// Token: 0x04001B41 RID: 6977
		[DomainData(DomainDataType.SingleValue, false, false, true, true)]
		private IntPair _skillAttackedIndexAndHit;

		// Token: 0x04001B42 RID: 6978
		public static readonly string[] InjuryAni;

		// Token: 0x04001B43 RID: 6979
		public static readonly string[] AvoidAni;

		// Token: 0x04001B44 RID: 6980
		[TupleElementNames(new string[]
		{
			"bodyPart",
			"isInner"
		})]
		private static readonly List<ValueTuple<sbyte, bool>> CachedInjuryRandomPool;

		// Token: 0x04001B45 RID: 6981
		private static readonly sbyte[] AddCaptureRateEquipSlot;

		// Token: 0x04001B46 RID: 6982
		public const sbyte RopeEnsureHitConsummateGap = 6;

		// Token: 0x04001B47 RID: 6983
		[DomainData(DomainDataType.SingleValue, false, false, true, true)]
		private SpecialMiscData _showUseGoldenWire;

		// Token: 0x04001B48 RID: 6984
		public Dictionary<int, int> EquipmentPowerChangeInCombat = new Dictionary<int, int>();

		// Token: 0x04001B49 RID: 6985
		public Dictionary<ItemKey, int> EquipmentOldDurability = new Dictionary<ItemKey, int>();

		// Token: 0x04001B4A RID: 6986
		public const byte NormalWeaponCount = 3;

		// Token: 0x04001B4B RID: 6987
		public const byte MaxWeaponCount = 7;

		// Token: 0x04001B4C RID: 6988
		public const int WeaponIndexEmptyHand = 3;

		// Token: 0x04001B4D RID: 6989
		public const int WeaponIndexBranch = 4;

		// Token: 0x04001B4E RID: 6990
		public const int WeaponIndexStone = 5;

		// Token: 0x04001B4F RID: 6991
		public const int WeaponIndexVoice = 6;

		// Token: 0x04001B50 RID: 6992
		public const byte WeaponTrickCount = 6;

		// Token: 0x04001B51 RID: 6993
		public const byte MaxPursueAttack = 5;

		// Token: 0x04001B52 RID: 6994
		public const short NormalAttackMoveWaitFrame = 6;

		// Token: 0x04001B53 RID: 6995
		public static readonly List<string> SpecialCharBlockAni;

		// Token: 0x04001B54 RID: 6996
		private static readonly Dictionary<sbyte, sbyte> GodTrickUseTrickType;

		// Token: 0x04001B55 RID: 6997
		[DomainData(DomainDataType.ObjectCollection, false, false, true, true)]
		private Dictionary<int, CombatWeaponData> _weaponDataDict;

		// Token: 0x04001B56 RID: 6998
		[DomainData(DomainDataType.SingleValue, false, false, true, true)]
		private WeaponExpectInnerRatioData _expectRatioData;

		// Token: 0x04001B57 RID: 6999
		public const string OtherActionPrepareAni = "C_007";

		// Token: 0x04001B58 RID: 7000
		public static readonly int[] OtherActionSpecialEffectId;

		// Token: 0x04001B59 RID: 7001
		public static readonly short[] OtherActionPrepareFrame;

		// Token: 0x04001B5A RID: 7002
		public const short HealInjuryMinPrepareFrame = 120;

		// Token: 0x04001B5B RID: 7003
		public const short HealPoisonMinPrepareFrame = 120;

		// Token: 0x04001B5C RID: 7004
		private static readonly Dictionary<sbyte, MixPoisonEffectDelegate> MixPoisonEffectImplements;

		// Token: 0x04001B5D RID: 7005
		public const string CommonPrepareAni = "C_007";

		// Token: 0x04001B5E RID: 7006
		public const string SkillFinalAvoidAni = "H_008";

		// Token: 0x04001B5F RID: 7007
		public const string SkillPrepareSound = "se_combat_preskill";

		// Token: 0x04001B60 RID: 7008
		[DomainData(DomainDataType.ObjectCollection, false, false, true, true)]
		private Dictionary<CombatSkillKey, CombatSkillData> _skillDataDict;

		// Token: 0x04001B61 RID: 7009
		[DomainData(DomainDataType.SingleValueCollection, false, false, true, true)]
		private Dictionary<CombatSkillKey, SkillPowerChangeCollection> _skillPowerAddInCombat;

		// Token: 0x04001B62 RID: 7010
		[DomainData(DomainDataType.SingleValueCollection, false, false, true, true)]
		private Dictionary<CombatSkillKey, SkillPowerChangeCollection> _skillPowerReduceInCombat;

		// Token: 0x04001B63 RID: 7011
		[DomainData(DomainDataType.SingleValueCollection, false, false, true, true)]
		private Dictionary<CombatSkillKey, CombatSkillKey> _skillPowerReplaceInCombat;

		// Token: 0x04001B64 RID: 7012
		private readonly Dictionary<CombatSkillKey, int> _skillCastTimes = new Dictionary<CombatSkillKey, int>();

		// Token: 0x04001B65 RID: 7013
		private const string NoResourceTypeWhooshSound = "se_combat_whoosh_empty";

		// Token: 0x04001B66 RID: 7014
		private static readonly string[] NoResourceTypeStepSound;

		// Token: 0x04001B67 RID: 7015
		[DomainData(DomainDataType.SingleValue, false, false, true, true)]
		private sbyte _bgmIndex;

		// Token: 0x04001B68 RID: 7016
		private const byte TeamCapacity = 4;

		// Token: 0x04001B69 RID: 7017
		public const sbyte MaxTeammateCommandCount = 3;

		// Token: 0x04001B6A RID: 7018
		public const sbyte TeammateCommandBaseCdSpeed = 100;

		// Token: 0x04001B6B RID: 7019
		public const sbyte TeammateFightBaseBreathStancePercent = 50;

		// Token: 0x04001B6C RID: 7020
		public const sbyte TeammateFightBaseMobilityPercent = 50;

		// Token: 0x04001B6D RID: 7021
		[DomainData(DomainDataType.ObjectCollection, false, false, true, true)]
		private Dictionary<int, CombatCharacter> _combatCharacterDict;

		// Token: 0x04001B6E RID: 7022
		[DomainData(DomainDataType.SingleValue, false, false, true, true)]
		private List<int> _taiwuSpecialGroupCharIds;

		// Token: 0x04001B6F RID: 7023
		[DomainData(DomainDataType.SingleValue, false, false, true, true, ArrayElementsCount = 4)]
		private int[] _selfTeam;

		// Token: 0x04001B70 RID: 7024
		[DomainData(DomainDataType.SingleValue, false, false, true, true)]
		private int _selfCharId;

		// Token: 0x04001B71 RID: 7025
		private CombatCharacter _selfChar;

		// Token: 0x04001B72 RID: 7026
		[DomainData(DomainDataType.SingleValue, false, false, true, true)]
		private sbyte _selfTeamWisdomType;

		// Token: 0x04001B73 RID: 7027
		[DomainData(DomainDataType.SingleValue, false, false, true, true)]
		private short _selfTeamWisdomCount;

		// Token: 0x04001B74 RID: 7028
		[DomainData(DomainDataType.SingleValue, false, false, true, true, ArrayElementsCount = 4)]
		private int[] _enemyTeam;

		// Token: 0x04001B75 RID: 7029
		[DomainData(DomainDataType.SingleValue, false, false, true, true)]
		private int _enemyCharId;

		// Token: 0x04001B76 RID: 7030
		private CombatCharacter _enemyChar;

		// Token: 0x04001B77 RID: 7031
		[DomainData(DomainDataType.SingleValue, false, false, true, true)]
		private sbyte _enemyTeamWisdomType;

		// Token: 0x04001B78 RID: 7032
		[DomainData(DomainDataType.SingleValue, false, false, true, true)]
		private short _enemyTeamWisdomCount;

		// Token: 0x04001B79 RID: 7033
		[DomainData(DomainDataType.SingleValue, false, false, true, true)]
		private int _carrierAnimalCombatCharId;

		// Token: 0x04001B7A RID: 7034
		[DomainData(DomainDataType.SingleValue, false, false, true, true)]
		private int _specialShowCombatCharId;

		// Token: 0x04001B7B RID: 7035
		public TeammateCommandChangeData PreRandomizedTeammateCommandReplaceData;

		// Token: 0x04001B7C RID: 7036
		private Dictionary<SectStoryThreeVitalsCharacterType, int> _vitalTeammateData;

		// Token: 0x04001B7D RID: 7037
		public static readonly Dictionary<ETeammateCommandImplement, ETeammateCommandOption> TeammateCommandOptions;

		// Token: 0x04001B7E RID: 7038
		private static readonly Dictionary<ETeammateCommandImplement, ITeammateCommandChecker> TeammateCommandCheckers;

		// Token: 0x04001B7F RID: 7039
		private static readonly string[] FailAni;

		// Token: 0x04001B80 RID: 7040
		private const string WaitMercyAni = "C_011_stun";

		// Token: 0x04001B81 RID: 7041
		private const string FallAni = "C_005";

		// Token: 0x04001B82 RID: 7042
		[DomainData(DomainDataType.SingleValue, false, false, true, true)]
		private sbyte _combatStatus;

		// Token: 0x04001B83 RID: 7043
		[DomainData(DomainDataType.SingleValue, false, false, true, true)]
		private bool _waitingDelaySettlement;

		// Token: 0x04001B84 RID: 7044
		[DomainData(DomainDataType.SingleValue, false, false, true, true)]
		private sbyte _showMercyOption;

		// Token: 0x04001B85 RID: 7045
		[DomainData(DomainDataType.SingleValue, false, false, true, true)]
		private sbyte _selectedMercyOption;

		// Token: 0x04001B86 RID: 7046
		private readonly HashSet<int> _needCheckFallenCharSet = new HashSet<int>();

		// Token: 0x04001B87 RID: 7047
		private bool _skipCombatLoop;

		// Token: 0x04001B88 RID: 7048
		private static readonly DataInfluence[][] CacheInfluences;

		// Token: 0x04001B89 RID: 7049
		private SingleValueCollectionModificationCollection<CombatSkillKey> _modificationsSkillPowerAddInCombat = SingleValueCollectionModificationCollection<CombatSkillKey>.Create();

		// Token: 0x04001B8A RID: 7050
		private SingleValueCollectionModificationCollection<CombatSkillKey> _modificationsSkillPowerReduceInCombat = SingleValueCollectionModificationCollection<CombatSkillKey>.Create();

		// Token: 0x04001B8B RID: 7051
		private SingleValueCollectionModificationCollection<CombatSkillKey> _modificationsSkillPowerReplaceInCombat = SingleValueCollectionModificationCollection<CombatSkillKey>.Create();

		// Token: 0x04001B8C RID: 7052
		private static readonly DataInfluence[][] CacheInfluencesCombatCharacterDict;

		// Token: 0x04001B8D RID: 7053
		private readonly ObjectCollectionDataStates _dataStatesCombatCharacterDict = new ObjectCollectionDataStates(145, 0);

		// Token: 0x04001B8E RID: 7054
		public readonly ObjectCollectionHelperData HelperDataCombatCharacterDict;

		// Token: 0x04001B8F RID: 7055
		private static readonly DataInfluence[][] CacheInfluencesSkillDataDict;

		// Token: 0x04001B90 RID: 7056
		private readonly ObjectCollectionDataStates _dataStatesSkillDataDict = new ObjectCollectionDataStates(10, 0);

		// Token: 0x04001B91 RID: 7057
		public readonly ObjectCollectionHelperData HelperDataSkillDataDict;

		// Token: 0x04001B92 RID: 7058
		private static readonly DataInfluence[][] CacheInfluencesWeaponDataDict;

		// Token: 0x04001B93 RID: 7059
		private readonly ObjectCollectionDataStates _dataStatesWeaponDataDict = new ObjectCollectionDataStates(10, 0);

		// Token: 0x04001B94 RID: 7060
		public readonly ObjectCollectionHelperData HelperDataWeaponDataDict;

		// Token: 0x04001B95 RID: 7061
		private Queue<uint> _pendingLoadingOperationIds;

		// Token: 0x02000B50 RID: 2896
		// (Invoke) Token: 0x06008AB8 RID: 35512
		public delegate void OnCombatCharAboutToFall(DataContext context, CombatCharacter combatChar, int eventIndex);
	}
}
