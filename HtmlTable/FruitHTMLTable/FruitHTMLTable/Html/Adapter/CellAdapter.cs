using Tricentis.Automation.Creation;
using Tricentis.Automation.Engines.Adapters;
using Tricentis.Automation.Engines.Adapters.Attributes;
using Tricentis.Automation.Engines.Adapters.Html.Generic;
using Tricentis.Automation.Engines.Technicals.Html;

namespace FruitHTMLTable.Html.Adapter
{
    [SupportedTechnical(typeof(HtmlElementTechnical))]
    public class CellAdapter : AbstractHtmlDomNodeAdapter<IHtmlDivTechnical>, ITableCellAdapter
    {
        public CellAdapter(IHtmlDivTechnical technical, Validator validator) : base(technical, validator)
        {
            validator.AssertTrue(() => technical.ClassName.Contains("z-column"));
        }

        public int ColSpan => 1;

        public int RowSpan => 1;

        public string Text => Technical.InnerText;

    }
}
