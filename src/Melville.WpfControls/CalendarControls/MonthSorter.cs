using System;
using System.Collections.Generic;
using System.Linq;
using Melville.Hacks;
using Melville.Linq;

namespace Melville.WpfControls.CalendarControls
{
 public readonly struct ItemPositionPair<T>
    {
        public T Item { get; }
        public CalendarBar Position { get; }

        public ItemPositionPair(T item, CalendarBar position)
        {
            Item = item;
            Position = position;
        }
    }
    public class MonthSorter<T>
    {
        private readonly DateTime baseDate;
        private readonly DateTime lastDate;
        private readonly double dayHeaderHeight;
        private readonly Func<T, CalendarBar> dateFunc;
        private readonly Func<T, int, int, double, double> effector;
       
        public double TotalHeight { get; private set; }
        private readonly List<double> weekHeights  = new();

        public MonthSorter(DateTime baseDate, double dayHeaderHeight,
            Func<T, CalendarBar> dateFunc,
            Func<T, int, int, double, double> effector)
        {
            this.baseDate = baseDate;
            lastDate = ComputeLastDay();
            this.dayHeaderHeight = dayHeaderHeight;
            this.dateFunc = dateFunc;
            this.effector = effector;
            TotalHeight = 0;
        }
        private DateTime ComputeLastDay() => baseDate.EndOfWeek().LastDayOnCalendar();

        public IList<double> SortDates(IEnumerable<T> items) // T item, int column, int colSpan, double Y Position
        {
            foreach (var week in GroupItemsByDay(items).Chunks(7))
            {
                TotalHeight += dayHeaderHeight;
                RenderRow(week.Select(i=>i.GetEnumerator()).ToArray());
            }

            return weekHeights;
        }

        private IEnumerable<IEnumerable<ItemPositionPair<T>>> GroupItemsByDay(IEnumerable<T> items) =>
            AllDays().GroupJoin(AllDatedItems(items), i => i, i => i.Position.BaseDate, (_, i) => i);
        
        private IEnumerable<ItemPositionPair<T>> AllDatedItems(IEnumerable<T> items) => 
            items.Select(i=>new ItemPositionPair<T>(i, dateFunc(i)))
                .OrderBy(i=>i.Position.Item.Begin);

        private IEnumerable<DateTime> AllDays() => 
            FunctionalMethods.Sequence(baseDate, i => i.AddDays(1)).TakeWhile(i=>i <= lastDate);

        private void RenderRow(IEnumerator<ItemPositionPair<T>>?[] days)
        {
            double weekHeight = dayHeaderHeight;
            double rowHeight = 0.0;
            do
            {
                rowHeight = 0.0;
                for (int i = 0; i < days.Length; i++)
                {
                    if (days[i] is not { } day) continue;
                    if (!day.MoveNext())
                    {
                        days[i] = null;
                    }
                    else
                    {
                        var currentItem = day.Current;
                        var itemWidth = currentItem.Position.DisplayWidth();
                        var myHeight = effector(currentItem.Item, i, itemWidth, TotalHeight);
                        rowHeight = Math.Max(rowHeight, myHeight);
                        i += itemWidth - 1;
                    }
                }
                TotalHeight += rowHeight;
                weekHeight += rowHeight;
            } while (rowHeight > 0.0);
            weekHeights.Add(weekHeight);
        }
    }}