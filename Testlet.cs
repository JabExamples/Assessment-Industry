using System;
using System.Collections.Generic;
using System.Linq;

namespace StraszAssessment
{
    public struct Item
    {
        public string ItemId;
        public ItemTypeEnum ItemType;
    }

    public enum ItemTypeEnum
    {
        Pretest = 0,
        Operational = 1
    }
    public class Testlet
    {
        /*
        // First and second pass 
        public string TestletId { get; }
        private List<Item> Items;

        public Testlet(string testletId, List<Item> items)
        {
            TestletId = testletId;
            Items = items;
        }
        */


        public string TestletId { get; }
        private List<Item> Items;
        private ITestletRandomizer testletRandomizer;

        public Testlet(string testletId, List<Item> items)
        {
            TestletId = testletId;
            Items = items;
            testletRandomizer = new TestletRandomizer(); //indication the design should be reconsidered to use dependency injection. 
        }

        public Testlet(string testletId, List<Item> items, ITestletRandomizer testletRandomizer)
        {
            TestletId = testletId;
            Items = items;
            this.testletRandomizer = testletRandomizer;
        }

        /*
        // First pass
        private static readonly Random random = new Random();
        public List<Item> Randomize()
        {
            var pretestItems = Items.Where(o => o.ItemType == ItemTypeEnum.Pretest)
                .OrderBy(o => random.Next()).ToList();

            var operationalItems = Items.Where(o => o.ItemType == ItemTypeEnum.Operational);
            var remainingQuestions = operationalItems.Concat(pretestItems.Skip(2)).OrderBy(o => random.Next());
            return pretestItems.Take(2)
                .Concat(remainingQuestions)
                .ToList();
        }
        */

        /*
        // Second pass
        private static readonly Random random = new Random();
        public List<Item> Randomize()
        {
            var pretestItems = RandomizeItemList(Items.Where(o => o.ItemType == ItemTypeEnum.Pretest));
            var operationalItems = Items.Where(o => o.ItemType == ItemTypeEnum.Operational);           
            var remainingQuestions = RandomizeItemList(operationalItems.Concat(pretestItems.Skip(2)));            
            return pretestItems.Take(2).Concat(remainingQuestions).ToList();
        }
        

        private List<Item> RandomizeItemList(IEnumerable<Item> items)
        {            
            return items.OrderBy(o => random.Next()).ToList();
        }      
        */

        public List<Item> Randomize()
        {
            var pretestItems = Items.Where(o => o.ItemType == ItemTypeEnum.Pretest);
            var operationalItems = Items.Where(o => o.ItemType == ItemTypeEnum.Operational);
            var pretestQuestions = testletRandomizer.RandomizeItemList(pretestItems);
            var remainingQuestions = testletRandomizer.RandomizeItemList(operationalItems.Concat(pretestQuestions.Skip(2)));
            return pretestQuestions.Take(2).Concat(remainingQuestions).ToList();
        }

        public interface ITestletRandomizer
        {
            List<Item> RandomizeItemList(IEnumerable<Item> items);
        }

        public class TestletRandomizer : ITestletRandomizer
        {
            private static readonly Random random = new Random();
            public List<Item> RandomizeItemList(IEnumerable<Item> items)
            {
                return items.OrderBy(o => random.Next()).ToList();
            }
        }
    }
}
