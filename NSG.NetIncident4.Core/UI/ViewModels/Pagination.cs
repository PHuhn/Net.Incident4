// ===========================================================================
// File: Pagination.cs
using System;
using System.Collections.Generic;
//
using NSG.PrimeNG.LazyLoading;
//
namespace NSG.NetIncident4.Core.UI.ViewModels
{
    //
    /// <summary>
    /// Class in support of a pageination control.
    /// <<  < 1 2 3 4 5 6 7 >  >> select v
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public partial class Pagination<T> : IPagination
    {
        /// <summary>
        /// The list of T containing only this page of rows
        /// </summary>
        public List<T> items { get; set; }
        /// <summary>
        /// PrimeNG styled LazyLoadEvent instance
        /// </summary>
        public LazyLoadEvent2 lazyLoadEvent { get; set; }
        /// <summary>
        /// Total number of records in the selection/table
        /// </summary>
        public long totalRecords { get; set; }
        /// <summary>
        /// Array of rows per page for dropdown
        /// </summary>
        public int[] rowsPerPageOptions { get; set; }
        /// <summary>
        /// Current page index
        /// </summary>
        public long pageIndex { get; set; }
        /// <summary>
        /// Current page count
        /// </summary>
        public long pageCount { get; set; }
        /// <summary>
        /// How many pages to display, default is 7
        /// </summary>
        public long pageWindowSize { get; set; }
        /// <summary>
        /// The action, defaults to 'Index' action.
        /// </summary>
        public string action { get; set; }
        //
        /// <summary>
        /// Constructor
        /// <example>
        ///   LazyLoadEvent2 event2 = new LazyLoadEvent2() {first=0, rows = 4}; 
        ///   Pagination<TestData> pagination = new Pagination<TestData>(
        ///     _data.Select(d => d).AsQueryable().LazySkipTake2<TestData>(event2).ToList(),
        ///     event2,
        ///     _data.Count
        ///   )
        ///   {
        ///     rowsPerPageOptions = new int[3] { 4, 8, 16 }
        ///   };
        /// In view:
        ///   @model NSG.NetIncident4.Core.UI.ViewModels.Pagination<TestData>
        ///   ...
        ///   <partial name="PagerPartial" model="Model" />
        /// or:
        ///   <paginator paginator-model="@Model"></paginator>
        /// </example>
        /// </summary>
        /// <param name="items"></param>
        /// <param name="event2"></param>
        /// <param name="total"></param>
        public Pagination(List<T> items, LazyLoadEvent2 event2, long total)
        {
            // parameter values
            this.items = items;
            this.lazyLoadEvent = event2;
            this.totalRecords = total;
            // calculated values
            this.pageIndex = (long)Math.Ceiling((decimal)((decimal)this.lazyLoadEvent.first / (decimal)this.lazyLoadEvent.rows)) + 1;
            this.pageCount = (long)Math.Ceiling((decimal)((decimal)this.totalRecords / (decimal)this.lazyLoadEvent.rows));
            // default values
            this.rowsPerPageOptions = new int[3] { 5, 10, 20 };
            this.pageWindowSize = (long)7;
            this.action = "Index";
        }
        //
        public LazyLoadEvent2 GetRouteForPage(long pageIndex)
        {
            LazyLoadEvent2 _ret = new LazyLoadEvent2()
            {
                rows = this.lazyLoadEvent.rows,
                sortField = this.lazyLoadEvent.sortField,
                sortOrder = this.lazyLoadEvent.sortOrder,
                multiSortMeta = this.lazyLoadEvent.multiSortMeta,
                filters = this.lazyLoadEvent.filters,
                globalFilter = this.lazyLoadEvent.globalFilter
            };
            if ( pageIndex < 2)
            {
                _ret.first = 0;
            }
            else
            {
                if( pageIndex > this.pageCount)
                {
                    _ret.first = (this.pageCount - 1) * this.lazyLoadEvent.rows;
                }
                else
                {
                    _ret.first = (pageIndex - 1) * this.lazyLoadEvent.rows;
                }
            }
            return _ret;
        }
        //
        private long[] GetStartEndWindow()
        {
            long startPage = 1;
            long numberOfPages = this.pageCount + 1;
            long visiblePages = Math.Min(this.pageWindowSize, numberOfPages);
            // calculate range, keep current in middle if necessary
            long start = Math.Max(startPage, (long)Math.Ceiling((decimal)this.pageIndex - (((decimal)visiblePages) / (decimal)2))),
                end = Math.Min(numberOfPages - 1, start + visiblePages - 1);
            // check when approaching to last page
            var delta = this.pageWindowSize - (end - start + 1);
            start = Math.Max(startPage, start - delta);
            return new long[2] { start, end };
        }
        //
        /// <summary>
        /// Returns the start page index for the sliding window of pages.
        /// </summary>
        /// <returns></returns>
        public long GetStartWindow()
        {
            long[] startEnd = GetStartEndWindow();
            return startEnd[0];
        }
        //
        /// <summary>
        /// Returns the end page index for the sliding window of pages.
        /// </summary>
        /// <returns></returns>
        public long GetEndWindow()
        {
            long[] startEnd = GetStartEndWindow();
            return startEnd[1];
        }
        //
        /// <summary>
        /// Is the current pageIndex the first page.
        /// </summary>
        /// <returns></returns>
        public bool IsFirstPage()
        {
            return this.pageIndex == 1;
        }
        //
        /// <summary>
        /// Is the current pageIndex the last page.
        /// </summary>
        /// <returns></returns>
        public bool IsLastPage()
        {
            return this.pageIndex == this.pageCount;
        }
    }
}
// ===========================================================================
