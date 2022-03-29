// ===========================================================================
// File: IPagination.cs
using System;
//
using NSG.PrimeNG.LazyLoading;
//
namespace NSG.NetIncident4.Core.UI.ViewModels
{
    //
    /// <summary>
    /// Class in support of a pageination control.
    /// <<  < 1 2 3 4 5 6 7 >  >>
    /// </summary>
    public interface IPagination
    {
        /// <summary>
        /// PrimeNG styled LazyLoadEvent instance
        /// </summary>
        LazyLoadEvent2 lazyLoadEvent { get; set; }
        /// <summary>
        /// Total number of records in the selection/table
        /// </summary>
        long totalRecords { get; set; }
        /// <summary>
        /// Array of rows per page for dropdown
        /// </summary>
        int[] rowsPerPageOptions { get; set; }
        /// <summary>
        /// Current page index
        /// </summary>
        long pageIndex { get; set; }
        /// <summary>
        /// Current page count
        /// </summary>
        long pageCount { get; set; }
        /// <summary>
        /// How many pages to display, default is 7
        /// </summary>
        long pageWindowSize { get; set; }
        /// <summary>
        /// The action, defaults to 'Index' action.
        /// </summary>
        string action { get; set; }
        /// <summary>
        /// Toggle for displaying the rowsPerPageOptions dropdown.
        /// </summary>
        bool pageOptions { get; set; }
        /// <summary>
        /// Toggle for displaying the previous and next buttons.
        /// </summary>
        bool previousNext { get; set; }
        LazyLoadEvent2 GetRouteForPage(long pageIndex);
        //
        /// <summary>
        /// Returns the start page index for the sliding window of pages.
        /// </summary>
        /// <returns></returns>
        long GetStartWindow();
        //
        /// <summary>
        /// Returns the end page index for the sliding window of pages.
        /// </summary>
        /// <returns></returns>
        long GetEndWindow();
        //
        /// <summary>
        /// Is the current pageIndex the first page.
        /// </summary>
        /// <returns></returns>
        bool IsFirstPage();
        //
        /// <summary>
        /// Is the current pageIndex the last page.
        /// </summary>
        /// <returns></returns>
        bool IsLastPage();
        //
    }
}
// ===========================================================================
