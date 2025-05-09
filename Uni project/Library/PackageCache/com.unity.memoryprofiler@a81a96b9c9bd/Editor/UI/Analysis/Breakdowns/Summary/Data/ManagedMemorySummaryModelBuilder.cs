using System;
using System.Collections.Generic;
using Unity.MemoryProfiler.Editor.UIContentData;
using UnityEditor;
using UnityEngine;
using static Unity.MemoryProfiler.Editor.CachedSnapshot;

namespace Unity.MemoryProfiler.Editor.UI
{
    /// <summary>
    /// Managed VM state for memory usage breakdown table view model builder
    /// </summary>
    internal class ManagedMemorySummaryModelBuilder : IMemorySummaryModelBuilder<MemorySummaryModel>
    {
        CachedSnapshot m_SnapshotA;
        CachedSnapshot m_SnapshotB;

        public ManagedMemorySummaryModelBuilder(CachedSnapshot snapshotA, CachedSnapshot snapshotB)
        {
            m_SnapshotA = snapshotA;
            m_SnapshotB = snapshotB;
        }

        public MemorySummaryModel Build()
        {
            Summary a, b;
            BuildSummary(m_SnapshotA, out a);
            if (m_SnapshotB != null)
                BuildSummary(m_SnapshotB, out b);
            else
                b = new Summary();

            bool compareMode = m_SnapshotB != null;
            return new MemorySummaryModel(
                SummaryTextContent.kManagedMemoryTitle,
                SummaryTextContent.kManagedMemoryDescription,
                compareMode,
                a.Total,
                b.Total,
                new List<MemorySummaryModel.Row>() {
                    new MemorySummaryModel.Row(SummaryTextContent.kManagedMemoryCategoryVM, a.VM, 0, b.VM, 0, "virtual-machine",
                    TextContent.ManagedDomainDescription +
                    (m_SnapshotB != null ? string.Format(TextContent.ManagedDomainDescriptionStaticFieldAddendumDiff, EditorUtility.FormatBytes(a.VMStaticFields), EditorUtility.FormatBytes(b.VMStaticFields))
                    : string.Format(TextContent.ManagedDomainDescriptionStaticFieldAddendumOneSnapshot, EditorUtility.FormatBytes(a.VMStaticFields)))
                    , null),
                    new MemorySummaryModel.Row(SummaryTextContent.kManagedMemoryCategoryObjects, a.Objects, 0, b.Objects, 0, "objects", TextContent.ManagedObjectsDescription, null),
                    new MemorySummaryModel.Row(SummaryTextContent.kManagedMemoryCategoryFreeHeap, a.EmptyHeapSpace, 0, b.EmptyHeapSpace, 0, "free-in-active-heap-section", TextContent.EmptyActiveHeapDescription, null),
                },
                null);
        }

        void BuildSummary(CachedSnapshot cs, out Summary res)
        {
            CalculateTotal(cs, out res);

            // Add VM root.
            var vmRootSize = cs.NativeRootReferences.AccumulatedSizeOfVMRoot;
            res.VM += vmRootSize;
            res.Total += vmRootSize;
        }

        static void CalculateTotal(CachedSnapshot cs, out Summary res)
        {
            var data = cs.EntriesMemoryMap.Data;
            var summary = new Summary();
            cs.EntriesMemoryMap.ForEachFlat((_, address, size, source) =>
            {
                switch (source.Id)
                {
                    case SourceIndex.SourceId.ManagedHeapSection:
                    {
                        summary.Total += size;
                        var sectionType = cs.ManagedHeapSections.SectionType[source.Index];
                        switch (sectionType)
                        {
                            case MemorySectionType.VirtualMachine:
                                summary.VM += size;
                                break;
                            case MemorySectionType.GarbageCollector:
                                summary.EmptyHeapSpace += size;
                                break;
                            default:
                                Debug.Assert(false, $"Unknown managed memory section type ({sectionType}), plese report a bug.");
                                break;
                        }
                        return;
                    }
                    case SourceIndex.SourceId.ManagedObject:
                    {
                        summary.Total += size;
                        summary.Objects += size;
                        return;
                    }
                    default:
                        return;
                }
            });
            summary.VMStaticFields = 0;
            for (long i = 0; i < cs.TypeDescriptions.Count; i++)
            {
                summary.VMStaticFields += cs.TypeDescriptions.StaticFieldBytes.Count(i);
            }

            res = summary;
        }

        struct Summary
        {
            public ulong Total;

            public ulong VM;
            public long VMStaticFields;
            public ulong Objects;
            public ulong EmptyHeapSpace;
        }
    }
}
