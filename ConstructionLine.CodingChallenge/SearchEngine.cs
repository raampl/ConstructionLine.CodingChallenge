using System;
using System.Collections.Generic;
using System.Linq;

namespace ConstructionLine.CodingChallenge
{
    public class SearchEngine
    {
        private readonly List<Shirt> _shirts;

        public SearchEngine(List<Shirt> shirts)
        {
            _shirts = shirts;
        }


        public SearchResults Search(SearchOptions options)
        {
            var sizes = options.Sizes.Select(s => s.Id).ToArray();
            var colors = options.Colors.Select(s => s.Id).ToArray();

            var matchedBySize = sizes.Any() ? _shirts.Where(s => sizes.Contains(s.Size.Id)).ToList() : _shirts;
            var matchedByColor = colors.Any() ? _shirts.Where(s => colors.Contains(s.Color.Id)).ToList() : _shirts;

            var matchedBySizeColor = matchedBySize.Intersect(matchedByColor);

            var colorsResult = matchedBySizeColor
               .GroupBy(x => x.Color)
               .Select(c => new ColorCount()
               {
                   Color = c.Key,
                   Count = c.Count()
               }).ToList();

            foreach (var color in Color.All.Except(colorsResult.Select(x => x.Color)))
            {
                colorsResult.Add(new ColorCount() { Color = color, Count = 0 });
            }

            var sizesResult = matchedBySizeColor
               .GroupBy(x => x.Size)
               .Select(c => new SizeCount()
               {
                   Size = c.Key,
                   Count = c.Count()
               }).ToList();
            foreach (var size in Size.All.Except(sizesResult.Select(x => x.Size)))
            {
                sizesResult.Add(new SizeCount() { Size = size, Count = 0 });
            }


            return new SearchResults
            {
                Shirts = matchedBySizeColor.ToList(),
                ColorCounts = colorsResult,
                SizeCounts = sizesResult
            };
        }
    }
}