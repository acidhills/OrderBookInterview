using System;
using System.Collections.Generic;
using System.Linq;

namespace OrderBookInterview
{
    public static class DiffHelper
    {

        public static double[] Restore(double[] target, DiffS[] diff)
        {
            var deleteCandidates = diff.Where(x => double.IsNaN(x.Price)).ToList();
            foreach (var delete in deleteCandidates)
            {
                var startPosition = delete.Position;
                for (int i = startPosition + 2; i < target.Length; i += 2)
                {
                    if (double.IsNaN(target[i]))
                        break;

                    target[i - 2] = target[i];
                    target[i - 1] = target[i + 1];
                }
            }
            foreach (var apply in diff.Except(deleteCandidates).ToList())
            {
                var pos = apply.Position;
                target[pos] = apply.Price;
                target[pos + 1] = apply.Value;
            }
            return target;
        }
        public static DiffS[] FindDiff(double[] source, double[] target)
        {
            var diffs = new List<DiffS>(source.Length);
            var askhashes = new Dictionary<double, int>();
            var newAskHashes = new HashSet<double>();

            var bidhashes = new Dictionary<double, int>();
            var newBidHashes = new HashSet<double>();

            var isAsk = true;
            for (int i = 0; i < source.Length; i += 2)
            {
                if (double.IsNaN(source[i]))
                {
                    isAsk = false;
                    continue;
                }
                if (isAsk)
                    askhashes.Add(source[i], i);
                else
                    bidhashes.Add(source[i], i);
            }

            isAsk = true;
            for (int i = 0; i < source.Length; i += 2)
            {
                if (double.IsNaN(source[i]))
                {
                    isAsk = false;
                    continue;
                }
                DiffS? diff = null;
                if (isAsk)
                    diff = FindDiffsInPart(i, source, target, askhashes, newAskHashes);
                else
                    diff = FindDiffsInPart(i, source, target, bidhashes, newBidHashes);
                if (diff.HasValue)
                    diffs.Add(diff.Value);
            }

            if (newAskHashes.Count != askhashes.Count)
            {
                var todelete = askhashes.Keys.Except(newAskHashes);
                foreach (var key in todelete)
                {
                    diffs.Add(new DiffS(Double.NaN, Double.NaN, askhashes[key]));
                }
            }
            if (newBidHashes.Count != bidhashes.Count)
            {
                var todelete = bidhashes.Keys.Except(newBidHashes);
                foreach (var key in todelete)
                {
                    diffs.Add(new DiffS(Double.NaN, Double.NaN, bidhashes[key]));
                }
            }

            return diffs.ToArray();
        }


        private static DiffS? FindDiffsInPart(int currentIndex,
            double[] source,
            double[] target,
            Dictionary<double, int> oldHashes,
            HashSet<double> newHashes)
        {
            DiffS? diff = null;
            var pricediff = source[currentIndex] - target[currentIndex];
            var valuediff = source[currentIndex + 1] - target[currentIndex + 1];
            if (pricediff > double.Epsilon)
            {

                if (oldHashes.ContainsKey(target[currentIndex]))
                {
                    var oldPos = oldHashes[target[currentIndex]];
                    var newValDiff = source[oldPos + 1] - target[currentIndex + 1];
                    if (newValDiff > Double.Epsilon)
                        diff = new DiffS(target[currentIndex], target[currentIndex + 1], oldPos);
                    newHashes.Add(target[currentIndex]);
                }
                else
                {
                    diff = new DiffS(target[currentIndex], target[currentIndex + 1], currentIndex);
                }
            }
            else if (valuediff > double.Epsilon)
            {
                diff = new DiffS(target[currentIndex], target[currentIndex + 1], currentIndex);
            }
            else
            {
                newHashes.Add(target[currentIndex]);
            }
            return diff;
        }

    }
}