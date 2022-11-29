using System;
using Tricentis.Automation.Creation;
using Tricentis.Automation.Engines.Adapters;
using Tricentis.Automation.Engines.Adapters.Attributes;
using Tricentis.Automation.Engines.Adapters.Html.Generic;
using Tricentis.Automation.Engines.Technicals.Html;
using System.Linq;

namespace FruitHTMLTable.Html.Adapter
{
    /// <summary>
    /// Header row is a <div></div>, all other rows are table <tr></tr>
    /// </summary>
    [SupportedTechnical(typeof(IHtmlDivTechnical))]
    public class RowAdapter : AbstractHtmlDomNodeAdapter<IHtmlDivTechnical>, ITableRowAdapter
    {
        public RowAdapter(IHtmlDivTechnical technical, Validator validator) : base(technical, validator)
        {
            validator.AssertTrue(() => technical.ClassName.Contains("z-header"));
        }

    }
}
