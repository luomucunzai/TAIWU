using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Config;
using GameData.Domains.Map;
using GameData.Utilities;
using Redzen.Random;

namespace GameData.Domains.Extra
{
	// Token: 0x0200068D RID: 1677
	internal class TreasureMaterialHelper
	{
		// Token: 0x06005F60 RID: 24416 RVA: 0x00363FE0 File Offset: 0x003621E0
		private static bool IsValid(short templateId, int brokenLevel)
		{
			MiscItem config = Misc.Instance[templateId];
			return config.AllowBrokenLevels.Contains(brokenLevel);
		}

		// Token: 0x06005F61 RID: 24417 RVA: 0x0036400C File Offset: 0x0036220C
		private static bool IsValidExclusive(short templateId, int brokenLevel)
		{
			return TreasureMaterialHelper.IsValid(templateId, brokenLevel) && brokenLevel == Misc.Instance[templateId].AllowBrokenLevels.Min<int>();
		}

		// Token: 0x06005F62 RID: 24418 RVA: 0x00364044 File Offset: 0x00362244
		public TreasureMaterialHelper(EMiscGenerateType type)
		{
			bool flag = type == EMiscGenerateType.Invalid;
			if (!flag)
			{
				foreach (MiscItem misc in ((IEnumerable<MiscItem>)Misc.Instance))
				{
					bool flag2 = misc.GenerateType < type;
					if (!flag2)
					{
						foreach (TreasureStateInfo info in misc.StateBuryAmount)
						{
							bool flag3 = info.Amount <= 0;
							if (!flag3)
							{
								Dictionary<short, int> status = this._complexConfig.GetOrNew(info.MapState);
								status[misc.TemplateId] = status.GetOrDefault(misc.TemplateId) + (int)info.Amount;
							}
						}
					}
				}
				foreach (Dictionary<short, int> status2 in this._complexConfig.Values)
				{
					this._templateCaches.Clear();
					foreach (KeyValuePair<short, int> keyValuePair in status2)
					{
						short num;
						int num2;
						keyValuePair.Deconstruct(out num, out num2);
						short templateId = num;
						int amount = num2;
						bool flag4 = amount <= 0;
						if (flag4)
						{
							this._templateCaches.Add(templateId);
						}
					}
					foreach (short templateId2 in this._templateCaches)
					{
						status2.Remove(templateId2);
					}
				}
			}
		}

		// Token: 0x06005F63 RID: 24419 RVA: 0x0036426C File Offset: 0x0036246C
		public void RegenerateInState(sbyte stateId, IReadOnlyList<short> templateIds)
		{
			bool flag = templateIds.Count == 0;
			if (!flag)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(37, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Regenerate ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(templateIds.Count);
				defaultInterpolatedStringHandler.AppendLiteral(" broken material in state ");
				defaultInterpolatedStringHandler.AppendFormatted<sbyte>(stateId);
				AdaptableLog.Warning(defaultInterpolatedStringHandler.ToStringAndClear(), false);
				Dictionary<short, int> status = this._complexConfig.GetOrNew(stateId);
				foreach (short templateId in templateIds)
				{
					MiscItem config = Misc.Instance[templateId];
					bool flag2 = config == null || config.GenerateType == EMiscGenerateType.Invalid;
					if (!flag2)
					{
						status[templateId] = status.GetOrDefault(templateId) + 1;
					}
				}
			}
		}

		// Token: 0x06005F64 RID: 24420 RVA: 0x00364358 File Offset: 0x00362558
		private int CalcPreferPickAmount(sbyte stateId, int brokenLevel)
		{
			Dictionary<short, int> status;
			bool flag = !this._complexConfig.TryGetValue(stateId, out status);
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				int sum = 0;
				int minLevel = brokenLevel;
				foreach (KeyValuePair<short, int> keyValuePair in status)
				{
					short num;
					int num2;
					keyValuePair.Deconstruct(out num, out num2);
					short templateId = num;
					int amount = num2;
					bool flag2 = !TreasureMaterialHelper.IsValid(templateId, brokenLevel);
					if (!flag2)
					{
						sum += amount;
						MiscItem config = Misc.Instance[templateId];
						foreach (int allowBrokenLevel in config.AllowBrokenLevels)
						{
							minLevel = Math.Min(minLevel, allowBrokenLevel);
						}
					}
				}
				result = sum / Math.Max(brokenLevel - minLevel + 1, 1);
			}
			return result;
		}

		// Token: 0x06005F65 RID: 24421 RVA: 0x00364464 File Offset: 0x00362664
		private short PickInState(IRandomSource random, sbyte stateId, int brokenLevel)
		{
			Dictionary<short, int> status;
			bool flag = !this._complexConfig.TryGetValue(stateId, out status);
			short result;
			if (flag)
			{
				result = -1;
			}
			else
			{
				this._templateCaches.Clear();
				foreach (KeyValuePair<short, int> keyValuePair in status)
				{
					short num;
					int num2;
					keyValuePair.Deconstruct(out num, out num2);
					short templateId = num;
					int amount = num2;
					bool flag2 = TreasureMaterialHelper.IsValid(templateId, brokenLevel) && amount > 0;
					if (flag2)
					{
						this._templateCaches.Add(templateId);
					}
				}
				bool flag3 = this._templateCaches.Count == 0;
				if (flag3)
				{
					result = -1;
				}
				else
				{
					short picked = this._templateCaches.GetRandom(random);
					Dictionary<short, int> dictionary = status;
					short num = picked;
					dictionary[num]--;
					bool flag4 = status[picked] <= 0;
					if (flag4)
					{
						status.Remove(picked);
					}
					result = picked;
				}
			}
			return result;
		}

		// Token: 0x06005F66 RID: 24422 RVA: 0x00364574 File Offset: 0x00362774
		private void PickInStateExclusive(IList<short> picked, sbyte stateId, int brokenLevel)
		{
			Dictionary<short, int> status;
			bool flag = !this._complexConfig.TryGetValue(stateId, out status);
			if (!flag)
			{
				List<short> removedKeys = ObjectPool<List<short>>.Instance.Get();
				foreach (KeyValuePair<short, int> keyValuePair in status)
				{
					short num;
					int num2;
					keyValuePair.Deconstruct(out num, out num2);
					short templateId = num;
					int amount = num2;
					bool flag2 = !TreasureMaterialHelper.IsValidExclusive(templateId, brokenLevel);
					if (!flag2)
					{
						for (int i = 0; i < amount; i++)
						{
							picked.Add(templateId);
						}
						removedKeys.Add(templateId);
					}
				}
				foreach (short templateId2 in removedKeys)
				{
					status.Remove(templateId2);
				}
				ObjectPool<List<short>>.Instance.Return(removedKeys);
			}
		}

		// Token: 0x06005F67 RID: 24423 RVA: 0x00364688 File Offset: 0x00362888
		public void PickAreaTemplates(IRandomSource random, IList<short> picked, short areaId)
		{
			bool flag = picked == null;
			if (flag)
			{
				throw new ArgumentNullException("picked");
			}
			MapDomain mapDomain = DomainManager.Map;
			int brokenLevel = mapDomain.QueryAreaBrokenLevel(areaId);
			sbyte stateId = mapDomain.GetStateTemplateIdByAreaId(areaId);
			int preferAmount = this.CalcPreferPickAmount(stateId, brokenLevel);
			this.PickInStateExclusive(picked, stateId, brokenLevel);
			for (int i = picked.Count; i < preferAmount; i++)
			{
				short templateId = this.PickInState(random, stateId, brokenLevel);
				bool flag2 = templateId >= 0;
				if (flag2)
				{
					picked.Add(templateId);
				}
			}
		}

		// Token: 0x04001935 RID: 6453
		private readonly Dictionary<sbyte, Dictionary<short, int>> _complexConfig = new Dictionary<sbyte, Dictionary<short, int>>();

		// Token: 0x04001936 RID: 6454
		private readonly List<short> _templateCaches = new List<short>();
	}
}
