using System;
using CompDevLib.Interpreter;
using GameData.Domains.Character;
using GameData.Domains.Item;
using GameData.Domains.Map;
using GameData.Domains.Organization;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.TaiwuEvent.ValueSelector
{
	// Token: 0x02000087 RID: 135
	public class ValueConverters
	{
		// Token: 0x060018E0 RID: 6368 RVA: 0x00167CA0 File Offset: 0x00165EA0
		[ValueConverter(typeof(int), typeof(Character))]
		public static ValueInfo CharIdToCharacter(Evaluator evaluator, ValueInfo valueInfo)
		{
			int id = evaluator.EvaluationStack.PopUnmanaged<int>();
			Character character;
			DomainManager.Character.TryGetElement_Objects(id, out character);
			return evaluator.PushEvaluationResult(character);
		}

		// Token: 0x060018E1 RID: 6369 RVA: 0x00167CD4 File Offset: 0x00165ED4
		[ValueConverter(typeof(int), typeof(Settlement))]
		public static ValueInfo SettlementIdToSettlement(Evaluator evaluator, ValueInfo valueInfo)
		{
			int id = evaluator.EvaluationStack.PopUnmanaged<int>();
			Settlement settlement = DomainManager.Organization.GetSettlement((short)id);
			return evaluator.PushEvaluationResult(settlement);
		}

		// Token: 0x060018E2 RID: 6370 RVA: 0x00167D08 File Offset: 0x00165F08
		[ValueConverter(typeof(int), typeof(MapAreaData))]
		public static ValueInfo AreaTemplateIdToMapAreaData(Evaluator evaluator, ValueInfo valueInfo)
		{
			int id = evaluator.EvaluationStack.PopUnmanaged<int>();
			short areaId = DomainManager.Map.GetAreaIdByAreaTemplateId((short)id);
			MapAreaData areaData = DomainManager.Map.GetAreaByAreaId(areaId);
			return evaluator.PushEvaluationResult(areaData);
		}

		// Token: 0x060018E3 RID: 6371 RVA: 0x00167D48 File Offset: 0x00165F48
		[ValueConverter(typeof(int), typeof(UnmanagedVariant<TemplateKey>))]
		public static ValueInfo UIntToTemplateKey(Evaluator evaluator, ValueInfo valueInfo)
		{
			int id = evaluator.EvaluationStack.PopUnmanaged<int>();
			TemplateKey templateKey = (TemplateKey)((uint)id);
			return evaluator.PushEvaluationResult(new UnmanagedVariant<TemplateKey>(templateKey));
		}

		// Token: 0x060018E4 RID: 6372 RVA: 0x00167D7C File Offset: 0x00165F7C
		[ValueConverter(typeof(Location), typeof(MapBlockData))]
		public static ValueInfo LocationToMapBlock(Evaluator evaluator, ValueInfo valueInfo)
		{
			Location location = (Location)evaluator.EvaluationStack.PopObject<ISerializableGameData>();
			MapBlockData mapBlock = DomainManager.Map.GetBlock(location);
			return evaluator.PushEvaluationResult(mapBlock);
		}
	}
}
