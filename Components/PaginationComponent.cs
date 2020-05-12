using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorServerApp.Components
{
    public abstract class PaginationResultBase
    {
        public int CurrentPage { get; set; }
        public int PageCount { get; set; }
        public int PageSize { get; set; }
        public int RowCount { get; set; }

        public int FirstRowOnPage {

            get { return (CurrentPage - 1) * PageSize + 1; }
        }

        public int LastRowOnPage {
            get { return Math.Min(CurrentPage * PageSize, RowCount); }
        }
    }

    public class PaginationResult<T> : PaginationResultBase where T : class
    {
        public IList<T> Results { get; set; }

        public PaginationResult()
        {
            Results = new List<T>();
        }
    }

    public class PaginationComponent : ComponentBase
    {
        [Parameter]
        public PaginationResultBase Result { get; set; }

        [Parameter]
        public Action<int> PageChanged { get; set; }

        protected int StartIndex { get; private set; } = 0;
        protected int FinishIndex { get; private set; } = 0;

        protected override void OnParametersSet()
        {
            StartIndex = Math.Max(Result.CurrentPage - 5, 1);
            FinishIndex = Math.Min(Result.CurrentPage + 5, Result.PageCount);

            base.OnParametersSet();
        }

        protected void PagerButtonClicked(int page)
        {
            PageChanged?.Invoke(page);
        }
    }
}
