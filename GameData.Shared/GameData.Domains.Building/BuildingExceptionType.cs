using System;

namespace GameData.Domains.Building;

public enum BuildingExceptionType : sbyte
{
	Damaged,
	[Obsolete]
	ExpandStoppedForWorkerShortage,
	[Obsolete]
	ExpandStoppedForResourcesShortage,
	[Obsolete]
	ExpandStoppedForDependency,
	[Obsolete]
	ExpandStoppedForAutoList,
	BuildStoppedForWorkerShortage,
	DemolishStoppedForWorkerShortage,
	[Obsolete]
	ManageStoppedForWorkerShortage,
	ManageStoppedForDependency,
	EffectStoppedForDependency,
	ManageStoppedForNoLeader,
	ComfortableHouseEntertainNoFood
}
