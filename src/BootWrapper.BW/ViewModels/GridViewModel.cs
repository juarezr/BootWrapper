using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BootWrapper.BW.Formatter;

namespace BootWrapper.BW.ViewModels
{
    #region DataTableParameters

    /// <summary>
    /// Class that encapsulates most common parameters sent by DataTables plugin
    /// See: http://datatables.net/usage/server-side for reference
    /// </summary>
    public class DataTableParameters
    {
        /// <summary>
        /// Request sequence number sent by DataTable,
        /// same value must be returned in response
        /// </summary>       
        public string sEcho { get; set; }

        /// <summary>
        /// First record that should be shown(used for paging)
        /// </summary>
        public int iDisplayStart { get; set; }

        /// <summary>
        /// Number of records that should be shown in table
        /// </summary>
        public int iDisplayLength { get; set; }

        /// <summary>
        /// Number of columns in table
        /// </summary>
        public int iColumns { get; set; }

        public string mDataProp_0 { get; set; }
        public string mDataProp_1 { get; set; }
        public string mDataProp_2 { get; set; }
        public bool bSortable_0 { get; set; }
        public bool bSortable_1 { get; set; }
        public bool bSortable_2 { get; set; }

        /// <summary>
        /// Comma separated list of column names
        /// </summary>
        public string sColumns { get; set; }

        /// <summary>
        /// Number of columns that are used in sorting
        /// </summary>
        public int iSortingCols { get; set; }

        public int iSortCol_0 { get; set; }
        public int iSortCol_1 { get; set; }
        public int iSortCol_2 { get; set; }
        public string sSortDir_0 { get; set; }
        public string sSortDir_1 { get; set; }
        public string sSortDir_2 { get; set; }

        /// <summary>
        /// Text used for filtering
        /// </summary>
        public string sSearch { get; set; }

        public bool bSearchable_0 { get; set; }
        public bool bSearchable_1 { get; set; }
        public bool bSearchable_2 { get; set; }

        public string sSearch_0 { get; set; }
        public string sSearch_1 { get; set; }
        public string sSearch_2 { get; set; }

        public bool bRegex { get; set; }

        public bool bRegex_0 { get; set; }
        public bool bRegex_1 { get; set; }
        public bool bRegex_2 { get; set; }

        public string GetOrderByFields()
        {
            var result = string.Empty;
            var columns = sColumns.Split(',');
            if (iSortingCols >= 1)
                result += string.Format("{0} {1}", columns[iSortCol_0], sSortDir_0);
            if (iSortingCols >= 2)
                result += string.Format(", {0} {1}", columns[iSortCol_1], sSortDir_1);
            if (iSortingCols >= 3)
                result += string.Format(", {0} {1}", columns[iSortCol_2], sSortDir_2);
            return result;
        }
    }

    #endregion

    public class GridViewModel<TResultType> : BaseViewModel
    {
        public List<TResultType> List { get; set; }

        public GridViewModel()
        {
            List = new List<TResultType>();
        }

        public object OutputToJson(DataTableParameters dataTableGrid, Func<TResultType, object> fieldSelector)
        {
            var iTotalRecords = this.CalculateTotalRecords(dataTableGrid);
            var json = new
            {
                dataTableGrid.sEcho,
                iTotalRecords,
                iTotalDisplayRecords = iTotalRecords,
                Errors = Errors.ToList(),
                aaData = this.List.Select(fieldSelector).ToList()
            };
            return json;
        }

        public object OutputErrorToJson(DataTableParameters dataTableGrid, string message, Exception ex)
        {
            this.AddError(message, ex);
            var json = new
            {
                dataTableGrid.sEcho,
                iTotalRecords = 0,
                iTotalDisplayRecords = 0,
                Errors = Errors.ToList(),
                aaData = "[]"
            };
            return json;
        }
        public int CalculateTotalRecords(DataTableParameters grid)
        {
            // Truque para enviar sempre uma página a mais na paginação
            return CalculateTotalRecords(grid, this.List);
        }

        public int CalculateTotalRecords(DataTableParameters grid, ICollection list)
        {
            // Truque para enviar sempre uma página a mais na paginação
            var iTotalRecords = grid.iDisplayStart + grid.iDisplayLength + 1;
            if (list.Count < grid.iDisplayLength)
            {
                iTotalRecords = grid.iDisplayStart + list.Count;
            }
            return iTotalRecords;
        }

        public string FormatDateTime(DateTime? date)
        {
            if (date == null && !date.HasValue) return String.Empty;
            return FormatUtil.FormatDateTime(date.Value);
        }

        public string FormatDate(DateTime? date)
        {
            if (date == null && !date.HasValue) return String.Empty;
            return FormatUtil.FormatDate(date.Value);
        }

        public string FormatTime(DateTime? date)
        {
            if (date == null && !date.HasValue) return String.Empty;
            return FormatUtil.FormatTime(date.Value.TimeOfDay);
        }
    }
}
